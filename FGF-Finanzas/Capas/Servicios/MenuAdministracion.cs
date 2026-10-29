using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace FGF_Finanzas.Capas.Servicios
{
    public sealed class SeccionAdministracion
    {
        public string Pagina { get; }
        public string Permiso { get; }
        public string ClaveTexto { get; }
        public string Texto { get; }

        internal SeccionAdministracion(string pagina, string permiso, string claveTexto, string texto)
        {
            Pagina = pagina;
            Permiso = permiso;
            ClaveTexto = claveTexto;
            Texto = texto;
        }
    }

    public static class MenuAdministracion
    {
        public static IReadOnlyList<SeccionAdministracion> Secciones { get; } = Array.AsReadOnly(new[]
        {
            new SeccionAdministracion("Gestion-Usuarios.aspx", "UsuarioGestionar", "lblMenuUsuarios", "Gestión Usuarios"),
            new SeccionAdministracion("BitacoraEventos.aspx", "BitacoraVer", "lnkMenuBitacora", "Bitácora eventos"),
            new SeccionAdministracion("Gestion-Perfiles.aspx", "PerfilGestionar", "lblMenuPerfiles", "Gestión Perfiles"),
            new SeccionAdministracion("Gestion-Familias.aspx", "FamiliaGestionar", "lblMenuFamilias", "Gestión Familias"),
            new SeccionAdministracion("Backup-Restore.aspx", "BackUpRestore", "lnkMenuBackup", "Backup/Restore"),
            new SeccionAdministracion("Gestion-Idioma.aspx", "IdiomaGestionar", "lnkMenuIdiomas", "Gestión Idiomas")
        });

        public static IList<SeccionAdministracion> ObtenerSeccionesPermitidas(Rol rol)
        {
            if (rol?.Componentes == null) return new List<SeccionAdministracion>();
            // Los IDs se resuelven con los permisos actuales; no dependen del nombre del rol.
            const string clave = "MenuAdministracion.Permisos";
            var contexto = HttpContext.Current;
            var permisos = contexto?.Items[clave] as List<Permiso>;
            if (permisos == null)
            {
                permisos = CodigosPermiso.ObtenerPermisos();
                if (contexto != null) contexto.Items[clave] = permisos;
            }
            return Secciones.Where(s => permisos.Any(p => p.Nombre == s.Permiso && rol.TienePermiso(p))).ToList();
        }

        public static bool TieneAcceso(Rol rol)
        {
            return ObtenerSeccionesPermitidas(rol).Count > 0;
        }

        public static void ActualizarEstadoMenu(ControlCollection controles, Rol rol)
        {
            ActualizarOpciones(controles, ObtenerSeccionesPermitidas(rol));
        }

        private static void ActualizarOpciones(ControlCollection controles, IList<SeccionAdministracion> permitidas)
        {
            foreach (Control control in controles)
            {
                var item = control as HtmlGenericControl;
                string codigo = item?.Attributes["data-permiso"];
                if (item != null && item.TagName.Equals("li", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(codigo))
                {
                    var seccion = Secciones.FirstOrDefault(s => s.Permiso == codigo);
                    bool habilitada = seccion != null && permitidas.Any(s => s.Permiso == codigo);
                    var clases = (item.Attributes["class"] ?? string.Empty).Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                        .Where(c => c != "opcion-sin-permiso").ToList();
                    if (!habilitada) clases.Add("opcion-sin-permiso");
                    item.Attributes["class"] = string.Join(" ", clases);
                    if (habilitada) item.Attributes.Remove("aria-disabled");
                    else item.Attributes["aria-disabled"] = "true";
                    ActualizarEnlaces(item.Controls, habilitada ? "~/" + seccion.Pagina : string.Empty);
                }
                if (control.HasControls()) ActualizarOpciones(control.Controls, permitidas);
            }
        }

        private static void ActualizarEnlaces(ControlCollection controles, string destino)
        {
            foreach (Control control in controles)
            {
                var enlace = control as HyperLink;
                if (enlace != null) enlace.NavigateUrl = destino;
                var anchor = control as HtmlAnchor;
                if (anchor != null)
                {
                    if (string.IsNullOrEmpty(destino)) anchor.Attributes.Remove("href");
                    else anchor.HRef = destino;
                }
                if (enlace != null || anchor != null)
                {
                    var atributos = enlace != null ? enlace.Attributes : anchor.Attributes;
                    if (string.IsNullOrEmpty(destino))
                    {
                        atributos["aria-disabled"] = "true";
                        atributos["tabindex"] = "-1";
                    }
                    else
                    {
                        atributos.Remove("aria-disabled");
                        atributos.Remove("tabindex");
                    }
                }
                if (control.HasControls()) ActualizarEnlaces(control.Controls, destino);
            }
        }
    }
}
