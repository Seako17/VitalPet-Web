
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
        
        public static IdiomaManager Instancia
        {
            get
            {
                if (HttpContext.Current.Session["IdiomaManager"] == null)
                {
                    HttpContext.Current.Session["IdiomaManager"] = new IdiomaManager();
                }
                return (IdiomaManager)HttpContext.Current.Session["IdiomaManager"];
            }
        }

        public string IdiomaActual
        {
            get => HttpContext.Current.Session["IdiomaActual"]?.ToString() ?? "es-AR";
            set
            {
                HttpContext.Current.Session["IdiomaActual"] = value;
                Notificar();
            }
        }
        public void Suscribir(IIdiomaObserver observer)
        {
            if(!_observadores.Contains(observer))
                _observadores.Add(observer);
        }
        public void Desuscribir(IIdiomaObserver observer)
        {
            _observadores.Remove(observer);
        }

        public void Notificar()
        {
            foreach (var observer in _observadores) 
            {
                if(observer is BasePage page)
                {
                    var textos = _traduccionService.ObtenerTraducciones(page.NombreFormulario, IdiomaActual);
                    observer.ActualizarIdioma(IdiomaActual, textos);
                }
            }
        }

    }
}