using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.DAL;
using System.Collections.Generic;
using System.Web;


namespace FGF_Finanzas.Capas.Servicios.Cambio_Idioma
{
    public class IdiomaManager : IIdiomaSubject
    {
        private readonly List<IIdiomaObserver> _observadores = new List<IIdiomaObserver>();
        private readonly DALTraduccion _dalTraduccion = new DALTraduccion();

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
            if (!_observadores.Contains(observer))
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
                var textos = _dalTraduccion.ObtenerTraducciones(observer.NombreFormulario, IdiomaActual);
                observer.ActualizarIdioma(IdiomaActual, textos);
            }
        }
        public string ObtenerTexto(string formulario, string clave)
        {
            var textos = _dalTraduccion.ObtenerTraducciones(formulario, IdiomaActual);
            if (textos.TryGetValue(clave, out string texto))
                return texto;

            return $"[{clave}]";
        }

        public List<BEIdioma> ObtenerIdiomasDisponibles()
        {
            return _dalTraduccion.ObtenerIdiomasDisponibles();
        }
    }
}