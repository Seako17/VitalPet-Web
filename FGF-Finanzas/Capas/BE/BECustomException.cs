using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FGF_Finanzas.Capas.BE
{
    public class BECustomException : Exception
    {
        public string CodigoError { get; }
        public object[] Argumentos { get; }

        public BECustomException(string codigoError, params object[] argumentos)
            : base(codigoError)
        {
            CodigoError = codigoError;
            Argumentos = argumentos ?? new object[0];
        }

        public BECustomException(string codigoError, Exception innerException, params object[] argumentos)
            : base(codigoError, innerException)
        {
            CodigoError = codigoError;
            Argumentos = argumentos ?? new object[0];
        }
    }
}