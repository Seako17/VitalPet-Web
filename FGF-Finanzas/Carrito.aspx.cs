using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.BLL;
using FGF_Finanzas.Capas.Servicios;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace FGF_Finanzas
{
    public partial class Carrito : BasePage
    {
        private readonly BLLProducto bllProducto = new BLLProducto();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionManager.IsLogged())
            {
                Session["AvisoLoginMarketplace"] = TextoTraducido("msgRequiereCliente", "Iniciá sesión con una cuenta de cliente para usar el carrito.");
                Response.Redirect("~/Login.aspx?ReturnUrl=" + Server.UrlEncode("~/Carrito.aspx"));
                return;
            }
            if (!string.Equals(SessionManager.Instancia.Usuario?.Rol?.Nombre, "Cliente", StringComparison.OrdinalIgnoreCase))
            {
                Session["AvisoMarketplace"] = TextoTraducido("msgSoloCliente", "El carrito está disponible solo para cuentas de cliente.");
                Response.Redirect("~/Marketplace.aspx");
                return;
            }
            if (!IsPostBack) Cargar();
            AplicarTextoDePagina();
        }

        private void AplicarTextoDePagina()
        {
            Title = TextoTraducido("PageTitle", "Mi carrito");
            litSeguirComprando.Text = TextoTraducido("litSeguirComprando", "← Seguir comprando");
            litKicker.Text = TextoTraducido("litKicker", "SU PEDIDO");
            litTitulo.Text = TextoTraducido("litTitulo", "Mi carrito");
            LitTotal.Text = TextoTraducido("LitTotal", "Total");
            litPagoTitulo.Text = TextoTraducido("litPagoTitulo", "Pago de demostración");
            litPagoNotaTitulo.Text = TextoTraducido("litPagoNotaTitulo", "No se realizará ningún cargo");
            litNombreTitular.Text = TextoTraducido("litNombreTitular", "Nombre del titular");
            litNumeroTarjeta.Text = TextoTraducido("litNumeroTarjeta", "Número de tarjeta (16 dígitos, sin espacios)");
            litVencimiento.Text = TextoTraducido("litVencimiento", "Vencimiento (MM/AA)");
            litCvv.Text = TextoTraducido("litCvv", "Código de seguridad");
            litNotaPago.Text = TextoTraducido("litNotaPago", "Los datos de la tarjeta solo se usan para validar esta compra de prueba y no se guardan.");
            btnComprar.Text = TextoTraducido("btnComprar", "Confirmar compra");
            txtTarjeta.Attributes["oninput"] = "this.value=this.value.replace(/[^0-9]/g,'').slice(0,16);";
            txtTarjeta.Attributes["autocomplete"] = "off";
            txtVencimiento.Attributes["placeholder"] = TextoTraducido("txtVencimiento", "MM/AA");
            TraducirColumna(gvCarrito, 0, "gvCarrito_Header_Producto", "Producto");
            TraducirColumna(gvCarrito, 1, "gvCarrito_Header_Cantidad", "Cantidad");
            TraducirColumna(gvCarrito, 2, "gvCarrito_Header_Subtotal", "Subtotal");
            TraducirColumna(gvCarrito, 3, "gvCarrito_Header_Acciones", "Acciones");
            TraducirSinDatos(gvCarrito, "gvCarrito_Vacio", "Tu carrito está vacío.");
            TraducirBotonFila(gvCarrito, "btnEliminar", "btnEliminar", "Eliminar");
            valTarjeta.ErrorMessage = TextoTraducido("valTarjeta", "Ingresá exactamente 16 dígitos, sin espacios.");
        }

        private void Cargar()
        {
            var rows = bllProducto.ObtenerCarrito(SessionManager.Instancia.Usuario.DNI);
            foreach (BECarritoItem row in rows)
            {
                int maximo = Math.Min(row.Stock, 99);
                if (maximo > 0 && row.Cantidad > maximo)
                {
                    bllProducto.ActualizarCantidadCarrito(SessionManager.Instancia.Usuario.DNI, row.IdProducto, maximo);
                    row.Cantidad = maximo;
                }
            }
            gvCarrito.DataSource = rows;
            gvCarrito.DataBind();
            decimal total = 0;
            foreach (BECarritoItem row in rows) total += row.Subtotal;
            lblTotal.Text = total.ToString("N2", CultureInfo.GetCultureInfo("es-AR"));
            pnlPago.Visible = rows.Exists(row => !row.SinStockSuficiente);
            TraducirColumna(gvCarrito, 0, "gvCarrito_Header_Producto", "Producto");
            TraducirColumna(gvCarrito, 1, "gvCarrito_Header_Cantidad", "Cantidad");
            TraducirColumna(gvCarrito, 2, "gvCarrito_Header_Subtotal", "Subtotal");
            TraducirColumna(gvCarrito, 3, "gvCarrito_Header_Acciones", "Acciones");
            TraducirSinDatos(gvCarrito, "gvCarrito_Vacio", "Tu carrito está vacío.");
            TraducirBotonFila(gvCarrito, "btnEliminar", "btnEliminar", "Eliminar");
        }

        protected void gvCarrito_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;
            Literal precioUnitario = e.Row.FindControl("litPrecioUnitario") as Literal;
            TextBox cantidad = e.Row.FindControl("txtCantidad") as TextBox;
            if (precioUnitario != null) precioUnitario.Text = TextoTraducido("litPrecioUnitario", "por unidad");
            BECarritoItem item = e.Row.DataItem as BECarritoItem;
            if (cantidad != null)
            {
                cantidad.Attributes["aria-label"] = TextoTraducido("litCantidadAria", "Cantidad");
                cantidad.Enabled = item != null && item.Stock > 0;
                cantidad.Attributes["max"] = item == null ? "99" : Math.Min(item.Stock, 99).ToString(CultureInfo.InvariantCulture);
                cantidad.Visible = item == null || !item.SinStockSuficiente;
            }
            Literal estadoStock = e.Row.FindControl("litEstadoStockCantidad") as Literal;
            if (item != null && estadoStock != null && item.SinStockSuficiente)
            {
                estadoStock.Text = "<strong class=\"cart-stock-unavailable\">" + Server.HtmlEncode(TextoTraducido("litSinStock", "Sin stock")) + "</strong>";
            }
        }

        protected void gvCarrito_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Eliminar") return;
            int id;
            if (!int.TryParse(Convert.ToString(e.CommandArgument), out id)) return;
            try
            {
                bllProducto.EliminarDelCarrito(SessionManager.Instancia.Usuario.DNI, id);
                lblMensaje.Text = TextoTraducido("msgProductoEliminadoCarrito", "Producto eliminado del carrito.");
                lblMensaje.CssClass = "market-message success";
            }
            catch (BECustomException ex) when (ex.CodigoError == "ERR_PRODUCTO_SIN_STOCK") { MostrarError("msgStockInsuficiente", "La cantidad supera el stock disponible."); }
            catch (BECustomException ex) { MostrarError(ex.CodigoError, "La operación no se pudo completar por una validación de seguridad."); }
            catch
            {
                lblMensaje.Text = TextoTraducido("msgErrorCarrito", "No se pudo actualizar el carrito.");
                lblMensaje.CssClass = "market-message error";
            }
            Cargar();
        }

        protected void txtCantidad_TextChanged(object sender, EventArgs e)
        {
            TextBox txtCantidad = sender as TextBox;
            GridViewRow row = txtCantidad == null ? null : txtCantidad.NamingContainer as GridViewRow;
            int id, cantidad;
            if (row == null || !int.TryParse(Convert.ToString(gvCarrito.DataKeys[row.RowIndex].Value), out id)) return;

            try
            {
                BEProducto producto = bllProducto.ObtenerPorId(id);
                if (producto == null || producto.Stock <= 0)
                {
                    MostrarError("msgProductoSinStockCarrito", "El producto no tiene stock. Podés eliminarlo del carrito.");
                }
                else
                {
                    int maximo = Math.Min(producto.Stock, 99);
                    cantidad = int.TryParse(txtCantidad.Text, out cantidad) ? cantidad : 1;
                    cantidad = Math.Max(1, Math.Min(cantidad, maximo));
                    bllProducto.ActualizarCantidadCarrito(SessionManager.Instancia.Usuario.DNI, id, cantidad);
                }
            }
            catch (BECustomException ex) when (ex.CodigoError == "ERR_PRODUCTO_SIN_STOCK")
            {
                MostrarError("msgProductoSinStockCarrito", "El producto no tiene stock suficiente.");
            }
            catch
            {
                MostrarError("msgErrorCarrito", "No se pudo actualizar el carrito.");
            }
            Cargar();
        }

        protected void btnComprar_Click(object sender, EventArgs e)
        {
            string titular = txtTitular.Text.Trim(), tarjeta = txtTarjeta.Text, exp = txtVencimiento.Text.Trim(), cvv = txtCvv.Text.Trim();
            try
            {
                bllProducto.ConfirmarCompra(SessionManager.Instancia.Usuario.DNI, titular, tarjeta, exp, cvv);
                txtTarjeta.Text = ""; txtCvv.Text = ""; txtVencimiento.Text = ""; txtTitular.Text = "";
                lblMensaje.Text = TextoTraducido("msgCompraExitosa", "Pedido realizado correctamente");
                lblMensaje.CssClass = "market-message success";
            }
            catch (BECustomException ex)
            {
                string clave = ex.CodigoError == "ERR_PRODUCTO_TARJETA_FORMATO" ? "msgErrorTarjeta" :
                    ex.CodigoError == "ERR_PRODUCTO_TARJETA_VENCIDA" ? "msgTarjetaVencida" :
                    ex.CodigoError == "ERR_PRODUCTO_FECHA_FORMATO" ? "msgErrorFormatoFecha" :
                    ex.CodigoError == "ERR_PRODUCTO_CVV" ? "msgErrorCvv" :
                    ex.CodigoError == "ERR_PRODUCTO_TITULAR" ? "msgErrorTitular" :
                    ex.CodigoError == "ERR_PRODUCTO_SIN_STOCK" ? "msgCheckoutSinStock" : ex.CodigoError;
                MostrarError(clave, "Revisá los datos de pago e intentá nuevamente.");
            }
            catch
            {
                MostrarError("msgErrorCompra", "No se pudo completar la compra. Revisá el stock disponible e intentá nuevamente.");
            }
            Cargar();
        }

        private void MostrarError(string clave, string porDefecto)
        {
            lblMensaje.Text = Server.HtmlEncode(TextoTraducido(clave, porDefecto));
            lblMensaje.CssClass = "market-message error";
        }

    }
}
