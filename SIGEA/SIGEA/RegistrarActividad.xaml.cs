using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Lógica de interacción para RegistrarActividad.xaml
    /// </summary>
    public partial class RegistrarActividad : Window {
        private Evento evento;
        private ObservableCollection<PresentacionTabla> PresentacionesObservableCollection { get; } =
            new ObservableCollection<PresentacionTabla>();

        /// <summary>
        /// Crea la instancia.
        /// </summary>
        public RegistrarActividad(Evento evento) {
            InitializeComponent();
            this.evento = evento;
            foreach (string tipoActividad in Sesion.TiposActividad) {
                tipoActividadComboBox.Items.Add(tipoActividad);
            }
        }

        /// <summary>
        /// Verifica que los campos estén completos.
        /// </summary>
        /// <returns>true si están completos; false si no</returns>
        private bool VerificarCampos() {
            return !string.IsNullOrWhiteSpace(nombreTextBox.Text) &&
                !string.IsNullOrWhiteSpace(costoTextBox.Text) &&
                tipoActividadComboBox.SelectedIndex != -1 &&
                !string.IsNullOrWhiteSpace(descripcionTextBox.Text) &&
                PresentacionesObservableCollection.Count > 0;
        }

        /// <summary>
        /// Verifica que los campos tengan datos válidos.
        /// </summary>
        /// <returns>true si tienen datos válidos; false si no</returns>
        private bool VerificarDatos() {
            return Regex.IsMatch(costoTextBox.Text, Herramientas.REGEX_SOLO_ENTEROS_Y_FLOTANTES);
        }

        /// <summary>
        /// Muestra una ventana para añadir una nueva presentación.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void AñadirPresentacionButton_Click(object sender, RoutedEventArgs e) {
            AgregarPresentacion agregarPresentacionWindow = new AgregarPresentacion();
            agregarPresentacionWindow.Closing += (windowSender, windowEvent) => {
                if (agregarPresentacionWindow.Presentacion != null) {
                    PresentacionesObservableCollection.Add(new PresentacionTabla {
                        Seleccionado = false,
                        Fecha = agregarPresentacionWindow.Presentacion.fechaPresentacion,
                        HoraInicio = agregarPresentacionWindow.Presentacion.horaInicio,
                        HoraFin = agregarPresentacionWindow.Presentacion.horaFin
                    });
                    presentacionesDataGrid.Items.Refresh();
                }
            };
        }

        /// <summary>
        /// Quita las presentaciones seleccionadas de la tabla.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void QuitarPresentacionButton_Click(object sender, RoutedEventArgs e) {
            List<PresentacionTabla> presentacionesEliminadas = new List<PresentacionTabla>();
            foreach (PresentacionTabla presentacionTabla in PresentacionesObservableCollection) {
                if (presentacionTabla.Seleccionado) {
                    presentacionesEliminadas.Add(presentacionTabla);
                }
            }
            foreach (PresentacionTabla presentacionTabla in presentacionesEliminadas) {
                PresentacionesObservableCollection.Remove(presentacionTabla);
            }
            presentacionesDataGrid.Items.Refresh();
        }

        /// <summary>
        /// Verifica que los campos estén completos, que tengan datos válidos y
        /// registra la actividad en la base de datos.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void RegistrarButton_Click(object sender, RoutedEventArgs e) {
            if (!VerificarCampos()) {
                MessageBox.Show("Faltan campos por completar.");
                return;
            } else if (!VerificarDatos()) {
                MessageBox.Show("Debes introducir datos válidos.");
                return;
            }
            try {
                if (new Actividad {
                        nombre = nombreTextBox.Text,
                        descripcion = descripcionTextBox.Text,
                        tipo = tipoActividadComboBox.Text,
                        Evento = evento
                    }.Registrar()
                ) {
                    MessageBox.Show("Actividad registrada.");
                    return;
                }
                MessageBox.Show("Error al registrar el artículo.");
            } catch (DbUpdateException dbUpdateException) {
                MessageBox.Show("Error al registrar el artículo.");
                Console.WriteLine("DbUpdateException@RegistrarActividad->RegistrarButton_Click() -> " + dbUpdateException.Message);
            } catch (EntityException entityException) {
                MessageBox.Show("Error al registrar el artículo.");
                Console.WriteLine("EntityException@RegistrarActividad->RegistrarButton_Click() -> " + entityException.Message);
            } catch (Exception exception) {
                MessageBox.Show("Error al registrar el artículo.");
                Console.WriteLine("Exception@RegistrarActividad->RegistrarButton_Click() -> " + exception.Message);
            }
        }

        /// <summary>
        /// Representa una Presentación en una tabla.
        /// </summary>
        public struct PresentacionTabla {
            public bool Seleccionado { get; set; }
            public DateTime Fecha { get; set; }
            public TimeSpan HoraInicio { get; set; }
            public TimeSpan HoraFin { get; set; }
        }
    }
}
