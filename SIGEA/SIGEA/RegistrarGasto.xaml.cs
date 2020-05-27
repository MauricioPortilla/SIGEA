using SIGEABD;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SIGEA {
    /// <summary>
    /// Lógica de interacción para RegistrarGasto.xaml
    /// </summary>
    public partial class RegistrarGasto : Window {

        /// <summary>
        /// Crea una instancia.
        /// </summary>
        public RegistrarGasto() {
            InitializeComponent();
            CargarMagistrales();
        }

        /// <summary>
        /// Muestra el panel principal al cerrarse.
        /// </summary>
        /// <param name="e">Evento</param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e) {
            if (Sesion.Comite != null) {
                new PanelLiderComite().Show();
            } else {
                new PanelLiderEvento().Show();
            }
            // TODO: Agregar para panel de organizador.
        }

        /// <summary>
        /// Carga los magistrales de la base de datos y los ingresa en el combo box.
        /// </summary>
        private void CargarMagistrales() {
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    var magistrales = sigeaBD.Magistral.AsNoTracking().Where(
                        magistral => magistral.Actividad.id_evento == Sesion.Evento.id_evento
                    );
                    foreach (var magistral in magistrales) {
                        magistralComboBox.Items.Add(magistral);
                    }
                }
            } catch (Exception) {
                MessageBox.Show("Error al establecer una conexión.");
                Close();
            }
        }

        /// <summary>
        /// Verifica que los campos estén completos.
        /// </summary>
        /// <returns>true si están completos; false si no</returns>
        private bool VerificarCampos() {
            return !string.IsNullOrWhiteSpace(cantidadTextBox.Text) &&
                fechaDatePicker.SelectedDate.HasValue &&
                !string.IsNullOrWhiteSpace(motivoTextBox.Text);
        }

        /// <summary>
        /// Valida que los campos tengan datos válidos.
        /// </summary>
        /// <returns>true si son válidos; false si no</returns>
        private bool ValidarCampos() {
            return Regex.IsMatch(cantidadTextBox.Text, Herramientas.REGEX_SOLO_ENTEROS_Y_FLOTANTES) &&
                Regex.IsMatch(fechaDatePicker.SelectedDate.Value.ToString("dd/MM/yyyy"), Herramientas.REGEX_FECHA);
        }

        /// <summary>
        /// Registra el gasto.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void RegistrarButton_Click(object sender, RoutedEventArgs e) {
            if (!VerificarCampos()) {
                MessageBox.Show("Faltan campos por completar.");
                return;
            } else if (!ValidarCampos()) {
                MessageBox.Show("Debes introducir datos válidos.");
                return;
            }
            var magistralSeleccionado = new Collection<Magistral>();
            if (magistralComboBox.SelectedIndex != -1) {
                magistralSeleccionado.Add(magistralComboBox.SelectedItem as Magistral);
            }
            try {
                Gasto gasto = new Gasto {
                    cantidad = float.Parse(cantidadTextBox.Text),
                    fecha = fechaDatePicker.SelectedDate.Value,
                    motivo = motivoTextBox.Text,
                    id_evento = Sesion.Evento.id_evento,
                    Magistral = magistralSeleccionado
                };
                if (gasto.Registrar()) {
                    MessageBox.Show("Gasto registrado con éxito.");
                    Close();
                    return;
                }
                MessageBox.Show("Error al establecer una conexión.");
            } catch (Exception) {
                MessageBox.Show("Error al establecer una conexión.");
            }
        }

        /// <summary>
        /// Cierra la ventana actual.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void CancelarButton_Click(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}
