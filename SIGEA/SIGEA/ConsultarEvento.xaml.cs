using SIGEABD;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace SIGEA {
    /// <summary>
    /// Lógica de interacción para ConsultarEvento.xaml
    /// </summary>
    public partial class ConsultarEvento : Window {
        public ObservableCollection<ComiteTabla> ComitesLista { get; } = new ObservableCollection<ComiteTabla>();
        private int id_evento;

        /// <summary>
        /// Crea una instancia.
        /// </summary>
        public ConsultarEvento(int id_evento) {
            InitializeComponent();
            DataContext = this;
            this.id_evento = id_evento;
            CargarEvento();
            CargarComites();
        }

        /// <summary>
        /// Muestra el menú principal al cerrarse.
        /// </summary>
        /// <param name="e">Evento</param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e) {
            new MenuPrincipal().Show();
        }

        /// <summary>
        /// Representa a un comité en una tabla.
        /// </summary>
        public struct ComiteTabla {
            public Comite Comite;
            public string Nombre { get; set; }
            public string Lider { get; set; }
        }

        /// <summary>
        /// Carga el evento de la base de datos.
        /// </summary>
        private void CargarEvento() {
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    var evento = sigeaBD.Evento.Find(id_evento);
                    if (evento == null) {
                        MessageBox.Show("Error al establecer una conexión.");
                        return;
                    }
                    nombreEvento.Text = evento.nombre;
                    fechaInicioEvento.Text = evento.fechaInicio.ToShortDateString();
                    fechaFinEvento.Text = evento.fechaFin.ToShortDateString();
                    sedeEvento.Text = evento.sede;
                    cuotaEvento.Text = evento.cuota.ToString();
                }
            } catch (Exception) {
                MessageBox.Show("Error al establecer una conexión.");
            }
        }

        /// <summary>
        /// Carga los comités del evento de la base de datos.
        /// </summary>
        private void CargarComites() {
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    var comites = sigeaBD.Comite.Where(comite => comite.Evento.id_evento == id_evento);
                    foreach (var comite in comites) {
                        ComitesLista.Add(new ComiteTabla {
                            Comite = comite,
                            Nombre = comite.nombre,
                            Lider = comite.Organizador.nombre + " " + comite.Organizador.paterno + " " + (comite.Organizador.materno ?? "")
                        });
                    }
                }
            } catch (Exception) {
                MessageBox.Show("Error al establecer una conexión.");
            }
        }

        /// <summary>
        /// Cierra la ventana actual.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void VolverButton_Click(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}
