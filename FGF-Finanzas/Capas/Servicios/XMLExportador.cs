using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

namespace FGF_Finanzas.Capas.Servicios
{
    public class XMLExportador
    {
        public void Exportar(DataTable dt, string ruta)
        {
            dt.TableName = "Usuarios";

            dt.WriteXml(ruta, XmlWriteMode.IgnoreSchema);
        }

        public DataTable Importar(Stream xml)
        {
            DataSet ds = new DataSet();

            XmlReaderSettings settings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Prohibit,
                XmlResolver = null
            };

            using (XmlReader reader = XmlReader.Create(xml, settings))
            {
                ds.ReadXml(reader);
            }

            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }

            return new DataTable();
        }
    }
}