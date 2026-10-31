using FGF_Finanzas.Capas.BLL;
using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.Servicios;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FGF_Finanzas
{
    public partial class Marketplace : BasePage
    {
        private readonly BLLProducto bllProducto = new BLLProducto();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) Cargar();
            if (Session["AvisoMarketplace"] is string aviso)
            {
                lblEstadoOperacion.Text = Server.HtmlEncode(aviso);
                lblEstadoOperacion.CssClass = "market-message error";
                Session.Remove("AvisoMarketplace");
            }
            AplicarTextoDePagina();
        }

        private void AplicarTextoDePagina()
        {
            Title = TextoTraducido("PageTitle", "Tienda");
            litKicker.Text = TextoTraducido("litKicker", "VITALPET · TIENDA");
            litTitulo.Text = TextoTraducido("litTitulo", "Todo para su bienestar");
            litDescripcion.Text = TextoTraducido("litDescripcion", "Productos seleccionados para acompañar cada etapa de su mascota.");
            litCarrito.Text = TextoTraducido("litCarrito", "Mi carrito");
            litVacioTitulo.Text = TextoTraducido("litVacioTitulo", "Pronto habrá productos");
            litVacioDescripcion.Text = TextoTraducido("litVacioDescripcion", "Estamos preparando nuestra tienda.");
        }

        private void Cargar()
        {
            var productos = bllProducto.ObtenerTodos();
            rptProductos.DataSource = productos;
            rptProductos.DataBind();
            pnlVacio.Visible = productos.Count == 0;
            lblCantidadCarrito.Text = "0";
            if (SessionManager.IsLogged() && string.Equals(SessionManager.Instancia.Usuario?.Rol?.Nombre, "Cliente", StringComparison.OrdinalIgnoreCase))
            {
                try { lblCantidadCarrito.Text = bllProducto.ObtenerCarrito(SessionManager.Instancia.Usuario.DNI).Count.ToString(); }
                catch { }
            }
        }

        protected void rptProductos_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
            var btn = (LinkButton)e.Item.FindControl("btnAgregar");
            var cantidad = (TextBox)e.Item.FindControl("txtCantidad");
            var disponible = (Literal)e.Item.FindControl("litDisponibilidad");
            int stock = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "Stock"));
            if (btn != null) btn.Text = TextoTraducido("litAgregar", "Agregar");
            if (cantidad != null)
            {
                cantidad.Attributes["aria-label"] = TextoTraducido("litCantidadAria", "Cantidad");
                cantidad.Attributes["max"] = Math.Max(1, stock).ToString();
                cantidad.Enabled = stock > 0;
            }
            if (disponible != null)
            {
                string texto = stock == 0 ? TextoTraducido("litSinStock", "Sin stock") :
                    stock <= 5 ? string.Format(TextoTraducido("litUltimasUnidades", "Últimas {0} unidades"), stock) : TextoTraducido("litDisponible", "Disponible");
                disponible.Text = Server.HtmlEncode(texto);
            }
        }

        protected void rptProductos_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName != "Agregar") return;
            if (!SessionManager.IsLogged())
            {
                Session["AvisoLoginMarketplace"] = TextoTraducido("msgLoginAgregarCarrito", "Iniciá sesión con una cuenta de cliente para agregar productos al carrito.");
                Response.Redirect("~/Login.aspx?ReturnUrl=" + Server.UrlEncode("~/Marketplace.aspx"));
                return;
            }
            if (!string.Equals(SessionManager.Instancia.Usuario?.Rol?.Nombre, "Cliente", StringComparison.OrdinalIgnoreCase))
            {
                lblEstadoOperacion.Text = Server.HtmlEncode(TextoTraducido("msgSoloCliente", "El carrito está disponible solo para cuentas de cliente."));
                lblEstadoOperacion.CssClass = "market-message error";
                return;
            }

            try
            {
                int id, cantidad;
                if (!int.TryParse(Convert.ToString(e.CommandArgument), out id))
                    throw new InvalidOperationException(TextoTraducido("msgProductoInvalido", "Producto inválido."));
                TextBox cantidadBox = (TextBox)e.Item.FindControl("txtCantidad");
                if (!int.TryParse(cantidadBox.Text, out cantidad) || cantidad < 1 || cantidad > 99)
                    throw new InvalidOperationException(TextoTraducido("msgCantidadInvalida", "Elegí una cantidad entre 1 y 99."));
                int stockDisponible = bllProducto.ObtenerPorId(id)?.Stock ?? 0;
                int enCarrito = 0;
                foreach (BECarritoItem item in bllProducto.ObtenerCarrito(SessionManager.Instancia.Usuario.DNI))
                    if (item.IdProducto == id) { enCarrito = item.Cantidad; break; }
                if (cantidad > stockDisponible - enCarrito)
                    throw new BECustomException("ERR_PRODUCTO_SIN_STOCK");
                bllProducto.AgregarAlCarrito(SessionManager.Instancia.Usuario.DNI, id, cantidad);
                lblEstadoOperacion.Text = TextoTraducido("lblMensaje", "Producto agregado al carrito.");
                lblEstadoOperacion.CssClass = "market-message success";
            }
            catch (InvalidOperationException ex)
            {
                lblEstadoOperacion.Text = Server.HtmlEncode(ex.Message);
                lblEstadoOperacion.CssClass = "market-message error";
            }
            catch (BECustomException ex)
            {
                lblEstadoOperacion.Text = Server.HtmlEncode(TextoTraducido(ex.CodigoError == "ERR_PRODUCTO_SIN_STOCK" ? "msgStockInsuficiente" : ex.CodigoError, "No hay más unidades disponibles de este producto."));
                lblEstadoOperacion.CssClass = "market-message error";
            }
            catch
            {
                lblEstadoOperacion.Text = TextoTraducido("msgErrorAgregar", "No se pudo agregar el producto. Revisá la disponibilidad e intentá nuevamente.");
                lblEstadoOperacion.CssClass = "market-message error";
            }
            Cargar();
        }
    }
}
