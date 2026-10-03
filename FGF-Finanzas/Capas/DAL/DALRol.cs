using FGF_Finanzas.Capas.Servicios;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

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

        public List<Rol> ObtenerRoles()
        {
            List<Rol> lista = new List<Rol>();

            using (SqlConnection conexion = new SqlConnection(_conexion))
            {
                string query =
                    @"SELECT id_rol, nombre, descripcion
                      FROM Rol
                      ORDER BY nombre";

                SqlCommand cmd = new SqlCommand(query, conexion);

                conexion.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new Rol
                    {
                        Id = Convert.ToInt32(reader["id_rol"]),
                        Nombre = reader["nombre"].ToString(),
                        Descripcion = reader["descripcion"].ToString()
                    });
                }
            }

            return lista;
        }

        public bool ExisteRol(string nombre)
        {
            using (SqlConnection conexion = new SqlConnection(_conexion))
            {
                string query =
                    @"SELECT COUNT(*)
                      FROM Rol
                      WHERE LOWER(nombre) = LOWER(@nombre)";

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@nombre", nombre);

                conexion.Open();

                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        public void AgregarRol(Rol rol)
        {
            using (SqlConnection conexion = new SqlConnection(_conexion))
            {
                string query =
                    @"INSERT INTO Rol (nombre, descripcion)
                      VALUES (@nombre, @descripcion)";

                SqlCommand cmd = new SqlCommand(query, conexion);

                cmd.Parameters.AddWithValue("@nombre", rol.Nombre);

                cmd.Parameters.AddWithValue(
                    "@descripcion",
                    string.IsNullOrWhiteSpace(rol.Descripcion)
                        ? (object)DBNull.Value
                        : rol.Descripcion
                );

                conexion.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public bool ExistePermisoEnRol(Rol rol, Permiso permiso)
        {
            using (SqlConnection conexion = new SqlConnection(_conexion))
            {
                string query =
                    @"SELECT COUNT(*)
                      FROM RolPermiso
                      WHERE id_rol = @idRol
                        AND id_permiso = @idPermiso";

                SqlCommand cmd = new SqlCommand(query, conexion);

                cmd.Parameters.AddWithValue("@idRol", rol.Id);
                cmd.Parameters.AddWithValue("@idPermiso", permiso.Id);

                conexion.Open();

                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        public void AsignarPermiso(Rol rol, Permiso permiso)
        {
            using (SqlConnection conexion = new SqlConnection(_conexion))
            {
                string query =
                    @"INSERT INTO RolPermiso
                      (id_rol, id_permiso)
                      VALUES
                      (@idRol, @idPermiso)";

                SqlCommand cmd = new SqlCommand(query, conexion);

                cmd.Parameters.AddWithValue("@idRol", rol.Id);
                cmd.Parameters.AddWithValue("@idPermiso", permiso.Id);

                conexion.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void EliminarPermiso(Rol rol, Permiso permiso)
        {
            using (SqlConnection conexion = new SqlConnection(_conexion))
            {
                string query =
                    @"DELETE FROM RolPermiso
                      WHERE id_rol = @idRol
                        AND id_permiso = @idPermiso";

                SqlCommand cmd = new SqlCommand(query, conexion);

                cmd.Parameters.AddWithValue("@idRol", rol.Id);
                cmd.Parameters.AddWithValue("@idPermiso", permiso.Id);

                conexion.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public bool ExisteFamiliaEnRol(Rol rol, Familia familia)
        {
            using (SqlConnection conexion = new SqlConnection(_conexion))
            {
                string query =
                    @"SELECT COUNT(*)
                      FROM RolFamilia
                      WHERE id_rol = @idRol
                        AND id_familia = @idFamilia";

                SqlCommand cmd = new SqlCommand(query, conexion);

                cmd.Parameters.AddWithValue("@idRol", rol.Id);
                cmd.Parameters.AddWithValue("@idFamilia", familia.Id);

                conexion.Open();

                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        public void AsignarFamilia(Rol rol, Familia familia)
        {
            using (SqlConnection conexion = new SqlConnection(_conexion))
            {
                string query =
                    @"INSERT INTO RolFamilia
                      (id_rol, id_familia)
                      VALUES
                      (@idRol, @idFamilia)";

                SqlCommand cmd = new SqlCommand(query, conexion);

                cmd.Parameters.AddWithValue("@idRol", rol.Id);
                cmd.Parameters.AddWithValue("@idFamilia", familia.Id);

                conexion.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void EliminarFamilia(Rol rol, Familia familia)
        {
            using (SqlConnection conexion = new SqlConnection(_conexion))
            {
                string query =
                    @"DELETE FROM RolFamilia
                      WHERE id_rol = @idRol
                        AND id_familia = @idFamilia";

                SqlCommand cmd = new SqlCommand(query, conexion);

                cmd.Parameters.AddWithValue("@idRol", rol.Id);
                cmd.Parameters.AddWithValue("@idFamilia", familia.Id);

                conexion.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void EliminarRol(Rol rol)
        {
            using (SqlConnection conexion = new SqlConnection(_conexion))
            {
                conexion.Open();

                SqlTransaction transaccion =
                    conexion.BeginTransaction();

                try
                {
                    using (SqlCommand cmd = new SqlCommand(
                        @"DELETE FROM RolPermiso
                          WHERE id_rol = @id",
                        conexion,
                        transaccion))
                    {
                        cmd.Parameters.AddWithValue("@id", rol.Id);
                        cmd.ExecuteNonQuery();
                    }

                    using (SqlCommand cmd = new SqlCommand(
                        @"DELETE FROM RolFamilia
                          WHERE id_rol = @id",
                        conexion,
                        transaccion))
                    {
                        cmd.Parameters.AddWithValue("@id", rol.Id);
                        cmd.ExecuteNonQuery();
                    }

                    using (SqlCommand cmd = new SqlCommand(
                        @"DELETE FROM Rol
                          WHERE id_rol = @id",
                        conexion,
                        transaccion))
                    {
                        cmd.Parameters.AddWithValue("@id", rol.Id);
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
        public bool EstaAsignadoAUsuarios(Rol rol)
        {
            using (SqlConnection conexion = new SqlConnection(_conexion))
            {
                string query =
                    @"SELECT COUNT(*)
              FROM Usuario
              WHERE rol = @idRol";

                SqlCommand cmd = new SqlCommand(query, conexion);

                cmd.Parameters.AddWithValue("@idRol", rol.Id);

                conexion.Open();

                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }
    }
}