using SIGEABD;
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

namespace SIGEA {
    public partial class RegistrarEvento : Window {
        public RegistrarEvento () {
            InitializeComponent();
        }

        private void CancelarButton_Click (object sender, RoutedEventArgs e) {
            MenuPrincipal menuPrincipal = new MenuPrincipal();
            menuPrincipal.Show();
            this.Close();
        }

        private void RegistrarButton_Click (object sender, RoutedEventArgs e) {
            if (VerificarCampos() && VerificarDatos() && VerificarExistencia()) {
                try {
                    using (SigeaBD sigeaBD = new SigeaBD()) {
                        if (new Evento {
                            nombre = nombreTextBox.Text,
                            sede = sedeTextBox.Text,
                            cuota = Double.Parse(cuotaTextBox.Text),
                            fechaInicio = (DateTime) inicioDataPicker.SelectedDate,
                            fechaFin = (DateTime) finDataPicker.SelectedDate
                        }.Registrar()) {
                            MessageBox.Show("El Evento se registro correctamente");
                        } else {
                            MessageBox.Show("El Evento no se pudo registrar");
                        }
                    }
                } catch (Exception exception) {
                    MessageBox.Show("Error al registrar el evento.");
                    Console.WriteLine("Exception@RegistrarButton_Click -> " + exception.Message);
                }
            }
        }

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
