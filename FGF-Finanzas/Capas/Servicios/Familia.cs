using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FGF_Finanzas.Capas.Servicios
{
    public class Familia : Componente
    {
        public Familia()
        {
            Componentes = new List<Componente>();
        }
        public List<Componente> Componentes { get; set; }
        public void Agregar(Componente componente)
        {
            Componentes.Add(componente);
        }
        public void Eliminar(Componente componente)
        {
            Componentes.Remove(componente);
        }
        public override bool TienePermiso(Permiso permiso)
        {
            return Componentes.Any(x => x.TienePermiso(permiso));
        }
    }
}