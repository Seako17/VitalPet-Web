using FGF_Finanzas.Capas.BE;
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
            return dt;
        }
        public void AgregarEvento(BEEvento evento)
        {
            DataTable dt = ObtenerEventos();
            dt.Rows.Add(new object[] { evento.usuario.DNI, evento.fechaHora, evento.modulo, evento.evento, evento.criticidad });

            SqlDataAdapter adapter = new SqlDataAdapter("Select * from Evento", _conexion);

            SqlCommandBuilder cb = new SqlCommandBuilder(adapter);

            adapter.Update(dt);
        }
    }
}