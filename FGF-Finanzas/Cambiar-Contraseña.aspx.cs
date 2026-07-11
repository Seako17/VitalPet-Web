using FGF_Finanzas.Capas.BLL;
using FGF_Finanzas.Capas.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FGF_Finanzas
{
    public partial class Cambiar_Contraseña : System.Web.UI.Page
    {
        BLLUsuario bllUsuario = new BLLUsuario();
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            if (!SessionManager.IsLogged())
            {
                Response.Redirect("Default.aspx?ReturnUrl=Cambiar-Contraseña.aspx");
            }
        }

        protected void btnCambiar_Click(object sender, EventArgs e)
        {
            try
            {
                string contraseña = txtContraseñaActual.Text;
                string nuevaContraseña = txtNuevaContraseña.Text;
                string confirmarContraseña = txtConfirmarContraseña.Text;

                if (string.IsNullOrWhiteSpace(contraseña) || string.IsNullOrWhiteSpace(nuevaContraseña) || string.IsNullOrWhiteSpace(confirmarContraseña)) throw new Exception("Debe completar todos los campos.");
                if (nuevaContraseña != confirmarContraseña) throw new Exception("La contraseña nueva no coincide con la contraseña de confirmación.");
                bllUsuario.CambiarContraseña(contraseña, nuevaContraseña);

                txtContraseñaActual.Text = "";
                txtNuevaContraseña.Text = "";
                txtConfirmarContraseña.Text = "";
                txtContraseñaActual.Attributes.Remove("value");
                txtNuevaContraseña.Attributes.Remove("value");
                txtConfirmarContraseña.Attributes.Remove("value");

                MostrarAlerta("La contraseña fue cambiada exitosamente.");
            }
            catch (Exception ex)
            {
                MostrarAlerta(ex.Message, "error");
                txtContraseñaActual.Attributes.Add("value", txtContraseñaActual.Text);
                txtNuevaContraseña.Attributes.Add("value", txtNuevaContraseña.Text);
                txtConfirmarContraseña.Attributes.Add("value", txtConfirmarContraseña.Text);
            }
        }

        private void MostrarAlerta(string mensaje, string tipo = "exito")
        {
            string mensajeFormateado = mensaje.Replace("'", "\\'");
            string script = $"mostrarAlerta('{mensajeFormateado}', '{tipo}');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), script, true);
        }

    }
}