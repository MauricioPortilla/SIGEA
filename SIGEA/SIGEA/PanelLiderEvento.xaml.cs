using SIGEABD;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using static SIGEA.RegistrarAsistente;

namespace SIGEA {
    /// <summary>
    /// Lógica de interacción para PanelLiderEvento.xaml
    /// </summary>
    public partial class PanelLiderEvento : Window {
        private bool mostrarMenuPrincipal = true;
        public ObservableCollection<ActividadTabla> ActividadesLista { get; } = new ObservableCollection<ActividadTabla>();

        /// <summary>
        /// Crea una instancia.
        /// </summary>
        public PanelLiderEvento() {
            InitializeComponent();
            DataContext = this;
            CargarTabla();
        }

        /// <summary>
        /// Muestra el menú principal al cerrarse.
        /// </summary>
        /// <param name="e">Evento</param>
        protected override void OnClosing(CancelEventArgs e) {
            if (mostrarMenuPrincipal) {
                new MenuPrincipal().Show();
            }
        }

        /// <summary>
        /// Método que carga la tabla de actividades.
        /// </summary>
        public void CargarTabla() {
            using (SigeaBD sigeaBD = new SigeaBD()) {
                var listaActividades = from actividad in sigeaBD.Actividad.AsNoTracking()
                                       where actividad.id_evento == Sesion.Evento.id_evento
                                       select actividad;
                foreach (Actividad actividad in listaActividades) {
                    ActividadesLista.Add(new ActividadTabla {
                        Actividad = actividad,
                        Nombre = actividad.nombre,
                        Tipo = actividad.tipo,
                        Descripcion = actividad.descripcion
                    });
                }
            }
        }

        /// <summary>
        /// Cierra la ventana.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void RegresarButton_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        /// <summary>
        /// Muestra la ventana de Registrar Comité.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void RegistrarComiteButton_Click(object sender, RoutedEventArgs e) {
            mostrarMenuPrincipal = false;
            new RegistrarComite().Show();
            Close();
        }

        /// <summary>
        /// Muestra la ventana de Registrar Actividad.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void RegistrarActividadButton_Click(object sender, RoutedEventArgs e) {
            mostrarMenuPrincipal = false;
            new RegistrarActividad().Show();
            Close();
        }

        /// <summary>
        /// Muestra la ventana de Registrar Track.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void RegistrarTrackButton_Click(object sender, RoutedEventArgs e) {
            mostrarMenuPrincipal = false;
            new RegistrarTrack().Show();
            Close();
        }

        /// <summary>
        /// Muestra la ventana de Generar Reporte de Ingresos de Actividad.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void GenerarReporteIngresosButton_Click(object sender, RoutedEventArgs e) {
            if (actividadesListView.SelectedItem != null) {
                var actividad = (ActividadTabla) actividadesListView.SelectedItem;
                mostrarMenuPrincipal = false;
                new GenerarReporteIngresosActividad(actividad.Actividad).Show();
                Close();
            } else {
                MessageBox.Show("Seleccione una actividad");
            }
        }

        /// <summary>
        /// Muestra la ventana de Modificar Actividad.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void ModificarActividadButton_Click(object sender, RoutedEventArgs e) {
            if (actividadesListView.SelectedItem != null) {
                var actividad = (ActividadTabla) actividadesListView.SelectedItem;
                mostrarMenuPrincipal = false;
                new ModificarActividad(actividad.Actividad.id_actividad).Show();
                Close();
            } else {
                MessageBox.Show("Seleccione una actividad");
            }
        }

        /// <summary>
        /// Muestra la ventana de Asignar Artículos a Revisor.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void AsignarArticulosRevisorButton_Click(object sender, RoutedEventArgs e) {
            mostrarMenuPrincipal = false;
            new AsignarArticulo().Show();
            this.Close();
        }

        /// <summary>
        /// Muestra la ventana de Generar Programa del Evento.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void GenerarProgramaEventoButton_Click(object sender, RoutedEventArgs e) {
            mostrarMenuPrincipal = false;
            new GenerarProgramaEvento(Sesion.Evento).Show();
            Close();
        }

        /// <summary>
        /// Muestra la ventana de Generar Constancias de Evento.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void GenerarConstanciasEventoButton_Click(object sender, RoutedEventArgs e) {
            mostrarMenuPrincipal = false;
            new GenerarConstanciasEvento(Sesion.Evento.id_evento).Show();
            Close();
        }

        /// <summary>
        /// Muestra la ventana de Consultar Asistentes de Actividad.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void ConsultarAsistentesActividadButton_Click(object sender, RoutedEventArgs e) {
            mostrarMenuPrincipal = false;
            if (actividadesListView.SelectedItem != null) {
                var actividad = (ActividadTabla) actividadesListView.SelectedItem;
                mostrarMenuPrincipal = false;
                new ConsultarAsistentesActividad(actividad.Actividad).Show();
                Close();
            } else {
                MessageBox.Show("Seleccione una actividad");
            }
        }

        /// <summary>
        /// Muestra la ventana de Consultar Asistentes de Evento.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void ConsultarAsistentesEventoButton_Click(object sender, RoutedEventArgs e) {
            mostrarMenuPrincipal = false;
        }

        /// <summary>
        /// Muestra la ventana de Generar Constancias de Actividad.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void GenerarConstanciasActividadButton_Click(object sender, RoutedEventArgs e) {
            if (actividadesListView.SelectedItem != null) {
                var actividad = (ActividadTabla) actividadesListView.SelectedItem;
                mostrarMenuPrincipal = false;
                new GenerarConstanciasActividad(actividad.Actividad.id_actividad).Show();
                Close();
            } else {
                MessageBox.Show("Seleccione una actividad");
            }
        }

        /// <summary>
        /// Muestra la ventana de Registrar Pago de Asistente.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void RegistrarPagoAsistenteButton_Click(object sender, RoutedEventArgs e) {
            mostrarMenuPrincipal = false;
            new RegistrarPagoAsistente().Show();
            Close();
        }

        /// <summary>
        /// Muestra la ventana de Registrar Gasto.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void RegistrarGastoButton_Click(object sender, RoutedEventArgs e) {
            mostrarMenuPrincipal = false;
            new RegistrarGasto().Show();
            Close();
        }

        /// <summary>
        /// Muestra la ventana de Consultar Gastos.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void ConsultarGastosButton_Click(object sender, RoutedEventArgs e) {
            mostrarMenuPrincipal = false;
            new ConsultarGastos().Show();
            Close();
        }

        /// <summary>
        /// Muestra la ventana de Consultar Evaluaciones de Artículos.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void ConsultarEvaluacionesArticuloButton_Click(object sender, RoutedEventArgs e) {
            mostrarMenuPrincipal = false;
            new ConsultarEvaluacionesArticulos().Show();
            Close();
        }
    }
}
