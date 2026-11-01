using System;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace FGF_Finanzas.Capas.Servicios
{
    public class Encriptacion
    {
        public static readonly IDictionary<string, string[]> CamposCifrados =
            new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
            {
                { "Usuario", new[] { "nombre", "apellido", "usuario", "mail", "rol_viejo" } },
                { "Mascota", new[] { "nombre", "especie", "raza" } },
                { "Consulta", new[] { "motivo", "fechaHora", "estado", "diagnostico", "fechaCancelacion" } },
                { "Atencion", new[] { "diagnostico", "observaciones" } },
                { "Evento", new[] { "modulo", "evento" } },
                { "Rol", new[] { "nombre", "descripcion" } },
                { "Familia", new[] { "nombre", "descripcion" } },
                { "Permiso", new[] { "descripcion" } },
                { "Idioma", new[] { "Nombre" } },
                { "Traduccion", new[] { "Texto" } },
                { "Producto", new[] { "Nombre", "Descripcion", "Imagen" } },
                { "Compra", new[] { "Estado", "TitularTarjeta", "Ultimos4" } },
                { "CompraItem", new[] { "NombreProducto" } },
                { "InformeConsultas", new[] { "NombreVeterinario" } },
                { "InformeConsultasDetalle", new[] { "NombreVeterinario" } }
            };

        public static object DescifrarCampo(string tabla, string columna, object valor)
        {
            if (valor == null || valor == DBNull.Value) return valor;
            if (CamposFecha.TryGetValue(tabla, out string[] fechas) && fechas.Contains(columna, StringComparer.OrdinalIgnoreCase))
                return DescifrarFecha(valor);
            return CamposCifrados.TryGetValue(tabla, out string[] campos) &&
                campos.Contains(columna, StringComparer.OrdinalIgnoreCase)
                ? DesencriptarAES(valor.ToString()) : valor;
        }

        public static void DescifrarTabla(System.Data.DataTable datos, string tabla)
        {
            foreach (System.Data.DataRow fila in datos.Rows)
                foreach (System.Data.DataColumn columna in datos.Columns)
                    fila[columna] = DescifrarCampo(tabla, columna.ColumnName, fila[columna]);
            datos.AcceptChanges();
        }

        public static readonly IDictionary<string, string[]> CamposFecha =
            new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
            {
                { "Mascota", new[] { "fechaNacimiento" } },
                { "Evento", new[] { "fechaHora" } },
                { "Compra", new[] { "Fecha" } },
                { "InformeConsultas", new[] { "Generado" } }
            };

        public static string CifrarFecha(DateTime fecha)
        {
            return EncriptarAES(fecha.ToString("yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.InvariantCulture));
        }

        public static DateTime DescifrarFecha(object valor)
        {
            if (valor is DateTime fecha) return fecha;
            return DateTime.ParseExact(DesencriptarAES(valor.ToString()),
                "yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.InvariantCulture);
        }

        public static System.Data.DataTable DescifrarFechas(System.Data.DataTable datos, params string[] columnas)
        {
            var resultado = datos.Clone();
            foreach (string columna in columnas) resultado.Columns[columna].DataType = typeof(DateTime);
            foreach (System.Data.DataRow fila in datos.Rows)
            {
                var nueva = resultado.NewRow();
                foreach (System.Data.DataColumn col in datos.Columns)
                    nueva[col.ColumnName] = columnas.Contains(col.ColumnName, StringComparer.OrdinalIgnoreCase) && fila[col] != DBNull.Value
                        ? (object)DescifrarFecha(fila[col]) : fila[col];
                resultado.Rows.Add(nueva);
            }
            resultado.AcceptChanges();
            return resultado;
        }

        private static readonly byte[] Key = Encoding.UTF8.GetBytes("12345678901234567890123456789012");
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("1234567890123456");

        public static string Encriptar(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytesTexto = Encoding.UTF8.GetBytes(s);
                byte[] hash = sha256.ComputeHash(bytesTexto);
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hash) sb.Append(b.ToString("x2", CultureInfo.InvariantCulture));
                return sb.ToString();
            }
        }
        public static string EncriptarAES(string textoPlano)
        {
            if (string.IsNullOrEmpty(textoPlano)) return textoPlano;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(textoPlano);
                        }
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }
        public static string DesencriptarAES(string textoEncriptado)
        {
            if (string.IsNullOrEmpty(textoEncriptado)) return textoEncriptado;

            try
            {
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Key;
                    aesAlg.IV = IV;

                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(textoEncriptado)))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                return srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch
            {
                return textoEncriptado;
            }
        }
    }
}
