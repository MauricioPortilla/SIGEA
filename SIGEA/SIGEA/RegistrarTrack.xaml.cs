using SIGEABD;
using System;
using System.Linq;
using System.Windows;

namespace SIGEA {
    /// <summary>
    /// Lógica de interacción para RegistrarTrack.xaml
    /// </summary>
    public partial class RegistrarTrack : Window {
        /// <summary>
        /// Crea una instancia.
        /// </summary>
        public RegistrarTrack() {
            InitializeComponent();
        }

        /// <summary>
        /// Muestra el panel principal al cerrarse.
        /// </summary>
        /// <param name="e">Evento</param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e) {
            new PanelLiderEvento().Show();
        }

        /// <summary>
        /// Verifica que los campos estén completos.
        /// </summary>
        /// <returns>true si están completos; false si no</returns>
        private bool VerificarCampos() {
            return !string.IsNullOrWhiteSpace(nombreTextBox.Text) &&
                !string.IsNullOrWhiteSpace(descripcionTextBox.Text);
        }

        /// <summary>
        /// Verifica si existe un track con el nombre ingresado.
        /// </summary>
        /// <returns>true si existe; false si no</returns>
        private bool VerificarExistencia() {
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    return sigeaBD.Track.Where(track => track.nombre == nombreTextBox.Text).Count() == 0;
                }
            } catch (Exception) {
                MessageBox.Show("Error al registrar el track.");
                return false;
            }
        }

        /// <summary>
        /// Registra el track.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void RegistrarButton_Click(object sender, RoutedEventArgs e) {
            if (!VerificarCampos()) {
                MessageBox.Show("Faltan campos por completar.");
                return;
            }
            if (!VerificarExistencia()) {
                MessageBox.Show("Ya existe un track registrado con este nombre.");
                return;
            }
            Track track = new Track {
                nombre = nombreTextBox.Text,
                descripcion = descripcionTextBox.Text,
                id_evento = Sesion.Evento.id_evento
            };
            try {
                if (track.Registrar()) {
                    MessageBox.Show("Track registrado con éxito.");
                    Close();
                } else {
                    MessageBox.Show("Error al registrar el track.");
                }
            } catch (Exception) {
                MessageBox.Show("Error al registrar el track.");
            }
        }

        /// <summary>
        /// Cierra la ventana.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void CancelarButton_Click(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}
