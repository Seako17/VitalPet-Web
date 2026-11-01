using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FGF_Finanzas.Capas.BE
{
    public class BETraduccion
    {
        public int IdTraduccion { get; set; }
        public int IdIdioma { get; set; }
        public int IdEtiqueta { get; set; }
        public string Texto { get; set; }

        public BETraduccion() { }

        public BETraduccion(int idIdioma, int idEtiqueta, string texto)
        {
            IdIdioma = idIdioma;
            IdEtiqueta = idEtiqueta;
            Texto = texto;
        }

        public BETraduccion(int idTraduccion, int idIdioma, int idEtiqueta, string texto)
        {
            IdTraduccion = idTraduccion;
            IdIdioma = idIdioma;
            IdEtiqueta = idEtiqueta;
            Texto = texto;
        }
    }
}
