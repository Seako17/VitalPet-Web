using FGF_Finanzas.Capas.BE;
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
    public partial class Cambiar_Contraseña : PaginaSegura
    {
        public override string NombreFormulario => "Cambiar-Contraseña.aspx";

        protected override Permiso PermisoRequerido
        {
            get { return CodigosPermiso.ObtenerPermisos().Find(x => x.Nombre == "ContrasenaCambiar"); }
        }
        BLLUsuario bllUsuario = new BLLUsuario();
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!SessionManager.IsLogged())
            {
                Response.Redirect(
                    "~/Default.aspx?ReturnUrl=" +
                    Server.UrlEncode(Request.RawUrl)
                );

                return;
            }

        }
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

                if (string.IsNullOrWhiteSpace(contraseña) || string.IsNullOrWhiteSpace(nuevaContraseña) || string.IsNullOrWhiteSpace(confirmarContraseña)) throw new BECustomException("ERR_CAMPOS_OBLIGATORIOS");
                if (nuevaContraseña != confirmarContraseña) throw new BECustomException("ERR_CONFIRMACION_NO_COINCIDE");
                bllUsuario.CambiarContraseña(contraseña, nuevaContraseña);

                txtContraseñaActual.Text = "";
                txtNuevaContraseña.Text = "";
                txtConfirmarContraseña.Text = "";
                txtContraseñaActual.Attributes.Remove("value");
                txtNuevaContraseña.Attributes.Remove("value");
                txtConfirmarContraseña.Attributes.Remove("value");

                MostrarAlerta(ObtenerMensaje("MSG_CONTRASENA_CAMBIADA"));
            }
            catch (Exception ex)
            {
                MostrarAlerta(TraducirError(ex), "error");
                txtContraseñaActual.Attributes.Add("value", txtContraseñaActual.Text);
                txtNuevaContraseña.Attributes.Add("value", txtNuevaContraseña.Text);
                txtConfirmarContraseña.Attributes.Add("value", txtConfirmarContraseña.Text);
            }
        }

    }
}