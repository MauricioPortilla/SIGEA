using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using static SIGEA.RegistrarActividad;

namespace SIGEA {
    /// <summary>
    /// Lógica de interacción para ModificarPresentacion.xaml
    /// </summary>
    public partial class ModificarPresentacion : Window {

        public PresentacionTabla PresentacionTabla;

        /// <summary>
        /// Crea una instancia.
        /// </summary>
        /// <param name="presentacionTabla">Presentación a modificar</param>
        public ModificarPresentacion(PresentacionTabla presentacionTabla) {
            InitializeComponent();
            this.PresentacionTabla = presentacionTabla;
            fechaTextBox.Text = presentacionTabla.Fecha;
            horaInicioTextBox.Text = presentacionTabla.HoraInicio.ToString();
            horaFinTextBox.Text = presentacionTabla.HoraFin.ToString();
        }

        /// <summary>
        /// Verifica que los campos estén completos.
        /// </summary>
        /// <returns>true si están completos; false si no</returns>
        private bool VerificarCamposCompletos() {
            return !string.IsNullOrWhiteSpace(fechaTextBox.Text) &&
                !string.IsNullOrWhiteSpace(horaInicioTextBox.Text) &&
                !string.IsNullOrWhiteSpace(horaFinTextBox.Text);
        }

        /// <summary>
        /// Verifica que los campos tengan datos válidos.
        /// </summary>
        /// <returns>true si tienen datos válidos; false si no</returns>
        private bool VerificarDatosValidos() {
            return Regex.IsMatch(fechaTextBox.Text, Herramientas.REGEX_FECHA) &&
                Regex.IsMatch(horaInicioTextBox.Text, Herramientas.REGEX_HORA) &&
                Regex.IsMatch(horaFinTextBox.Text, Herramientas.REGEX_HORA);
        }

        /// <summary>
        /// Modifica los datos de la presentación.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void ModificarButtton_Click(object sender, RoutedEventArgs e) {
            if (VerificarCamposCompletos() && VerificarDatosValidos()) {
                if (PresentacionTabla.Presentacion != null) {
                    PresentacionTabla.Presentacion.fechaPresentacion = Convert.ToDateTime(fechaTextBox.Text, new CultureInfo("es-MX"));
                    PresentacionTabla.Presentacion.horaInicio = TimeSpan.Parse(horaInicioTextBox.Text);
                    PresentacionTabla.Presentacion.horaFin = TimeSpan.Parse(horaFinTextBox.Text);
                }
                PresentacionTabla.Fecha = fechaTextBox.Text;
                PresentacionTabla.HoraInicio = TimeSpan.Parse(horaInicioTextBox.Text);
                PresentacionTabla.HoraFin = TimeSpan.Parse(horaFinTextBox.Text);
                Close();
            }
        }
    }
}
