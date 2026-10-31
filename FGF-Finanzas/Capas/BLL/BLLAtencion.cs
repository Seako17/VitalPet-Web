using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.Servicios;
using System;
using System.Data;
using System.Linq;

namespace FGF_Finanzas.Capas.BLL
{
    public class BLLAtencion
    {
        private readonly ServicioConsultas servicio = new ServicioConsultas();
        BLLConsulta bllConsulta;
        BLLMascota bllMascota;
        BLLUsuario bllUsuario;
        BLLDigitoVerificador bLLDigitoVerificador;

        public BLLAtencion()
        {
            bllConsulta = new BLLConsulta();
            bllMascota = new BLLMascota();
            bllUsuario = new BLLUsuario();
            bLLDigitoVerificador = new BLLDigitoVerificador();
        }

        private BEUsuario UsuarioActual()
        {
            BEUsuario usuario = SessionManager.Instancia?.Usuario;
            if (usuario == null || string.IsNullOrEmpty(usuario.DNI) || string.IsNullOrEmpty(usuario.Contraseña))
                throw new BECustomException("ERR_CONSULTA_ACCESO");
            return usuario;
        }

        public BEAtencion ObtenerPorConsulta(int idConsulta)
        {
            DataTable tabla = servicio.ObtenerAtencion(UsuarioActual(), idConsulta);
            if (tabla.Rows.Count == 0) return null;
            DataRow fila = tabla.Rows[0];
            return new BEAtencion
            {
                id_atencion = (int)fila["id_atencion"], id_consulta = (int)fila["id_consulta"],
                diagnostico = fila["diagnostico"].ToString(), observaciones = fila["observaciones"].ToString(),
                atendido = (bool)fila["atendido"]
            };
        }

        public DataTable ObtenerConsultasPorEstado(string estado)
        {
            DataTable consultas = ObtenerConsultasConDetalle();
            var pendientes = consultas.AsEnumerable()
                .Where(c => c["estado"].ToString() == estado);

            DataTable resultado = pendientes.Any() ? pendientes.CopyToDataTable() : consultas.Clone();

            DataView dv = resultado.DefaultView;
            dv.Sort = "fechaHora ASC";
            return dv.ToTable();
        }

        public DataRow ObtenerConsultaPorId(int idConsulta)
        {
            return ObtenerConsultasConDetalle().AsEnumerable()
                .FirstOrDefault(c => Convert.ToInt32(c["idConsulta"]) == idConsulta);
        }

        public void MarcarNoSePresento(int idConsulta)
        {
            BEUsuario usuario = UsuarioActual();
            if (!bLLDigitoVerificador.ValidarIntegridadDelSistema())
                throw new BECustomException("ERR_CONSULTA_MANTENIMIENTO");
            servicio.MarcarNoSePresento(usuario, idConsulta);
            bLLDigitoVerificador.InicializarTablaCompleta("Consulta");
        }

        public void Guardar(BEAtencion atencion)
        {
            ValidarAtencion(atencion);
            if (atencion.id_atencion != 0) throw new BECustomException("ERR_ATENCION_ID_INVALIDO");
            GuardarEnServicio(atencion);
        }

        public void Actualizar(BEAtencion atencion)
        {
            if (atencion == null || atencion.id_atencion <= 0)
            {
                throw new BECustomException("ERR_ATENCION_ID_INVALIDO");
            }

            ValidarAtencion(atencion);
            GuardarEnServicio(atencion);
        }

        private void GuardarEnServicio(BEAtencion atencion)
        {
            BEUsuario usuario = UsuarioActual();
            if (!bLLDigitoVerificador.ValidarIntegridadDelSistema())
                throw new BECustomException("ERR_CONSULTA_MANTENIMIENTO");
            atencion.id_atencion = servicio.GuardarAtencion(usuario, atencion);
            atencion.atendido = true;
            bLLDigitoVerificador.InicializarTablaCompleta("Atencion");
            bLLDigitoVerificador.InicializarTablaCompleta("Consulta");
        }

        private DataTable ObtenerConsultasConDetalle()
        {
            DataTable consultas = bllConsulta.ObtenerConsultas();

            consultas.Columns.Add("mascota", typeof(string));
            consultas.Columns.Add("veterinario", typeof(string));

            var mascotas = bllMascota.ObtenerMascotas().AsEnumerable().ToList();
            var usuarios = bllUsuario.ObtenerUsuarios().AsEnumerable().ToList();

            foreach (DataRow consulta in consultas.Rows)
            {
                int idMascota = Convert.ToInt32(consulta["idMascota"]);
                string dniVeterinario = consulta["DNI_Veterinario"].ToString();

                consulta["mascota"] = mascotas
                    .Where(m => Convert.ToInt32(m["id_mascota"]) == idMascota)
                    .Select(m => m["nombre"].ToString())
                    .FirstOrDefault();

                consulta["veterinario"] = usuarios
                    .Where(u => u["DNI"].ToString() == dniVeterinario)
                    .Select(u => u["nombre"] + " " + u["apellido"])
                    .FirstOrDefault();
            }

            return consultas;
        }

        private void ValidarAtencion(BEAtencion atencion)
        {
            if (atencion == null)
            {
                throw new BECustomException("ERR_ATENCION_NULA");
            }

            if (atencion.id_consulta <= 0)
            {
                throw new BECustomException("ERR_ATENCION_CONSULTA_INVALIDA");
            }

            if (string.IsNullOrWhiteSpace(atencion.diagnostico))
            {
                throw new BECustomException("ERR_ATENCION_DIAGNOSTICO_OBLIGATORIO");
            }

            atencion.diagnostico = atencion.diagnostico.Trim();
            if (string.IsNullOrWhiteSpace(atencion.observaciones))
                throw new BECustomException("ERR_ATENCION_OBSERVACIONES_OBLIGATORIAS");
            if (!string.IsNullOrWhiteSpace(atencion.observaciones))
            {
                atencion.observaciones = atencion.observaciones.Trim();
            }
        }
    }
}
