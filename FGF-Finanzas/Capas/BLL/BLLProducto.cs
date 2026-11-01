using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.DAL;
using FGF_Finanzas.Capas.Servicios;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace FGF_Finanzas.Capas.BLL
{
    public class BLLProducto
    {
        private readonly DALProducto dalProducto = new DALProducto();
        private readonly BLLDigitoVerificador bllDV = new BLLDigitoVerificador();

        public List<BEProducto> ObtenerTodos() { return dalProducto.ObtenerTodos(); }
        public BEProducto ObtenerPorId(int id) { if (id <= 0) throw new ArgumentOutOfRangeException("id"); return dalProducto.ObtenerPorId(id); }
        public List<BECarritoItem> ObtenerCarrito(string dni) { ValidarDni(dni); return dalProducto.ObtenerCarrito(dni); }

        public void Guardar(BEProducto producto)
        {
            ValidarPermisoGestion();
            if (producto == null) throw new ArgumentNullException("producto");
            if (!bllDV.ValidarIntegridadDelSistema()) throw new BECustomException("ERR_SISTEMA_MANTENIMIENTO_PRODUCTO");
            producto.Nombre = (producto.Nombre ?? string.Empty).Trim();
            producto.Descripcion = (producto.Descripcion ?? string.Empty).Trim();
            if (producto.Nombre.Length < 2 || producto.Nombre.Length > 120 || string.IsNullOrWhiteSpace(producto.Descripcion) || producto.Descripcion.Length > 1000)
                throw new BECustomException("ERR_PRODUCTO_NOMBRE_DESCRIPCION");
            if (producto.Precio <= 0 || producto.Precio > 99999999.99m) throw new BECustomException("ERR_PRODUCTO_PRECIO");
            if (producto.Stock < 0 || producto.Stock > 1000000) throw new BECustomException("ERR_PRODUCTO_STOCK");
            if (producto.IdProducto < 0) throw new BECustomException("ERR_PRODUCTO_INVALIDO");
            dalProducto.Guardar(producto);
        }

        public void Eliminar(int id)
        {
            ValidarPermisoGestion();
            if (id <= 0) throw new BECustomException("ERR_PRODUCTO_INVALIDO");
            if (!bllDV.ValidarIntegridadDelSistema()) throw new BECustomException("ERR_SISTEMA_MANTENIMIENTO_PRODUCTO");
            dalProducto.Eliminar(id);
        }

        public void AgregarAlCarrito(string dni, int idProducto, int cantidad)
        {
            ValidarDni(dni); ValidarClienteActual(dni); ValidarProductoCantidad(idProducto, cantidad);
            try { dalProducto.AgregarAlCarrito(dni, idProducto, cantidad); }
            catch (SqlException ex) when (ex.Number == 51000) { throw new BECustomException("ERR_PRODUCTO_SIN_STOCK", ex); }
        }

        public void ActualizarCantidadCarrito(string dni, int idProducto, int cantidad)
        {
            ValidarDni(dni); ValidarClienteActual(dni); ValidarProductoCantidad(idProducto, cantidad);
            try { dalProducto.ActualizarCantidadCarrito(dni, idProducto, cantidad); }
            catch (SqlException ex) when (ex.Number == 51000) { throw new BECustomException("ERR_PRODUCTO_SIN_STOCK", ex); }
        }

        public void EliminarDelCarrito(string dni, int idProducto)
        {
            ValidarDni(dni); ValidarClienteActual(dni);
            if (idProducto <= 0) throw new BECustomException("ERR_PRODUCTO_INVALIDO");
            dalProducto.EliminarDelCarrito(dni, idProducto);
        }

        public int ConfirmarCompra(string dni, string titular, string tarjetaCompleta, string vencimiento, string cvv)
        {
            ValidarDni(dni); ValidarClienteActual(dni);
            titular = (titular ?? string.Empty).Trim();
            if (!System.Text.RegularExpressions.Regex.IsMatch(titular, @"^[\p{L}][\p{L} .'-]{2,79}$")) throw new BECustomException("ERR_PRODUCTO_TITULAR");
            if (!System.Text.RegularExpressions.Regex.IsMatch(tarjetaCompleta ?? string.Empty, @"^[0-9]{16}$")) throw new BECustomException("ERR_PRODUCTO_TARJETA_FORMATO");
            if (!System.Text.RegularExpressions.Regex.IsMatch(cvv ?? string.Empty, @"^[0-9]{3,4}$")) throw new BECustomException("ERR_PRODUCTO_CVV");
            if (!System.Text.RegularExpressions.Regex.IsMatch(vencimiento ?? string.Empty, @"^(0[1-9]|1[0-2])/[0-9]{2}$")) throw new BECustomException("ERR_PRODUCTO_FECHA_FORMATO");
            int mes = int.Parse(vencimiento.Substring(0, 2)), anio = 2000 + int.Parse(vencimiento.Substring(3, 2));
            DateTime vence = new DateTime(anio, mes, 1).AddMonths(1);
            if (vence <= DateTime.Today) throw new BECustomException("ERR_PRODUCTO_TARJETA_VENCIDA");
            if (!bllDV.ValidarIntegridadDelSistema()) throw new BECustomException("ERR_SISTEMA_MANTENIMIENTO_PRODUCTO");
            try { return dalProducto.ConfirmarCompra(dni, titular, tarjetaCompleta.Substring(12, 4)); }
            catch (SqlException ex) when (ex.Number == 51000) { throw new BECustomException("ERR_PRODUCTO_SIN_STOCK", ex); }
        }

        private static void ValidarProductoCantidad(int idProducto, int cantidad)
        {
            if (idProducto <= 0) throw new BECustomException("ERR_PRODUCTO_INVALIDO");
            if (cantidad < 1 || cantidad > 99) throw new BECustomException("ERR_PRODUCTO_CANTIDAD");
        }

        private static void ValidarDni(string dni) { if (string.IsNullOrWhiteSpace(dni)) throw new BECustomException("ERR_PRODUCTO_CLIENTE"); }

        private static void ValidarClienteActual(string dni)
        {
            if (!SessionManager.IsLogged() || SessionManager.Instancia.Usuario == null ||
                !string.Equals(SessionManager.Instancia.Usuario.Rol?.Nombre, "Cliente", StringComparison.OrdinalIgnoreCase) ||
                !string.Equals(SessionManager.Instancia.Usuario.DNI, dni, StringComparison.Ordinal))
                throw new BECustomException("ERR_PRODUCTO_CLIENTE");
        }

        private static void ValidarPermisoGestion()
        {
            if (!SessionManager.IsLogged() || SessionManager.Instancia.Usuario == null ||
                SessionManager.Instancia.Usuario.Rol == null ||
                !string.Equals(SessionManager.Instancia.Usuario.Rol.Nombre, "Admin", StringComparison.OrdinalIgnoreCase))
                throw new BECustomException("ERR_PERMISO_DENEGADO");
            var permiso = CodigosPermiso.ObtenerPermisos().Find(p => p.Nombre == "ProductoGestionar");
            if (permiso == null || !SessionManager.Instancia.Usuario.Rol.TienePermiso(permiso))
                throw new BECustomException("ERR_PERMISO_DENEGADO");
        }

    }
}
