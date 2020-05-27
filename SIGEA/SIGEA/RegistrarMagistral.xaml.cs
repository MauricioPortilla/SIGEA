using SIGEABD;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace SIGEA {

    public partial class RegistrarMagistral : Window {

        String nombreActividad;

        /// <summary>Crea una instancia</summary>
        /// <param name="nombreActividad">Nombre de la actividad</param>
        public RegistrarMagistral (String nombreActividad) {

            this.nombreActividad = nombreActividad;
            InitializeComponent();
        }

        /// <summary>
        /// Metodo que cierra la ventana
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelarButton_Click (object sender, RoutedEventArgs e) {

            PanelLiderEvento ventanaActividades = new PanelLiderEvento();
            ventanaActividades.Show();
            this.Close();
        }

        /// <summary>
        /// Metodo que registra al magistral
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RegistrarButton_Click (object sender, RoutedEventArgs e) {

            if (VerificarCampos() && VerificarDatos() && VerificarExistencia()) {

                try {

                    using (SigeaBD sigeaBD = new SigeaBD()) {

                        var actividadObtenida = sigeaBD.Actividad.ToList().Find(
                            actividad => actividad.nombre == this.nombreActividad &&
                            actividad.id_evento == Sesion.Evento.id_evento);

                        if (new Magistral {

                            nombre = nombreTextBox.Text,
                            paterno = paternoTextBox.Text,
                            materno = maternoTextBox.Text,
                            correo = correoTextBox.Text,
                            telefono = telefonoTextBox.Text,
                            lugarOrigen = lugarTextBox.Text,
                            id_actividad = actividadObtenida.id_actividad
                        }.Registrar()
                        ) {

                            MessageBox.Show("Magistral registrado con exitó");
                            new PanelLiderEvento().Show();
                            this.Close();

                        } else {

                            MessageBox.Show("No se registro el magistral");
                        }
                    }

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

        public bool VerificarExistencia () {

            try {

                using (SigeaBD sigeaBD = new SigeaBD()) {

                    var magistralOptenido = sigeaBD.Magistral.ToList().Find(
                        magistral => magistral.nombre == nombreTextBox.Text &&
                        magistral.correo == correoTextBox.Text);

                    if (magistralOptenido == null) {

                        return true;

                    } else {

                        MessageBox.Show("El magistral ya existe");
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
