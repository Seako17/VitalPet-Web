using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.BLL;
using FGF_Finanzas.Capas.DAL;
using FGF_Finanzas.Capas.Servicios;
using FGF_Finanzas.Capas.Servicios.Cambio_Idioma;
using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FGF_Finanzas
{
    public partial class Gestion_Usuarios : PaginaSegura
    {
        protected override Permiso PermisoRequerido
        {
            get { return CodigosPermiso.ObtenerPermisos().Find(x => x.Nombre == "UsuarioGestionar"); }
        }
        BLLUsuario bllUsuario = new BLLUsuario();
        BLLEvento bllEvento = new BLLEvento();
        BLLRol bllRol = new BLLRol();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CargarRoles();
                EstablecerEstado("Consulta");
                CargarGrillaUsuarios();
            }

        }
        private void CargarRoles()
        {
            var roles = bllRol.ObtenerRoles();

            ddlRol.DataSource = roles;
            ddlRol.DataTextField = "Nombre";
            ddlRol.DataValueField = "Id";
            ddlRol.DataBind();

            ddlRol.Items.Insert(
                0,
                new System.Web.UI.WebControls.ListItem(
                    "-- Seleccionar Rol --",
                    ""
                )
            );
        }
        private void CargarGrillaUsuarios()
        {
            DataTable dt;

            if (chkVerEncriptado.Checked)
            {
                dt = bllUsuario.ObtenerUsuariosPuros();
            }
            else
            {
                dt = bllUsuario.ObtenerUsuarios();
            }

            if (rblFiltroTodosActivos.SelectedValue == "Bloqueados")
            {
                var filasFiltradas = dt.AsEnumerable().Where(row => Convert.ToBoolean(row.Field<object>("Bloqueado")) == true);
                if (filasFiltradas.Any())
                    dgvUsuarios.DataSource = filasFiltradas.CopyToDataTable();
                else
                    dgvUsuarios.DataSource = dt.Clone();
            }
            else
            {
                dgvUsuarios.DataSource = dt.DefaultView;
            }

            dgvUsuarios.DataBind();
        }

        

        private void EstablecerEstado(string modo)
        {
            txtModo.Text = modo;

            bool esConsulta = (modo == "Consulta");
            btnCrear.Enabled = esConsulta;
            btnDesbloquear.Enabled = esConsulta;
            btnModificar.Enabled = esConsulta;
            btnAplicar.Enabled = !esConsulta;
            btnCancelar.Enabled = !esConsulta;

            switch (modo)
            {
                case "Consulta":
                    LimpiarFormulario();
                    AlternarCampos(false);
                    if (dgvUsuarios.Rows.Count > 0) dgvUsuarios.SelectedIndex = -1;
                    break;

                case "Crear":
                    LimpiarFormulario();
                    AlternarCampos(true);
                    txtNombreUsuario.Enabled = false;
                    if (dgvUsuarios.Rows.Count > 0) dgvUsuarios.SelectedIndex = -1;
                    break;

                case "Modificar":
                    if (dgvUsuarios.SelectedRow != null)
                    {
                        LlenarCamposForm();
                        AlternarCampos(true);
                        txtDni.Enabled = false;
                    }
                    else
                    {
                        AlternarCampos(false);
                    }
                    break;

                case "Desbloquear":
                    AlternarCampos(false);
                    if (dgvUsuarios.SelectedRow != null)
                    {
                        LlenarCamposForm();
                    }
                    break;
            }
        }

        private void AlternarCampos(bool editable)
        {
            txtDni.Enabled = editable;
            txtNombre.Enabled = editable;
            txtApellido.Enabled = editable;
            txtEmail.Enabled = editable;
            txtNombreUsuario.Enabled = editable;
            ddlRol.Enabled = editable;
        }

        private void LimpiarFormulario()
        {
            txtDni.Text = string.Empty;
            txtNombre.Text = string.Empty;
            txtApellido.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtNombreUsuario.Text = string.Empty;
            ddlRol.SelectedIndex = -1;
        }

        private void LlenarCamposForm()
        {
            if (dgvUsuarios.SelectedRow == null)
                return;

            var cells = dgvUsuarios.SelectedRow.Cells;

            string dni = System.Web.HttpUtility.HtmlDecode(cells[0].Text).Trim();

            BEUsuario usuarioReal =
                bllUsuario.ConsultaIndividual(dni);

            if (usuarioReal != null)
            {
                txtDni.Text = usuarioReal.DNI;
                txtNombre.Text = usuarioReal.Nombre;
                txtApellido.Text = usuarioReal.Apellido;
                txtEmail.Text = usuarioReal.Mail;
                txtNombreUsuario.Text = usuarioReal.Usuario;

                if (usuarioReal.Rol != null)
                {
                    string idRol = usuarioReal.Rol.Id.ToString();

                    if (ddlRol.Items.FindByValue(idRol) != null)
                    {
                        ddlRol.SelectedValue = idRol;
                    }
                }
                else
                {
                    ddlRol.SelectedIndex = 0;
                }
            }
        }



        protected void btnCrear_Click(object sender, EventArgs e)
        {
            EstablecerEstado("Crear");
        }

        protected void btnModificar_Click(object sender, EventArgs e)
        {
            EstablecerEstado("Modificar");
        }

        protected void btnDesbloquear_Click(object sender, EventArgs e)
        {
            EstablecerEstado("Desbloquear");
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            EstablecerEstado("Consulta");
        }


        protected void dgvUsuarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            string modoActual = txtModo.Text;

            if (modoActual == "Modificar" || modoActual == "Consulta" || modoActual == "Desbloquear" || modoActual == "Crear")
            {
                EstablecerEstado(modoActual);
            }
        }

        protected void btnAplicar_Click(object sender, EventArgs e)
        {
            string modo = txtModo.Text;
            try
            {
                switch (modo)
                {
                    case "Crear":
                        string dni = txtDni.Text;
                        string apellido = txtApellido.Text;
                        string nombre = txtNombre.Text;
                        string username = dni + nombre;
                        string password = dni + apellido;
                        string email = txtEmail.Text;
                        int rol = int.Parse(ddlRol.SelectedValue);
                        if (string.IsNullOrWhiteSpace(dni) ||
                            string.IsNullOrWhiteSpace(apellido) ||
                            string.IsNullOrWhiteSpace(nombre) ||
                            string.IsNullOrWhiteSpace(email) ||
                            string.IsNullOrEmpty(rol.ToString()))
                        {
                            throw new BECustomException("ERR_USUARIO_CAMPOS_CREAR");
                        }
                        if (!Regex.IsMatch(dni, @"^\d{8}$")) throw new BECustomException("ERR_DNI_8_DIGITOS_USUARIO");

                        DataTable dtUsuarios = bllUsuario.ObtenerUsuarios();
                        if (dtUsuarios.AsEnumerable().Any(row => row.Field<string>("DNI") == dni)) throw new BECustomException("ERR_DNI_EN_USO");
                        if (dtUsuarios.AsEnumerable().Any(row => row.Field<string>("mail").Equals(email, StringComparison.OrdinalIgnoreCase))) throw new BECustomException("ERR_EMAIL_EN_USO");

                        bllUsuario.AgregarUsuario(new BEUsuario(dni, nombre, apellido, username, password, email, bllRol.ObtenerCompleto(new Rol(rol))));
                        MostrarAlerta(ObtenerMensaje("MSG_USUARIO_AGREGADO"));
                        CargarGrillaUsuarios();
                        EstablecerEstado("Consulta");
                        break;
                    case "Desbloquear":
                        if (dgvUsuarios.SelectedRow == null) throw new BECustomException("ERR_SELECCIONAR_USUARIO_DESBLOQUEAR");
                        string dnibloqueado = dgvUsuarios.SelectedRow.Cells[0].Text.Trim();
                        BEUsuario usuario = bllUsuario.ConsultaIndividual(dnibloqueado);
                        if (usuario == null) throw new BECustomException("ERR_USUARIO_NO_EXISTE");
                        if (!usuario.Bloqueado) throw new BECustomException("ERR_USUARIO_NO_BLOQUEADO");
                        usuario.Bloqueado = false;
                        usuario.Intento = 0;
                        string nuevaContraseña = usuario.DNI + usuario.Apellido;
                        usuario.Contraseña = Encriptacion.Encriptar(nuevaContraseña);
                        bllUsuario.ActualizarUsuario(usuario);
                        bllEvento.AgregarEvento(new BEEvento(SessionManager.Instancia.Usuario, DateTime.Now, "Usuarios", "Desbloquear Usuario", 2));
                        MostrarAlerta(ObtenerMensaje("MSG_USUARIO_DESBLOQUEADO"));
                        CargarGrillaUsuarios();
                        EstablecerEstado("Consulta");
                        break;
                    case "Modificar":
                        if (dgvUsuarios.SelectedRow == null) throw new BECustomException("ERR_SELECCIONAR_USUARIO_MODIFICAR");
                        string dniModificar = txtDni.Text.Trim();
                        string apellidoModificar = txtApellido.Text.Trim();
                        string nombreModificar = txtNombre.Text.Trim();
                        string emailModificar = txtEmail.Text.Trim();
                        string usernameModificar = txtNombreUsuario.Text.Trim();
                        int rolModificar = int.Parse(ddlRol.SelectedValue);

                        if (string.IsNullOrWhiteSpace(dniModificar) ||
                            string.IsNullOrWhiteSpace(apellidoModificar) ||
                            string.IsNullOrWhiteSpace(nombreModificar) ||
                            string.IsNullOrWhiteSpace(emailModificar) ||
                            string.IsNullOrEmpty(rolModificar.ToString()) ||
                            string.IsNullOrEmpty(usernameModificar))
                        {
                            throw new BECustomException("ERR_USUARIO_CAMPOS_MODIFICAR");
                        }
                        BEUsuario usuarioModificar = bllUsuario.ConsultaIndividual(dniModificar);
                        if (usuarioModificar == null)
                        {
                            throw new BECustomException("ERR_USUARIO_MODIFICAR_NO_EXISTE");
                        }
                        DataTable dtUsuariosM = bllUsuario.ObtenerUsuarios();
                        if (dtUsuariosM.AsEnumerable().Any(row => row.Field<string>("mail").Equals(emailModificar, StringComparison.OrdinalIgnoreCase) && row.Field<string>("DNI") != dniModificar)) throw new BECustomException("ERR_EMAIL_EN_USO");
                        if (dtUsuariosM.AsEnumerable().Any(row => row.Field<string>("Usuario").Equals(usernameModificar, StringComparison.OrdinalIgnoreCase) && row.Field<string>("DNI") != dniModificar)) throw new BECustomException("ERR_USERNAME_EN_USO");
                        usuarioModificar.Nombre = nombreModificar;
                        usuarioModificar.Apellido = apellidoModificar;
                        usuarioModificar.Mail = emailModificar;
                        usuarioModificar.Rol = bllRol.ObtenerCompleto(new Rol(rolModificar));
                        usuarioModificar.Usuario = usernameModificar;
                        bllUsuario.ActualizarUsuario(usuarioModificar);
                        bllEvento.AgregarEvento(new BEEvento(SessionManager.Instancia.Usuario, DateTime.Now, "Usuarios", "Modificar Usuario", 2));
                        MostrarAlerta(ObtenerMensaje("MSG_USUARIO_MODIFICADO"));
                        CargarGrillaUsuarios();
                        EstablecerEstado("Consulta");
                        break;
                }
            }
            catch (Exception ex)
            {

                MostrarAlerta(TraducirError(ex), "error");
            }
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            TraducirColumna(dgvUsuarios, 0, "dgvUsuarios_Header_Dni", "DNI");
            TraducirColumna(dgvUsuarios, 1, "dgvUsuarios_Header_Nombre", "Nombre");
            TraducirColumna(dgvUsuarios, 2, "dgvUsuarios_Header_Apellido", "Apellido");
            TraducirColumna(dgvUsuarios, 3, "dgvUsuarios_Header_Mail", "Email");
            TraducirColumna(dgvUsuarios, 4, "dgvUsuarios_Header_Usuario", "Usuario");
            TraducirColumna(dgvUsuarios, 5, "dgvUsuarios_Header_Rol", "Rol");
            TraducirBotonSeleccion(dgvUsuarios, 6, "dgvUsuarios_Seleccionar", "Seleccionar");
            TraducirSinDatos(dgvUsuarios, "dgvUsuarios_Vacio", "No hay usuarios para mostrar.");

            TraducirItem(rblFiltroTodosActivos, "Bloqueados", "rblFiltroTodosActivos_Bloqueados", "Bloqueados");
            TraducirItem(rblFiltroTodosActivos, "Todos", "rblFiltroTodosActivos_Todos", "Todos");
            TraducirItem(ddlRol, string.Empty, "ddlRol_Seleccionar", "-- Seleccionar Rol --");
        }

        protected void rblFiltroTodosActivos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CargarGrillaUsuarios(); 
                if (dgvUsuarios.Rows.Count > 0) dgvUsuarios.SelectedIndex = -1;
                EstablecerEstado("Consulta");
            }
            catch (Exception ex)
            {
                MostrarAlerta(ObtenerError("ERR_FILTRAR_LISTA", ex.Message), "error");
            }
        }
        protected void chkVerEncriptado_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CargarGrillaUsuarios();
                if (dgvUsuarios.Rows.Count > 0) dgvUsuarios.SelectedIndex = -1;
                EstablecerEstado("Consulta");
            }
            catch (Exception ex)
            {
                MostrarAlerta(ObtenerError("ERR_CAMBIAR_VISUALIZACION", ex.Message), "error");
            }
        }
    }
}