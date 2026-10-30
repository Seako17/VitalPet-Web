using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.BLL;
using FGF_Finanzas.Capas.Servicios;
using FGF_Finanzas.Capas.Servicios.Cambio_Idioma;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FGF_Finanzas
{
    public partial class Consulta : PaginaSegura
    {
        protected override Permiso PermisoRequerido
        {
            get { return CodigosPermiso.ObtenerPermisos().Find(x => x.Nombre == "MascotaRegistrar"); }
        }
        public override string NombreFormulario => "Consulta.aspx";
        BLLMascota bllMascota = new BLLMascota();
        BLLConsulta bllConsulta = new BLLConsulta();

        private void MostrarError(Exception ex)
        {
            var error = ex as BECustomException;
            ViewState["CodigoErrorConsulta"] = error?.CodigoError;
            ViewState["ArgumentosErrorConsulta"] = error?.Argumentos;
            lblError.Text = TraducirError(ex);
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            lblAviso.Text = TextoTraducido("lblAviso", "La mascota ya tiene consultas agendadas");
            lblConfirmarAviso.Text = TextoTraducido("lblConfirmarAviso", "¿Querés agendar igualmente una nueva consulta?");
            btnVolver.Text = TextoTraducido("btnVolver", "Volver al formulario");
            btnConfirmar.Text = TextoTraducido("btnConfirmar", "Agendar igualmente");
            TraducirColumna(grillaPendientes, 0, "pendientesFecha", "Fecha y hora");
            TraducirColumna(grillaPendientes, 1, "pendientesVeterinario", "Veterinario/a");
            TraducirColumna(grillaPendientes, 2, "pendientesMotivo", "Motivo");
            TraducirItem(ddlHora, "", "ddlHora_Seleccione", "Hora");
            TraducirItem(ddlVeterinario, "", "ddlVeterinario_Seleccione", "Seleccione un veterinario/a");
            string codigo = ViewState["CodigoErrorConsulta"] as string;
            if (!string.IsNullOrEmpty(codigo))
                lblError.Text = ObtenerError(codigo, ViewState["ArgumentosErrorConsulta"] as object[]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarMascotas();
                CargarHorarios();
                CargarVeterinarios();
                txtFecha.Attributes["min"] = DateTime.Today.ToString("yyyy-MM-dd");
                txtMotivo.Attributes["placeholder"] = TextoTraducido("txtMotivo", "Ej: Vacunación");
            }

            ConfigurarDisponibilidad();
        }

        private void ConfigurarDisponibilidad()
        {
            pnlDisponibilidad.Attributes["data-url"] =
                ResolveUrl("~/WebServices/DisponibilidadService.asmx/ObtenerDisponibilidad");
            pnlDisponibilidad.Attributes["data-msg-cantidad"] =
                TextoTraducido("lblDisponibilidadCantidad", "Horarios libres: {0}");
            pnlDisponibilidad.Attributes["data-msg-primero"] =
                TextoTraducido("lblDisponibilidadPrimero", "Primer horario disponible: {0}");
            pnlDisponibilidad.Attributes["data-msg-sin"] =
                TextoTraducido("lblDisponibilidadSinHorarios", "Sin horarios disponibles");
            pnlDisponibilidad.Attributes["data-msg-consultando"] =
                TextoTraducido("lblDisponibilidadConsultando", "Consultando disponibilidad...");
            pnlDisponibilidad.Attributes["data-msg-sesion"] = ObtenerError("ERR_SESION");
            pnlDisponibilidad.Attributes["data-msg-error"] = ObtenerError("ERR_INESPERADO");
        }

        protected void btnAgendar_Click(object sender, EventArgs e)
        {
            AgendarConsulta(false);
        }

        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (pnlAviso.Visible) AgendarConsulta(true);
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            OcultarAviso();
        }

        private void OcultarAviso()
        {
            pnlAviso.Visible = false;
            pnlFormulario.Visible = true;
            ViewState.Remove("MascotaAvisada");
        }

        private void AgendarConsulta(bool confirmar)
        {
            lblError.Text = "";
            ViewState.Remove("CodigoErrorConsulta");
            lblOk.Text = "";

            DateTime fechaHora;
            int idMascota;
            if (!TryObtenerFechaHora(out fechaHora))
            {
                MostrarError(new BECustomException("ERR_CONSULTA_FECHA_HORA"));
                return;
            }
            if (!int.TryParse(dropMascota.SelectedValue, out idMascota))
            {
                MostrarError(new BECustomException("ERR_CONSULTA_MASCOTA"));
                return;
            }
            if (string.IsNullOrEmpty(ddlVeterinario.SelectedValue))
            {
                MostrarError(new BECustomException("ERR_CONSULTA_VETERINARIO"));
                return;
            }

            try
            {
                if (string.IsNullOrWhiteSpace(txtMotivo.Text))
                    throw new BECustomException("ERR_CONSULTA_MOTIVO");

                if (!confirmar || !Equals(ViewState["MascotaAvisada"], idMascota))
                {
                    var pendientes = bllConsulta.ObtenerProximasConsultas(idMascota);
                    if (pendientes.Count > 0)
                    {
                        grillaPendientes.DataSource = pendientes;
                        grillaPendientes.DataBind();
                        ViewState["MascotaAvisada"] = idMascota;
                        pnlAviso.Visible = true;
                        pnlFormulario.Visible = false;
                        return;
                    }
                }

                OcultarAviso();
                var veterinario = new BEUsuario { DNI = ddlVeterinario.SelectedValue };
                bllConsulta.Agendar(new BEConsulta(idMascota, veterinario, txtMotivo.Text.Trim(), fechaHora));

                string mensaje = HttpUtility.JavaScriptStringEncode(ObtenerMensaje("MSG_CONSULTA_AGENDADA"));
                string script = "alert('" + mensaje + "'); window.location.href = 'MisConsultas.aspx';";
                ScriptManager.RegisterStartupScript(this, GetType(), "ConsultaAgendada", script, true);
            }
            catch (Exception ex)
            {
                OcultarAviso();
                MostrarError(ex);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        #region Funciones
        private void CargarMascotas()
        {
            DataTable dt = bllMascota.ObtenerMascotasDeUsuario(SessionManager.Instancia.Usuario);

            dropMascota.DataSource = dt;
            dropMascota.DataTextField = "nombre";
            dropMascota.DataValueField = dt.Columns[0].ColumnName;
            dropMascota.DataBind();

            if (dt.Rows.Count == 0)
            {
                MostrarError(new BECustomException("ERR_CONSULTA_SIN_MASCOTAS"));
                btnAgendar.Enabled = false;
            }
        }

        private void CargarHorarios()
        {
            ddlHora.Items.Clear();
            ddlHora.Items.Add(new ListItem("Hora", ""));

            foreach (string hora in bllConsulta.ObtenerHorarios())
            {
                ddlHora.Items.Add(new ListItem(hora, hora));
            }
        }

        private void CargarVeterinarios()
        {
            ddlVeterinario.Items.Clear();
            ddlVeterinario.DataSource = bllConsulta.ObtenerVeterinarios();
            ddlVeterinario.DataTextField = "nombreCompleto";
            ddlVeterinario.DataValueField = "dni";
            ddlVeterinario.DataBind();
            ddlVeterinario.Items.Insert(0,
                new ListItem(TextoTraducido("ddlVeterinario_Seleccione", "Seleccione un veterinario/a"), ""));
        }

        private bool TryObtenerFechaHora(out DateTime fechaHora)
        {
            fechaHora = DateTime.MinValue;

            if (string.IsNullOrWhiteSpace(txtFecha.Text) || string.IsNullOrEmpty(ddlHora.SelectedValue))
                return false;

            DateTime fecha;
            if (!DateTime.TryParseExact(txtFecha.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                                        DateTimeStyles.None, out fecha))
                return false;

            fechaHora = fecha.Date + TimeSpan.Parse(ddlHora.SelectedValue);
            return true;
        }
        #endregion
    }
}
