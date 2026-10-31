using FGF_Finanzas.Capas.BE;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;
using System.Linq;
using System.Globalization;

namespace FGF_Finanzas.Capas.Servicios
{
    public class ServicioInformePdf
    {
        public static string Resumen(BEInformeConsultas informe, Func<string, string> texto)
        {
            return string.Format(texto("resumen"), informe.Detalle.Sum(r => r.Turnos), informe.Detalle.Sum(r => r.Atendidos),
                informe.Detalle.Sum(r => r.Cancelados), informe.Detalle.Sum(r => r.Ausentes), informe.Detalle.Sum(r => r.Pendientes));
        }

        public static string Lideres(BEInformeConsultas informe, Func<string, string> texto)
        {
            int maximo = informe.Detalle.Select(r => r.Atendidos).DefaultIfEmpty().Max();
            return maximo == 0 ? texto("sinAtenciones") : string.Format(texto("lideres"),
                string.Join(", ", informe.Detalle.Where(r => r.Atendidos == maximo).Select(r => r.Veterinario)), maximo);
        }

        public byte[] Generar(BEInformeConsultas informe, Func<string, string> texto, string idioma, string logo)
        {
            var cultura = CultureInfo.GetCultureInfo(idioma);
            var fuente = BaseFont.CreateFont(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            var normal = new Font(fuente, 9.5f);
            var titulo = new Font(fuente, 17, Font.BOLD, new BaseColor(31, 58, 95));
            var azul = new BaseColor(128, 195, 218);
            using (var memoria = new MemoryStream())
            {
                using (var documento = new Document(PageSize.A4, 30, 30, 25, 30))
                {
                    var writer = PdfWriter.GetInstance(documento, memoria);
                    writer.PageEvent = new Numeracion(fuente, texto("pagina"));
                    documento.AddTitle(texto("lblTitulo"));
                    documento.AddAuthor("VitalPet");
                    documento.Open();
                    if (File.Exists(logo))
                    {
                        var imagen = Image.GetInstance(logo); imagen.ScaleToFit(45, 35); documento.Add(imagen);
                    }
                    documento.Add(new Paragraph("VitalPet — " + texto("lblTitulo"), titulo));
                    documento.Add(new Paragraph(string.Format(texto("periodo"), cultura.DateTimeFormat.GetMonthName(informe.Mes), informe.Anio), normal));
                    documento.Add(new Paragraph(texto("lblVeterinario") + " " + (informe.NombreVeterinario ?? texto("todos")), normal));
                    documento.Add(new Paragraph(texto("generado") + " " + informe.Generado.ToString("g", cultura), normal) { SpacingAfter = 12 });
                    documento.Add(new Paragraph(Resumen(informe, texto), normal) { SpacingAfter = 7 });
                    documento.Add(new Paragraph(Lideres(informe, texto), normal) { SpacingAfter = 12 });
                    var tabla = new PdfPTable(7) { WidthPercentage = 100, HeaderRows = 1, SpacingAfter = 15 };
                    tabla.SetWidths(new float[] { 3, 1.3f, 1.1f, 1.1f, 1.1f, 1.1f, 1.3f });
                    string[] claves = { "colVeterinario", "colTurnos", "colAtendidos", "colCancelados", "colAusentes", "colPendientes", "colPorcentaje" };
                    foreach (string clave in claves) tabla.AddCell(new PdfPCell(new Phrase(texto(clave), new Font(fuente, 8.5f, Font.BOLD))) { BackgroundColor = azul, Padding = 4 });
                    foreach (var fila in informe.Detalle)
                    {
                        string[] valores = { fila.Veterinario, fila.Turnos.ToString(), fila.Atendidos.ToString(), fila.Cancelados.ToString(), fila.Ausentes.ToString(), fila.Pendientes.ToString(), fila.Porcentaje.ToString("N1", cultura) + " %" };
                        foreach (string valor in valores) tabla.AddCell(new PdfPCell(new Phrase(valor, new Font(fuente, 9))) { Padding = 4 });
                    }
                    if (informe.Detalle.Count == 0) tabla.AddCell(new PdfPCell(new Phrase(texto("sinDatos"), normal)) { Colspan = 7, Padding = 8 });
                    documento.Add(tabla);
                    {
                        int[] cantidades = { informe.Detalle.Sum(r => r.Atendidos), informe.Detalle.Sum(r => r.Cancelados),
                            informe.Detalle.Sum(r => r.Ausentes), informe.Detalle.Sum(r => r.Pendientes) };
                        string[] estados = { "colAtendidos", "colCancelados", "colAusentes", "colPendientes" };
                        BaseColor[] colores = { azul, new BaseColor(160, 160, 160), new BaseColor(221, 149, 92), new BaseColor(229, 197, 111) };
                        var dibujo = writer.DirectContent.CreateTemplate(535, 120);
                        int total = cantidades.Sum(); float inicio = 90;
                        if (total == 0)
                        {
                            dibujo.SetColorFill(new BaseColor(230, 230, 230)); dibujo.Circle(80, 60, 50); dibujo.Fill();
                        }
                        else for (int i = 0; i < cantidades.Length; i++)
                        {
                            if (cantidades[i] == 0) continue;
                            float angulo = -360f * cantidades[i] / total;
                            dibujo.SetColorFill(colores[i]); dibujo.SetColorStroke(BaseColor.WHITE); dibujo.SetLineWidth(1);
                            dibujo.Arc(30, 10, 130, 110, inicio, angulo); dibujo.LineTo(80, 60); dibujo.ClosePathFillStroke();
                            inicio += angulo;
                        }
                        for (int i = 0; i < cantidades.Length; i++)
                        {
                            float y = 94 - i * 23;
                            dibujo.SetColorFill(colores[i]); dibujo.Rectangle(175, y - 2, 12, 12); dibujo.Fill();
                            dibujo.SetColorFill(BaseColor.BLACK);
                            ColumnText.ShowTextAligned(dibujo, Element.ALIGN_LEFT, new Phrase(texto(estados[i]) + ": " + cantidades[i], normal), 198, y, 0);
                        }
                        var grafico = new PdfPTable(1) { WidthPercentage = 100, KeepTogether = true, SpacingAfter = 8 };
                        grafico.AddCell(new PdfPCell(new Phrase(texto("graficoEstados"), new Font(fuente, 12, Font.BOLD))) { Border = Rectangle.NO_BORDER, PaddingBottom = 4 });
                        grafico.AddCell(new PdfPCell(Image.GetInstance(dibujo), true) { Border = Rectangle.NO_BORDER });
                        documento.Add(grafico);
                    }
                    if (string.IsNullOrEmpty(informe.DniVeterinario) && informe.Detalle.Count > 0)
                    {
                        int paso = Math.Max(1, (int)Math.Ceiling(informe.Detalle.Max(r => r.Atendidos) / 5d));
                        for (int inicio = 0; inicio < informe.Detalle.Count; inicio += 6)
                        {
                            var filas = informe.Detalle.Skip(inicio).Take(6).ToList();
                            var dibujo = writer.DirectContent.CreateTemplate(535, 165);
                            const float izquierda = 50, abajo = 50, ancho = 470, alto = 85;
                            var pequena = new Font(fuente, 9);
                            for (int marca = 0; marca <= 5; marca++)
                            {
                                float y = abajo + alto * marca / 5;
                                dibujo.SetColorStroke(new BaseColor(220, 220, 220));
                                dibujo.SetLineWidth(0.5f); dibujo.MoveTo(izquierda, y); dibujo.LineTo(izquierda + ancho, y); dibujo.Stroke();
                                ColumnText.ShowTextAligned(dibujo, Element.ALIGN_RIGHT, new Phrase((marca * paso).ToString(), pequena), izquierda - 8, y - 3, 0);
                            }
                            dibujo.SetColorStroke(BaseColor.BLACK); dibujo.SetLineWidth(1);
                            dibujo.MoveTo(izquierda, abajo + alto); dibujo.LineTo(izquierda, abajo); dibujo.LineTo(izquierda + ancho, abajo); dibujo.Stroke();
                            ColumnText.ShowTextAligned(dibujo, Element.ALIGN_CENTER, new Phrase(texto("colAtendidos"), normal), 15, abajo + alto / 2, 90);
                            ColumnText.ShowTextAligned(dibujo, Element.ALIGN_CENTER, new Phrase(texto("colVeterinario"), normal), izquierda + ancho / 2, 6, 0);
                            float espacio = ancho / filas.Count;
                            for (int i = 0; i < filas.Count; i++)
                            {
                                var fila = filas[i];
                                float centro = izquierda + espacio * (i + 0.5f);
                                float barra = Math.Min(55, espacio * 0.6f), altura = alto * fila.Atendidos / (paso * 5f);
                                dibujo.SetColorFill(azul); dibujo.Rectangle(centro - barra / 2, abajo, barra, altura); dibujo.Fill();
                                dibujo.SetColorFill(BaseColor.BLACK);
                                ColumnText.ShowTextAligned(dibujo, Element.ALIGN_CENTER, new Phrase(fila.Atendidos.ToString(), pequena), centro, abajo + altura + 5, 0);
                                var nombre = new ColumnText(dibujo);
                                nombre.SetSimpleColumn(new Phrase(fila.Veterinario, pequena), centro - espacio / 2 + 3, 14, centro + espacio / 2 - 3, abajo - 7, 10, Element.ALIGN_CENTER);
                                nombre.Go();
                            }
                            var grafico = new PdfPTable(1) { WidthPercentage = 100, KeepTogether = true, SpacingAfter = 10 };
                            grafico.AddCell(new PdfPCell(new Phrase(texto("grafico"), new Font(fuente, 12, Font.BOLD))) { Border = Rectangle.NO_BORDER, PaddingBottom = 8 });
                            grafico.AddCell(new PdfPCell(Image.GetInstance(dibujo), true) { Border = Rectangle.NO_BORDER });
                            documento.Add(grafico);
                        }
                    }
                    if (informe.Meses.Count == 12)
                    {
                        var dibujo = writer.DirectContent.CreateTemplate(535, 155);
                        const float izquierda = 50, abajo = 35, ancho = 470, alto = 90;
                        var pequena = new Font(fuente, 9);
                        int paso = Math.Max(1, (int)Math.Ceiling(informe.Meses.Max(m => m.Atendidos) / 5d));
                        for (int marca = 0; marca <= 5; marca++)
                        {
                            float y = abajo + alto * marca / 5;
                            dibujo.SetColorStroke(new BaseColor(220, 220, 220)); dibujo.SetLineWidth(0.5f);
                            dibujo.MoveTo(izquierda, y); dibujo.LineTo(izquierda + ancho, y); dibujo.Stroke();
                            ColumnText.ShowTextAligned(dibujo, Element.ALIGN_RIGHT, new Phrase((marca * paso).ToString(), pequena), izquierda - 8, y - 3, 0);
                        }
                        dibujo.SetColorStroke(BaseColor.BLACK); dibujo.SetLineWidth(1);
                        dibujo.MoveTo(izquierda, abajo + alto); dibujo.LineTo(izquierda, abajo); dibujo.LineTo(izquierda + ancho, abajo); dibujo.Stroke();
                        ColumnText.ShowTextAligned(dibujo, Element.ALIGN_CENTER, new Phrase(texto("colAtendidos"), normal), 15, abajo + alto / 2, 90);
                        ColumnText.ShowTextAligned(dibujo, Element.ALIGN_CENTER, new Phrase(texto("lblMes"), normal), izquierda + ancho / 2, 6, 0);
                        var meses = informe.Meses.OrderBy(m => m.Mes).ToList();
                        dibujo.SetColorStroke(new BaseColor(31, 100, 135)); dibujo.SetLineWidth(2);
                        for (int i = 0; i < meses.Count; i++)
                        {
                            float x = izquierda + ancho * i / 11, y = abajo + alto * meses[i].Atendidos / (paso * 5f);
                            if (i == 0) dibujo.MoveTo(x, y); else dibujo.LineTo(x, y);
                        }
                        dibujo.Stroke();
                        for (int i = 0; i < meses.Count; i++)
                        {
                            float x = izquierda + ancho * i / 11, y = abajo + alto * meses[i].Atendidos / (paso * 5f);
                            dibujo.SetColorFill(new BaseColor(31, 100, 135)); dibujo.Circle(x, y, 3); dibujo.Fill();
                            dibujo.SetColorFill(BaseColor.BLACK);
                            ColumnText.ShowTextAligned(dibujo, Element.ALIGN_CENTER, new Phrase(meses[i].Atendidos.ToString(), pequena), x, y + 7, 0);
                            ColumnText.ShowTextAligned(dibujo, Element.ALIGN_CENTER, new Phrase(cultura.DateTimeFormat.GetAbbreviatedMonthName(meses[i].Mes), pequena), x, abajo - 16, 0);
                        }
                        var grafico = new PdfPTable(1) { WidthPercentage = 100, KeepTogether = true };
                        grafico.AddCell(new PdfPCell(new Phrase(string.Format(texto("graficoAnual"), informe.Anio), new Font(fuente, 12, Font.BOLD))) { Border = Rectangle.NO_BORDER, PaddingBottom = 8 });
                        grafico.AddCell(new PdfPCell(Image.GetInstance(dibujo), true) { Border = Rectangle.NO_BORDER });
                        documento.Add(grafico);
                    }
                    documento.Close();
                }
                return memoria.ToArray();
            }
        }

        private class Numeracion : PdfPageEventHelper
        {
            private readonly BaseFont fuente; private readonly string texto;
            public Numeracion(BaseFont fuente, string texto) { this.fuente = fuente; this.texto = texto; }
            public override void OnEndPage(PdfWriter writer, Document document)
            {
                ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_RIGHT,
                    new Phrase(texto + " " + writer.PageNumber, new Font(fuente, 9)), document.Right, 16, 0);
            }
        }
    }
}
