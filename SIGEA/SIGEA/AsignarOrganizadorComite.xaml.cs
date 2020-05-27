using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
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
using SIGEABD;

namespace SIGEA {
    /// <summary>
    /// Lógica de interacción para AsignarOrganizadorComite.xaml
    /// </summary>
    public partial class AsignarOrganizadorComite : Window {
        /// <summary>
        /// Crea una instancia.
        /// </summary>
        public AsignarOrganizadorComite() {
            InitializeComponent();
            CargarComites();
        }

        /// <summary>
        /// Muestra el panel principal al cerrarse.
        /// </summary>
        /// <param name="e">Evento</param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e) {
            new PanelLiderComite().Show();
        }

        /// <summary>
        /// Carga de la base de datos los comités y los muestra en su
        /// respectivo combo box.
        /// </summary>
        private void CargarComites() {
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    var comites = sigeaBD.Comite.Where(
                        comite => comite.Organizador.id_organizador == Sesion.Organizador.id_organizador
                    );
                    foreach (Comite comite in comites) {
                        comitesComboBox.Items.Add(comite);
                    }
                }
            } catch (EntityException entityException) {
                Console.WriteLine("EntityException@AsignarOrganizadorComite->CargarComites() -> " + entityException.Message);
                MessageBox.Show("Error al establecer una conexión.");
            } catch (Exception exception) {
                Console.WriteLine("Exception@AsignarOrganizadorComite->CargarComites() -> " + exception.Message);
                MessageBox.Show("Error al establecer una conexión.");
            }
        }

        /// <summary>
        /// Carga de la base de datos los organizadores que no tengan un comité asignado.
        /// </summary>
        private void CargarOrganizadores() {
            organizadoresComboBox.Items.Clear();
            Comite comiteSeleccionado = comitesComboBox.SelectedItem as Comite;
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    var organizadores = sigeaBD.Organizador.Where(
                        organizador => organizador.Comites.FirstOrDefault(comite => comite.id_evento == comiteSeleccionado.id_evento) == null &&
                        organizador.id_organizador != Sesion.Organizador.id_organizador
                    );
                    foreach (Organizador organizador in organizadores) {
                        if (organizador.id_organizador == Sesion.Organizador.id_organizador) {
                            continue;
                        }
                        organizadoresComboBox.Items.Add(organizador);
                    }
                }
                organizadoresComboBox.IsEnabled = true;
            } catch (EntityException entityException) {
                Console.WriteLine("EntityException@AsignarOrganizadorComite->CargarOrganizadores() -> " + entityException.Message);
                MessageBox.Show("Error al establecer una conexión.");
            } catch (Exception exception) {
                Console.WriteLine("Exception@AsignarOrganizadorComite->CargarOrganizadores() -> " + exception.Message);
                MessageBox.Show("Error al establecer una conexión.");
            }
        }

        /// <summary>
        /// Carga los organizadores al seleccionar un comité.
        /// </summary>
        /// <param name="sender">Combobox</param>
        /// <param name="e">Evento del combobox</param>
        private void ComitesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            CargarOrganizadores();
        }

        /// <summary>
        /// Verifica que los campos estén completos.
        /// </summary>
        /// <returns>true si están completos; false si no</returns>
        private bool VerificarCampos() {
            return comitesComboBox.SelectedIndex != -1 && organizadoresComboBox.SelectedIndex != -1;
        }

        /// <summary>
        /// Asigna el organizador.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void AsignarButton_Click(object sender, RoutedEventArgs e) {
            if (!VerificarCampos()) {
                MessageBox.Show("Faltan campos por completar.");
                return;
            }
            Comite comiteSeleccionado = comitesComboBox.SelectedItem as Comite;
            Organizador organizadorSeleccionado = organizadoresComboBox.SelectedItem as Organizador;
            try {
                if (comiteSeleccionado.AsignarOrganizador(organizadorSeleccionado)) {
                    MessageBox.Show("Se ha asignado el organizador al comité.");
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
        /// <param name="e">Evento del botón</param>
        private void CancelarButton_Click(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}
