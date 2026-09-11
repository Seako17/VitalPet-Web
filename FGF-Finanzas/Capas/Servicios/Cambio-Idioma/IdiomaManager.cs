
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FGF_Finanzas.Capas.Servicios.Cambio_Idioma
{
    public class IdiomaManager : IIdiomaSubject
    {
        private readonly List<IIdiomaObserver> _observadores = new List<IIdiomaObserver>();
        private readonly TraduccionService _traduccionService = new TraduccionService();
        public void Desuscribir(IIdiomaObserver observer)
        {
            throw new NotImplementedException();
        }

        public void Notificar()
        {
            throw new NotImplementedException();
        }

        public void Suscribir(IIdiomaObserver observer)
        {
            throw new NotImplementedException();
        }
    }
}