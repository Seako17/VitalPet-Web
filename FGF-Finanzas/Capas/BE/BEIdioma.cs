using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FGF_Finanzas.Capas.BE
{
    public class BEIdioma
    {
        public int IdIdioma { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }

        public BEIdioma() { }

        public BEIdioma(string codigo, string nombre)
        {
            Codigo = codigo;
            Nombre = nombre;
        }

        public BEIdioma(int idIdioma, string codigo, string nombre)
        {
            IdIdioma = idIdioma;
            Codigo = codigo;
            Nombre = nombre;
        }
    }
}