using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.Servicios;
using System;
using System.Data;

namespace FGF_Finanzas.Capas.BLL
{
    public class BLLConsulta
    {
        private readonly ServicioConsultas servicio = new ServicioConsultas();
        private readonly BLLDigitoVerificador digitos = new BLLDigitoVerificador();

        private BEUsuario UsuarioActual()
        {
            BEUsuario usuario = SessionManager.Instancia?.Usuario;
            if (usuario == null || string.IsNullOrEmpty(usuario.DNI) || string.IsNullOrEmpty(usuario.Contraseña))
                throw new BECustomException("ERR_CONSULTA_ACCESO");
            return usuario;
        }

        public DataTable ObtenerMascotas() { return servicio.ObtenerMascotas(UsuarioActual()); }
        public string[] ObtenerHorarios() { return servicio.ObtenerHorarios(UsuarioActual()); }
        public DataTable ObtenerDisponibilidadDia(int idMascota, DateTime fecha)
        { return servicio.ObtenerDisponibilidadDia(UsuarioActual(), idMascota, fecha); }
        public DataTable ObtenerVeterinariosDisponibles(int idMascota, DateTime fechaHora)
        { return servicio.ObtenerVeterinariosDisponibles(UsuarioActual(), idMascota, fechaHora); }
        public DataTable ObtenerVeterinariosDelHorario(int idMascota, DateTime fechaHora)
        { return servicio.ObtenerVeterinariosDelHorario(UsuarioActual(), idMascota, fechaHora); }
        public DataTable ObtenerMisConsultas(string estado = "")
        { return servicio.ObtenerMisConsultas(UsuarioActual(), estado); }
        public DataTable ObtenerProximasConsultas(int idMascota)
        { return servicio.ObtenerProximasConsultas(UsuarioActual(), idMascota); }
        public DataRow ObtenerDetallePropio(int idConsulta)
        { return servicio.ObtenerDetallePropio(UsuarioActual(), idConsulta); }

        public int Agendar(BEConsulta consulta)
        {
            BEUsuario usuario = UsuarioActual();
            if (consulta == null || consulta.veterinario == null) throw new BECustomException("ERR_CONSULTA_VETERINARIO");
            if (!digitos.ValidarIntegridadDelSistema()) throw new BECustomException("ERR_CONSULTA_MANTENIMIENTO");
            int id = servicio.Agendar(usuario, consulta);
            digitos.InicializarTablaCompleta("Consulta");
            return id;
        }

        public void CancelarPropia(int idConsulta)
        {
            BEUsuario usuario = UsuarioActual();
            if (!digitos.ValidarIntegridadDelSistema()) throw new BECustomException("ERR_CONSULTA_MANTENIMIENTO");
            if (!servicio.CancelarPropia(usuario, idConsulta)) throw new BECustomException("ERR_CONSULTA_NO_CANCELABLE");
            digitos.InicializarTablaCompleta("Consulta");
        }
    }
}
