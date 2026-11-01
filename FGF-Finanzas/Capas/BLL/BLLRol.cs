using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.DAL;
using FGF_Finanzas.Capas.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FGF_Finanzas.Capas.BLL
{
    public class BLLRol
    {
        private DALRol rolDAL = new DALRol();
        private DALPermiso permisoDAL = new DALPermiso();
        private DALFamilia familiaDAL = new DALFamilia();
        private BLLFamilia familiaBLL = new BLLFamilia();

        public Rol ObtenerCompleto(Rol rol)
        {
            Rol rolCompleto = rolDAL.ObtenerRol(rol);

            if (rolCompleto == null)
                return null;

            CargarComponentes(rolCompleto);

            return rolCompleto;
        }

        public List<Rol> ObtenerRoles()
        {
            List<Rol> roles = rolDAL.ObtenerRoles();

            foreach (Rol rol in roles)
            {
                CargarComponentes(rol);
            }

            return roles;
        }

        private void CargarComponentes(Rol rol)
        {
            List<Permiso> permisos = permisoDAL.ListarPorRol(rol);

            foreach (Permiso permiso in permisos)
            {
                rol.Componentes.Add(permiso);
            }

            List<Familia> familias = familiaDAL.ListarPorRol(rol);

            foreach (Familia familia in familias)
            {
                Familia completa = familiaBLL.ObtenerCompleta(familia);

                rol.Componentes.Add(completa);
            }
        }
        public void AgregarRol(Rol rol)
        {
            if (rol == null)
                throw new BECustomException("ERR_PERFIL_INVALIDO");

            if (string.IsNullOrWhiteSpace(rol.Nombre))
                throw new BECustomException("ERR_PERFIL_NOMBRE_OBLIGATORIO");

            rol.Nombre = rol.Nombre.Trim();

            if (rolDAL.ExisteRol(rol.Nombre))
                throw new BECustomException("ERR_PERFIL_NOMBRE_DUPLICADO");

            rolDAL.AgregarRol(rol);
        }
        private List<Permiso> ObtenerPermisosTotales(Rol rol)
        {
            Rol completo = ObtenerCompleto(rol);

            List<Permiso> permisos = new List<Permiso>();

            if (completo == null)
                return permisos;

            foreach (Componente componente in completo.Componentes)
            {
                if (componente is Permiso permiso)
                {
                    if (!permisos.Any(p => p.Id == permiso.Id))
                    {
                        permisos.Add(permiso);
                    }
                }
                else if (componente is Familia familia)
                {
                    ObtenerPermisosFamilia(familia, permisos);
                }
            }

            return permisos;
        }

        private void ObtenerPermisosFamilia(Familia familia, List<Permiso> permisos)
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
                else if (componente is Familia hija)
                {
                    ObtenerPermisosFamilia(hija, permisos);
                }
            }
        }
        public void AsignarPermiso(Rol rol, Permiso permiso)
        {
            if (rol == null)
                throw new BECustomException("ERR_PERFIL_SELECCIONAR");

            if (permiso == null)
                throw new BECustomException("ERR_PERFIL_SELECCIONAR_PERMISO");

            Rol rolCompleto = ObtenerCompleto(rol);

            if (rolCompleto == null)
                throw new BECustomException("ERR_PERFIL_NO_EXISTE");

            List<Permiso> permisosActuales =
                ObtenerPermisosTotales(rolCompleto);

            Permiso repetido = permisosActuales.FirstOrDefault(p => p.Id == permiso.Id);

            if (repetido != null)
            {
                throw new BECustomException("ERR_PERFIL_PERMISO_REPETIDO", repetido.Nombre, rolCompleto.Nombre);
            }

            rolDAL.AsignarPermiso(rolCompleto, permiso);
        }
        public void AsignarFamilia(Rol rol, Familia familia)
        {
            if (rol == null)
                throw new BECustomException("ERR_PERFIL_SELECCIONAR");

            if (familia == null)
                throw new BECustomException("ERR_PERFIL_SELECCIONAR_FAMILIA");

            Rol rolCompleto = ObtenerCompleto(rol);

            Familia familiaCompleta = familiaBLL.ObtenerCompleta(familia);

            if (rolCompleto == null)
                throw new BECustomException("ERR_PERFIL_NO_EXISTE");

            if (familiaCompleta == null)
                throw new BECustomException("ERR_PERFIL_FAMILIA_NO_EXISTE");

            if (rolDAL.ExisteFamiliaEnRol(rolCompleto, familiaCompleta))
            {
                throw new BECustomException("ERR_PERFIL_FAMILIA_ASIGNADA", familiaCompleta.Nombre, rolCompleto.Nombre);
            }

            List<Permiso> permisosRol = ObtenerPermisosTotales(rolCompleto);

            List<Permiso> permisosFamilia = new List<Permiso>();

            ObtenerPermisosFamilia(familiaCompleta, permisosFamilia);

            Permiso repetido = permisosRol.FirstOrDefault(pr => permisosFamilia.Any(pf => pf.Id == pr.Id));

            if (repetido != null)
            {
                throw new BECustomException("ERR_PERFIL_FAMILIA_PERMISO_REPETIDO", familiaCompleta.Nombre, rolCompleto.Nombre, repetido.Nombre);
            }

            rolDAL.AsignarFamilia(rolCompleto, familiaCompleta);
        }
        public void EliminarPermiso(Rol rol, Permiso permiso)
        {
            if (rol == null || permiso == null)
                throw new BECustomException("ERR_PERFIL_COMPONENTE_INVALIDO");

            rolDAL.EliminarPermiso(rol, permiso);
        }

        public void EliminarFamilia(Rol rol, Familia familia)
        {
            if (rol == null || familia == null)
                throw new BECustomException("ERR_PERFIL_COMPONENTE_INVALIDO");

            rolDAL.EliminarFamilia(rol, familia);
        }

        public void EliminarRol(Rol rol)
        {
            if (rol == null)
                throw new BECustomException("ERR_PERFIL_SELECCIONAR");

            Rol completo = rolDAL.ObtenerRol(rol);

            if (completo == null)
                throw new BECustomException("ERR_PERFIL_NO_EXISTE");

            if (rolDAL.EstaAsignadoAUsuarios(completo))
            {
                throw new BECustomException("ERR_PERFIL_ELIMINAR_ASIGNADO", completo.Nombre);
            }

            rolDAL.EliminarRol(completo);
        }
    }
}
