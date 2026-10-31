using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.BLL;
using FGF_Finanzas.Capas.Servicios;
using System;
using System.Globalization;
using System.IO;

using System.Web.UI.WebControls;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using DrawingImage = System.Drawing.Image;

namespace FGF_Finanzas
{
    public partial class Gestion_Productos : PaginaSegura
    {
        private readonly BLLProducto bllProducto = new BLLProducto();
        protected override Permiso PermisoRequerido { get { return CodigosPermiso.ObtenerPermisos().Find(x => x.Nombre == "ProductoGestionar"); } }
        protected override bool TieneAcceso(BEUsuario usuario)
        {
            return string.Equals(usuario?.Rol?.Nombre, "Admin", StringComparison.OrdinalIgnoreCase) && base.TieneAcceso(usuario);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Form.Enctype = "multipart/form-data";
            fuImagen.Attributes["accept"] = "image/png,image/jpeg,image/gif,.png,.jpg,.jpeg,.gif";
            if (!IsPostBack) Cargar();
            AplicarTextoDePagina();
        }

        private void AplicarTextoDePagina()
        {
            Title = TextoTraducido("PageTitle", "Gestión de productos");
            litPanelTitulo.Text = TextoTraducido("litPanelTitulo", "Panel de Administrador");
            litAdministracion.Text = TextoTraducido("litAdministracion", "Administración");
            litMenuUsuarios.Text = TextoTraducido("litMenuUsuarios", "Gestión Usuarios");
            litMenuBitacora.Text = TextoTraducido("litMenuBitacora", "Bitácora eventos");
            litMenuPerfiles.Text = TextoTraducido("litMenuPerfiles", "Gestión Perfiles");
            litMenuFamilias.Text = TextoTraducido("litMenuFamilias", "Gestión Familias");
            litMenuBackup.Text = TextoTraducido("litMenuBackup", "Backup/Restore");
            litMenuIdiomas.Text = TextoTraducido("litMenuIdiomas", "Gestión Idiomas");
            litGestionMenu.Text = TextoTraducido("litGestionMenu", "Gestión de productos");
            litKicker.Text = TextoTraducido("litKicker", "VITALPET · TIENDA");
            litTitulo.Text = TextoTraducido("litTitulo", "Gestión de productos");
            litNombre.Text = TextoTraducido("litNombre", "Nombre");
            litPrecio.Text = TextoTraducido("litPrecio", "Precio");
            litDescripcion.Text = TextoTraducido("litDescripcion", "Descripción");
            litStock.Text = TextoTraducido("litStock", "Stock");
            litImagen.Text = TextoTraducido("litImagen", "Imagen (PNG, JPG o GIF; máximo 2 MB)");
            btnGuardar.Text = TextoTraducido("btnGuardar", "Guardar producto");
            btnLimpiar.Text = TextoTraducido("btnLimpiar", "Limpiar");
            TraducirGrilla();
            ActualizarModoFormulario();
        }

        private void ActualizarModoFormulario()
        {
            bool editando = !string.IsNullOrEmpty(hfIdProducto.Value);
            lblFormulario.Text = TextoTraducido(editando ? "lblFormularioEditar" : "lblFormularioNuevo", editando ? "Editar producto" : "Nuevo producto");
            pnlModoEdicion.Visible = editando;
            if (editando) litModoEdicion.Text = Server.HtmlEncode(string.Format(TextoTraducido("litModoEdicion", "Estás modificando el producto: {0}"), txtNombre.Text));
        }

        private void TraducirGrilla()
        {
            TraducirColumna(gvProductos, 0, "gvProductos_Header_Producto", "Producto");
            TraducirColumna(gvProductos, 1, "gvProductos_Header_Precio", "Precio");
            TraducirColumna(gvProductos, 2, "gvProductos_Header_Stock", "Stock");
            TraducirColumna(gvProductos, 3, "gvProductos_Header_Acciones", "Acciones");
            TraducirSinDatos(gvProductos, "gvProductos_Vacio", "Aún no hay productos cargados.");
            TraducirBotonFila(gvProductos, "btnEditar", "btnEditar", "Editar");
            TraducirBotonFila(gvProductos, "btnEliminar", "btnEliminar", "Eliminar");
        }

        private void Cargar() { gvProductos.DataSource = bllProducto.ObtenerTodos(); gvProductos.DataBind(); TraducirGrilla(); }

