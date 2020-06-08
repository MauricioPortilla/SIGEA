using SIGEABD;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace SIGEA {

    /// <summary>
    /// Lógica de interacción para ModificarTarea.xaml
    /// </summary>
    public partial class ModificarTarea : Window {
        private readonly Tarea tarea;

        public ModificarTarea(Tarea tarea) {
            InitializeComponent();
            this.tarea = tarea;
            tituloTextBox.Text = tarea.titulo;
            descripcionTextBox.Text = tarea.descripcion;
        }

        /// <summary>
        /// Muestra el panel principal al cerrarse.
        /// </summary>
        /// <param name="e">Evento</param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e) {
            new PanelLiderComite().Show();
        }

        /// <summary>
        /// Metodo que vuelve a la ventana Tareas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelarButton_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        /// <summary>
        /// Guarda la tarea.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void GuardarButton_Click(object sender, RoutedEventArgs e) {
            if (VerificarCampos() && ValidarDatos() && VerificarExistencia()) {
                try {
                    using(SigeaBD sigeaBD = new SigeaBD()) {
                        var tareaSeleccionada = sigeaBD.Tarea.Where(
                            tarea => tarea.titulo == this.tarea.titulo
                        ).FirstOrDefault();
                        tareaSeleccionada.titulo = tituloTextBox.Text;
                        tareaSeleccionada.descripcion = descripcionTextBox.Text;
                        if(sigeaBD.SaveChanges() != 0) {
                            MessageBox.Show("Modificación de la tarea con éxito");
                            Close();
                        } else {
                            MessageBox.Show("No se guardó la modificación");
                        }
                    }
                } catch(Exception) {
                    MessageBox.Show("Lo sentimos inténtelo más tarde");
                }
            }
        }

        /// <summary>
        /// Metodo que verifica que los campos no esten vacios
        /// </summary>
        /// <returns>true si están completos; false si no</returns>
        public Boolean VerificarCampos() {

            if(!string.IsNullOrWhiteSpace(tituloTextBox.Text) &&
                !string.IsNullOrWhiteSpace(descripcionTextBox.Text)) {
                return true;
            } else {
                MessageBox.Show("Por favor llenar todos los campos");
                return false;
            }
        }

        /// <summary>
        /// Metodo que valida los datos introducidos
        /// </summary>
        /// <returns>true si son válidos; false si no</returns>
        public Boolean ValidarDatos() {
            if(Regex.IsMatch(tituloTextBox.Text, Herramientas.REGEX_SOLO_LETRAS)) {
                return true;
            } else {
                MessageBox.Show("Los datos proporcionados son incorrectos");
                return false;
            }
        }

        /// <summary>
        /// Verifica si existe una tarea con el título ingresado.
        /// </summary>
        /// <returns>true si no existe; false si existe</returns>
        public Boolean VerificarExistencia() {
            try {
                using(SigeaBD sigeaBD = new SigeaBD()) {
                    var tareaExistente = sigeaBD.Tarea.ToList().Find(
                        tarea => tarea.titulo == this.tarea.titulo && tarea.id_tarea != this.tarea.id_tarea
                    );
                    if(tareaExistente == null) {
                        return true;
                    } else {
                        MessageBox.Show("Modificación no valida, tarea ya existente");
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
