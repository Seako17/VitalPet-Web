using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.Servicios;
using FGF_Finanzas.Capas.BLL;
using FGF_Finanzas.Capas.Servicios.Cambio_Idioma;
using System;
using System.Globalization;
using System.Data;
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
        private readonly BLLConsulta consultas = new BLLConsulta();
        private string ClaveGuiaVeterinario
        {
            get { return (string)ViewState["ClaveGuiaVeterinario"] ?? "lblElijaFechaHora"; }
            set { ViewState["ClaveGuiaVeterinario"] = value; }
        }

        private void MostrarError(Exception ex)
        {
            var error = ex as BECustomException;
            ViewState["CodigoErrorConsulta"] = error?.CodigoError;
            ViewState["ArgumentosErrorConsulta"] = error?.Argumentos;
            lblError.Text = error?.CodigoError == "ERR_CONSULTA_SERVICIO"
                ? TextoDisponibilidad("ERR_CONSULTA_SERVICIO", "El servicio de consultas no está disponible. Intentá nuevamente en unos momentos.", "The appointment service is unavailable. Please try again shortly.")
                : TraducirError(ex);
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            MostrarDisponibilidadDia();
            MostrarVeterinariosHorario();
            lblAviso.Text = TextoTraducido("lblAviso", "La mascota ya tiene consultas agendadas");
            lblConfirmarAviso.Text = TextoTraducido("lblConfirmarAviso", "¿Querés agendar igualmente una nueva consulta?");
            btnVolver.Text = TextoTraducido("btnVolver", "Volver al formulario");
            btnConfirmar.Text = TextoTraducido("btnConfirmar", "Agendar igualmente");
            TraducirColumna(grillaPendientes, 0, "pendientesFecha", "Fecha y hora");
            TraducirColumna(grillaPendientes, 1, "pendientesVeterinario", "Veterinario/a");
            TraducirColumna(grillaPendientes, 2, "pendientesMotivo", "Motivo");
            TraducirItem(ddlHora, "", "ddlHora_Seleccione", "Hora");
            string textoGuia = ClaveGuiaVeterinario == "ddlVeterinario_Seleccione"
                ? "Seleccione un veterinario/a"
                : ClaveGuiaVeterinario == "lblHorarioNoDisponible" ? "Horario no disponible" : "Elija fecha y hora";
            TraducirItem(ddlVeterinario, "", ClaveGuiaVeterinario, textoGuia);
            string codigo = ViewState["CodigoErrorConsulta"] as string;
            if (!string.IsNullOrEmpty(codigo))
                lblError.Text = codigo == "ERR_CONSULTA_SERVICIO"
                    ? TextoDisponibilidad(codigo, "El servicio de consultas no está disponible. Intentá nuevamente en unos momentos.", "The appointment service is unavailable. Please try again shortly.")
                    : ObtenerError(codigo, ViewState["ArgumentosErrorConsulta"] as object[]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    CargarMascotas();
                    CargarHorarios();
                    txtFecha.Attributes["min"] = DateTime.Today.ToString("yyyy-MM-dd");
                    ReiniciarVeterinarios("lblElijaFechaHora");
                    txtMotivo.Attributes["placeholder"] = TextoTraducido("txtMotivo", "Ej: Vacunación");
                }
                catch (Exception ex) { btnAgendar.Enabled = false; MostrarError(ex); }
            }
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
                    var pendientes = consultas.ObtenerProximasConsultas(idMascota);
                    if (pendientes.Rows.Count > 0)
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
                consultas.Agendar(new BEConsulta(idMascota, new BEUsuario { DNI = ddlVeterinario.SelectedValue }, txtMotivo.Text.Trim(), fechaHora));

                string mensaje = HttpUtility.JavaScriptStringEncode(ObtenerMensaje("MSG_CONSULTA_AGENDADA"));
                string script = "alert('" + mensaje + "'); window.location.href = 'MisConsultas.aspx';";
                ScriptManager.RegisterStartupScript(this, GetType(), "ConsultaAgendada", script, true);
            }
            catch (Exception ex)
            {
                OcultarAviso();
                ActualizarDisponibilidadSinBorrarMensaje();
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
            var mascotas = consultas.ObtenerMascotas();

            dropMascota.DataSource = mascotas;
            dropMascota.DataTextField = "Nombre";
            dropMascota.DataValueField = "IdMascota";
            dropMascota.DataBind();

            if (mascotas.Rows.Count == 0)
            {
                MostrarError(new BECustomException("ERR_CONSULTA_SIN_MASCOTAS"));
                btnAgendar.Enabled = false;
            }
        }

        private void CargarHorarios()
        {
            ddlHora.Enabled = false;
            ddlHora.Items.Clear();
            ddlHora.Items.Add(new ListItem("Hora", ""));

            foreach (string hora in consultas.ObtenerHorarios())
            {
                ddlHora.Items.Add(new ListItem(hora, hora));
            }
        }

        private void ReiniciarVeterinarios(string clave)
        {
            ViewState.Remove("VeterinariosHorario");
            ClaveGuiaVeterinario = clave;
            ddlVeterinario.Items.Clear();
            ddlVeterinario.Items.Add(new ListItem(TextoTraducido(clave, "Elija fecha y hora"), ""));
            ddlVeterinario.Enabled = false;
        }

        protected void Horario_Changed(object sender, EventArgs e)
        {
            ActualizarDisponibilidad();
        }

        private string TextoDisponibilidad(string clave, string castellano, string ingles)
        {
            return TextoTraducido(clave, IdiomaManager.Instancia.IdiomaActual.StartsWith("en", StringComparison.OrdinalIgnoreCase) ? ingles : castellano);
        }

        private bool CargarDisponibilidadDia()
        {
            DateTime fecha;
            int idMascota;
            ViewState.Remove("DisponibilidadDia");
            ViewState.Remove("ErrorDisponibilidadDia");
            if (!DateTime.TryParseExact(txtFecha.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out fecha) ||
                !int.TryParse(dropMascota.SelectedValue, out idMascota))
            {
                ddlHora.ClearSelection();
                ddlHora.Enabled = false;
                return false;
            }
            try
            {
                ViewState["DisponibilidadDia"] = consultas.ObtenerDisponibilidadDia(idMascota, fecha);
                MostrarDisponibilidadDia();
                return true;
            }
            catch
            {
                ViewState["ErrorDisponibilidadDia"] = true;
                ddlHora.ClearSelection();
                ddlHora.Enabled = false;
                throw;
            }
        }

        private void MostrarDisponibilidadDia()
        {
            DataTable disponibilidad = ViewState["DisponibilidadDia"] as DataTable;
            if (disponibilidad == null)
            {
                lblDisponibilidadDia.Text = Equals(ViewState["ErrorDisponibilidadDia"], true)
                    ? TextoDisponibilidad("errorDisponibilidadDia", "No se pudo consultar la disponibilidad. Intentá nuevamente.", "Availability could not be loaded. Please try again.")
                    : TextoDisponibilidad("elegirFechaDisponibilidad", "Elegí una fecha para ver los turnos disponibles.", "Choose a date to see available appointments.");
                return;
            }
            string seleccionada = ddlHora.SelectedValue;
            ddlHora.Items.Clear();
            ddlHora.Items.Add(new ListItem(TextoDisponibilidad("ddlHora_Seleccione", "Hora", "Time"), ""));
            int turnos = 0;
            int horarios = 0;
            foreach (DataRow fila in disponibilidad.Rows)
            {
                int cupos = (int)fila["Cupos"];
                if (cupos > 0)
                {
                    turnos += cupos;
                    horarios++;
                }
                ListItem item = new ListItem(fila["Hora"].ToString(), fila["Hora"].ToString());
                // Enabled=false hace que Web Forms omita el option; usamos disabled para que siga visible.
                if (cupos == 0)
                {
                    item.Attributes["disabled"] = "disabled";
                    item.Attributes["style"] = "color:#757575;background-color:#eeeeee";
                }
                ddlHora.Items.Add(item);
                if (cupos > 0 && item.Value == seleccionada) item.Selected = true;
            }
            // Un turno equivale a un cupo de un veterinario; una hora puede tener varios.
            lblDisponibilidadDia.Text = string.Format(TextoDisponibilidad("resumenDisponibilidadDia",
                "Turnos disponibles para esta mascota: {0}, en {1} horarios. Los horarios en gris no se pueden elegir.",
                "Available appointments for this pet: {0}, across {1} time slots. Greyed out times cannot be selected."), turnos, horarios);
            ddlHora.Enabled = true;
            if (string.IsNullOrEmpty(ddlHora.SelectedValue)) ReiniciarVeterinarios("lblElijaFechaHora");
        }

        private void ActualizarDisponibilidadSinBorrarMensaje()
        {
            string mensaje = lblError.Text;
            string veterinarioSeleccionado = ddlVeterinario.SelectedValue;
            ActualizarDisponibilidad();
            ListItem veterinario = ddlVeterinario.Items.FindByValue(veterinarioSeleccionado);
            if (veterinario != null && veterinario.Attributes["disabled"] == null)
            {
                ddlVeterinario.ClearSelection();
                veterinario.Selected = true;
            }
            if (string.IsNullOrEmpty(lblError.Text)) lblError.Text = mensaje;
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

        private bool ActualizarDisponibilidad()
        {
            lblError.Text = "";
            ViewState.Remove("CodigoErrorConsulta");
            lblOk.Text = "";

            if (dropMascota.Items.Count == 0)
            {
                MostrarError(new BECustomException("ERR_CONSULTA_SIN_MASCOTAS"));
                return false;
            }

            try
            {
                if (!CargarDisponibilidadDia())
                {
                    ReiniciarVeterinarios("lblElijaFechaHora");
                    return false;
                }
                DateTime fechaHora;
                int idMascota;
                if (!TryObtenerFechaHora(out fechaHora) || !int.TryParse(dropMascota.SelectedValue, out idMascota))
                {
                    ReiniciarVeterinarios("lblElijaFechaHora");
                    return false;
                }
                var veterinarios = consultas.ObtenerVeterinariosDelHorario(idMascota, fechaHora);
                CargarVeterinarios(veterinarios);
                ddlVeterinario.Enabled = true;
                ClaveGuiaVeterinario = "ddlVeterinario_Seleccione";
                return true;
            }
            catch (Exception ex)
            {
                MostrarError(ex);
                ReiniciarVeterinarios("lblHorarioNoDisponible");
                return false;
            }
        }

        private void CargarVeterinarios(DataTable veterinarios)
        {
            ViewState["VeterinariosHorario"] = veterinarios;
            MostrarVeterinariosHorario();
        }

        private void MostrarVeterinariosHorario()
        {
            DataTable veterinarios = ViewState["VeterinariosHorario"] as DataTable;
            if (veterinarios == null) return;
            string seleccionada = ddlVeterinario.SelectedValue;
            ddlVeterinario.Items.Clear();
            ddlVeterinario.Items.Add(new ListItem(TextoTraducido("ddlVeterinario_Seleccione", "Seleccione un veterinario/a"), ""));
            foreach (DataRow fila in veterinarios.Rows)
            {
                ListItem item = new ListItem(fila["NombreCompleto"].ToString(), fila["Dni"].ToString());
                if (!(bool)fila["Disponible"])
                {
                    item.Attributes["disabled"] = "disabled";
                    item.Attributes["style"] = "color:#757575;background-color:#eeeeee";
                }
                ddlVeterinario.Items.Add(item);
                if ((bool)fila["Disponible"] && item.Value == seleccionada) item.Selected = true;
            }
        }
        #endregion
    }
}
