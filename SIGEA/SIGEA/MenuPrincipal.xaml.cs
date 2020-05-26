using SIGEABD;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;

namespace SIGEA {
    public partial class MenuPrincipal : Window {

        public ObservableCollection<EventoTabla> EventosLista { get; } = new ObservableCollection<EventoTabla>();

        /// <summary>
        /// Crea una instancia.
        /// </summary>
        public MenuPrincipal() {
            InitializeComponent();
            DataContext = this;
            CargarTabla();
        }

        public struct EventoTabla {
            public Evento Evento;
            public string Nombre { get; set; }
            public string Sede { get; set; }
            public string FechaInicio { get; set; }
            public string FechaFin { get; set; }
        }

        /// <summary>
        /// Metodo de acción para cerrar la ventana y regresar añ inicio de sesión
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void CerrarButton_Click(object sender, RoutedEventArgs e) {
            IniciarSesion inicioSesion = new IniciarSesion();
            inicioSesion.Show();
            this.Close();
        }

        /// <summary>
        /// Metodo de acción que muestra la ventana de registro del evento
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void CrearButton_Click(object sender, RoutedEventArgs e) {
            RegistrarEvento registroEvento = new RegistrarEvento();
            registroEvento.Show();
            this.Close();
        }

        /// <summary>
        /// Metodo que optiene los Eventos a los que esta registrado el organizador
        /// </summary>
        public void CargarTabla() {
            using (SigeaBD sigeaBD = new SigeaBD()) {
                var listaEventos = sigeaBD.Evento.AsNoTracking().Where(
                    evento => evento.id_organizador == Sesion.Organizador.id_organizador
                );

                foreach (Evento evento in listaEventos) {
                    EventosLista.Add(new EventoTabla {
                        Evento = evento,
                        Nombre = evento.nombre,
                        Sede = evento.sede,
                        FechaInicio = evento.fechaInicio.ToShortDateString(),
                        FechaFin = evento.fechaFin.Date.ToShortDateString()
                    });
                }

                var listaEventos2 = sigeaBD.Evento.AsNoTracking().Where(evento =>
                    evento.Comite.Where(comite =>
                        comite.id_organizador == Sesion.Organizador.id_organizador
                    ).Count() != 0
                );

                foreach (Evento evento in listaEventos2) {
                    EventosLista.Add(new EventoTabla {
                        Evento = evento,
                        Nombre = evento.nombre,
                        Sede = evento.sede,
                        FechaInicio = evento.fechaInicio.ToShortDateString(),
                        FechaFin = evento.fechaFin.Date.ToShortDateString()
                    });
                }
            }
        }

        private void AdministrarButton_Click(object sender, RoutedEventArgs e) {
            if (eventosListView.SelectedItem != null) {
                var eventoSeleccionado = (EventoTabla) eventosListView.SelectedItem;
                try {
                    using (SigeaBD sigeaBD = new SigeaBD()) {
                        var eventoEncontrado = sigeaBD.Evento.AsNoTracking().Where(
                            evento => evento.id_evento == eventoSeleccionado.Evento.id_evento
                        ).First();
                        if (eventoEncontrado != null) {
                            Sesion.Evento = eventoEncontrado;
                        } else {
                            throw new Exception();
                        }
                    }
                    new Actividades().Show();
                    this.Close();
                } catch (Exception ex) {
                    Console.WriteLine(ex);
                    MessageBox.Show("Error al seleccionar el evento.");
                }
            } else {
                MessageBox.Show("Selecciona un evento de la tabla.");
            }
        }
    }
}
