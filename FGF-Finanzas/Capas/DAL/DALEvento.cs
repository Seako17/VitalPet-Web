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
    public class DALEvento : DALAbstracta
    {
        public DataTable ObtenerEventos()
        {
            string query = "SELECT * FROM Evento";
            DataTable dt = new DataTable();

            SqlDataAdapter adapter = new SqlDataAdapter(query, _conexion);
            adapter.Fill(dt);
            dt = Encriptacion.DescifrarFechas(dt, "fechaHora");
            Encriptacion.DescifrarTabla(dt, "Evento");
            return dt;
        }
        public void AgregarEvento(BEEvento evento)
        {
            using (var con = new SqlConnection(_conexion))
            using (var cmd = new SqlCommand(@"INSERT Evento(dni,fechaHora,modulo,evento,criticidad)
                VALUES(@Dni,@Fecha,@Modulo,@Evento,@Criticidad)", con))
            {
                cmd.Parameters.AddWithValue("@Dni", evento.usuario.DNI);
                cmd.Parameters.AddWithValue("@Fecha", Encriptacion.CifrarFecha(evento.fechaHora));
                cmd.Parameters.AddWithValue("@Modulo", Encriptacion.EncriptarAES(evento.modulo));
                cmd.Parameters.AddWithValue("@Evento", Encriptacion.EncriptarAES(evento.evento));
                cmd.Parameters.AddWithValue("@Criticidad", evento.criticidad);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}