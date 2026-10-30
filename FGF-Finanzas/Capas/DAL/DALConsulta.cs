using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.Servicios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

namespace FGF_Finanzas.Capas.DAL
{
    public class DALConsulta : DALAbstracta
    {
        private const string FORMATO_FECHA = "yyyy-MM-dd HH:mm";
        private const string FORMATO_CANCELACION = "yyyy-MM-dd HH:mm:ss.fffffff";

        public DataTable ObtenerConsultas()
        {
            DataTable crudo = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Consulta", _conexion);
            adapter.Fill(crudo);

            return InterpretarConsultas(crudo);
        }

        private DataTable InterpretarConsultas(DataTable crudo)
        {
            DataTable dt = crudo.Clone();
            dt.Columns["fechaHora"].DataType = typeof(DateTime);
            if (dt.Columns.Contains("fechaCancelacion"))
                dt.Columns["fechaCancelacion"].DataType = typeof(DateTime);

            foreach (DataRow fila in crudo.Rows)
            {
                DataRow nueva = dt.NewRow();
                foreach (DataColumn col in crudo.Columns)
                {
                    string nombre = col.ColumnName;

                    if (nombre.Equals("motivo", StringComparison.OrdinalIgnoreCase) ||
                        nombre.Equals("estado", StringComparison.OrdinalIgnoreCase) ||
                        nombre.Equals("diagnostico", StringComparison.OrdinalIgnoreCase) ||
                        nombre.Equals("nombreVeterinario", StringComparison.OrdinalIgnoreCase) ||
                        nombre.Equals("apellidoVeterinario", StringComparison.OrdinalIgnoreCase))
                    {
                        nueva[nombre] = Encriptacion.DesencriptarAES(fila[nombre].ToString());
                    }
                    else if (nombre.Equals("fechaHora", StringComparison.OrdinalIgnoreCase))
                    {
                        nueva[nombre] = LeerFecha(Encriptacion.DesencriptarAES(fila[nombre].ToString()));
                    }
                    else if (nombre.Equals("fechaCancelacion", StringComparison.OrdinalIgnoreCase))
                    {
                        nueva[nombre] = fila.IsNull(col) ? (object)DBNull.Value : DateTime.ParseExact(
                            Encriptacion.DesencriptarAES(fila[nombre].ToString()), FORMATO_CANCELACION, CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        nueva[nombre] = fila[nombre];
                    }
                }
                dt.Rows.Add(nueva);
            }

            dt.AcceptChanges();
            return dt;
        }

        public void AgregarConsulta(BEConsulta consulta)
        {
            const string query = @"INSERT INTO Consulta (idMascota, DNI_Veterinario, motivo, fechaHora, estado)
                                   VALUES (@IdMascota, @DniVeterinario, @Motivo, @FechaHora, @Estado)";

            using (SqlConnection con = new SqlConnection(_conexion))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@IdMascota", consulta.idMascota);
                cmd.Parameters.AddWithValue("@DniVeterinario", consulta.veterinario.DNI);
                cmd.Parameters.AddWithValue("@Motivo", Encriptacion.EncriptarAES(consulta.motivo));
                cmd.Parameters.AddWithValue("@Estado", Encriptacion.EncriptarAES(BEConsulta.Agendada));
                cmd.Parameters.AddWithValue("@FechaHora",
                    Encriptacion.EncriptarAES(consulta.fechaHora.ToString(FORMATO_FECHA, CultureInfo.InvariantCulture)));
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public DataTable ObtenerConsultasDeCliente(string dni)
        {
            const string query = @"SELECT c.*, m.nombre AS nombreMascota,
                u.nombre AS nombreVeterinario, u.apellido AS apellidoVeterinario
                FROM Consulta c INNER JOIN Mascota m ON m.id_mascota = c.idMascota
                INNER JOIN Usuario u ON u.dni = c.DNI_Veterinario WHERE m.dni = @Dni";
            using (var con = new SqlConnection(_conexion))
            using (var cmd = new SqlCommand(query, con))
            using (var adapter = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.Add("@Dni", SqlDbType.NVarChar, 8).Value = dni;
                var datos = new DataTable();
                adapter.Fill(datos);
                return InterpretarConsultas(datos);
            }
        }

        public bool CancelarConsulta(int idConsulta, string dni, DateTime ahora)
        {
            using (var con = new SqlConnection(_conexion))
            {
                con.Open();
                using (var tx = con.BeginTransaction(IsolationLevel.Serializable))
                {
                    const string buscar = @"SELECT c.fechaHora FROM Consulta c WITH (UPDLOCK)
                        INNER JOIN Mascota m ON m.id_mascota = c.idMascota
                        WHERE c.idConsulta = @Id AND m.dni = @Dni AND c.estado = @Estado";
                    using (var cmd = new SqlCommand(buscar, con, tx))
                    {
                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = idConsulta;
                        cmd.Parameters.Add("@Dni", SqlDbType.NVarChar, 8).Value = dni;
                        cmd.Parameters.Add("@Estado", SqlDbType.NVarChar, 128).Value = Encriptacion.EncriptarAES(BEConsulta.Agendada);
                        var fecha = cmd.ExecuteScalar();
                        if (fecha == null || LeerFecha(Encriptacion.DesencriptarAES(fecha.ToString())) <= ahora)
                        {
                            tx.Rollback();
                            return false;
                        }
                    }
                    using (var cmd = new SqlCommand(@"UPDATE Consulta SET estado = @Estado,
                        fechaCancelacion = @Ahora, canceladaPor = @Dni WHERE idConsulta = @Id", con, tx))
                    {
                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = idConsulta;
                        cmd.Parameters.Add("@Dni", SqlDbType.NVarChar, 8).Value = dni;
                        cmd.Parameters.Add("@Estado", SqlDbType.NVarChar, 128).Value = Encriptacion.EncriptarAES(BEConsulta.Cancelada);
                        cmd.Parameters.Add("@Ahora", SqlDbType.NVarChar, -1).Value = Encriptacion.EncriptarAES(
                            ahora.ToString(FORMATO_CANCELACION, CultureInfo.InvariantCulture));
                        cmd.ExecuteNonQuery();
                    }
                    tx.Commit();
                    return true;
                }
            }
        }

        private DateTime LeerFecha(string texto)
        {
            DateTime fecha;
            if (!DateTime.TryParseExact(texto, FORMATO_FECHA, CultureInfo.InvariantCulture, DateTimeStyles.None, out fecha))
            {
                throw new Exception("Error al leer la fecha de una consulta: el dato guardado no es válido.");
            }
            return fecha;
        }
    }
}
