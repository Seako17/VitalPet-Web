using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.SessionState;
using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.Servicios;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Services.Protocols;
using FGF_Finanzas.Capas.BLL;
using Proxy = FGF_Finanzas.ServiceReferences.Consultas;
using VitalPet.ConsultasService;

internal static class Program
{
    private static readonly string Conexion = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
    private static int pruebas;
    private const string Cliente = "11111111";
    private const string OtroCliente = "22222222";
    private static string Clave(string dni) { return Cifrado.Hash(dni); }

    private static void Comprobar(bool valor, string nombre)
    {
        if (!valor) throw new Exception("FALLÓ: " + nombre);
        pruebas++;
        Console.WriteLine("OK: " + nombre);
    }

    private static void Sql(string sql)
    {
        using (var con = new SqlConnection(Conexion))
        using (var cmd = new SqlCommand(sql, con)) { con.Open(); cmd.ExecuteNonQuery(); }
    }

    private static object Escalar(string sql)
    {
        using (var con = new SqlConnection(Conexion))
        using (var cmd = new SqlCommand(sql, con)) { con.Open(); return cmd.ExecuteScalar(); }
    }

    private static Proxy.ConsultasService Servicio()
    { return new Proxy.ConsultasService { Url = ConfigurationManager.AppSettings["ConsultasService.Url"], Timeout = 15000 }; }

    private static void Error(Action accion, string codigo, string nombre)
    {
        try { accion(); }
        catch (SoapException ex) { Comprobar(ex.Message.Contains(codigo), nombre); return; }
        throw new Exception("No se rechazó: " + nombre);
    }

