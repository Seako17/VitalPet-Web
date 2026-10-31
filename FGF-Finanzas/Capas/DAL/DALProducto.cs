using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.Servicios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Numerics;

namespace FGF_Finanzas.Capas.DAL
{
    public class DALProducto : DALAbstracta
    {
        private static void Param(SqlCommand c, string name, object value, SqlDbType type, int size = 0)
        {
            SqlParameter p = size == 0 ? c.Parameters.Add(name, type) : c.Parameters.Add(name, type, size);
            p.Value = value ?? DBNull.Value;
        }

        public List<BEProducto> ObtenerTodos()
        {
            return ObtenerProductos(string.Empty);
        }

        public BEProducto ObtenerPorId(int id)
        {
            using (SqlConnection c = new SqlConnection(_conexion))
            using (SqlCommand q = new SqlCommand("SELECT IdProducto,Nombre,Descripcion,Precio,Stock,Imagen FROM Producto WHERE IdProducto=@id", c))
            {
                Param(q, "@id", id, SqlDbType.Int);
                c.Open();
                using (SqlDataReader r = q.ExecuteReader())
                    return r.Read() ? LeerProducto(r) : null;
            }
        }

        private List<BEProducto> ObtenerProductos(string filtro)
        {
            List<BEProducto> productos = new List<BEProducto>();
            using (SqlConnection c = new SqlConnection(_conexion))
            using (SqlCommand q = new SqlCommand("SELECT IdProducto,Nombre,Descripcion,Precio,Stock,Imagen FROM Producto " + filtro + " ORDER BY IdProducto DESC", c))
            {
                c.Open();
                using (SqlDataReader r = q.ExecuteReader()) while (r.Read()) productos.Add(LeerProducto(r));
            }
            return productos;
        }

        private static BEProducto LeerProducto(SqlDataReader r)
        {
            return new BEProducto
            {
                IdProducto = Convert.ToInt32(r["IdProducto"]),
                Nombre = Encriptacion.DesencriptarAES(Convert.ToString(r["Nombre"])),
                Descripcion = Encriptacion.DesencriptarAES(Convert.ToString(r["Descripcion"])),
                Precio = Convert.ToDecimal(r["Precio"]),
                Stock = Convert.ToInt32(r["Stock"]),
                Imagen = r["Imagen"] == DBNull.Value ? null : Encriptacion.DesencriptarAES(Convert.ToString(r["Imagen"]))
            };
        }

        public void Guardar(BEProducto producto)
        {
            using (SqlConnection c = new SqlConnection(_conexion))
            {
                c.Open();
                using (SqlTransaction tx = c.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        if (producto.IdProducto > 0)
                        {
                            const string sql = "UPDATE Producto SET Nombre=@n,Descripcion=@d,Precio=@p,Stock=@s,Imagen=COALESCE(@i,Imagen) WHERE IdProducto=@id";
                            using (SqlCommand q = new SqlCommand(sql, c, tx))
                            {
                                AgregarParametrosProducto(q, producto);
                                Param(q, "@id", producto.IdProducto, SqlDbType.Int);
                                if (q.ExecuteNonQuery() != 1) throw new InvalidOperationException("El producto ya no existe.");
                            }
                        }
                        else
                        {
                            const string sql = "INSERT Producto(Nombre,Descripcion,Precio,Stock,Imagen) VALUES(@n,@d,@p,@s,@i)";
                            using (SqlCommand q = new SqlCommand(sql, c, tx)) { AgregarParametrosProducto(q, producto); q.ExecuteNonQuery(); }
                        }
                        ActualizarDVEnTransaccion(c, tx, "Producto");
                        tx.Commit();
                    }
                    catch { tx.Rollback(); throw; }
                }
            }
        }

        private static void AgregarParametrosProducto(SqlCommand q, BEProducto p)
        {
            Param(q, "@n", Encriptacion.EncriptarAES(p.Nombre), SqlDbType.NVarChar, -1);
            Param(q, "@d", Encriptacion.EncriptarAES(p.Descripcion), SqlDbType.NVarChar, -1);
            Param(q, "@p", p.Precio, SqlDbType.Decimal); q.Parameters["@p"].Precision = 10; q.Parameters["@p"].Scale = 2;
            Param(q, "@s", p.Stock, SqlDbType.Int);
            Param(q, "@i", p.Imagen == null ? null : Encriptacion.EncriptarAES(p.Imagen), SqlDbType.NVarChar, -1);
        }

