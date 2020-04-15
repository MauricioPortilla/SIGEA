using SIGEABD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using Forms = System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Drawing.Printing;
using System.Drawing;
using System.Drawing.Imaging;

namespace SIGEA {
    /// <summary>
    /// Lógica de interacción para GenerarConstanciasEvento.xaml
    /// </summary>
    public partial class GenerarConstanciasEvento : Window {
        
        public List<AsistenteTabla> AsistentesLista { get; } = new List<AsistenteTabla>();
        public string DirectorioSeleccionado = string.Empty;
        private Evento evento;

        /// <summary>
        /// Crea una instancia.
        /// </summary>
        /// <param name="id_evento">Identificador del Evento</param>
        public GenerarConstanciasEvento(int id_evento) {
            InitializeComponent();
            DataContext = this;
            CargarAsistentes(id_evento);
        }

        /// <summary>
        /// Carga todos los asistentes del evento y los muestra en
        /// la tabla.
        /// </summary>
        /// <param name="id_evento">Identificador del Evento</param>
        private void CargarAsistentes(int id_evento) {
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    evento = sigeaBD.Evento.Find(id_evento);
                    var asistentes = sigeaBD.Asistente.Where(
                        asistente => asistente.Evento.FirstOrDefault(
                            eventoLista => eventoLista.id_evento == evento.id_evento
                        ) != null
                    );
                    foreach (Asistente asistente in asistentes.ToList()) {
                        AsistentesLista.Add(new AsistenteTabla {
                            Nombre = asistente.nombre,
                            Paterno = asistente.paterno,
                            Materno = asistente.materno ?? ""
                        });
                    }
                }
            } catch (Exception) {
                MessageBox.Show("Error al cargar los asistentes del evento.");
            }
        }

        /// <summary>
        /// Abre una ventana de explorador de archivos y almacena la ruta de
        /// la carpeta seleccionada.
        /// </summary>
        /// <returns>true si se seleccionó una carpeta; false si no</returns>
        private bool SeleccionarDirectorio() {
            Forms.FolderBrowserDialog exploradorArchivos = new Forms.FolderBrowserDialog();
            if (exploradorArchivos.ShowDialog() == Forms.DialogResult.OK) {
                DirectorioSeleccionado = exploradorArchivos.SelectedPath;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Verifica si se seleccionó un asistente de la tabla, abre una ventana de
        /// explorador de archivos, verifica si se seleccionó una carpeta y genera
        /// y guarda la constancia del asistente seleccionado en la carpeta seleccionada.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void GenerarSeleccionadoButton_Click(object sender, RoutedEventArgs e) {
            if (asistentesListView.SelectedItem == null) {
                MessageBox.Show("Debes seleccionar un asistente.");
                return;
            }
            if (!SeleccionarDirectorio()) {
                return;
            }
            GenerarConstancias(false);
            MessageBox.Show("Se ha generado la constancia.");
        }

        /// <summary>
        /// Abre una ventana de explorador de archivos, verifica si se seleccionó una
        /// carpeta y genera y guarda las constancias de todos los asistentes en
        /// la carpeta seleccionada.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void GenerarTodasButton_Click(object sender, RoutedEventArgs e) {
            if (!SeleccionarDirectorio()) {
                return;
            }
            GenerarConstancias();
            MessageBox.Show("Se han generado las constancias.");
        }

        /// <summary>
        /// Genera una constancia y la guarda en la ruta seleccionada con el nombre completo
        /// del asistente en formato PNG.
        /// </summary>
        /// <param name="asistenteTabla">Asistente en tabla</param>
        public void GenerarConstancia(AsistenteTabla asistenteTabla) {
            var nombreAsistente = asistenteTabla.Paterno + " " + (asistenteTabla.Materno ?? "") + " " + asistenteTabla.Nombre;
            Bitmap constancia = new Bitmap(842, 595);
            Graphics g = Graphics.FromImage(constancia);
            Font fontTitulos = new Font("Arial", 30, System.Drawing.FontStyle.Regular);
            Font fontSubtitulos = new Font("Arial", 18, System.Drawing.FontStyle.Regular);
            SolidBrush sb = new SolidBrush(System.Drawing.Color.Black);
            float spaceX = 0f;
            float spaceY = 100f;
            g.FillRectangle(System.Drawing.Brushes.White, 0, 0, constancia.Width, constancia.Height);
            spaceX = (constancia.Width / 2) - (g.MeasureString("Se otorga la presente", fontSubtitulos).Width / 2);
            g.DrawString("Se otorga la presente", fontSubtitulos, sb, spaceX, spaceY);
            spaceY += 35f;
            spaceX = (constancia.Width / 2) - (g.MeasureString("CONSTANCIA", fontTitulos).Width / 2);
            g.DrawString("CONSTANCIA", fontTitulos, sb, spaceX, spaceY);
            spaceY += 35f;
            spaceX = (constancia.Width / 2) - (g.MeasureString("a:", fontSubtitulos).Width / 2);
            g.DrawString("a:", fontSubtitulos, sb, spaceX, spaceY);
            spaceY += 22f;
            spaceX = (constancia.Width / 2) - (g.MeasureString(nombreAsistente, fontTitulos).Width / 2);
            g.DrawString(nombreAsistente, fontTitulos, sb, spaceX, spaceY);
            spaceY += 65f;
            spaceX = (constancia.Width / 2) - (g.MeasureString("Por su asistencia al evento:", fontSubtitulos).Width / 2);
            g.DrawString("Por su asistencia al evento:", fontSubtitulos, sb, spaceX, spaceY);
            spaceY += 28f;
            spaceX = (constancia.Width / 2) - (g.MeasureString(evento.nombre, fontTitulos).Width / 2);
            g.DrawString(evento.nombre, fontTitulos, sb, spaceX, spaceY);
            spaceY += 70f;
            spaceX = (constancia.Width / 2) - (
                g.MeasureString(
                    "Del " + evento.fechaInicio.ToShortDateString() + " al " + evento.fechaFin.ToShortDateString(), 
                    fontSubtitulos
                ).Width / 2
            );
            g.DrawString(
                "Del " + evento.fechaInicio.ToShortDateString() + " al " + evento.fechaFin.ToShortDateString(), 
                fontSubtitulos, sb, spaceX, spaceY
            );
            constancia.Save(DirectorioSeleccionado + "/" + nombreAsistente + ".png", ImageFormat.Png);
        }

        /// <summary>
        /// Genera constancias de los asistentes.
        /// </summary>
        /// <param name="imprimirTodas">
        ///     true para imprimir todas; false para imprimir solo la del asistente seleccionado
        /// </param>
        private void GenerarConstancias(bool imprimirTodas = true) {
            if (imprimirTodas) {
                foreach (AsistenteTabla asistenteTabla in AsistentesLista) {
                    GenerarConstancia(asistenteTabla);
                }
            } else {
                GenerarConstancia((AsistenteTabla) asistentesListView.SelectedItem);
            }
        }

        /// <summary>
        /// Representa un Asistente en una tabla.
        /// </summary>
        public struct AsistenteTabla {
            public string Nombre { get; set; }
            public string Paterno { get; set; }
            public string Materno { get; set; }
        }
    }
}
