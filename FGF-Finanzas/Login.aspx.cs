using FGF_Finanzas.Capas.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FGF_Finanzas
{
    public partial class Login : System.Web.UI.Page
    {
        BLLUsuario bllUsuario = new BLLUsuario();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void LogIn(object sender, EventArgs e)
        {
            try
            {
                bllUsuario.IniciarSesion(UserName.Text, Password.Text);
                Response.Redirect("~/");
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
    }
}