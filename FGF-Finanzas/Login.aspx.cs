using FGF_Finanzas.Capas.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FGF_Finanzas
{
    public partial class Login : System.Web.UI.Page
    {
        BLLUsuario bllUsuario = new BLLUsuario(); BLLDigitoVerificador bllDigitoVerificador = new BLLDigitoVerificador();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void LogIn(object sender, EventArgs e)
        {
            try
            {
                HttpCookie usuarioCookie = null;
                bllUsuario.IniciarSesion(UserName.Text, Password.Text);
                if (RememberMe.Checked)
                {
                    usuarioCookie = new HttpCookie("UserSessionFGF");
                    usuarioCookie.Value = UserName.Text;
                    usuarioCookie.Expires = DateTime.Now.AddDays(30);

                    usuarioCookie.HttpOnly = true;

                    Response.Cookies.Add(usuarioCookie);
                }
                
                Response.Redirect("~/");
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
    }
}