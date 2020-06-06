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
    public partial class AsignarArticulo : Window {

        private List<Revisor> revisores = new List<Revisor>();
        private List<Articulo> articulos = new List<Articulo>();

        public ObservableCollection<RevisorTabla> RevisoresLista { get; } = 
            new ObservableCollection<RevisorTabla>();
        public ObservableCollection<ArticuloTabla> ArticulosLista { get; } = 
            new ObservableCollection<ArticuloTabla>();

        public AsignarArticulo() {

            InitializeComponent();
            DataContext = this;
            CargarTabla();
        }

        /// <summary>
        /// Metodo que cierra la ventana y vuelve a Panel Lider de Evento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelarButton_Click(object sender, RoutedEventArgs e) {

            new PanelLiderEvento().Show();
            this.Close();
        }

        private void AsignarButton_Click(object sender, RoutedEventArgs e) {

            try {

                using(SigeaBD sigeaBD = new SigeaBD()) {

                    Collection<Revisor> revisorSeleccionado = new Collection<Revisor>();

                    foreach(RevisorTabla revisorTabla in revisoresListView.SelectedItems) {

                        revisorSeleccionado.Add(revisorTabla.Revisor);
                    }

                    var articuloSeleccionado = (Articulo) articulosListView.SelectedItem;
                    var articulo = sigeaBD.Articulo.Find(articuloSeleccionado.id_articulo);
                    //articulo.RevisorArticulo;

                    if(sigeaBD.SaveChanges() != 0) {

                        MessageBox.Show("Artículo asignado con éxito");
                        revisores.Clear();
                        articulos.Clear();
                        RevisoresLista.Clear();
                        ArticulosLista.Clear();
                        CargarTabla();

                    } else {

                        MessageBox.Show("No se asigno el artículo");
                    }
                }
            } catch(Exception) {

                MessageBox.Show("Lo sentimos inténtelo más tarde");
            }
        }

        /// <summary>
        /// Carga ambas tablas con sus respectivos datos (arrticulos y organizadores)
        /// </summary>
        public void CargarTabla() {

            try {

                using(SigeaBD sigeaBD = new SigeaBD()) {

                    revisores = sigeaBD.Revisor.ToList();
                }

                using(SigeaBD sigeaBD = new SigeaBD()) {

                    articulos = sigeaBD.Articulo.Where(
                        articulo => articulo.Track.id_evento == Sesion.Evento.id_evento &&
                        articulo.AutorArticulo.Count == 0).ToList();
                }
            } catch(Exception) {

                MessageBox.Show("Lo sentimos inténtelo más tarde");
            }

            foreach(var revisor in revisores) {

                RevisoresLista.Add(new RevisorTabla { 

                    Nombre = revisor.nombre,
                    Paterno = revisor.paterno,
                    Materno = revisor.materno
                });
            }

            foreach(var articulo in articulos) {

                ArticulosLista.Add(new ArticuloTabla {

                    Titulo = articulo.titulo,
                    KeyWords = articulo.keywords
                });
            }
        }

        /// <summary>
        /// verifica que existan ambas sleecciones
        /// </summary>
        /// <returns></returns>
        public Boolean RevisarSeleccion() {

            var revisor = (Revisor) revisoresListView.SelectedItem;
            var articulo = (Articulo) articulosListView.SelectedItem;

            if(revisor != null && articulo != null) {

                return true;

            } else {

                MessageBox.Show("Seleccione un artículo y un revisor");
                return false;
            }
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
            public string KeyWords { get; set; }
        }
    }
}
