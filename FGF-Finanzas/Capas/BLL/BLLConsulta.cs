using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.DAL;
using FGF_Finanzas.Capas.Servicios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace FGF_Finanzas.Capas.BLL
{
    public class BLLConsulta
    {
        private const int HORA_INICIO = 8;      // 08:00
        private const int HORA_FIN = 20;        // 20:00
        private const int INTERVALO_MIN = 30;
        private const string PERMISO_AGENDAR_CONSULTA = "ConsultaAtender";

        DALConsulta dalConsulta;
        BLLUsuario bllUsuario;
        BLLMascota bllMascota;
        BLLEvento bllEvento;
        BLLDigitoVerificador bllDigitoVerificador;
        BLLPermiso bllPermiso;

        public BLLConsulta()
        {
            dalConsulta = new DALConsulta();
            bllUsuario = new BLLUsuario();
            bllMascota = new BLLMascota();
            bllEvento = new BLLEvento();
            bllDigitoVerificador = new BLLDigitoVerificador();
            bllPermiso = new BLLPermiso();
        }

        public List<string> ObtenerHorarios()
        {
            var horarios = new List<string>();
            DateTime actual = DateTime.Today.AddHours(HORA_INICIO);
            DateTime fin = DateTime.Today.AddHours(HORA_FIN);

            while (actual <= fin)
            {
                horarios.Add(actual.ToString("HH:mm"));
                actual = actual.AddMinutes(INTERVALO_MIN);
            }
            return horarios;
        }

        public DataTable ObtenerVeterinarios()
        {
            Permiso permisoAgendar = bllPermiso.ObtenerPermisos()
                .Find(p => p.Nombre == PERMISO_AGENDAR_CONSULTA);

            if (permisoAgendar == null)
                throw new BECustomException("ERR_CONSULTA_PERMISO_FALTANTE", PERMISO_AGENDAR_CONSULTA);

  
            DataTable usuarios = bllUsuario.ObtenerUsuarios();

            var filas = usuarios.AsEnumerable().Where(u =>
            {
                Rol rol = u["rol"] as Rol;
                return rol != null && rol.TienePermiso(permisoAgendar);
            });

            DataTable veterinarios = filas.Any() ? filas.CopyToDataTable() : usuarios.Clone();

            veterinarios.Columns.Add("nombreCompleto", typeof(string));
            foreach (DataRow fila in veterinarios.Rows)
            {
                fila["nombreCompleto"] = fila["nombre"] + " " + fila["apellido"];
            }
            return veterinarios;
        }

        public DataTable ObtenerVeterinariosDisponibles(int idMascota, DateTime fechaHora)
        {
            ValidarHorario(fechaHora);

            DataTable consultas = dalConsulta.ObtenerConsultas();
            var enEseHorario = consultas.AsEnumerable()
                .Where(c => Convert.ToDateTime(c["fechaHora"]) == fechaHora)
                .ToList();

            if (enEseHorario.Any(c => Convert.ToInt32(c["idMascota"]) == idMascota))
            {
                throw new BECustomException("ERR_CONSULTA_MASCOTA_OCUPADA", fechaHora);
            }

            var ocupados = new HashSet<string>(enEseHorario.Select(c => c["DNI_Veterinario"].ToString()));

            DataTable veterinarios = ObtenerVeterinarios();
            var libres = veterinarios.AsEnumerable()
                .Where(v => !ocupados.Contains(v["dni"].ToString()));

            if (!libres.Any())
            {
                throw new BECustomException("ERR_CONSULTA_SIN_VETERINARIOS", fechaHora);
            }

            return libres.CopyToDataTable();
        }

        public void Agendar(BEConsulta consulta)
        {
            if (!bllDigitoVerificador.ValidarIntegridadDelSistema())
            {
                throw new BECustomException("ERR_CONSULTA_MANTENIMIENTO");
            }

            if (string.IsNullOrWhiteSpace(consulta.motivo))
                throw new BECustomException("ERR_CONSULTA_MOTIVO");

            DataTable mascotas = bllMascota.ObtenerMascotasDeUsuario(SessionManager.Instancia.Usuario);
            string columnaId = mascotas.Columns[0].ColumnName;
            if (!mascotas.AsEnumerable().Any(m => Convert.ToInt32(m[columnaId]) == consulta.idMascota))
                throw new BECustomException("ERR_CONSULTA_MASCOTA_INVALIDA");

            DataTable libres = ObtenerVeterinariosDisponibles(consulta.idMascota, consulta.fechaHora);
            if (!libres.AsEnumerable().Any(v => v["DNI"].ToString() == consulta.veterinario.DNI))
                throw new BECustomException("ERR_CONSULTA_VETERINARIO_OCUPADO");

            dalConsulta.AgregarConsulta(consulta);
            bllEvento.AgregarEvento(new BEEvento(SessionManager.Instancia.Usuario, DateTime.Now, "Consultas", "Agendar Consulta", 4));
            bllDigitoVerificador.InicializarTablaCompleta("Consulta");
        }


        private void ValidarHorario(DateTime fechaHora)
        {
            if (fechaHora <= DateTime.Now)
                throw new BECustomException("ERR_CONSULTA_FECHA_PASADA");

            if (!ObtenerHorarios().Contains(fechaHora.ToString("HH:mm")))
                throw new BECustomException("ERR_CONSULTA_FUERA_HORARIO");
        }
    }
}