        protected void gvProductos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;
            var boton = e.Row.FindControl("btnEliminar") as LinkButton;
            if (boton != null) boton.OnClientClick = "return confirm('" + HttpUtility.JavaScriptStringEncode(TextoTraducido("msgConfirmarEliminar", "¿Eliminar este producto de la tienda?")) + "');";
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            ActualizarModoFormulario();
            try
            {
                string nombre = txtNombre.Text.Trim(), descripcion = txtDescripcion.Text.Trim();
                decimal precio; int stock;
                if (nombre.Length < 2 || nombre.Length > 120 || descripcion.Length > 1000 || string.IsNullOrWhiteSpace(descripcion)) throw new InvalidOperationException(TextoTraducido("msgValidarNombreDescripcion", "Ingresá nombre y descripción (hasta 1000 caracteres)."));
                if (!decimal.TryParse(txtPrecio.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out precio) || precio <= 0) throw new InvalidOperationException(TextoTraducido("msgValidarPrecio", "El precio debe ser mayor que cero."));
                if (!int.TryParse(txtStock.Text, out stock) || stock < 0 || stock > 1000000) throw new InvalidOperationException(TextoTraducido("msgValidarStock", "El stock debe ser un entero válido."));
                int? id = string.IsNullOrEmpty(hfIdProducto.Value) ? (int?)null : int.Parse(hfIdProducto.Value);
                string imagen = null; if (fuImagen.HasFile) imagen = LeerImagen();
                bllProducto.Guardar(new BEProducto { IdProducto = id ?? 0, Nombre = nombre, Descripcion = descripcion, Precio = precio, Stock = stock, Imagen = imagen });
                Limpiar(); lblMensaje.Text = TextoTraducido("msgProductoGuardado", "Producto guardado."); lblMensaje.CssClass = "market-message success"; Cargar();
            }
            catch (InvalidOperationException ex) { lblMensaje.Text = Server.HtmlEncode(ex.Message); lblMensaje.CssClass = "market-message error"; }
            catch (BECustomException ex) { lblMensaje.Text = Server.HtmlEncode(TextoTraducido(ex.CodigoError, "No se pudo guardar el producto porque la validación del sistema falló.")); lblMensaje.CssClass = "market-message error"; }
            catch { lblMensaje.Text = TextoTraducido("msgErrorGuardar", "No se pudo guardar el producto. Revisá los datos e intentá nuevamente."); lblMensaje.CssClass = "market-message error"; }
        }

        private string LeerImagen()
        {
            const int tamanoMaximoImagen = 8 * 1024 * 1024;
            if (fuImagen.PostedFile.ContentLength <= 0 || fuImagen.PostedFile.ContentLength > tamanoMaximoImagen) throw new InvalidOperationException(TextoTraducido("msgValidarTamanoImagen", "La imagen debe pesar hasta 8 MB."));
            byte[] bytes; using (BinaryReader r = new BinaryReader(fuImagen.PostedFile.InputStream)) bytes = r.ReadBytes(fuImagen.PostedFile.ContentLength);
            string mime;
            try
            {
                using (MemoryStream m = new MemoryStream(bytes)) using (DrawingImage image = DrawingImage.FromStream(m, false, true))
                {
                    if (image.RawFormat.Guid == ImageFormat.Jpeg.Guid) mime = "image/jpeg";
                    else if (image.RawFormat.Guid == ImageFormat.Png.Guid) mime = "image/png";
                    else if (image.RawFormat.Guid == ImageFormat.Gif.Guid) mime = "image/gif";
                    else throw new InvalidOperationException(TextoTraducido("msgValidarFormatoImagen", "El archivo debe ser una imagen PNG, JPG o GIF válida."));
                }
            }
            catch (InvalidOperationException) { throw; }
            catch (ArgumentException) { throw new InvalidOperationException(TextoTraducido("msgValidarFormatoImagen", "El archivo debe ser una imagen PNG, JPG o GIF válida.")); }
            return "data:" + mime + ";base64," + Convert.ToBase64String(bytes);
        }

        protected void gvProductos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id; if (!int.TryParse(Convert.ToString(e.CommandArgument), out id)) return;
            try
            {
                if (e.CommandName == "Eliminar")
                {
                    bllProducto.Eliminar(id); lblMensaje.Text = TextoTraducido("msgProductoEliminado", "Producto eliminado de la tienda."); lblMensaje.CssClass = "market-message success";
                    if (hfIdProducto.Value == id.ToString()) Limpiar();
                }
                else if (e.CommandName == "Editar")
                {
                    BEProducto producto = bllProducto.ObtenerPorId(id);
                    if (producto != null)
                    {
                        hfIdProducto.Value = id.ToString(); txtNombre.Text = producto.Nombre; txtDescripcion.Text = producto.Descripcion; txtPrecio.Text = producto.Precio.ToString("0.00", CultureInfo.InvariantCulture); txtStock.Text = producto.Stock.ToString(CultureInfo.InvariantCulture); lblFormulario.Text = TextoTraducido("lblFormularioEditar", "Editar producto"); pnlModoEdicion.Visible = true; litModoEdicion.Text = Server.HtmlEncode(string.Format(TextoTraducido("litModoEdicion", "Estás modificando el producto: {0}"), txtNombre.Text));
                    }                }
                Cargar();
            }
            catch (BECustomException ex) { lblMensaje.Text = Server.HtmlEncode(TextoTraducido(ex.CodigoError, "No se pudo actualizar el producto por una validación de seguridad.")); lblMensaje.CssClass = "market-message error"; }
            catch { lblMensaje.Text = TextoTraducido("msgErrorActualizar", "No se pudo actualizar el producto."); lblMensaje.CssClass = "market-message error"; }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e) { Limpiar(); }
        private void Limpiar() { hfIdProducto.Value = ""; txtNombre.Text = ""; txtDescripcion.Text = ""; txtPrecio.Text = ""; txtStock.Text = ""; lblFormulario.Text = TextoTraducido("lblFormularioNuevo", "Nuevo producto"); pnlModoEdicion.Visible = false; }
    }
}
