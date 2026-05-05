using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace FGF_Finanzas.Capas.DAL
{
    public abstract class DALAbstracta
    {
        protected string _conexion = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;

    }
}