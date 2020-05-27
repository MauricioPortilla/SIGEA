using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using SIGEABD;
using static SIGEA.AgregarAutor;

namespace SIGEA {
    /// <summary>
    /// Lógica de interacción para RegistrarArticulo.xaml
    /// </summary>
    public partial class RegistrarArticulo : Window {

        public ObservableCollection<AutorTabla> AutoresList { get; } = new ObservableCollection<AutorTabla>();
        private List<AutorTabla> autoresSeleccionados = new List<AutorTabla>();
        private string archivoSeleccionado;
        private string nombreArchivoSeleccionado;

        /// <summary>
        /// Crea una instancia.
        /// </summary>
        public RegistrarArticulo() {
            DataContext = this;
            InitializeComponent();
            AutoresList.CollectionChanged += AutoresList_CollectionChanged;
            CargarTracks();
        }

        /// <summary>
        /// Muestra el panel principal al cerrarse.
        /// </summary>
        /// <param name="e">Evento</param>
        protected override void OnClosing(CancelEventArgs e) {
            new PanelLiderComite().Show();
        }

        /// <summary>
        /// Actualiza la tabla de autores en cuanto surjan cambios en la colección.
        /// </summary>
        /// <param name="sender">Colección</param>
        /// <param name="e">Evento</param>
        private void AutoresList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            autoresDataGrid.Items.Refresh();
        }

