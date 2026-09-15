using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FGF_Finanzas.Capas.BE
{
    public class BECustomException : Exception
    {
        public string CodigoError { get; }

        public BECustomException(string codigoError): base(codigoError)
        {
            CodigoError = codigoError;
        }
    }
}