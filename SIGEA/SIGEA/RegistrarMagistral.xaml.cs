using SIGEABD;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace SIGEA {

    public partial class RegistrarMagistral : Window {

        String nombreActividad;

        /// <summary>
        /// Crea una instancia
        /// </summary>
        /// <param name="nombreActividad">Nombre de la actividad</param>
        public RegistrarMagistral(String nombreActividad) {
            this.nombreActividad = nombreActividad;
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
        /// Metodo que cierra la ventana
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void CancelarButton_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        /// <summary>
        /// Metodo que registra al magistral
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void RegistrarButton_Click(object sender, RoutedEventArgs e) {
            if(VerificarCampos() && VerificarDatos() && VerificarExistencia()) {
                try {
                    using(SigeaBD sigeaBD = new SigeaBD()) {
                        var actividadObtenida = sigeaBD.Actividad.AsNoTracking().FirstOrDefault(
                            actividad => actividad.nombre == this.nombreActividad &&
                            actividad.id_evento == Sesion.Evento.id_evento);
                        if(new Magistral {
                            nombre = nombreTextBox.Text,
                            paterno = paternoTextBox.Text,
                            materno = maternoTextBox.Text,
                            correo = correoTextBox.Text,
                            telefono = telefonoTextBox.Text,
                            lugarOrigen = lugarTextBox.Text,
                            id_actividad = actividadObtenida.id_actividad
                        }.Registrar()) {
                            MessageBox.Show("Magistral registrado con éxito");
                            Close();
                        } else {
                            MessageBox.Show("No se registró el magistral");
                        }
                    }
                } catch(Exception) {
                    MessageBox.Show("Lo sentimos inténtelo más tarde");
                }
            }
        }

        /// <summary>
        /// Metodo que verifica que ningun campo este vacio
        /// </summary>
        /// <returns>true si están completos; false si no</returns>
        public Boolean VerificarCampos() {
            if(!string.IsNullOrWhiteSpace(nombreTextBox.Text) &&
                !string.IsNullOrWhiteSpace(paternoTextBox.Text) &&
                !string.IsNullOrWhiteSpace(correoTextBox.Text) &&
                !string.IsNullOrWhiteSpace(telefonoTextBox.Text) &&
                !string.IsNullOrWhiteSpace(lugarTextBox.Text)) {
                return true;
            } else {
                MessageBox.Show("Por favor llenar todos los campos");
                return false;
            }
        }

        /// <summary>
        /// Metodo que busca caracteres raros en los datos introducidos
        /// </summary>
        /// <returns>true si son datos válidos; false si no</returns>
        private bool VerificarDatos() {
            if(Regex.IsMatch(nombreTextBox.Text, Herramientas.REGEX_SOLO_LETRAS) &&
                Regex.IsMatch(paternoTextBox.Text, Herramientas.REGEX_SOLO_LETRAS) &&
                Regex.IsMatch(correoTextBox.Text, Herramientas.REGEX_CORREO) &&
                Regex.IsMatch(telefonoTextBox.Text, Herramientas.REGEX_SOLO_NUMEROS) &&
                Regex.IsMatch(lugarTextBox.Text, Herramientas.REGEX_SOLO_LETRAS)) {
                return true;
            } else {
                MessageBox.Show("Los datos son incorrectos verifíquelos");
                return false;
            }
        }

        /// <summary>
        /// Verifica si existe un magistral con el nombre y correo ingresados.
        /// </summary>
        /// <returns>true si existe; false si no</returns>
        public bool VerificarExistencia() {
            try {
                using(SigeaBD sigeaBD = new SigeaBD()) {
                    var magistralOptenido = sigeaBD.Magistral.AsNoTracking().FirstOrDefault(
                        magistral => magistral.nombre == nombreTextBox.Text &&
                        magistral.correo == correoTextBox.Text &&
                        magistral.Actividad.Evento.id_evento == Sesion.Evento.id_evento
                    );
                    if(magistralOptenido == null) {
                        return true;
                    } else {
                        MessageBox.Show("El magistral ya existe en el evento");
                        return false;
                    }
                }
            } catch(Exception) {
                MessageBox.Show("Lo sentimos inténtelo más tarde");
                return false;
            }
        }
    }
}
