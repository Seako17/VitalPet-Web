using FGF_Finanzas.Capas.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace FGF_Finanzas
{
    public abstract class PaginaSegura : BasePage
    {
        protected virtual Permiso PermisoRequerido { get; }

        protected virtual bool TieneAcceso(FGF_Finanzas.Capas.BE.BEUsuario usuario)
        {
            var permiso = PermisoRequerido;
            return usuario?.Rol?.Componentes != null && permiso != null && usuario.Rol.TienePermiso(permiso);
        }

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

            if (!TieneAcceso(usuario))
            {
                Response.Redirect("~/Default.aspx?ReturnUrl=" + Server.UrlEncode(Request.RawUrl));
                return;
            }
        }
    }
}
