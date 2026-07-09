using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FGF_Finanzas.Capas.BE
{
    public class BEDigitoVerificador
    {
        public string NombreTabla { get; set; }
        public string DV_Vertical { get; set; }
        public int CantidadRegistros { get; set; }

        public BEDigitoVerificador() { }
        public BEDigitoVerificador(string nombreTabla)
        {
            NombreTabla = nombreTabla;
        }
        public BEDigitoVerificador(string nombreTabla, string dvVertical, int cantRegistros)
        {
            NombreTabla = nombreTabla;
            DV_Vertical = dvVertical;
            CantidadRegistros = cantRegistros;
        }
    }

    public class FilaGenerica
    {
        public string Id { get; set; }
        public string DV_HorizontalGuardado { get; set; }
        public List<object> ValoresCampos { get; set; } = new List<object>();
    }

    public class InconsistenciaReporte
    {
        public string NombreTabla { get; set; }
        public string IdRegistro { get; set; }
        public string TipoFalla { get; set; }
    }
}