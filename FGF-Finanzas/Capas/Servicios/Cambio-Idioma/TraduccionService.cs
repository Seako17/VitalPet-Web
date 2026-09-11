using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace FGF_Finanzas.Capas.Servicios.Cambio_Idioma
{
    public class TraduccionService
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
        public IDictionary<string, string> ObtenerTraducciones(string formulario, string codigoIdioma)
        {
            var diccionario = new Dictionary<string, string>();
            const string query = @"
            SELECT e.ControlId, t.Texto
            FROM Traduccion t
            INNER JOIN Etiqueta e ON t.IdEtiqueta = e.IdEtiqueta
            INNER JOIN Idioma i ON t.IdIdioma = i.IdIdioma
            WHERE e.Formulario = @Formulario AND i.Codigo = @CodigoIdioma";

            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Formulario", formulario);
                cmd.Parameters.AddWithValue("@CodigoIdioma", codigoIdioma);
                con.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        diccionario[reader["ControlId"].ToString()] = reader["Texto"].ToString();
                    }
                }
                return diccionario;
            }
        }
    }
}