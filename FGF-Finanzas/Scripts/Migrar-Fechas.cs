// Ejecutar después de Migrar-Cifrado.cs con la misma conexión DBConnection.
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using FGF_Finanzas.Capas.BLL;
using FGF_Finanzas.Capas.DAL;
using FGF_Finanzas.Capas.Servicios;

class MigrarFechas
{
    static DataTable Leer(SqlConnection con, SqlTransaction tx, string sql)
    {
        var dt = new DataTable();
        using (var cmd = new SqlCommand(sql, con, tx))
        using (var a = new SqlDataAdapter(cmd)) a.Fill(dt);
        return dt;
    }
    static void Main()
    {
        var dv = new BLLDigitoVerificador();
        if (!dv.ValidarIntegridadDelSistema()) throw new Exception("Integridad inicial inválida. Sin cambios.");
        using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString))
        {
            con.Open();
            string carpeta;
            using (var cmd = new SqlCommand("SELECT CONVERT(nvarchar(4000),SERVERPROPERTY('InstanceDefaultBackupPath'))", con)) carpeta = Convert.ToString(cmd.ExecuteScalar());
            if (string.IsNullOrWhiteSpace(carpeta)) throw new Exception("Sin carpeta de respaldos.");
            string archivo = Path.Combine(carpeta, "VitalPet-antes-fechas-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".bak");
            foreach (string sql in new[] { "BACKUP DATABASE [FGF-BDD] TO DISK=@Ruta WITH COPY_ONLY,CHECKSUM", "RESTORE VERIFYONLY FROM DISK=@Ruta WITH CHECKSUM" })
            using (var cmd = new SqlCommand(sql, con)) { cmd.CommandTimeout = 300; cmd.Parameters.AddWithValue("@Ruta", archivo); cmd.ExecuteNonQuery(); }
            Console.WriteLine("Respaldo verificado: " + archivo);
            using (var tx = con.BeginTransaction())
            try
            {
                foreach (var par in Encriptacion.CamposFecha)
                {
                    string tabla = par.Key;
                    var datos = Leer(con, tx, "SELECT * FROM [" + tabla + "]");
                    string pk = datos.Columns[0].ColumnName;
                    foreach (string campo in par.Value)
                    {
                        // Solo la primera ejecución convierte la columna de date/datetime a texto.
                        if (datos.Columns[campo].DataType != typeof(DateTime)) continue;
                        using (var cmd = new SqlCommand("ALTER TABLE [" + tabla + "] ALTER COLUMN [" + campo + "] nvarchar(max) NOT NULL", con, tx)) cmd.ExecuteNonQuery();
                        foreach (DataRow fila in datos.Rows)
                        using (var cmd = new SqlCommand("UPDATE [" + tabla + "] SET [" + campo + "]=@Fecha WHERE [" + pk + "]=@Id", con, tx))
                        {
                            cmd.Parameters.AddWithValue("@Fecha", Encriptacion.CifrarFecha((DateTime)fila[campo]));
                            cmd.Parameters.AddWithValue("@Id", fila[pk]);
                            if (cmd.ExecuteNonQuery() != 1) throw new Exception("Fecha no actualizada.");
                        }
                    }
                    var despues = Leer(con, tx, "SELECT * FROM [" + tabla + "]");
                    if (datos.Rows.Count != despues.Rows.Count) throw new Exception("Cambió cantidad de filas.");
                    var porId = despues.AsEnumerable().ToDictionary(x => x[pk].ToString());
                    foreach (DataRow antes in datos.Rows)
                    foreach (DataColumn col in datos.Columns)
                    {
                        object a = antes[col], b = porId[antes[pk].ToString()][col.ColumnName];
                        if (par.Value.Contains(col.ColumnName)) { a = Encriptacion.DescifrarFecha(a); b = Encriptacion.DescifrarFecha(b); }
                        if (!object.Equals(a,b)) throw new Exception("Cambió un dato: " + tabla + "." + col.ColumnName);
                    }
                    Console.WriteLine(tabla + ": fechas y demás valores conservados.");
                }
                var dal = new DALDigitoVerificador();
                foreach (DataRow fila in Leer(con, tx, "SELECT NombreTabla FROM DigitoVerificador").Rows)
                    dal.ActualizarIntegridadTabla(fila[0].ToString(), con, tx);
                tx.Commit();
            }
            catch { tx.Rollback(); throw; }
        }
        if (!dv.ValidarIntegridadDelSistema()) throw new Exception("Integridad final inválida.");
        Console.WriteLine("Fechas cifradas. Integridad válida.");
    }
}
