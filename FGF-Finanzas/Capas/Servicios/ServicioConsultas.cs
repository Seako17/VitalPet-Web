using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.ServiceReferences.Consultas;
using System;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.Services.Protocols;

namespace FGF_Finanzas.Capas.Servicios
{
    public class ServicioConsultas
    {
        private ConsultasService CrearServicio()
        {
            string url = ConfigurationManager.AppSettings["ConsultasService.Url"];
            Uri direccion;
            if (!Uri.TryCreate(url, UriKind.Absolute, out direccion) ||
                (direccion.Scheme != "https" && !(direccion.Scheme == "http" && direccion.IsLoopback)))
                throw new BECustomException("ERR_CONSULTA_SERVICIO");
            return new ConsultasService { Url = url, Timeout = 15000, AllowAutoRedirect = false };
        }

        public DataTable ObtenerMascotas(BEUsuario usuario)
        {
            using (ConsultasService ws = CrearServicio())
            {
                try { return ws.ObtenerMascotas(usuario.DNI, usuario.Contraseña); }
                catch (Exception ex) { throw TraducirError(ex); }
            }
        }

        public DataTable ObtenerConsultasVeterinario(BEUsuario usuario)
        {
            using (ConsultasService ws = CrearServicio())
            {
                try { return ws.ObtenerConsultasVeterinario(usuario.DNI, usuario.Contraseña); }
                catch (Exception ex) { throw TraducirError(ex); }
            }
        }

        public string[] ObtenerHorarios(BEUsuario usuario)
        {
            using (ConsultasService ws = CrearServicio())
            {
                try { return ws.ObtenerHorarios(usuario.DNI, usuario.Contraseña); }
                catch (Exception ex) { throw TraducirError(ex); }
            }
        }

        public DataTable ObtenerVeterinariosDisponibles(BEUsuario usuario, int idMascota, DateTime fechaHora)
        {
            using (ConsultasService ws = CrearServicio())
            {
                try { return ws.ObtenerVeterinariosDisponibles(usuario.DNI, usuario.Contraseña, idMascota, fechaHora); }
                catch (Exception ex) { throw TraducirError(ex, fechaHora); }
            }
        }

        public DataTable ObtenerDisponibilidadDia(BEUsuario usuario, int idMascota, DateTime fecha)
        {
            using (ConsultasService ws = CrearServicio())
            {
                try { return ws.ObtenerDisponibilidadDia(usuario.DNI, usuario.Contraseña, idMascota, fecha); }
                catch (Exception ex) { throw TraducirError(ex); }
            }
        }

        public DataTable ObtenerVeterinariosDelHorario(BEUsuario usuario, int idMascota, DateTime fechaHora)
        {
            using (ConsultasService ws = CrearServicio())
            {
                try { return ws.ObtenerVeterinariosDelHorario(usuario.DNI, usuario.Contraseña, idMascota, fechaHora); }
                catch (Exception ex) { throw TraducirError(ex, fechaHora); }
            }
        }

        public int Agendar(BEUsuario usuario, BEConsulta consulta)
        {
            using (ConsultasService ws = CrearServicio())
            {
                try { return ws.Agendar(usuario.DNI, usuario.Contraseña, consulta.idMascota, consulta.veterinario.DNI, consulta.motivo, consulta.fechaHora); }
                catch (Exception ex) { throw TraducirError(ex, consulta.fechaHora); }
            }
        }

        public DataTable ObtenerMisConsultas(BEUsuario usuario, string estado)
        {
            using (ConsultasService ws = CrearServicio())
            {
                try { return ws.ObtenerMisConsultas(usuario.DNI, usuario.Contraseña, estado); }
                catch (Exception ex) { throw TraducirError(ex); }
            }
        }

        public DataTable ObtenerProximasConsultas(BEUsuario usuario, int idMascota)
        {
            using (ConsultasService ws = CrearServicio())
            {
                try { return ws.ObtenerProximasConsultas(usuario.DNI, usuario.Contraseña, idMascota); }
                catch (Exception ex) { throw TraducirError(ex); }
            }
        }

        public DataRow ObtenerDetallePropio(BEUsuario usuario, int idConsulta)
        {
            using (ConsultasService ws = CrearServicio())
            {
                try { return ws.ObtenerDetallePropio(usuario.DNI, usuario.Contraseña, idConsulta).Rows[0]; }
                catch (Exception ex) { throw TraducirError(ex); }
            }
        }

        public bool CancelarPropia(BEUsuario usuario, int idConsulta)
        {
            using (ConsultasService ws = CrearServicio())
            {
                try { return ws.CancelarPropia(usuario.DNI, usuario.Contraseña, idConsulta); }
                catch (Exception ex) { throw TraducirError(ex); }
            }
        }

        private static BECustomException TraducirError(Exception ex, params object[] argumentos)
        {
            string codigo = ex is SoapException ? Regex.Match(ex.Message, @"\bERR_CONSULTA_[A-Z_]+\b").Value : "";
            return new BECustomException(string.IsNullOrEmpty(codigo) ? "ERR_CONSULTA_SERVICIO" : codigo, ex, argumentos);
        }
    }
}
