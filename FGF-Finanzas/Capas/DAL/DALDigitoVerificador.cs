using FGF_Finanzas.Capas.BE;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Numerics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using FGF_Finanzas.Capas.Servicios;

namespace FGF_Finanzas.Capas.DAL
{
    public class DALDigitoVerificador : DALAbstracta
    {
        #region Reales

        public FilaGenerica ObtenerFilaPorId(string nombreTabla, string id)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                con.Open();
                string nombrePK = ObtenerNombrePK(nombreTabla, con);

                string consulta = $"SELECT * FROM {nombreTabla} WHERE {nombrePK} = @id";
                SqlCommand cmd = new SqlCommand(consulta, con);
                cmd.Parameters.AddWithValue("@id", id);

                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        var fila = new FilaGenerica();
                        fila.Id = rdr.GetValue(0).ToString();
                        for (int i = 0; i < rdr.FieldCount; i++)
                        {
                            string nombreColumna = rdr.GetName(i);

                            if (nombreColumna.Equals("DV_Horizontal", StringComparison.OrdinalIgnoreCase))
                            {
                                fila.DV_HorizontalGuardado = rdr.GetValue(i).ToString();
                                continue;
                            }
                            else
                            {
                                object valor = rdr.GetValue(i);

                                if (nombreTabla.Equals("Usuario", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (nombreColumna.Equals("nombre", StringComparison.OrdinalIgnoreCase) ||
                                        nombreColumna.Equals("apellido", StringComparison.OrdinalIgnoreCase) ||
                                        nombreColumna.Equals("usuario", StringComparison.OrdinalIgnoreCase) ||
                                        nombreColumna.Equals("mail", StringComparison.OrdinalIgnoreCase))
                                    {
                                        valor = Encriptacion.DesencriptarAES(valor?.ToString());
                                    }
                                }
                                else if (nombreTabla.Equals("Producto", StringComparison.OrdinalIgnoreCase) &&
                                    (nombreColumna.Equals("Nombre", StringComparison.OrdinalIgnoreCase) || nombreColumna.Equals("Descripcion", StringComparison.OrdinalIgnoreCase) || nombreColumna.Equals("Imagen", StringComparison.OrdinalIgnoreCase)))
                                {
                                    valor = Encriptacion.DesencriptarAES(valor?.ToString());
                                }
                                if (nombreTabla.Equals("Compra", StringComparison.OrdinalIgnoreCase) &&
                                    (nombreColumna.Equals("TitularTarjeta", StringComparison.OrdinalIgnoreCase) || nombreColumna.Equals("Ultimos4", StringComparison.OrdinalIgnoreCase)))
                                    valor = Encriptacion.DesencriptarAES(valor?.ToString());
                                fila.ValoresCampos.Add(valor);
                            }
                        }
                        return fila;
                    }
                }
            }
            return null;
        }

        public void ActualizarIntegridadTabla(string nombreTabla) { ActualizarIntegridadTabla(nombreTabla, null, null); }

        public void ActualizarIntegridadTabla(string nombreTabla, SqlConnection conexion, SqlTransaction transaccion)
        {
            // Los identificadores SQL no admiten parámetros; solo aceptamos nombres simples de tabla.
            if (string.IsNullOrWhiteSpace(nombreTabla) || !Regex.IsMatch(nombreTabla, @"^[A-Za-z_][A-Za-z0-9_]*$"))
                throw new ArgumentException("Nombre de tabla inválido.", "nombreTabla");
            bool propia = conexion == null;
            if (propia) { conexion = new SqlConnection(_conexion); conexion.Open(); }
            bool transaccionPropia = propia && transaccion == null;
            if (transaccionPropia) transaccion = conexion.BeginTransaction();
            try
            {
                string pk = ObtenerNombrePK(nombreTabla, conexion, transaccion);
                List<FilaGenerica> filas = LeerFilasIntegridad(nombreTabla, conexion, transaccion);
                BigInteger vertical = BigInteger.Zero;
                foreach (FilaGenerica fila in filas)
                {
                    string dvh = CalcularDVHorizontalFila(fila);
                    using (SqlCommand q = transaccion == null ? new SqlCommand("UPDATE " + nombreTabla + " SET DV_Horizontal=@dvh WHERE " + pk + "=@id", conexion) : new SqlCommand("UPDATE " + nombreTabla + " SET DV_Horizontal=@dvh WHERE " + pk + "=@id", conexion, transaccion)) { q.Parameters.AddWithValue("@dvh", dvh); q.Parameters.AddWithValue("@id", fila.Id); q.ExecuteNonQuery(); }
                    vertical += BigInteger.Parse("00" + Encriptacion.Encriptar(dvh), NumberStyles.HexNumber);
                }
                using (SqlCommand q = transaccion == null ? new SqlCommand("UPDATE DigitoVerificador SET DigitoVertical=@v,CantidadRegistros=@n WHERE NombreTabla=@t; IF @@ROWCOUNT=0 INSERT DigitoVerificador(NombreTabla,DigitoVertical,CantidadRegistros) VALUES(@t,@v,@n)", conexion) : new SqlCommand("UPDATE DigitoVerificador SET DigitoVertical=@v,CantidadRegistros=@n WHERE NombreTabla=@t; IF @@ROWCOUNT=0 INSERT DigitoVerificador(NombreTabla,DigitoVertical,CantidadRegistros) VALUES(@t,@v,@n)", conexion, transaccion)) { q.Parameters.AddWithValue("@v", vertical.ToString("X")); q.Parameters.AddWithValue("@n", filas.Count); q.Parameters.AddWithValue("@t", nombreTabla); q.ExecuteNonQuery(); }
                if (transaccionPropia) transaccion.Commit();
            }
            catch { if (transaccionPropia) transaccion.Rollback(); throw; }
            finally
            {
                if (transaccionPropia) transaccion.Dispose();
                if (propia) conexion.Dispose();
            }
        }

        private List<FilaGenerica> LeerFilasIntegridad(string nombreTabla, SqlConnection conexion, SqlTransaction transaccion)
        {
            var filas = new List<FilaGenerica>();
            using (SqlCommand cmd = transaccion == null ? new SqlCommand("SELECT * FROM " + nombreTabla, conexion) : new SqlCommand("SELECT * FROM " + nombreTabla, conexion, transaccion))
            using (SqlDataReader r = cmd.ExecuteReader()) while (r.Read())
            {
                var fila = new FilaGenerica { Id = Convert.ToString(r.GetValue(0)) };
                for (int i = 0; i < r.FieldCount; i++)
                {
                    string col = r.GetName(i);
                    if (col.Equals("DV_Horizontal", StringComparison.OrdinalIgnoreCase)) { fila.DV_HorizontalGuardado = Convert.ToString(r.GetValue(i)); continue; }
                    object value = r.IsDBNull(i) ? null : r.GetValue(i);
                    if (nombreTabla.Equals("Usuario", StringComparison.OrdinalIgnoreCase) && new[] { "nombre", "apellido", "usuario", "mail" }.Contains(col, StringComparer.OrdinalIgnoreCase)) value = Encriptacion.DesencriptarAES(Convert.ToString(value));
                    if (nombreTabla.Equals("Producto", StringComparison.OrdinalIgnoreCase) && new[] { "Nombre", "Descripcion", "Imagen" }.Contains(col, StringComparer.OrdinalIgnoreCase)) value = Encriptacion.DesencriptarAES(Convert.ToString(value));
                    if (nombreTabla.Equals("Compra", StringComparison.OrdinalIgnoreCase) && new[] { "TitularTarjeta", "Ultimos4" }.Contains(col, StringComparer.OrdinalIgnoreCase)) value = Encriptacion.DesencriptarAES(Convert.ToString(value));
                    if (nombreTabla.Equals("Atencion", StringComparison.OrdinalIgnoreCase) && new[] { "diagnostico", "observaciones" }.Contains(col, StringComparer.OrdinalIgnoreCase)) value = Encriptacion.DesencriptarAES(Convert.ToString(value));
                    fila.ValoresCampos.Add(value);
                }
                filas.Add(fila);
            }
            return filas;
        }
        public void ActualizarDVHorizontalFilaEspecifica(string nombreTabla, string id, string nuevoDVH)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                con.Open();
                // 1. Averiguamos la PK dinámicamente
                string nombrePK = ObtenerNombrePK(nombreTabla, con);

                string query = $"UPDATE {nombreTabla} SET DV_Horizontal = @dvh WHERE {nombrePK} = @id";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@dvh", nuevoDVH);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public List<FilaGenerica> ObtenerFilasDeTablaNegocio(string nombreTabla)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                con.Open();
                return ObtenerFilasDeTablaNegocio(nombreTabla, con, null);
            }
        }

        private List<FilaGenerica> ObtenerFilasDeTablaNegocio(string nombreTabla, SqlConnection con, SqlTransaction tx)
        {
            List<FilaGenerica> filas = new List<FilaGenerica>();
            string consulta = "SELECT * FROM " + nombreTabla;
            using (SqlCommand cmd = tx == null ? new SqlCommand(consulta, con) : new SqlCommand(consulta, con, tx))
            {
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        var fila = new FilaGenerica();
                        fila.Id = rdr.GetValue(0).ToString();

                        for (int i = 0; i < rdr.FieldCount; i++)
                        {
                            string nombreColumna = rdr.GetName(i);

                            if (nombreColumna.Equals("DV_Horizontal", StringComparison.OrdinalIgnoreCase))
                            {
                                fila.DV_HorizontalGuardado = rdr.GetValue(i).ToString();
                                continue;
                            }
                            else
                            {
                                object valor = rdr.GetValue(i);

                                if (nombreTabla.Equals("Usuario", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (nombreColumna.Equals("nombre", StringComparison.OrdinalIgnoreCase) ||
                                        nombreColumna.Equals("apellido", StringComparison.OrdinalIgnoreCase) ||
                                        nombreColumna.Equals("usuario", StringComparison.OrdinalIgnoreCase) ||
                                        nombreColumna.Equals("mail", StringComparison.OrdinalIgnoreCase))
                                    {
                                        valor = Encriptacion.DesencriptarAES(valor?.ToString());
                                    }
                                }
                                else if (nombreTabla.Equals("Atencion", StringComparison.OrdinalIgnoreCase) &&
                                    (nombreColumna.Equals("diagnostico", StringComparison.OrdinalIgnoreCase) || nombreColumna.Equals("observaciones", StringComparison.OrdinalIgnoreCase)))
                                {
                                    valor = Encriptacion.DesencriptarAES(valor?.ToString());
                                }
                                else if (nombreTabla.Equals("Producto", StringComparison.OrdinalIgnoreCase) &&
                                    (nombreColumna.Equals("Nombre", StringComparison.OrdinalIgnoreCase) || nombreColumna.Equals("Descripcion", StringComparison.OrdinalIgnoreCase) || nombreColumna.Equals("Imagen", StringComparison.OrdinalIgnoreCase)))
                                {
                                    valor = Encriptacion.DesencriptarAES(valor?.ToString());
                                }
                                if (nombreTabla.Equals("Compra", StringComparison.OrdinalIgnoreCase) &&
                                    (nombreColumna.Equals("TitularTarjeta", StringComparison.OrdinalIgnoreCase) || nombreColumna.Equals("Ultimos4", StringComparison.OrdinalIgnoreCase)))
                                    valor = Encriptacion.DesencriptarAES(valor?.ToString());
                                fila.ValoresCampos.Add(valor);
                            }
                        }

                        filas.Add(fila);
                    }
                }
            }
            return filas;
        }

        public string ObtenerConexionString() { return _conexion; }

        public string CalcularDVHorizontalFila(FilaGenerica fila)
        {
            BigInteger sumaParcial = 0;
            foreach (object o in fila.ValoresCampos)
            {
                string texto = o?.ToString() ?? "";
                string hex = Encriptacion.Encriptar(texto);
                BigInteger num = BigInteger.Parse("00" + hex, NumberStyles.HexNumber);
                sumaParcial += num;
            }

            string hex2 = Encriptacion.Encriptar(sumaParcial.ToString());
            BigInteger resultadoFinal = BigInteger.Parse("00" + hex2, NumberStyles.HexNumber);

            return resultadoFinal.ToString("X");
        }

        public BEDigitoVerificador ObtenerDV_Tabla(string nombreTabla)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                string consulta = "SELECT NombreTabla, DigitoVertical, CantidadRegistros FROM DigitoVerificador WHERE NombreTabla = @nombre";
                SqlCommand cmd = new SqlCommand(consulta, con);
                cmd.Parameters.AddWithValue("@nombre", nombreTabla);
                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var dv = new BEDigitoVerificador(
                            reader["NombreTabla"].ToString(),
                            reader["DigitoVertical"].ToString(),
                            Convert.ToInt32(reader["CantidadRegistros"])
                        );

                        return dv;
                    }
                }
            }
            return null;
        }

        public void GuardarDigitoVerificador(BEDigitoVerificador dv)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                string consulta = "SELECT COUNT(*) FROM DigitoVerificador WHERE NombreTabla = @nombre";
                SqlCommand cmd = new SqlCommand(consulta, con);
                cmd.Parameters.AddWithValue("@nombre", dv.NombreTabla);
                con.Open();
                int existe = (int)cmd.ExecuteScalar();

                if (existe > 0)
                {
                    string queryUpdate = @"UPDATE DigitoVerificador 
                                   SET DigitoVertical = @vertical, 
                                       CantidadRegistros = @cantidad 
                                   WHERE NombreTabla = @nombre";

                    cmd = new SqlCommand(queryUpdate, con);
                    cmd.Parameters.AddWithValue("@vertical", dv.DV_Vertical);
                    cmd.Parameters.AddWithValue("@cantidad", dv.CantidadRegistros);
                    cmd.Parameters.AddWithValue("@nombre", dv.NombreTabla);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    string queryInsert = @"INSERT INTO DigitoVerificador (NombreTabla, DigitoVertical, CantidadRegistros) 
                                   VALUES (@nombre, @vertical, @cantidad)";

                    cmd = new SqlCommand(queryInsert, con);
                    cmd.Parameters.AddWithValue("@nombre", dv.NombreTabla);
                    cmd.Parameters.AddWithValue("@vertical", dv.DV_Vertical);
                    cmd.Parameters.AddWithValue("@cantidad", dv.CantidadRegistros);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<BEDigitoVerificador> ObtenerTodos()
        {
            List<BEDigitoVerificador> lista = new List<BEDigitoVerificador>();
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                SqlCommand cmd = new SqlCommand("SELECT NombreTabla, DigitoVertical, CantidadRegistros FROM DigitoVerificador", con);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new BEDigitoVerificador(
                            reader["NombreTabla"].ToString(),
                            reader["DigitoVertical"].ToString(),
                            Convert.ToInt32(reader["CantidadRegistros"])
                        ));
                    }
                }
            }
            return lista;
        }

        private string ObtenerNombrePK(string nombreTabla, SqlConnection con, SqlTransaction tx = null)
        {
            string query = @"SELECT COLUMN_NAME 
                     FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE 
                     WHERE OBJECTPROPERTY(OBJECT_ID(CONSTRAINT_SCHEMA + '.' + CONSTRAINT_NAME), 'IsPrimaryKey') = 1
                     AND TABLE_NAME = @tabla";

            using (SqlCommand cmd = tx == null ? new SqlCommand(query, con) : new SqlCommand(query, con, tx))
            {
                cmd.Parameters.AddWithValue("@tabla", nombreTabla);
                object resultado = cmd.ExecuteScalar();
                if (resultado == null || resultado == DBNull.Value)
                    throw new InvalidOperationException("La tabla " + nombreTabla + " no tiene una clave primaria reconocida para recalcular su DV.");
                return resultado.ToString();
            }
        }
        public string ObtenerPKPublico(string nombreTabla)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                con.Open();
                return ObtenerNombrePK(nombreTabla, con);
            }
        }
        #endregion
    }
}
