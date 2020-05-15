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
    /// Lógica de interacción para ConsultarEvaluacionesArticulo.xaml
    /// </summary>
    public partial class ConsultarEvaluacionesArticulo : Window {
        private Articulo articulo;
        private ObservableCollection<EvaluacionArticuloTabla> evaluacionesList = new ObservableCollection<EvaluacionArticuloTabla>();
        public ObservableCollection<EvaluacionArticuloTabla> EvaluacionesList {
            get => evaluacionesList;
        }

        public ConsultarEvaluacionesArticulo(Articulo articulo) {
            InitializeComponent();
            DataContext = this;
            this.articulo = articulo;
            CargarEvaluacionesArticulo();
            if (articulo.estado == "Pendiente") {
                aceptarArticuloButton.IsEnabled = true;
                requerirActualizacionButton.IsEnabled = true;
            }
        }

        private void CargarEvaluacionesArticulo() {
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    var evaluacionesArticulo = sigeaBD.EvaluacionArticulo.Where(
                        evaluacionArticulo => 
                            evaluacionArticulo.RevisorArticulo.Articulo.id_articulo == articulo.id_articulo
                    );
                    foreach (EvaluacionArticulo evaluacionArticulo in evaluacionesArticulo) {
                        evaluacionesList.Add(new EvaluacionArticuloTabla {
                            Fecha = evaluacionArticulo.fecha.ToString("dd/MM/yyyy"),
                            Revisor = evaluacionArticulo.RevisorArticulo.Revisor.ToString(),
                            Calificacion = evaluacionArticulo.calificacion,
                            GradoExpertiz = evaluacionArticulo.gradoExpertiz,
                            Estado = evaluacionArticulo.estado
                        });
                    }
                }
            } catch (Exception) {
                MessageBox.Show("Error al establecer una conexión.");
                Close();
            }
        }
        private void AceptarArticuloButton_Click(object sender, RoutedEventArgs e) {
            articulo.estado = "Aceptado";
            try {
                if (!articulo.Actualizar()) {
                    MessageBox.Show("Error al establecer una conexión.");
                    return;
                }
                MessageBox.Show("Artículo aceptado.");
                Close();
            } catch (Exception) {
                MessageBox.Show("Error al establecer una conexión.");
            }
        }

        private void RequerirActualizacionButton_Click(object sender, RoutedEventArgs e) {
            articulo.estado = "Requiere actualizarse";
            try {
                if (!articulo.Actualizar()) {
                    MessageBox.Show("Error al establecer una conexión.");
                    return;
                }
                MessageBox.Show("Solicitud de actualización asignada.");
                Close();
            } catch (Exception) {
                MessageBox.Show("Error al establecer una conexión.");
            }
        }

        public struct EvaluacionArticuloTabla {
            public string Fecha { get; set; }
            public string Revisor { get; set; }
            public int Calificacion { get; set; }
            public int GradoExpertiz { get; set; }
            public string Estado { get; set; }
        }
    }
}