    private static void Sembrar()
    {
        Sql(@"CREATE TABLE Usuario (dni nvarchar(8) PRIMARY KEY, nombre nvarchar(max), apellido nvarchar(max),
            usuario nvarchar(max), mail nvarchar(max), bloqueado bit, rol int, DV_Horizontal nvarchar(max));
            CREATE TABLE Mascota (id_mascota int PRIMARY KEY, dni nvarchar(8), nombre nvarchar(100), DV_Horizontal nvarchar(max));
            CREATE TABLE Consulta (idConsulta int IDENTITY PRIMARY KEY, idMascota int NOT NULL REFERENCES Mascota(id_mascota),
            DNI_Veterinario nvarchar(8) NOT NULL REFERENCES Usuario(dni), motivo nvarchar(max) NOT NULL, fechaHora nvarchar(max) NOT NULL,
            DV_Horizontal nvarchar(max), estado nvarchar(128), diagnostico nvarchar(max), fechaCancelacion nvarchar(max), canceladaPor nvarchar(8));
            CREATE TABLE Evento (id_Evento int IDENTITY PRIMARY KEY, dni nvarchar(8), fechaHora datetime, modulo nvarchar(20), evento nvarchar(20), criticidad int);
            CREATE TABLE DigitoVerificador (NombreTabla nvarchar(50) PRIMARY KEY, DigitoVertical nvarchar(max), CantidadRegistros int);
            CREATE TABLE Permiso (id_permiso int PRIMARY KEY, nombre nvarchar(100));
            CREATE TABLE RolPermiso (id_rol int, id_permiso int);
            CREATE TABLE RolFamilia (id_rol int, id_familia int);
            CREATE TABLE FamiliaFamilia (id_familia_padre int, id_familia_hija int);
            CREATE TABLE FamiliaPermiso (id_familia int, id_permiso int);
            INSERT INTO Permiso VALUES (1, 'MascotaRegistrar'), (2, 'ConsultaAtender');
            INSERT INTO RolPermiso VALUES (1,1), (3,2);
            INSERT INTO RolFamilia VALUES (2,1);
            INSERT INTO FamiliaFamilia VALUES (1,2), (2,1);
            INSERT INTO FamiliaPermiso VALUES (2,2);
            INSERT INTO Usuario VALUES ('11111111','Cliente','Uno','cliente1','correo1',0,1,NULL),
                ('22222222','Cliente','Dos','cliente2','correo2',0,1,NULL),
                ('33333333','Vet','Familia','vet1','correo3',0,2,NULL),
                ('44444444','Vet','Directo','vet2','correo4',0,3,NULL),
                ('55555555','Cliente','Bloqueado','bloqueado','correo5',1,1,NULL),
                ('66666666','Sin','Permiso','sinpermiso','correo6',0,4,NULL);
            INSERT INTO Mascota VALUES (1,'11111111','Luna',NULL),(2,'22222222','Sol',NULL),(3,'11111111','Nube',NULL);
            ALTER TABLE Usuario ADD contraseña nvarchar(64);");
        Sql(@"CREATE TABLE Idioma (IdIdioma int, Codigo nvarchar(20), Nombre nvarchar(50));
            CREATE TABLE Etiqueta (IdEtiqueta int, ControlId nvarchar(100), Formulario nvarchar(100));
            CREATE TABLE Traduccion (IdEtiqueta int, IdIdioma int, Texto nvarchar(max));");
        using (var con = new SqlConnection(Conexion))
        {
            con.Open();
            foreach (string columna in new[] { "nombre", "apellido", "usuario", "mail", "contraseña" })
            {
                var datos = new DataTable();
                using (var adapter = new SqlDataAdapter("SELECT dni, " + columna + " FROM Usuario", con)) adapter.Fill(datos);
                foreach (DataRow fila in datos.Rows)
                using (var cmd = new SqlCommand("UPDATE Usuario SET " + columna + " = @Valor WHERE dni = @Dni", con))
                {
                    cmd.Parameters.AddWithValue("@Valor", columna == "contraseña" ? Clave(fila["dni"].ToString()) : Cifrado.Cifrar(fila[columna].ToString()));
                    cmd.Parameters.AddWithValue("@Dni", fila["dni"]); cmd.ExecuteNonQuery();
                }
            }
        }
        var integridadWeb = new BLLDigitoVerificador();
        foreach (string tabla in new[] { "Usuario", "Mascota", "Consulta" }) integridadWeb.InicializarTablaCompleta(tabla);
    }

    public static int Main()
    {
        try
        {
            if (!new SqlConnectionStringBuilder(Conexion).InitialCatalog.StartsWith("VitalPetConsultas_Test_", StringComparison.Ordinal))
                throw new Exception("Solo se permiten bases temporales de pruebas.");
            Sembrar();
            using (var s = Servicio())
            {
                string clave = Clave(Cliente);
                string otraClave = Clave(OtroCliente);
                DateTime fecha = DateTime.Today.AddDays(2).AddHours(10);
                var diaLibre = s.ObtenerDisponibilidadDia(Cliente, clave, 1, fecha);
                Comprobar(diaLibre.Rows.Count == 25 && Convert.ToInt32(diaLibre.Compute("SUM(Cupos)", "")) == 50, "disponibilidad diaria suma cupos por veterinario");
                Error(() => s.ObtenerDisponibilidadDia(Cliente, clave, 2, fecha), "ERR_CONSULTA_MASCOTA_INVALIDA", "disponibilidad diaria valida propiedad");
                var diaPasado = s.ObtenerDisponibilidadDia(Cliente, clave, 1, DateTime.Today.AddDays(-1));
                Comprobar(Convert.ToInt32(diaPasado.Compute("SUM(Cupos)", "")) == 0 && diaPasado.Select("Estado = 'Pasado'").Length == 25, "día pasado no ofrece cupos");
                Comprobar(s.ObtenerMascotas(Cliente, clave).Rows.Count == 2, "tabla de mascotas propias por SOAP");
                Comprobar(s.ObtenerHorarios(Cliente, clave).Length == 25, "horarios de 08:00 a 20:00");
                Comprobar(s.ObtenerVeterinariosDisponibles(Cliente, clave, 1, fecha).Rows.Count == 2, "permisos directos y familias anidadas con ciclo");
                Error(() => s.ObtenerHorarios(null, null), "ERR_CONSULTA_ACCESO", "usuario ausente");
                Error(() => s.ObtenerHorarios(OtroCliente, clave), "ERR_CONSULTA_ACCESO", "DNI ajeno con clave de otro usuario");
                Error(() => s.ObtenerHorarios(Cliente, "incorrecta"), "ERR_CONSULTA_ACCESO", "clave incorrecta");
                Error(() => s.ObtenerHorarios("55555555", Clave("55555555")), "ERR_CONSULTA_ACCESO", "usuario bloqueado");
                Error(() => s.ObtenerHorarios("66666666", Clave("66666666")), "ERR_CONSULTA_ACCESO", "usuario sin permiso");
                Error(() => s.ObtenerVeterinariosDisponibles(Cliente, clave, 2, fecha), "ERR_CONSULTA_MASCOTA_INVALIDA", "disponibilidad de mascota ajena");
                Error(() => s.Agendar(Cliente, clave, 2, "33333333", "Control", fecha), "ERR_CONSULTA_MASCOTA_INVALIDA", "reserva de mascota ajena");
                Error(() => s.ObtenerVeterinariosDisponibles(Cliente, clave, 1, fecha.AddSeconds(1)), "ERR_CONSULTA_FUERA_HORARIO", "segundos fuera del turno");
                Error(() => s.ObtenerVeterinariosDisponibles(Cliente, clave, 1, fecha.Date.AddHours(20.5)), "ERR_CONSULTA_FUERA_HORARIO", "fuera de atención");
                Error(() => s.ObtenerVeterinariosDisponibles(Cliente, clave, 1, DateTime.Today.AddDays(-1).AddHours(10)), "ERR_CONSULTA_FECHA_PASADA", "fecha pasada");
                Error(() => s.Agendar(Cliente, clave, 1, "33333333", "  ", fecha), "ERR_CONSULTA_MOTIVO", "motivo vacío");
                int id = s.Agendar(Cliente, clave, 1, "33333333", "Control", fecha);
                Comprobar(s.ObtenerDisponibilidadDia(Cliente, clave, 1, fecha).Select("Hora = '10:00'")[0]["Estado"].ToString() == "MascotaOcupada",
                    "mascota ocupada bloquea la hora aunque haya otro veterinario libre");
                Comprobar((int)s.ObtenerDisponibilidadDia(OtroCliente, otraClave, 2, fecha).Select("Hora = '10:00'")[0]["Cupos"] == 1,
                    "ocupación parcial conserva el cupo del otro veterinario");
                var veterinariosHora = s.ObtenerVeterinariosDelHorario(OtroCliente, otraClave, 2, fecha);
                Comprobar(veterinariosHora.Rows.Count == 2 && !(bool)veterinariosHora.Select("Dni = '33333333'")[0]["Disponible"] &&
                    (bool)veterinariosHora.Select("Dni = '44444444'")[0]["Disponible"], "lista completa distingue veterinario ocupado del disponible");
                int otroId = s.Agendar(OtroCliente, otraClave, 2, "44444444", "Completar hora", fecha);
                Comprobar(s.ObtenerDisponibilidadDia(Cliente, clave, 3, fecha).Select("Hora = '10:00'")[0]["Estado"].ToString() == "Ocupado",
                    "todos los veterinarios ocupados bloquean la hora");
                s.CancelarPropia(OtroCliente, otraClave, otroId);
                Comprobar(!new BLLDigitoVerificador().ValidarIntegridadDelSistema(), "el servicio guarda sin recalcular los dígitos");
                new BLLDigitoVerificador().InicializarTablaCompleta("Consulta");
                DataTable agendadas = s.ObtenerMisConsultas(Cliente, clave, "Agendada");
                Comprobar(id > 0 && agendadas.Rows.Count == 1 && (bool)agendadas.Rows[0]["puedeCancelar"], "reserva y listado filtrado");
                Comprobar((int)s.ObtenerProximasConsultas(Cliente, clave, 1).Rows[0]["idConsulta"] == id, "aviso de próximas consultas");
                Comprobar(s.ObtenerMisConsultas(OtroCliente, otraClave, "Agendada").Rows.Count == 0, "listado propio sin datos ajenos");
                Error(() => s.ObtenerDetallePropio(OtroCliente, otraClave, id), "ERR_CONSULTA_NO_DISPONIBLE", "detalle ajeno");
                Error(() => s.CancelarPropia(OtroCliente, otraClave, id), "ERR_CONSULTA_NO_CANCELABLE", "cancelación ajena");
                Error(() => s.ObtenerMisConsultas(Cliente, clave, "OtroEstado"), "ERR_CONSULTA_ESTADO", "filtro inválido");
                Error(() => s.Agendar(Cliente, clave, 1, "44444444", "Duplicado mascota", fecha), "ERR_CONSULTA_MASCOTA_OCUPADA", "mascota ocupada");
                Error(() => s.Agendar(OtroCliente, otraClave, 2, "33333333", "Duplicado vet", fecha), "ERR_CONSULTA_VETERINARIO_OCUPADO", "veterinario ocupado");
                Comprobar(s.ObtenerVeterinariosDisponibles(OtroCliente, otraClave, 2, fecha).Rows[0]["Dni"].ToString() == "44444444", "excluir veterinario ocupado");
                Comprobar(Escalar("SELECT motivo FROM Consulta WHERE idConsulta = " + id).ToString() != "Control", "motivo cifrado");
                Comprobar(new BLLDigitoVerificador().ValidarIntegridadDelSistema(), "integridad compatible después de reservar");
                Comprobar(s.CancelarPropia(Cliente, clave, id) && s.ObtenerDetallePropio(Cliente, clave, id).Rows[0]["estado"].ToString() == "Cancelada" &&
                    s.ObtenerVeterinariosDisponibles(Cliente, clave, 1, fecha).Rows.Count == 2, "cancelación libera el turno");
                DateTime cancelada;
                Comprobar(Escalar("SELECT estado FROM Consulta WHERE idConsulta = " + id).ToString() != "Cancelada" &&
                    DateTime.TryParseExact(Cifrado.Descifrar(Escalar("SELECT fechaCancelacion FROM Consulta WHERE idConsulta = " + id).ToString()),
                    "yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.InvariantCulture, DateTimeStyles.None, out cancelada), "estado y fecha de cancelación cifrados");
                Error(() => s.CancelarPropia(Cliente, clave, id), "ERR_CONSULTA_NO_CANCELABLE", "no cancelar dos veces");
                Comprobar(!new BLLDigitoVerificador().ValidarIntegridadDelSistema(), "cancelar no recalcula integridad dentro del servicio");
                new BLLDigitoVerificador().InicializarTablaCompleta("Consulta");
                Comprobar((int)s.ObtenerDisponibilidadDia(Cliente, clave, 1, fecha).Select("Hora = '10:00'")[0]["Cupos"] == 2,
                    "cancelaciones liberan los cupos del resumen diario");
                Comprobar(new BLLDigitoVerificador().ValidarIntegridadDelSistema(), "integridad actualizada por BLL después de cancelar");
                int[] reservas = Task.WhenAll(Reservar(1, Cliente, fecha.AddHours(1)), Reservar(2, OtroCliente, fecha.AddHours(1))).GetAwaiter().GetResult();
                Comprobar(reservas.Count(r => r > 0) == 1 && reservas.Count(r => r == -1) == 1, "dos reservas simultáneas: solo una ocupa el turno");
                int historica = s.Agendar(Cliente, clave, 3, "44444444", "Histórica", fecha.AddHours(2));
                ActualizarHistorica(historica, false);
                DataRow pasada = s.ObtenerDetallePropio(Cliente, clave, historica).Rows[0];
                Comprobar((bool)pasada["pendienteDeRegistro"] && !(bool)pasada["puedeCancelar"] && s.ObtenerProximasConsultas(Cliente, clave, 3).Rows.Count == 0, "pasada pendiente de registro");
                Error(() => s.CancelarPropia(Cliente, clave, historica), "ERR_CONSULTA_NO_CANCELABLE", "no cancelar consulta pasada");
                ActualizarHistorica(historica, true);
                DataTable atendidas = s.ObtenerMisConsultas(Cliente, clave, "Atendida");
                Comprobar(atendidas.Rows.Count == 1 && atendidas.Rows[0]["diagnostico"].ToString() == "Sin hallazgos" && !(bool)atendidas.Rows[0]["puedeCancelar"], "diagnóstico de atendida");
                Error(() => s.CancelarPropia(Cliente, clave, historica), "ERR_CONSULTA_NO_CANCELABLE", "no cancelar atendida");
                int cantidad = Convert.ToInt32(Escalar("SELECT COUNT(*) FROM Consulta"));
                Sql("CREATE TRIGGER FallarEvento ON Evento AFTER INSERT AS THROW 51000, 'Error de prueba', 1;");
                try { s.Agendar(Cliente, clave, 3, "44444444", "No guardar", fecha.AddHours(3)); throw new Exception("No falló la auditoría"); }
                catch (SoapException) { Comprobar(true, "fallo de auditoría rechaza la operación"); }
                Sql("DROP TRIGGER FallarEvento");
                Comprobar(Convert.ToInt32(Escalar("SELECT COUNT(*) FROM Consulta")) == cantidad && new BLLDigitoVerificador().ValidarIntegridadDelSistema(), "rollback de reserva e integridad");

                var contexto = new HttpContext(new HttpRequest("", "http://localhost/", ""), new HttpResponse(new StringWriter()));
                SessionStateUtility.AddHttpSessionStateToContext(contexto, new HttpSessionStateContainer("test", new SessionStateItemCollection(),
                    new HttpStaticObjectsCollection(), 20, true, HttpCookieMode.UseCookies, SessionStateMode.InProc, false));
                HttpContext.Current = contexto;
                SessionManager.Login(new BEUsuario { DNI = Cliente, Contraseña = clave });
                var bll = new BLLConsulta();
                Comprobar(bll.ObtenerDisponibilidadDia(3, fecha).Rows.Count == 25, "disponibilidad diaria recorre BLL, Servicios y SOAP");
                HttpContext.Current = null;
                var pagina = new ConsultaPruebas();
                HttpContext.Current = contexto;
                pagina.Preparar();
                Comprobar(!pagina.CargarDia() && !pagina.Horas.Enabled, "formulario espera una fecha antes de habilitar horarios");
                pagina.Fecha.Text = fecha.ToString("yyyy-MM-dd");
                Comprobar(pagina.CargarDia() && pagina.Horas.Items.Count == 26, "elegir fecha carga el día sin exigir elegir primero una hora");
                var sinCupo = bll.ObtenerDisponibilidadDia(3, fecha);
                sinCupo.Rows[0]["Cupos"] = 0;
                sinCupo.Rows[0]["Estado"] = "Ocupado";
                pagina.Horas.SelectedValue = "08:00";
                pagina.Mostrar(sinCupo);
                Comprobar(pagina.Horas.Items.FindByValue("08:00").Attributes["disabled"] == "disabled" && pagina.Horas.SelectedValue == "" &&
                    pagina.Horas.Items.FindByValue("08:00").Attributes["style"].Contains("#757575"), "hora ocupada queda gris, deshabilitada y sin selección anterior");
                var html = new StringWriter();
                pagina.Horas.RenderControl(new System.Web.UI.HtmlTextWriter(html));
                Comprobar(System.Text.RegularExpressions.Regex.IsMatch(html.ToString(), "<option(?=[^>]*value=\"08:00\")(?=[^>]*disabled)[^>]*>"),
                    "el HTML del selector impide elegir la opción ocupada");
                contexto.Session["IdiomaActual"] = "en-US";
                Comprobar(pagina.MostrarErrorServicio().Contains("service is unavailable"), "error de conexión se muestra en inglés");
                pagina.Mostrar(sinCupo);
                Comprobar(pagina.Resumen.Text.Contains("Available appointments") && pagina.Horas.Items.FindByValue("08:00").Text == "08:00" &&
                    pagina.Horas.Items.FindByValue("08:30").Text == "08:30", "resumen traducido y opciones con solo la hora");
                pagina.MostrarVeterinarios(veterinariosHora);
                Comprobar(pagina.Veterinarios.Items.FindByValue("33333333").Attributes["disabled"] == "disabled" &&
                    pagina.Veterinarios.Items.FindByValue("44444444").Attributes["disabled"] == null, "selector de veterinarios muestra ocupados deshabilitados y libres seleccionables");
                var htmlVeterinarios = new StringWriter();
                pagina.Veterinarios.RenderControl(new System.Web.UI.HtmlTextWriter(htmlVeterinarios));
                Comprobar(System.Text.RegularExpressions.Regex.IsMatch(htmlVeterinarios.ToString(), "<option(?=[^>]*value=\"33333333\")(?=[^>]*disabled)[^>]*>"),
                    "HTML conserva el veterinario ocupado visible y deshabilitado");
                pagina.Veterinarios.SelectedValue = "44444444";
                pagina.Veterinarios.Items.FindByValue("33333333").Attributes.Clear();
                pagina.RestaurarVeterinarios();
                Comprobar(pagina.Veterinarios.Items.FindByValue("33333333").Attributes["disabled"] == "disabled" && pagina.Veterinarios.SelectedValue == "44444444",
                    "postbacks restauran la marca de ocupado y conservan el veterinario libre elegido");
                contexto.Session["IdiomaActual"] = "es-AR";
                Comprobar(bll.ObtenerMascotas().Rows.Count == 2 && bll.ObtenerHorarios().Length == 25, "BLL obtiene datos mediante la capa de servicios");
                var reserva = new BEConsulta(3, new BEUsuario { DNI = "44444444" }, "Desde BLL", fecha.AddHours(5));
                int idBll = bll.Agendar(reserva);
                Comprobar(new BLLDigitoVerificador().ValidarIntegridadDelSistema() && (int)bll.ObtenerDetallePropio(idBll)["idConsulta"] == idBll,
                    "BLL reserva por SOAP y recalcula integridad");
                bll.CancelarPropia(idBll);
                Comprobar(new BLLDigitoVerificador().ValidarIntegridadDelSistema() && bll.ObtenerDetallePropio(idBll)["estado"].ToString() == "Cancelada",
                    "BLL cancela por SOAP y recalcula integridad");
                try { bll.ObtenerVeterinariosDisponibles(2, fecha); throw new Exception("No falló el acceso ajeno"); }
                catch (BECustomException ex) { Comprobar(ex.CodigoError == "ERR_CONSULTA_MASCOTA_INVALIDA" && ex.Argumentos.Length == 1,
                    "Servicios convierte el error SOAP y preserva argumentos para traducción"); }
                Sql("UPDATE Consulta SET motivo = 'alterado' WHERE idConsulta = " + id);
                Comprobar(!new BLLDigitoVerificador().ValidarIntegridadDelSistema(), "la web detecta corrupción con su BLL antes de escribir");
                try { bll.Agendar(reserva); throw new Exception("No se bloqueó la escritura"); }
                catch (BECustomException ex) { Comprobar(ex.CodigoError == "ERR_CONSULTA_MANTENIMIENTO", "BLL bloquea escritura antes de llamar al servicio si hay corrupción"); }
                SessionManager.LogOut();
                try { bll.ObtenerMisConsultas(); throw new Exception("No se bloqueó la sesión vacía"); }
                catch (BECustomException ex) { Comprobar(ex.CodigoError == "ERR_CONSULTA_ACCESO", "BLL requiere sesión para acceder al servicio"); }
            }
            Console.WriteLine("PASS: " + pruebas + " comprobaciones de integración.");
            return 0;
        }
        catch (Exception ex) { Console.Error.WriteLine(ex); return 1; }
    }

    private static void ActualizarHistorica(int id, bool atendida)
    {
        using (var con = new SqlConnection(Conexion))
        using (var cmd = new SqlCommand("UPDATE Consulta SET fechaHora = @Fecha, estado = @Estado, diagnostico = @Diagnostico WHERE idConsulta = @Id", con))
        {
            cmd.Parameters.AddWithValue("@Fecha", Cifrado.Cifrar(DateTime.Today.AddDays(-1).AddHours(10).ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)));
            cmd.Parameters.AddWithValue("@Estado", Cifrado.Cifrar(atendida ? "Atendida" : "Agendada"));
            cmd.Parameters.AddWithValue("@Diagnostico", Cifrado.Cifrar(atendida ? "Sin hallazgos" : ""));
            cmd.Parameters.AddWithValue("@Id", id); con.Open(); cmd.ExecuteNonQuery();
        }
        new BLLDigitoVerificador().InicializarTablaCompleta("Consulta");
    }

    private static Task<int> Reservar(int mascota, string dni, DateTime fecha)
    {
        return Task.Run(() => {
            using (var s = Servicio())
            {
                try { return s.Agendar(dni, Clave(dni), mascota, "33333333", "Simultánea", fecha); }
                catch (SoapException ex) when (ex.Message.Contains("ERR_CONSULTA_VETERINARIO_OCUPADO")) { return -1; }
            }
        });
    }
}

internal class ConsultaPruebas : FGF_Finanzas.Consulta
{
    public System.Web.UI.WebControls.DropDownList Horas { get { return ddlHora; } }
    public System.Web.UI.WebControls.DropDownList Veterinarios { get { return ddlVeterinario; } }
    public System.Web.UI.WebControls.TextBox Fecha { get { return txtFecha; } }
    public System.Web.UI.WebControls.Label Resumen { get { return lblDisponibilidadDia; } }
    public void Preparar()
    {
        lblError = new System.Web.UI.WebControls.Label();
        ddlHora = new System.Web.UI.WebControls.DropDownList();
        ddlVeterinario = new System.Web.UI.WebControls.DropDownList();
        txtFecha = new System.Web.UI.WebControls.TextBox();
        dropMascota = new System.Web.UI.WebControls.DropDownList();
        dropMascota.Items.Add(new System.Web.UI.WebControls.ListItem("Nube", "3"));
        lblDisponibilidadDia = new System.Web.UI.WebControls.Label();
    }
    public bool CargarDia()
    {
        return (bool)typeof(FGF_Finanzas.Consulta).GetMethod("CargarDisponibilidadDia", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(this, null);
    }
    public void Mostrar(DataTable datos)
    {
        ViewState["DisponibilidadDia"] = datos;
        typeof(FGF_Finanzas.Consulta).GetMethod("MostrarDisponibilidadDia", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(this, null);
    }
    public void MostrarVeterinarios(DataTable datos)
    {
        typeof(FGF_Finanzas.Consulta).GetMethod("CargarVeterinarios", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(this, new object[] { datos });
    }
    public void RestaurarVeterinarios()
    {
        typeof(FGF_Finanzas.Consulta).GetMethod("MostrarVeterinariosHorario", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(this, null);
    }
    public string MostrarErrorServicio()
    {
        typeof(FGF_Finanzas.Consulta).GetMethod("MostrarError", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(this, new object[] { new BECustomException("ERR_CONSULTA_SERVICIO") });
        return lblError.Text;
    }
}
