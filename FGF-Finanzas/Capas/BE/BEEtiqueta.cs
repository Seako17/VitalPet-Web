using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FGF_Finanzas.Capas.BE
{
    public class BEEtiqueta
    {
        public int IdEtiqueta { get; set; }
        public string Formulario { get; set; }
        public string ControlId { get; set; }

        public BEEtiqueta() { }

        public BEEtiqueta(string formulario, string controlId)
        {
            Formulario = formulario;
            ControlId = controlId;
        }

        public BEEtiqueta(int idEtiqueta, string formulario, string controlId)
        {
            IdEtiqueta = idEtiqueta;
            Formulario = formulario;
            ControlId = controlId;
        }
    }
}
