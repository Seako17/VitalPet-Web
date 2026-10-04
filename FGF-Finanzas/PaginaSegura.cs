using FGF_Finanzas.Capas.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace FGF_Finanzas
{
    public abstract class PaginaSegura : Page
    {
        protected virtual Permiso PermisoRequerido { get; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!SessionManager.IsLogged())
            {
                Response.Redirect("~/Default.aspx?ReturnUrl=" + Server.UrlEncode(Request.RawUrl)
                );

                return;
            }

            var usuario = SessionManager.Instancia.Usuario;

            if (usuario == null || !usuario.Rol.TienePermiso(PermisoRequerido))
            {
                Response.Redirect("~/Default.aspx?ReturnUrl=" + Server.UrlEncode(Request.RawUrl));
                return;
            }
        }
    }
}