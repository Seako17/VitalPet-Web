using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FGF_Finanzas.Capas.BE
{
    public class BEDigitoVerificador
    {
        public string NombreTabla { get; set; }
        public string DV_Horizontal { get; set; }
        public string DV_Vertical { get; set; }

        public BEDigitoVerificador() { }
        public BEDigitoVerificador(string nombreTabla)
        {
            NombreTabla = nombreTabla;
        }
        public BEDigitoVerificador(string nombreTabla, string dvHorizontal, string dvVertical)
        {
            NombreTabla = nombreTabla;
            DV_Horizontal = dvHorizontal;
            DV_Vertical = dvVertical;
        }
    }
}