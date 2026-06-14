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
    public partial class Backup_Restore : System.Web.UI.Page
    {
        private readonly BLLBackupRestore _bllBackupRestore = new BLLBackupRestore();
        BLLEvento bllEvento;
        protected void Page_Load(object sender, EventArgs e)
        {
            bllEvento = new BLLEvento();
        }

        protected void btnBackup_Click(object sender, EventArgs e)
        {
            string rutaFisicaServidor = string.Empty;
            try
            {
                string rutaBase = Server.MapPath("~/TempBackups/");//Carpeta en la que se guardan temporalmente los backups hasta que se descarguen.
                rutaFisicaServidor = _bllBackupRestore.HacerBackup(rutaBase);
                FileInfo archivo = new FileInfo(rutaFisicaServidor);
                if (archivo.Exists)
                {
                    Response.Clear();
                    Response.ClearHeaders();
                    Response.ClearContent();

                    Response.ContentType = "application/octet-stream";//Tipo de archivo


                    Response.AddHeader("Content-Disposition", $"attachment; filename={archivo.Name}");
                    Response.AddHeader("Content-Length", archivo.Length.ToString());
                    Response.TransmitFile(archivo.FullName);//Envia el archivo del servidor al cliente
                    Response.Flush(); //Finaliza la respuesta

                    //Borra el archivo en el servidor
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    File.Delete(rutaFisicaServidor);
                    Response.End();

                    lblMensaje.Text = "Backup creado con exito.";
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = $"ERROR: {ex.Message}";
                lblMensaje.ForeColor = System.Drawing.Color.Red;
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