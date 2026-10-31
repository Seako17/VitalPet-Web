using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.BLL;
using FGF_Finanzas.Capas.Servicios;
using System;
using System.Data;
using System.Web.UI;
using System.Web;

namespace FGF_Finanzas
{
    public partial class AtenderConsulta : PaginaSegura
    {
        protected override Permiso PermisoRequerido
        {
            get { return CodigosPermiso.ObtenerPermisos().Find(x => x.Nombre == "ConsultaAtender"); }
        }

        BLLAtencion bllAtencion = new BLLAtencion();
        private bool SoloLectura => Request.QueryString["ver"] == "1";

        private int IdConsulta
        {
            get
            {
                int id;
                return int.TryParse(Session["IdConsultaSeleccionada"]?.ToString(), out id) ? id : 0;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (SoloLectura)
            {
                txtDiagnostico.ReadOnly = true;
                txtObservaciones.ReadOnly = true;
                btnGuardar.Visible = false;
                btnCancelar.Text = "Volver";
                lblTitulo.Text = "VER ATENCIÓN";
            }
            if (Session["IdConsultaSeleccionada"] == null)
            {
                Response.Redirect("Atenciones.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (!IsPostBack)
            {
                try { CargarAtencion(); }
                catch (Exception ex)
                {
                    MostrarFallo(ex);
                    btnGuardar.Enabled = false;
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            lblTitulo.Text = TextoTraducido(SoloLectura ? "tituloVer" : "lblTitulo", SoloLectura ? "VER ATENCIÓN" : "ATENDER CONSULTA");
            lblSubtitulo.Text = TextoTraducido("lblSubtitulo", "Datos de la atención");
            lblMascota.Text = TextoTraducido("lblMascota", "Mascota:");
            lblVeterinario.Text = TextoTraducido("lblVeterinario", "Veterinario/a:");
            lblMotivo.Text = TextoTraducido("lblMotivo", "Motivo:");
            lblFechaHora.Text = TextoTraducido("lblFechaHora", "Fecha y hora:");
            lblDiagnostico.Text = TextoTraducido("lblDiagnostico", "Diagnóstico:");
            lblObservaciones.Text = TextoTraducido("lblObservaciones", "Observaciones:");
            btnGuardar.Text = TextoTraducido("btnGuardar", "Guardar atención");
            btnCancelar.Text = TextoTraducido(SoloLectura ? "btnVolver" : "btnCancelar", SoloLectura ? "Volver" : "Cancelar");
        }

        private void CargarAtencion()
        {
            if (IdConsulta <= 0)
            {
                VolverALista();
                return;
            }

            DataRow consulta = bllAtencion.ObtenerConsultaPorId(IdConsulta);
            if (consulta == null)
            {
                MostrarError("ERR_ATENCION_CONSULTA_INVALIDA");
                btnGuardar.Enabled = false;
                return;
            }

            txtMascota.Text = consulta["mascota"].ToString();
            txtVeterinario.Text = consulta["veterinario"].ToString();
            txtMotivo.Text = consulta["motivo"].ToString();
            txtFechaHora.Text = Convert.ToDateTime(consulta["fechaHora"]).ToString("dd/MM/yyyy HH:mm");

            BEAtencion atencion = bllAtencion.ObtenerPorConsulta(IdConsulta);

            if (atencion != null)
            {
                ViewState["IdAtencion"] = atencion.id_atencion;
                txtDiagnostico.Text = atencion.diagnostico;
                txtObservaciones.Text = atencion.observaciones;
            }
            else
            {
                ViewState["IdAtencion"] = 0;
                if (SoloLectura) MostrarError("ERR_ATENCION_SIN_REGISTRO");
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (SoloLectura) return;
            LimpiarMensajeVisible(lblError);

            if (string.IsNullOrWhiteSpace(txtDiagnostico.Text))
            {
                MostrarError("ERR_ATENCION_DIAGNOSTICO_OBLIGATORIO");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtObservaciones.Text))
            {
                MostrarError("ERR_ATENCION_OBSERVACIONES_OBLIGATORIAS");
                return;
            }

            try
            {
                int idAtencion = Convert.ToInt32(ViewState["IdAtencion"] ?? 0);

                BEAtencion atencion = new BEAtencion
                {
                    id_atencion = idAtencion,
                    id_consulta = IdConsulta,
                    diagnostico = txtDiagnostico.Text,
                    observaciones = txtObservaciones.Text,
                    atendido = true
                };

                if (idAtencion > 0)
                {
                    bllAtencion.Actualizar(atencion);
                }
                else
                {
                    bllAtencion.Guardar(atencion);
                }

                Session.Remove("IdConsultaSeleccionada");

                ScriptManager.RegisterStartupScript(this, GetType(), "AtencionGuardada", "alert('" + HttpUtility.JavaScriptStringEncode(ObtenerMensaje("atencionGuardada")) + "'); " +
                    "window.location.href='Atenciones.aspx';", true);
            }
            catch (Exception ex)
            {
                MostrarFallo(ex);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Session.Remove("IdConsultaSeleccionada");
            VolverALista();
        }

        private void VolverALista()
        {
            Response.Redirect("Atenciones.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        private void MostrarFallo(Exception ex)
        {
            MostrarErrorVisible(lblError, ex);
        }

        private void MostrarError(string codigo)
        {
            MostrarErrorVisible(lblError, codigo);
        }
    }
}