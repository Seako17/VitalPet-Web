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
        private readonly TraduccionService _traduccionService = new TraduccionService();
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
                            btn.Text = texto; break;
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
                if(c.HasControls())
                {
                    AplicarTraducciones(c.Controls, traducciones);
                }
            }
        }

        public string ObtenerError(string codigoError)
        {
            string idioma = IdiomaManager.Instancia.IdiomaActual;
            var errores = _traduccionService.ObtenerTraducciones("Errores", idioma);

            if (errores.TryGetValue(codigoError, out string mensaje))
                return mensaje;

            return $"[{codigoError}]";
        }
    }
}