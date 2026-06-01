using FGF_Finanzas.Capas.BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

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
            dt.Rows.Add(new object[] { usuario.DNI, usuario.Nombre, usuario.Apellido, usuario.Usuario, usuario.Contraseña, usuario.Intento, usuario.Bloqueado, usuario.Mail });

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
                            bloqueado = @Bloqueado, usuario = @Usuario, mail = @Mail WHERE DNI = @Dni";

                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Dni", usuario.DNI);
                cmd.Parameters.AddWithValue("@Usuario", usuario.Usuario);
                cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                cmd.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                cmd.Parameters.AddWithValue("@Intento", usuario.Intento);
                cmd.Parameters.AddWithValue("@Contraseña", usuario.Contraseña);
                cmd.Parameters.AddWithValue("@Bloqueado", usuario.Bloqueado);
                cmd.Parameters.AddWithValue("@Mail", usuario.Mail);


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
    }
}