using SIGEABD;
using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace SIGEA {

    public partial class RegistrarComite : Window {

        public RegistrarComite () {
            InitializeComponent();
            CargarComboBox();
        }

        /// <summary>
        /// Metodo que cierra la ventana
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelarButton_Click (object sender, RoutedEventArgs e) {
            new MenuPrincipal().Show();
            this.Close();
        }

        /// <summary>
        /// Metodo que registra el comite en el sistema
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RegistrarButton_Click (object sender, RoutedEventArgs e) {
            if (VerificarCampos() && VerificarDatos() && VerificarExistencia()) {
                try {
                    var organizador = (Organizador) organizadorComboBox.SelectedItem;
                    using (SigeaBD sigeaBD = new SigeaBD()) {
                        if (new Comite {
                            nombre = nombreTextBox.Text,
                            responsabilidades = responsabilidadesTextBox.Text,
                            id_evento = Sesion.Evento.id_evento,
                            id_organizador = organizador.id_organizador
                        }.Registrar()) {
                            MessageBox.Show("El Comite se registro correctamente");
                        } else {
                            MessageBox.Show("El Comite no se pudo registrar");
                        }
                    }
                } catch (Exception exception) {
                    MessageBox.Show("Error al registrar el comite.");
                    Console.WriteLine("Exception@RegistrarButton_Click -> " + exception.Message);
                }
            }
        }

        /// <summary>
        /// Metodo que carga el combo con los organizadores
        /// </summary>
        private void CargarComboBox () {
            using (SigeaBD sigeaBD = new SigeaBD()) {
                try {
                    foreach (Organizador organizador in sigeaBD.Organizador.ToList()) {
                        organizadorComboBox.Items.Add(organizador);
                    }

                } catch (EntityException entityException) {
                    MessageBox.Show("Error al cargar los eventos.");
                    Console.WriteLine("EntityException@RegistrarArticulo->CargarEventos() -> " + entityException.Message);
                }
            }
        }

        /// <summary>
        /// Metodo que verifica que ningun campo este vacio
        /// </summary>
        /// <returns></returns>
        public Boolean VerificarCampos () {
            if (!string.IsNullOrWhiteSpace(nombreTextBox.Text) &&
                !string.IsNullOrWhiteSpace(responsabilidadesTextBox.Text)) {
                if (organizadorComboBox.SelectedItem != null ) {
                    return true;
                } else {
                    MessageBox.Show("Debe seleccionar al menos una actividad.");
                    return false;
                }
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
                Regex.IsMatch(responsabilidadesTextBox.Text, Herramientas.REGEX_SOLO_LETRAS)) {
                return true;
            } else {
                MessageBox.Show("Por favor revise los datos ingresados");
                return false;
            }
        }

        /// <summary>
        /// Metodo que verifica la existencia del comite
        /// </summary>
        /// <returns></returns>
        public Boolean VerificarExistencia () {
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    var comiteOptenido = sigeaBD.Comite.ToList().Find(
                        comite => comite.nombre == nombreTextBox.Text);
                    if (comiteOptenido == null) {
                        return true;
                    } else {
                        MessageBox.Show("Ya esta registrado el comté");
                        return false;
                    }
                }
            } catch (Exception e) {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}
