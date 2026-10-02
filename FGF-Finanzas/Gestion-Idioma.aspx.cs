using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FGF_Finanzas
{
    public partial class Gestion_Idioma : System.Web.UI.Page
    {
        BLLTraduccion bllTraduccion = new BLLTraduccion();
        DataTable _traduccionesCargadas;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarIdiomas();
                CargarFormularios();
                CargarGrillaIdiomas();
                CargarGrillaTraducciones();
            }
        }

        private void CargarIdiomas()
        {
            string seleccionado = ddlIdioma.SelectedValue;

            ddlIdioma.DataSource = bllTraduccion.ObtenerIdiomas();
            ddlIdioma.DataTextField = "Nombre";
            ddlIdioma.DataValueField = "Codigo";
            ddlIdioma.DataBind();

            if (ddlIdioma.Items.FindByValue(seleccionado) != null)
                ddlIdioma.SelectedValue = seleccionado;
            else if (ddlIdioma.Items.FindByValue(BLLTraduccion.IDIOMA_POR_DEFECTO) != null)
                ddlIdioma.SelectedValue = BLLTraduccion.IDIOMA_POR_DEFECTO;
        }

        private void CargarFormularios()
        {
            string seleccionado = ddlFormulario.SelectedValue;

            ddlFormulario.DataSource = bllTraduccion.ObtenerFormularios();
            ddlFormulario.DataBind();

            if (ddlFormulario.Items.FindByValue(seleccionado) != null)
                ddlFormulario.SelectedValue = seleccionado;
        }

        private void CargarGrillaTraducciones()
        {
            _traduccionesCargadas = null;

            if (ddlIdioma.Items.Count == 0 || ddlFormulario.Items.Count == 0)
            {
                dgvTraducciones.DataSource = null;
                dgvTraducciones.DataBind();
                lblResumenTraduccion.Text = string.Empty;
                return;
            }

            try
            {
                DataTable etiquetas = bllTraduccion.ObtenerEtiquetasConTraduccion(ddlFormulario.SelectedValue, ddlIdioma.SelectedValue);
                _traduccionesCargadas = etiquetas;

                dgvTraducciones.DataSource = etiquetas;
                dgvTraducciones.DataBind();

                int sinTraducir = etiquetas.Rows.Cast<DataRow>().Count(r => string.IsNullOrEmpty(r["Texto"] as string));
                lblResumenTraduccion.Text = $"Editando <strong>{HttpUtility.HtmlEncode(ddlIdioma.SelectedItem.Text)}</strong> ({HttpUtility.HtmlEncode(ddlIdioma.SelectedValue)}) en <strong>{HttpUtility.HtmlEncode(ddlFormulario.SelectedValue)}</strong> &mdash; {etiquetas.Rows.Count} etiqueta(s), {sinTraducir} sin traducir.";
            }
            catch (Exception ex)
            {
                dgvTraducciones.DataSource = null;
                dgvTraducciones.DataBind();
                lblResumenTraduccion.Text = string.Empty;
                MostrarAlerta(ex.Message, "error");
            }
        }

        private void CargarGrillaIdiomas()
        {
            try
            {
                dgvIdiomas.DataSource = bllTraduccion.ObtenerIdiomas();
                dgvIdiomas.DataBind();
            }
            catch (Exception ex)
            {
                dgvIdiomas.DataSource = null;
                dgvIdiomas.DataBind();
                MostrarAlerta(ex.Message, "error");
            }
        }

        private DataTable ObtenerTraduccionesActuales()
        {
            if (_traduccionesCargadas != null)
                return _traduccionesCargadas;

            if (ddlIdioma.Items.Count == 0 || ddlFormulario.Items.Count == 0)
                return null;

            _traduccionesCargadas = bllTraduccion.ObtenerEtiquetasConTraduccion(ddlFormulario.SelectedValue, ddlIdioma.SelectedValue);
            return _traduccionesCargadas;
        }

        protected void ddlIdioma_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarGrillaTraducciones();
        }

        protected void ddlFormulario_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarGrillaTraducciones();
        }

        protected void dgvTraducciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;

            var boton = e.Row.FindControl("btnGuardarTraduccion") as LinkButton;
            if (boton != null)
                boton.CommandArgument = e.Row.RowIndex.ToString();
        }

        protected void dgvTraducciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "GuardarTraduccion")
                return;

            try
            {
                int indice = Convert.ToInt32(e.CommandArgument);
                DataTable traducciones = ObtenerTraduccionesActuales();
                if (traducciones == null || indice < 0 || indice >= traducciones.Rows.Count)
                    throw new Exception("No se pudo identificar la etiqueta a actualizar.");

                int idEtiqueta = Convert.ToInt32(traducciones.Rows[indice]["IdEtiqueta"]);
                string controlId = traducciones.Rows[indice]["ControlId"].ToString();
                string texto = ((TextBox)dgvTraducciones.Rows[indice].FindControl("txtTexto")).Text;

                bllTraduccion.ActualizarTraduccion(ddlFormulario.SelectedValue, ddlIdioma.SelectedValue, idEtiqueta, texto);

                MostrarAlerta($"Traducción de '{controlId}' guardada para '{ddlIdioma.SelectedValue}'.");
                CargarGrillaTraducciones();
                CargarGrillaIdiomas();
            }
            catch (Exception ex)
            {
                MostrarAlerta(ex.Message, "error");
            }
        }

        protected void btnCrearIdioma_Click(object sender, EventArgs e)
        {
            try
            {
                BEIdioma idioma = bllTraduccion.CrearIdioma(txtCodigoIdioma.Text, txtNombreIdioma.Text);

                txtCodigoIdioma.Text = string.Empty;
                txtNombreIdioma.Text = string.Empty;

                CargarIdiomas();
                CargarGrillaIdiomas();

                ddlIdioma.SelectedValue = idioma.Codigo;
                CargarGrillaTraducciones();

                MostrarAlerta($"Idioma '{idioma.Nombre}' creado con las traducciones por defecto.");
            }
            catch (Exception ex)
            {
                MostrarAlerta(ex.Message, "error");
            }
        }

        protected void dgvIdiomas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "EliminarIdioma")
                return;

            string codigo = Convert.ToString(e.CommandArgument);
            string seleccionado = ddlIdioma.SelectedValue;

            try
            {
                bllTraduccion.EliminarIdioma(codigo);

                CargarIdiomas();
                CargarGrillaIdiomas();

                if (ddlIdioma.Items.FindByValue(seleccionado) != null)
                    ddlIdioma.SelectedValue = seleccionado;

                CargarGrillaTraducciones();

                MostrarAlerta($"Idioma '{codigo}' eliminado correctamente.");
            }
            catch (Exception ex)
            {
                MostrarAlerta(ex.Message, "error");
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            litIdiomaDefecto.Text = BLLTraduccion.IDIOMA_POR_DEFECTO;
        }

        private void MostrarAlerta(string mensaje, string tipo = "exito")
        {
            string mensajeFormateado = mensaje.Replace("'", "\\'");
            string script = $"mostrarAlerta('{mensajeFormateado}', '{tipo}');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), script, true);
        }
    }
}
