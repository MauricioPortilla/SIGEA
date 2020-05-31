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
using static SIGEA.ConsultarEvaluacionesArticulos;

namespace SIGEA {
    /// <summary>
    /// Lógica de interacción para PanelRevisor.xaml
    /// </summary>
    public partial class PanelRevisor : Window {
        public ObservableCollection<ArticuloTabla> ArticulosLista { get; } = new ObservableCollection<ArticuloTabla>();

        /// <summary>
        /// Crea una instancia.
        /// </summary>
        public PanelRevisor() {
            InitializeComponent();
            DataContext = this;
            CargarArticulos();
        }

        /// <summary>
        /// Carga los artículos.
        /// </summary>
        private void CargarArticulos() {
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    var articulos = sigeaBD.Articulo.Where(articulo => articulo.Track.id_evento == Sesion.Evento.id_evento);
                    foreach (Articulo articulo in articulos) {
                        ArticulosLista.Add(new ArticuloTabla {
                            Articulo = articulo,
                            Titulo = articulo.titulo,
                            Estado = articulo.estado,
                            Track = articulo.Track
                        });
                    }
                }
            } catch (Exception) {
                MessageBox.Show("Error al establecer una conexión.");
                Close();
            }
        }

        /// <summary>
        /// Abre una ventana de Evaluar artículo para el artículo seleccionado.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void EvaluarButton_Click(object sender, RoutedEventArgs e) {
            if (articulosListView.SelectedIndex != -1) {
                ArticuloTabla articuloSeleccionado = (ArticuloTabla) articulosListView.SelectedItem;
                new EvaluarArticulo(articuloSeleccionado.Articulo.id_articulo).Show();
                Close();
            } else {
                MessageBox.Show("Debes seleccionar un artículo.");
            }
        }

        /// <summary>
        /// Cierra la ventana y vuelve a la de Iniciar sesión.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void cerrarSesionButton_Click(object sender, RoutedEventArgs e) {
            IniciarSesion inicioSesion = new IniciarSesion();
            inicioSesion.Show();
            Sesion.Cuenta = null;
            Sesion.Comite = null;
            Sesion.Evento = null;
            Sesion.Organizador = null;
            Sesion.Revisor = null;
            Close();
        }
    }
}
