using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.Servicios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

namespace FGF_Finanzas.Capas.DAL
{
    public class DALConsulta : DALAbstracta
    {
        private const string FORMATO_FECHA = "yyyy-MM-dd HH:mm";

        public DataTable ObtenerConsultas()
        {
            DataTable crudo = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Consulta", _conexion);
            adapter.Fill(crudo);

            DataTable dt = crudo.Clone();
            dt.Columns["fechaHora"].DataType = typeof(DateTime);

            foreach (DataRow fila in crudo.Rows)
            {
                DataRow nueva = dt.NewRow();
                foreach (DataColumn col in crudo.Columns)
                {
                    string nombre = col.ColumnName;

                    if (nombre.Equals("motivo", StringComparison.OrdinalIgnoreCase))
                    {
                        nueva[nombre] = Encriptacion.DesencriptarAES(fila[nombre].ToString());
                    }
                    else if (nombre.Equals("fechaHora", StringComparison.OrdinalIgnoreCase))
                    {
                        nueva[nombre] = LeerFecha(Encriptacion.DesencriptarAES(fila[nombre].ToString()));
                    }
                    else
                    {
                        nueva[nombre] = fila[nombre];
                    }
                }
                dt.Rows.Add(nueva);
            }

            dt.AcceptChanges();
            return dt;
        }

        public void AgregarConsulta(BEConsulta consulta)
        {
            const string query = @"INSERT INTO Consulta (idMascota, DNI_Veterinario, motivo, fechaHora)
                                   VALUES (@IdMascota, @DniVeterinario, @Motivo, @FechaHora)";

            using (SqlConnection con = new SqlConnection(_conexion))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@IdMascota", consulta.idMascota);
                cmd.Parameters.AddWithValue("@DniVeterinario", consulta.veterinario.DNI);
                cmd.Parameters.AddWithValue("@Motivo", Encriptacion.EncriptarAES(consulta.motivo));
                cmd.Parameters.AddWithValue("@FechaHora",
                    Encriptacion.EncriptarAES(consulta.fechaHora.ToString(FORMATO_FECHA, CultureInfo.InvariantCulture)));
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private DateTime LeerFecha(string texto)
        {
            DateTime fecha;
            if (!DateTime.TryParseExact(texto, FORMATO_FECHA, CultureInfo.InvariantCulture, DateTimeStyles.None, out fecha))
            {
                throw new Exception("Error al leer la fecha de una consulta: el dato guardado no es válido.");
            }
            return fecha;
        }
    }
}