        /// <summary>
        /// Carga los tracks del evento.
        /// </summary>
        private void CargarTracks() {
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    var tracks = sigeaBD.Track.Where(trackEvento => trackEvento.Evento.id_evento == Sesion.Evento.id_evento).ToList();
                    trackComboBox.Items.Clear();
                    foreach (Track track in tracks) {
                        trackComboBox.Items.Add(track);
                    }
                    trackComboBox.IsEnabled = true;
                }
            } catch (EntityException entityException) {
                MessageBox.Show("Error al establecer una conexión.");
                Console.WriteLine("EntityException@RegistrarArticulo->CargarTracks() -> " + entityException.Message);
            } catch (Exception exception) {
                MessageBox.Show("Error al establecer una conexión.");
                Console.WriteLine("Exception@RegistrarArticulo->CargarTracks() -> " + exception.Message);
            }
        }

        /// <summary>
        /// Si se selecciona algún Autor, se añade a la lista de autores seleccionados.
        /// </summary>
        /// <param name="sender">AutorTabla</param>
        /// <param name="e">Evento</param>
        public void AutorTabla_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            var autorSeleccion = (AutorTabla)sender;
            if (autorSeleccion.Seleccionado) {
                if (!autoresSeleccionados.Exists(autorLista => autorLista.Autor == autorSeleccion.Autor)) {
                    autoresSeleccionados.Add(autorSeleccion);
                }
            } else {
                if (autoresSeleccionados.Exists(autorLista => autorLista.Autor == autorSeleccion.Autor)) {
                    autoresSeleccionados.RemoveAll(autorLista => autorLista.Autor == autorSeleccion.Autor);
                }
            }
        }

        /// <summary>
        /// Muestra la ventana para agregar un autor a la tabla.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void añadirAutorButton_Click(object sender, RoutedEventArgs e) {
            AgregarAutor agregarAutorVentana = new AgregarAutor();
            agregarAutorVentana.Closing += (agregarAutorSender, agregarAutorEvent) => {
                foreach (AutorTabla autorTabla in agregarAutorVentana.AutoresSeleccionados) {
                    if (!AutoresList.Contains(autorTabla)) {
                        var autorNuevo = new AutorTabla {
                            Seleccionado = false,
                            Autor = autorTabla.Autor,
                            Nombre = autorTabla.Nombre,
                            Paterno = autorTabla.Paterno,
                            Materno = autorTabla.Materno,
                            Correo = autorTabla.Correo
                        };
                        autorNuevo.PropertyChanged += AutorTabla_PropertyChanged;
                        AutoresList.Add(autorNuevo);
                    }
                }
            };
            agregarAutorVentana.Show();
        }

        /// <summary>
        /// Quita un autor seleccionado de la tabla.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void quitarAutorButton_Click(object sender, RoutedEventArgs e) {
            if (autoresSeleccionados.Where(autor => autor.Seleccionado).Count() == 0) {
                MessageBox.Show("Debes seleccionar un autor de la tabla.");
                return;
            }
            foreach (var autorRemovido in autoresSeleccionados) {
                AutoresList.Remove(AutoresList.First(autor => autor.Autor == autorRemovido.Autor));
            }
            autoresSeleccionados.Clear();
        }

        /// <summary>
        /// Abre un cuadro de diálogo para seleccionar un archivo PDF y almacena su ruta.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void subirArchivoButton_Click(object sender, RoutedEventArgs e) {
            var seleccionArchivo = new OpenFileDialog();
            seleccionArchivo.Filter = "PDF Files|*.pdf";
            seleccionArchivo.Title = "Selecciona tu artículo";
            seleccionArchivo.Multiselect = false;
            var resultado = seleccionArchivo.ShowDialog();
            if (resultado.HasValue && resultado.Value) {
                archivoSeleccionado = seleccionArchivo.FileName;
                nombreArchivoSeleccionado = seleccionArchivo.SafeFileName;
                archivoSeleccionadoTextBlock.Text = nombreArchivoSeleccionado;
                archivoSeleccionadoTextBlock.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Verifica que los campos estén completos, que se haya seleccionado
        /// el archivo del artículo, y registra el artículo en la base de datos.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void registrarButton_Click(object sender, RoutedEventArgs e) {
            if (!VerificarCampos()) {
                MessageBox.Show("Faltan campos por completar.");
                return;
            }
            if (!ValidarDatos()) {
                MessageBox.Show("Debes introducir datos válidos.");
                return;
            }
            if (string.IsNullOrWhiteSpace(archivoSeleccionado)) {
                MessageBox.Show("Debes seleccionar el archivo que incluye tu artículo.");
                return;
            }
            var autoresArticulo = new Collection<AutorArticulo>();
            foreach (var autor in AutoresList) {
                autoresArticulo.Add(new AutorArticulo {
                    Autor = autor.Autor,
                    fecha = DateTime.Now
                });
            }
            var nombreArchivoEncriptado = Herramientas.CifrarConSHA512(
                nombreArchivoSeleccionado + DateTime.Now.ToUniversalTime()
            ) + ".pdf";
            try {
                Articulo nuevoArticulo = new Articulo {
                    titulo = tituloTextBox.Text,
                    anio = int.Parse(añoCreacionTextBox.Text),
                    keywords = keywordsTextBox.Text,
                    resumen = resumenTextBox.Text,
                    Track = (Track)trackComboBox.SelectedItem,
                    archivo = nombreArchivoEncriptado,
                    estado = "Pendiente",
                    AutorArticulo = autoresArticulo
                };
                if (nuevoArticulo.Registrar()) {
                    File.Copy(archivoSeleccionado, App.ARTICULOS_DIRECTORIO + "/" + nombreArchivoEncriptado);
                    MessageBox.Show("Artículo registrado.");
                    Close();
                    return;
                }
                MessageBox.Show("Error al establecer una conexión.");
            } catch (DbUpdateException dbUpdateException) {
                MessageBox.Show("Error al establecer una conexión.");
                Console.WriteLine("DbUpdateException@RegistrarArticulo->registrarButton_Click() -> " + dbUpdateException.Message);
            } catch (Exception exception) {
                MessageBox.Show("Error al establecer una conexión.");
                Console.WriteLine("Exception@RegistrarArticulo->registrarButton_Click() -> " + exception.Message);
            }
        }

        /// <summary>
        /// Verifica que los campos estén completos.
        /// </summary>
        /// <returns>true si están completos; false si no</returns>
        private bool VerificarCampos() {
            return !string.IsNullOrWhiteSpace(tituloTextBox.Text) &&
                !string.IsNullOrWhiteSpace(añoCreacionTextBox.Text) &&
                !string.IsNullOrWhiteSpace(keywordsTextBox.Text) &&
                !string.IsNullOrWhiteSpace(resumenTextBox.Text) &&
                trackComboBox.SelectedIndex != -1 &&
                AutoresList.Count > 0;
        }

        /// <summary>
        /// Verifica que los campos contengan datos válidos.
        /// </summary>
        /// <returns>true si son válidos; false si no</returns>
        private bool ValidarDatos() {
            return Regex.IsMatch(añoCreacionTextBox.Text, Herramientas.REGEX_SOLO_NUMEROS);
        }
    }
}
