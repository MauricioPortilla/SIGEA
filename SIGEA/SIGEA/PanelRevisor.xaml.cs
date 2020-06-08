using SIGEABD;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
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
                    var articulos = sigeaBD.Articulo.Where(
                        articulo => articulo.RevisorArticulo.Where(
                            revisorArticulo => revisorArticulo.id_revisor == Sesion.Revisor.id_revisor
                        ).Count() > 0
                    );
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
                try {
                    bool acceder = true;
                    EvaluacionArticulo.ObtenerEvaluacionArticulo(articuloSeleccionado.Articulo.id_articulo, Sesion.Revisor.id_revisor, (evaluacionArticulo) => {
                        if (evaluacionArticulo.estado == "Finalizada") {
                            MessageBox.Show("Ya has emitido una evaluación para este artículo.");
                            acceder = false;
                        }
                    });
                    if (acceder) {
                        new EvaluarArticulo(articuloSeleccionado.Articulo.id_articulo).Show();
                        Close();
                    }
                } catch (Exception) {
                    MessageBox.Show("Error al establecer una conexión.");
                }
            } else {
                MessageBox.Show("Debes seleccionar un artículo.");
            }
        }

        /// <summary>
        /// Cierra la ventana y vuelve a la de Iniciar sesión.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void CerrarSesionButton_Click(object sender, RoutedEventArgs e) {
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
