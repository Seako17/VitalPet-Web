using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGF_Finanzas.Capas.Servicios.Cambio_Idioma
{
    public interface IIdiomaObserver
    {
        void ActualizarIdioma(string codigoIdioma, IDictionary<string, string> traducciones);
    }

}
