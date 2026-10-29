using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGF_Finanzas.Capas.Servicios.Cambio_Idioma
{
    public interface IIdiomaSubject
    {
        void Suscribir(IIdiomaObserver observer);
        void Desuscribir(IIdiomaObserver observer);
        void Notificar();
    }
}
