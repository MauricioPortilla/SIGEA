using SIGEABD;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;

namespace SIGEA {
    public partial class MenuPrincipal : Window {

        public ObservableCollection<EventoTabla> EventosObservableCollection { get; } =
                new ObservableCollection<EventoTabla>();

        public MenuPrincipal () {
            InitializeComponent();
            DataContext = this;
            CargarDataGrid();
        }

        /// <summary>
        /// Metodo de acción para cerrar la ventana y regresar añ inicio de sesión
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CerrarButton_Click (object sender, RoutedEventArgs e) {
            IniciarSesion inicioSesion = new IniciarSesion();
            inicioSesion.Show();
            this.Close();
        }

        /// <summary>
        /// Metodo de acción que muestra la ventana de registro del evento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CrearButton_Click (object sender, RoutedEventArgs e) {
            RegistrarEvento registroEvento = new RegistrarEvento();
            registroEvento.Show();
            this.Close();
        }

        /// <summary>
        /// Metodo que optiene los Eventos a los que esta registrado el organizador
        /// </summary>
        public void CargarDataGrid () {
            using (SigeaBD sigeaBD = new SigeaBD()) {
                try {
                    var eventosObtenidos = (from evento in sigeaBD.Evento
                                            where evento.Organizador.id_organizador ==
                                            Sesion.Organizador.id_organizador
                                            select evento).ToList();
                    foreach (Evento evento in eventosObtenidos) {
                        EventosObservableCollection.Add(new EventoTabla {
                            nombre = evento.nombre,
                            sede = evento.sede,
                            fechaInicio = evento.fechaInicio.Date,
                            fechaFin = evento.fechaFin.Date
                        });
                    }
                    var eventosObtenidos2 = (from comite in sigeaBD.Comite
                                             where comite.Organizador.id_organizador ==
                                             Sesion.Organizador.id_organizador
                                             select comite.Evento).ToList();
                    foreach (Evento evento in eventosObtenidos2) {
                        EventosObservableCollection.Add(new EventoTabla {
                            nombre = evento.nombre,
                            sede = evento.sede,
                            fechaInicio = evento.fechaInicio.Date,
                            fechaFin = evento.fechaFin.Date
                        });
                    }
                } catch (EntityException) {
                    MessageBox.Show("Error al cargar los proveedores.");
                } catch (Exception) {
                    MessageBox.Show("Error al cargar los proveedores.");
                }
            }
        }

        /// <summary>
        /// Estruct para llenar de información la tabla de eventos
        /// </summary>
        public struct EventoTabla {
            public String nombre { get; set; }
            public String sede { get; set; }
            public DateTime fechaInicio { get; set; }
            public DateTime fechaFin { get; set; }
        }

        private void PruebaButton_Click (object sender, RoutedEventArgs e) {
            if (eventosDataGrid.SelectedItem != null) {
                var eventoSeleccionado = (EventoTabla) eventosDataGrid.SelectedItem;
                try {
                    using (SigeaBD sigeaBD = new SigeaBD()) {
                        var eventoOptenido = sigeaBD.Evento.ToList().Find(
                            evento => evento.nombre ==
                            eventoSeleccionado.nombre);
                        Sesion.Evento = eventoOptenido;
                        new RegistrarComite().Show();
                        this.Close();
                    }
                } catch (Exception excepcion) {

                }
            } else {
                MessageBox.Show("Por favor seleccione un evento primero");
            }
        }
    }
}
