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

        public List<BEEtiqueta> ObtenerEtiquetas(string formulario)
        {
            return _dalTraduccion.ObtenerEtiquetas(formulario);
        }

        public BEIdioma ObtenerIdioma(string codigo)
        {
            return _dalTraduccion.ObtenerIdiomaPorCodigo(codigo);
        }

        public DataTable ObtenerEtiquetasConTraduccion(string formulario, string codigoIdioma)
        {
            if (string.IsNullOrWhiteSpace(formulario))
                throw new Exception("Debe seleccionar un formulario.");

            BEIdioma idioma = ObtenerIdiomaSeleccionado(codigoIdioma);
            return _dalTraduccion.ObtenerEtiquetasConTraduccion(formulario, idioma.IdIdioma);
        }

        public void ActualizarTraduccion(string formulario, string codigoIdioma, int idEtiqueta, string texto)
        {
            if (string.IsNullOrWhiteSpace(formulario))
                throw new Exception("Debe seleccionar un formulario.");

            BEIdioma idioma = ObtenerIdiomaSeleccionado(codigoIdioma);

            BEEtiqueta etiqueta = _dalTraduccion
                .ObtenerEtiquetas(formulario)
                .FirstOrDefault(x => x.IdEtiqueta == idEtiqueta);

            if (etiqueta == null)
                throw new Exception("La etiqueta seleccionada no pertenece al formulario indicado.");

            _dalTraduccion.GuardarTraduccion(new BETraduccion(idioma.IdIdioma, etiqueta.IdEtiqueta, texto ?? string.Empty));

            RegistrarEvento("Editar Traduccion", 2);
        }

        public BEIdioma CrearIdioma(string codigo, string nombre)
        {
            codigo = (codigo ?? string.Empty).Trim();
            nombre = (nombre ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(codigo))
                throw new Exception("El código del idioma es obligatorio.");

            if (string.IsNullOrWhiteSpace(nombre))
                throw new Exception("El nombre del idioma es obligatorio.");

            if (nombre.Length > 50)
                throw new Exception("El nombre del idioma no puede superar los 50 caracteres.");

            if (!Regex.IsMatch(codigo, @"^[a-zA-Z]{2,3}(-[a-zA-Z]{2,4})?$"))
                throw new Exception("El código debe tener el formato 'es-AR' o 'en-US' (por ejemplo: pt-BR).");

            if (_dalTraduccion.ObtenerIdiomaPorCodigo(codigo) != null)
                throw new Exception($"El código de idioma '{codigo}' ya se encuentra registrado.");

            BEIdioma origen = _dalTraduccion.ObtenerIdiomaPorCodigo(IDIOMA_POR_DEFECTO);
            if (origen == null)
                throw new Exception($"No se encontró el idioma por defecto '{IDIOMA_POR_DEFECTO}' necesario para copiar las traducciones.");

            int idIdiomaNuevo = _dalTraduccion.AgregarIdioma(new BEIdioma(codigo, nombre));
            _dalTraduccion.CopiarTraducciones(origen.IdIdioma, idIdiomaNuevo);

            RegistrarEvento("Crear Idioma", 4);

            return new BEIdioma(idIdiomaNuevo, codigo, nombre);
        }

        public void EliminarIdioma(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new Exception("Debe seleccionar un idioma.");

            BEIdioma idioma = _dalTraduccion.ObtenerIdiomaPorCodigo(codigo);
            if (idioma == null)
                throw new Exception("El idioma que intenta eliminar no existe.");

            if (idioma.Codigo.Equals(IDIOMA_POR_DEFECTO, StringComparison.OrdinalIgnoreCase))
                throw new Exception($"No se puede eliminar el idioma por defecto '{IDIOMA_POR_DEFECTO}'.");

            if (_dalTraduccion.ObtenerIdiomas().Count <= 1)
                throw new Exception("No se puede eliminar el idioma porque es el único registrado.");

            int usuariosAsociados = _dalTraduccion.ContarUsuariosConIdioma(idioma.Codigo);
            if (usuariosAsociados > 0)
                throw new Exception($"No se puede eliminar el idioma porque {usuariosAsociados} usuario(s) lo tienen asignado como idioma preferido.");

            _dalTraduccion.EliminarIdioma(idioma.IdIdioma);

            RegistrarEvento("Eliminar Idioma", 2);
        }

        private BEIdioma ObtenerIdiomaSeleccionado(string codigoIdioma)
        {
            if (string.IsNullOrWhiteSpace(codigoIdioma))
                throw new Exception("Debe seleccionar un idioma.");

            BEIdioma idioma = _dalTraduccion.ObtenerIdiomaPorCodigo(codigoIdioma);
            if (idioma == null)
                throw new Exception($"El idioma '{codigoIdioma}' no se encuentra registrado.");

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
