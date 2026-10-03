using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.BLL;
using FGF_Finanzas.Capas.Servicios.Cambio_Idioma;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
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

        internal static void AplicarTraducciones(ControlCollection controls, IDictionary<string, string> traducciones)
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

        protected string TextoTraducido(string clave, string porDefecto)
        {
            string texto = IdiomaManager.Instancia.ObtenerTexto(NombreFormulario, clave);
            return texto == "[" + clave + "]" ? porDefecto : texto;
        }

        protected void TraducirColumna(GridView grilla, int columna, string clave, string porDefecto)
        {
            string texto = TextoTraducido(clave, porDefecto);
            grilla.Columns[columna].HeaderText = texto;

            if (grilla.HeaderRow != null && columna < grilla.HeaderRow.Cells.Count)
                grilla.HeaderRow.Cells[columna].Text = texto;
        }

        protected void TraducirSinDatos(GridView grilla, string clave, string porDefecto)
        {
            string texto = TextoTraducido(clave, porDefecto);
            grilla.EmptyDataText = texto;

            GridViewRow filaVacia = BuscarFilaVacia(grilla);
            if (filaVacia != null && filaVacia.Cells.Count > 0)
                filaVacia.Cells[0].Text = texto;
        }

        protected void TraducirBotonFila(GridView grilla, string idBoton, string clave, string porDefecto)
        {
            string texto = TextoTraducido(clave, porDefecto);

            foreach (GridViewRow fila in grilla.Rows)
            {
                if (fila.RowType != DataControlRowType.DataRow)
                    continue;

                LinkButton boton = fila.FindControl(idBoton) as LinkButton;
                if (boton != null)
                    boton.Text = texto;
            }
        }

        protected void TraducirBotonSeleccion(GridView grilla, int columna, string clave, string porDefecto)
        {
            string texto = TextoTraducido(clave, porDefecto);

            if (grilla.Columns[columna] is CommandField campo)
                campo.SelectText = texto;

            foreach (GridViewRow fila in grilla.Rows)
            {
                if (fila.RowType != DataControlRowType.DataRow || columna >= fila.Cells.Count)
                    continue;

                LinkButton boton = fila.Cells[columna].Controls.OfType<LinkButton>()
                    .FirstOrDefault(b => b.CommandName == "Select");

                if (boton != null)
                    boton.Text = texto;
            }
        }

        protected void TraducirItem(ListControl control, string valor, string clave, string porDefecto)
        {
            ListItem item = control.Items.FindByValue(valor);
            if (item != null)
                item.Text = TextoTraducido(clave, porDefecto);
        }

        protected void TraducirOpciones(HtmlSelect control, string[] claves, string[] textosPorDefecto)
        {
            for (int i = 0; i < control.Items.Count && i < claves.Length; i++)
            {
                control.Items[i].Text = TextoTraducido(claves[i], textosPorDefecto[i]);
            }
        }

        private GridViewRow BuscarFilaVacia(Control contenedor)
        {
            foreach (Control hijo in contenedor.Controls)
            {
                if (hijo is GridViewRow fila && fila.RowType == DataControlRowType.EmptyDataRow)
                    return fila;

                GridViewRow encontrada = BuscarFilaVacia(hijo);
                if (encontrada != null)
                    return encontrada;
            }
            return null;
        }

        public string ObtenerError(string codigoError, params object[] argumentos)
        {
            return FormatearMensaje(new[] { "Errores" }, codigoError, argumentos);
        }

        public string ObtenerMensaje(string codigoMensaje, params object[] argumentos)
        {
            return FormatearMensaje(new[] { NombreFormulario, "Mensajes" }, codigoMensaje, argumentos);
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

        private string FormatearMensaje(string[] formularios, string clave, object[] argumentos)
        {
            foreach (string formulario in formularios)
            {
                string texto = IdiomaManager.Instancia.ObtenerTexto(formulario, clave);

                if (texto == "[" + clave + "]")
                    texto = IdiomaManager.Instancia.ObtenerTexto(formulario, clave, BLLTraduccion.IDIOMA_POR_DEFECTO);

                if (texto != "[" + clave + "]")
                {
                    return argumentos != null && argumentos.Length > 0
                        ? string.Format(texto, argumentos)
                        : texto;
                }
            }

            return clave;
        }

        protected void MostrarAlerta(string mensaje, string tipo = "exito")
        {
            string mensajeFormateado = mensaje.Replace("'", "\\'");
            string script = $"mostrarAlerta('{mensajeFormateado}', '{tipo}');";
            ScriptManager.RegisterStartupScript(this, GetType(), Guid.NewGuid().ToString(), script, true);
        }
    }
}