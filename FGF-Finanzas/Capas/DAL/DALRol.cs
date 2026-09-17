using FGF_Finanzas.Capas.Servicios;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FGF_Finanzas.Capas.DAL
{
    public class DALRol : DALAbstracta
    {
        public Rol ObtenerRol(Rol roli)
        {
            Rol rol = null;

            using (SqlConnection conexion = new SqlConnection(_conexion))
            {
                string query =
                    @"SELECT id_rol, nombre, descripcion
                      FROM Rol
                      WHERE id_rol = @IdRol";

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@IdRol", roli.Id);

                conexion.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    rol = new Rol
                    {
                        Id = Convert.ToInt32(reader["id_rol"]),
                        Nombre = reader["nombre"].ToString(),
                        Descripcion = reader["descripcion"].ToString()
                    };
                }
            }

            return rol;
        }
    }
}