using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FGF_Finanzas
{
    public partial class Gestion_Usuarios : System.Web.UI.Page
    {
        BLLUsuario bllUsuario = new BLLUsuario();
        protected void Page_Load(object sender, EventArgs e)
        {
            ModoConsulta();
            CargarGrillaUsuarios();
        }

        private void CargarGrillaUsuarios()
        {
            DataView dv = bllUsuario.ObtenerUsuarios().DefaultView;
            dgvUsuarios.DataSource = dv;
            dgvUsuarios.DataBind();
        }

        private void ModoConsulta()
        {
            txtDni.Enabled = true;
            btnCrear.Enabled = true;
            btnDesbloquear.Enabled = true;
            btnModificar.Enabled = true;
            btnAplicar.Enabled = false;
            btnCancelar.Enabled = false;

            if (dgvUsuarios.Rows.Count > 0)
            {
                dgvUsuarios.SelectedIndex = -1;
            }
            txtDni.Enabled = true;
            txtNombre.Enabled = true;
            txtApellido.Enabled = true;
            txtEmail.Enabled = true;
            txtNombreUsuario.Enabled = true;
            txtModo.Enabled = false;
            ddlRol.Enabled = true;

            txtDni.Text = string.Empty;
            txtNombre.Text = string.Empty;
            txtApellido.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtNombreUsuario.Text = string.Empty;
            txtModo.Text = "Modo: Consulta";
            ddlRol.Text = string.Empty;
        }
        protected void btnCrear_Click(object sender, EventArgs e)
        {
            txtModo.Text = "Modo: Crear";
            btnCrear.Enabled = false;
            btnDesbloquear.Enabled = false;
            btnModificar.Enabled = false;
            btnAplicar.Enabled = true;
            btnCancelar.Enabled = true;
            txtNombreUsuario.Enabled = false;
        }
        protected void btnDesbloquear_Click(object sender, EventArgs e)
        {
            txtModo.Text = "Modo: Desbloquear";
            btnCrear.Enabled = false;
            btnDesbloquear.Enabled = false;
            btnModificar.Enabled = false;
            btnAplicar.Enabled = true;
            btnCancelar.Enabled = true;

            txtDni.Enabled = false;
            txtNombre.Enabled = false;
            txtApellido.Enabled = false;
            txtEmail.Enabled = false;
            txtNombreUsuario.Enabled = false;
            ddlRol.Enabled = false;
            txtModo.Enabled = false;
        }
        protected void btnModificar_Click(object sender, EventArgs e)
        {
            txtModo.Text = "Modo: Modificar";
            txtDni.Enabled = false;
            btnCrear.Enabled = false;
            btnDesbloquear.Enabled = false;
            btnModificar.Enabled = false;
            
            btnAplicar.Enabled = true;
            btnCancelar.Enabled = true;
            txtNombreUsuario.Enabled = false;
            if (dgvUsuarios.SelectedRow != null)
            {
                llenarCamposForm();
            }
        }

        private void llenarCamposForm()
        {
            if (dgvUsuarios.SelectedRow != null)
            {
                txtDni.Text = dgvUsuarios.SelectedRow.Cells[0].Text;
                txtNombre.Text = dgvUsuarios.SelectedRow.Cells[1].Text;
                txtApellido.Text = dgvUsuarios.SelectedRow.Cells[2].Text;
                txtEmail.Text = dgvUsuarios.SelectedRow.Cells[3].Text;
                txtNombreUsuario.Text = dgvUsuarios.SelectedRow.Cells[4].Text;

                // Para el DropDownList del Rol
                string rol = dgvUsuarios.SelectedRow.Cells[5].Text;
                if (ddlRol.Items.FindByValue(rol) != null)
                    ddlRol.SelectedValue = rol;
            }
        }

        protected void dgvUsuarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRow != null)
            {
                llenarCamposForm();
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            ModoConsulta();
        }
    }
}