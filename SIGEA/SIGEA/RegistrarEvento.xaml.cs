using SIGEABD;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace SIGEA {

    public partial class RegistrarEvento : Window {

        public RegistrarEvento () {

            InitializeComponent();

        }

        /// <summary>
        /// Metodo de acción que regresa a la ventana menu principal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelarButton_Click (object sender, RoutedEventArgs e) {

            MenuPrincipal menuPrincipal = new MenuPrincipal();
            menuPrincipal.Show();
            this.Close();

        }

        /// <summary>
        /// Metodo de acción que registra el evento en la base de datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RegistrarButton_Click (object sender, RoutedEventArgs e) {

            if (VerificarCampos() && VerificarDatos() && VerificarExistencia()) {

                try {

                    using (SigeaBD sigeaBD = new SigeaBD()) {

                        if (new Evento {

                            nombre = nombreTextBox.Text,
                            sede = sedeTextBox.Text,
                            cuota = Double.Parse(cuotaTextBox.Text),
                            fechaInicio = (DateTime) inicioDataPicker.SelectedDate,
                            fechaFin = (DateTime) finDataPicker.SelectedDate,
                            id_organizador = Sesion.Organizador.id_organizador
                        }.Registrar()
                        ) {

                            MessageBox.Show("Evento registrado con exitó");
                            new MenuPrincipal().Show();
                            this.Close();

                        } else {

                            MessageBox.Show("No se puede registrar el evento");
                        }
                    }

                } catch (Exception exception) {

                    MessageBox.Show("Error al registrar el evento.");
                    Console.WriteLine("Exception@RegistrarButton_Click -> " + exception.Message);
                }
            }
        }

        /// <summary>
        /// Metodo que verifica que ningun campo este vacío
        /// </summary>
        /// <returns>true si ningun campo esta vacio, false si al menos uno esta vacio</returns>
        public Boolean VerificarCampos () {

            if (!string.IsNullOrWhiteSpace(nombreTextBox.Text) &&
                !string.IsNullOrWhiteSpace(sedeTextBox.Text) &&
                !string.IsNullOrWhiteSpace(cuotaTextBox.Text)) {

                if (inicioDataPicker.SelectedDate != null &&
                    finDataPicker.SelectedDate != null) {

                    return true;

                } else {

                    return false;
                }

            } else {

                MessageBox.Show("Debe llenar todos los campos.");
                return false;
            }
        }

        /// <summary>
        /// Metodo que Verficia que no existan caracteres raros
        /// </summary>
        /// <returns>true si todoe s correcto, false si llevan caracteres erroneos</returns>
        private bool VerificarDatos () {

            if (Regex.IsMatch(nombreTextBox.Text, Herramientas.REGEX_SOLO_LETRAS) &&
                Regex.IsMatch(sedeTextBox.Text, Herramientas.REGEX_SOLO_LETRAS) &&
                Regex.IsMatch(cuotaTextBox.Text, Herramientas.REGEX_SOLO_NUMEROS)) {

                return true;

            } else {

                MessageBox.Show("Por favor revise los datos ingresados");
                return false;
            }
        }

        /// <summary>
        /// Verficia que el evento no exista en el sistema
        /// </summary>
        /// <returns></returns>
        public Boolean VerificarExistencia () {

            try {

                using (SigeaBD sigeaBD = new SigeaBD()) {

                    var eventoOptenido =sigeaBD.Evento.ToList().Find(
                        evento => evento.nombre == nombreTextBox.Text &&
                        evento.fechaInicio == inicioDataPicker.SelectedDate &&
                        evento.fechaFin == finDataPicker.SelectedDate);

                    if (eventoOptenido == null) {

                        return true;

                    } else {

                        MessageBox.Show("Ya esta registrado el evento");
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
