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
    /// Lógica de interacción para AsignarArticulo.xaml
    /// </summary>
    public partial class AsignarArticuloRevisor : Window {
        public ObservableCollection<RevisorTabla> RevisoresLista { get; } = 
            new ObservableCollection<RevisorTabla>();
        public ObservableCollection<ArticuloTabla> ArticulosLista { get; } = 
            new ObservableCollection<ArticuloTabla>();

        /// <summary>
        /// Crea una instancia.
        /// </summary>
        public AsignarArticuloRevisor() {
            InitializeComponent();
            DataContext = this;
            CargarTabla();
        }

        /// <summary>
        /// Muestra el panel principal al cerrarse.
        /// </summary>
        /// <param name="e">Evento</param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e) {
            new PanelLiderEvento().Show();
        }

        /// <summary>
        /// Carga ambas tablas con sus respectivos datos (articulos y organizadores)
        /// </summary>
        public void CargarTabla() {
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    var revisores = sigeaBD.Revisor.ToList();
                    foreach (var revisor in revisores) {
                        RevisoresLista.Add(new RevisorTabla {
                            Revisor = revisor,
                            Nombre = revisor.nombre,
                            Paterno = revisor.paterno,
                            Materno = revisor.materno
                        });
                    }
                    var articulos = sigeaBD.Articulo.Where(
                        articulo => articulo.Track.id_evento == Sesion.Evento.id_evento &&
                        articulo.RevisorArticulo.Count == 0
                    ).ToList();
                    foreach (var articulo in articulos) {
                        ArticulosLista.Add(new ArticuloTabla {
                            Articulo = articulo,
                            Titulo = articulo.titulo,
                            Keywords = articulo.keywords
                        });
                    }
                }
            } catch (Exception) {
                MessageBox.Show("Lo sentimos inténtelo más tarde");
            }
        }

        /// <summary>
        /// Metodo que cierra la ventana.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void CancelarButton_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        /// <summary>
        /// Asigna el artículo seleccionado a los revisores seleccionados.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void AsignarButton_Click(object sender, RoutedEventArgs e) {
            if (!RevisarSeleccion()) {
                MessageBox.Show("Seleccione un artículo y un revisor");
                return;
            }
            try {
                using(SigeaBD sigeaBD = new SigeaBD()) {
                    var articuloSeleccionado = (ArticuloTabla) articulosListView.SelectedItem;
                    foreach(RevisorTabla revisorTabla in revisoresListView.SelectedItems) {
                        sigeaBD.RevisorArticulo.Add(new RevisorArticulo {
                            id_articulo = articuloSeleccionado.Articulo.id_articulo,
                            id_revisor = revisorTabla.Revisor.id_revisor
                        });
                    }
                    if(sigeaBD.SaveChanges() != 0) {
                        MessageBox.Show("Artículo asignado con éxito");
                        RevisoresLista.Clear();
                        ArticulosLista.Clear();
                        CargarTabla();
                    } else {
                        MessageBox.Show("No se asignó el artículo");
                    }
                }
            } catch(Exception) {
                MessageBox.Show("Lo sentimos inténtelo más tarde");
            }
        }

        /// <summary>
        /// verifica que existan ambas selecciones.
        /// </summary>
        /// <returns>true si se seleccionaron revisores y un artículo; false si no</returns>
        public bool RevisarSeleccion() {
            var revisor = revisoresListView.SelectedItems;
            var articulo = articulosListView.SelectedItem;
            return revisor.Count > 0 && articulo != null;
        }

        /// <summary>
        /// Representa a un Revisor en la tabla
        /// </summary>
        public struct RevisorTabla {
            public Revisor Revisor;
            public string Nombre { get; set; }
            public string Paterno { get; set; }
            public string Materno { get; set; }
        }

        /// <summary>
        /// Representa un Articulo en la tabla
        /// </summary>
        public struct ArticuloTabla {
            public Articulo Articulo;
            public string Titulo { get; set; }
            public string Keywords { get; set; }
        }
    }
}
