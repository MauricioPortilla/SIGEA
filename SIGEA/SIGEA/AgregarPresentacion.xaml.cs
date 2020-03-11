using System;
using System.Collections.Generic;
using System.Globalization;
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
using SIGEABD;

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

        private bool VerificarCampos() {
            return !string.IsNullOrWhiteSpace(fechaTextBox.Text) &&
                !string.IsNullOrWhiteSpace(horaInicioTextBox.Text) &&
                !string.IsNullOrWhiteSpace(horaFinTextBox.Text);
        }

        private bool VerificarDatos() {
            return Regex.IsMatch(fechaTextBox.Text, Herramientas.REGEX_FECHA) &&
                Regex.IsMatch(horaInicioTextBox.Text, Herramientas.REGEX_HORA) &&
                Regex.IsMatch(horaFinTextBox.Text, Herramientas.REGEX_HORA);
        }

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
