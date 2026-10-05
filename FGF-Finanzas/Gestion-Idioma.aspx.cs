using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.BLL;
using FGF_Finanzas.Capas.Servicios;
using FGF_Finanzas.Capas.Servicios.Cambio_Idioma;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FGF_Finanzas
{
    public partial class Gestion_Idioma : PaginaSegura
    {
        protected override Permiso PermisoRequerido
        {
            get { return CodigosPermiso.ObtenerPermisos().Find(x => x.Nombre == "IdiomaGestionar"); }
        }
        BLLTraduccion bllTraduccion = new BLLTraduccion();
        DataTable _traduccionesCargadas;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarIdiomas();
                CargarFormularios();

                string idioma = Request.QueryString["idioma"];
                if (!string.IsNullOrEmpty(idioma) && ddlIdioma.Items.FindByValue(idioma) != null)
                    ddlIdioma.SelectedValue = idioma;

                string formulario = Request.QueryString["formulario"];
                if (!string.IsNullOrEmpty(formulario) && ddlFormulario.Items.FindByValue(formulario) != null)
                    ddlFormulario.SelectedValue = formulario;

                CargarGrillaIdiomas();
                CargarGrillaTraducciones();

                MostrarAlertaPendiente();
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
                lblResumenTraduccion.Text = ObtenerMensaje("MSG_RESUMEN_TRADUCCION",
                    HttpUtility.HtmlEncode(ddlIdioma.SelectedItem.Text),
                    HttpUtility.HtmlEncode(ddlIdioma.SelectedValue),
                    HttpUtility.HtmlEncode(ddlFormulario.SelectedValue),
                    etiquetas.Rows.Count,
                    sinTraducir);
            }
            catch (Exception ex)
            {
                dgvTraducciones.DataSource = null;
                dgvTraducciones.DataBind();
                lblResumenTraduccion.Text = string.Empty;
                MostrarAlerta(TraducirError(ex), "error");
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
                MostrarAlerta(TraducirError(ex), "error");
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
                    throw new BECustomException("ERR_ETIQUETA_NO_IDENTIFICADA");

                int idEtiqueta = Convert.ToInt32(traducciones.Rows[indice]["IdEtiqueta"]);
                string controlId = traducciones.Rows[indice]["ControlId"].ToString();
                string texto = ((TextBox)dgvTraducciones.Rows[indice].FindControl("txtTexto")).Text;

                bllTraduccion.ActualizarTraduccion(ddlFormulario.SelectedValue, ddlIdioma.SelectedValue, idEtiqueta, texto);

                RedirigirConAlerta(ObtenerMensaje("MSG_TRADUCCION_GUARDADA", controlId, ddlIdioma.SelectedValue));
                return;
            }
            catch (Exception ex)
            {
                MostrarAlerta(TraducirError(ex), "error");
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
                ddlIdioma.SelectedValue = idioma.Codigo;

                RedirigirConAlerta(ObtenerMensaje("MSG_IDIOMA_CREADO", idioma.Nombre));
                return;
            }
            catch (Exception ex)
            {
                MostrarAlerta(TraducirError(ex), "error");
            }
        }

        protected void dgvIdiomas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "EliminarIdioma")
                return;

            string codigo = Convert.ToString(e.CommandArgument);

            try
            {
                bllTraduccion.EliminarIdioma(codigo);

                if (codigo.Equals(IdiomaManager.Instancia.IdiomaActual, StringComparison.OrdinalIgnoreCase))
                {
                    IdiomaManager.Instancia.IdiomaActual = BLLTraduccion.IDIOMA_POR_DEFECTO;

                    if (SessionManager.IsLogged())
                        SessionManager.Instancia.Usuario.Idioma = BLLTraduccion.IDIOMA_POR_DEFECTO;
                }

                RedirigirConAlerta(ObtenerMensaje("MSG_IDIOMA_ELIMINADO", codigo));
                return;
            }
            catch (Exception ex)
            {
                MostrarAlerta(TraducirError(ex), "error");
            }
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            TraducirColumna(dgvTraducciones, 0, "dgvTraducciones_Header_Control", "Control");
            TraducirColumna(dgvTraducciones, 1, "dgvTraducciones_Header_TextoTraducido", "Texto traducido");
            TraducirColumna(dgvTraducciones, 2, "dgvTraducciones_Header_Acciones", "Acciones");
            TraducirSinDatos(dgvTraducciones, "dgvTraducciones_Vacio", "No hay etiquetas para mostrar.");
            TraducirBotonFila(dgvTraducciones, "btnGuardarTraduccion", "btnGuardarTraduccion", "Guardar");

            TraducirColumna(dgvIdiomas, 0, "dgvIdiomas_Header_Codigo", "Código");
            TraducirColumna(dgvIdiomas, 1, "dgvIdiomas_Header_Nombre", "Nombre");
            TraducirColumna(dgvIdiomas, 2, "dgvIdiomas_Header_Acciones", "Acciones");
            TraducirSinDatos(dgvIdiomas, "dgvIdiomas_Vacio", "No hay idiomas registrados.");
            TraducirBotonFila(dgvIdiomas, "btnEliminarIdioma", "btnEliminarIdioma", "Eliminar");
        }

        private void MostrarAlertaPendiente()
        {
            if (Session["GestionIdioma_Alerta"] is string[] alerta)
            {
                Session.Remove("GestionIdioma_Alerta");
                MostrarAlerta(alerta[0], alerta[1]);
            }
        }

        private void RedirigirConAlerta(string mensaje, string tipo = "exito")
        {
            Session["GestionIdioma_Alerta"] = new[] { mensaje, tipo };
            string url = $"~/{NombreFormulario}?idioma={HttpUtility.UrlEncode(ddlIdioma.SelectedValue)}&formulario={HttpUtility.UrlEncode(ddlFormulario.SelectedValue)}";
            Response.Redirect(url, false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
