using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.DAL;
using FGF_Finanzas.Capas.Servicios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace FGF_Finanzas.Capas.BLL
{
    public class BLLTraduccion
    {
        public const string IDIOMA_POR_DEFECTO = "es-AR";

        private readonly DALTraduccion _dalTraduccion;
        private readonly BLLEvento _bllEvento;

        public BLLTraduccion()
        {
            _dalTraduccion = new DALTraduccion();
            _bllEvento = new BLLEvento();
        }

        public List<BEIdioma> ObtenerIdiomas()
        {
            return _dalTraduccion.ObtenerIdiomas();
        }

        public List<string> ObtenerFormularios()
        {
            return _dalTraduccion.ObtenerFormularios();
        }

        public DataTable ObtenerEtiquetasConTraduccion(string formulario, string codigoIdioma)
        {
            if (string.IsNullOrWhiteSpace(formulario))
                throw new BECustomException("ERR_FORMULARIO_OBLIGATORIO");

            BEIdioma idioma = ObtenerIdiomaSeleccionado(codigoIdioma);
            return _dalTraduccion.ObtenerEtiquetasConTraduccion(formulario, idioma.IdIdioma);
        }

        public void ActualizarTraduccion(string formulario, string codigoIdioma, int idEtiqueta, string texto)
        {
            if (string.IsNullOrWhiteSpace(formulario))
                throw new BECustomException("ERR_FORMULARIO_OBLIGATORIO");

            BEIdioma idioma = ObtenerIdiomaSeleccionado(codigoIdioma);

            BEEtiqueta etiqueta = _dalTraduccion
                .ObtenerEtiquetas(formulario)
                .FirstOrDefault(x => x.IdEtiqueta == idEtiqueta);

            if (etiqueta == null)
                throw new BECustomException("ERR_ETIQUETA_NO_PERTENECE");

            _dalTraduccion.GuardarTraduccion(new BETraduccion(idioma.IdIdioma, etiqueta.IdEtiqueta, texto ?? string.Empty));

            RegistrarEvento("Editar Traduccion", 2);
        }

        public BEIdioma CrearIdioma(string codigo, string nombre)
        {
            codigo = (codigo ?? string.Empty).Trim();
            nombre = (nombre ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(codigo))
                throw new BECustomException("ERR_CODIGO_IDIOMA_OBLIGATORIO");

            if (string.IsNullOrWhiteSpace(nombre))
                throw new BECustomException("ERR_NOMBRE_IDIOMA_OBLIGATORIO");

            if (nombre.Length > 50)
                throw new BECustomException("ERR_NOMBRE_IDIOMA_LARGO");

            if (!Regex.IsMatch(codigo, @"^[a-zA-Z]{2,3}(-[a-zA-Z]{2,4})?$"))
                throw new BECustomException("ERR_CODIGO_IDIOMA_FORMATO");

            if (_dalTraduccion.ObtenerIdiomaPorCodigo(codigo) != null)
                throw new BECustomException("ERR_CODIGO_IDIOMA_DUPLICADO", codigo);

            BEIdioma origen = _dalTraduccion.ObtenerIdiomaPorCodigo(IDIOMA_POR_DEFECTO);
            if (origen == null)
                throw new BECustomException("ERR_IDIOMA_POR_DEFECTO_FALTANTE", IDIOMA_POR_DEFECTO);

            int idIdiomaNuevo = _dalTraduccion.AgregarIdioma(new BEIdioma(codigo, nombre));
            _dalTraduccion.CopiarTraducciones(origen.IdIdioma, idIdiomaNuevo);

            RegistrarEvento("Crear Idioma", 4);

            return new BEIdioma(idIdiomaNuevo, codigo, nombre);
        }

        public void EliminarIdioma(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new BECustomException("ERR_IDIOMA_NO_SELECCIONADO");

            BEIdioma idioma = _dalTraduccion.ObtenerIdiomaPorCodigo(codigo);
            if (idioma == null)
                throw new BECustomException("ERR_IDIOMA_NO_EXISTE");

            if (idioma.Codigo.Equals(IDIOMA_POR_DEFECTO, StringComparison.OrdinalIgnoreCase))
                throw new BECustomException("ERR_IDIOMA_POR_DEFECTO", IDIOMA_POR_DEFECTO);

            _dalTraduccion.EliminarIdioma(idioma.IdIdioma);

            RegistrarEvento("Eliminar Idioma", 2);
        }

        private BEIdioma ObtenerIdiomaSeleccionado(string codigoIdioma)
        {
            if (string.IsNullOrWhiteSpace(codigoIdioma))
                throw new BECustomException("ERR_IDIOMA_NO_SELECCIONADO");

            BEIdioma idioma = _dalTraduccion.ObtenerIdiomaPorCodigo(codigoIdioma);
            if (idioma == null)
                throw new BECustomException("ERR_IDIOMA_NO_REGISTRADO", codigoIdioma);

            return idioma;
        }

        private void RegistrarEvento(string evento, int criticidad)
        {
            if (SessionManager.IsLogged())
            {
                _bllEvento.AgregarEvento(new BEEvento(SessionManager.Instancia.Usuario, DateTime.Now, "Idiomas", evento, criticidad));
            }
        }
    }
}
