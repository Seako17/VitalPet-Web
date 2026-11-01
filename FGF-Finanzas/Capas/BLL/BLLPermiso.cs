using FGF_Finanzas.Capas.DAL;
using FGF_Finanzas.Capas.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FGF_Finanzas.Capas.BLL
{
    public class BLLPermiso
    {
        private DALPermiso permisoDAL = new DALPermiso();
        public List<Permiso> ObtenerPermisos()
        {
            return permisoDAL.ObtenerPermisos();
        }
    }
}