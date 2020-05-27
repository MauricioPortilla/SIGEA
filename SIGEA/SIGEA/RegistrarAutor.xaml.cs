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
        /// <summary>
        /// Crea una instancia.
        /// </summary>
        public RegistrarAutor() {
            InitializeComponent();
        }

        /// <summary>
        /// Muestra el panel principal al cerrarse.
        /// </summary>
        /// <param name="e">Evento</param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e) {
            new PanelLiderComite().Show();
        }

        /// <summary>
        /// Verifica que los campos estén completos.
        /// </summary>
        /// <returns>true si están completos; false si no</returns>
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

        /// <summary>
        /// Verifica que los campos tengan datos válidos.
        /// </summary>
        /// <returns>true si tienen datos válidos; false si no</returns>
        private bool ValidarDatos() {
            return Regex.IsMatch(nombreTextBox.Text, Herramientas.REGEX_SOLO_LETRAS) &&
                Regex.IsMatch(paternoTextBox.Text, Herramientas.REGEX_SOLO_LETRAS) &&
                Regex.IsMatch(maternoTextBox.Text, Herramientas.REGEX_SOLO_LETRAS) &&
                Regex.IsMatch(telefonoTextBox.Text, Herramientas.REGEX_SOLO_NUMEROS) &&
                Regex.IsMatch(correoTextBox.Text, Herramientas.REGEX_CORREO) &&
                Regex.IsMatch(adscripcionNombreDependenciaTextBox.Text, Herramientas.REGEX_SOLO_LETRAS) &&
                Regex.IsMatch(adscripcionTelefonoTextBox.Text, Herramientas.REGEX_SOLO_NUMEROS);
        }

        /// <summary>
        /// Verifica que los campos estén completos, que tengan datos válidos, y
        /// registra al autor con su adscripción en la base de datos.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void RegistrarButton_Click(object sender, RoutedEventArgs e) {
            if (!VerificarCampos()) {
                MessageBox.Show("Faltan campos por completar.");
                return;
            } else if (!ValidarDatos()) {
                MessageBox.Show("Debes introducir datos válidos.");
                return;
            }
            try {
                if (new Adscripcion {
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
                    }.Registrar()
                ) {
                    MessageBox.Show("Autor registrado.");
                    Close();
                    return;
                }
                MessageBox.Show("Error al registrar el autor.");
            } catch (DbUpdateException dbUpdateException) {
                MessageBox.Show("Error al registrar el autor.");
                Console.WriteLine("DbUpdateException@RegistrarAutor->RegistrarButton_Click() -> " + dbUpdateException.Message);
            } catch (EntityException entityException) {
                MessageBox.Show("Error al registrar el autor.");
                Console.WriteLine("EntityException@RegistrarAutor->RegistrarButton_Click() -> " + entityException.Message);
            } catch (Exception exception) {
                MessageBox.Show("Error al registrar el autor.");
                Console.WriteLine("Exception@RegistrarAutor->RegistrarButton_Click() -> " + exception.Message);
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
