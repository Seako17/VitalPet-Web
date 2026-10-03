using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.BLL;
using FGF_Finanzas.Capas.Servicios;
using FGF_Finanzas.Capas.Servicios.Cambio_Idioma;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FGF_Finanzas
{
    public partial class DV_Form : BasePage
    {
        private BLLDigitoVerificador _bllDV = new BLLDigitoVerificador();
        private readonly BLLBackupRestore _bllBackupRestore = new BLLBackupRestore();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<InconsistenciaReporte> listaInconsistencias = _bllDV.CompararDigito();
                if (listaInconsistencias.Count > 0)
                {
                    GridInconsistencias.DataSource = listaInconsistencias;
                    GridInconsistencias.DataBind();
                    GridInconsistencias.Visible = true;
                }
                else
                {
                    Response.Redirect("~/Default.aspx");
                }
            }
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            TraducirColumna(GridInconsistencias, 0, "GridInconsistencias_Header_NombreTabla", "Tabla Afectada");
            TraducirColumna(GridInconsistencias, 1, "GridInconsistencias_Header_IdRegistro", "ID Registro");
            TraducirColumna(GridInconsistencias, 2, "GridInconsistencias_Header_TipoFalla", "Diagnóstico de la Inconsistencia");
        }

        private string TextoTraducido(string clave, string porDefecto)
        {
            string texto = IdiomaManager.Instancia.ObtenerTexto(NombreFormulario, clave);
            return texto == "[" + clave + "]" ? porDefecto : texto;
        }

        private void TraducirColumna(GridView grilla, int columna, string clave, string porDefecto)
        {
            string texto = TextoTraducido(clave, porDefecto);
            grilla.Columns[columna].HeaderText = texto;

            if (grilla.HeaderRow != null && columna < grilla.HeaderRow.Cells.Count)
                grilla.HeaderRow.Cells[columna].Text = texto;
        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            SessionManager.LogOut();

            Session.Clear();
            Session.Abandon();

            if (Request.Cookies["UserSessionFGF"] != null)
            {
                HttpCookie myCookie = new HttpCookie("UserSessionFGF");
                myCookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(myCookie);
            }

            Response.Redirect("~/Default.aspx");
        }

        protected void btnRecalcular_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> tablasAControlar = new List<string> { "Usuario", "Mascota" };

                foreach (string tabla in tablasAControlar)
                {
                    _bllDV.InicializarTablaCompleta(tabla);
                }
                string mensaje = ObtenerMensaje("MSG_DV_RESTABLECIDOS").Replace("'", "\\'");
                string mensajeScript = "alert('" + mensaje + "'); window.location.href = 'Default.aspx';";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertaInconsistencia", mensajeScript, true);
            }
            catch (Exception ex)
            {
                lblMensaje.Text = TraducirError(ex, "ERR_REESTABLECER_DV");
                lblMensaje.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void btnRestore_Click(object sender, EventArgs e)
        {
            if (!fileRestore.HasFile)
            {
                lblMensaje.Text = ObtenerError("ERR_ARCHIVO_REQUERIDO");
                lblMensaje.ForeColor = System.Drawing.Color.Orange;
                return;
            }

            string extension = System.IO.Path.GetExtension(fileRestore.FileName).ToLower();
            if (extension != ".bak")
            {
                lblMensaje.Text = ObtenerError("ERR_ARCHIVO_EXTENSION_BAK");
                lblMensaje.ForeColor = System.Drawing.Color.Red;
                return;
            }

            string rutaTemp = string.Empty;
            try
            {
                string nombreArchivo = fileRestore.FileName.Trim();
                string carpetaTemp = Server.MapPath("~/TempBackups/");

                if (!Directory.Exists(carpetaTemp))
                {
                    Directory.CreateDirectory(carpetaTemp);
                }

                rutaTemp = Path.Combine(carpetaTemp, nombreArchivo);
                fileRestore.SaveAs(rutaTemp);
                _bllBackupRestore.HacerRestore(rutaTemp);
                string mensaje = ObtenerMensaje("MSG_BASE_RESTAURADA_INICIO").Replace("'", "\\'");
                string script = "alert('" + mensaje + "'); window.location.href = 'Default.aspx';";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertaRestoreExito", script, true);
            }
            catch (Exception ex)
            {
                lblMensaje.Text = TraducirError(ex, "ERR_RESTORE_CRITICO");
                lblMensaje.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}