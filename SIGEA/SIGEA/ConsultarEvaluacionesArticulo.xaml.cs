using SIGEABD;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

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

        /// <summary>
        /// Crea una instancia.
        /// </summary>
        /// <param name="articulo">Artículo a consultar evaluaciones</param>
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

        protected override void OnClosing(CancelEventArgs e) {
            new ConsultarEvaluacionesArticulos().Show();
        }

        /// <summary>
        /// Carga las evaluaciones artículo del artículo.
        /// </summary>
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

        /// <summary>
        /// Cambia el estado del artículo a Aceptado y guarda los cambios.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
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

        /// <summary>
        /// Cambia el estado del artículo a Requiere actualizarse y guarda los cambios.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
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

        /// <summary>
        /// Representa una evaluación artículo en una tabla.
        /// </summary>
        public struct EvaluacionArticuloTabla {
            public string Fecha { get; set; }
            public string Revisor { get; set; }
            public int Calificacion { get; set; }
            public int GradoExpertiz { get; set; }
            public string Estado { get; set; }
        }
    }
}
