using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.DAL;
using FGF_Finanzas.Capas.Servicios;
using System;
using System.Data;
using System.Globalization;
using System.Linq;

namespace FGF_Finanzas.Capas.BLL
{
    public class BLLInformeConsultas
    {
        private readonly DALInformeConsultas dal = new DALInformeConsultas();
        private readonly BLLDigitoVerificador digitos = new BLLDigitoVerificador();

        private void ValidarAcceso()
        {
            var usuario = SessionManager.Instancia?.Usuario;
            var permiso = CodigosPermiso.ObtenerPermisos().Find(p => p.Nombre == "InformeVer");
            if (usuario?.Rol?.Componentes == null || permiso == null || !usuario.Rol.TienePermiso(permiso))
                throw new BECustomException("ERR_INFORME_ACCESO");
        }

        public DataTable ObtenerVeterinarios() { ValidarAcceso(); return dal.ObtenerVeterinarios(); }
        public DataTable Listar() { ValidarAcceso(); return dal.Listar(); }
        public BEInformeConsultas Obtener(int id)
        {
            ValidarAcceso();
            if (!digitos.ValidarIntegridadDelSistema()) throw new BECustomException("ERR_CONSULTA_MANTENIMIENTO");
            return dal.Obtener(id) ?? throw new BECustomException("ERR_INFORME_NO_EXISTE");
        }

        public int Generar(int anio, int mes, string dniVeterinario)
        {
            ValidarAcceso();
            DateTime hoy = DateTime.Today;
            if (anio < 2000 || anio > hoy.Year || mes < 1 || mes > 12 || (anio == hoy.Year && mes > hoy.Month))
                throw new BECustomException("ERR_INFORME_PERIODO");
            if (!digitos.ValidarIntegridadDelSistema()) throw new BECustomException("ERR_CONSULTA_MANTENIMIENTO");
            var informe = new BEInformeConsultas { Anio = anio, Mes = mes, Generado = DateTime.Now,
                DniVeterinario = string.IsNullOrEmpty(dniVeterinario) ? null : dniVeterinario };
            if (informe.DniVeterinario != null)
            {
                var vet = dal.ObtenerVeterinarios().AsEnumerable().FirstOrDefault(r => r["dni"].ToString() == dniVeterinario);
                if (vet == null) throw new BECustomException("ERR_INFORME_VETERINARIO");
                informe.NombreVeterinario = vet["NombreCompleto"].ToString();
            }
            DateTime inicio = new DateTime(anio, mes, 1), fin = inicio.AddMonths(1);
            for (int numero = 1; numero <= 12; numero++) informe.Meses.Add(new BEInformeMes { Mes = numero });
            foreach (DataRow dato in dal.ObtenerDatos().Rows)
            {
                DateTime fecha = DateTime.ParseExact(Encriptacion.DesencriptarAES(dato["fechaHora"].ToString()), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                string dni = dato["DNI_Veterinario"].ToString();
                if (fecha.Year != anio || (informe.DniVeterinario != null && dni != informe.DniVeterinario)) continue;
                string estado = Encriptacion.DesencriptarAES(dato["estado"].ToString());
                if (estado == BEConsulta.Atendida) informe.Meses[fecha.Month - 1].Atendidos++;
                if (fecha < inicio || fecha >= fin) continue;
                var fila = informe.Detalle.Find(r => r.Dni == dni);
                if (fila == null)
                {
                    fila = new BEInformeVeterinario { Dni = dni, Veterinario = Encriptacion.DesencriptarAES(dato["nombre"].ToString()) + " " + Encriptacion.DesencriptarAES(dato["apellido"].ToString()) };
                    informe.Detalle.Add(fila);
                }
                fila.Turnos++;
                switch (estado)
                {
                    case BEConsulta.Agendada: fila.Pendientes++; break;
                    case BEConsulta.Atendida: fila.Atendidos++; break;
                    case BEConsulta.Cancelada: fila.Cancelados++; break;
                    case BEConsulta.NoSePresento: fila.Ausentes++; break;
                    default: throw new BECustomException("ERR_INFORME_ESTADO");
                }
            }
            int id = dal.Guardar(informe, SessionManager.Instancia.Usuario.DNI);
            digitos.InicializarTablaCompleta("InformeConsultas");
            digitos.InicializarTablaCompleta("InformeConsultasDetalle");
            digitos.InicializarTablaCompleta("InformeConsultasMes");
            return id;
        }
    }
}
