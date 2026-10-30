using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FGF_Finanzas.Capas.BE
{
    public class BEAtencion
    {
        public int id_atencion { get; set; }
        public int id_consulta { get; set; }
        public string diagnostico { get; set; }
        public string observaciones { get; set; }
        public bool atendido { get; set; }
        public BEAtencion() { }
        public BEAtencion(int consulta, string observaciones, string tratamiento)
        {
            id_consulta = consulta;
            this.observaciones = observaciones;
            diagnostico = tratamiento;
            atendido = false;
        }
    }
}