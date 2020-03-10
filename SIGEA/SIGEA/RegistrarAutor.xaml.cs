using System;
using System.Collections.Generic;
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
    /// Lógica de interacción para RegistrarAutor.xaml
    /// </summary>
    public partial class RegistrarAutor : Window {
        public RegistrarAutor() {
            InitializeComponent();
        }

        private bool VerificarCampos() {
            return !string.IsNullOrWhiteSpace(nombreTextBox.Text) &&
                !string.IsNullOrWhiteSpace(paternoTextBox.Text) &&
                !string.IsNullOrWhiteSpace(telefonoTextBox.Text) &&
                !string.IsNullOrWhiteSpace(correoTextBox.Text) &&
                !string.IsNullOrWhiteSpace(adscripcionNombreDependenciaTextBox.Text) &&
                !string.IsNullOrWhiteSpace(adscripcionDireccionTextBox.Text) &&
                !string.IsNullOrWhiteSpace(adscripcionTelefonoTextBox.Text) &&
                !string.IsNullOrWhiteSpace(adscripcionPuestoTextBox.Text);
        }

        private bool VerificarDatos() {
            return Regex.IsMatch(nombreTextBox.Text, Herramientas.REGEX_SOLO_LETRAS) &&
                Regex.IsMatch(paternoTextBox.Text, Herramientas.REGEX_SOLO_LETRAS) &&
                Regex.IsMatch(maternoTextBox.Text, Herramientas.REGEX_SOLO_LETRAS) &&
                Regex.IsMatch(telefonoTextBox.Text, Herramientas.REGEX_SOLO_NUMEROS) &&
                Regex.IsMatch(correoTextBox.Text, Herramientas.REGEX_CORREO) &&
                Regex.IsMatch(adscripcionNombreDependenciaTextBox.Text, Herramientas.REGEX_SOLO_LETRAS) &&
                Regex.IsMatch(adscripcionTelefonoTextBox.Text, Herramientas.REGEX_SOLO_NUMEROS);
        }

        private void RegistrarButton_Click(object sender, RoutedEventArgs e) {
            if (!VerificarCampos()) {
                MessageBox.Show("Faltan campos por completar.");
                return;
            } else if (!VerificarDatos()) {
                MessageBox.Show("Debes introducir datos válidos.");
                return;
            }
            using (SigeaBD sigeaBD = new SigeaBD()) {
                sigeaBD.Autor.Add(new Autor {
                    nombre = nombreTextBox.Text,
                    paterno = paternoTextBox.Text,
                    materno = string.IsNullOrWhiteSpace(maternoTextBox.Text) ? DBNull.Value.ToString() : maternoTextBox.Text,
                    correo = correoTextBox.Text,
                    telefono = telefonoTextBox.Text
                });
                //sigeaBD.Adscripcion.Add(new Adscripcion {
                    
                //})
            }
        }

        private void CancelarButton_Click(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}
