using SIGEABD;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using static SIGEA.GenerarConstanciasEvento;

namespace SIGEA {
    /// <summary>
    /// Lógica de interacción para RegistrarPagoAsistente.xaml
    /// </summary>
    public partial class RegistrarPagoAsistente : Window {
        public ObservableCollection<AsistenteTabla> AsistentesLista { get; } = new ObservableCollection<AsistenteTabla>();

        /// <summary>
        /// Crea una instancia.
        /// </summary>
        public RegistrarPagoAsistente() {
            InitializeComponent();
            DataContext = this;
            CargarAsistentes();
            CargarActividades();
        }

        /// <summary>
        /// Vuelve al panel principal al cerrarse.
        /// </summary>
        /// <param name="e">Evento</param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e) {
            if (Sesion.Comite != null && Sesion.Evento == null) {
                new PanelLiderComite().Show();
            } else if (Sesion.Evento != null && Sesion.Comite == null) {
                new PanelLiderEvento().Show();
            } else {
                new PanelOrganizador().Show();
            }
        }

        /// <summary>
        /// Carga los asistentes pertenecientes al evento.
        /// </summary>
        private void CargarAsistentes() {
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    var asistentes = sigeaBD.Asistente.Where(
                        asistente => asistente.Evento.FirstOrDefault(
                            evento => evento.id_evento == Sesion.Evento.id_evento
                        ) != null
                    );
                    foreach (var asistente in asistentes) {
                        AsistentesLista.Add(new AsistenteTabla {
                            Asistente = asistente,
                            Nombre = asistente.nombre,
                            Paterno = asistente.paterno,
                            Materno = asistente.materno ?? ""
                        });
                    }
                }
            } catch (Exception) {
                MessageBox.Show("Error al establecer una conexión.");
                Close();
            }
        }

        /// <summary>
        /// Carga las actividades pertenecientes al evento.
        /// </summary>
        private void CargarActividades() {
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    var actividades = sigeaBD.Actividad.Where(actividad => actividad.id_evento == Sesion.Evento.id_evento);
                    foreach (var actividad in actividades) {
                        actividadComboBox.Items.Add(actividad);
                    }
                }
            } catch (Exception) {
                MessageBox.Show("Error al establecer una conexión.");
                Close();
            }
        }

        /// <summary>
        /// Verifica que los campos estén completos.
        /// </summary>
        /// <returns>true si están completos; false si no</returns>
        private bool VerificarCampos() {
            return !string.IsNullOrWhiteSpace(cantidadTextBox.Text) && 
                asistentesListView.SelectedIndex != -1;
        }

        /// <summary>
        /// Verifica que los campos tengan datos válidos.
        /// </summary>
        /// <returns>true si son válidos; false si no</returns>
        private bool ValidarCampos() {
            return Regex.IsMatch(cantidadTextBox.Text, Herramientas.REGEX_SOLO_ENTEROS_Y_FLOTANTES);
        }

        /// <summary>
        /// Verifica si existe un pago en la base de datos. Si se seleccionó una actividad,
        /// busca un pago relacionado a la actividad con el asistente seleccionado; si no, busca
        /// un pago relacionado al evento con el asistente seleccionado.
        /// </summary>
        /// <returns>true si existe el pago; false si no</returns>
        private bool VerificarPagoExistente() {
            try {
                Asistente asistenteSeleccionado = ((AsistenteTabla) asistentesListView.SelectedItem).Asistente;
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    Pago pagoBusqueda = null;
                    if (actividadComboBox.SelectedIndex == -1) {
                        pagoBusqueda = sigeaBD.Pago.FirstOrDefault(
                            pago => pago.id_asistente == asistenteSeleccionado.id_asistente &&
                            pago.id_evento == Sesion.Evento.id_evento
                        );
                    } else {
                        Actividad actividadSeleccionada = actividadComboBox.SelectedItem as Actividad;
                        pagoBusqueda = sigeaBD.Pago.FirstOrDefault(
                            pago => pago.id_asistente == asistenteSeleccionado.id_asistente &&
                            pago.id_actividad == actividadSeleccionada.id_actividad
                        );
                    }
                    return pagoBusqueda != null;
                }
            } catch (Exception) {
                MessageBox.Show("Error al establecer una conexión");
                throw;
            }
        }

        /// <summary>
        /// Registra el pago.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void RegistrarButton_Click(object sender, RoutedEventArgs e) {
            if (!VerificarCampos()) {
                MessageBox.Show("Faltan campos por completar.");
                return;
            } else if (!ValidarCampos()) {
                MessageBox.Show("Debes introducir datos válidos.");
                return;
            }
            try {
                if (VerificarPagoExistente()) {
                    MessageBox.Show("Ya existe un pago de este asistente.");
                    return;
                }
            } catch (Exception) {
                return;
            }
            try {
                Asistente asistenteSeleccionado = ((AsistenteTabla) asistentesListView.SelectedItem).Asistente;
                int? id_actividad = null;
                int? id_evento = null;
                if (actividadComboBox.SelectedIndex != -1) {
                    Actividad actividadSeleccionada = actividadComboBox.SelectedItem as Actividad;
                    id_actividad = actividadSeleccionada.id_actividad;
                } else {
                    id_evento = Sesion.Evento.id_evento;
                }
                Pago pago = new Pago {
                    cantidad = float.Parse(cantidadTextBox.Text),
                    id_asistente = asistenteSeleccionado.id_asistente,
                    id_actividad = id_actividad,
                    id_evento = id_evento,
                    fecha = DateTime.Now
                };
                if (pago.Registrar()) {
                    MessageBox.Show("Pago registrado con éxito.");
                    Close();
                    return;
                }
                MessageBox.Show("Error al establecer una conexión.");
            } catch (Exception) {
                MessageBox.Show("Error al establecer una conexión.");
            }
        }

        /// <summary>
        /// Cierra la ventana.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void CancelarButton_Click(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}
