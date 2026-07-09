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
            if (!IsPostBack)
            {
                if (Session["InconsistenciasDetectadas"] != null)
                {
                    var listaInconsistencias = (List<InconsistenciaReporte>)Session["InconsistenciasDetectadas"];
                    GridInconsistencias.DataSource = listaInconsistencias;
                    GridInconsistencias.DataBind();
                    GridInconsistencias.Visible = true;

                    Session["InconsistenciasDetectadas"] = null;
                }
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
                List<string> tablasAControlar = new List<string> { "Usuario"};

                foreach (string tabla in tablasAControlar)
                {
                    _bllDV.InicializarTablaCompleta(tabla);
                }

            }
            catch (Exception ex)
            {
            }
        }

        protected void btnRestore_Click(object sender, EventArgs e)
        {
            if (fileRestore.HasFile)
            {
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

                    lblMensaje.Text = "Base de datos restaurada con éxito.";
                    lblMensaje.ForeColor = System.Drawing.Color.Green;
                }
                catch (Exception ex)
                {
                    lblMensaje.Text = $"ERROR al restaurar: {ex.Message}";
                    lblMensaje.ForeColor = System.Drawing.Color.Red;
                }
            }
            else
            {
                lblMensaje.Text = "Debe seleccionar un archivo .bak en su computadora primero.";
                lblMensaje.ForeColor = System.Drawing.Color.Orange;
            }
        }
    }
}