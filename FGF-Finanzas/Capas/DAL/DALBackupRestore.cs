using System.Configuration;
using System.Data.SqlClient;

namespace FGF_Finanzas.Capas.DAL
{
    public class DALBackupRestore : DALAbstracta
    {
        public void RealizarBackup(string rutaServidor)
        {
            _conexion = ConfigurationManager.ConnectionStrings["MasterDbConn"].ConnectionString;
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                string query = $"BACKUP DATABASE [FGF-BDD] TO DISK='{rutaServidor}' WITH FORMAT, INIT;";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.CommandTimeout = 600;
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

}

