using SIGEABD;
using System;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Text.RegularExpressions;
using System.Windows;

namespace SIGEA {

    public partial class RegistrarMagistral : Window {

        private Actividad actividad;

        /// <summary>
        /// Se modifivo el constructor de la pantalla debido
        /// a que se requiere una actividad para registrar
        /// al magistral
        /// </summary>
        /// <param name="actividadSeleccionada"></param>
        public RegistrarMagistral (Actividad actividadSeleccionada) {
            this.actividad = actividadSeleccionada;
            InitializeComponent();
        }

        /// <summary>
        /// Metodo que cierra la ventana
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelarButton_Click (object sender, RoutedEventArgs e) {
            MenuPrincipal menu = new MenuPrincipal();
            menu.Show();
            this.Close();
        }

        /// <summary>
        /// Metodo que registra al magistral
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RegistrarButton_Click (object sender, RoutedEventArgs e) {
            if (VerificarCampos() && VerificarDatos()) {
                try {
                    using (SigeaBD sigeaBD = new SigeaBD()) {
                        if (new Magistral {
                            nombre = nombreTextBox.Text,
                            paterno = paternoTextBox.Text,
                            materno = maternoTextBox.Text,
                            correo = correoTextBox.Text,
                            telefono = telefonoTextBox.Text,
                            lugarOrigen = lugarTextBox.Text,
                            Actividad = this.actividad
                        }.Registrar()) {
                            MessageBox.Show("El MAgistral se registro correctamente");
                        } else {
                            MessageBox.Show("El Magistral no se pudo registrar");
                        }
                    }
                } catch (DbUpdateException dbUpdateException) {
                    MessageBox.Show("Error al registrar el magistral.");
                    Console.WriteLine("DbUpdateException@RegistrarButton_Click -> " + dbUpdateException.Message);
                } catch (EntityException entityException) {
                    MessageBox.Show("Error al registrar el magistral.");
                    Console.WriteLine("EntityException@RegistrarButton_Click -> " + entityException.Message);
                } catch (Exception exception) {
                    MessageBox.Show("Error al registrar el magistral.");
                    Console.WriteLine("Exception@RegistrarButton_Click -> " + exception.Message);
                }
            }
        }

        /// <summary>
        /// Metodo que verifica que ningun campo este vacio
        /// </summary>
        /// <returns></returns>
        public Boolean VerificarCampos () {
            if (!string.IsNullOrWhiteSpace(nombreTextBox.Text) &&
                !string.IsNullOrWhiteSpace(paternoTextBox.Text) &&
                !string.IsNullOrWhiteSpace(maternoTextBox.Text) &&
                !string.IsNullOrWhiteSpace(correoTextBox.Text) &&
                !string.IsNullOrWhiteSpace(telefonoTextBox.Text) &&
                !string.IsNullOrWhiteSpace(lugarTextBox.Text)) {
                return true;
            } else {
                MessageBox.Show("Debe llenar todos los campos.");
                return false;
            }
        }

        /// <summary>
        /// Metodo que busca caracteres raros en los datos introducidos
        /// </summary>
        /// <returns></returns>
        private bool VerificarDatos () {
            if (Regex.IsMatch(nombreTextBox.Text, Herramientas.REGEX_SOLO_LETRAS) &&
                Regex.IsMatch(paternoTextBox.Text, Herramientas.REGEX_SOLO_LETRAS) &&
                Regex.IsMatch(maternoTextBox.Text, Herramientas.REGEX_SOLO_LETRAS) &&
                Regex.IsMatch(correoTextBox.Text, Herramientas.REGEX_CORREO) &&
                Regex.IsMatch(telefonoTextBox.Text, Herramientas.REGEX_SOLO_NUMEROS) &&
                Regex.IsMatch(lugarTextBox.Text, Herramientas.REGEX_SOLO_LETRAS)) {
                return true;
            } else {
                MessageBox.Show("Por favor revise los datos ingresados");
                return false;
            }
        }
    }
}
