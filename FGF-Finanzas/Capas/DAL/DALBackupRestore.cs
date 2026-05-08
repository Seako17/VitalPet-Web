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
            using (SqlConnection con = new SqlConnection(_conexionMaster))
            {

                con.Open();
                using (SqlCommand setMaster = new SqlCommand("USE master", con))
                {
                    setMaster.ExecuteNonQuery();
                }
                using (SqlCommand setSingleUser = new SqlCommand("ALTER DATABASE [FGF-BDD] SET SINGLE_USER WITH ROLLBACK IMMEDIATE", con))
                {
                    setSingleUser.ExecuteNonQuery();
                }
                using (SqlCommand cmd = new SqlCommand($"RESTORE DATABASE [FGF-BDD] FROM DISK='{rutaArchivo}' WITH REPLACE", con))
                {
                    cmd.ExecuteNonQuery();
                }
                using (SqlCommand setMultiUser = new SqlCommand("ALTER DATABASE [FGF-BDD] SET MULTI_USER", con))
                {
                    setMultiUser.ExecuteNonQuery();
                }
            }
        }
    }

}