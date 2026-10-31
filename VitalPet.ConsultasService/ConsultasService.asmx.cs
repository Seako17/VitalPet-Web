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
        public void MarcarNoSePresento(string dni, string claveUsuario, int idConsulta)
        {
            using (SqlConnection con = new SqlConnection(cadenaConexion))
            {
                con.Open();
                using (SqlTransaction transaccion = con.BeginTransaction())
                {
                    ValidarUsuario(con, transaccion, dni, claveUsuario);
                    DataTable consulta = LeerTabla(con, transaccion, "Consulta", @"SELECT estado, fechaHora FROM Consulta
                        WHERE idConsulta = @Consulta AND DNI_Veterinario = @Dni",
                        new SqlParameter("@Consulta", idConsulta), new SqlParameter("@Dni", dni));
                    if (consulta.Rows.Count == 0) throw Error("ERR_ATENCION_CONSULTA_INVALIDA");
                    if (Cifrado.Descifrar(consulta.Rows[0]["estado"].ToString()) != "Agendada")
                        throw Error("ERR_ATENCION_ESTADO");
                    if (LeerFecha(consulta.Rows[0]["fechaHora"]) > DateTime.Now) throw Error("ERR_ATENCION_AUSENCIA_ANTICIPADA");
                    using (SqlCommand cmd = new SqlCommand("UPDATE Consulta SET estado = @Estado WHERE idConsulta = @Consulta", con, transaccion))
                    {
                        cmd.Parameters.AddWithValue("@Estado", Cifrado.Cifrar("NoSePresento"));
                        cmd.Parameters.AddWithValue("@Consulta", idConsulta);
                        cmd.ExecuteNonQuery();
                    }
                    RegistrarEvento(con, transaccion, dni, "Registrar ausencia");
                    transaccion.Commit();
                }
            }
        }

        [WebMethod]
        public DataTable ObtenerAtencion(string dni, string claveUsuario, int idConsulta)
        {
            using (SqlConnection con = new SqlConnection(cadenaConexion))
            {
                con.Open();
                ValidarUsuario(con, null, dni, claveUsuario);
                DataTable atenciones = LeerTabla(con, null, "Atencion", @"SELECT a.id_atencion, a.id_consulta,
                    a.diagnostico, a.observaciones FROM Atencion a
                    INNER JOIN Consulta c ON c.idConsulta = a.id_consulta
                    WHERE c.idConsulta = @Consulta AND c.DNI_Veterinario = @Dni",
                    new SqlParameter("@Consulta", idConsulta), new SqlParameter("@Dni", dni));
                foreach (DataRow fila in atenciones.Rows)
                {
                    fila["diagnostico"] = Cifrado.Descifrar(fila["diagnostico"].ToString());
                    fila["observaciones"] = Cifrado.Descifrar(fila["observaciones"].ToString());
                }
                return atenciones;
            }
        }

        [WebMethod]
        public int GuardarAtencion(string dni, string claveUsuario, int idConsulta, int idAtencion, string diagnostico, string observaciones)
        {
            using (SqlConnection con = new SqlConnection(cadenaConexion))
            {
                con.Open();
                using (SqlTransaction transaccion = con.BeginTransaction())
                {
                    ValidarUsuario(con, transaccion, dni, claveUsuario);
                    if (string.IsNullOrWhiteSpace(diagnostico)) throw Error("ERR_ATENCION_DIAGNOSTICO_OBLIGATORIO");
                    if (string.IsNullOrWhiteSpace(observaciones)) throw Error("ERR_ATENCION_OBSERVACIONES_OBLIGATORIAS");
                    DataTable consulta = LeerTabla(con, transaccion, "Consulta", @"SELECT estado FROM Consulta
                        WHERE idConsulta = @Consulta AND DNI_Veterinario = @Dni",
                        new SqlParameter("@Consulta", idConsulta), new SqlParameter("@Dni", dni));
                    if (consulta.Rows.Count == 0) throw Error("ERR_ATENCION_CONSULTA_INVALIDA");
                    string estado = Cifrado.Descifrar(consulta.Rows[0]["estado"].ToString());
                    if (estado == "Cancelada") throw Error("ERR_ATENCION_CANCELADA");
                    if (estado != "Agendada" && estado != "Atendida" && estado != "NoSePresento") throw Error("ERR_ATENCION_ESTADO");
                    if (idAtencion < 0) throw Error("ERR_ATENCION_ID_INVALIDO");
                    DataTable existentes = LeerTabla(con, transaccion, "Atencion", "SELECT id_atencion FROM Atencion WHERE id_consulta = @Consulta",
                        new SqlParameter("@Consulta", idConsulta));
                    if (idAtencion == 0 && existentes.Rows.Count > 0) throw Error("ERR_ATENCION_EXISTENTE");
                    if (idAtencion > 0 && (existentes.Rows.Count != 1 || (int)existentes.Rows[0]["id_atencion"] != idAtencion))
                        throw Error("ERR_ATENCION_ID_INVALIDO");
                    string sql = idAtencion == 0
                        ? @"INSERT Atencion (id_consulta, diagnostico, observaciones)
                            VALUES (@Consulta, @Diagnostico, @Observaciones); SELECT CAST(SCOPE_IDENTITY() AS int)"
                        : @"UPDATE Atencion SET diagnostico = @Diagnostico, observaciones = @Observaciones
                            WHERE id_atencion = @Atencion AND id_consulta = @Consulta; SELECT @Atencion";
                    using (SqlCommand cmd = new SqlCommand(sql, con, transaccion))
                    {
                        cmd.Parameters.AddWithValue("@Consulta", idConsulta);
                        cmd.Parameters.AddWithValue("@Atencion", idAtencion);
                        cmd.Parameters.AddWithValue("@Diagnostico", Cifrado.Cifrar(diagnostico.Trim()));
                        cmd.Parameters.AddWithValue("@Observaciones", Cifrado.Cifrar(observaciones.Trim()));
                        idAtencion = (int)cmd.ExecuteScalar();
                    }
                    using (SqlCommand cmd = new SqlCommand("UPDATE Consulta SET estado = @Estado WHERE idConsulta = @Consulta", con, transaccion))
                    {
                        cmd.Parameters.AddWithValue("@Estado", Cifrado.Cifrar("Atendida"));
                        cmd.Parameters.AddWithValue("@Consulta", idConsulta);
                        cmd.ExecuteNonQuery();
                    }
                    RegistrarEvento(con, transaccion, dni, "Guardar Atencion");
                    transaccion.Commit();
                    return idAtencion;
                }
            }
        }

        [WebMethod]
        public DataTable ObtenerConsultasVeterinario(string dni, string claveUsuario)
        {
            using (SqlConnection con = new SqlConnection(cadenaConexion))
            {
                con.Open();
                ValidarUsuario(con, null, dni, claveUsuario);
                DataTable datos = LeerTabla(con, null, "Consultas", @"SELECT idConsulta, idMascota, DNI_Veterinario,
                    motivo, fechaHora, estado
                    FROM Consulta WHERE DNI_Veterinario = @Dni", new SqlParameter("@Dni", dni));
                DataTable resultado = datos.Clone();
                resultado.Columns["fechaHora"].DataType = typeof(DateTime);
                foreach (DataRow fila in datos.Rows)
                {
                    string estado = Cifrado.Descifrar(fila["estado"].ToString());
                    if (estado == "Cancelada") continue;
                    resultado.Rows.Add(fila["idConsulta"], fila["idMascota"], fila["DNI_Veterinario"],
                        Cifrado.Descifrar(fila["motivo"].ToString()), LeerFecha(fila["fechaHora"]), estado);
                }
                return resultado;
            }
        }

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
                DataTable usuarios = ObtenerVeterinarios(con, null);
                foreach (DataRow usuario in usuarios.Rows)
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
            if (!string.IsNullOrEmpty(estado) && estado != "Agendada" && estado != "Atendida" && estado != "Cancelada" && estado != "NoSePresento")
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


        private void ValidarUsuario(SqlConnection con, SqlTransaction transaccion, string dni, string claveUsuario)
        {
            if (string.IsNullOrWhiteSpace(dni) || string.IsNullOrEmpty(claveUsuario)) throw Error("ERR_CONSULTA_ACCESO");
            if (Context != null && !Context.Request.IsLocal && !Context.Request.IsSecureConnection) throw Error("ERR_CONSULTA_ACCESO");
            using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Usuario WHERE dni = @Dni AND contraseña = @Clave AND bloqueado = 0", con, transaccion))
            {
                cmd.Parameters.AddWithValue("@Dni", dni);
                cmd.Parameters.AddWithValue("@Clave", claveUsuario);
                if ((int)cmd.ExecuteScalar() == 0)
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

        private static DataTable ObtenerVeterinarios(SqlConnection con, SqlTransaction transaccion)
        {
            return LeerTabla(con, transaccion, "Veterinarios", @"SELECT u.dni, u.nombre, u.apellido
                FROM Usuario u INNER JOIN Rol r ON r.id_rol = u.rol
                WHERE u.bloqueado = 0 AND r.nombre = @Rol", new SqlParameter("@Rol", "Veterinario"));
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
            DataTable usuarios = ObtenerVeterinarios(con, transaccion);
            DataTable disponibles = new DataTable("Veterinarios");
            disponibles.Columns.Add("Dni", typeof(string));
            disponibles.Columns.Add("NombreCompleto", typeof(string));
            disponibles.Columns.Add("Disponible", typeof(bool));
            foreach (DataRow usuario in usuarios.Rows)
            {
                bool libre = !ocupados.Contains(usuario["dni"].ToString());
                if (!libre && !incluirOcupados) continue;
                disponibles.Rows.Add(usuario["dni"], Cifrado.Descifrar(usuario["nombre"].ToString()) + " " + Cifrado.Descifrar(usuario["apellido"].ToString()), libre);
            }
            if (disponibles.Rows.Count == 0) throw Error("ERR_CONSULTA_SIN_VETERINARIOS");
            return disponibles;
        }

        private static DataTable BuscarConsultas(SqlConnection con, SqlTransaction transaccion, string dni)
        {
            DataTable datos = LeerTabla(con, transaccion, "Consultas", @"SELECT c.idConsulta, c.idMascota, c.DNI_Veterinario,
                c.fechaHora, c.motivo, c.estado, COALESCE(a.diagnostico, c.diagnostico) AS diagnostico,
                m.nombre AS nombreMascota, u.nombre, u.apellido
                FROM Consulta c INNER JOIN Mascota m ON m.id_mascota = c.idMascota
                OUTER APPLY (SELECT TOP 1 diagnostico FROM Atencion
                    WHERE id_consulta = c.idConsulta ORDER BY id_atencion DESC) a
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
