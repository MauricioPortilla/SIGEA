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
using System.Drawing.Imaging;
using System.Drawing;

namespace SIGEA {
    /// <summary>
    /// Lógica de interacción para GenerarProgramaEvento.xaml
    /// </summary>
    public partial class GenerarProgramaEvento : Window {
        private Evento evento;
        public List<ActividadPresentacionTabla> ActividadPresentacionList { get; } = new List<ActividadPresentacionTabla>();
        public string RutaSeleccionada;

        /// <summary>
        /// Crea uns instancia.
        /// </summary>
        /// <param name="evento">Evento del cual se generará el programa</param>
        public GenerarProgramaEvento(Evento evento) {
            InitializeComponent();
            DataContext = this;
            this.evento = evento;
            CargarActividadesPresentaciones();
        }

        /// <summary>
        /// Carga las actividades del evento y las presentaciones de cada actividad
        /// para mostrarlas en la tabla.
        /// </summary>
        private void CargarActividadesPresentaciones() {
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    var actividades = sigeaBD.Actividad.Where(actividad => actividad.id_evento == evento.id_evento);
                    foreach (Actividad actividad in actividades) {
                        foreach (Presentacion presentacion in actividad.Presentacion) {
                            ActividadPresentacionList.Add(new ActividadPresentacionTabla {
                                Nombre = actividad.nombre,
                                Fecha = presentacion.fechaPresentacion.ToString("dd/MM/yyyy"),
                                HoraInicio = presentacion.horaInicio,
                                HoraFin = presentacion.horaFin
                            });
                        }
                    }
                }
            } catch (Exception) {
                MessageBox.Show("Error al cargar el programa del evento");
            }
        }

        /// <summary>
        /// Muestra una ventana de explorador de archivos y genera el programa
        /// en la ruta seleccionada.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void GenerarProgramaButton_Click(object sender, RoutedEventArgs e) {
            Forms.FolderBrowserDialog exploradorArchivos = new Forms.FolderBrowserDialog();
            if (exploradorArchivos.ShowDialog() != Forms.DialogResult.OK) {
                MessageBox.Show("Debes seleccionar una ruta.");
                return;
            }
            RutaSeleccionada = exploradorArchivos.SelectedPath;
            List<List<string>> rowsContent = new List<List<string>>();
            int contador = 1;
            foreach (ActividadPresentacionTabla actividadPresentacionTabla in ActividadPresentacionList) {
                if (rowsContent.Count == 13) {
                    GenerarPrograma(rowsContent, contador++);
                    rowsContent.Clear();
                }
                rowsContent.Add(new List<string>() {
                    actividadPresentacionTabla.Nombre, actividadPresentacionTabla.Fecha, 
                    actividadPresentacionTabla.HoraInicio.ToString(), actividadPresentacionTabla.HoraFin.ToString()
                });
            }
            if (rowsContent.Count > 0) {
                GenerarPrograma(rowsContent, contador);
            }
            MessageBox.Show("Programa generado con éxito.");
        }

        /// <summary>
        /// Genera un archivo del programa con base en la información proporcionada
        /// y le asigna un número al archivo con el fin de identificarlo.
        /// </summary>
        /// <param name="actividadesPresentacionesFilas"></param>
        /// <param name="numeroArchivo"></param>
        public void GenerarPrograma(List<List<string>> actividadesPresentacionesFilas, int numeroArchivo) {
            int alturaPrograma = 595;
            Bitmap reporte = new Bitmap(842, alturaPrograma);
            Graphics g = Graphics.FromImage(reporte);
            Font fontSubtitulos = new Font("Arial", 18, System.Drawing.FontStyle.Regular);
            SolidBrush sb = new SolidBrush(System.Drawing.Color.Black);
            float spaceX = 10f;
            float spaceY = 10f;
            g.FillRectangle(System.Drawing.Brushes.White, 0, 0, reporte.Width, reporte.Height);
            g.DrawString("Programa del Evento: " + evento.nombre, fontSubtitulos, sb, spaceX, spaceY);
            spaceY += 50f;
            HerramientasGraficas.DrawTable(
                ref g,
                new string[] { "Nombre", "Fecha", "Hora de Inicio", "Hora de Fin" },
                new float[] { 2f, 1.5f, 1.5f, 1.5f },
                ref actividadesPresentacionesFilas,
                spaceX, spaceY, alturaPrograma, out _
            );
            reporte.Save(
                RutaSeleccionada + "/ProgramaEvento_" + evento.nombre + "_" +
                numeroArchivo + ".png",
                ImageFormat.Png
            );
        }

        /// <summary>
        /// Representa una Actividad junto con su Presentación en una tabla.
        /// </summary>
        public struct ActividadPresentacionTabla {
            public string Nombre { get; set; }
            public string Fecha { get; set; }
            public TimeSpan HoraInicio { get; set; }
            public TimeSpan HoraFin { get; set; }
        }
    }
}
