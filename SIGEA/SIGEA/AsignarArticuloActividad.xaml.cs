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
    /// Lógica de interacción para AsignarArticuloActividad.xaml
    /// </summary>
    public partial class AsignarArticuloActividad : Window {

        public ObservableCollection<ArticuloTabla> ListaArticulos { get; } = 
            new ObservableCollection<ArticuloTabla>();
        public ObservableCollection<Presentacion> ListaPresentaciones { get; } =
            new ObservableCollection<Presentacion>();
        public int actividad;

        public AsignarArticuloActividad(int idActividad) {
            InitializeComponent();
            DataContext = this;
            this.actividad = idActividad;
        }

        /// <summary>
        /// Metodo que cierra la ventana
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelarButton_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void AsignarButton_Click(object sender, RoutedEventArgs e) {
            if(VerificarSeleccion()) {
                try {
                    using(SigeaBD sigeaBD = new SigeaBD()) {
                        var articuloSelec = (ArticuloTabla) articulosListView.SelectedItem;
                        var articuloObtenido = sigeaBD.Articulo.Find(articuloSelec.Articulo.id_articulo);
                        articuloObtenido.Presentacion = new Collection<Presentacion>()
                        { presentacionesComboBox.SelectedItem as Presentacion };
                        sigeaBD.Presentacion.Attach(presentacionesComboBox.SelectedItem as Presentacion);
                        if(sigeaBD.SaveChanges() != 0) {
                            MessageBox.Show("Asignación con éxito");
                        } else {
                            MessageBox.Show("No se asignó el articulo");
                        }
                    }
                } catch(Exception) {
                    MessageBox.Show("Lo sentimos inténtelo más tarde");
                }
            }
        }

        /// <summary>
        /// Muestra el panel principal al cerrarse.
        /// </summary>
        /// <param name="e">Evento</param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e) {
            new PanelLiderEvento().Show();
        }

        public void CargarDatos() {
            try {
                using(SigeaBD sigeaBD = new SigeaBD()) {
                    var articulos = sigeaBD.Articulo.Where(
                        articulo => articulo.Track.Evento.id_evento == Sesion.Evento.id_evento &&
                        articulo.Presentacion == null).ToList();
                    foreach(Articulo articulo in articulos) {
                        ListaArticulos.Add(new ArticuloTabla {
                            Articulo = articulo,
                            Titulo = articulo.titulo,
                            KeyWords = articulo.keywords
                        });
                    }
                    var presentaciones = sigeaBD.Presentacion.Where(
                        presentacion => presentacion.Actividad.id_actividad == actividad).ToList();
                    foreach(Presentacion presentacion in presentaciones) {
                        ListaPresentaciones.Add(presentacion);
                    }
                    presentacionesComboBox.ItemsSource = ListaPresentaciones;
                }
             } catch(Exception) {
                MessageBox.Show("Lo sentimos inténtelo más tarde");
            }
        }

        public Boolean VerificarSeleccion() {
            if(articulosListView.SelectedItem != null &&
                presentacionesComboBox.SelectedItem != null) {
                return true;
            } else {
                return false;
            }
        }

        /// <summary>
        /// Estrucutra para llenar la Tabla
        /// </summary>
        public struct ArticuloTabla {
            public Articulo Articulo;
            public string Titulo { get; set; }
            public string KeyWords { get; set; }
        }
    }
}
