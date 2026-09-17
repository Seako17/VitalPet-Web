using FGF_Finanzas.Capas.DAL;
using FGF_Finanzas.Capas.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FGF_Finanzas.Capas.BLL
{
    public class BLLFamilia
    {
        private DALFamilia familiaDAL = new DALFamilia();
        private DALPermiso permisoDAL = new DALPermiso();

        public Familia ObtenerCompleta(Familia fam)
        {
            Familia familia = familiaDAL.ObtenerFamilia(fam);

            if (familia == null)
                return null;

            CargarComponentes(familia);

            return familia;
        }

        private void CargarComponentes(Familia familia)
        {
            List<Permiso> permisos = permisoDAL.ListarPorFamilia(familia);

            foreach (Permiso permiso in permisos)
            {
                familia.Agregar(permiso);
            }

            List<Familia> familiasHijas =
                familiaDAL.ListarHijas(familia);

            foreach (Familia hija in familiasHijas)
            {
                CargarComponentes(hija);

                familia.Agregar(hija);
            }
        }
    }
}