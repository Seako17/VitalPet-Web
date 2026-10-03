using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.BLL;
using FGF_Finanzas.Capas.Servicios;
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
    public partial class Consulta : System.Web.UI.Page
    {
        BLLMascota bllMascota = new BLLMascota();
        BLLConsulta bllConsulta = new BLLConsulta();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionManager.IsLogged())
            {
                Response.Redirect("~/Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (!IsPostBack)
            {
                CargarMascotas();
                CargarHorarios();
                txtFecha.Attributes["min"] = DateTime.Today.ToString("yyyy-MM-dd");
                ReiniciarVeterinarios("Elija fecha y hora");
            }
        }

        protected void btnAgendar_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            lblOk.Text = "";

            DateTime fechaHora;
            int idMascota;
            if (!TryObtenerFechaHora(out fechaHora))
            {
                lblError.Text = "Elija una fecha y una hora.";
                return;
            }
            if (!int.TryParse(dropMascota.SelectedValue, out idMascota))
            {
                lblError.Text = "Seleccione una mascota.";
                return;
            }
            if (string.IsNullOrEmpty(ddlVeterinario.SelectedValue))
            {
                lblError.Text = "Seleccione un veterinario/a.";
                return;
            }

            try
            {
                var veterinario = new BEUsuario { DNI = ddlVeterinario.SelectedValue };
                bllConsulta.Agendar(new BEConsulta(idMascota, veterinario, txtMotivo.Text.Trim(), fechaHora));

                string script = "alert('Consulta agendada correctamente.'); window.location.href = 'Default.aspx';";
                ScriptManager.RegisterStartupScript(this, GetType(), "ConsultaAgendada", script, true);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                ActualizarDisponibilidadSinBorrarMensaje();
            }
        }

        private void CargarMascotas()
        {
            DataTable dt = bllMascota.ObtenerMascotasDeUsuario(SessionManager.Instancia.Usuario);

            dropMascota.DataSource = dt;
            dropMascota.DataTextField = "nombre";
            dropMascota.DataValueField = dt.Columns[0].ColumnName;
            dropMascota.DataBind();

            if (dt.Rows.Count == 0)
            {
                lblError.Text = "No hay mascotas registradas.";
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

        private void ReiniciarVeterinarios(string textoGuia)
        {
            ddlVeterinario.Items.Clear();
            ddlVeterinario.Items.Add(new ListItem(textoGuia, ""));
            ddlVeterinario.Enabled = false;
        }

        protected void Horario_Changed(object sender, EventArgs e)
        {
            ActualizarDisponibilidad();
        }

        private void ActualizarDisponibilidadSinBorrarMensaje()
        {
            string mensaje = lblError.Text;
            ActualizarDisponibilidad();
            if (string.IsNullOrEmpty(lblError.Text)) lblError.Text = mensaje;
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
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
            lblOk.Text = "";

            DateTime fechaHora;
            int idMascota;
            if (!TryObtenerFechaHora(out fechaHora) || !int.TryParse(dropMascota.SelectedValue, out idMascota))
            {
                ReiniciarVeterinarios("Elija fecha y hora");
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
                ddlVeterinario.Items.Insert(0, new ListItem("Seleccione un veterinario/a", ""));
                ddlVeterinario.Enabled = true;
                return true;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                ReiniciarVeterinarios("Horario no disponible");
                return false;
            }
        }
    }
}