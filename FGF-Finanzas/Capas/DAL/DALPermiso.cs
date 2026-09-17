using FGF_Finanzas.Capas.Servicios;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FGF_Finanzas.Capas.DAL
{
    public class DALPermiso : DALAbstracta
    {
        public List<Permiso> ObtenerPermisos()
        {
            List<Permiso> lista = new List<Permiso>();

            using (SqlConnection conexion = new SqlConnection(_conexion))
            {
                string query = @"SELECT id_permiso, nombre, descripcion FROM Permiso";

                SqlCommand cmd = new SqlCommand(query, conexion);

                conexion.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Permiso permiso = new Permiso
                    {
                        Id = Convert.ToInt32(reader["IdPermiso"]),
                        Nombre = reader["Nombre"].ToString(),
                        Descripcion = reader["Descripcion"].ToString(),
                    };
                    lista.Add(permiso);
                }
            }

            return lista;
        }
        public List<Permiso> ListarPorFamilia(Familia familia)
        {
            List<Permiso> lista = new List<Permiso>();

            using (SqlConnection conexion = new SqlConnection(_conexion))
            {
                string query =
                    @"SELECT P.id_permiso,
                             P.nombre,
                             P.descripcion
                      FROM Permiso P
                      INNER JOIN FamiliaPermiso FP
                          ON P.id_permiso = FP.id_permiso
                      WHERE FP.id_familia = @id_familia";

                SqlCommand cmd = new SqlCommand(query, conexion);

                cmd.Parameters.AddWithValue("@id_familia", familia.Id);

                conexion.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new Permiso
                    {
                        Id = Convert.ToInt32(reader["id_permiso"]),
                        Nombre = reader["nombre"].ToString(),
                        Descripcion = reader["descripcion"].ToString()
                    });
                }
            }
            return lista;
        }
        public List<Permiso> ListarPorRol(Rol rol)
        {
            List<Permiso> lista = new List<Permiso>();

            using (SqlConnection conexion = new SqlConnection(_conexion))
            {
                string query =
                    @"SELECT P.id_permiso,
                             P.nombre,
                             P.descripcion
                      FROM Permiso P
                      INNER JOIN RolPermiso RP
                          ON P.id_permiso = RP.id_permiso
                      WHERE RP.id_rol = @id_rol";

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@id_rol", rol.Id);

                conexion.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new Permiso
                    {
                        Id = Convert.ToInt32(reader["id_permiso"]),
                        Nombre = reader["nombre"].ToString(),
                        Descripcion = reader["descripcion"].ToString()
                    });
                }
            }

            return lista;
        }
    }
}