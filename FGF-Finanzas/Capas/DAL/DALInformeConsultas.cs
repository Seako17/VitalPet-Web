using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.Servicios;
using System;
using System.Data;
using System.Data.SqlClient;

namespace FGF_Finanzas.Capas.DAL
{
    public class DALInformeConsultas : DALAbstracta
    {
        public DataTable ObtenerVeterinarios()
        {
            var tabla = new DataTable();
            using (var con = new SqlConnection(_conexion))
            using (var adapter = new SqlDataAdapter(@"SELECT u.dni, u.nombre, u.apellido FROM Usuario u
                JOIN Rol r ON r.id_rol = u.rol WHERE r.nombre = 'Veterinario'
                OR EXISTS(SELECT 1 FROM Consulta c WHERE c.DNI_Veterinario = u.dni)", con)) adapter.Fill(tabla);
            tabla.Columns.Add("NombreCompleto", typeof(string));
            foreach (DataRow fila in tabla.Rows)
                fila["NombreCompleto"] = Encriptacion.DesencriptarAES(fila["nombre"].ToString()) + " " + Encriptacion.DesencriptarAES(fila["apellido"].ToString());
            return tabla;
        }

        public DataTable ObtenerDatos()
        {
            var tabla = new DataTable();
            using (var con = new SqlConnection(_conexion))
            using (var adapter = new SqlDataAdapter(@"SELECT c.DNI_Veterinario, c.fechaHora, c.estado, u.nombre, u.apellido
                FROM Consulta c JOIN Usuario u ON u.dni = c.DNI_Veterinario", con)) adapter.Fill(tabla);
            return tabla;
        }

        public int Guardar(BEInformeConsultas informe, string dniUsuario)
        {
            using (var con = new SqlConnection(_conexion))
            {
                con.Open();
                using (var tx = con.BeginTransaction())
                {
                    using (var cmd = new SqlCommand(@"INSERT InformeConsultas(Anio, Mes, DniVeterinario, NombreVeterinario, Generado, GeneradoPor)
                        VALUES(@Anio,@Mes,@Dni,@Nombre,@Generado,@Usuario); SELECT CAST(SCOPE_IDENTITY() AS int)", con, tx))
                    {
                        cmd.Parameters.AddWithValue("@Anio", informe.Anio);
                        cmd.Parameters.AddWithValue("@Mes", informe.Mes);
                        cmd.Parameters.AddWithValue("@Dni", (object)informe.DniVeterinario ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Nombre", (object)Encriptacion.EncriptarAES(informe.NombreVeterinario) ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Generado", informe.Generado);
                        cmd.Parameters.AddWithValue("@Usuario", dniUsuario);
                        informe.Id = (int)cmd.ExecuteScalar();
                    }
                    foreach (var fila in informe.Detalle)
                    using (var cmd = new SqlCommand(@"INSERT InformeConsultasDetalle(IdInforme,DniVeterinario,NombreVeterinario,Turnos,Atendidos,Cancelados,Ausentes,Pendientes)
                        VALUES(@Informe,@Dni,@Nombre,@Turnos,@Atendidos,@Cancelados,@Ausentes,@Pendientes)", con, tx))
                    {
                        cmd.Parameters.AddWithValue("@Informe", informe.Id);
                        cmd.Parameters.AddWithValue("@Dni", fila.Dni);
                        cmd.Parameters.AddWithValue("@Nombre", Encriptacion.EncriptarAES(fila.Veterinario));
                        cmd.Parameters.AddWithValue("@Turnos", fila.Turnos);
                        cmd.Parameters.AddWithValue("@Atendidos", fila.Atendidos);
                        cmd.Parameters.AddWithValue("@Cancelados", fila.Cancelados);
                        cmd.Parameters.AddWithValue("@Ausentes", fila.Ausentes);
                        cmd.Parameters.AddWithValue("@Pendientes", fila.Pendientes);
                        cmd.ExecuteNonQuery();
                    }
                    foreach (var mes in informe.Meses)
                    using (var cmd = new SqlCommand("INSERT InformeConsultasMes(IdInforme,Mes,Atendidos) VALUES(@Informe,@Mes,@Atendidos)", con, tx))
                    {
                        cmd.Parameters.AddWithValue("@Informe", informe.Id);
                        cmd.Parameters.AddWithValue("@Mes", mes.Mes);
                        cmd.Parameters.AddWithValue("@Atendidos", mes.Atendidos);
                        cmd.ExecuteNonQuery();
                    }
                    tx.Commit();
                    return informe.Id;
                }
            }
        }

        public DataTable Listar()
        {
            var tabla = new DataTable();
            using (var con = new SqlConnection(_conexion))
            using (var adapter = new SqlDataAdapter("SELECT IdInforme,Anio,Mes,NombreVeterinario,Generado FROM InformeConsultas ORDER BY Generado DESC,IdInforme DESC", con)) adapter.Fill(tabla);
            foreach (DataRow fila in tabla.Rows)
                fila["NombreVeterinario"] = Encriptacion.DesencriptarAES(fila["NombreVeterinario"].ToString());
            return tabla;
        }

        public BEInformeConsultas Obtener(int id)
        {
            using (var con = new SqlConnection(_conexion))
            {
                con.Open();
                BEInformeConsultas informe;
                using (var cmd = new SqlCommand("SELECT * FROM InformeConsultas WHERE IdInforme=@Id", con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read()) return null;
                        informe = new BEInformeConsultas { Id = id, Anio = (int)reader["Anio"], Mes = (int)reader["Mes"],
                            DniVeterinario = reader["DniVeterinario"] as string,
                            NombreVeterinario = reader["NombreVeterinario"] == DBNull.Value ? null : Encriptacion.DesencriptarAES(reader["NombreVeterinario"].ToString()), Generado = (DateTime)reader["Generado"] };
                    }
                }
                using (var cmd = new SqlCommand("SELECT * FROM InformeConsultasDetalle WHERE IdInforme=@Id ORDER BY Atendidos DESC,NombreVeterinario", con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (var reader = cmd.ExecuteReader())
                    while (reader.Read()) informe.Detalle.Add(new BEInformeVeterinario {
                        Dni = reader["DniVeterinario"].ToString(), Veterinario = Encriptacion.DesencriptarAES(reader["NombreVeterinario"].ToString()),
                        Turnos = (int)reader["Turnos"], Atendidos = (int)reader["Atendidos"], Cancelados = (int)reader["Cancelados"],
                        Ausentes = (int)reader["Ausentes"], Pendientes = (int)reader["Pendientes"] });
                }
                using (var cmd = new SqlCommand("SELECT Mes,Atendidos FROM InformeConsultasMes WHERE IdInforme=@Id ORDER BY Mes", con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (var reader = cmd.ExecuteReader())
                        while (reader.Read()) informe.Meses.Add(new BEInformeMes { Mes = (int)reader["Mes"], Atendidos = (int)reader["Atendidos"] });
                }
                return informe;
            }
        }
    }
}
