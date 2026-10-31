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
            DataTable exportable = new DataTable("Usuarios");
            foreach (DataColumn columna in dt.Columns)
            {
                Type tipo = columna.DataType == typeof(Rol) ? typeof(string) : columna.DataType;
                exportable.Columns.Add(columna.ColumnName, tipo);
            }

            foreach (DataRow fila in dt.Rows)
            {
                DataRow nuevaFila = exportable.NewRow();
                foreach (DataColumn columna in dt.Columns)
                {
                    object valor = fila[columna];
                    if (columna.DataType == typeof(Rol))
                    {
                        Rol rol = valor == DBNull.Value ? null : valor as Rol;
                        nuevaFila[columna.ColumnName] = rol == null ? (object)DBNull.Value : rol.Nombre;
                    }
                    else
                    {
                        nuevaFila[columna.ColumnName] = valor;
                    }
                }
                exportable.Rows.Add(nuevaFila);
            }

            exportable.WriteXml(ruta, XmlWriteMode.IgnoreSchema);
        }

        public DataTable Importar(Stream xml)
        {
            XmlReaderSettings settings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Prohibit,
                XmlResolver = null
            };

            XmlDocument documento = new XmlDocument();
            using (XmlReader reader = XmlReader.Create(xml, settings))
            {
                documento.Load(reader);
            }

            DataTable tablaUsuarios = new DataTable("Usuarios");
            XmlNodeList nodos = documento.SelectNodes("//*");
            List<XmlElement> filasUsuario = nodos.Cast<XmlNode>()
                .OfType<XmlElement>()
                .Where(nodo => nodo.ChildNodes.Cast<XmlNode>()
                    .OfType<XmlElement>()
                    .Any(hijo => hijo.LocalName.Equals("DNI", StringComparison.OrdinalIgnoreCase)))
                .ToList();

            string[] columnasRequeridas =
            {
                "DNI", "Nombre", "Apellido", "Usuario", "Contraseña",
                "Intento", "Bloqueado", "Mail", "Rol"
            };

            foreach (XmlElement filaUsuario in filasUsuario)
            {
                List<XmlElement> campos = filaUsuario.ChildNodes.Cast<XmlNode>()
                    .OfType<XmlElement>()
                    .ToList();
                HashSet<string> nombresCampos = new HashSet<string>(
                    campos.Select(campo => campo.LocalName),
                    StringComparer.OrdinalIgnoreCase);
                string[] faltantes = columnasRequeridas
                    .Where(columna => !nombresCampos.Contains(columna))
                    .ToArray();

                if (faltantes.Length > 0)
                {
                    XmlElement dni = campos.FirstOrDefault(campo =>
                        campo.LocalName.Equals("DNI", StringComparison.OrdinalIgnoreCase));
                    string identificador = dni == null ? "sin DNI" : "con DNI " + dni.InnerText;
                    throw new Exception("El registro " + identificador + " no contiene las columnas: " +
                        string.Join(", ", faltantes) + ".");
                }

                foreach (XmlElement campo in campos)
                {
                    if (!tablaUsuarios.Columns.Contains(campo.LocalName))
                        tablaUsuarios.Columns.Add(campo.LocalName, typeof(string));
                }
            }

            foreach (XmlElement filaUsuario in filasUsuario)
            {
                DataRow fila = tablaUsuarios.NewRow();
                foreach (XmlElement campo in filaUsuario.ChildNodes.Cast<XmlNode>().OfType<XmlElement>())
                    fila[campo.LocalName] = campo.InnerText;

                tablaUsuarios.Rows.Add(fila);
            }

            return tablaUsuarios;
        }
    }
}
