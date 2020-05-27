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
    /// Lógica de interacción para RegistrarPagoArticulo.xaml
    /// </summary>
    public partial class RegistrarPagoArticulo : Window {
        private readonly Articulo articulo;

        /// <summary>
        /// Crea una instancia.
        /// </summary>
        public RegistrarPagoArticulo(Articulo articulo) {
            InitializeComponent();
            this.articulo = articulo;
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
            return !string.IsNullOrWhiteSpace(cantidadTextBox.Text);
        }

        /// <summary>
        /// Valida que los campos tengan datos válidos.
        /// </summary>
        /// <returns>true si son válidos; false si no</returns>
        private bool ValidarCampos() {
            return Regex.IsMatch(cantidadTextBox.Text, Herramientas.REGEX_SOLO_ENTEROS_Y_FLOTANTES);
        }

        /// <summary>
        /// Verifica si existe un pago relacionado con el artículo.
        /// </summary>
        /// <returns>true si existe el pago; false si no</returns>
        private bool VerificarExistenciaPago() {
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    var pagoBusqueda = sigeaBD.Pago.First(pago => pago.id_articulo == articulo.id_articulo);
                    return pagoBusqueda != null;
                }
            } catch (Exception) {
                MessageBox.Show("Error al establecer una conexión.");
                return true;
            }
        }

        /// <summary>
        /// Registra el pago.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void RegistrarButton_Click(object sender, RoutedEventArgs e) {
            if (!VerificarCampos()) {
                MessageBox.Show("Faltan campos por completar.");
                return;
            } else if (!ValidarCampos()) {
                MessageBox.Show("Debes introducir datos válidos.");
                return;
            } else if (VerificarExistenciaPago()) {
                MessageBox.Show("Ya existe un pago relacionado a este artículo.");
                return;
            }
            try {
                Pago pago = new Pago {
                    cantidad = float.Parse(cantidadTextBox.Text),
                    fecha = DateTime.Now,
                    id_articulo = articulo.id_articulo
                };
                if (pago.Registrar()) {
                    MessageBox.Show("Pago registrado con éxito.");
                    Close();
                    return;
                }
                MessageBox.Show("Error al establecer una conexión.");
            } catch (Exception) {
                MessageBox.Show("Error al establecer una conexión.");
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
