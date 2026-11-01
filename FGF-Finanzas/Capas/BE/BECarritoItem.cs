namespace FGF_Finanzas.Capas.BE
{
    public class BECarritoItem
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public int Stock { get; set; }
        public string Imagen { get; set; }
        public bool SinStockSuficiente { get { return Stock < Cantidad; } }
        public decimal Subtotal { get { return SinStockSuficiente ? 0 : Precio * Cantidad; } }
    }
}
