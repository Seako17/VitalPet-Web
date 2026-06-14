using System.Configuration;
using System.Data.SqlClient;

namespace FGF_Finanzas.Capas.DAL
{
    public class DALBackupRestore : DALAbstracta
    {
        private readonly string _conexionMaster = ConfigurationManager.ConnectionStrings["MasterDbConn"].ConnectionString;
        public void RealizarBackup(string rutaServidor)
        {

            using (SqlConnection con = new SqlConnection(_conexionMaster))
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

        public void RestaurarBDD(string rutaArchivo)
        {
            // Construimos todo el script en un único bloque de ejecución transaccional
            string query = @"
                USE master;
                ALTER DATABASE [FGF-BDD] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                RESTORE DATABASE [FGF-BDD] FROM DISK = @ruta WITH REPLACE;
                ALTER DATABASE [FGF-BDD] SET MULTI_USER;";

            using (SqlConnection con = new SqlConnection(_conexionMaster))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ruta", rutaArchivo);

                    cmd.CommandTimeout = 120;
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

}