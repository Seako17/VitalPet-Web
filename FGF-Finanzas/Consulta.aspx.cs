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
            lblError.Text = TraducirError(ex);
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            TraducirItem(ddlHora, "", "ddlHora_Seleccione", "Hora");
            string textoGuia = ClaveGuiaVeterinario == "ddlVeterinario_Seleccione"
                ? "Seleccione un veterinario/a"
                : ClaveGuiaVeterinario == "lblHorarioNoDisponible" ? "Horario no disponible" : "Elija fecha y hora";
            TraducirItem(ddlVeterinario, "", ClaveGuiaVeterinario, textoGuia);
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
                txtFecha.Attributes["min"] = DateTime.Today.ToString("yyyy-MM-dd");
                ReiniciarVeterinarios("lblElijaFechaHora");
                txtMotivo.Attributes["placeholder"] = TextoTraducido("txtMotivo", "Ej: Vacunación");
            }
        }

        protected void btnAgendar_Click(object sender, EventArgs e)
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
                var veterinario = new BEUsuario { DNI = ddlVeterinario.SelectedValue };
                bllConsulta.Agendar(new BEConsulta(idMascota, veterinario, txtMotivo.Text.Trim(), fechaHora));

                string mensaje = HttpUtility.JavaScriptStringEncode(ObtenerMensaje("MSG_CONSULTA_AGENDADA"));
                string script = "alert('" + mensaje + "'); window.location.href = 'Default.aspx';";
                ScriptManager.RegisterStartupScript(this, GetType(), "ConsultaAgendada", script, true);
            }
            catch (Exception ex)
            {
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

        private void ReiniciarVeterinarios(string clave)
        {
            ClaveGuiaVeterinario = clave;
            ddlVeterinario.Items.Clear();
            ddlVeterinario.Items.Add(new ListItem(TextoTraducido(clave, "Elija fecha y hora"), ""));
            ddlVeterinario.Enabled = false;
        }

        protected void Horario_Changed(object sender, EventArgs e)
        {
            ActualizarDisponibilidad();
        }

        private void ActualizarDisponibilidadSinBorrarMensaje()
        {
            string mensaje = lblError.Text;
            string veterinarioSeleccionado = ddlVeterinario.SelectedValue;
            ActualizarDisponibilidad();
            ListItem veterinario = ddlVeterinario.Items.FindByValue(veterinarioSeleccionado);
            if (veterinario != null)
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

            DateTime fechaHora;
            int idMascota;
            if (!TryObtenerFechaHora(out fechaHora) || !int.TryParse(dropMascota.SelectedValue, out idMascota))
            {
                ReiniciarVeterinarios("lblElijaFechaHora");
                return false;
            }

            try
            {
                DataTable libres = bllConsulta.ObtenerVeterinariosDisponibles(idMascota, fechaHora);

                ddlVeterinario.Items.Clear();
                ddlVeterinario.DataSource = libres;
                ddlVeterinario.DataTextField = "nombreCompleto";
                ddlVeterinario.DataValueField = "DNI";
                ddlVeterinario.DataBind();
                ddlVeterinario.Items.Insert(0, new ListItem(TextoTraducido("ddlVeterinario_Seleccione", "Seleccione un veterinario/a"), ""));
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
        #endregion
    }
}
