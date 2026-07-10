using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.DAL;
using FGF_Finanzas.Capas.Servicios;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace FGF_Finanzas.Capas.BLL
{
    public class BLLBackupRestore
    {
        private DALBackupRestore _dalBackupRestore = new DALBackupRestore();
        BLLEvento bllEvento;
        public BLLBackupRestore()
        {
            bllEvento=new BLLEvento();
        }

        public string HacerBackup(string rutaBase)
        {
            string nombreArchivo = $"Backup_Vital-Pet{DateTime.Now:yyyyMMdd_HHmmss}.bak";
            if (!Directory.Exists(rutaBase)) throw new Exception("No existe la carpeta temporal en el servidor");
            string rutaServidor = System.IO.Path.Combine(rutaBase, nombreArchivo);
            _dalBackupRestore.RealizarBackup(rutaServidor);
            bllEvento.AgregarEvento(new BEEvento(SessionManager.Instancia.Usuario, DateTime.Now, "Administrador", "Hacer Backup", 4));
            return rutaServidor;
        }

        public void HacerRestore(string rutaArchivo)
        {
            if (!System.IO.File.Exists(rutaArchivo))
                throw new Exception("El archivo no se subió correctamente al servidor.");
            try
            {
                _dalBackupRestore.RestaurarBDD(rutaArchivo);
                bllEvento.AgregarEvento(new BEEvento(SessionManager.Instancia.Usuario, DateTime.Now, "Administrador", "Hacer Restore", 2));
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en el proceso de restauración: {ex.Message}", ex);
            }
            finally
            {
                if (System.IO.File.Exists(rutaArchivo))
                {
                    System.IO.File.Delete(rutaArchivo);
                }
            }
        }
    }
}