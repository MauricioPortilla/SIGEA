using SIGEABD;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
    /// Lógica de interacción para RegistrarTarea.xaml
    /// </summary>
    public partial class RegistrarTarea : Window {

        /// <summary>
        /// Crea una instancia.
        /// </summary>
        public RegistrarTarea() {
            InitializeComponent();
            CargarActividades();
        }

        /// <summary>
        /// Carga las actividades de la base de datos y las muestra en el combo box.
        /// </summary>
        private void CargarActividades() {
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    var actividades = sigeaBD.Actividad.AsNoTracking().Where(
                        actividad => actividad.id_evento == Sesion.Evento.id_evento
                    );
                    foreach (var actividad in actividades) {
                        actividadComboBox.Items.Add(actividad);
                    }
                }
            } catch (Exception) {
                MessageBox.Show("Error al establecer una conexión.");
                Close();
            }
        }

        /// <summary>
        /// Muestra el panel del líder de comité al cerrarse.
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
            return !string.IsNullOrWhiteSpace(tituloTextBox.Text) &&
                !string.IsNullOrWhiteSpace(descripcionTextBox.Text) &&
                !string.IsNullOrWhiteSpace(asignadoATextBox.Text);
        }

        /// <summary>
        /// Registra la tarea.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void RegistrarButton_Click(object sender, RoutedEventArgs e) {
            if (VerificarCampos()) {
                var actividadSeleccionada = new Collection<Actividad>();
                if (actividadComboBox.SelectedIndex != -1) {
                    actividadSeleccionada.Add(actividadComboBox.SelectedItem as Actividad);
                }
                Tarea tarea = new Tarea {
                    titulo = tituloTextBox.Text,
                    descripcion = descripcionTextBox.Text,
                    asignadoA = asignadoATextBox.Text,
                    id_comite = Sesion.Comite.id_comite,
                    Actividad = actividadSeleccionada
                };
                try {
                    if (tarea.Registrar()) {
                        MessageBox.Show("Tarea registrada con éxito.");
                        Close();
                        return;
                    }
                    MessageBox.Show("Error al establecer una conexión.");
                } catch (Exception) {
                    MessageBox.Show("Error al establecer una conexión.");
                }
            } else {
                MessageBox.Show("Faltan campos por completar.");
            }
        }

        /// <summary>
        /// Cierra la ventana.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void CancelarButton_Click(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}
