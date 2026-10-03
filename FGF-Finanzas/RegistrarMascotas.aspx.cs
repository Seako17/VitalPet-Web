using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.BLL;
using FGF_Finanzas.Capas.DAL;
using FGF_Finanzas.Capas.Servicios;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FGF_Finanzas
{
    public partial class RegistrarMascotas : BasePage
    {
        BLLMascota bllMascota = new BLLMascota();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            try
            {
                var nombre = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtNombre.Text.ToLower()).Trim();
                if (Convert.ToDateTime(txtFechaNacimiento.Value) > DateTime.Now)
                {
                    throw new BECustomException("ERR_FECHA_NACIMIENTO");
                }

                BEMascota mascota = new BEMascota(
                    SessionManager.Instancia.Usuario,
                    nombre,
                    txtEspecie.Text.Trim(),
                    txtRaza.Text.Trim(),
                    Convert.ToDateTime(txtFechaNacimiento.Value)
                );

                bllMascota.AgregarMascota(mascota);
                string mensaje = ObtenerMensaje("MSG_MASCOTA_REGISTRADA").Replace("'", "\\'");
                string script = "alert('" + mensaje + "'); window.location.href = 'Default.aspx';";
                ScriptManager.RegisterStartupScript(this, GetType(), "MascotaRegistrada", script, true);
            }
            catch (Exception ex)
            {
                lblError.Text = TraducirError(ex);
            }
        }
    }
}