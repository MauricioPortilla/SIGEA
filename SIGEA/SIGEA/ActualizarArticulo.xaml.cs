using Microsoft.Win32;
using SIGEABD;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
    /// Lógica de interacción para ActualizarArticulo.xaml
    /// </summary>
    public partial class ActualizarArticulo : Window {
        
        public ObservableCollection<AutorTabla> AutoresList { get; } = new ObservableCollection<AutorTabla>();
        private Articulo articulo;
        private string rutaArchivo = string.Empty;

        /// <summary>
        /// Crea una instancia.
        /// </summary>
        /// <param name="id_articulo">Identificador de Articulo</param>
        public ActualizarArticulo(int id_articulo) {
            InitializeComponent();
            DataContext = this;
            CargarArticulo(id_articulo);
        }

        /// <summary>
        /// Carga la información de un Artículo.
        /// </summary>
        /// <param name="id_articulo">Identificador del Artículo</param>
        private void CargarArticulo(int id_articulo) {
            try {
                Articulo.ObtenerArticulo(id_articulo, (articulo) => {
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
        /// Muestra una ventana de explorador de archivos para seleccionar un
        /// archivo y obtener su ruta.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento del botón</param>
        private void ReemplazarArchivoButton_Click(object sender, RoutedEventArgs e) {
            var seleccionArchivo = new OpenFileDialog();
            seleccionArchivo.Filter = "PDF Files|*.pdf";
            seleccionArchivo.Title = "Selecciona tu artículo";
            seleccionArchivo.Multiselect = false;
            var resultado = seleccionArchivo.ShowDialog();
            if (resultado.HasValue && resultado.Value) {
                rutaArchivo = seleccionArchivo.FileName;
                archivoSeleccionadoTextBlock.Text = seleccionArchivo.SafeFileName;
                archivoSeleccionadoTextBlock.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Verifica si se seleccionó un archivo; si sí, reemplaza el existente del Articulo
        /// por el nuevo, conservando el nombre del antiguo archivo.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento del botón</param>
        private void GuardarCambiosButton_Click(object sender, RoutedEventArgs e) {
            if (rutaArchivo.Equals(string.Empty)) {
                MessageBox.Show("Debes seleccionar un archivo a reemplazar.");
                return;
            }
            articulo.estado = "Aceptado";
            try {
                if (!articulo.Actualizar()) {
                    MessageBox.Show("Error al establecer una conexión.");
                    return;
                }
                File.Copy(rutaArchivo, App.ARTICULOS_DIRECTORIO + "/" + articulo.archivo, true);
                MessageBox.Show("Cambios guardados.");
                Close();
            } catch (Exception) {
                MessageBox.Show("Error al establecer una conexión.");
            }
        }
    }
}
