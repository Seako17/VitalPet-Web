using FGF_Finanzas.Capas.BLL;
using FGF_Finanzas.Capas.Servicios;
using System;
using System.Web.UI;

namespace FGF_Finanzas
{
    public partial class Atenciones : PaginaSegura
    {
        protected override Permiso PermisoRequerido
        {
            get { return CodigosPermiso.ObtenerPermisos().Find(x => x.Nombre == "ConsultaAtender"); }
        }

        BLLAtencion bllAtencion = new BLLAtencion();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarConsultas();

                if (Request.QueryString["guardado"] == "1")
                {
                    lblMensaje.Text = "La atención se guardó correctamente.";
                }
            }
        }

        private void CargarConsultas()
        {
            gvConsultas.DataSource = bllAtencion.ObtenerConsultasSinAtender();
            gvConsultas.DataBind();
        }

        protected void gvConsultas_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMensaje.CssClass = "lblOk";
            lblMensaje.Text = string.Empty;
        }

        protected void btnAtender_Click(object sender, EventArgs e)
        {
            if (gvConsultas.SelectedIndex < 0)
            {
                lblMensaje.CssClass = "lblError";
                lblMensaje.Text = "Seleccione una consulta de la grilla para atenderla.";
                return;
            }

            int idConsulta = Convert.ToInt32(gvConsultas.DataKeys[gvConsultas.SelectedIndex].Value);

            Response.Redirect("AtenderConsulta.aspx?idConsulta=" + idConsulta, false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
