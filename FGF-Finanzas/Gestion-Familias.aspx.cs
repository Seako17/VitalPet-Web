using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.BLL;
using FGF_Finanzas.Capas.Servicios;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FGF_Finanzas
{
    public partial class Gestion_Familias : PaginaSegura
    {
        protected override Permiso PermisoRequerido
        {
            get { return CodigosPermiso.ObtenerPermisos().Find(x => x.Nombre == "FamiliaGestionar"); }
        }
        BLLEvento bllEvento = new BLLEvento();
        BLLPermiso bLLPermiso = new BLLPermiso();
        BLLFamilia BLLFamilia = new BLLFamilia();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                lstPermisos.DataSource = bLLPermiso.ObtenerPermisos();
                lstPermisos.DataTextField = "Nombre";
                lstPermisos.DataValueField = "Id";
                lstPermisos.DataBind();
                CargarFamiliasEnListBox();
            }
        }
        private void CargarFamiliasEnListBox()
        {
            List<Familia> familias =
                BLLFamilia.ObtenerFamilias();

            lstFamiliaBase.Items.Clear();
            lstFamiliaAgregar.Items.Clear();

            foreach (Familia familia in familias)
            {
                AgregarFamiliaAlListBox(
                    familia,
                    0,
                    null,
                    familia.Id
                );

                lstFamiliaAgregar.Items.Add(
                    new ListItem(
                        familia.Nombre,
                        familia.Id.ToString()
                    )
                );
            }
        }
        private void AgregarFamiliaAlListBox(
     Familia familia,
     int nivel,
     int? idPadre,
     int idRaiz)
        {
            string sangria =
                new string('\u00A0', nivel * 4);

            string textoFamilia =
                nivel == 0
                    ? familia.Nombre
                    : sangria + "└─ " + familia.Nombre;

            string valorFamilia;

            if (idPadre.HasValue)
            {
                valorFamilia =
                    $"F-{familia.Id}-PADRE-{idPadre.Value}-ROOT-{idRaiz}";
            }
            else
            {
                valorFamilia =
                    $"F-{familia.Id}-ROOT-{idRaiz}";
            }

            lstFamiliaBase.Items.Add(
                new ListItem(
                    textoFamilia,
                    valorFamilia
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

                    lstFamiliaBase.Items.Add(
                        new ListItem(
                            sangriaPermiso + "• " + permiso.Nombre,
                            $"P-{permiso.Id}-PADRE-{familia.Id}-ROOT-{idRaiz}"
                        )
                    );
                }
                else if (componente is Familia familiaHija)
                {
                    AgregarFamiliaAlListBox(
                        familiaHija,
                        nivel + 1,
                        familia.Id,
                        idRaiz
                    );
                }
            }
        }

        protected void btnAsignarPermiso_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstFamiliaBase.SelectedItem == null)
                    throw new BECustomException(
                        "ERR_FAMILIA_SELECCIONAR_BASE"
                    );

                if (lstPermisos.SelectedItem == null)
                    throw new BECustomException(
                        "ERR_FAMILIA_SELECCIONAR_PERMISO"
                    );

                string valor =
                    lstFamiliaBase.SelectedValue;

                if (!valor.StartsWith("F-"))
                    throw new BECustomException(
                        "ERR_FAMILIA_SELECCIONAR_NO_PERMISO"
                    );

                string[] partes =
                    valor.Split('-');

                int idFamilia =
                    Convert.ToInt32(partes[1]);

                int idPermiso =
                    Convert.ToInt32(
                        lstPermisos.SelectedValue
                    );

                Familia familia =
                    new Familia
                    {
                        Id = idFamilia
                    };

                Permiso permiso =
                    new Permiso
                    {
                        Id = idPermiso
                    };

                BLLFamilia.AsignarPermiso(
                    familia,
                    permiso
                );

                CargarFamiliasEnListBox();

                MostrarMensaje(
                    ObtenerMensaje("MSG_FAMILIA_PERMISO_ASIGNADO"),
                    false
                );
            }
            catch (Exception ex)
            {
                MostrarMensaje(TraducirError(ex), true);
            }
        }

        protected void btnAsignarFamilia_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstFamiliaBase.SelectedItem == null)
                    throw new BECustomException(
                        "ERR_FAMILIA_SELECCIONAR_BASE"
                    );

                if (lstFamiliaAgregar.SelectedItem == null)
                    throw new BECustomException(
                        "ERR_FAMILIA_SELECCIONAR_AGREGAR"
                    );

                string valorBase =
                    lstFamiliaBase.SelectedValue;

                if (!valorBase.StartsWith("F-"))
                    throw new BECustomException(
                        "ERR_FAMILIA_SELECCIONAR_BASE_FAMILIA"
                    );

                string[] partes =
                    valorBase.Split('-');

                int idPadre =
                    Convert.ToInt32(partes[1]);

                int idHija =
                    Convert.ToInt32(
                        lstFamiliaAgregar.SelectedValue
                    );

                Familia padre =
                    new Familia
                    {
                        Id = idPadre
                    };

                Familia hija =
                    new Familia
                    {
                        Id = idHija
                    };

                BLLFamilia.AsignarFamilia(
                    padre,
                    hija
                );

                CargarFamiliasEnListBox();

                MostrarMensaje(
                    ObtenerMensaje("MSG_FAMILIA_ASIGNADA"),
                    false
                );
            }
            catch (Exception ex)
            {
                MostrarMensaje(TraducirError(ex), true);
            }
        }

        protected void btnEliminarFamilia_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstFamiliaBase.SelectedItem == null)
                    throw new BECustomException(
                        "ERR_FAMILIA_SELECCIONAR"
                    );

                string valor =
                    lstFamiliaBase.SelectedValue;

                if (!valor.StartsWith("F-"))
                    throw new BECustomException(
                        "ERR_FAMILIA_SELECCIONAR"
                    );

                string[] partes =
                    valor.Split('-');

                int idFamilia =
                    Convert.ToInt32(partes[1]);

                Familia familia =
                    new Familia
                    {
                        Id = idFamilia
                    };

                BLLFamilia.EliminarFamilia(familia);
                bllEvento.AgregarEvento(new BEEvento(SessionManager.Instancia.Usuario, DateTime.Now, "Administrador", "Eliminar Familia", 2));

                CargarFamiliasEnListBox();

                MostrarMensaje(
                    ObtenerMensaje("MSG_FAMILIA_ELIMINADA"),
                    false
                );
            }
            catch (Exception ex)
            {
                MostrarMensaje(TraducirError(ex), true);
            }
        }

        protected void btnEliminarComponente_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstFamiliaBase.SelectedItem == null)
                    throw new BECustomException(
                        "ERR_FAMILIA_SELECCIONAR_COMPONENTE"
                    );

                string valor =
                    lstFamiliaBase.SelectedValue;

                string[] partes =
                    valor.Split('-');

                if (valor.StartsWith("P-"))
                {
                    int idPermiso =
                        Convert.ToInt32(partes[1]);

                    int idFamilia =
                        Convert.ToInt32(partes[3]);

                    Familia familia =
                        new Familia
                        {
                            Id = idFamilia
                        };

                    Permiso permiso =
                        new Permiso
                        {
                            Id = idPermiso
                        };

                    BLLFamilia.EliminarPermiso(
                        familia,
                        permiso
                    );
                }
                else if (
                    valor.StartsWith("F-") &&
                    valor.Contains("-PADRE-"))
                {
                    int idHija =
                        Convert.ToInt32(partes[1]);

                    int idPadre =
                        Convert.ToInt32(partes[3]);

                    Familia padre =
                        new Familia
                        {
                            Id = idPadre
                        };

                    Familia hija =
                        new Familia
                        {
                            Id = idHija
                        };

                    BLLFamilia.EliminarRelacionFamilias(
                        padre,
                        hija
                    );
                }
                else
                {
                    throw new BECustomException(
                        "ERR_FAMILIA_RAIZ"
                    );
                }

                CargarFamiliasEnListBox();

                MostrarMensaje(
                    ObtenerMensaje("MSG_FAMILIA_COMPONENTE_ELIMINADO"),
                    false
                );
            }
            catch (Exception ex)
            {
                MostrarMensaje(TraducirError(ex), true);
            }
        }

        protected void btnAgregarFamilia_Click(object sender, EventArgs e)
        {
            try
            {
                Familia familia = new Familia
                {
                    Nombre = txtNombreFamilia.Text.Trim(),
                    Descripcion = ""
                };

                BLLFamilia.AgregarFamilia(familia);
                bllEvento.AgregarEvento(new BEEvento(SessionManager.Instancia.Usuario, DateTime.Now, "Administrador", "Agregar Familia", 2));

                txtNombreFamilia.Text = "";

                CargarFamiliasEnListBox();

                MostrarMensaje(
                    ObtenerMensaje("MSG_FAMILIA_CREADA"),
                    false
                );
            }
            catch (Exception ex)
            {
                MostrarMensaje(TraducirError(ex), true);
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