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
                throw new Exception("El perfil no es válido.");

            if (string.IsNullOrWhiteSpace(rol.Nombre))
                throw new Exception("Debe ingresar un nombre para el perfil.");

            rol.Nombre = rol.Nombre.Trim();

            if (rolDAL.ExisteRol(rol.Nombre))
                throw new Exception("Ya existe un perfil con ese nombre.");

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
                throw new Exception("Debe seleccionar un perfil.");

            if (permiso == null)
                throw new Exception("Debe seleccionar un permiso.");

            Rol rolCompleto = ObtenerCompleto(rol);

            if (rolCompleto == null)
                throw new Exception("El perfil seleccionado no existe.");

            List<Permiso> permisosActuales =
                ObtenerPermisosTotales(rolCompleto);

            Permiso repetido = permisosActuales.FirstOrDefault(p => p.Id == permiso.Id);

            if (repetido != null)
            {
                throw new Exception("No se puede asignar el permiso '" + repetido.Nombre + "' porque el perfil '" + rolCompleto.Nombre + "' ya lo posee directamente o mediante una familia.");
            }

            rolDAL.AsignarPermiso(rolCompleto, permiso);
        }
        public void AsignarFamilia(Rol rol, Familia familia)
        {
            if (rol == null)
                throw new Exception("Debe seleccionar un perfil.");

            if (familia == null)
                throw new Exception("Debe seleccionar una familia.");

            Rol rolCompleto = ObtenerCompleto(rol);

            Familia familiaCompleta = familiaBLL.ObtenerCompleta(familia);

            if (rolCompleto == null)
                throw new Exception("El perfil seleccionado no existe.");

            if (familiaCompleta == null)
                throw new Exception("La familia seleccionada no existe.");

            if (rolDAL.ExisteFamiliaEnRol(rolCompleto, familiaCompleta))
            {
                throw new Exception("La familia '" + familiaCompleta.Nombre + "' ya está asignada al perfil '" + rolCompleto.Nombre + "'.");
            }

            List<Permiso> permisosRol = ObtenerPermisosTotales(rolCompleto);

            List<Permiso> permisosFamilia = new List<Permiso>();

            ObtenerPermisosFamilia(familiaCompleta, permisosFamilia);

            Permiso repetido = permisosRol.FirstOrDefault(pr => permisosFamilia.Any(pf => pf.Id == pr.Id));

            if (repetido != null)
            {
                throw new Exception("No se puede asignar la familia '" + familiaCompleta.Nombre + "' al perfil '" + rolCompleto.Nombre + "' porque ambos contienen el permiso '" + repetido.Nombre + "'.");
            }

            rolDAL.AsignarFamilia(rolCompleto, familiaCompleta);
        }
        public void EliminarPermiso(Rol rol, Permiso permiso)
        {
            if (rol == null || permiso == null)
                throw new Exception("El componente seleccionado no es válido.");

            rolDAL.EliminarPermiso(rol, permiso);
        }

        public void EliminarFamilia(Rol rol, Familia familia)
        {
            if (rol == null || familia == null)
                throw new Exception("El componente seleccionado no es válido.");

            rolDAL.EliminarFamilia(rol, familia);
        }

        public void EliminarRol(Rol rol)
        {
            if (rol == null)
                throw new Exception(
                    "Debe seleccionar un perfil."
                );

            Rol completo = rolDAL.ObtenerRol(rol);

            if (completo == null)
                throw new Exception(
                    "El perfil seleccionado no existe."
                );

            if (rolDAL.EstaAsignadoAUsuarios(completo))
            {
                throw new Exception(
                    "No se puede eliminar el perfil '" +
                    completo.Nombre +
                    "' porque está asignado a uno o más usuarios."
                );
            }

            rolDAL.EliminarRol(completo);
        }
    }
}
