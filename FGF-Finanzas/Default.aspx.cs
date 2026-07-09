using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.BLL;
using FGF_Finanzas.Capas.Servicios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FGF_Finanzas
{
    public partial class Default : System.Web.UI.Page
    {
        BLLUsuario bllUsuario = new BLLUsuario();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["ReturnUrl"] != null)
                {
                    string script = "alert('Acceso denegado: No tienes los permisos necesarios para ingresar a esta sección.');";
                    script += "window.history.replaceState({}, document.title, window.location.pathname);";
                    ScriptManager.RegisterStartupScript(this, GetType(), "AlertaAccesoDenegado", script, true);
                }
            }
            lblBienvenida.Text = "¡Bienvenido!";
            HttpCookie cookie = Request.Cookies["UserSessionFGF"];
            if(cookie != null)
            {
                BEUsuario usuario = new BEUsuario(bllUsuario.ObtenerUsuarios().AsEnumerable().Where(x=> x["usuario"].ToString() == cookie.Value.ToString()).FirstOrDefault());
                if(usuario != null)
                {
                    SessionManager.Login(usuario);
                }
            }
            if(SessionManager.IsLogged())
            {
                lblBienvenida.Text += $" {SessionManager.Instancia.Usuario.Usuario.ToString()}";
                BEUsuario usuario = SessionManager.Instancia.Usuario;
                string rolUsuario = "Cliente";
                if (usuario != null)
                {
                    rolUsuario = usuario.Rol;
                }
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    1,
                    usuario.Usuario,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(30),
                    true,
                    rolUsuario
                );

                string encryptedTicket = FormsAuthentication.Encrypt(ticket);
                HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                Response.Cookies.Add(authCookie);
            }
        }
    }
}