using FGF_Finanzas.Capas.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FGF_Finanzas.Capas.Servicios
{
    public static class CodigosPermiso
    {
        private static BLLPermiso bllPermiso = new BLLPermiso();
        public static List<Permiso> ObtenerPermisos()
        {
            List<Permiso> permisos = new List<Permiso>();
            foreach (var item in bllPermiso.ObtenerPermisos())
            {
                permisos.Add(item);
            }
            return permisos;
        }
    }
}