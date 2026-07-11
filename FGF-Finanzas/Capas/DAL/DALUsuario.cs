using FGF_Finanzas.Capas.BE;
using System;
using System.Data;
using System.Data.SqlClient;

namespace FGF_Finanzas.Capas.DAL
{
    public class DALUsuario : DALAbstracta
    {
        public DALUsuario() { }

        public DataTable ObtenerUsuarios()
        {
            string query = "SELECT * FROM Usuario";
            DataTable dt = new DataTable();

            SqlDataAdapter adapter = new SqlDataAdapter(query, _conexion);
            adapter.Fill(dt);
            return dt;
        }

        public void AgregarUsuario(BEUsuario usuario)
        {
            DataTable dt = ObtenerUsuarios();
            dt.Rows.Add(new object[] { usuario.DNI, usuario.Nombre, usuario.Apellido, usuario.Usuario, usuario.Contraseña, usuario.Intento, usuario.Bloqueado, usuario.Mail, usuario.Rol });

            SqlDataAdapter adapter = new SqlDataAdapter("Select * from Usuario", _conexion);

            SqlCommandBuilder cb = new SqlCommandBuilder(adapter);

            adapter.Update(dt);
        }

        public void Actualizar(BEUsuario usuario)
        {
            SqlConnection con = new SqlConnection(_conexion);
            SqlCommand cmd;

            try
            {
                con.Open();
                string query = @"UPDATE Usuario
                         SET contraseña = @Contraseña, intento = @Intento, nombre = @Nombre, apellido = @Apellido,
                            bloqueado = @Bloqueado, usuario = @Usuario, mail = @Mail, rol = @Rol WHERE DNI = @Dni";

                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Dni", usuario.DNI);
                cmd.Parameters.AddWithValue("@Usuario", usuario.Usuario);
                cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                cmd.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                cmd.Parameters.AddWithValue("@Intento", usuario.Intento);
                cmd.Parameters.AddWithValue("@Contraseña", usuario.Contraseña);
                cmd.Parameters.AddWithValue("@Bloqueado", usuario.Bloqueado);
                cmd.Parameters.AddWithValue("@Mail", usuario.Mail);
                cmd.Parameters.AddWithValue("@Rol", usuario.Rol);


                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception("Error al actualizar usuario", e);
            }
            finally
            {
                con.Close();
            }
        }

        public BEUsuario ConsultaIndividual(string dni)
        {
            BEUsuario usuarioEncontrado = null;
            SqlConnection con = new SqlConnection(_conexion);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                con.Open();
                string query = "SELECT DNI, nombre, apellido, usuario, contraseña, intento, bloqueado, mail, rol FROM Usuario WHERE DNI = @Dni";

                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Dni", dni);
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    usuarioEncontrado = new BEUsuario();
                    usuarioEncontrado.DNI = reader["DNI"].ToString();
                    usuarioEncontrado.Nombre = reader["nombre"].ToString();
                    usuarioEncontrado.Apellido = reader["apellido"].ToString();
                    usuarioEncontrado.Usuario = reader["usuario"].ToString();
                    usuarioEncontrado.Contraseña = reader["contraseña"].ToString();
                    usuarioEncontrado.Intento = Convert.ToInt32(reader["intento"]);
                    usuarioEncontrado.Bloqueado = Convert.ToBoolean(reader["bloqueado"]);
                    usuarioEncontrado.Mail = reader["mail"].ToString();
                    usuarioEncontrado.Rol = reader["rol"].ToString();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error al consultar el usuario de forma individual", e);
            }
            finally
            {
                if (reader != null) reader.Close();
                con.Close();
            }

            return usuarioEncontrado;
        }

        public void ActualizarContraseña(string dni, string nuevaContraseñaEncriptada)
        {
            SqlConnection con = new SqlConnection(_conexion);
            SqlCommand cmd;

            try
            {
                con.Open();
                string query = "UPDATE Usuario SET contraseña = @Contraseña WHERE DNI = @Dni";

                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Dni", dni);
                cmd.Parameters.AddWithValue("@Contraseña", nuevaContraseñaEncriptada);

                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception("Error al actualizar la contraseña del usuario", e);
            }
            finally
            {
                con.Close();
            }
        }

        public void ActualizarIntentosYBloqueo(string dni, int intentos, bool bloqueado)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                string query = @"UPDATE Usuario 
                         SET intento = @intentos, 
                             bloqueado = @bloqueado 
                         WHERE dni = @dni";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@intentos", intentos);
                    cmd.Parameters.AddWithValue("@bloqueado", bloqueado);
                    cmd.Parameters.AddWithValue("@dni", dni);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}