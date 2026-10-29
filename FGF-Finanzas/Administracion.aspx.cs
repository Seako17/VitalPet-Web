using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.Servicios;
using FGF_Finanzas.Capas.Servicios.Cambio_Idioma;
using System;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace FGF_Finanzas
{
    public partial class Administracion : PaginaSegura
    {
        protected override bool TieneAcceso(BEUsuario usuario)
        {
            return MenuAdministracion.TieneAcceso(usuario?.Rol);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            // Reutiliza los textos existentes y los actualiza despues del cambio de idioma.
            string tituloPanel = Texto("Gestion-Usuarios.aspx", "lblTituloPanel", "Panel de Administrador");
            lblTituloPanel.Text = HttpUtility.HtmlEncode(tituloPanel);
            Title = tituloPanel;
            lblTituloMenu.Text = HttpUtility.HtmlEncode(Texto("Gestion-Usuarios.aspx", "lblTituloMenu", "Menú"));
            menuSecciones.Controls.Clear();
            var permitidas = MenuAdministracion.ObtenerSeccionesPermitidas(SessionManager.Instancia?.Usuario?.Rol);
            foreach (var seccion in MenuAdministracion.Secciones)
            {
                var item = new HtmlGenericControl("li");
                string texto = Texto("Gestion-Usuarios.aspx", seccion.ClaveTexto, seccion.Texto);
                if (permitidas.Any(s => s.Permiso == seccion.Permiso))
                {
                    item.Controls.Add(new HyperLink
                    {
                        ID = "acceso" + seccion.Permiso,
                        NavigateUrl = "~/" + seccion.Pagina,
                        Text = HttpUtility.HtmlEncode(texto)
                    });
                }
                else
                {
                    item.Attributes["class"] = "opcion-sin-permiso";
                    item.Attributes["aria-disabled"] = "true";
                    item.InnerText = texto;
                }
                menuSecciones.Controls.Add(item);
            }
        }

        private string Texto(string formulario, string clave, string porDefecto)
        {
            string texto = IdiomaManager.Instancia.ObtenerTexto(formulario, clave);
            return string.IsNullOrWhiteSpace(texto) || texto == "[" + clave + "]" ? porDefecto : texto;
        }
    }
}
