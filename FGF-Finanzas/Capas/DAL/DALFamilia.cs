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
                string query = @"SELECT id_familia, nombre, descripcion FROM Familia WHERE id_familia = @IdFamilia";

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
        public List<Familia> ObtenerFamilias()
        {
            List<Familia> lista = new List<Familia>();

            using (SqlConnection conexion = new SqlConnection(_conexion))
            {
                string query = @"SELECT id_familia, nombre, descripcion FROM Familia ORDER BY nombre";

                SqlCommand cmd = new SqlCommand(query, conexion);

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
        public void AgregarFamilia(Familia familia)
        {
            using (SqlConnection conexion = new SqlConnection(_conexion))
            {
                string query = @"INSERT INTO Familia (nombre, descripcion) VALUES (@nombre, @descripcion)";

                SqlCommand cmd = new SqlCommand(query, conexion);

                cmd.Parameters.AddWithValue("@nombre", familia.Nombre);
                cmd.Parameters.AddWithValue("@descripcion", string.IsNullOrWhiteSpace(familia.Descripcion) ? (object)DBNull.Value : familia.Descripcion);

                conexion.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public bool ExisteFamilia(string nombre)
        {
            using (SqlConnection conexion = new SqlConnection(_conexion))
            {
                string query = @"SELECT COUNT(*) FROM Familia WHERE LOWER(nombre) = LOWER(@nombre)";

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@nombre", nombre);

                conexion.Open();

                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        public bool ExistePermisoEnFamilia(Familia familia, Permiso permiso)
        {
            using (SqlConnection conexion = new SqlConnection(_conexion))
            {
                string query = @"SELECT COUNT(*) FROM FamiliaPermiso WHERE id_familia = @idFamilia AND id_permiso = @idPermiso";

                SqlCommand cmd = new SqlCommand(query, conexion);

                cmd.Parameters.AddWithValue("@idFamilia", familia.Id);
                cmd.Parameters.AddWithValue("@idPermiso", permiso.Id);

                conexion.Open();

                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        public void AsignarPermiso(Familia familia, Permiso permiso)
        {
            using (SqlConnection conexion = new SqlConnection(_conexion))
            {
                string query = @"INSERT INTO FamiliaPermiso (id_familia, id_permiso) VALUES (@idFamilia, @idPermiso)";

                SqlCommand cmd = new SqlCommand(query, conexion);

                cmd.Parameters.AddWithValue("@idFamilia", familia.Id);
                cmd.Parameters.AddWithValue("@idPermiso", permiso.Id);

                conexion.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void EliminarPermiso(Familia familia, Permiso permiso)
        {
            using (SqlConnection conexion = new SqlConnection(_conexion))
            {
                string query = @"DELETE FROM FamiliaPermiso WHERE id_familia = @idFamilia AND id_permiso = @idPermiso";

                SqlCommand cmd = new SqlCommand(query, conexion);

                cmd.Parameters.AddWithValue("@idFamilia", familia.Id);
                cmd.Parameters.AddWithValue("@idPermiso", permiso.Id);

                conexion.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public bool ExisteRelacionFamilias(Familia padre, Familia hija)
        {
            using (SqlConnection conexion = new SqlConnection(_conexion))
            {
                string query = @"SELECT COUNT(*) FROM FamiliaFamilia WHERE id_familia_padre = @padre AND id_familia_hija = @hija";

                SqlCommand cmd = new SqlCommand(query, conexion);

                cmd.Parameters.AddWithValue("@padre", padre.Id);
                cmd.Parameters.AddWithValue("@hija", hija.Id);

                conexion.Open();

                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        public void AsignarFamilia(Familia padre, Familia hija)
        {
            using (SqlConnection conexion = new SqlConnection(_conexion))
            {
                string query = @"INSERT INTO FamiliaFamilia (id_familia_padre, id_familia_hija) VALUES (@padre, @hija)";

                SqlCommand cmd = new SqlCommand(query, conexion);

                cmd.Parameters.AddWithValue("@padre", padre.Id);
                cmd.Parameters.AddWithValue("@hija", hija.Id);

                conexion.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void EliminarRelacionFamilias(Familia padre, Familia hija)
        {
            using (SqlConnection conexion = new SqlConnection(_conexion))
            {
                string query = @"DELETE FROM FamiliaFamilia WHERE id_familia_padre = @padre AND id_familia_hija = @hija";

                SqlCommand cmd = new SqlCommand(query, conexion);

                cmd.Parameters.AddWithValue("@padre", padre.Id);
                cmd.Parameters.AddWithValue("@hija", hija.Id);

                conexion.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void EliminarFamilia(Familia familia)
        {
            using (SqlConnection conexion = new SqlConnection(_conexion))
            {
                conexion.Open();

                SqlTransaction transaccion = conexion.BeginTransaction();

                try
                {
                    string queryRolFamilia =@"DELETE FROM RolFamilia WHERE id_familia = @id";

                    using (SqlCommand cmd =
                        new SqlCommand(queryRolFamilia, conexion, transaccion))
                    {
                        cmd.Parameters.AddWithValue("@id", familia.Id);
                        cmd.ExecuteNonQuery();
                    }

                    string queryFamiliaPermiso = @"DELETE FROM FamiliaPermiso WHERE id_familia = @id";

                    using (SqlCommand cmd =
                        new SqlCommand(queryFamiliaPermiso, conexion, transaccion))
                    {
                        cmd.Parameters.AddWithValue("@id", familia.Id);
                        cmd.ExecuteNonQuery();
                    }

                    string queryFamiliaFamilia = @"DELETE FROM FamiliaFamilia WHERE id_familia_padre = @id OR id_familia_hija = @id";

                    using (SqlCommand cmd =
                        new SqlCommand(queryFamiliaFamilia, conexion, transaccion))
                    {
                        cmd.Parameters.AddWithValue("@id", familia.Id);
                        cmd.ExecuteNonQuery();
                    }

                    string queryFamilia = @"DELETE FROM Familia WHERE id_familia = @id";

                    using (SqlCommand cmd =
                        new SqlCommand(queryFamilia, conexion, transaccion))
                    {
                        cmd.Parameters.AddWithValue("@id", familia.Id);
                        cmd.ExecuteNonQuery();
                    }

                    transaccion.Commit();
                }
                catch
                {
                    transaccion.Rollback();
                    throw;
                }
            }
        }
        public List<Familia> ListarPadres(Familia fam)
        {
            List<Familia> lista = new List<Familia>();

            using (SqlConnection conexion = new SqlConnection(_conexion))
            {
                string query = @"SELECT F.id_familia, F.nombre, F.descripcion
                                 FROM Familia F
                                 INNER JOIN FamiliaFamilia FF ON F.id_familia = FF.id_familia_padre
                                 WHERE FF.id_familia_hija = @IdFamiliaHija";

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@IdFamiliaHija", fam.Id);

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
        public List<Familia> ObtenerFamiliasRaiz()
        {
            List<Familia> lista = new List<Familia>();

            using (SqlConnection conexion = new SqlConnection(_conexion))
            {
                string query = @"SELECT F.id_familia, F.nombre, F.descripcion
                                 FROM Familia F
                                 WHERE NOT EXISTS
                                 (SELECT 1 FROM FamiliaFamilia FF WHERE FF.id_familia_hija = F.id_familia)
                                 ORDER BY F.nombre";

                SqlCommand cmd = new SqlCommand(query, conexion);

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