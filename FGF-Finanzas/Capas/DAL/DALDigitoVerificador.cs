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
        public string CalcularDigitoVerificadorHorizontal(BEDigitoVerificador dv)
        {
            BigInteger sumaTotal = 0;
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                string consulta = $"SELECT * FROM {dv.NombreTabla}";
                SqlCommand cmd = new SqlCommand(consulta, con);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    object[] datos = new object[rdr.FieldCount];
                    rdr.GetValues(datos);
                    BigInteger sumaParcial = 0;
                    foreach (object o in datos)
                    {
                        string hex = Encriptacion.Encriptar(o.ToString());
                        BigInteger num = BigInteger.Parse("00" + hex, NumberStyles.HexNumber);
                        sumaParcial += num;
                    }
                    string hex2 = Encriptacion.Encriptar(sumaParcial.ToString());
                    sumaParcial = BigInteger.Parse("00" + hex2, NumberStyles.HexNumber);
                    sumaTotal += sumaParcial;
                }
            }
            return sumaTotal.ToString("X");
        }

        public string CalcularDigitoVerificadorVertical(BEDigitoVerificador dv)
        {
            BigInteger sumaTotal = 0;
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                string consulta = $"SELECT * FROM {dv.NombreTabla}";
                SqlCommand cmd = new SqlCommand(consulta, con);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                List<object[]> registros = new List<object[]>();
                while (rdr.Read())
                {
                    object[] fila = new object[rdr.FieldCount];
                    rdr.GetValues(fila);
                    registros.Add(fila);
                }
                rdr.Close();
                if (registros.Count > 0)
                {
                    for (int col = 0; col < registros[0].Length; col++)
                    {
                        BigInteger sumaColumna = 0;

                        foreach (var fila in registros)
                        {
                            string texto = fila[col]?.ToString() ?? "";
                            string hex = Encriptacion.Encriptar(texto);

                            try
                            {
                                BigInteger valor = BigInteger.Parse("00" + hex, NumberStyles.HexNumber);
                                sumaColumna += valor;
                            }
                            catch
                            {

                            }
                        }
                        string hexCol = Encriptacion.Encriptar(sumaColumna.ToString());
                        sumaColumna = BigInteger.Parse("00" + hexCol, NumberStyles.HexNumber);

                        sumaTotal += sumaColumna;
                    }
                }
            }
            return sumaTotal.ToString("X");
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
                    string queryUpdate = @"UPDATE DigitoVerificador SET DigitoHorizontal = @horizontal, DigitoVertical = @vertical WHERE NombreTabla = @nombre";

                    cmd = new SqlCommand(queryUpdate, con);
                    cmd.Parameters.AddWithValue("@horizontal", dv.DV_Horizontal);
                    cmd.Parameters.AddWithValue("@vertical", dv.DV_Vertical);
                    cmd.Parameters.AddWithValue("@nombre", dv.NombreTabla);

                    cmd.ExecuteNonQuery();
                }
            }

        }
        public List<BEDigitoVerificador> ObtenerTodos()
        {
            List<BEDigitoVerificador> lista = new List<BEDigitoVerificador>();

            using (SqlConnection con = new SqlConnection(_conexion))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM DigitoVerificador", con);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    BEDigitoVerificador dv = new BEDigitoVerificador(reader["NombreTabla"].ToString(), reader["DigitoHorizontal"].ToString()
                        , reader["DigitoVertical"].ToString());

                    lista.Add(dv);
                }
            }
            return lista;
        }
    }
}