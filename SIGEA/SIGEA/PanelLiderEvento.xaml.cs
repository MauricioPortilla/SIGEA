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

        public PanelLiderEvento() {
            InitializeComponent();
            DataContext = this;
            CargarTabla();
        }

        protected override void OnClosing(CancelEventArgs e) {
            if (mostrarMenuPrincipal) {
                new MenuPrincipal().Show();
            }
        }

        /// <summary>
        /// Método que carga la tabla de actividades
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

        private void RegresarButton_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        private void RegistrarComiteButton_Click(object sender, RoutedEventArgs e) {
            mostrarMenuPrincipal = false;
            new RegistrarComite().Show();
            this.Close();
        }

        private void RegistrarActividadButton_Click(object sender, RoutedEventArgs e) {
            mostrarMenuPrincipal = false;
            new RegistrarActividad().Show();
            this.Close();
        }

        private void RegistrarTrackButton_Click(object sender, RoutedEventArgs e) {
            mostrarMenuPrincipal = false;
            new RegistrarTrack().Show();
            this.Close();
        }

        private void GenerarReporteIngresosButton_Click(object sender, RoutedEventArgs e) {
            if (actividadesListView.SelectedItem != null) {
                var actividad = (ActividadTabla) actividadesListView.SelectedItem;
                mostrarMenuPrincipal = false;
                new GenerarReporteIngresosActividad(actividad.Actividad).Show();
                this.Close();
            } else {
                MessageBox.Show("Seleccione una actividad");
            }
        }

        private void ModificarActividadButton_Click(object sender, RoutedEventArgs e) {
            if (actividadesListView.SelectedItem != null) {
                var actividad = (ActividadTabla) actividadesListView.SelectedItem;
                mostrarMenuPrincipal = false;
                new ModificarActividad(actividad.Actividad.id_actividad).Show();
                this.Close();
            } else {
                MessageBox.Show("Seleccione una actividad");
            }
        }

        private void AsignarArticulosRevisorButton_Click(object sender, RoutedEventArgs e) {
            mostrarMenuPrincipal = false;
        }

        private void GenerarProgramaEventoButton_Click(object sender, RoutedEventArgs e) {
            mostrarMenuPrincipal = false;
            new GenerarProgramaEvento(Sesion.Evento).Show();
            Close();
        }

        private void GenerarConstanciasEventoButton_Click(object sender, RoutedEventArgs e) {
            mostrarMenuPrincipal = false;
            new GenerarConstanciasEvento(Sesion.Evento.id_evento).Show();
            Close();
        }

        private void ConsultarAsistentesActividadButton_Click(object sender, RoutedEventArgs e) {
            mostrarMenuPrincipal = false;
            if (actividadesListView.SelectedItem != null) {
                var actividad = (ActividadTabla) actividadesListView.SelectedItem;
                mostrarMenuPrincipal = false;
                new ConsultarAsistentesActividad(actividad.Actividad).Show();
                this.Close();
            } else {
                MessageBox.Show("Seleccione una actividad");
            }
        }

        private void ConsultarAsistentesEventoButton_Click(object sender, RoutedEventArgs e) {
            mostrarMenuPrincipal = false;
        }

        private void GenerarConstanciasActividadButton_Click(object sender, RoutedEventArgs e) {
            mostrarMenuPrincipal = false;
        }

        private void RegistrarPagoAsistenteButton_Click(object sender, RoutedEventArgs e) {
            mostrarMenuPrincipal = false;
            new RegistrarPagoAsistente().Show();
            Close();
        }

        private void RegistrarGastoButton_Click(object sender, RoutedEventArgs e) {
            mostrarMenuPrincipal = false;
            new RegistrarGasto().Show();
            Close();
        }

        private void ConsultarGastosButton_Click(object sender, RoutedEventArgs e) {
            mostrarMenuPrincipal = false;
            new ConsultarGastos().Show();
            Close();
        }
    }
}
