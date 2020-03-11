using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
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
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    sigeaBD.Adscripcion.Add(new Adscripcion {
                        nombreDependencia = adscripcionNombreDependenciaTextBox.Text,
                        direccion = adscripcionDireccionTextBox.Text,
                        puesto = adscripcionPuestoTextBox.Text,
                        telefono = telefonoTextBox.Text,
                        Autor = new Collection<Autor>() {
                            new Autor {
                                nombre = nombreTextBox.Text,
                                paterno = paternoTextBox.Text,
                                materno = string.IsNullOrWhiteSpace(maternoTextBox.Text) ? DBNull.Value.ToString() : maternoTextBox.Text,
                                correo = correoTextBox.Text,
                                telefono = telefonoTextBox.Text
                            }
                        }
                    });
                    if (sigeaBD.SaveChanges() != 1) {
                        MessageBox.Show("Error al registrar el autor.");
                        return;
                    }
                    MessageBox.Show("Autor registrado.");
                }
            } catch (DbUpdateException dbUpdateException) {
                MessageBox.Show("Error al registrar el autor.");
                Console.WriteLine("DbUpdateException@RegistrarButton_Click -> " + dbUpdateException.Message);
            } catch (EntityException entityException) {
                MessageBox.Show("Error al registrar el autor.");
                Console.WriteLine("EntityException@RegistrarButton_Click -> " + entityException.Message);
            } catch (Exception exception) {
                MessageBox.Show("Error al registrar el autor.");
                Console.WriteLine("Exception@RegistrarButton_Click -> " + exception.Message);
            }
        }

        private void CancelarButton_Click(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}
