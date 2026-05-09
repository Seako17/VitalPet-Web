using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FGF_Finanzas.Capas.BLL
{
    public class BLLDigitoVerificador
    {
        private DALDigitoVerificador _dalDigito;
        public BLLDigitoVerificador()
        {
            _dalDigito = new DALDigitoVerificador();
        }

        public void GuardarDigitoVerificador(BEDigitoVerificador digito)
        {
            digito.DV_Horizontal = _dalDigito.CalcularDigitoVerificadorHorizontal(digito);
            digito.DV_Vertical = _dalDigito.CalcularDigitoVerificadorVertical(digito);
            _dalDigito.GuardarDigitoVerificador(digito);
        }

        public List<string> CompararDigito()
        {
            List<string> lista = new List<string>();
            foreach (BEDigitoVerificador item in _dalDigito.ObtenerTodos())
            {
                if (_dalDigito.CalcularDigitoVerificadorHorizontal(item) != item.DV_Horizontal || _dalDigito.CalcularDigitoVerificadorVertical(item) != item.DV_Vertical)
                {
                    lista.Add(item.NombreTabla);
                }
            }
            return lista;
        }

        public List<BEDigitoVerificador> ObtenerTodos()
        {
            return _dalDigito.ObtenerTodos();
        }
    }
}