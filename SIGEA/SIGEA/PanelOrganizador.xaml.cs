using SIGEABD;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using static SIGEA.PanelLiderComite;

namespace SIGEA {
    /// <summary>
    /// Lógica de interacción para PanelOrganizador.xaml
    /// </summary>
    public partial class PanelOrganizador : Window {
        private bool mostrarMenuPrincipal = true;
        public ObservableCollection<TareaTabla> TareasLista { get; } = new ObservableCollection<TareaTabla>();

        /// <summary>
        /// Crea una instancia.
        /// </summary>
        public PanelOrganizador() {
            InitializeComponent();
            DataContext = this;
            CargarTablaTareas();
        }

        /// <summary>
        /// Muestra el menú principal al cerrarse.
        /// </summary>
        /// <param name="e">Evento</param>
        protected override void OnClosing(CancelEventArgs e) {
            if (mostrarMenuPrincipal) {
                new MenuPrincipal().Show();
            }
        }

        /// <summary>
        /// Carga las tareas del comité en la tabla.
        /// </summary>
        private void CargarTablaTareas() {
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    var listaTareas = sigeaBD.Tarea.AsNoTracking().Where(
                        tarea => tarea.id_comite == Sesion.Comite.id_comite
                    );
                    foreach (Tarea tarea in listaTareas) {
                        TareasLista.Add(new TareaTabla {
                            Tarea = tarea,
                            Titulo = tarea.titulo,
                            Descripcion = tarea.descripcion,
                            AsignadoA = tarea.asignadoA,
                            Actividad = tarea.Actividad.FirstOrDefault()
                        });
                    }
                }
            } catch (Exception) {
                MessageBox.Show("Error al establecer una conexión.");
            }
        }

        /// <summary>
        /// Abre una ventana para registrar un gasto.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void RegistrarGastoButton_Click(object sender, RoutedEventArgs e) {
            mostrarMenuPrincipal = false;
            new RegistrarGasto().Show();
            Close();
        }

        /// <summary>
        /// Abre una ventana para registrar el pago de un asistente.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void RegistrarPagoAsistenteButton_Click(object sender, RoutedEventArgs e) {
            mostrarMenuPrincipal = false;
            new RegistrarPagoAsistente().Show();
            Close();
        }

        /// <summary>
        /// Cierra la ventana.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void RegresarButton_Click(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}
