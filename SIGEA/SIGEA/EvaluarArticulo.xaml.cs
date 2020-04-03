using SIGEABD;
using System;
using System.Collections.Generic;
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
    /// Lógica de interacción para EvaluarArticulo.xaml
    /// </summary>
    public partial class EvaluarArticulo : Window {
        private EvaluacionArticulo evaluacionArticulo;
        private int id_articulo;

        public EvaluarArticulo(int id_articulo) {
            InitializeComponent();
            this.id_articulo = id_articulo;
            foreach (var gradoExpertiz in Sesion.GRADOS_EXPERTIZ) {
                gradoExpertizComboBox.Items.Add(gradoExpertiz.Value);
            }
            for (int calificacion = 1; calificacion <= 3; calificacion++) {
                calificacionComboBox.Items.Add(calificacion);
            }
            VerificarExistenciaEvaluacionArticulo(id_articulo);
        }

        public void VerificarExistenciaEvaluacionArticulo(int id_articulo) {
            try {
                EvaluacionArticulo.ObtenerEvaluacionArticulo(id_articulo, (evaluacionArticulo) => {
                    this.evaluacionArticulo = evaluacionArticulo;
                    gradoExpertizComboBox.SelectedItem = Sesion.GRADOS_EXPERTIZ[evaluacionArticulo.gradoExpertiz].ToString();
                    calificacionComboBox.SelectedItem = evaluacionArticulo.calificacion;
                    observacionesTextBox.Text = evaluacionArticulo.observaciones;
                });
            } catch (Exception) {
                MessageBox.Show("Ocurrió un error al cargar la evaluación.");
                Close();
            }
        }

        private void CancelarButton_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        private void GuardarButton_Click(object sender, RoutedEventArgs e) {
            if (evaluacionArticulo == null) {
                evaluacionArticulo = new EvaluacionArticulo {
                    gradoExpertiz = gradoExpertizComboBox.SelectedIndex + 1,
                    calificacion = int.Parse(calificacionComboBox.SelectedItem.ToString()),
                    observaciones = observacionesTextBox.Text,
                    fecha = DateTime.Now,
                    estado = "En proceso"
                };
                if (!evaluacionArticulo.Registrar(Sesion.Revisor.id_revisor, id_articulo)) {
                    MessageBox.Show("Ocurrió un error al realizar la evaluación.");
                    return;
                }
            } else {
                evaluacionArticulo.gradoExpertiz = gradoExpertizComboBox.SelectedIndex + 1;
                evaluacionArticulo.calificacion = int.Parse(calificacionComboBox.SelectedItem.ToString());
                evaluacionArticulo.observaciones = observacionesTextBox.Text;
                evaluacionArticulo.fecha = DateTime.Now;
                if (!evaluacionArticulo.Actualizar()) {
                    MessageBox.Show("Ocurrió un error al realizar la evaluación.");
                    return;
                }
            }
            MessageBox.Show("Evaluación guardada.");
        }

        private bool VerificarCamposCompletos() {
            return gradoExpertizComboBox.SelectedIndex != -1 &&
                calificacionComboBox.SelectedIndex != -1 &&
                !string.IsNullOrWhiteSpace(observacionesTextBox.Text);
        }

        private void EmitirEvaluacionButton_Click(object sender, RoutedEventArgs e) {
            if (!VerificarCamposCompletos()) {
                MessageBox.Show("Faltan campos por completar.");
                return;
            }
            if (evaluacionArticulo == null) {
                evaluacionArticulo = new EvaluacionArticulo {
                    gradoExpertiz = gradoExpertizComboBox.SelectedIndex + 1,
                    calificacion = int.Parse(calificacionComboBox.SelectedItem.ToString()),
                    observaciones = observacionesTextBox.Text,
                    fecha = DateTime.Now,
                    estado = "Finalizada"
                };
                if (!evaluacionArticulo.Registrar(Sesion.Revisor.id_revisor, id_articulo)) {
                    MessageBox.Show("Ocurrió un error al realizar la evaluación.");
                    return;
                }
            } else {
                evaluacionArticulo.gradoExpertiz = gradoExpertizComboBox.SelectedIndex + 1;
                evaluacionArticulo.calificacion = int.Parse(calificacionComboBox.SelectedItem.ToString());
                evaluacionArticulo.observaciones = observacionesTextBox.Text;
                evaluacionArticulo.fecha = DateTime.Now;
                evaluacionArticulo.estado = "Finalizada";
                if (!evaluacionArticulo.Actualizar()) {
                    MessageBox.Show("Ocurrió un error al realizar la evaluación.");
                    return;
                }
            }
            MessageBox.Show("Se ha realizado la evaluación.");
            Close();
        }
    }
}
