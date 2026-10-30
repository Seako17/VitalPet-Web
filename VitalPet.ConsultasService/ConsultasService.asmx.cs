using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.Services;
using System.Web.Services.Protocols;

namespace VitalPet.ConsultasService
{
    [WebService(Namespace = "http://vitalpet/consultas/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ConsultasService : WebService
    {
        private readonly string cadenaConexion = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
        private const string FormatoFecha = "yyyy-MM-dd HH:mm";

        [WebMethod]
        public DataTable ObtenerMascotas(string dni, string claveUsuario)
        {
            using (SqlConnection con = new SqlConnection(cadenaConexion))
            {
                con.Open();
                ValidarUsuario(con, null, dni, claveUsuario);
                return LeerTabla(con, null, "Mascotas", "SELECT id_mascota AS IdMascota, nombre AS Nombre FROM Mascota WHERE dni = @Dni ORDER BY nombre",
                    new SqlParameter("@Dni", dni));
            }
        }

        [WebMethod]
        public string[] ObtenerHorarios(string dni, string claveUsuario)
        {
            using (SqlConnection con = new SqlConnection(cadenaConexion))
            {
                con.Open();
                ValidarUsuario(con, null, dni, claveUsuario);
            }
            List<string> horarios = new List<string>();
            for (DateTime hora = DateTime.Today.AddHours(8); hora <= DateTime.Today.AddHours(20); hora = hora.AddMinutes(30))
                horarios.Add(hora.ToString("HH:mm"));
            return horarios.ToArray();
        }

        [WebMethod]
        public DataTable ObtenerDisponibilidadDia(string dni, string claveUsuario, int idMascota, DateTime fecha)
        {
            using (SqlConnection con = new SqlConnection(cadenaConexion))
            {
                con.Open();
                ValidarUsuario(con, null, dni, claveUsuario);
                ValidarMascota(con, null, dni, idMascota);
                HashSet<string> veterinarios = new HashSet<string>();
                DataTable usuarios = LeerTabla(con, null, "Usuarios", "SELECT dni, rol FROM Usuario WHERE bloqueado = 0");
                foreach (DataRow usuario in usuarios.Rows)
                    if (!usuario.IsNull("rol") && TienePermiso(con, null, (int)usuario["rol"], "ConsultaAtender"))
                        veterinarios.Add(usuario["dni"].ToString());

                Dictionary<DateTime, HashSet<string>> ocupados = new Dictionary<DateTime, HashSet<string>>();
                HashSet<DateTime> mascotaOcupada = new HashSet<DateTime>();
                DataTable consultas = LeerTabla(con, null, "Consultas", "SELECT idMascota, DNI_Veterinario, fechaHora, estado FROM Consulta");
                foreach (DataRow consulta in consultas.Rows)
                {
                    if (Cifrado.Descifrar(consulta["estado"].ToString()) == "Cancelada") continue;
                    DateTime hora = LeerFecha(consulta["fechaHora"]);
                    if (hora.Date != fecha.Date) continue;
                    if (!ocupados.ContainsKey(hora)) ocupados[hora] = new HashSet<string>();
                    ocupados[hora].Add(consulta["DNI_Veterinario"].ToString());
                    if ((int)consulta["idMascota"] == idMascota) mascotaOcupada.Add(hora);
                }
                DataTable resultado = new DataTable("DisponibilidadDia");
                resultado.Columns.Add("Hora", typeof(string));
                resultado.Columns.Add("Cupos", typeof(int));
                resultado.Columns.Add("Estado", typeof(string));
                DateTime ahora = DateTime.Now;
                for (DateTime hora = fecha.Date.AddHours(8); hora <= fecha.Date.AddHours(20); hora = hora.AddMinutes(30))
                {
                    int cupos = 0;
                    foreach (string veterinario in veterinarios)
                        if (!ocupados.ContainsKey(hora) || !ocupados[hora].Contains(veterinario)) cupos++;
                    string estado = "Disponible";
                    if (hora <= ahora) estado = "Pasado";
                    else if (mascotaOcupada.Contains(hora)) estado = "MascotaOcupada";
                    else if (cupos == 0) estado = "Ocupado";
                    resultado.Rows.Add(hora.ToString("HH:mm"), estado == "Disponible" ? cupos : 0, estado);
                }
                return resultado;
            }
        }

        [WebMethod]
        public DataTable ObtenerVeterinariosDisponibles(string dni, string claveUsuario, int idMascota, DateTime fechaHora)
        {
            using (SqlConnection con = new SqlConnection(cadenaConexion))
            {
                con.Open();
                ValidarUsuario(con, null, dni, claveUsuario);
                return BuscarDisponibles(con, null, dni, idMascota, fechaHora);
            }
        }

        [WebMethod]
        public DataTable ObtenerVeterinariosDelHorario(string dni, string claveUsuario, int idMascota, DateTime fechaHora)
        {
            using (SqlConnection con = new SqlConnection(cadenaConexion))
            {
                con.Open();
                ValidarUsuario(con, null, dni, claveUsuario);
                return BuscarDisponibles(con, null, dni, idMascota, fechaHora, true);
            }
        }

        [WebMethod]
        public int Agendar(string dni, string claveUsuario, int idMascota, string dniVeterinario, string motivo, DateTime fechaHora)
        {
            using (SqlConnection con = new SqlConnection(cadenaConexion))
            {
                con.Open();
                using (SqlTransaction transaccion = con.BeginTransaction(IsolationLevel.Serializable))
                {
                    BloquearConsultas(con, transaccion);
                    ValidarUsuario(con, transaccion, dni, claveUsuario);
                    if (string.IsNullOrWhiteSpace(motivo)) throw Error("ERR_CONSULTA_MOTIVO");
                    DataTable disponibles = BuscarDisponibles(con, transaccion, dni, idMascota, fechaHora);
                    bool veterinarioLibre = false;
                    foreach (DataRow fila in disponibles.Rows)
                        if (fila["Dni"].ToString() == dniVeterinario) veterinarioLibre = true;
                    if (!veterinarioLibre) throw Error("ERR_CONSULTA_VETERINARIO_OCUPADO");

                    string consulta = @"INSERT INTO Consulta (idMascota, DNI_Veterinario, motivo, fechaHora, estado)
                        VALUES (@Mascota, @Veterinario, @Motivo, @Fecha, @Estado); SELECT CAST(SCOPE_IDENTITY() AS int)";
                    int idConsulta;
                    using (SqlCommand cmd = new SqlCommand(consulta, con, transaccion))
                    {
                        cmd.Parameters.AddWithValue("@Mascota", idMascota);
                        cmd.Parameters.AddWithValue("@Veterinario", dniVeterinario);
                        cmd.Parameters.AddWithValue("@Motivo", Cifrado.Cifrar(motivo.Trim()));
                        cmd.Parameters.AddWithValue("@Fecha", Cifrado.Cifrar(fechaHora.ToString(FormatoFecha, CultureInfo.InvariantCulture)));
                        cmd.Parameters.AddWithValue("@Estado", Cifrado.Cifrar("Agendada"));
                        idConsulta = (int)cmd.ExecuteScalar();
                    }
                    RegistrarEvento(con, transaccion, dni, "Agendar Consulta");
                    transaccion.Commit();
                    return idConsulta;
                }
            }
        }

        [WebMethod]
        public DataTable ObtenerMisConsultas(string dni, string claveUsuario, string estado)
        {
            if (!string.IsNullOrEmpty(estado) && estado != "Agendada" && estado != "Atendida" && estado != "Cancelada")
                throw Error("ERR_CONSULTA_ESTADO");
            using (SqlConnection con = new SqlConnection(cadenaConexion))
            {
                con.Open();
                ValidarUsuario(con, null, dni, claveUsuario);
                DataTable consultas = BuscarConsultas(con, null, dni);
                DataTable resultado = consultas.Clone();
                foreach (DataRow fila in consultas.Select("", "fechaHora DESC"))
                    if (string.IsNullOrEmpty(estado) || fila["estado"].ToString() == estado) resultado.ImportRow(fila);
                return resultado;
            }
        }

        [WebMethod]
        public DataTable ObtenerProximasConsultas(string dni, string claveUsuario, int idMascota)
        {
            using (SqlConnection con = new SqlConnection(cadenaConexion))
            {
                con.Open();
                ValidarUsuario(con, null, dni, claveUsuario);
                ValidarMascota(con, null, dni, idMascota);
                DataTable consultas = BuscarConsultas(con, null, dni);
                DataTable resultado = consultas.Clone();
                foreach (DataRow fila in consultas.Select("", "fechaHora ASC"))
                    if ((int)fila["idMascota"] == idMascota && fila["estado"].ToString() == "Agendada" && (DateTime)fila["fechaHora"] > DateTime.Now)
                        resultado.ImportRow(fila);
                return resultado;
            }
        }

        [WebMethod]
        public DataTable ObtenerDetallePropio(string dni, string claveUsuario, int idConsulta)
        {
            using (SqlConnection con = new SqlConnection(cadenaConexion))
            {
                con.Open();
                ValidarUsuario(con, null, dni, claveUsuario);
                DataTable consultas = BuscarConsultas(con, null, dni);
                DataTable resultado = consultas.Clone();
                foreach (DataRow fila in consultas.Rows)
                    if ((int)fila["idConsulta"] == idConsulta) resultado.ImportRow(fila);
                if (resultado.Rows.Count == 0) throw Error("ERR_CONSULTA_NO_DISPONIBLE");
                return resultado;
            }
        }

        [WebMethod]
        public bool CancelarPropia(string dni, string claveUsuario, int idConsulta)
        {
            using (SqlConnection con = new SqlConnection(cadenaConexion))
            {
                con.Open();
                using (SqlTransaction transaccion = con.BeginTransaction(IsolationLevel.Serializable))
                {
                    BloquearConsultas(con, transaccion);
                    ValidarUsuario(con, transaccion, dni, claveUsuario);
                    DataTable consultas = BuscarConsultas(con, transaccion, dni);
                    DataRow propia = null;
                    foreach (DataRow fila in consultas.Rows)
                        if ((int)fila["idConsulta"] == idConsulta) propia = fila;
                    if (propia == null || !(bool)propia["puedeCancelar"]) throw Error("ERR_CONSULTA_NO_CANCELABLE");
                    using (SqlCommand cmd = new SqlCommand(@"UPDATE Consulta SET estado = @Estado, fechaCancelacion = @Fecha,
                        canceladaPor = @Dni WHERE idConsulta = @Id", con, transaccion))
                    {
                        cmd.Parameters.AddWithValue("@Estado", Cifrado.Cifrar("Cancelada"));
                        cmd.Parameters.AddWithValue("@Fecha", Cifrado.Cifrar(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.InvariantCulture)));
                        cmd.Parameters.AddWithValue("@Dni", dni);
                        cmd.Parameters.AddWithValue("@Id", idConsulta);
                        cmd.ExecuteNonQuery();
                    }
                    RegistrarEvento(con, transaccion, dni, "Cancelar Consulta");
                    transaccion.Commit();
                    return true;
                }
            }
        }

        // Funciones privadas: no se publican como operaciones del web service.
        private void ValidarUsuario(SqlConnection con, SqlTransaction transaccion, string dni, string claveUsuario)
        {
            // La web ya tiene el hash del usuario en su sesión. No se guarda la contraseña en texto plano.
            if (string.IsNullOrWhiteSpace(dni) || string.IsNullOrEmpty(claveUsuario)) throw Error("ERR_CONSULTA_ACCESO");
            if (Context != null && !Context.Request.IsLocal && !Context.Request.IsSecureConnection) throw Error("ERR_CONSULTA_ACCESO");
            using (SqlCommand cmd = new SqlCommand("SELECT rol FROM Usuario WHERE dni = @Dni AND contraseña = @Clave AND bloqueado = 0", con, transaccion))
            {
                cmd.Parameters.AddWithValue("@Dni", dni);
                cmd.Parameters.AddWithValue("@Clave", claveUsuario);
                object rol = cmd.ExecuteScalar();
                if (rol == null || rol == DBNull.Value || !TienePermiso(con, transaccion, Convert.ToInt32(rol), "MascotaRegistrar"))
                    throw Error("ERR_CONSULTA_ACCESO");
            }
        }

        private static DataTable LeerTabla(SqlConnection con, SqlTransaction transaccion, string nombre, string consulta, params SqlParameter[] parametros)
        {
            using (SqlCommand cmd = new SqlCommand(consulta, con, transaccion))
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddRange(parametros);
                DataTable tabla = new DataTable(nombre);
                adapter.Fill(tabla);
                return tabla;
            }
        }

        private static bool TienePermiso(SqlConnection con, SqlTransaction transaccion, int rol, string permiso)
        {
            DataTable directos = LeerTabla(con, transaccion, "Permisos", @"SELECT p.id_permiso FROM RolPermiso rp
                INNER JOIN Permiso p ON p.id_permiso = rp.id_permiso WHERE rp.id_rol = @Rol AND p.nombre = @Permiso",
                new SqlParameter("@Rol", rol), new SqlParameter("@Permiso", permiso));
            if (directos.Rows.Count > 0) return true;
            DataTable familias = LeerTabla(con, transaccion, "Familias", "SELECT id_familia FROM RolFamilia WHERE id_rol = @Rol", new SqlParameter("@Rol", rol));
            HashSet<int> visitadas = new HashSet<int>();
            foreach (DataRow familia in familias.Rows)
                if (FamiliaTienePermiso(con, transaccion, (int)familia["id_familia"], permiso, visitadas)) return true;
            return false;
        }

        private static bool FamiliaTienePermiso(SqlConnection con, SqlTransaction transaccion, int familia, string permiso, HashSet<int> visitadas)
        {
            if (!visitadas.Add(familia)) return false;
            DataTable permisos = LeerTabla(con, transaccion, "Permisos", @"SELECT p.id_permiso FROM FamiliaPermiso fp
                INNER JOIN Permiso p ON p.id_permiso = fp.id_permiso WHERE fp.id_familia = @Familia AND p.nombre = @Permiso",
                new SqlParameter("@Familia", familia), new SqlParameter("@Permiso", permiso));
            if (permisos.Rows.Count > 0) return true;
            DataTable hijas = LeerTabla(con, transaccion, "Familias", "SELECT id_familia_hija FROM FamiliaFamilia WHERE id_familia_padre = @Familia", new SqlParameter("@Familia", familia));
            foreach (DataRow hija in hijas.Rows)
                if (FamiliaTienePermiso(con, transaccion, (int)hija["id_familia_hija"], permiso, visitadas)) return true;
            return false;
        }

        private static void ValidarMascota(SqlConnection con, SqlTransaction transaccion, string dni, int idMascota)
        {
            DataTable mascotas = LeerTabla(con, transaccion, "Mascotas", "SELECT id_mascota FROM Mascota WHERE id_mascota = @Id AND dni = @Dni",
                new SqlParameter("@Id", idMascota), new SqlParameter("@Dni", dni));
            if (mascotas.Rows.Count == 0) throw Error("ERR_CONSULTA_MASCOTA_INVALIDA");
        }

        private static DataTable BuscarDisponibles(SqlConnection con, SqlTransaction transaccion, string dni, int idMascota, DateTime fechaHora, bool incluirOcupados = false)
        {
            ValidarMascota(con, transaccion, dni, idMascota);
            if (fechaHora <= DateTime.Now) throw Error("ERR_CONSULTA_FECHA_PASADA");
            if (fechaHora.TimeOfDay < TimeSpan.FromHours(8) || fechaHora.TimeOfDay > TimeSpan.FromHours(20) || fechaHora.Ticks % TimeSpan.FromMinutes(30).Ticks != 0)
                throw Error("ERR_CONSULTA_FUERA_HORARIO");
            DataTable consultas = LeerTabla(con, transaccion, "Consultas", "SELECT idMascota, DNI_Veterinario, fechaHora, estado FROM Consulta");
            HashSet<string> ocupados = new HashSet<string>();
            foreach (DataRow fila in consultas.Rows)
            {
                if (Cifrado.Descifrar(fila["estado"].ToString()) == "Cancelada" || LeerFecha(fila["fechaHora"]) != fechaHora) continue;
                if ((int)fila["idMascota"] == idMascota) throw Error("ERR_CONSULTA_MASCOTA_OCUPADA");
                ocupados.Add(fila["DNI_Veterinario"].ToString());
            }
            DataTable usuarios = LeerTabla(con, transaccion, "Usuarios", "SELECT dni, nombre, apellido, rol FROM Usuario WHERE bloqueado = 0");
            DataTable disponibles = new DataTable("Veterinarios");
            disponibles.Columns.Add("Dni", typeof(string));
            disponibles.Columns.Add("NombreCompleto", typeof(string));
            disponibles.Columns.Add("Disponible", typeof(bool));
            foreach (DataRow usuario in usuarios.Rows)
            {
                bool libre = !ocupados.Contains(usuario["dni"].ToString());
                if (usuario.IsNull("rol") || (!libre && !incluirOcupados)) continue;
                if (TienePermiso(con, transaccion, (int)usuario["rol"], "ConsultaAtender"))
                    disponibles.Rows.Add(usuario["dni"], Cifrado.Descifrar(usuario["nombre"].ToString()) + " " + Cifrado.Descifrar(usuario["apellido"].ToString()), libre);
            }
            if (disponibles.Rows.Count == 0) throw Error("ERR_CONSULTA_SIN_VETERINARIOS");
            return disponibles;
        }

        private static DataTable BuscarConsultas(SqlConnection con, SqlTransaction transaccion, string dni)
        {
            DataTable datos = LeerTabla(con, transaccion, "Consultas", @"SELECT c.idConsulta, c.idMascota, c.DNI_Veterinario,
                c.fechaHora, c.motivo, c.estado, c.diagnostico, m.nombre AS nombreMascota, u.nombre, u.apellido
                FROM Consulta c INNER JOIN Mascota m ON m.id_mascota = c.idMascota
                INNER JOIN Usuario u ON u.dni = c.DNI_Veterinario WHERE m.dni = @Dni", new SqlParameter("@Dni", dni));
            DataTable resultado = datos.Clone();
            resultado.Columns["fechaHora"].DataType = typeof(DateTime);
            resultado.Columns.Remove("nombre");
            resultado.Columns.Remove("apellido");
            resultado.Columns.Add("nombreVeterinario", typeof(string));
            resultado.Columns.Add("puedeCancelar", typeof(bool));
            resultado.Columns.Add("pendienteDeRegistro", typeof(bool));
            DateTime ahora = DateTime.Now;
            foreach (DataRow fila in datos.Rows)
            {
                DataRow nueva = resultado.NewRow();
                nueva["idConsulta"] = fila["idConsulta"];
                nueva["idMascota"] = fila["idMascota"];
                nueva["DNI_Veterinario"] = fila["DNI_Veterinario"];
                nueva["fechaHora"] = LeerFecha(fila["fechaHora"]);
                nueva["motivo"] = Cifrado.Descifrar(fila["motivo"].ToString());
                nueva["estado"] = Cifrado.Descifrar(fila["estado"].ToString());
                nueva["diagnostico"] = Cifrado.Descifrar(fila["diagnostico"].ToString());
                nueva["nombreMascota"] = fila["nombreMascota"];
                nueva["nombreVeterinario"] = Cifrado.Descifrar(fila["nombre"].ToString()) + " " + Cifrado.Descifrar(fila["apellido"].ToString());
                nueva["puedeCancelar"] = nueva["estado"].ToString() == "Agendada" && (DateTime)nueva["fechaHora"] > ahora;
                nueva["pendienteDeRegistro"] = nueva["estado"].ToString() == "Agendada" && (DateTime)nueva["fechaHora"] < ahora;
                resultado.Rows.Add(nueva);
            }
            return resultado;
        }

        private static DateTime LeerFecha(object valor)
        {
            return DateTime.ParseExact(Cifrado.Descifrar(valor.ToString()), FormatoFecha, CultureInfo.InvariantCulture);
        }

        private static void BloquearConsultas(SqlConnection con, SqlTransaction transaccion)
        {
            // Evita que dos pedidos reserven el mismo cupo antes de confirmar la transacción.
            using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Consulta WITH (TABLOCKX, HOLDLOCK)", con, transaccion)) cmd.ExecuteScalar();
        }

        private static void RegistrarEvento(SqlConnection con, SqlTransaction transaccion, string dni, string evento)
        {
            using (SqlCommand cmd = new SqlCommand(@"INSERT INTO Evento (dni, fechaHora, modulo, evento, criticidad)
                VALUES (@Dni, @Fecha, 'Consultas', @Evento, 4)", con, transaccion))
            {
                cmd.Parameters.AddWithValue("@Dni", dni);
                cmd.Parameters.AddWithValue("@Fecha", DateTime.Now);
                cmd.Parameters.AddWithValue("@Evento", evento);
                cmd.ExecuteNonQuery();
            }
        }

        private static SoapException Error(string codigo) { return new SoapException(codigo, SoapException.ClientFaultCode); }
    }
}
