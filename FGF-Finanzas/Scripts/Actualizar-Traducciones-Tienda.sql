-- Sincroniza las traducciones de Tienda, Gestión de productos y Carrito en una base existente.
-- Ejecutar con Actualizar-Traducciones-Tienda.ps1 para recalcular los dígitos verificadores.
USE [FGF-BDD];
SET NOCOUNT ON;
SET XACT_ABORT ON;
BEGIN TRANSACTION;

DECLARE @Etiquetas TABLE(Formulario nvarchar(100), ControlId nvarchar(100), TextoEs nvarchar(1000), TextoEn nvarchar(1000));
INSERT INTO @Etiquetas(Formulario,ControlId,TextoEs,TextoEn) VALUES
(N'Site.Master',N'lnkMarketplace',N'Tienda',N'Store'),
(N'Gestion-Productos.aspx',N'PageTitle',N'Gestión de productos',N'Product management'),
(N'Carrito.aspx',N'msgRequiereCliente',N'Iniciá sesión con una cuenta de cliente para usar el carrito.',N'Sign in with a customer account to use the cart.'),
(N'Carrito.aspx',N'msgSoloCliente',N'El carrito está disponible solo para cuentas de cliente.',N'The cart is only available to customer accounts.'),
(N'Marketplace.aspx',N'msgLoginAgregarCarrito',N'Iniciá sesión con una cuenta de cliente para agregar productos al carrito.',N'Sign in with a customer account to add products to your cart.'),
(N'Marketplace.aspx',N'msgSoloCliente',N'El carrito está disponible solo para cuentas de cliente.',N'The cart is only available to customer accounts.'),
(N'Marketplace.aspx',N'lblMensaje',N'Producto agregado al carrito.',N'Product added to cart.'),
(N'Marketplace.aspx',N'PageTitle',N'Tienda',N'Store'),
(N'Carrito.aspx',N'PageTitle',N'Mi carrito',N'My cart'),
(N'Gestion-Productos.aspx',N'PageTitle',N'Gestión de productos',N'Product management'),
(N'Gestion-Productos.aspx',N'litSeleccionarArchivo',N'Seleccionar archivo',N'Choose file'),
(N'Gestion-Productos.aspx',N'lblArchivoNinguno',N'Ningún archivo seleccionado',N'No file selected'),
(N'Marketplace.aspx',N'litKicker',N'VITALPET · TIENDA',N'VITALPET · STORE'),
(N'Marketplace.aspx',N'litTitulo',N'Todo para su bienestar',N'Everything for their wellbeing'),
(N'Marketplace.aspx',N'litDescripcion',N'Productos seleccionados para acompañar cada etapa de su mascota.',N'Selected products for every stage of your pet’s life.'),
(N'Marketplace.aspx',N'litCarrito',N'Mi carrito',N'My cart'),
(N'Marketplace.aspx',N'litCantidadAria',N'Cantidad',N'Quantity'),
(N'Marketplace.aspx',N'litAgregar',N'Agregar',N'Add'),
(N'Marketplace.aspx',N'litDisponible',N'Disponible',N'In stock'),
(N'Marketplace.aspx',N'litSinStock',N'Sin stock',N'Out of stock'),
(N'Marketplace.aspx',N'litUltimasUnidades',N'Últimas {0} unidades',N'Only {0} left'),
(N'Marketplace.aspx',N'litQuedanUnidades',N'Unidades disponibles: {0}',N'Units available: {0}'),
(N'Marketplace.aspx',N'msgStockInsuficiente',N'No se puede agregar: no hay más unidades disponibles de este producto.',N'Cannot add this item: no more units are available.'),
(N'Marketplace.aspx',N'litVacioTitulo',N'Pronto habrá productos',N'Products coming soon'),
(N'Marketplace.aspx',N'litVacioDescripcion',N'Estamos preparando nuestra tienda.',N'We are getting our store ready.'),
(N'Marketplace.aspx',N'msgProductoInvalido',N'Producto inválido.',N'Invalid product.'),
(N'Marketplace.aspx',N'msgCantidadInvalida',N'Elegí una cantidad entre 1 y 99.',N'Choose a quantity between 1 and 99.'),
(N'Marketplace.aspx',N'msgErrorAgregar',N'No se pudo agregar el producto. Revisá la disponibilidad e intentá nuevamente.',N'The product could not be added. Check availability and try again.'),
(N'Carrito.aspx',N'litSeguirComprando',N'← Seguir comprando',N'← Continue shopping'),
(N'Carrito.aspx',N'litKicker',N'SU PEDIDO',N'YOUR ORDER'),
(N'Carrito.aspx',N'litTitulo',N'Mi carrito',N'My cart'),
(N'Carrito.aspx',N'LitTotal',N'Total',N'Total'),
(N'Carrito.aspx',N'litPagoTitulo',N'Pago de demostración',N'Demo payment'),
(N'Carrito.aspx',N'litNombreTitular',N'Nombre del titular',N'Cardholder name'),
(N'Carrito.aspx',N'litNumeroTarjeta',N'Número de tarjeta (16 dígitos, sin espacios)',N'Card number (16 digits, no spaces)'),
(N'Carrito.aspx',N'litVencimiento',N'Vencimiento (MM/AA)',N'Expiration (MM/YY)'),
(N'Carrito.aspx',N'litCvv',N'Código de seguridad',N'Security code'),
(N'Carrito.aspx',N'btnComprar',N'Confirmar compra',N'Place order'),
(N'Carrito.aspx',N'txtVencimiento',N'MM/AA',N'MM/YY'),
(N'Carrito.aspx',N'gvCarrito_Header_Producto',N'Producto',N'Product'),
(N'Carrito.aspx',N'gvCarrito_Header_Subtotal',N'Subtotal',N'Subtotal'),
(N'Carrito.aspx',N'gvCarrito_Header_Cantidad',N'Cantidad',N'Quantity'),
(N'Carrito.aspx',N'gvCarrito_Header_Acciones',N'Acciones',N'Actions'),
(N'Carrito.aspx',N'gvCarrito_Vacio',N'Tu carrito está vacío.',N'Your cart is empty.'),
(N'Carrito.aspx',N'btnEliminar',N'Eliminar',N'Delete'),
(N'Carrito.aspx',N'valTarjeta',N'Ingresá exactamente 16 dígitos, sin espacios.',N'Enter exactly 16 digits, no spaces.'),
(N'Carrito.aspx',N'msgProductoEliminadoCarrito',N'Producto eliminado del carrito.',N'Product removed from cart.'),
(N'Carrito.aspx',N'msgErrorCarrito',N'No se pudo actualizar el carrito.',N'The cart could not be updated.'),
(N'Carrito.aspx',N'btnActualizarCantidad',N'Actualizar',N'Update'),
(N'Carrito.aspx',N'litPrecioUnitario',N'por unidad',N'per item'),
(N'Carrito.aspx',N'litCantidadAria',N'Cantidad',N'Quantity'),
(N'Carrito.aspx',N'litSinStock',N'Sin stock',N'Out of stock'),
(N'Carrito.aspx',N'msgCantidadActualizada',N'Se actualizó la cantidad del producto.',N'Product quantity updated.'),
(N'Carrito.aspx',N'msgCantidadInvalidaCarrito',N'Ingresá una cantidad entre 1 y 99.',N'Enter a quantity between 1 and 99.'),
(N'Carrito.aspx',N'msgStockInsuficiente',N'La cantidad supera el stock disponible.',N'Quantity exceeds available stock.'),
(N'Carrito.aspx',N'msgProductoSinStockCarrito',N'Sin stock',N'Out of stock'),
(N'Carrito.aspx',N'msgCheckoutSinStock',N'No hay productos disponibles para comprar. Eliminá los productos sin stock del carrito.',N'No products are available to purchase. Remove the out-of-stock items from your cart.'),
(N'Carrito.aspx',N'msgErrorTitular',N'Revisá el nombre del titular.',N'Check the cardholder name.'),
(N'Carrito.aspx',N'msgErrorTarjeta',N'Ingresá los 16 dígitos de la tarjeta, sin espacios.',N'Enter the 16 card digits without spaces.'),
(N'Carrito.aspx',N'msgErrorCvv',N'El código de seguridad debe tener 3 o 4 dígitos.',N'The security code must have 3 or 4 digits.'),
(N'Carrito.aspx',N'msgErrorFormatoFecha',N'Usá el formato MM/AA para el vencimiento.',N'Use MM/YY for the expiration date.'),
(N'Carrito.aspx',N'msgTarjetaVencida',N'La tarjeta está vencida.',N'The card has expired.'),
(N'Carrito.aspx',N'msgCompraExitosa',N'Pedido realizado correctamente',N'Order placed successfully'),
(N'Carrito.aspx',N'msgErrorCompra',N'No se pudo completar la compra. Revisá el stock disponible e intentá nuevamente.',N'The purchase could not be completed. Check stock and try again.'),
(N'Gestion-Productos.aspx',N'litPanelTitulo',N'Panel de Administrador',N'Administrator panel'),
(N'Gestion-Productos.aspx',N'litAdministracion',N'Administración',N'Administration'),
(N'Gestion-Productos.aspx',N'litMenuPrincipal',N'Menú principal',N'Main menu'),
(N'Gestion-Productos.aspx',N'litMenuUsuarios',N'Gestión Usuarios',N'User management'),
(N'Gestion-Productos.aspx',N'litMenuBitacora',N'Bitácora eventos',N'Event log'),
(N'Gestion-Productos.aspx',N'litMenuPerfiles',N'Gestión Perfiles',N'Profile management'),
(N'Gestion-Productos.aspx',N'litMenuFamilias',N'Gestión Familias',N'Family management'),
(N'Gestion-Productos.aspx',N'litMenuBackup',N'Backup/Restore',N'Backup/Restore'),
(N'Gestion-Productos.aspx',N'litMenuIdiomas',N'Gestión Idiomas',N'Language management'),
(N'Gestion-Productos.aspx',N'litModoEdicion',N'Estás modificando el producto: {0}',N'You are editing product: {0}'),
(N'Gestion-Productos.aspx',N'litGestionMenu',N'Gestión de productos',N'Product management'),
(N'Gestion-Productos.aspx',N'litKicker',N'VITALPET · TIENDA',N'VITALPET · STORE'),
(N'Gestion-Productos.aspx',N'litTitulo',N'Gestión de productos',N'Product management'),
(N'Gestion-Productos.aspx',N'litNombre',N'Nombre',N'Name'),
(N'Gestion-Productos.aspx',N'litPrecio',N'Precio',N'Price'),
(N'Gestion-Productos.aspx',N'litDescripcion',N'Descripción',N'Description'),
(N'Gestion-Productos.aspx',N'litStock',N'Stock',N'Stock'),
(N'Gestion-Productos.aspx',N'litImagen',N'Imagen (PNG, JPG o GIF; máximo 8 MB)',N'Image (PNG, JPG or GIF; 8 MB maximum)'),
(N'Gestion-Productos.aspx',N'btnGuardar',N'Guardar producto',N'Save product'),
(N'Gestion-Productos.aspx',N'btnLimpiar',N'Limpiar',N'Clear'),
(N'Gestion-Productos.aspx',N'gvProductos_Header_Producto',N'Producto',N'Product'),
(N'Gestion-Productos.aspx',N'gvProductos_Header_Precio',N'Precio',N'Price'),
(N'Gestion-Productos.aspx',N'gvProductos_Header_Stock',N'Stock',N'Stock'),
(N'Gestion-Productos.aspx',N'gvProductos_Header_Acciones',N'Acciones',N'Actions'),
(N'Gestion-Productos.aspx',N'gvProductos_Vacio',N'Aún no hay productos cargados.',N'No products have been added yet.'),
(N'Gestion-Productos.aspx',N'btnEditar',N'Editar',N'Edit'),
(N'Gestion-Productos.aspx',N'btnEliminar',N'Eliminar',N'Delete'),
(N'Gestion-Productos.aspx',N'msgConfirmarEliminar',N'¿Eliminar este producto de la tienda?',N'Delete this product from the store?'),
(N'Gestion-Productos.aspx',N'lblFormularioNuevo',N'Nuevo producto',N'New product'),
(N'Gestion-Productos.aspx',N'lblFormularioEditar',N'Editar producto',N'Edit product'),
(N'Gestion-Productos.aspx',N'msgValidarNombreDescripcion',N'Ingresá nombre y descripción (hasta 1000 caracteres).',N'Enter a name and description (up to 1000 characters).'),
(N'Gestion-Productos.aspx',N'msgValidarPrecio',N'El precio debe ser mayor que cero.',N'Price must be greater than zero.'),
(N'Gestion-Productos.aspx',N'msgValidarStock',N'El stock debe ser un entero válido.',N'Stock must be a valid whole number.'),
(N'Gestion-Productos.aspx',N'msgProductoGuardado',N'Producto guardado.',N'Product saved.'),
(N'Gestion-Productos.aspx',N'msgErrorGuardar',N'No se pudo guardar el producto. Revisá los datos e intentá nuevamente.',N'The product could not be saved. Check the details and try again.'),
(N'Gestion-Productos.aspx',N'msgValidarTamanoImagen',N'La imagen debe pesar hasta 8 MB.',N'The image must be no larger than 8 MB.'),
(N'Gestion-Productos.aspx',N'msgValidarFormatoImagen',N'El archivo debe ser una imagen PNG, JPG o GIF válida.',N'The file must be a valid PNG, JPG or GIF image.'),
(N'Gestion-Productos.aspx',N'msgProductoEliminado',N'Producto eliminado de la tienda.',N'Product removed from the store.'),
(N'Gestion-Productos.aspx',N'msgErrorActualizar',N'No se pudo actualizar el producto.',N'The product could not be updated.');;

