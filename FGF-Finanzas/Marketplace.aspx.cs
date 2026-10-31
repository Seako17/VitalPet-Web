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
            if (btn != null) btn.Text = TextoTraducido("litAgregar", "Agregar");
            if (cantidad != null) cantidad.Attributes["aria-label"] = TextoTraducido("litCantidadAria", "Cantidad");
            if (disponible != null)
            {
                bool tieneStock = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "Stock")) > 0;
                disponible.Text = Server.HtmlEncode(TextoTraducido(tieneStock ? "litDisponible" : "litSinStock", tieneStock ? "Disponible" : "Sin stock"));
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
                lblEstadoOperacion.Text = Server.HtmlEncode(TextoTraducido(ex.CodigoError, "No se pudo agregar el producto al carrito."));
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
