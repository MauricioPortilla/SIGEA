using SIGEABD;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace SIGEA {

    /// <summary>
    /// Lógica de interacción para AsignarArticuloActividad.xaml
    /// </summary>
    public partial class AsignarArticuloActividad : Window {

        public ObservableCollection<ArticuloTabla> ListaArticulos { get; } = 
            new ObservableCollection<ArticuloTabla>();
        public int id_actividad;

        /// <summary>
        /// Crea una instancia.
        /// </summary>
        /// <param name="idActividad">Identificador de la Actividad</param>
        public AsignarArticuloActividad(int idActividad) {
            InitializeComponent();
            DataContext = this;
            this.id_actividad = idActividad;
            CargarDatos();
        }

        /// <summary>
        /// Metodo que cierra la ventana
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void CancelarButton_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        /// <summary>
        /// Realiza la asignación.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void AsignarButton_Click(object sender, RoutedEventArgs e) {
            if(VerificarSeleccion()) {
                try {
                    using(SigeaBD sigeaBD = new SigeaBD()) {
                        var articuloSelec = (ArticuloTabla) articulosListView.SelectedItem;
                        var articuloObtenido = sigeaBD.Articulo.Find(articuloSelec.Articulo.id_articulo);
                        var presentacion = sigeaBD.Presentacion.Find(
                            (presentacionesComboBox.SelectedItem as Presentacion).id_presentacion
                        );
                        articuloObtenido.Presentacion = new Collection<Presentacion>() {
                            presentacion
                        };
                        if(sigeaBD.SaveChanges() != 0) {
                            MessageBox.Show("Asignación con éxito");
                            Close();
                        } else {
                            MessageBox.Show("No se asignó el articulo");
                        }
                    }
                } catch(Exception) {
                    MessageBox.Show("Lo sentimos inténtelo más tarde");
                }
            } else {
                MessageBox.Show("Seleccione un artículo y una presentación");
            }
        }

        /// <summary>
        /// Muestra el panel principal al cerrarse.
        /// </summary>
        /// <param name="e">Evento</param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e) {
            new PanelLiderEvento().Show();
        }

        /// <summary>
        /// Carga los artículos y las presentaciones de la base de datos.
        /// </summary>
        public void CargarDatos() {
            try {
                using(SigeaBD sigeaBD = new SigeaBD()) {
                    var articulos = sigeaBD.Articulo.Where(
                        articulo => articulo.Track.Evento.id_evento == Sesion.Evento.id_evento &&
                        articulo.Presentacion.Count == 0);
                    foreach(Articulo articulo in articulos) {
                        ListaArticulos.Add(new ArticuloTabla {
                            Articulo = articulo,
                            Titulo = articulo.titulo,
                            KeyWords = articulo.keywords
                        });
                    }
                    var presentaciones = sigeaBD.Presentacion.Where(
                        presentacion => presentacion.Actividad.id_actividad == id_actividad);
                    foreach(Presentacion presentacion in presentaciones) {
                        presentacionesComboBox.Items.Add(presentacion);
                    }
                }
            } catch(Exception) {
                MessageBox.Show("Lo sentimos inténtelo más tarde");
            }
        }

        /// <summary>
        /// Verifica si se ha seleccionado el artículo y la presentación.
        /// </summary>
        /// <returns>true si se seleccionaron; false si no</returns>
        public Boolean VerificarSeleccion() {
            if(articulosListView.SelectedItem != null &&
                presentacionesComboBox.SelectedItem != null) {
                return true;
            } else {
                return false;
            }
        }

        /// <summary>
        /// Estructura para llenar la Tabla
        /// </summary>
        public struct ArticuloTabla {
            public Articulo Articulo;
            public string Titulo { get; set; }
            public string KeyWords { get; set; }
        }
    }
}
