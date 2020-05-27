using SIGEABD;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using static SIGEA.GenerarConstanciasEvento;

namespace SIGEA {
    /// <summary>
    /// Lógica de interacción para ConsultarAsistentesActividad.xaml
    /// </summary>
    public partial class ConsultarAsistentesActividad : Window {
        public ObservableCollection<AsistenteTabla> AsistentesLista { get; } = new ObservableCollection<AsistenteTabla>();
        private readonly Actividad actividad;

        /// <summary>
        /// Crea una instancia.
        /// </summary>
        /// <param name="actividad">Actividad</param>
        public ConsultarAsistentesActividad(Actividad actividad) {
            InitializeComponent();
            DataContext = this;
            this.actividad = actividad;
            CargarAsistentes();
        }

        /// <summary>
        /// Muestra el panel principal al cerrarse.
        /// </summary>
        /// <param name="e">Evento</param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e) {
            new PanelLiderEvento().Show();
        }

        /// <summary>
        /// Carga los asistentes pertenecientes a la actividad.
        /// </summary>
        private void CargarAsistentes() {
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    var asistentes = sigeaBD.Asistente.Where(
                        asistente => asistente.Actividad.First(
                            actividad => actividad.id_actividad == this.actividad.id_actividad
                        ) != null
                    );
                    foreach (var asistente in asistentes) {
                        AsistentesLista.Add(new AsistenteTabla {
                            Nombre = asistente.nombre,
                            Paterno = asistente.paterno,
                            Materno = asistente.materno ?? ""
                        });
                    }
                }
            } catch (Exception) {
                MessageBox.Show("Error al establecer una conexión.");
                Close();
            }
        }

        /// <summary>
        /// Cierra la ventana actual.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void VolverButton_Click(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}
