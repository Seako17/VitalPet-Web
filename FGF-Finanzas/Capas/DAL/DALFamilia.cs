using FGF_Finanzas.Capas.Servicios;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FGF_Finanzas.Capas.DAL
{
    public class DALFamilia : DALAbstracta
    {
        public Familia ObtenerFamilia(Familia fam)
        {
            Familia familia = null;

            using (SqlConnection conexion = new SqlConnection(_conexion))
            {
                string query =
                    @"SELECT id_familia, nombre, descripcion
                      FROM Familia
                      WHERE id_familia = @IdFamilia";

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@IdFamilia", fam.Id);

                conexion.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    familia = new Familia
                    {
                        Id = Convert.ToInt32(reader["id_familia"]),
                        Nombre = reader["nombre"].ToString(),
                        Descripcion = reader["descripcion"].ToString()
                    };
                }
            }

            return familia;
        }
        public List<Familia> ListarHijas(Familia fam)
        {
            List<Familia> lista = new List<Familia>();

            using (SqlConnection conexion = new SqlConnection(_conexion))
            {
                string query =
                    @"SELECT F.id_familia,
                             F.nombre,
                             F.descripcion
                      FROM Familia F
                      INNER JOIN FamiliaFamilia FF
                          ON F.id_familia = FF.id_familia_hija
                      WHERE FF.id_familia_padre = @IdFamiliaPadre";

                SqlCommand cmd = new SqlCommand(query, conexion);

                cmd.Parameters.AddWithValue("@IdFamiliaPadre",fam.Id);

                conexion.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new Familia
                    {
                        Id = Convert.ToInt32(reader["id_familia"]),
                        Nombre = reader["nombre"].ToString(),
                        Descripcion = reader["descripcion"].ToString()
                    });
                }
            }

            return lista;
        }
        public List<Familia> ListarPorRol(Rol rol)
        {
            List<Familia> lista = new List<Familia>();

            using (SqlConnection conexion = new SqlConnection(_conexion))
            {
                string query =
                    @"SELECT F.id_familia,
                             F.nombre,
                             F.descripcion
                      FROM Familia F
                      INNER JOIN RolFamilia RF
                          ON F.id_familia = RF.id_familia
                      WHERE RF.id_rol = @IdRol";

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@IdRol", rol.Id);

                conexion.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new Familia
                    {
                        Id = Convert.ToInt32(reader["id_familia"]),
                        Nombre = reader["nombre"].ToString(),
                        Descripcion = reader["descripcion"].ToString()
                    });
                }
            }

            return lista;
        }
    }
}