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
    public partial class Gestion_Perfiles : PaginaSegura
    {
        protected override Permiso PermisoRequerido
        {
            get { return CodigosPermiso.ObtenerPermisos().Find(x => x.Nombre == "PerfilGestionar"); }
        }
        BLLEvento bllEvento = new BLLEvento();
        BLLPermiso bLLPermiso = new BLLPermiso();
        BLLFamilia BLLFamilia = new BLLFamilia();
        BLLRol bllRol = new BLLRol();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                lstPermisos.DataSource = bLLPermiso.ObtenerPermisos();
                lstPermisos.DataTextField = "Nombre";
                lstPermisos.DataValueField = "Id";
                lstPermisos.DataBind();
                CargarFamiliasEnListBox();
                CargarPerfilesEnListBox();
            }
        }
        private void CargarFamiliasEnListBox()
        {
            List<Familia> familias =
                BLLFamilia.ObtenerFamilias();

            lstFamilias.Items.Clear();

            foreach (Familia familia in familias)
            {
                lstFamilias.Items.Add(
                    new ListItem(
                        familia.Nombre,
                        familia.Id.ToString()
                    )
                );
            }
        }
        private void CargarPerfilesEnListBox()
        {
            List<Rol> perfiles =
                bllRol.ObtenerRoles();

            lstPerfiles.Items.Clear();

            foreach (Rol perfil in perfiles)
            {
                AgregarPerfilAlListBox(perfil);
            }
        }
        private void AgregarPerfilAlListBox(Rol perfil)
        {
            lstPerfiles.Items.Add(
                new ListItem(
                    perfil.Nombre,
                    $"R-{perfil.Id}"
                )
            );

            foreach (Componente componente in perfil.Componentes)
            {
                if (componente is Permiso permiso)
                {
                    lstPerfiles.Items.Add(
                        new ListItem(
                            "\u00A0\u00A0\u00A0\u00A0• " +
                            permiso.Nombre,

                            $"P-{permiso.Id}-ROL-{perfil.Id}"
                        )
                    );
                }
                else if (componente is Familia familia)
                {
                    AgregarFamiliaDePerfil(
                        familia,
                        perfil.Id,
                        1
                    );
                }
            }
        }
        private void AgregarFamiliaDePerfil(
    Familia familia,
    int idRol,
    int nivel)
        {
            string sangria =
                new string('\u00A0', nivel * 4);

            lstPerfiles.Items.Add(
                new ListItem(
                    sangria + "└─ " + familia.Nombre,
                    $"F-{familia.Id}-ROL-{idRol}"
                )
            );

            foreach (Componente componente in familia.Componentes)
            {
                if (componente is Permiso permiso)
                {
                    string sangriaPermiso =
                        new string(
                            '\u00A0',
                            (nivel + 1) * 4
                        );

                    // OJO:
                    // estos permisos son heredados de la familia,
                    // no se eliminan directamente desde el rol.
                    lstPerfiles.Items.Add(
                        new ListItem(
                            sangriaPermiso +
                            "• " +
                            permiso.Nombre,

                            $"PF-{permiso.Id}-FAMILIA-{familia.Id}-ROL-{idRol}"
                        )
                    );
                }
                else if (componente is Familia hija)
                {
                    AgregarFamiliaDePerfil(
                        hija,
                        idRol,
                        nivel + 1
                    );
                }
            }
        }


        protected void btnAsignarPermiso_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstPerfiles.SelectedItem == null)
                    throw new Exception(
                        "Debe seleccionar un perfil."
                    );

                if (lstPermisos.SelectedItem == null)
                    throw new Exception(
                        "Debe seleccionar un permiso."
                    );

                string valor =
                    lstPerfiles.SelectedValue;

                if (!valor.StartsWith("R-"))
                    throw new Exception(
                        "Debe seleccionar el perfil principal, no uno de sus componentes."
                    );

                string[] partes =
                    valor.Split('-');

                int idRol =
                    Convert.ToInt32(partes[1]);

                int idPermiso =
                    Convert.ToInt32(
                        lstPermisos.SelectedValue
                    );

                Rol rol = new Rol
                {
                    Id = idRol
                };

                Permiso permiso = new Permiso
                {
                    Id = idPermiso,
                    Nombre = lstPermisos.SelectedItem.Text
                };

                bllRol.AsignarPermiso(
                    rol,
                    permiso
                );

                CargarPerfilesEnListBox();

                MostrarMensaje(
                    "Permiso asignado correctamente.",
                    false
                );
            }
            catch (Exception ex)
            {
                MostrarMensaje(
                    ex.Message,
                    true
                );
            }
        }

        protected void btnAsignarFamilia_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstPerfiles.SelectedItem == null)
                    throw new Exception(
                        "Debe seleccionar un perfil."
                    );

                if (lstFamilias.SelectedItem == null)
                    throw new Exception(
                        "Debe seleccionar una familia."
                    );

                string valor =
                    lstPerfiles.SelectedValue;

                if (!valor.StartsWith("R-"))
                    throw new Exception(
                        "Debe seleccionar el perfil principal."
                    );

                string[] partes =
                    valor.Split('-');

                int idRol =
                    Convert.ToInt32(partes[1]);

                int idFamilia =
                    Convert.ToInt32(
                        lstFamilias.SelectedValue
                    );

                Rol rol = new Rol
                {
                    Id = idRol
                };

                Familia familia = new Familia
                {
                    Id = idFamilia
                };

                bllRol.AsignarFamilia(
                    rol,
                    familia
                );

                CargarPerfilesEnListBox();

                MostrarMensaje(
                    "Familia asignada correctamente.",
                    false
                );
            }
            catch (Exception ex)
            {
                MostrarMensaje(
                    ex.Message,
                    true
                );
            }
        }

        protected void btnEliminarPerfil_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstPerfiles.SelectedItem == null)
                    throw new Exception(
                        "Debe seleccionar un perfil."
                    );

                string valor =
                    lstPerfiles.SelectedValue;

                if (!valor.StartsWith("R-"))
                    throw new Exception(
                        "Debe seleccionar el perfil principal que desea eliminar."
                    );

                string[] partes =
                    valor.Split('-');

                int idRol =
                    Convert.ToInt32(partes[1]);

                bllRol.EliminarRol(
                    new Rol
                    {
                        Id = idRol
                    }
                );

                CargarPerfilesEnListBox();

                MostrarMensaje(
                    "Perfil eliminado correctamente.",
                    false
                );
            }
            catch (Exception ex)
            {
                MostrarMensaje(
                    ex.Message,
                    true
                );
            }
        }

        protected void btnEliminarComponente_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstPerfiles.SelectedItem == null)
                    throw new Exception(
                        "Debe seleccionar un componente."
                    );

                string valor =
                    lstPerfiles.SelectedValue;

                string[] partes =
                    valor.Split('-');

                // Permiso directo del rol
                if (valor.StartsWith("P-"))
                {
                    int idPermiso =
                        Convert.ToInt32(partes[1]);

                    int idRol =
                        Convert.ToInt32(partes[3]);

                    bllRol.EliminarPermiso(
                        new Rol { Id = idRol },
                        new Permiso { Id = idPermiso }
                    );
                }

                // Familia directamente asignada al rol
                else if (valor.StartsWith("F-"))
                {
                    int idFamilia =
                        Convert.ToInt32(partes[1]);

                    int idRol =
                        Convert.ToInt32(partes[3]);

                    bllRol.EliminarFamilia(
                        new Rol { Id = idRol },
                        new Familia { Id = idFamilia }
                    );
                }

                // Permiso que viene de una familia
                else if (valor.StartsWith("PF-"))
                {
                    throw new Exception(
                        "Ese permiso pertenece a una familia. " +
                        "Debe modificarlo desde Gestión de Familias."
                    );
                }
                else
                {
                    throw new Exception(
                        "Seleccione un permiso o una familia del perfil."
                    );
                }

                CargarPerfilesEnListBox();

                MostrarMensaje(
                    "Componente eliminado correctamente.",
                    false
                );
            }
            catch (Exception ex)
            {
                MostrarMensaje(
                    ex.Message,
                    true
                );
            }
        }

        protected void btnAgregarPerfil_Click(object sender, EventArgs e)
        {
            try
            {
                Rol nuevoRol = new Rol
                {
                    Nombre = txtNombrePerfil.Text.Trim(),
                    Descripcion = ""
                };

                bllRol.AgregarRol(nuevoRol);

                txtNombrePerfil.Text = "";

                CargarPerfilesEnListBox();

                MostrarMensaje(
                    "Perfil agregado correctamente.",
                    false
                );
            }
            catch (Exception ex)
            {
                MostrarMensaje(
                    ex.Message,
                    true
                );
            }
        }
        private void MostrarMensaje(
    string mensaje,
    bool error)
        {
            mensaje = mensaje
                .Replace("\\", "\\\\")
                .Replace("'", "\\'")
                .Replace("\r", "")
                .Replace("\n", "\\n");

            string clase =
                error
                    ? "alerta-web alerta-error"
                    : "alerta-web";

            string script = $@"
        document.getElementById('contenedor-alertas').innerHTML =
        '<div class=""{clase}"">{mensaje}</div>';
    ";

            ScriptManager.RegisterStartupScript(
                this,
                GetType(),
                Guid.NewGuid().ToString(),
                script,
                true
            );
        }
    }
}