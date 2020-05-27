using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Forms = System.Windows.Forms;
using SIGEABD;
using System.Drawing;
using System.Drawing.Imaging;

namespace SIGEA {
    /// <summary>
    /// Lógica de interacción para GenerarReporteIngresosActividad.xaml
    /// </summary>
    public partial class GenerarReporteIngresosActividad : Window {
        private Actividad actividad;
        public List<PagoTabla> PagosList { get; } = new List<PagoTabla>();
        private float sumaTotal = 0;
        public string RutaSeleccionada = string.Empty;

        /// <summary>
        /// Crea una instancia.
        /// </summary>
        /// <param name="actividad">Actividad con la cual se genera el reporte</param>
        public GenerarReporteIngresosActividad(Actividad actividad) {
            InitializeComponent();
            DataContext = this;
            this.actividad = actividad;
            CargarPagos();
        }

        /// <summary>
        /// Muestra el panel principal al cerrarse.
        /// </summary>
        /// <param name="e">Evento</param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e) {
            new PanelLiderEvento().Show();
        }

        /// <summary>
        /// Carga los pagos de la base de datos y los ingresa en la tabla.
        /// </summary>
        private void CargarPagos() {
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    var pagos = sigeaBD.Pago.Where(pago => pago.id_actividad == this.actividad.id_actividad);
                    foreach (Pago pago in pagos) {
                        PagosList.Add(new PagoTabla {
                            FechaPago = pago.fecha.ToString("dd/MM/yyyy"),
                            Cantidad = (float) pago.cantidad
                        });
                        sumaTotal += (float) pago.cantidad;
                    }
                    sumaTotalTextBlock.Text = sumaTotal.ToString();
                }
            } catch (Exception) {
                MessageBox.Show("Error al cargar el reporte.");
            }
        }

        /// <summary>
        /// Solicita la ruta para almacenar el reporte y genera el reporte de los pagos.
        /// Por cada 5 pagos, se generará una imagen nueva en la ruta seleccionada.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void GenerarReporteButton_Click(object sender, RoutedEventArgs e) {
            Forms.FolderBrowserDialog exploradorArchivos = new Forms.FolderBrowserDialog();
            if (exploradorArchivos.ShowDialog() != Forms.DialogResult.OK) {
                MessageBox.Show("Debes seleccionar una ruta.");
                return;
            }
            RutaSeleccionada = exploradorArchivos.SelectedPath;
            List<List<string>> rowsContent = new List<List<string>>();
            int contador = 1;
            foreach (PagoTabla pagoTabla in PagosList) {
                if (rowsContent.Count == 5) {
                    GenerarReporte(rowsContent, contador++);
                    rowsContent.Clear();
                }
                rowsContent.Add(new List<string>() {
                    pagoTabla.FechaPago, pagoTabla.Cantidad.ToString()
                });
            }
            if (rowsContent.Count > 0) {
                GenerarReporte(rowsContent, contador);
            }
            MessageBox.Show("Reporte generado con éxito.");
        }

        /// <summary>
        /// Genera el reporte con base en un conjunto de datos de pagos.
        /// </summary>
        /// <param name="pagosFilas">Datos de los pagos</param>
        /// <param name="numeroArchivo">Número de archivo a generar</param>
        public void GenerarReporte(List<List<string>> pagosFilas, int numeroArchivo) {
            int alturaReporte = 595;
            Bitmap reporte = new Bitmap(842, alturaReporte);
            Graphics g = Graphics.FromImage(reporte);
            Font fontSubtitulos = new Font("Arial", 18, System.Drawing.FontStyle.Regular);
            SolidBrush sb = new SolidBrush(System.Drawing.Color.Black);
            float spaceX = 10f;
            float spaceY = 10f;
            g.FillRectangle(System.Drawing.Brushes.White, 0, 0, reporte.Width, reporte.Height);
            g.DrawString("Reporte de Ingresos de Actividad: " + actividad.nombre, fontSubtitulos, sb, spaceX, spaceY);
            spaceY += 50f;
            HerramientasGraficas.DrawTable(
                ref g,
                new string[] { "Fecha de pago", "Cantidad" },
                new float[] { 1.2f, 2f },
                ref pagosFilas,
                spaceX, spaceY, alturaReporte, out _
            );
            reporte.Save(
                RutaSeleccionada + "/ReporteActividad_" + this.actividad.nombre + "_" + 
                numeroArchivo + ".png", 
                ImageFormat.Png
            );
        }

        /// <summary>
        /// Representa un Pago en una tabla.
        /// </summary>
        public struct PagoTabla {
            public string FechaPago { get; set; }
            public float Cantidad { get; set; }
        }
    }
}
