using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.BLL;
using FGF_Finanzas.Capas.Servicios;
using System;
using System.Data;
using System.Web.UI;

namespace FGF_Finanzas
{
    public partial class AtenderConsulta : PaginaSegura
    {
        protected override Permiso PermisoRequerido
        {
            get { return CodigosPermiso.ObtenerPermisos().Find(x => x.Nombre == "ConsultaAtender"); }
        }

        BLLAtencion bllAtencion = new BLLAtencion();

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
            if (Session["IdConsultaSeleccionada"] == null)
            {
                Response.Redirect("Atenciones.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (!IsPostBack)
            {
                CargarAtencion();
            }
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
                MostrarError("La consulta seleccionada no existe.");
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
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            lblError.Text = string.Empty;

            if (string.IsNullOrWhiteSpace(txtDiagnostico.Text))
            {
                MostrarError("Ingrese el diagnóstico de la atención.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtObservaciones.Text))
            {
                MostrarError("Ingrese las observaciones de la atención.");
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

                ScriptManager.RegisterStartupScript(this, GetType(), "AtencionGuardada", "alert('La atención se guardó correctamente.'); " +
                    "window.location.href='Atenciones.aspx';", true);
            }
            catch (Exception ex)
            {
                MostrarError(ObtenerMensajeError(ex));
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

        private string ObtenerMensajeError(Exception ex)
        {
            BECustomException error = ex as BECustomException;

            if (error != null)
            {
                switch (error.CodigoError)
                {
                    case "ERR_ATENCION_NULA":
                        return "No se pudo cargar la atención.";
                    case "ERR_ATENCION_CONSULTA_INVALIDA":
                        return "La consulta seleccionada no es válida.";
                    case "ERR_ATENCION_DIAGNOSTICO_OBLIGATORIO":
                        return "Ingrese el diagnóstico de la atención.";
                    case "ERR_ATENCION_ID_INVALIDO":
                        return "La atención seleccionada no es válida.";
                }
            }

            return TraducirError(ex);
        }

        private void MostrarError(string mensaje)
        {
            lblError.Text = mensaje;
        }
    }
}
