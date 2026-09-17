using FGF_Finanzas.Capas.DAL;
using FGF_Finanzas.Capas.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FGF_Finanzas.Capas.BLL
{
    public class BLLRol
    {
        private DALRol rolDAL = new DALRol();
        private DALPermiso permisoDAL = new DALPermiso();
        private DALFamilia familiaDAL = new DALFamilia();

        public Rol ObtenerCompleto(Rol roli)
        {
            Rol rol = rolDAL.ObtenerRol(roli);

            if (rol == null)
                return null;

            List<Permiso> permisos = permisoDAL.ListarPorRol(roli);

            foreach (Permiso permiso in permisos)
            {
                rol.Componentes.Add(permiso);
            }

            List<Familia> familias = familiaDAL.ListarPorRol(roli);

            foreach (Familia familia in familias)
            {
                CargarFamilia(familia);

                rol.Componentes.Add(familia);
            }

            return rol;
        }

        private void CargarFamilia(Familia familia)
        {
            List<Permiso> permisos = permisoDAL.ListarPorFamilia(familia);

            foreach (Permiso permiso in permisos)
            {
                familia.Agregar(permiso);
            }

            List<Familia> hijas = familiaDAL.ListarHijas(familia);

            foreach (Familia hija in hijas)
            {
                CargarFamilia(hija);

                familia.Agregar(hija);
            }
        }

        public bool TienePermiso(Rol roli, Permiso permiso)
        {
            Rol rol = ObtenerCompleto(roli);

            if (rol == null)
                return false;

            return rol.TienePermiso(permiso);
        }
    }
}