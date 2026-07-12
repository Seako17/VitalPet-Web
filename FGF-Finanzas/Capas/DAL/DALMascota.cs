using FGF_Finanzas.Capas.BE;
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
            return dt;
        }
        public void AgregarMascota(BEMascota mascota)
        {
            DataTable dt = ObtenerMascotas();
            dt.Rows.Add(new object[] { 1, mascota.usuario.DNI, mascota.nombre, mascota.especie, mascota.raza, mascota.fechaNacimiento });

            SqlDataAdapter adapter = new SqlDataAdapter("Select * from Mascota", _conexion);

            SqlCommandBuilder cb = new SqlCommandBuilder(adapter);

            adapter.Update(dt);
        }


    }
}