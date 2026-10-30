using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.Servicios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace FGF_Finanzas.Capas.DAL
{
    public class DALAtencion : DALAbstracta
    {
        public List<BEAtencion> ObtenerTodos()
        {
            List<BEAtencion> atenciones = new List<BEAtencion>();

            const string query = @"SELECT id_atencion, id_consulta, diagnostico, observaciones, atendido
                                   FROM Atencion
                                   ORDER BY id_atencion";

            using (SqlConnection con = new SqlConnection(_conexion))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        atenciones.Add(MapearAtencion(reader));
                    }
                }
            }

            return atenciones;
        }

        public BEAtencion ObtenerPorConsulta(int idConsulta)
        {
            const string query = @"SELECT id_atencion, id_consulta, diagnostico, observaciones, atendido
                                   FROM Atencion
                                   WHERE id_consulta = @IdConsulta";

            using (SqlConnection con = new SqlConnection(_conexion))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@IdConsulta", idConsulta);

                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapearAtencion(reader);
                    }
                }
            }

            return null;
        }

        public void Guardar(BEAtencion atencion)
        {
            const string query = @"INSERT INTO Atencion (id_consulta, diagnostico, observaciones, atendido)
                                   VALUES (@IdConsulta, @Diagnostico, @Observaciones, @Atendido);
                                   SELECT CAST(SCOPE_IDENTITY() AS int);";

            using (SqlConnection con = new SqlConnection(_conexion))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@IdConsulta", atencion.id_consulta);
                cmd.Parameters.AddWithValue("@Diagnostico", Encriptacion.EncriptarAES(atencion.diagnostico));
                cmd.Parameters.AddWithValue("@Observaciones", Encriptacion.EncriptarAES(atencion.observaciones);
                cmd.Parameters.AddWithValue("@Atendido", atencion.atendido);

                con.Open();
                atencion.id_atencion = Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public void Actualizar(BEAtencion atencion)
        {
            const string query = @"UPDATE Atencion
                                   SET id_consulta = @IdConsulta,
                                       diagnostico = @Diagnostico,
                                       observaciones = @Observaciones,
                                       atendido = @Atendido
                                   WHERE id_atencion = @IdAtencion";

            using (SqlConnection con = new SqlConnection(_conexion))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@IdAtencion", atencion.id_atencion);
                cmd.Parameters.AddWithValue("@IdConsulta", atencion.id_consulta);
                cmd.Parameters.AddWithValue("@Diagnostico", Encriptacion.EncriptarAES(atencion.diagnostico));
                cmd.Parameters.AddWithValue("@Observaciones", Encriptacion.EncriptarAES(atencion.observaciones));
                cmd.Parameters.AddWithValue("@Atendido", atencion.atendido);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private BEAtencion MapearAtencion(SqlDataReader reader)
        {
            return new BEAtencion
            {
                id_atencion = Convert.ToInt32(reader["id_atencion"]),
                id_consulta = Convert.ToInt32(reader["id_consulta"]),
                diagnostico = Encriptacion.DesencriptarAES(reader["diagnostico"] == DBNull.Value ? null : reader["diagnostico"].ToString()),
                observaciones = Encriptacion.DesencriptarAES(reader["observaciones"] == DBNull.Value ? null : reader["observaciones"].ToString()),
                atendido = Convert.ToBoolean(reader["atendido"])
            };
        }
    }
}
