using System;
using System.Web;
using System.Web.UI.WebControls;
using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.BLL;
using FGF_Finanzas.Capas.Servicios;

namespace FGF_Finanzas
{
    public partial class MisConsultas : PaginaSegura
    {
        private readonly BLLConsulta consultas = new BLLConsulta();
        protected override Permiso PermisoRequerido => CodigosPermiso.ObtenerPermisos().Find(p => p.Nombre == "MascotaRegistrar");
        private int? Seleccionada
        {
            get { return ViewState["ConsultaSeleccionada"] as int?; }
            set { ViewState["ConsultaSeleccionada"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionManager.IsLogged() || !TieneAcceso(SessionManager.Instancia.Usuario)) return;
            if (!IsPostBack) CargarGrilla();
        }

        private void CargarGrilla()
        {
            try { grilla.DataSource = consultas.ObtenerMisConsultas(ddlEstado.SelectedValue); grilla.DataBind(); }
            catch (Exception ex) { MostrarFallo(ex); }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!SessionManager.IsLogged() || !TieneAcceso(SessionManager.Instancia.Usuario)) return;
            CargarGrilla();
            foreach (string estado in new[] { "", BEConsulta.Agendada, BEConsulta.Atendida, BEConsulta.Cancelada })
                TraducirItem(ddlEstado, estado, "filtro" + estado, estado == "" ? "Todas" : estado == BEConsulta.Agendada ? "Agendadas" : estado == BEConsulta.Atendida ? "Atendidas" : "Canceladas");
            var nombres = new[] { "Mascota", "Fecha y hora", "Veterinario/a", "Motivo", "Estado", "Acciones" };
            for (int i = 0; i < nombres.Length; i++) TraducirColumna(grilla, i, "columna" + i, nombres[i]);
            TraducirSinDatos(grilla, "sinConsultas", "No hay consultas para el estado seleccionado.");
            pnlDetalle.Visible = false;
            if (Seleccionada.HasValue)
            {
                try
                {
                    var consulta = consultas.ObtenerDetallePropio(Seleccionada.Value);
                    if (consulta.estado != BEConsulta.Atendida) { Seleccionada = null; return; }
                    pnlDetalle.Visible = true;
                    litResumen.Text = consulta.nombreMascota + " — " + consulta.fechaHora.ToString("dd/MM/yyyy HH:mm") + " — " + consulta.nombreVeterinario;
                    litDiagnostico.Text = string.IsNullOrWhiteSpace(consulta.diagnostico)
                        ? TextoTraducido("sinDiagnostico", "Sin diagnóstico registrado") : consulta.diagnostico;
                }
                catch (Exception ex) { Seleccionada = null; MostrarFallo(ex); }
            }
        }

        protected void EstadoChanged(object sender, EventArgs e) { Seleccionada = null; LimpiarMensajeVisible(lblMensaje); }
        protected void CerrarDetalle(object sender, EventArgs e) { Seleccionada = null; }

        protected void ConsultaBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;
            var consulta = (BEConsulta)e.Row.DataItem;
            ((Literal)e.Row.FindControl("litEstado")).Text = BLLConsulta.PendienteDeRegistro(consulta, DateTime.Now)
                ? TextoTraducido("estadoPendienteRegistro", "Pendiente de registro")
                : TextoTraducido("estado" + consulta.estado, consulta.estado);
            var detalle = (LinkButton)e.Row.FindControl("btnDetalle");
            detalle.Visible = consulta.estado == BEConsulta.Atendida;
            detalle.Text = TextoTraducido("btnDetalle", "Ver detalle");
            var cancelar = (LinkButton)e.Row.FindControl("btnCancelarConsulta");
            cancelar.Visible = BLLConsulta.PuedeCancelar(consulta, DateTime.Now);
            cancelar.Text = TextoTraducido("btnCancelarConsulta", "Cancelar cita");
            cancelar.OnClientClick = "return confirm('" + HttpUtility.JavaScriptStringEncode(TextoTraducido("confirmarCancelacion", "¿Querés cancelar esta cita?")) + "');";
        }

        protected void ConsultaCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Detalle" && e.CommandName != "CancelarConsulta") return;
            int id;
            if (!int.TryParse(e.CommandArgument.ToString(), out id)) return;
            try
            {
                if (e.CommandName == "Detalle")
                {
                    var consulta = consultas.ObtenerDetallePropio(id);
                    Seleccionada = consulta.estado == BEConsulta.Atendida ? (int?)id : null;
                }
                else
                {
                    consultas.CancelarPropia(id);
                    Seleccionada = null;
                    MostrarMensajeVisible(lblMensaje, "cancelacionExitosa");
                }
            }
            catch (Exception ex) { MostrarFallo(ex); }
        }

        private void MostrarFallo(Exception ex)
        {
            LimpiarMensajeVisible(lblMensaje);
            string codigo = (ex as BECustomException)?.CodigoError ?? "errorCarga";
            lblMensaje.Text = HttpUtility.HtmlEncode(
                TextoTraducido(codigo, "No se pudo completar la operación."));
        }
    }
}
