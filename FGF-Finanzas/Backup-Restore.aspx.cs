using FGF_Finanzas.Capas.BLL;
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
        protected void Page_Load(object sender, EventArgs e)
        {

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
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text =$"ERROR: {ex.Message}";
                lblMensaje.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}