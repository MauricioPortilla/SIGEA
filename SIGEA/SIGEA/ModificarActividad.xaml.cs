using SIGEABD;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
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
using static SIGEA.RegistrarActividad;

namespace SIGEA {
    /// <summary>
    /// Lógica de interacción para ModificarActividad.xaml
    /// </summary>
    public partial class ModificarActividad : Window {

        private Actividad actividad;
        public ObservableCollection<PresentacionTabla> PresentacionesObservableCollection { get; } =
            new ObservableCollection<PresentacionTabla>();
        private List<PresentacionTabla> presentacionesSeleccionadas = new List<PresentacionTabla>();

        /// <summary>
        /// Crea una instancia.
        /// </summary>
        /// <param name="id_actividad">Identificador de la Actividad</param>
        public ModificarActividad(int id_actividad) {
            InitializeComponent();
            DataContext = this;
            foreach (string tipoActividad in Sesion.TIPOS_ACTIVIDAD) {
                tipoActividadComboBox.Items.Add(tipoActividad);
            }
            CargarActividad(id_actividad);
        }

        /// <summary>
        /// Carga la Actividad y sus Presentaciones de la base de datos y muestra sus datos.
        /// </summary>
        /// <param name="id_actividad">Identificador de la Actividad</param>
        private void CargarActividad(int id_actividad) {
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    this.actividad = sigeaBD.Actividad.Find(id_actividad);
                    if (actividad == null) {
                        MessageBox.Show("Error al establecer una conexión.");
                        return;
                    }
                    nombreTextBox.Text = actividad.nombre;
                    costoTextBox.Text = float.Parse(actividad.costo.ToString()).ToString();
                    tipoActividadComboBox.SelectedItem = actividad.tipo;
                    descripcionTextBox.Text = actividad.descripcion;
                    foreach (Presentacion presentacion in actividad.Presentacion) {
                        var presentacionExistente = new PresentacionTabla {
                            Presentacion = presentacion,
                            Seleccionado = false,
                            Fecha = presentacion.fechaPresentacion.ToString(),
                            HoraInicio = presentacion.horaInicio,
                            HoraFin = presentacion.horaFin,
                        };
                        presentacionExistente.PropertyChanged += PresentacionTabla_PropertyChanged;
                        PresentacionesObservableCollection.Add(presentacionExistente);
                    }
                }
            } catch (Exception) {
                MessageBox.Show("Error al establecer una conexión.");
            }
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
        private bool VerificarCamposCompletos() {
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
        private bool VerificarDatosValidos() {
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
                        Presentacion = null,
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
        /// Muestra una ventana para modificar una presentación seleccionada de la tabla.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void ModificarPresentacionButton_Click(object sender, RoutedEventArgs e) {
            if (presentacionesSeleccionadas.Count == 0) {
                MessageBox.Show("Debes seleccionar una presentación.");
                return;
            }
            var presentacionSeleccionada = presentacionesSeleccionadas.First();
            ModificarPresentacion modificarPresentacionVentana = new ModificarPresentacion(
                presentacionSeleccionada
            );
            modificarPresentacionVentana.Closing += (windowSender, windowEvent) => {
                var indexPresentacionLista = PresentacionesObservableCollection.ToList().FindIndex(
                    presentacion => presentacion.Fecha == presentacionSeleccionada.Fecha &&
                    presentacion.HoraInicio == presentacionSeleccionada.HoraInicio &&
                    presentacion.HoraFin == presentacionSeleccionada.HoraFin
                );
                var indexPresentacionSeleccionada = presentacionesSeleccionadas.ToList().FindIndex(
                    presentacion => presentacion.Fecha == presentacionSeleccionada.Fecha &&
                    presentacion.HoraInicio == presentacionSeleccionada.HoraInicio &&
                    presentacion.HoraFin == presentacionSeleccionada.HoraFin
                );
                PresentacionesObservableCollection[indexPresentacionLista] = modificarPresentacionVentana.PresentacionTabla;
                presentacionesSeleccionadas[indexPresentacionSeleccionada] = modificarPresentacionVentana.PresentacionTabla;
                presentacionesDataGrid.Items.Refresh();
            };
            modificarPresentacionVentana.Show();
        }

        /// <summary>
        /// Quita las presentaciones seleccionadas de la tabla.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void QuitarPresentacionButton_Click(object sender, RoutedEventArgs e) {
            if (presentacionesSeleccionadas.Count == 0) {
                MessageBox.Show("Debes seleccionar una presentación.");
                return;
            }
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
        /// Verifica que los campos estén completos, que tengan datos válidos y guarda
        /// los cambios realizados en la base de datos.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void GuardarCambiosButton_Click(object sender, RoutedEventArgs e) {
            if (!VerificarCamposCompletos()) {
                MessageBox.Show("Faltan campos por completar.");
                return;
            } else if (!VerificarDatosValidos()) {
                MessageBox.Show("Debes introducir datos válidos.");
                return;
            }
            try {
                actividad.nombre = nombreTextBox.Text;
                actividad.costo = double.Parse(costoTextBox.Text);
                actividad.descripcion = descripcionTextBox.Text;
                actividad.tipo = tipoActividadComboBox.Text;
                Collection<Presentacion> presentaciones = new Collection<Presentacion>();
                foreach (PresentacionTabla presentacionTabla in PresentacionesObservableCollection) {
                    if (presentacionTabla.Presentacion != null) {
                        presentaciones.Add(presentacionTabla.Presentacion);
                        continue;
                    }
                    presentaciones.Add(new Presentacion {
                        fechaPresentacion = Convert.ToDateTime(presentacionTabla.Fecha, new CultureInfo("es-MX")),
                        horaInicio = presentacionTabla.HoraInicio,
                        horaFin = presentacionTabla.HoraFin,
                        id_actividad = actividad.id_actividad
                    });
                }
                actividad.Presentacion = presentaciones;
                if (actividad.Actualizar()) {
                    MessageBox.Show("Se han guardado los cambios.");
                    Close();
                    return;
                }
                MessageBox.Show("Error al establecer una conexión.");
            } catch (Exception) {
                MessageBox.Show("Error al establecer una conexión.");
            }
        }
    }
}