DELETE tr FROM dbo.Traduccion tr JOIN dbo.Etiqueta e ON e.IdEtiqueta=tr.IdEtiqueta WHERE e.Formulario=N'Carrito.aspx' AND e.ControlId IN(N'litPagoNotaTitulo',N'litNotaPago');
DELETE FROM dbo.Etiqueta WHERE Formulario=N'Carrito.aspx' AND ControlId IN(N'litPagoNotaTitulo',N'litNotaPago');

INSERT INTO dbo.Etiqueta(Formulario,ControlId)
SELECT src.Formulario,src.ControlId FROM @Etiquetas src
WHERE NOT EXISTS(SELECT 1 FROM dbo.Etiqueta e WHERE e.Formulario=src.Formulario AND e.ControlId=src.ControlId);

INSERT INTO dbo.Traduccion(IdIdioma,IdEtiqueta,Texto)
SELECT i.IdIdioma,e.IdEtiqueta,CASE i.Codigo WHEN N'es-AR' THEN src.TextoEs ELSE src.TextoEn END
FROM @Etiquetas src
JOIN dbo.Etiqueta e ON e.Formulario=src.Formulario AND e.ControlId=src.ControlId
CROSS JOIN dbo.Idioma i
WHERE i.Codigo IN(N'es-AR',N'en-US')
AND NOT EXISTS(SELECT 1 FROM dbo.Traduccion tr WHERE tr.IdIdioma=i.IdIdioma AND tr.IdEtiqueta=e.IdEtiqueta);

UPDATE tr SET Texto=CASE i.Codigo WHEN N'es-AR' THEN src.TextoEs ELSE src.TextoEn END
FROM dbo.Traduccion tr
JOIN dbo.Etiqueta e ON e.IdEtiqueta=tr.IdEtiqueta
JOIN @Etiquetas src ON src.Formulario=e.Formulario AND src.ControlId=e.ControlId
JOIN dbo.Idioma i ON i.IdIdioma=tr.IdIdioma;

COMMIT TRANSACTION;
