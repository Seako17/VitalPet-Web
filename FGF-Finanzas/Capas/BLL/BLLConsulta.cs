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
        private const string ROL_VETERINARIO = "Veterinario";
        private const int HORA_INICIO = 8;      // 08:00
        private const int HORA_FIN = 20;        // 20:00
        private const int INTERVALO_MIN = 30;

        DALConsulta dalConsulta;
        BLLUsuario bllUsuario;
        BLLMascota bllMascota;
        BLLEvento bllEvento;
        BLLDigitoVerificador bllDigitoVerificador;

        public BLLConsulta()
        {
            dalConsulta = new DALConsulta();
            bllUsuario = new BLLUsuario();
            bllMascota = new BLLMascota();
            bllEvento = new BLLEvento();
            bllDigitoVerificador = new BLLDigitoVerificador();
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


        public DataTable ObtenerConsultas()
        {
            return dalConsulta.ObtenerConsultas();
        }

        public DataTable ObtenerVeterinarios()
        {
            DataTable usuarios = bllUsuario.ObtenerUsuarios();

            var filas = usuarios.AsEnumerable()
                .Where(u => u["rol"].ToString() == ROL_VETERINARIO);

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

            if (enEseHorario.Any(c => Convert.ToInt32(c["id_mascota"]) == idMascota))
            {
                throw new Exception(string.Format(
                    "La mascota ya tiene una consulta agendada el {0:dd/MM/yyyy} a las {0:HH:mm}. No es posible agendar en ese horario.",
                    fechaHora));
            }

            var ocupados = new HashSet<string>(enEseHorario.Select(c => c["DNI_Veterinario"].ToString()));

            DataTable veterinarios = ObtenerVeterinarios();
            var libres = veterinarios.AsEnumerable()
                .Where(v => !ocupados.Contains(v["dni"].ToString()));

            if (!libres.Any())
            {
                throw new Exception(string.Format(
                    "El horario {0:dd/MM/yyyy} {0:HH:mm} ya está agendado y no hay veterinarios disponibles. No es posible agendar en ese horario.",
                    fechaHora));
            }

            return libres.CopyToDataTable();
        }

        public void Agendar(BEConsulta consulta)
        {
            if (!bllDigitoVerificador.ValidarIntegridadDelSistema())
            {
                throw new Exception("No se pueden registrar consultas. El sistema se encuentra en mantenimiento.");
            }

            if (string.IsNullOrWhiteSpace(consulta.motivo))
                throw new Exception("Ingrese el motivo de la consulta.");

            DataTable mascotas = bllMascota.ObtenerMascotasDeUsuario(SessionManager.Instancia.Usuario);
            string columnaId = mascotas.Columns[0].ColumnName;
            if (!mascotas.AsEnumerable().Any(m => Convert.ToInt32(m[columnaId]) == consulta.idMascota))
                throw new Exception("La mascota seleccionada no es válida.");

            DataTable libres = ObtenerVeterinariosDisponibles(consulta.idMascota, consulta.fechaHora);
            if (!libres.AsEnumerable().Any(v => v["DNI"].ToString() == consulta.veterinario.DNI))
                throw new Exception("El veterinario/a elegido ya no está disponible en ese horario.");

            dalConsulta.AgregarConsulta(consulta);
            bllEvento.AgregarEvento(new BEEvento(SessionManager.Instancia.Usuario, DateTime.Now, "Consultas", "Agendar Consulta", 4));
            bllDigitoVerificador.InicializarTablaCompleta("Consulta");
        }


        private void ValidarHorario(DateTime fechaHora)
        {
            if (fechaHora <= DateTime.Now)
                throw new Exception("No se puede agendar una consulta en una fecha y hora pasadas.");

            if (!ObtenerHorarios().Contains(fechaHora.ToString("HH:mm")))
                throw new Exception("El horario elegido está fuera de la agenda de atención.");
        }
    }
}