using SIGEABD;
using System;
using System.Linq;
using System.Windows;

namespace SIGEA {
    /// <summary>
    /// Lógica de interacción para EvaluarArticulo.xaml
    /// </summary>
    public partial class EvaluarArticulo : Window {

        private EvaluacionArticulo evaluacionArticulo;
        private int id_articulo;
        private Articulo articulo;
        private RevisorArticulo revisorArticulo;

        /// <summary>
        /// Crea una instancia.
        /// </summary>
        /// <param name="id_articulo">Identificador del Articulo a evaluar</param>
        public EvaluarArticulo(int id_articulo) {
            InitializeComponent();
            this.id_articulo = id_articulo;
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    articulo = sigeaBD.Articulo.Find(id_articulo);
                    revisorArticulo = sigeaBD.RevisorArticulo.FirstOrDefault(
                        revisorArt => revisorArt.id_revisor == Sesion.Revisor.id_revisor && 
                            revisorArt.id_articulo == articulo.id_articulo
                    );
                    if (revisorArticulo == null) {
                        throw new Exception();
                    }
                }
            } catch (Exception) {
                MessageBox.Show("Error al establecer una conexión.");
                Close();
                return;
            }
            foreach (var gradoExpertiz in Sesion.GRADOS_EXPERTIZ) {
                gradoExpertizComboBox.Items.Add(gradoExpertiz.Value);
            }
            for (int calificacion = Sesion.MIN_CALIFICACION_EVALUACION_ARTICULO; 
                calificacion <= Sesion.MAX_CALIFICACION_EVALUACION_ARTICULO; 
                calificacion++
            ) {
                calificacionComboBox.Items.Add(calificacion);
            }
            VerificarExistenciaEvaluacionArticulo(id_articulo);
        }

        /// <summary>
        /// Muestra el panel principal al cerrarse.
        /// </summary>
        /// <param name="e">Evento</param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e) {
            new PanelRevisor().Show();
        }

        /// <summary>
        /// Verifica si existe una EvaluacionArticulo; si existe, carga los datos.
        /// </summary>
        /// <param name="id_articulo">Identificador del Articulo</param>
        public void VerificarExistenciaEvaluacionArticulo(int id_articulo) {
            try {
                EvaluacionArticulo.ObtenerEvaluacionArticulo(id_articulo, Sesion.Revisor.id_revisor, (evaluacionArticulo) => {
                    this.evaluacionArticulo = evaluacionArticulo;
                    if (evaluacionArticulo.gradoExpertiz == 0) {
                        gradoExpertizComboBox.SelectedIndex = -1;
                    } else {
                        gradoExpertizComboBox.SelectedItem = Sesion.GRADOS_EXPERTIZ[evaluacionArticulo.gradoExpertiz].ToString();
                    }
                    if (evaluacionArticulo.calificacion == -1) {
                        calificacionComboBox.SelectedIndex = -1;
                    } else {
                        calificacionComboBox.SelectedItem = evaluacionArticulo.calificacion;
                    }
                    observacionesTextBox.Text = evaluacionArticulo.observaciones;
                });
            } catch (Exception) {
                MessageBox.Show("Error al establecer una conexión.");
                Close();
            }
        }

        /// <summary>
        /// Cierra la ventana.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento del botón</param>
        private void CancelarButton_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        /// <summary>
        /// Verifica si se cargó una EvaluacionArticulo al principio; si sí,
        /// sobreescribe la información y la actualiza; si no, crea una instancia
        /// de EvaluacionArticulo y la guarda.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento del botón</param>
        private void GuardarButton_Click(object sender, RoutedEventArgs e) {
            try {
                if (evaluacionArticulo == null) {
                    evaluacionArticulo = new EvaluacionArticulo {
                        gradoExpertiz = gradoExpertizComboBox.SelectedIndex + 1,
                        calificacion = calificacionComboBox.SelectedIndex != -1 ? int.Parse(calificacionComboBox.SelectedItem.ToString()) : -1,
                        observaciones = observacionesTextBox.Text,
                        fecha = DateTime.Now,
                        estado = "En proceso",
                        id_revisorArticulo = revisorArticulo.id_revisorArticulo
                    };
                    if (!evaluacionArticulo.Registrar()) {
                        MessageBox.Show("Error al establecer una conexión.");
                        evaluacionArticulo = null;
                        return;
                    }
                } else {
                    evaluacionArticulo.gradoExpertiz = gradoExpertizComboBox.SelectedIndex + 1;
                    evaluacionArticulo.calificacion = calificacionComboBox.SelectedIndex != -1 ? int.Parse(calificacionComboBox.SelectedItem.ToString()) : -1;
                    evaluacionArticulo.observaciones = observacionesTextBox.Text;
                    evaluacionArticulo.fecha = DateTime.Now;
                    if (!evaluacionArticulo.Actualizar()) {
                        MessageBox.Show("Error al establecer una conexión.");
                        return;
                    }
                }
            } catch (Exception) {
                MessageBox.Show("Error al establecer una conexión.");
                return;
            }
            MessageBox.Show("Evaluación guardada.");
        }

        /// <summary>
        /// Verifica que los campos estén completos.
        /// </summary>
        /// <returns>true si están completos; false si no</returns>
        private bool VerificarCamposCompletos() {
            return gradoExpertizComboBox.SelectedIndex != -1 &&
                calificacionComboBox.SelectedIndex != -1 &&
                !string.IsNullOrWhiteSpace(observacionesTextBox.Text);
        }

        /// <summary>
        /// Verifica que los campos estén completos y si se cargó una EvaluacionArticulo
        /// al principio; si se cargó, sobreescribe los datos, cambia el estado a Finalizada
        /// y la actualiza; si no, crea una EvaluacionArticulo con los datos ingresados y con
        /// el estado Finalizada y la guarda.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento del botón</param>
        private void EmitirEvaluacionButton_Click(object sender, RoutedEventArgs e) {
            if (!VerificarCamposCompletos()) {
                MessageBox.Show("Faltan campos por completar.");
                return;
            }
            try {
                if (evaluacionArticulo == null) {
                    evaluacionArticulo = new EvaluacionArticulo {
                        gradoExpertiz = gradoExpertizComboBox.SelectedIndex + 1,
                        calificacion = calificacionComboBox.SelectedIndex != -1 ? int.Parse(calificacionComboBox.SelectedItem.ToString()) : -1,
                        observaciones = observacionesTextBox.Text,
                        fecha = DateTime.Now,
                        estado = "Finalizada",
                        id_revisorArticulo = revisorArticulo.id_revisorArticulo
                    };
                    if (!evaluacionArticulo.Registrar()) {
                        MessageBox.Show("Error al establecer una conexión.");
                        return;
                    }
                } else {
                    evaluacionArticulo.gradoExpertiz = gradoExpertizComboBox.SelectedIndex + 1;
                    evaluacionArticulo.calificacion = calificacionComboBox.SelectedIndex != -1 ? int.Parse(calificacionComboBox.SelectedItem.ToString()) : -1;
                    evaluacionArticulo.observaciones = observacionesTextBox.Text;
                    evaluacionArticulo.fecha = DateTime.Now;
                    evaluacionArticulo.estado = "Finalizada";
                    if (!evaluacionArticulo.Actualizar()) {
                        MessageBox.Show("Error al establecer una conexión.");
                        return;
                    }
                }
            } catch (Exception) {
                MessageBox.Show("Error al establecer una conexión.");
                return;
            }
            MessageBox.Show("Se ha realizado la evaluación.");
            Close();
        }

        /// <summary>
        /// Muestra el artículo.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void VisualizarArticuloButton_Click(object sender, RoutedEventArgs e) {
            System.Diagnostics.Process.Start(App.ARTICULOS_DIRECTORIO + "/" + articulo.archivo);
        }
    }
}
