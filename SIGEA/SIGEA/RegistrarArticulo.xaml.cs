using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
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
using Microsoft.Win32;
using SIGEABD;
using static SIGEA.AgregarAutor;

namespace SIGEA {
    /// <summary>
    /// Lógica de interacción para RegistrarArticulo.xaml
    /// </summary>
    public partial class RegistrarArticulo : Window {

        public ObservableCollection<AutorTabla> AutoresList { get; } = new ObservableCollection<AutorTabla>();
        private SigeaBD sigeaBD = new SigeaBD();
        private string archivoSeleccionado;
        private string nombreArchivoSeleccionado;

        public RegistrarArticulo() {
            DataContext = this;
            InitializeComponent();
            AutoresList.CollectionChanged += AutoresList_CollectionChanged;
            CargarEventos();
        }

        private void AutoresList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            autoresDataGrid.Items.Refresh();
        }

        private void CargarEventos() {
            try {
                foreach (Evento evento in sigeaBD.Evento.ToList()) {
                    eventoComboBox.Items.Add(evento);
                }
            } catch (EntityException entityException) {
                MessageBox.Show("Error al cargar los eventos.");
                Console.WriteLine("EntityException@RegistrarArticulo->CargarEventos() -> " + entityException.Message);
            }
        }

        private void eventoComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            try {
                Evento eventoSeleccionado = (Evento) eventoComboBox.SelectedItem;
                var tracks = sigeaBD.Track.Where(trackEvento => trackEvento.Evento.id_evento == eventoSeleccionado.id_evento).ToList();
                trackComboBox.Items.Clear();
                foreach (Track track in tracks) {
                    trackComboBox.Items.Add(track);
                }
                trackComboBox.IsEnabled = true;
            } catch (EntityException entityException) {
                MessageBox.Show("Error al cargar los tracks.");
                Console.WriteLine("EntityException@RegistrarArticulo->eventoComboBox_SelectionChanged() -> " + entityException.Message);
            } catch (Exception exception) {
                MessageBox.Show("Error al cargar los tracks.");
                Console.WriteLine("Exception@RegistrarArticulo->eventoComboBox_SelectionChanged() -> " + exception.Message);
            }
        }

        private void añadirAutorButton_Click(object sender, RoutedEventArgs e) {
            AgregarAutor agregarAutorVentana = new AgregarAutor();
            agregarAutorVentana.Closing += (agregarAutorSender, agregarAutorEvent) => {
                foreach (AutorTabla autorTabla in agregarAutorVentana.AutoresSeleccionados) {
                    if (!AutoresList.Contains(autorTabla)) {
                        AutoresList.Add(autorTabla);
                    }
                }
            };
            agregarAutorVentana.Show();
        }

        private void quitarAutorButton_Click(object sender, RoutedEventArgs e) {
            if (AutoresList.Where(autor => autor.Seleccionado).Count() == 0) {
                MessageBox.Show("Debes seleccionar un autor de la tabla.");
                return;
            }
            List<AutorTabla> autoresRemovidos = new List<AutorTabla>();
            foreach (var autor in AutoresList) {
                if (autor.Seleccionado) {
                    autoresRemovidos.Add(autor);
                }
            }
            foreach (var autorRemovido in autoresRemovidos) {
                AutoresList.Remove(autorRemovido);
            }
        }

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

        private void registrarButton_Click(object sender, RoutedEventArgs e) {
            if (!VerificarCampos()) {
                MessageBox.Show("Faltan campos por completar.");
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
            var nombreArchivoEncriptado = Herramientas.EncriptarConSHA512(
                nombreArchivoSeleccionado + DateTime.Now.ToUniversalTime()
            ) + ".pdf";
            sigeaBD.Articulo.Add(new Articulo {
                titulo = tituloTextBox.Text,
                anio = int.Parse(añoCreacionTextBox.Text),
                keywords = keywordsTextBox.Text,
                resumen = resumenTextBox.Text,
                Track = (Track)trackComboBox.SelectedItem,
                archivo = nombreArchivoEncriptado,
                estado = "Pendiente",
                AutorArticulo = autoresArticulo
            });
            try {
                File.Copy(archivoSeleccionado, App.ARTICULOS_DIRECTORIO + "/" + nombreArchivoEncriptado);
                if (sigeaBD.SaveChanges() != 0) {
                    MessageBox.Show("Artículo registrado.");
                    Close();
                }
            } catch (DbUpdateException dbUpdateException) {
                MessageBox.Show("Error al registrar el artículo.");
                Console.WriteLine("DbUpdateException@RegistrarArticulo->registrarButton_Click() -> " + dbUpdateException.Message);
            } catch (Exception exception) {
                Console.WriteLine("Exception@RegistrarArticulo->registrarButton_Click() -> " + exception.Message);
            }
        }

        private bool VerificarCampos() {
            return !string.IsNullOrWhiteSpace(tituloTextBox.Text) &&
                !string.IsNullOrWhiteSpace(añoCreacionTextBox.Text) &&
                !string.IsNullOrWhiteSpace(keywordsTextBox.Text) &&
                !string.IsNullOrWhiteSpace(resumenTextBox.Text) &&
                eventoComboBox.SelectedIndex != -1 &&
                trackComboBox.SelectedIndex != -1 &&
                AutoresList.Count > 0;
        }
    }
}
