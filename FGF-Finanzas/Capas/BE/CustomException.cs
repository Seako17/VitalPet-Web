using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FGF_Finanzas.Capas.BE
{
    public class CustomException : Exception
    {
        public string CodigoError { get; }

        public CustomException(string codigoError): base(codigoError)
        {
            CodigoError = codigoError;
        }
    }
}