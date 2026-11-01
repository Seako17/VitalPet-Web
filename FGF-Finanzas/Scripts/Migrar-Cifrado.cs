// Utilidad de consola: compilar contra FGF-Finanzas.dll y usar su conexión DBConnection.
// Primer argumento: "verificar" para no modificar nada; "migrar" para respaldar y migrar.
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.DAL;
using FGF_Finanzas.Capas.BLL;
using FGF_Finanzas.Capas.Servicios;

class MigrarCifrado
{
    static readonly string Conexion = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
    static string Q(string nombre) { return "[" + nombre.Replace("]", "]]") + "]"; }
    static DataTable Leer(SqlConnection con, SqlTransaction tx, string sql)
    {
        var dt = new DataTable();
        using (var cmd = new SqlCommand(sql, con, tx))
        using (var a = new SqlDataAdapter(cmd)) a.Fill(dt);
        return dt;
    }
    static void Ejecutar(SqlConnection con, SqlTransaction tx, string sql)
    {
        using (var cmd = new SqlCommand(sql, con, tx)) { cmd.CommandTimeout = 300; cmd.ExecuteNonQuery(); }
    }
    static string Descifrar(string texto)
    {
        string claro = Encriptacion.DesencriptarAES(texto);
        // Reconocer exclusivamente valores que hacen el recorrido AES completo.
        return Encriptacion.EncriptarAES(claro) == texto ? claro : texto;
    }
    static void VerificarIntegridadAnterior(SqlConnection con)
    {
        var dal = new DALDigitoVerificador();
        foreach (DataRow control in Leer(con, null, "SELECT * FROM DigitoVerificador").Rows)
        {
            string tabla = control["NombreTabla"].ToString();
            DataTable datos = Leer(con, null, "SELECT * FROM " + Q(tabla));
            if (datos.Rows.Count != Convert.ToInt32(control["CantidadRegistros"]))
                throw new Exception("Cantidad inconsistente en " + tabla);
            BigInteger suma = BigInteger.Zero;
            foreach (DataRow r in datos.Rows)
            {
                var fila = new FilaGenerica();
                foreach (DataColumn c in datos.Columns)
                {
                    if (c.ColumnName.Equals("DV_Horizontal", StringComparison.OrdinalIgnoreCase)) continue;
                    object valor = r[c];
                    bool cifradoAnterior =
                        (tabla == "Usuario" && new[] { "nombre", "apellido", "usuario", "mail" }.Contains(c.ColumnName, StringComparer.OrdinalIgnoreCase)) ||
                        (tabla == "Producto" && new[] { "Nombre", "Descripcion", "Imagen" }.Contains(c.ColumnName, StringComparer.OrdinalIgnoreCase)) ||
                        (tabla == "Compra" && new[] { "TitularTarjeta", "Ultimos4" }.Contains(c.ColumnName, StringComparer.OrdinalIgnoreCase));
                    if (cifradoAnterior && valor != DBNull.Value) valor = Encriptacion.DesencriptarAES(valor.ToString());
                    fila.ValoresCampos.Add(valor);
                }
                string hash = dal.CalcularDVHorizontalFila(fila);
                if (hash != r["DV_Horizontal"].ToString()) throw new Exception("Integridad anterior inválida: " + tabla);
                suma += BigInteger.Parse("00" + Encriptacion.Encriptar(hash), NumberStyles.HexNumber);
            }
            if (suma.ToString("X") != control["DigitoVertical"].ToString()) throw new Exception("DV vertical inválido: " + tabla);
        }
    }
    static void Main(string[] args)
    {
        if (args.Length != 1 || (args[0] != "migrar" && args[0] != "verificar"))
            throw new ArgumentException("Indicar verificar o migrar.");
        using (var con = new SqlConnection(Conexion))
        {
            con.Open();
            // Distinguir el criterio de integridad anterior del criterio con descifrado.
            var esquema = Leer(con, null, "SELECT CHARACTER_MAXIMUM_LENGTH FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='Mascota' AND COLUMN_NAME='nombre'");
            bool migrada = Convert.ToInt32(esquema.Rows[0][0]) == 256;
            if (migrada)
            {
                if (!new BLLDigitoVerificador().ValidarIntegridadDelSistema()) throw new Exception("Integridad inválida.");
            }
            else VerificarIntegridadAnterior(con);
            Console.WriteLine("Integridad inicial válida.");
            if (args[0] == "verificar") return;

            string carpeta;
            using (var cmd = new SqlCommand("SELECT CONVERT(nvarchar(4000), SERVERPROPERTY('InstanceDefaultBackupPath'))", con))
                carpeta = Convert.ToString(cmd.ExecuteScalar());
            if (string.IsNullOrWhiteSpace(carpeta)) throw new Exception("No se encontró la carpeta de respaldos de SQL Server.");
            string respaldo = Path.Combine(carpeta, "VitalPet-antes-cifrado-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".bak");
            using (var cmd = new SqlCommand("BACKUP DATABASE [FGF-BDD] TO DISK=@Ruta WITH COPY_ONLY, CHECKSUM", con))
            { cmd.Parameters.AddWithValue("@Ruta", respaldo); cmd.CommandTimeout = 300; cmd.ExecuteNonQuery(); }
            using (var cmd = new SqlCommand("RESTORE VERIFYONLY FROM DISK=@Ruta WITH CHECKSUM", con))
            { cmd.Parameters.AddWithValue("@Ruta", respaldo); cmd.CommandTimeout = 300; cmd.ExecuteNonQuery(); }
            Console.WriteLine("Respaldo verificado: " + respaldo);

            using (var tx = con.BeginTransaction())
            try
            {
                // Mantener las claves y los tipos numéricos/fechas. Ampliar textos por expansión AES/Base64.
                foreach (string sql in new[] {
                    "ALTER TABLE Mascota ALTER COLUMN nombre nvarchar(256) NOT NULL",
                    "ALTER TABLE Mascota ALTER COLUMN especie nvarchar(256) NOT NULL",
                    "ALTER TABLE Mascota ALTER COLUMN raza nvarchar(256) NOT NULL",
                    "ALTER TABLE Evento ALTER COLUMN modulo nvarchar(256) NOT NULL",
                    "ALTER TABLE Evento ALTER COLUMN evento nvarchar(256) NOT NULL",
                    "ALTER TABLE Rol ALTER COLUMN nombre nvarchar(256) NOT NULL",
                    "ALTER TABLE Rol ALTER COLUMN descripcion nvarchar(max) NULL",
                    "ALTER TABLE Familia ALTER COLUMN nombre nvarchar(256) NOT NULL",
                    "ALTER TABLE Familia ALTER COLUMN descripcion nvarchar(max) NULL",
                    "ALTER TABLE Permiso ALTER COLUMN descripcion nvarchar(max) NULL",
                    "ALTER TABLE Idioma ALTER COLUMN Nombre nvarchar(256) NOT NULL",
                    "ALTER TABLE Compra ALTER COLUMN Estado nvarchar(128) NOT NULL",
                    "ALTER TABLE Usuario ALTER COLUMN rol_viejo nvarchar(256) NULL"
                }) Ejecutar(con, tx, sql);

                foreach (var entrada in Encriptacion.CamposCifrados)
                {
                    string tabla = entrada.Key;
                    DataTable datos = Leer(con, tx, "SELECT * FROM " + Q(tabla));
                    string pk = Leer(con, tx, "SELECT c.name FROM sys.indexes i JOIN sys.index_columns ic ON ic.object_id=i.object_id AND ic.index_id=i.index_id JOIN sys.columns c ON c.object_id=ic.object_id AND c.column_id=ic.column_id WHERE i.is_primary_key=1 AND i.object_id=OBJECT_ID('dbo." + tabla + "')").Rows[0][0].ToString();
                    foreach (DataRow fila in datos.Rows)
                    foreach (string campo in entrada.Value)
                    {
                        if (fila[campo] == DBNull.Value || fila[campo].ToString() == "") continue;
                        string anterior = fila[campo].ToString();
                        string claro = Descifrar(anterior);
                        string cifrado = Encriptacion.EncriptarAES(claro);
                        if (Encriptacion.DesencriptarAES(cifrado) != claro) throw new Exception("Error de ida y vuelta: " + tabla + "." + campo);
                        if (cifrado == anterior) continue;
                        using (var cmd = new SqlCommand("UPDATE " + Q(tabla) + " SET " + Q(campo) + "=@Valor WHERE " + Q(pk) + "=@Id", con, tx))
                        { cmd.Parameters.AddWithValue("@Valor", cifrado); cmd.Parameters.AddWithValue("@Id", fila[pk]); if (cmd.ExecuteNonQuery() != 1) throw new Exception("Registro no actualizado."); }
                    }
                    DataTable despues = Leer(con, tx, "SELECT * FROM " + Q(tabla));
                    if (datos.Rows.Count != despues.Rows.Count) throw new Exception("Cambió cantidad de registros.");
                    var porId = despues.AsEnumerable().ToDictionary(x => x[pk].ToString());
                    foreach (DataRow antes in datos.Rows)
                    foreach (DataColumn col in datos.Columns)
                    {
                        object a = antes[col], b = porId[antes[pk].ToString()][col.ColumnName];
                        if (entrada.Value.Contains(col.ColumnName, StringComparer.OrdinalIgnoreCase) && a != DBNull.Value)
                        { a = Descifrar(a.ToString()); b = Descifrar(b.ToString()); }
                        if (!object.Equals(a, b)) throw new Exception("Cambió un valor: " + tabla + "." + col.ColumnName);
                    }
                    Console.WriteLine(tabla + ": " + datos.Rows.Count + " registros conservados.");
                }
                var dal = new DALDigitoVerificador();
                foreach (DataRow fila in Leer(con, tx, "SELECT NombreTabla FROM DigitoVerificador").Rows)
                    dal.ActualizarIntegridadTabla(fila[0].ToString(), con, tx);
                tx.Commit();
            }
            catch { tx.Rollback(); throw; }
        }
        if (!new BLLDigitoVerificador().ValidarIntegridadDelSistema()) throw new Exception("Integridad final inválida.");
        Console.WriteLine("Migración terminada. Integridad válida.");
    }
}
