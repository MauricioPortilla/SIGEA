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
        public AgregarAutor() {
            DataContext = this;
            InitializeComponent();
            AutoresList.CollectionChanged += AutoresList_CollectionChanged;
            CargarAutores();
        }

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

        public void AutorTabla_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            autoresDataGrid.Items.Refresh();
        }

        private void AutoresList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            autoresDataGrid.Items.Refresh();
        }

        public struct AutorTabla: INotifyPropertyChanged {
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

        private void añadirButton_Click(object sender, RoutedEventArgs e) {
            foreach (AutorTabla autorTabla in autoresDataGrid.Items) {
                if (autorTabla.Seleccionado) {
                    AutoresSeleccionados.Add(autorTabla);
                }
            }
            Close();
        }

        private void cancelarButton_Click(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}