        public void Eliminar(int id)
        {
            using (SqlConnection c = new SqlConnection(_conexion))
            {
                c.Open(); using (SqlTransaction tx = c.BeginTransaction(IsolationLevel.Serializable))
                try
                {
                    using (SqlCommand q = new SqlCommand("DELETE FROM CarritoItem WHERE IdProducto=@id; DELETE FROM Producto WHERE IdProducto=@id; IF @@ROWCOUNT=0 THROW 51001,'El producto ya no existe',1", c, tx)) { Param(q, "@id", id, SqlDbType.Int); q.ExecuteNonQuery(); }
                    ActualizarDVEnTransaccion(c, tx, "Producto"); ActualizarDVEnTransaccion(c, tx, "CarritoItem"); ActualizarDVEnTransaccion(c, tx, "CompraItem"); tx.Commit();
                }
                catch { tx.Rollback(); throw; }
            }
        }
        public List<BECarritoItem> ObtenerCarrito(string dni)
        {
            List<BECarritoItem> items = new List<BECarritoItem>();
            const string sql = "SELECT p.IdProducto,p.Nombre,p.Descripcion,p.Precio,ci.Cantidad,p.Imagen FROM CarritoItem ci JOIN Producto p ON p.IdProducto=ci.IdProducto WHERE ci.DNI=@dni ORDER BY ci.IdItem";
            using (SqlConnection c = new SqlConnection(_conexion)) using (SqlCommand q = new SqlCommand(sql, c))
            {
                Param(q, "@dni", dni, SqlDbType.NVarChar, 8); c.Open(); using (SqlDataReader r = q.ExecuteReader()) while (r.Read())
                    items.Add(new BECarritoItem { IdProducto = Convert.ToInt32(r["IdProducto"]), Nombre = Encriptacion.DesencriptarAES(Convert.ToString(r["Nombre"])), Descripcion = Encriptacion.DesencriptarAES(Convert.ToString(r["Descripcion"])), Precio = Convert.ToDecimal(r["Precio"]), Cantidad = Convert.ToInt32(r["Cantidad"]), Imagen = r["Imagen"] == DBNull.Value ? null : Encriptacion.DesencriptarAES(Convert.ToString(r["Imagen"])) });
            }
            return items;
        }

        public void AgregarAlCarrito(string dni, int idProducto, int cantidad)
        {
            const string sql = "IF NOT EXISTS(SELECT 1 FROM Producto p WITH(UPDLOCK,HOLDLOCK) WHERE p.IdProducto=@p AND p.Stock>=@cant+ISNULL((SELECT Cantidad FROM CarritoItem WITH(UPDLOCK,HOLDLOCK) WHERE DNI=@dni AND IdProducto=@p),0)) THROW 51000,'Producto sin stock suficiente',1; IF EXISTS(SELECT 1 FROM CarritoItem WHERE DNI=@dni AND IdProducto=@p) UPDATE CarritoItem SET Cantidad=Cantidad+@cant WHERE DNI=@dni AND IdProducto=@p ELSE INSERT CarritoItem(DNI,IdProducto,Cantidad) VALUES(@dni,@p,@cant)";
            using (SqlConnection c = new SqlConnection(_conexion)) { c.Open(); using (SqlTransaction tx = c.BeginTransaction(IsolationLevel.Serializable)) try { using (SqlCommand q = new SqlCommand(sql, c, tx)) { Param(q, "@dni", dni, SqlDbType.NVarChar, 8); Param(q, "@p", idProducto, SqlDbType.Int); Param(q, "@cant", cantidad, SqlDbType.Int); q.ExecuteNonQuery(); } ActualizarDVEnTransaccion(c, tx, "CarritoItem"); tx.Commit(); } catch { tx.Rollback(); throw; } }
}

        public void ActualizarCantidadCarrito(string dni, int idProducto, int cantidad)
        {
            const string sql = "IF NOT EXISTS(SELECT 1 FROM Producto p WITH(UPDLOCK,HOLDLOCK) WHERE p.IdProducto=@id AND p.Stock>=@cantidad) THROW 51000,'La cantidad supera el stock disponible',1; UPDATE CarritoItem SET Cantidad=@cantidad WHERE DNI=@dni AND IdProducto=@id; IF @@ROWCOUNT=0 THROW 51001,'El producto no está en el carrito',1";
            using (SqlConnection c = new SqlConnection(_conexion)) { c.Open(); using (SqlTransaction tx = c.BeginTransaction(IsolationLevel.Serializable)) try { using (SqlCommand q = new SqlCommand(sql, c, tx)) { Param(q, "@dni", dni, SqlDbType.NVarChar, 8); Param(q, "@id", idProducto, SqlDbType.Int); Param(q, "@cantidad", cantidad, SqlDbType.Int); q.ExecuteNonQuery(); } ActualizarDVEnTransaccion(c, tx, "CarritoItem"); tx.Commit(); } catch { tx.Rollback(); throw; } }
}

