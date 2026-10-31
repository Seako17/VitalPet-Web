using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FGF_Finanzas.Capas.Servicios
{
    public class Permiso : Componente
    {
        public override bool TienePermiso(Permiso permiso)
        {
            return permiso.Id == this.Id;
        }
    }
}