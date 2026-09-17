using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FGF_Finanzas.Capas.Servicios
{
    public static class CodigosPermiso
    {
        public static readonly Permiso BitacoraVer =
        new Permiso
        {
            Id = 1,
            Nombre = "BitacoraVer",
            Descripcion = "Ver la bitácora"
        };

        public static readonly Permiso UsuarioGestionar =
            new Permiso
            {
                Id = 2,
                Nombre = "UsuarioGestionar",
                Descripcion = "Gestionar usuarios"
            };

        public static readonly Permiso MascotaRegistrar =
            new Permiso
            {
                Id = 3,
                Nombre = "MascotaRegistrar",
                Descripcion = "Registrar mascotas"
            };

        public static readonly Permiso BackupRestore =
            new Permiso
            {
                Id = 4,
                Nombre = "BackUpRestore",
                Descripcion = "Realizar BackUp y Restore"
            };

        public static readonly Permiso CambiarContrasena =
            new Permiso
            {
                Id = 5,
                Nombre = "ContrasenaCambiar",
                Descripcion = "Cambiar la contraseña"
            };
    }
}