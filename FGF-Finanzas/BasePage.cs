using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.BLL;
using FGF_Finanzas.Capas.Servicios.Cambio_Idioma;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FGF_Finanzas
{
    public abstract class BasePage : Page, IIdiomaObserver
    {
        public virtual string NombreFormulario => Path.GetFileName(Request.AppRelativeCurrentExecutionFilePath);

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            IdiomaManager.Instancia.Suscribir(this);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!IsPostBack)
            {
                IdiomaManager.Instancia.Notificar();
            }
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            IdiomaManager.Instancia.Desuscribir(this);
        }

        public void ActualizarIdioma(string codigoIdioma, IDictionary<string, string> traducciones)
        {
            AplicarTraducciones(this.Controls, traducciones);
        }

        private void AplicarTraducciones(ControlCollection controls, IDictionary<string, string> traducciones)
        {
            foreach (Control c in controls)
            {
                if (!string.IsNullOrEmpty(c.ID) && traducciones.TryGetValue(c.ID, out string texto))
                {
                    switch (c)
                    {
                        case Button btn:
                            btn.Text = texto;
                            break;
                        case BaseValidator val:
                            val.ErrorMessage = texto;
                            break;
                        case Label lbl:
                            lbl.Text = texto;
                            break;
                        case LinkButton lbtn:
                            lbtn.Text = texto;
                            break;
                        case HyperLink hl:
                            hl.Text = texto;
                            break;
                        case CheckBox cb:
                            cb.Text = texto;
                            break;
                        case IButtonControl ibtn:
                            ibtn.Text = texto;
                            break;
                        case TextBox tb:
                            tb.Attributes["placeholder"] = texto;
                            break;
                    }
                }
                if (c.HasControls())
                {
                    AplicarTraducciones(c.Controls, traducciones);
                }
            }
        }

        public string ObtenerError(string codigoError, params object[] argumentos)
        {
            return FormatearMensaje("Errores", codigoError, argumentos);
        }

        public string ObtenerMensaje(string codigoMensaje, params object[] argumentos)
        {
            return FormatearMensaje("Mensajes", codigoMensaje, argumentos);
        }

        public string ObtenerError(BECustomException excepcion)
        {
            return ObtenerError(excepcion.CodigoError, excepcion.Argumentos);
        }

        protected string TraducirError(Exception ex, string codigoTecnico = null)
        {
            if (ex is BECustomException bex)
                return ObtenerError(bex);

            if (!string.IsNullOrWhiteSpace(codigoTecnico))
                return ObtenerError(codigoTecnico, ex.Message);

            return string.IsNullOrWhiteSpace(ex.Message)
                ? ObtenerError("ERR_INESPERADO")
                : ex.Message;
        }

        private string FormatearMensaje(string formulario, string clave, object[] argumentos)
        {
            string texto = IdiomaManager.Instancia.ObtenerTexto(formulario, clave);

            if (texto == "[" + clave + "]")
                texto = IdiomaManager.Instancia.ObtenerTexto(formulario, clave, BLLTraduccion.IDIOMA_POR_DEFECTO);

            if (texto == "[" + clave + "]")
                texto = clave;

            return argumentos != null && argumentos.Length > 0
                ? string.Format(texto, argumentos)
                : texto;
        }

        protected void MostrarAlerta(string mensaje, string tipo = "exito")
        {
            string mensajeFormateado = mensaje.Replace("'", "\\'");
            string script = $"mostrarAlerta('{mensajeFormateado}', '{tipo}');";
            ScriptManager.RegisterStartupScript(this, GetType(), Guid.NewGuid().ToString(), script, true);
        }
    }
}