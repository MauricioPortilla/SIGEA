using SIGEABD;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using static SIGEA.AgregarAutor;

namespace SIGEA {
    /// <summary>
    /// Lógica de interacción para ConsultarArticulo.xaml
    /// </summary>
    public partial class ConsultarArticulo : Window {
        private Articulo articulo;
        public ObservableCollection<AutorTabla> AutoresList { get; } = new ObservableCollection<AutorTabla>();

        /// <summary>
        /// Crea una instancia.
        /// </summary>
        /// <param name="articulo">Artículo a consultar</param>
        public ConsultarArticulo(Articulo articulo) {
            InitializeComponent();
            DataContext = this;
            this.articulo = articulo;
            CargarArticulo();
            if (articulo.estado == "Requiere actualizarse") {
                actualizarButton.IsEnabled = true;
            }
        }

        /// <summary>
        /// Carga el artículo de la base de datos.
        /// </summary>
        private void CargarArticulo() {
            try {
                Articulo.ObtenerArticulo(articulo.id_articulo, (articulo) => {
                    this.articulo = articulo;
                    if (articulo == null) {
                        MessageBox.Show("Error al establecer una conexión.");
                        Close();
                        return;
                    }
                    tituloTextBlock.Text = articulo.titulo;
                    anioTextBlock.Text = articulo.anio.ToString();
                    nombreTrackTextBlock.Text = articulo.Track.nombre;
                    keywordsTextBlock.Text = articulo.keywords;
                    resumenTextBlock.Text = articulo.resumen;
                    foreach (AutorArticulo autorArticulo in articulo.AutorArticulo) {
                        AutoresList.Add(new AutorTabla {
                            Autor = autorArticulo.Autor,
                            Nombre = autorArticulo.Autor.nombre,
                            Paterno = autorArticulo.Autor.paterno,
                            Materno = autorArticulo.Autor.materno == null ? "" : autorArticulo.Autor.materno,
                            Correo = autorArticulo.Autor.correo
                        });
                    }
                    autoresDataGrid.Items.Refresh();
                });
            } catch (Exception) {
                MessageBox.Show("Error al establecer una conexión.");
                Close();
            }
        }

        /// <summary>
        /// Muestra una ventana Actualizar artículo.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void ActualizarButton_Click(object sender, RoutedEventArgs e) {
            new ActualizarArticulo(articulo.id_articulo).Show();
            Close();
        }

        /// <summary>
        /// Muestra el archivo del artículo.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void VerButton_Click(object sender, RoutedEventArgs e) {
            System.Diagnostics.Process.Start(App.ARTICULOS_DIRECTORIO + "/" + articulo.archivo);
        }
    }
}
