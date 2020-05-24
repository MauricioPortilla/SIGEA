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

    /// <summary>
    /// Lógica de interacción para ModificarTarea.xaml
    /// </summary>
    public partial class ModificarTarea : Window {

        public ModificarTarea () {

            InitializeComponent();
            tituloTextBox.Text = Sesion.Tarea.titulo;
            descripcionTextBox.Text = Sesion.Tarea.descripcion;
        }

        /// <summary>
        /// Metodo que vuelve a la ventana Tareas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelarButton_Click (object sender, RoutedEventArgs e) {

            Sesion.Tarea = null;
            new Tareas().Show();
            this.Close();
        }

        private void GuardarButton_Click (object sender, RoutedEventArgs e) {

            if (VerificarCampos() && ValidarDatos() && VerificarExistencia()) {

                try {

                    using (SigeaBD sigeaBD = new SigeaBD()) {

                        var tareaSeleccionada = sigeaBD.Tarea.ToList().Find(
                            tarea => tarea.titulo == Sesion.Tarea.titulo);

                        tareaSeleccionada.titulo = tituloTextBox.Text;
                        tareaSeleccionada.descripcion = descripcionTextBox.Text;

                        if (sigeaBD.SaveChanges() != 0) {

                            MessageBox.Show("Modificación de la tarea con éxito");
                            Sesion.Tarea = null;
                            new Tareas().Show();
                            this.Close();

                        } else {

                            MessageBox.Show("No se guardo el cambio");
                        }
                    }

                } catch (Exception ex) {

                    MessageBox.Show("Lo sentimos inténtelo más tarde");
                }
            }
        }

        /// <summary>
        /// Metodo que vverifica que los campos no esten vacios
        /// </summary>
        public Boolean VerificarCampos () {

            if (!string.IsNullOrWhiteSpace(tituloTextBox.Text) &&
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
        /// <returns></returns>
        public Boolean ValidarDatos () {

            if (Regex.IsMatch(tituloTextBox.Text, Herramientas.REGEX_SOLO_LETRAS) &&
                Regex.IsMatch(descripcionTextBox.Text, Herramientas.REGEX_SOLO_LETRAS)) {

                return true;

            } else {

                MessageBox.Show("Los datos proporcionados son incorrectos");
                return false;
            }
        }

        public Boolean VerificarExistencia () {

            try {

                using (SigeaBD sigeaBD = new SigeaBD()) {

                    var tareExistente = sigeaBD.Tarea.ToList().Find(
                        tarea => tarea.titulo == Sesion.Tarea.titulo);

                    if (tareExistente == null) {

                        return true;

                    } else {

                        MessageBox.Show("Modificación no valida, tarea ya existente");
                        return false;
                    }
                }

            } catch (Exception e) {

                MessageBox.Show("Lo sentimos inténtelo más tarde");
                return false;
            }
        }
    }
}
