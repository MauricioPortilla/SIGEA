using SIGEABD;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using static SIGEA.ConsultarEvaluacionesArticulos;

namespace SIGEA {
    /// <summary>
    /// Lógica de interacción para ConsultarArticulos.xaml
    /// </summary>
    public partial class ConsultarArticulos : Window {
        public ObservableCollection<ArticuloTabla> ArticulosLista { get; } = new ObservableCollection<ArticuloTabla>();

        /// <summary>
        /// Crea una instancia.
        /// </summary>
        public ConsultarArticulos() {
            InitializeComponent();
            DataContext = this;
            CargarArticulos();
        }

        /// <summary>
        /// Muestra el panel principal al cerrarse.
        /// </summary>
        /// <param name="e">Evento</param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e) {
            new PanelLiderComite().Show();
        }

        /// <summary>
        /// Carga los artículos.
        /// </summary>
        private void CargarArticulos() {
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    var articulos = sigeaBD.Articulo.Where(articulo => articulo.Track.id_evento == Sesion.Evento.id_evento);
                    foreach (Articulo articulo in articulos) {
                        var autor = articulo.AutorArticulo.FirstOrDefault().Autor;
                        ArticulosLista.Add(new ArticuloTabla {
                            Articulo = articulo,
                            Titulo = articulo.titulo,
                            Estado = articulo.estado,
                            Autor = autor.nombre + " " + autor.paterno + " " + autor.materno
                        });
                    }
                }
            } catch (Exception) {
                MessageBox.Show("Error al establecer una conexión.");
                Close();
            }
        }

        /// <summary>
        /// Muestra una ventana Consultar Artículo con el artículo seleccionado.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void ConsultarButton_Click(object sender, RoutedEventArgs e) {
            if (articulosListView.SelectedIndex != -1) {
                new ConsultarArticulo(((ArticuloTabla) articulosListView.SelectedItem).Articulo).Show();
                Close();
            } else {
                MessageBox.Show("Debes seleccionar un artículo.");
            }
        }

        /// <summary>
        /// Muestra una ventana Registrar Pago Artículo con el artículo seleccionado.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void RegistrarPagoButton_Click(object sender, RoutedEventArgs e) {
            if (articulosListView.SelectedIndex != -1) {
                Articulo articuloSeleccionado = ((ArticuloTabla) articulosListView.SelectedItem).Articulo;
                if (articuloSeleccionado.estado == "Aceptado") {
                    new RegistrarPagoArticulo(articuloSeleccionado).Show();
                    Close();
                } else {
                    MessageBox.Show("Este artículo aún no ha sido aceptado.");
                }
            } else {
                MessageBox.Show("Debes seleccionar un artículo.");
            }
        }
    }
}
