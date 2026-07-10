using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.BLL;
using FGF_Finanzas.Capas.Servicios;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FGF_Finanzas
{
    public partial class DV_Form : System.Web.UI.Page
    {
        private BLLDigitoVerificador _bllDV = new BLLDigitoVerificador();
        private readonly BLLBackupRestore _bllBackupRestore = new BLLBackupRestore();
        protected void Page_Load(object sender, EventArgs e)
        {
            List<InconsistenciaReporte> listaInconsistencias = _bllDV.CompararDigito();
            if (listaInconsistencias.Count > 0)
            {
                GridInconsistencias.DataSource = listaInconsistencias;
                GridInconsistencias.DataBind();
                GridInconsistencias.Visible = true;
            }
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
                List<string> tablasAControlar = new List<string> { "Usuario" };

                foreach (string tabla in tablasAControlar)
                {
                    _bllDV.InicializarTablaCompleta(tabla);
                }
                string mensajeScript = "alert('Se han reestablecido los dígitos verificadores');";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertaInconsistencia", mensajeScript, true);
                Response.Redirect("~/Default.aspx");
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnRestore_Click(object sender, EventArgs e)
        {
            if (!fileRestore.HasFile)
            {
                lblMensaje.Text = "Debe seleccionar un archivo .bak en su computadora primero.";
                lblMensaje.ForeColor = System.Drawing.Color.Orange;
                return;
            }

            string extension = System.IO.Path.GetExtension(fileRestore.FileName).ToLower();
            if (extension != ".bak")
            {
                lblMensaje.Text = "Error: El archivo seleccionado debe tener la extensión .bak";
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
                string script = @"alert('Base de datos restaurada con éxito. El sistema volverá al inicio.'); 
                                window.location.href = 'Default.aspx';";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertaRestoreExito", script, true);
            }
            catch (Exception ex)
            {
                lblMensaje.Text = $"ERROR crítico en el proceso de restauración: {ex.Message}";
                lblMensaje.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}