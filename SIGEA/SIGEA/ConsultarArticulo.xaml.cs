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
        /// Muestra una ventana Consultar evaluaciones artículo.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void ConsultarEvaluacionesButton_Click(object sender, RoutedEventArgs e) {
            new ConsultarEvaluacionesArticulo(articulo).Show();
            // TODO: Revisar si debe ir aquí.
        }

        /// <summary>
        /// Muestra una ventana Actualizar artículo.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void ActualizarButton_Click(object sender, RoutedEventArgs e) {
            new ActualizarArticulo(articulo.id_articulo).Show();
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
