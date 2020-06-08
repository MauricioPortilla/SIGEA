using System.Collections.Generic;
using System.Drawing;

namespace SIGEA {
    class HerramientasGraficas {
        
        /// <summary>
        /// Dibuja un texto encerrado en un rectángulo.
        /// </summary>
        /// <param name="g">Gráficos</param>
        /// <param name="width">Largo</param>
        /// <param name="x">Posición en X</param>
        /// <param name="y">Posición en Y</param>
        /// <param name="pen">Pluma</param>
        /// <param name="text">Texto</param>
        /// <param name="font">Fuente</param>
        /// <param name="sb">Color</param>
        /// <returns>Tamaño del rectángulo dibujado</returns>
        public static SizeF DrawLabeledRectangle(ref Graphics g, float width, float x, float y, Pen pen, string text, Font font, SolidBrush sb) {
            RectangleF rect;
            if (text.Length > 15) {
                rect = new RectangleF(x, y, width, g.MeasureString(text, font).Height * (text.Length / 15));
            } else {
                rect = new RectangleF(x, y, width, g.MeasureString(text, font).Height);
            }
            rect = RectangleF.Inflate(rect, 6, 6);
            g.DrawRectangle(pen, Rectangle.Round(rect));
            StringFormat sf = new StringFormat {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            sf.Trimming = StringTrimming.EllipsisWord;
            g.DrawString(text, font, sb, rect, sf);
            return rect.Size;
        }

        /// <summary>
        /// Dibuja un texto encerrado en un rectángulo
        /// </summary>
        /// <param name="g">Gráficos</param>
        /// <param name="size">Medidas</param>
        /// <param name="x">Posición en X</param>
        /// <param name="y">Posición en Y</param>
        /// <param name="pen">Pluma</param>
        /// <param name="text">Texto</param>
        /// <param name="font">Fuente</param>
        /// <param name="sb">Color</param>
        /// <returns>Tamaño del rectángulo dibujado</returns>
        public static SizeF DrawLabeledRectangle(ref Graphics g, SizeF size, float x, float y, Pen pen, string text, Font font, SolidBrush sb) {
            RectangleF rect;
            rect = new RectangleF(x, y, size.Width, size.Height);
            rect = RectangleF.Inflate(rect, 6, 6);
            g.DrawRectangle(pen, Rectangle.Round(rect));
            StringFormat sf = new StringFormat {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            sf.Trimming = StringTrimming.EllipsisWord;
            g.DrawString(text, font, sb, rect, sf);
            return rect.Size;
        }

        /// <summary>
        /// Obtiene la medida de un rectángulo con un texto adentro.
        /// </summary>
        /// <param name="g">Gráficos</param>
        /// <param name="text">Texto</param>
        /// <param name="font">Fuente</param>
        /// <returns>Medida de un rectángulo con un texto adentro</returns>
        public static SizeF GetLabeledRectangleSize(Graphics g, string text, Font font) {
            SizeF size = g.MeasureString(text, font);
            RectangleF rect = new RectangleF {
                Size = new SizeF(size.Width, size.Height)
            };
            rect = RectangleF.Inflate(rect, 6, 6);
            return rect.Size;
        }

        /// <summary>
        /// Obtiene la medida de un rectángulo con un texto adentro.
        /// </summary>
        /// <param name="g">Gráficos</param>
        /// <param name="text">Texto</param>
        /// <param name="font">Fuente</param>
        /// <param name="textLength">Longitud máxima del texto</param>
        /// <returns>Medida de un rectángulo con un texto adentro</returns>
        public static SizeF GetLabeledRectangleSize(Graphics g, string text, Font font, int textLength) {
            SizeF size = g.MeasureString(text, font);
            RectangleF rect;
            if (text.Length > textLength) {
                rect = new RectangleF(0, 0, size.Width, g.MeasureString(text, font).Height * (text.Length / textLength));
            } else {
                rect = new RectangleF(0, 0, size.Width, g.MeasureString(text, font).Height);
            }
            return rect.Size;
        }

        /// <summary>
        /// Obtiene una lista del ancho de cada encabezado con base en multiplicadores asignados
        /// a cada encabezado, los cuales indican el ancho que debe ocupar cada uno.
        /// </summary>
        /// <param name="g">Gráficos</param>
        /// <param name="headers">Encabezados</param>
        /// <param name="headersPreferredMultiplier">Multiplicadores de encabezados</param>
        /// <param name="font">Fuente</param>
        /// <returns>Lista de anchos de cada encabezado</returns>
        public static List<float> GetHeadersWidth(Graphics g, string[] headers, float[] headersPreferredMultiplier, Font font) {
            List<float> headersWidth = new List<float>();
            for (int i = 0; i < headers.Length; i++) {
                var originalSize = GetLabeledRectangleSize(g, headers[i], font).Width;
                if (headersPreferredMultiplier[i] == 0)
                    headersWidth.Add(originalSize);
                else {
                    headersWidth.Add(originalSize * headersPreferredMultiplier[i]);
                }
            }
            return headersWidth;
        }

        /// <summary>
        /// Obtiene el alto de cada fila de datos.
        /// </summary>
        /// <param name="g">Gráficos</param>
        /// <param name="rowContent">Lista de contenidos de cada fila</param>
        /// <param name="font">Fuente</param>
        /// <returns>Lista de altos de cada fila</returns>
        public static List<float> GetRowsHeight(Graphics g, List<List<string>> rowContent, Font font) {
            List<float> rowsHeight = new List<float>();
            for (int k = 0; k < rowContent.Count; k++) { // Rows
                float rowHeight = 0f;
                for (int v = 0; v < rowContent[k].Count; v++) { // Cols
                    var rect = GetLabeledRectangleSize(g, rowContent[k][v], font, 15);
                    if (rowHeight < rect.Height) {
                        rowHeight = rect.Height;
                    }
                }
                rowsHeight.Add(rowHeight);
            }
            return rowsHeight;
        }

        /// <summary>
        /// Dibuja una tabla.
        /// </summary>
        /// <param name="g">Gráficos</param>
        /// <param name="headers">Encabezados</param>
        /// <param name="headersPreferredMultiplier">Multiplicadores de encabezados</param>
        /// <param name="rowContent">Datos de cada fila</param>
        /// <param name="x">Posición en X</param>
        /// <param name="y">Posición en Y</param>
        /// <param name="docHeight">Alto del documento</param>
        /// <param name="finished">true si terminó de dibujar todas las filas; false si no</param>
        /// <returns>Tamaño de la tabla en altura</returns>
        public static float DrawTable(ref Graphics g, string[] headers, float[] headersPreferredMultiplier, ref List<List<string>> rowContent, float x, float y, float docHeight, out bool finished) {
            Font fbody = new Font("Arial", 10, FontStyle.Regular);
            Font fbody_bold = new Font("Arial", 10, FontStyle.Bold);
            SolidBrush sb = new SolidBrush(Color.Black);
            List<float> headersWidth = GetHeadersWidth(g, headers, headersPreferredMultiplier, fbody_bold);
            List<float> rowsHeight = GetRowsHeight(g, rowContent, fbody);
            //Crear la tabla
            //Crear headers
            float spaceX = x, spaceY = y, heightTemp = 0f;
            for (int i = 0; i < headersWidth.Count; i++) {
                var draw = DrawLabeledRectangle(ref g, headersWidth[i], spaceX, spaceY, Pens.Black, headers[i], fbody_bold, sb);
                spaceX += draw.Width;
                heightTemp = draw.Height;
            }
            spaceY += heightTemp;
            //Crear contenido
            spaceX = x;
            int rowContentCount = rowContent.Count;
            List<List<string>> rowContentToDelete = new List<List<string>>();
            finished = true;
            for (int row = 0; row < rowContent.Count; row++) {
                for (int i = 0; i < rowContent[row].Count; i++) { // Read cols
                    if (GetLabeledRectangleSize(g, rowContent[row][rowContent[row].Count - 1], fbody).Height + spaceY > docHeight) {
                        finished = false;
                        break;
                    }
                    var draw = DrawLabeledRectangle(ref g, new SizeF(headersWidth[i], rowsHeight[row]), spaceX, spaceY, Pens.Black, rowContent[row][i], fbody, sb);
                    spaceX += draw.Width;
                    heightTemp = draw.Height;
                }
                if (!finished)
                    break;
                spaceX = x;
                spaceY += heightTemp;
                rowContentToDelete.Add(rowContent[row]);
            }
            foreach (var rowDeleted in rowContentToDelete) {
                rowContent.Remove(rowDeleted);
            }
            if (!finished) {
                return spaceY - y;
            }
            // Terminó con éxito
            finished = true;
            return spaceY - y; // Retorna el tamaño de la tabla
        }
    }
}
