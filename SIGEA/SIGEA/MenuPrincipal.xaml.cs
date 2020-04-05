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

        public MenuPrincipal() {
            InitializeComponent();
            DataContext = this;
            CargarDataGrid();
        }

        private void CerrarButton_Click (object sender, RoutedEventArgs e) {
            IniciarSesion inicioSesion = new IniciarSesion();
            inicioSesion.Show();
            this.Close();
        }

        private void CrearButton_Click (object sender, RoutedEventArgs e) {
            RegistrarEvento registroEvento = new RegistrarEvento();
            registroEvento.Show();
            this.Close();
        }

        public void CargarDataGrid () {
            using (SigeaBD sigeaBD = new SigeaBD()) {
                try {
                    foreach (Evento evento in sigeaBD.Evento.ToList()) {
                        EventosObservableCollection.Add(new EventoTabla {
                            nombre = evento.nombre,
                            sede = evento.sede,
                            fechaInicio = evento.fechaInicio,
                            fechaFin = evento.fechaFin
                        });
                    }
                } catch (EntityException) {
                    MessageBox.Show("Error al cargar los proveedores.");
                } catch (Exception) {
                    MessageBox.Show("Error al cargar los proveedores.");
                }
            }
        }

        public struct EventoTabla {
            public String nombre { get; set; }
            public String sede { get; set; }
            public DateTime fechaInicio { get; set; }
            public DateTime fechaFin { get; set; }
        }

    }
}
