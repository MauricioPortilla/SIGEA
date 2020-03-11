using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
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
using SIGEABD;

namespace SIGEA {
    /// <summary>
    /// Lógica de interacción para AgregarAutor.xaml
    /// </summary>
    public partial class AgregarAutor : Window {
        public List<AutorTabla> AutoresSeleccionados = new List<AutorTabla>();
        public ObservableCollection<AutorTabla> AutoresList { get; } = new ObservableCollection<AutorTabla>();

        /// <summary>
        /// Crea la instancia.
        /// </summary>
        public AgregarAutor() {
            DataContext = this;
            InitializeComponent();
            AutoresList.CollectionChanged += AutoresList_CollectionChanged;
            CargarAutores();
        }

        /// <summary>
        /// Carga los autores de la base de datos.
        /// </summary>
        private void CargarAutores() {
            using (SigeaBD sigeaBD = new SigeaBD()) {
                foreach (Autor autor in sigeaBD.Autor.ToList()) {
                    var autorTabla = new AutorTabla {
                        Autor = autor,
                        Seleccionado = false,
                        Nombre = autor.nombre,
                        Paterno = autor.paterno,
                        Materno = autor.materno,
                        Correo = autor.correo
                    };
                    autorTabla.PropertyChanged += AutorTabla_PropertyChanged;
                    AutoresList.Add(autorTabla);
                }
            }
        }

        /// <summary>
        /// Actualiza la tabla de autores si alguna de sus propiedades cambió su valor.
        /// </summary>
        /// <param name="sender">Tabla</param>
        /// <param name="e">Evento</param>
        public void AutorTabla_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            autoresDataGrid.Items.Refresh();
        }

        /// <summary>
        /// Actualiza la tabla de autores si su colección cambió.
        /// </summary>
        /// <param name="sender">Tabla</param>
        /// <param name="e">Evento</param>
        private void AutoresList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            autoresDataGrid.Items.Refresh();
        }

        /// <summary>
        /// Representa un Autor en las tablas de autores.
        /// </summary>
        public struct AutorTabla : INotifyPropertyChanged {
            public Autor Autor { get; set; }
            public bool Seleccionado { get; set; }
            public string Nombre { get; set; }
            public string Paterno { get; set; }
            public string Materno { get; set; }
            public string Correo { get; set; }
            public event PropertyChangedEventHandler PropertyChanged;
            public void NotifyPropertyChanged(string obj) {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(obj));
            }
        }

        /// <summary>
        /// Añade los autores seleccionados de la tabla a la lista de autores seleccionados
        /// y cierra la ventana actual.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void añadirButton_Click(object sender, RoutedEventArgs e) {
            foreach (AutorTabla autorTabla in autoresDataGrid.Items) {
                if (autorTabla.Seleccionado) {
                    AutoresSeleccionados.Add(autorTabla);
                }
            }
            Close();
        }

        /// <summary>
        /// Cierra la ventana actual.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void cancelarButton_Click(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}
