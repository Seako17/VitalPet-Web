using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.BLL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FGF_Finanzas
{
    public partial class Registrarse : System.Web.UI.Page
    {
        BLLUsuario bllUsuario = new BLLUsuario();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void CreateUser_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            try
            {
                var nombre = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Nombre.Text.ToLower()).Trim();
                var apellido = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Apellido.Text.ToLower()).Trim();

                bllUsuario.ValidarUsuario(DniUsuario.Text, UserName.Text.Trim(), nombre, apellido, Password.Text, ConfirmPassword.Text);
                BEUsuario usuario = new BEUsuario(DniUsuario.Text, nombre, apellido, UserName.Text.Trim(), Password.Text, Email.Text, "Cliente");
                bllUsuario.AgregarUsuario(usuario);
                Response.Redirect("~/Login.aspx");
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
    }
}