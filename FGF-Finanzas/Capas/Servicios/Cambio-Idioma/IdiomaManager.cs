using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.BLL;
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
            get => HttpContext.Current.Session["IdiomaActual"]?.ToString() ?? BLLTraduccion.IDIOMA_POR_DEFECTO;
            set
            {
                BEIdioma idioma = string.IsNullOrWhiteSpace(value) ? null : _dalTraduccion.ObtenerIdiomaPorCodigo(value);
                HttpContext.Current.Session["IdiomaActual"] = idioma != null ? idioma.Codigo : BLLTraduccion.IDIOMA_POR_DEFECTO;
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
            return ObtenerTexto(formulario, clave, IdiomaActual);
        }

        public string ObtenerTexto(string formulario, string clave, string codigoIdioma)
        {
            var textos = _dalTraduccion.ObtenerTraducciones(formulario, codigoIdioma);
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