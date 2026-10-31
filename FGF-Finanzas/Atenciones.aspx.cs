using FGF_Finanzas.Capas.BLL;
using FGF_Finanzas.Capas.Servicios;
using System;
using System.Web.UI;
using System.Web;

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
            }
        }

        private void CargarConsultas()
        {
            bool atendidas = ddlEstado.SelectedValue == "Atendida";
            bool pendientes = ddlEstado.SelectedValue == "Agendada";
            bool ausentes = ddlEstado.SelectedValue == "NoSePresento";
            lblSubtitulo.Text = atendidas ? "Consultas atendidas" : pendientes ? "Consultas sin atender" : "No se presentaron";
            btnAtender.Text = atendidas ? "Corregir atención" : "Atender";
            btnAtender.Visible = pendientes || atendidas || ausentes;
            btnVerAtencion.Visible = atendidas;
            btnNoSePresento.Visible = pendientes;
            gvConsultas.EmptyDataText = "No hay consultas para el estado seleccionado.";
            gvConsultas.SelectedIndex = -1;
            try
            {
                gvConsultas.DataSource = bllAtencion.ObtenerConsultasPorEstado(ddlEstado.SelectedValue);
                gvConsultas.DataBind();
            }
            catch (Exception ex) { MostrarFallo(ex); }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            bool atendidas = ddlEstado.SelectedValue == "Atendida";
            lblTitulo.Text = TextoTraducido("lblTitulo", "Atenciones");
            lblEstado.Text = TextoTraducido("lblEstado", "Mostrar:");
            lblSubtitulo.Text = TextoTraducido(atendidas ? "subtituloAtendidas" : ddlEstado.SelectedValue == "Agendada" ? "subtituloPendientes" : "subtituloAusentes", lblSubtitulo.Text);
            btnAtender.Text = TextoTraducido(atendidas ? "btnCorregir" : "btnAtender", atendidas ? "Corregir atención" : "Atender");
            btnVerAtencion.Text = TextoTraducido("btnVerAtencion", "Ver atención");
            btnNoSePresento.Text = TextoTraducido("btnNoSePresento", "No se presentó");
            btnNoSePresento.OnClientClick = "return confirm('" + HttpUtility.JavaScriptStringEncode(TextoTraducido("confirmarAusencia", "¿Confirmás que no se presentó al turno seleccionado?")) + "');";
            TraducirItem(ddlEstado, "Agendada", "filtroPendientes", "Pendientes");
            TraducirItem(ddlEstado, "Atendida", "filtroAtendidas", "Atendidas");
            TraducirItem(ddlEstado, "NoSePresento", "filtroNoSePresento", "No se presentó");
            string[] nombres = { "N°", "Mascota", "Veterinario/a", "Motivo", "Fecha y hora" };
            for (int i = 0; i < nombres.Length; i++) TraducirColumna(gvConsultas, i, "columna" + i, nombres[i]);
            TraducirBotonSeleccion(gvConsultas, 5, "seleccionar", "Seleccionar");
            TraducirSinDatos(gvConsultas, "sinConsultas", "No hay consultas para el estado seleccionado.");
        }

        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            LimpiarMensajeVisible(lblMensaje);
            CargarConsultas();
        }

        protected void gvConsultas_SelectedIndexChanged(object sender, EventArgs e)
        {
            LimpiarMensajeVisible(lblMensaje);
        }

        protected void btnAtender_Click(object sender, EventArgs e)
        {
            AbrirAtencion(false);
        }

        protected void btnVerAtencion_Click(object sender, EventArgs e)
        {
            AbrirAtencion(true);
        }

        protected void btnNoSePresento_Click(object sender, EventArgs e)
        {
            if (gvConsultas.SelectedIndex < 0)
            {
                MostrarMensajeVisible(lblMensaje, "seleccioneConsulta");
                return;
            }
            try
            {
                int idConsulta = Convert.ToInt32(gvConsultas.DataKeys[gvConsultas.SelectedIndex].Value);
                bllAtencion.MarcarNoSePresento(idConsulta);
                CargarConsultas();
                MostrarMensajeVisible(lblMensaje, "ausenciaGuardada");
            }
            catch (Exception ex) { MostrarFallo(ex); }
        }

        private void MostrarFallo(Exception ex)
        {
            MostrarErrorVisible(lblMensaje, ex);
        }

        private void AbrirAtencion(bool soloLectura)
        {
            if (gvConsultas.SelectedIndex < 0)
            {
                MostrarMensajeVisible(lblMensaje, "seleccioneConsulta");
                return;
            }

            int idConsulta = Convert.ToInt32(gvConsultas.DataKeys[gvConsultas.SelectedIndex].Value);

            Session["IdConsultaSeleccionada"] = idConsulta;

            Response.Redirect(soloLectura ? "AtenderConsulta.aspx?ver=1" : "AtenderConsulta.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
