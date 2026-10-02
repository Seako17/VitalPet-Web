using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using FGF_Finanzas.Capas.BE;

namespace FGF_Finanzas.Capas.DAL
{
    public class DALTraduccion : DALAbstracta
    {

        public IDictionary<string, string> ObtenerTraducciones(string formulario, string codigoIdioma)
        {
            var diccionario = new Dictionary<string, string>();
            const string query = @"
                SELECT e.ControlId, t.Texto
                FROM Traduccion t
                INNER JOIN Etiqueta e ON t.IdEtiqueta = e.IdEtiqueta
                INNER JOIN Idioma i ON t.IdIdioma = i.IdIdioma
                WHERE e.Formulario = @Formulario AND i.Codigo = @CodigoIdioma";

            using (var con = new SqlConnection(_conexion))
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
            }
            return diccionario;
        }

        public List<BEIdioma> ObtenerIdiomasDisponibles()
        {
            var lista = new List<BEIdioma>();
            const string query = "SELECT Codigo, Nombre FROM Idioma ORDER BY Nombre";

            using (var con = new SqlConnection(_conexion))
            using (var cmd = new SqlCommand(query, con))
            {
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new BEIdioma
                        {
                            Codigo = reader["Codigo"].ToString(),
                            Nombre = reader["Nombre"].ToString()
                        });
                    }
                }
            }
            return lista;
        }

        public List<BEIdioma> ObtenerIdiomas()
        {
            var lista = new List<BEIdioma>();
            const string query = "SELECT IdIdioma, Codigo, Nombre FROM Idioma ORDER BY Nombre";

            using (var con = new SqlConnection(_conexion))
            using (var cmd = new SqlCommand(query, con))
            {
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new BEIdioma(
                            Convert.ToInt32(reader["IdIdioma"]),
                            reader["Codigo"].ToString(),
                            reader["Nombre"].ToString()));
                    }
                }
            }
            return lista;
        }

        public BEIdioma ObtenerIdiomaPorCodigo(string codigo)
        {
            const string query = "SELECT IdIdioma, Codigo, Nombre FROM Idioma WHERE Codigo = @Codigo";

            using (var con = new SqlConnection(_conexion))
            using (var cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Codigo", codigo);
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new BEIdioma(
                            Convert.ToInt32(reader["IdIdioma"]),
                            reader["Codigo"].ToString(),
                            reader["Nombre"].ToString());
                    }
                }
            }
            return null;
        }

        public List<string> ObtenerFormularios()
        {
            var lista = new List<string>();
            const string query = "SELECT DISTINCT Formulario FROM Etiqueta ORDER BY Formulario";

            using (var con = new SqlConnection(_conexion))
            using (var cmd = new SqlCommand(query, con))
            {
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(reader["Formulario"].ToString());
                    }
                }
            }
            return lista;
        }

        public List<BEEtiqueta> ObtenerEtiquetas(string formulario)
        {
            var lista = new List<BEEtiqueta>();
            const string query = "SELECT IdEtiqueta, Formulario, ControlId FROM Etiqueta WHERE Formulario = @Formulario ORDER BY ControlId";

            using (var con = new SqlConnection(_conexion))
            using (var cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Formulario", formulario);
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new BEEtiqueta(
                            Convert.ToInt32(reader["IdEtiqueta"]),
                            reader["Formulario"].ToString(),
                            reader["ControlId"].ToString()));
                    }
                }
            }
            return lista;
        }

        public DataTable ObtenerEtiquetasConTraduccion(string formulario, int idIdioma)
        {
            const string query = @"
                SELECT e.IdEtiqueta, e.ControlId, ISNULL(t.Texto, '') AS Texto
                FROM Etiqueta e
                LEFT JOIN Traduccion t ON t.IdEtiqueta = e.IdEtiqueta AND t.IdIdioma = @IdIdioma
                WHERE e.Formulario = @Formulario
                ORDER BY e.ControlId";

            var dt = new DataTable();
            using (var con = new SqlConnection(_conexion))
            using (var cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Formulario", formulario);
                cmd.Parameters.AddWithValue("@IdIdioma", idIdioma);
                con.Open();
                using (var adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
            return dt;
        }

        public int AgregarIdioma(BEIdioma idioma)
        {
            const string query = @"
                INSERT INTO Idioma (Codigo, Nombre) VALUES (@Codigo, @Nombre);
                SELECT CAST(SCOPE_IDENTITY() AS int);";

            using (var con = new SqlConnection(_conexion))
            using (var cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Codigo", idioma.Codigo);
                cmd.Parameters.AddWithValue("@Nombre", idioma.Nombre);
                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public int CopiarTraducciones(int idIdiomaOrigen, int idIdiomaDestino)
        {
            const string query = @"
                INSERT INTO Traduccion (IdIdioma, IdEtiqueta, Texto)
                SELECT @IdIdiomaDestino, tr.IdEtiqueta, tr.Texto
                FROM Traduccion tr
                WHERE tr.IdIdioma = @IdIdiomaOrigen
                  AND NOT EXISTS (SELECT 1 FROM Traduccion x
                                  WHERE x.IdIdioma = @IdIdiomaDestino AND x.IdEtiqueta = tr.IdEtiqueta)";

            using (var con = new SqlConnection(_conexion))
            using (var cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@IdIdiomaOrigen", idIdiomaOrigen);
                cmd.Parameters.AddWithValue("@IdIdiomaDestino", idIdiomaDestino);
                con.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public void GuardarTraduccion(BETraduccion traduccion)
        {
            const string query = @"
                UPDATE Traduccion SET Texto = @Texto
                WHERE IdIdioma = @IdIdioma AND IdEtiqueta = @IdEtiqueta;

                IF @@ROWCOUNT = 0
                    INSERT INTO Traduccion (IdIdioma, IdEtiqueta, Texto) VALUES (@IdIdioma, @IdEtiqueta, @Texto);";

            using (var con = new SqlConnection(_conexion))
            using (var cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Texto", traduccion.Texto);
                cmd.Parameters.AddWithValue("@IdIdioma", traduccion.IdIdioma);
                cmd.Parameters.AddWithValue("@IdEtiqueta", traduccion.IdEtiqueta);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void EliminarIdioma(int idIdioma)
        {
            const string query = @"
                DELETE FROM Traduccion WHERE IdIdioma = @IdIdioma;
                DELETE FROM Idioma WHERE IdIdioma = @IdIdioma;";

            using (var con = new SqlConnection(_conexion))
            using (var cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@IdIdioma", idIdioma);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}