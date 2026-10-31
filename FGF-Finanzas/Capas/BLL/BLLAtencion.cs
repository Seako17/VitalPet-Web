using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FGF_Finanzas.Capas.BLL
{
    public class BLLAtencion
    {
        DALAtencion dalAtencion;
        BLLConsulta bllConsulta;
        BLLMascota bllMascota;
        BLLUsuario bllUsuario;
        BLLDigitoVerificador bLLDigitoVerificador;

        public BLLAtencion()
        {
            dalAtencion = new DALAtencion();
            bllConsulta = new BLLConsulta();
            bllMascota = new BLLMascota();
            bllUsuario = new BLLUsuario();
            bLLDigitoVerificador = new BLLDigitoVerificador();
        }

        public List<BEAtencion> ObtenerTodos()
        {
            return dalAtencion.ObtenerTodos();
        }

        public BEAtencion ObtenerPorConsulta(int idConsulta)
        {
            return dalAtencion.ObtenerPorConsulta(idConsulta);
        }

        public DataTable ObtenerConsultasSinAtender()
        {
            DataTable consultas = ObtenerConsultasConDetalle();
            List<BEAtencion> atenciones = dalAtencion.ObtenerTodos();

            HashSet<int> consultasAtendidas = new HashSet<int>(
                atenciones.Where(a => a.atendido).Select(a => a.id_consulta));

            var pendientes = consultas.AsEnumerable()
                .Where(c => c["estado"].ToString() != "Atendida" &&
                    !consultasAtendidas.Contains(Convert.ToInt32(c["idConsulta"])));

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

        public void Guardar(BEAtencion atencion)
        {
            ValidarAtencion(atencion);
            dalAtencion.Guardar(atencion);
            bLLDigitoVerificador.InicializarTablaCompleta("Atencion");
        }

        public void Actualizar(BEAtencion atencion)
        {
            if (atencion == null || atencion.id_atencion <= 0)
            {
                throw new BECustomException("ERR_ATENCION_ID_INVALIDO");
            }

            ValidarAtencion(atencion);
            dalAtencion.Actualizar(atencion);
            bLLDigitoVerificador.InicializarTablaCompleta("Atencion");
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
            if (!string.IsNullOrWhiteSpace(atencion.observaciones))
            {
                atencion.observaciones = atencion.observaciones.Trim();
            }
        }
    }
}
