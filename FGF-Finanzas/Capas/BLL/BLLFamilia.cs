using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.DAL;
using FGF_Finanzas.Capas.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public void AgregarFamilia(Familia familia)
        {
            if (familia == null)
                throw new BECustomException("ERR_FAMILIA_INVALIDA");

            if (string.IsNullOrWhiteSpace(familia.Nombre))
                throw new BECustomException("ERR_FAMILIA_NOMBRE_OBLIGATORIO");

            familia.Nombre = familia.Nombre.Trim();

            if (familiaDAL.ExisteFamilia(familia.Nombre))
                throw new BECustomException("ERR_FAMILIA_NOMBRE_DUPLICADO");

            familiaDAL.AgregarFamilia(familia);
        }
        public void AsignarPermiso(Familia familia,Permiso permiso)
        {
            if (familia == null)
                throw new BECustomException("ERR_FAMILIA_SELECCIONAR");

            if (permiso == null)
                throw new BECustomException("ERR_FAMILIA_SELECCIONAR_PERMISO");

            Familia familiaCompleta = ObtenerCompleta(familia);

            if (familiaCompleta == null)
                throw new BECustomException("ERR_FAMILIA_NO_EXISTE");

            List<Permiso> permisosActuales = ObtenerPermisosTotales(familiaCompleta);

            Permiso permisoRepetido = permisosActuales.FirstOrDefault(p => p.Id == permiso.Id);

            if (permisoRepetido != null)
            {
                throw new BECustomException("ERR_FAMILIA_PERMISO_REPETIDO", permisoRepetido.Nombre, familiaCompleta.Nombre);
            }

            List<Familia> ancestros = new List<Familia>();

            ObtenerFamiliasAncestro(familiaCompleta, ancestros);

            foreach (Familia ancestro in ancestros)
            {
                List<Permiso> permisosAncestro = ObtenerPermisosTotales(ancestro);

                Permiso repetidoEnAncestro = permisosAncestro.FirstOrDefault(p => p.Id == permiso.Id);

                if (repetidoEnAncestro != null)
                {
                    throw new BECustomException("ERR_FAMILIA_PERMISO_EN_ANCESTRO", repetidoEnAncestro.Nombre, familiaCompleta.Nombre, ancestro.Nombre);
                }
            }

            familiaDAL.AsignarPermiso(familiaCompleta, permiso);
        }
        public void EliminarPermiso(Familia familia, Permiso permiso)
        {
            if (familia == null || permiso == null)
                throw new BECustomException("ERR_FAMILIA_COMPONENTE_INVALIDO");

            familiaDAL.EliminarPermiso(familia, permiso);
        }
        public List<Familia> ObtenerFamiliasRaiz()
        {
            List<Familia> familias = familiaDAL.ObtenerFamiliasRaiz();

            foreach (Familia familia in familias)
            {
                CargarComponentes(familia);
            }

            return familias;
        }
        public void AsignarFamilia(Familia padre, Familia hija)
        {
            if (padre == null || hija == null)
                throw new BECustomException("ERR_FAMILIA_SELECCIONAR_AMBAS");

            Familia padreCompleta = ObtenerCompleta(padre);
            Familia hijaCompleta = ObtenerCompleta(hija);

            if (padreCompleta == null)
                throw new BECustomException("ERR_FAMILIA_BASE_NO_EXISTE");

            if (hijaCompleta == null)
                throw new BECustomException("ERR_FAMILIA_AGREGAR_NO_EXISTE");

            if (padreCompleta.Id == hijaCompleta.Id)
                throw new BECustomException("ERR_FAMILIA_AUTOASIGNAR");

            if (familiaDAL.ExisteRelacionFamilias(padreCompleta,hijaCompleta))
            {
                throw new BECustomException("ERR_FAMILIA_YA_PERTENECE", hijaCompleta.Nombre, padreCompleta.Nombre);
            }

            if (ContieneFamilia(hijaCompleta, padreCompleta.Id))
            {
                throw new BECustomException("ERR_FAMILIA_CICLO", hijaCompleta.Nombre, padreCompleta.Nombre);
            }

            List<Permiso> permisosPadre = ObtenerPermisosTotales(padreCompleta);

            List<Permiso> permisosHija = ObtenerPermisosTotales(hijaCompleta);

            Permiso permisoRepetido = permisosPadre.FirstOrDefault(pp => permisosHija.Any(ph => ph.Id == pp.Id));

            if (permisoRepetido != null)
            {
                throw new BECustomException("ERR_FAMILIA_PERMISO_REPETIDO_ENTRE", hijaCompleta.Nombre, padreCompleta.Nombre, permisoRepetido.Nombre);
            }

            familiaDAL.AsignarFamilia(padreCompleta,hijaCompleta);
        }
        private bool ContieneFamilia(Familia familia, int idBuscado)
        {
            if (familia.Id == idBuscado)
                return true;

            foreach (Componente componente in familia.Componentes)
            {
                if (componente is Familia familiaHija)
                {
                    if (ContieneFamilia(familiaHija, idBuscado))
                        return true;
                }
            }

            return false;
        }
        public void EliminarRelacionFamilias(Familia padre, Familia hija)
        {
            if (padre == null || hija == null)
                throw new BECustomException("ERR_FAMILIA_RELACION_INVALIDA");

            familiaDAL.EliminarRelacionFamilias(padre,hija);
        }
        public void EliminarFamilia(Familia familia)
        {
            if (familia == null)
                throw new BECustomException("ERR_FAMILIA_SELECCIONAR");

            familiaDAL.EliminarFamilia(familia);
        }

        public List<Familia> ObtenerFamilias()
        {
            List<Familia> familias = familiaDAL.ObtenerFamilias();

            foreach (Familia familia in familias)
            {
                CargarComponentes(familia);
            }

            return familias;
        }

        private void CargarComponentes(Familia familia)
        {
            List<Permiso> permisos = permisoDAL.ListarPorFamilia(familia);

            foreach (Permiso permiso in permisos)
            {
                familia.Agregar(permiso);
            }

            List<Familia> familiasHijas = familiaDAL.ListarHijas(familia);

            foreach (Familia hija in familiasHijas)
            {
                CargarComponentes(hija);

                familia.Agregar(hija);
            }
        }
        private List<Permiso> ObtenerPermisosTotales(Familia familia)
        {
            List<Permiso> permisos = new List<Permiso>();

            Familia familiaCompleta = ObtenerCompleta(familia);

            if (familiaCompleta == null)
                return permisos;

            ObtenerPermisosRecursivos(familiaCompleta, permisos);

            return permisos;
        }
        private void ObtenerFamiliasAncestro(Familia familia, List<Familia> ancestros)
        {
            List<Familia> padres = familiaDAL.ListarPadres(familia);

            foreach (Familia padre in padres)
            {
                if (!ancestros.Any(x => x.Id == padre.Id))
                {
                    ancestros.Add(padre);

                    ObtenerFamiliasAncestro(padre,ancestros);
                }
            }
        }
        private void ObtenerPermisosRecursivos(Familia familia, List<Permiso> permisos)
        {
            foreach (Componente componente in familia.Componentes)
            {
                if (componente is Permiso permiso)
                {
                    if (!permisos.Any(p => p.Id == permiso.Id))
                    {
                        permisos.Add(permiso);
                    }
                }
                else if (componente is Familia familiaHija)
                {
                    ObtenerPermisosRecursivos(familiaHija, permisos);
                }
            }
        }
    }
}
