using SIGEABD;
using System;
using System.Data.Entity.Core;
using System.Linq;
using System.Windows;

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
            CargarOrganizadores();
        }

        /// <summary>
        /// Muestra el panel principal al cerrarse.
        /// </summary>
        /// <param name="e">Evento</param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e) {
            new PanelLiderComite().Show();
        }

        /// <summary>
        /// Carga de la base de datos los organizadores que no tengan un comité asignado.
        /// </summary>
        private void CargarOrganizadores() {
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    var organizadores = sigeaBD.Organizador.Where(
                        organizador => organizador.Comites.FirstOrDefault(comite => comite.id_evento == Sesion.Comite.id_evento) == null &&
                        organizador.id_organizador != Sesion.Organizador.id_organizador
                    );
                    foreach (Organizador organizador in organizadores) {
                        if (organizador.id_organizador == Sesion.Organizador.id_organizador) {
                            continue;
                        }
                        organizadoresComboBox.Items.Add(organizador);
                    }
                }
            } catch (EntityException entityException) {
                Console.WriteLine("EntityException@AsignarOrganizadorComite->CargarOrganizadores() -> " + entityException.Message);
                MessageBox.Show("Error al establecer una conexión.");
            } catch (Exception exception) {
                Console.WriteLine("Exception@AsignarOrganizadorComite->CargarOrganizadores() -> " + exception.Message);
                MessageBox.Show("Error al establecer una conexión.");
            }
        }

        /// <summary>
        /// Verifica que los campos estén completos.
        /// </summary>
        /// <returns>true si están completos; false si no</returns>
        private bool VerificarCampos() {
            return organizadoresComboBox.SelectedIndex != -1;
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
            Organizador organizadorSeleccionado = organizadoresComboBox.SelectedItem as Organizador;
            try {
                if (Sesion.Comite.AsignarOrganizador(organizadorSeleccionado)) {
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
