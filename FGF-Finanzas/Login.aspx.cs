using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.BLL;
using FGF_Finanzas.Capas.Servicios.Cambio_Idioma;
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
    public partial class Login : BasePage
    {
        BLLUsuario bllUsuario = new BLLUsuario(); BLLDigitoVerificador bllDigitoVerificador = new BLLDigitoVerificador();
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            string aviso = Session["AvisoLoginMarketplace"] as string;
            if (!string.IsNullOrWhiteSpace(aviso))
            {
                lblError.Text = Server.HtmlEncode(aviso);
                Session.Remove("AvisoLoginMarketplace");
            }
        }

        protected void LogIn(object sender, EventArgs e)
        {
            LimpiarMensajeVisible(lblError);
            try
            {
                HttpCookie usuarioCookie = null;
                bllUsuario.IniciarSesion(UserName.Text, Password.Text);
                if (chkRememberMe.Checked)
                {
                    usuarioCookie = new HttpCookie("UserSessionFGF");
                    usuarioCookie.Value = UserName.Text;
                    usuarioCookie.Expires = DateTime.Now.AddDays(30);

                    usuarioCookie.HttpOnly = true;

                    Response.Cookies.Add(usuarioCookie);
                }
                
                string returnUrl = Request.QueryString["ReturnUrl"];
                bool destinoLocal = !string.IsNullOrWhiteSpace(returnUrl) &&
                    ((returnUrl.StartsWith("~/", StringComparison.Ordinal) &&
                      returnUrl.IndexOf('\\') < 0 && !returnUrl.StartsWith("~//", StringComparison.Ordinal)) ||
                     (returnUrl.StartsWith("/", StringComparison.Ordinal) &&
                      (returnUrl.Length == 1 || (returnUrl[1] != '/' && returnUrl[1] != '\\'))));
                Response.Redirect(destinoLocal ? returnUrl : "~/Default.aspx");
            }
            catch (BECustomException bex)
            {
                MostrarErrorVisible(lblError, bex);
            }
        }
    }
}
