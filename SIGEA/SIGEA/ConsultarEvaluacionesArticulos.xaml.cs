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
    /// Lógica de interacción para ConsultarEvaluacionesArticulos.xaml
    /// </summary>
    public partial class ConsultarEvaluacionesArticulos : Window {
        private bool mostrarPanelPrincipal = true;
        private ObservableCollection<ArticuloTabla> articulosList = new ObservableCollection<ArticuloTabla>();
        public ObservableCollection<ArticuloTabla> ArticulosList {
            get => articulosList;
        }

        /// <summary>
        /// Crea una nueva instancia.
        /// </summary>
        public ConsultarEvaluacionesArticulos() {
            InitializeComponent();
            DataContext = this;
            CargarArticulos();
        }

        /// <summary>
        /// Muestra el panel principal al cerrarse.
        /// </summary>
        /// <param name="e">Evento</param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e) {
            if (mostrarPanelPrincipal) {
                new PanelLiderEvento().Show();
            }
        }

        /// <summary>
        /// Carga los artículos y los coloca en la tabla.
        /// </summary>
        private void CargarArticulos() {
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    var articulos = sigeaBD.Articulo.Where(articulo => articulo.Track.id_evento == Sesion.Evento.id_evento);
                    foreach (Articulo articulo in articulos) {
                        articulosList.Add(new ArticuloTabla {
                            Articulo = articulo,
                            Titulo = articulo.titulo,
                            Track = articulo.Track,
                            Estado = articulo.estado
                        });
                    }
                }
            } catch (Exception) {
                MessageBox.Show("Error al establecer una conexión.");
                Close();
            }
        }

        /// <summary>
        /// Muestra una ventana de consultar evaluaciones artículo de acuerdo con
        /// el artículo seleccionado de la tabla.
        /// </summary>
        /// <param name="sender">Tabla</param>
        /// <param name="e">Evento</param>
        private void ArticulosListView_SelectionChanged(object sender, RoutedEventArgs e) {
            new ConsultarEvaluacionesArticulo(((ArticuloTabla) articulosListView.SelectedItem).Articulo).Show();
            mostrarPanelPrincipal = false;
            Close();
        }

        /// <summary>
        /// Representa un artículo en una tabla.
        /// </summary>
        public struct ArticuloTabla {
            public Articulo Articulo;
            public string Titulo { get; set; }
            public Track Track { get; set; }
            public string Estado { get; set; }
            public string Autor { get; set; }
        }
    }
}
