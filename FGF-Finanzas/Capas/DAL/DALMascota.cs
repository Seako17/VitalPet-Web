using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.Servicios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FGF_Finanzas.Capas.DAL
{
    public class DALMascota : DALAbstracta
    {
        public DataTable ObtenerMascotas()
        {
            string query = "SELECT * FROM Mascota";
            DataTable dt = new DataTable();

            SqlDataAdapter adapter = new SqlDataAdapter(query, _conexion);
            adapter.Fill(dt);
            dt = Encriptacion.DescifrarFechas(dt, "fechaNacimiento");
            Encriptacion.DescifrarTabla(dt, "Mascota");
            return dt;
        }
        public void AgregarMascota(BEMascota mascota)
        {
            using (var con = new SqlConnection(_conexion))
            using (var cmd = new SqlCommand(@"INSERT Mascota(dni,nombre,especie,raza,fechaNacimiento)
                VALUES(@Dni,@Nombre,@Especie,@Raza,@Fecha)", con))
            {
                cmd.Parameters.AddWithValue("@Dni", mascota.usuario.DNI);
                cmd.Parameters.AddWithValue("@Nombre", Encriptacion.EncriptarAES(mascota.nombre));
                cmd.Parameters.AddWithValue("@Especie", Encriptacion.EncriptarAES(mascota.especie));
                cmd.Parameters.AddWithValue("@Raza", Encriptacion.EncriptarAES(mascota.raza));
                cmd.Parameters.AddWithValue("@Fecha", Encriptacion.CifrarFecha(mascota.fechaNacimiento));
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }


    }
}