using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.BLL;
using FGF_Finanzas.Capas.Servicios;
using FGF_Finanzas.Capas.Servicios.Cambio_Idioma;
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
    public partial class Default : BasePage
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
            if(SessionManager.IsLogged())
            {
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

        protected void Page_PreRender(object sender, EventArgs e)
        {
            string saludo = TextoTraducido("lblBienvenida", "¡Bienvenido!");
            if (SessionManager.IsLogged() && SessionManager.Instancia.Usuario != null)
            {
                saludo += $" {SessionManager.Instancia.Usuario.Usuario}";
            }
            lblBienvenida.Text = saludo;
        }

        private string TextoTraducido(string clave, string porDefecto)
        {
            string texto = IdiomaManager.Instancia.ObtenerTexto(NombreFormulario, clave);
            return texto == "[" + clave + "]" ? porDefecto : texto;
        }
    }
}