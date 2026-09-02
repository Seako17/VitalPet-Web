using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.BLL;
using FGF_Finanzas.Capas.DAL;
using FGF_Finanzas.Capas.Servicios;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace FGF_Finanzas
{
    public partial class Gestion_Usuarios : System.Web.UI.Page
    {
        BLLUsuario bllUsuario = new BLLUsuario();
        BLLEvento bllEvento = new BLLEvento();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                EstablecerEstado("Consulta");
                CargarGrillaUsuarios();
            }

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
            if (dgvUsuarios.SelectedRow == null) return;
            var cells = dgvUsuarios.SelectedRow.Cells;
            string dni = System.Web.HttpUtility.HtmlDecode(cells[0].Text).Trim();
            BEUsuario usuarioReal = bllUsuario.ConsultaIndividual(dni);

            if (usuarioReal != null)
            {
                txtDni.Text = usuarioReal.DNI;
                txtNombre.Text = usuarioReal.Nombre;
                txtApellido.Text = usuarioReal.Apellido;
                txtEmail.Text = usuarioReal.Mail;
                txtNombreUsuario.Text = usuarioReal.Usuario;

                if (ddlRol.Items.FindByValue(usuarioReal.Rol) != null)
                    ddlRol.SelectedValue = usuarioReal.Rol;
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
                        string rol = ddlRol.SelectedValue;
                        if (string.IsNullOrWhiteSpace(dni) ||
                            string.IsNullOrWhiteSpace(apellido) ||
                            string.IsNullOrWhiteSpace(nombre) ||
                            string.IsNullOrWhiteSpace(email) ||
                            string.IsNullOrEmpty(rol))
                        {
                            throw new Exception("Todos los campos son obligatorios para crear el usuario.");
                        }
                        if (!Regex.IsMatch(dni, @"^\d{8}$")) throw new Exception("El DNI debe tener 8 digitos.");

                        DataTable dtUsuarios = bllUsuario.ObtenerUsuarios();
                        if (dtUsuarios.AsEnumerable().Any(row => row.Field<string>("DNI") == dni)) throw new Exception("El DNI ingresado ya se encuentra en uso.");
                        if (dtUsuarios.AsEnumerable().Any(row => row.Field<string>("mail").Equals(email, StringComparison.OrdinalIgnoreCase))) throw new Exception("El Email ingresado ya se encuentra en uso.");

                        bllUsuario.AgregarUsuario(new BEUsuario(dni, nombre, apellido, username, password, email, rol));
                        MostrarAlerta("Usuario añadido correctamente.");
                        CargarGrillaUsuarios();
                        EstablecerEstado("Consulta");
                        break;
                    case "Desbloquear":
                        if (dgvUsuarios.SelectedRow == null) throw new Exception("Debe seleccionar un usuario de la lista para poder desbloquearlo.");
                        string dnibloqueado = dgvUsuarios.SelectedRow.Cells[0].Text.Trim();
                        BEUsuario usuario = bllUsuario.ConsultaIndividual(dnibloqueado);
                        if (usuario == null) throw new Exception("El usuario no existe.");
                        if (!usuario.Bloqueado) throw new Exception("El usuario no está bloqueado.");
                        usuario.Bloqueado = false;
                        usuario.Intento = 0;
                        string nuevaContraseña = usuario.DNI + usuario.Apellido;
                        usuario.Contraseña = Encriptacion.Encriptar(nuevaContraseña);
                        bllUsuario.ActualizarUsuario(usuario);
                        bllEvento.AgregarEvento(new BEEvento(SessionManager.Instancia.Usuario, DateTime.Now, "Usuarios", "Desbloquear Usuario", 2));
                        MostrarAlerta("El usuario ha sido desbloqueado correctamente.");
                        CargarGrillaUsuarios();
                        EstablecerEstado("Consulta");
                        break;
                    case "Modificar":
                        if (dgvUsuarios.SelectedRow == null) throw new Exception("Debe seleccionar un usuario en la lista para poder modificarlo.");
                        string dniModificar = txtDni.Text.Trim();
                        string apellidoModificar = txtApellido.Text.Trim();
                        string nombreModificar = txtNombre.Text.Trim();
                        string emailModificar = txtEmail.Text.Trim();
                        string usernameModificar = txtNombreUsuario.Text.Trim();
                        string rolModificar = ddlRol.SelectedValue;

                        if (string.IsNullOrWhiteSpace(dniModificar) ||
                            string.IsNullOrWhiteSpace(apellidoModificar) ||
                            string.IsNullOrWhiteSpace(nombreModificar) ||
                            string.IsNullOrWhiteSpace(emailModificar) ||
                            string.IsNullOrEmpty(rolModificar) ||
                            string.IsNullOrEmpty(usernameModificar))
                        {
                            throw new Exception("Todos los campos son obligatorios para modificar el usuario.");
                        }
                        BEUsuario usuarioModificar = bllUsuario.ConsultaIndividual(dniModificar);
                        if (usuarioModificar == null)
                        {
                            throw new Exception("El usuario que intenta modificar no existe.");
                        }
                        DataTable dtUsuariosM = bllUsuario.ObtenerUsuarios();
                        if (dtUsuariosM.AsEnumerable().Any(row => row.Field<string>("mail").Equals(emailModificar, StringComparison.OrdinalIgnoreCase) && row.Field<string>("DNI") != dniModificar)) throw new Exception("El Email ingresado ya se encuentra en uso.");
                        if (dtUsuariosM.AsEnumerable().Any(row => row.Field<string>("Usuario").Equals(usernameModificar, StringComparison.OrdinalIgnoreCase) && row.Field<string>("DNI") != dniModificar)) throw new Exception("El nombre de usuario ingresado ya se encuentra en uso.");
                        usuarioModificar.Nombre = nombreModificar;
                        usuarioModificar.Apellido = apellidoModificar;
                        usuarioModificar.Mail = emailModificar;
                        usuarioModificar.Rol = rolModificar;
                        usuarioModificar.Usuario = usernameModificar;
                        bllUsuario.ActualizarUsuario(usuarioModificar);
                        bllEvento.AgregarEvento(new BEEvento(SessionManager.Instancia.Usuario, DateTime.Now, "Usuarios", "Modificar Usuario", 2));
                        MostrarAlerta("Usuario modificado correctamente.");
                        CargarGrillaUsuarios();
                        EstablecerEstado("Consulta");
                        break;
                }
            }
            catch (Exception ex)
            {

                MostrarAlerta(ex.Message, "error");
            }
        }

        private void MostrarAlerta(string mensaje, string tipo = "exito")
        {
            string mensajeFormateado = mensaje.Replace("'", "\\'");
            string script = $"mostrarAlerta('{mensajeFormateado}', '{tipo}');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), script, true);
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
                MostrarAlerta("Error al filtrar la lista: " + ex.Message, "error");
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
                MostrarAlerta("Error al cambiar visualización: " + ex.Message, "error");
            }
        }

        protected void btnExportar_Click(object sender, EventArgs e)
        {
            DataTable dt = bllUsuario.ObtenerUsuarios();

            string carpeta = Server.MapPath("~/Temp");

            if (!Directory.Exists(carpeta))
            {
                Directory.CreateDirectory(carpeta);
            }

            string ruta = Path.Combine(carpeta, "Usuarios.xml");

            XMLExportador exp = new XMLExportador();
            exp.Exportar(dt, ruta);

            Response.Clear();
            Response.ContentType = "application/xml";
            Response.AddHeader("Content-Disposition", "attachment; filename=usuarios.xml");
            Response.TransmitFile(ruta);
            Response.End();
        }
    }
}