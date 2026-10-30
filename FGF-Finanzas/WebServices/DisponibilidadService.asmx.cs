using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.BLL;
using FGF_Finanzas.Capas.Servicios;
using FGF_Finanzas.Capas.Servicios.Cambio_Idioma;
using System;
using System.Globalization;
using System.Web.Script.Services;
using System.Web.Services;

namespace FGF_Finanzas.WebServices
{
    [WebService(Namespace = "http://vitalpet.local/webservices/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [ScriptService]
    public class DisponibilidadService : WebService
    {
        private readonly BLLConsulta bllConsulta = new BLLConsulta();
        private const string FORMATO_FECHA = "yyyy-MM-dd";

        [WebMethod(EnableSession = true,
            Description = "Calcula los horarios libres de un veterinario para una mascota en una fecha.")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public BEDisponibilidad ObtenerDisponibilidad(int idMascota, string dniVeterinario, string fecha)
        {
            var respuesta = new BEDisponibilidad
            {
                IdMascota = idMascota,
                DniVeterinario = dniVeterinario,
                Fecha = fecha
            };

            try
            {
                if (!SessionManager.IsLogged() || SessionManager.Instancia?.Usuario == null)
                    throw new BECustomException("ERR_SESION");

                DateTime fechaConsulta;
                if (string.IsNullOrWhiteSpace(fecha) ||
                    !DateTime.TryParseExact(fecha, FORMATO_FECHA, CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out fechaConsulta))
                    throw new BECustomException("ERR_CONSULTA_FECHA_HORA");

                return bllConsulta.ObtenerDisponibilidad(idMascota, dniVeterinario, fechaConsulta);
            }
            catch (BECustomException ex)
            {
                respuesta.Ok = false;
                respuesta.CodigoError = ex.CodigoError;
                respuesta.Mensaje = TraducirError(ex);
                return respuesta;
            }
            catch (Exception)
            {
                var error = new BECustomException("ERR_INESPERADO");
                respuesta.Ok = false;
                respuesta.CodigoError = error.CodigoError;
                respuesta.Mensaje = TraducirError(error);
                return respuesta;
            }
        }

        private string TraducirError(BECustomException error)
        {
            string texto = IdiomaManager.Instancia.ObtenerTexto("Errores", error.CodigoError);

            if (texto == "[" + error.CodigoError + "]")
                texto = IdiomaManager.Instancia.ObtenerTexto("Errores", error.CodigoError,
                    BLLTraduccion.IDIOMA_POR_DEFECTO);

            if (texto == "[" + error.CodigoError + "]")
                return error.CodigoError;

            return error.Argumentos != null && error.Argumentos.Length > 0
                ? string.Format(texto, error.Argumentos)
                : texto;
        }
    }
}
