using FGF_Finanzas.Capas.BE;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Numerics;
using System.Linq;
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
                                fila.ValoresCampos.Add(valor);
                            }
                        }
                        return fila;
                    }
                }
            }
            return null;
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
            List<FilaGenerica> filas = new List<FilaGenerica>();

            using (SqlConnection con = new SqlConnection(_conexion))
            {
                string consulta = $"SELECT * FROM {nombreTabla}";
                SqlCommand cmd = new SqlCommand(consulta, con);
                con.Open();

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

        private string ObtenerNombrePK(string nombreTabla, SqlConnection con)
        {
            string query = @"SELECT COLUMN_NAME 
                     FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE 
                     WHERE OBJECTPROPERTY(OBJECT_ID(CONSTRAINT_SCHEMA + '.' + CONSTRAINT_NAME), 'IsPrimaryKey') = 1
                     AND TABLE_NAME = @tabla";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@tabla", nombreTabla);
                object resultado = cmd.ExecuteScalar();
                return resultado?.ToString() ?? "id";
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