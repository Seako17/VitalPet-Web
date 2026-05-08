using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.DAL;
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
        public BLLBackupRestore()
        {

        }

        public string HacerBackup(string rutaBase)
        {
            string nombreArchivo = $"Backup_FGF-Finanzas{DateTime.Now:yyyyMMdd_HHmmss}.bak";
            if (!Directory.Exists(rutaBase)) throw new Exception("No existe la carpeta temporal en el servidor");
            string rutaServidor = System.IO.Path.Combine(rutaBase, nombreArchivo);
            _dalBackupRestore.RealizarBackup(rutaServidor);
            return rutaServidor;

        }

        public void HacerRestore(string rutaArchivo)
        {
            if (!System.IO.File.Exists(rutaArchivo)) throw new Exception("El archivo no se subio correctamente.");
            _dalBackupRestore.RestaurarBDD(rutaArchivo);
        }
    }
}