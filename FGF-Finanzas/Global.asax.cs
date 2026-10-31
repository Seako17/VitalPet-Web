using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace FGF_Finanzas
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
        }
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            HttpCookie authCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie == null)
                return;

            FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

            if (authTicket == null)
                return;

            var identity = new System.Security.Principal.GenericIdentity(authTicket.Name,"Forms");

            var principal = new System.Security.Principal.GenericPrincipal(identity,new string[0]);
            Context.User = principal;
        }
    }
}