        public void EliminarDelCarrito(string dni, int idProducto)
        {
            using (SqlConnection c = new SqlConnection(_conexion)) { c.Open(); using (SqlTransaction tx = c.BeginTransaction()) try { using (SqlCommand q = new SqlCommand("DELETE FROM CarritoItem WHERE DNI=@dni AND IdProducto=@id", c, tx)) { Param(q, "@dni", dni, SqlDbType.NVarChar, 8); Param(q, "@id", idProducto, SqlDbType.Int); q.ExecuteNonQuery(); } ActualizarDVEnTransaccion(c, tx, "CarritoItem"); tx.Commit(); } catch { tx.Rollback(); throw; } }
}

        public int ConfirmarCompra(string dni, string titular, string ultimos4)
        {
            int orderId;
            using (SqlConnection c = new SqlConnection(_conexion)) { c.Open(); using (SqlTransaction tx = c.BeginTransaction(IsolationLevel.Serializable)) try
            {
                decimal total;
                using (SqlCommand q = new SqlCommand("IF EXISTS(SELECT 1 FROM CarritoItem ci JOIN Producto p WITH(UPDLOCK,HOLDLOCK) ON p.IdProducto=ci.IdProducto WHERE ci.DNI=@dni AND p.Stock<ci.Cantidad) THROW 51000,'Hay productos sin stock suficiente',1; SELECT SUM(p.Precio*ci.Cantidad) FROM CarritoItem ci JOIN Producto p ON p.IdProducto=ci.IdProducto WHERE ci.DNI=@dni", c, tx)) { Param(q, "@dni", dni, SqlDbType.NVarChar, 8); object v = q.ExecuteScalar(); if (v == DBNull.Value || v == null) throw new InvalidOperationException("El carrito está vacío."); total = Convert.ToDecimal(v); }
                using (SqlCommand q = new SqlCommand("INSERT Compra(DNI,Fecha,Total,Estado,TitularTarjeta,Ultimos4) VALUES(@dni,GETDATE(),@t,N'Completada',@tit,@ult); SELECT CAST(SCOPE_IDENTITY() AS int)", c, tx)) { Param(q, "@dni", dni, SqlDbType.NVarChar, 8); Param(q, "@t", total, SqlDbType.Decimal); q.Parameters["@t"].Precision = 10; q.Parameters["@t"].Scale = 2; Param(q, "@tit", Encriptacion.EncriptarAES(titular), SqlDbType.NVarChar, 256); Param(q, "@ult", Encriptacion.EncriptarAES(ultimos4), SqlDbType.NVarChar, 100); orderId = (int)q.ExecuteScalar(); }
                using (SqlCommand q = new SqlCommand("INSERT CompraItem(IdCompra,IdProducto,NombreProducto,PrecioUnitario,Cantidad) SELECT @order,p.IdProducto,p.Nombre,p.Precio,ci.Cantidad FROM CarritoItem ci JOIN Producto p ON p.IdProducto=ci.IdProducto WHERE ci.DNI=@dni; UPDATE p SET Stock=p.Stock-ci.Cantidad FROM Producto p JOIN CarritoItem ci ON ci.IdProducto=p.IdProducto WHERE ci.DNI=@dni; DELETE FROM CarritoItem WHERE DNI=@dni", c, tx)) { Param(q, "@order", orderId, SqlDbType.Int); Param(q, "@dni", dni, SqlDbType.NVarChar, 8); q.ExecuteNonQuery(); }

                ActualizarDVEnTransaccion(c, tx, "Compra"); ActualizarDVEnTransaccion(c, tx, "CompraItem"); ActualizarDVEnTransaccion(c, tx, "CarritoItem"); ActualizarDVEnTransaccion(c, tx, "Producto"); tx.Commit();
            } catch { tx.Rollback(); throw; } }
            return orderId;
        }
        private static void ActualizarDVEnTransaccion(SqlConnection c, SqlTransaction tx, string tabla) { new DALDigitoVerificador().ActualizarIntegridadTabla(tabla, c, tx); }
    }
}
