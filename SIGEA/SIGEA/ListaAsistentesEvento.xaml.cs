using SIGEABD;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace SIGEA {

    /// <summary>
    /// Lógica de interacción para ListaAsistentes.xaml
    /// </summary>
    public partial class ListaAsistentesEvento : Window {

        public ObservableCollection<AsistenteTabla> AsistentesLista { get; } =
            new ObservableCollection<AsistenteTabla>();

        /// <summary>
        /// Crea una instancia.
        /// </summary>
        public ListaAsistentesEvento() {
            InitializeComponent();
            DataContext = this;
            CargarTabla();
        }

        /// <summary>
        /// Cierra la ventana
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void RegresarButton_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        /// <summary>
        /// Muestra el panel principal al cerrarse.
        /// </summary>
        /// <param name="e">Evento</param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e) {
            new PanelLiderEvento().Show();
        }

        /// <summary>
        /// cragar la tabla con los asistentes del evento
        /// </summary>
        public void CargarTabla() {
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    var evento = sigeaBD.Evento.Find(Sesion.Evento.id_evento);
                    foreach (var asistente in evento.Asistente) {
                        AsistentesLista.Add(new AsistenteTabla {
                            Nombre = asistente.nombre,
                            Paterno = asistente.paterno,
                            Materno = asistente.materno,
                            Correo = asistente.correo
                        });
                    }
                }
            } catch (Exception) {
                MessageBox.Show("Lo sentimos inténtelo más tarde");
            }
        }

        /// <summary>
        /// Estructura para llenar la tabla con los aistentes
        /// </summary>
        public struct AsistenteTabla {
            public Asistente Asistente;
            public string Nombre { get; set; }
            public string Paterno { get; set; }
            public string Materno { get; set; }
            public string Correo { get; set; }
        }
    }
}