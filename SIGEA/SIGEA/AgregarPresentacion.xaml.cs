using SIGEABD;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;

namespace SIGEA {
    /// <summary>
    /// Lógica de interacción para AgregarPresentacion.xaml
    /// </summary>
    public partial class AgregarPresentacion : Window {

        public Presentacion Presentacion = null;

        /// <summary>
        /// Crea la instancia.
        /// </summary>
        public AgregarPresentacion() {
            InitializeComponent();
        }

        /// <summary>
        /// Verifica que los campos estén completos.
        /// </summary>
        /// <returns>true si están completos; false si no</returns>
        private bool VerificarCampos() {
            return !string.IsNullOrWhiteSpace(fechaTextBox.Text) &&
                !string.IsNullOrWhiteSpace(horaInicioTextBox.Text) &&
                !string.IsNullOrWhiteSpace(horaFinTextBox.Text);
        }

        /// <summary>
        /// Verifica que los campos tengan datos válidos.
        /// </summary>
        /// <returns>true si tienen datos válidos; false si no</returns>
        private bool VerificarDatos() {
            return Regex.IsMatch(fechaTextBox.Text, Herramientas.REGEX_FECHA) &&
                Regex.IsMatch(horaInicioTextBox.Text, Herramientas.REGEX_HORA) &&
                Regex.IsMatch(horaFinTextBox.Text, Herramientas.REGEX_HORA);
        }

        /// <summary>
        /// Verifica campos completos y sus datos y crea una instancia
        /// de Presentacion de acuerdo con los datos ingresados.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento del botón</param>
        private void AñadirButtton_Click(object sender, RoutedEventArgs e) {
            if (!VerificarCampos()) {
                MessageBox.Show("Faltan campos por completar.");
                return;
            } else if (!VerificarDatos()) {
                MessageBox.Show("Debes introducir datos válidos.");
                return;
            }
            Presentacion = new Presentacion {
                fechaPresentacion = Convert.ToDateTime(fechaTextBox.Text, new CultureInfo("es-MX")),
                horaInicio = TimeSpan.Parse(horaInicioTextBox.Text),
                horaFin = TimeSpan.Parse(horaFinTextBox.Text)
            };
            Close();
        }
    }
}
