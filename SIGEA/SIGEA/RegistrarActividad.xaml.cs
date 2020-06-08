using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using SIGEABD;

namespace SIGEA {
    /// <summary>
    /// Lógica de interacción para RegistrarActividad.xaml
    /// </summary>
    public partial class RegistrarActividad : Window {

        private Evento evento;
        public ObservableCollection<PresentacionTabla> PresentacionesObservableCollection { get; } =
            new ObservableCollection<PresentacionTabla>();
        private List<PresentacionTabla> presentacionesSeleccionadas = new List<PresentacionTabla>();

        /// <summary>
        /// Crea la instancia.
        /// </summary>
        public RegistrarActividad() {
            InitializeComponent();
            DataContext = this;
            this.evento = Sesion.Evento;
            foreach (string tipoActividad in Sesion.TIPOS_ACTIVIDAD) {
                tipoActividadComboBox.Items.Add(tipoActividad);
            }
        }

        /// <summary>
        /// Muestra el panel principal al cerrarse.
        /// </summary>
        /// <param name="e">Evento</param>
        protected override void OnClosing(CancelEventArgs e) {
            new PanelLiderEvento().Show();
        }

        /// <summary>
        /// Si se selecciona alguna Presentacion, se añade a la lista de autores seleccionados.
        /// </summary>
        /// <param name="sender">AutorTabla</param>
        /// <param name="e">Evento</param>
        public void PresentacionTabla_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            var presentacionSeleccion = (PresentacionTabla) sender;
            if (presentacionSeleccion.Seleccionado) {
                if (!presentacionesSeleccionadas.Exists(presentacionLista => presentacionLista.Presentacion == presentacionSeleccion.Presentacion)) {
                    presentacionesSeleccionadas.Add(presentacionSeleccion);
                }
            } else {
                if (presentacionesSeleccionadas.Exists(presentacionLista => presentacionLista.Presentacion == presentacionSeleccion.Presentacion)) {
                    presentacionesSeleccionadas.RemoveAll(presentacionLista => presentacionLista.Presentacion == presentacionSeleccion.Presentacion);
                }
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
            AgregarPresentacion agregarPresentacionVentana = new AgregarPresentacion();
            agregarPresentacionVentana.Closing += (windowSender, windowEvent) => {
                if (agregarPresentacionVentana.Presentacion != null) {
                    var presentacion = new PresentacionTabla {
                        Seleccionado = false,
                        Fecha = agregarPresentacionVentana.Presentacion.fechaPresentacion.ToString(),
                        HoraInicio = agregarPresentacionVentana.Presentacion.horaInicio,
                        HoraFin = agregarPresentacionVentana.Presentacion.horaFin
                    };
                    presentacion.PropertyChanged += PresentacionTabla_PropertyChanged;
                    PresentacionesObservableCollection.Add(presentacion);
                    presentacionesDataGrid.Items.Refresh();
                }
            };
            agregarPresentacionVentana.Show();
        }

        /// <summary>
        /// Quita las presentaciones seleccionadas de la tabla.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void QuitarPresentacionButton_Click(object sender, RoutedEventArgs e) {
            foreach (PresentacionTabla presentacionTabla in presentacionesSeleccionadas) {
                PresentacionesObservableCollection.Remove(
                    PresentacionesObservableCollection.First(
                        presentacion => presentacion.Fecha == presentacionTabla.Fecha && 
                        presentacion.HoraInicio == presentacionTabla.HoraInicio &&
                        presentacion.HoraFin == presentacionTabla.HoraFin
                    )
                );
            }
            presentacionesSeleccionadas.Clear();
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
                Collection<Presentacion> presentaciones = new Collection<Presentacion>();
                foreach (PresentacionTabla presentacionTabla in PresentacionesObservableCollection) {
                    presentaciones.Add(new Presentacion {
                        fechaPresentacion = Convert.ToDateTime(presentacionTabla.Fecha, new CultureInfo("es-MX")),
                        horaInicio = presentacionTabla.HoraInicio,
                        horaFin = presentacionTabla.HoraFin
                    });
                }
                if (new Actividad {
                        nombre = nombreTextBox.Text,
                        descripcion = descripcionTextBox.Text,
                        tipo = tipoActividadComboBox.Text,
                        costo = double.Parse(costoTextBox.Text),
                        Evento = evento,
                        Presentacion = presentaciones
                    }.Registrar()
                ) {
                    MessageBox.Show("Actividad registrada.");
                    Close();
                    return;
                }
                MessageBox.Show("Error al registrar la actividad.");
            } catch (DbUpdateException dbUpdateException) {
                MessageBox.Show("Error al registrar la actividad.");
                Console.WriteLine("DbUpdateException@RegistrarActividad->RegistrarButton_Click() -> " + dbUpdateException.Message);
            } catch (EntityException entityException) {
                MessageBox.Show("Error al registrar la actividad.");
                Console.WriteLine("EntityException@RegistrarActividad->RegistrarButton_Click() -> " + entityException.Message);
            } catch (Exception exception) {
                MessageBox.Show("Error al registrar la actividad.");
                Console.WriteLine("Exception@RegistrarActividad->RegistrarButton_Click() -> " + exception.Message);
            }
        }

        /// <summary>
        /// Representa una Presentación en una tabla.
        /// </summary>
        public struct PresentacionTabla {
            public Presentacion Presentacion;
            private bool seleccionado;
            public bool Seleccionado {
                get {
                    return seleccionado;
                }
                set {
                    seleccionado = value;
                    NotifyPropertyChanged("Seleccionado");
                }
            }
            private DateTime fecha;
            public string Fecha {
                get {
                    return fecha.ToString("dd/MM/yyyy");
                }
                set {
                    fecha = Convert.ToDateTime(value, new CultureInfo("es-MX"));
                }
            }
            public TimeSpan HoraInicio { get; set; }
            public TimeSpan HoraFin { get; set; }
            public event PropertyChangedEventHandler PropertyChanged;
            public void NotifyPropertyChanged(string obj) {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(obj));
            }
        }
    }
}
