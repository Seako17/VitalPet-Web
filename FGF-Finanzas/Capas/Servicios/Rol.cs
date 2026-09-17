using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace FGF_Finanzas.Capas.Servicios
{
    public class Rol
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public List<Componente> Componentes { get; set; }
        public Rol()
        {
            Componentes = new List<Componente>();
        }
        public Rol(int id)
        {
            this.Id = id;
        }
        public bool TienePermiso(Permiso permiso)
        {
            return Componentes.Any(x => x.TienePermiso(permiso));
        }
    }
}