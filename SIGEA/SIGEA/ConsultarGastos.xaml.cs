using SIGEABD;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace SIGEA {
    /// <summary>
    /// Lógica de interacción para ConsultarGastos.xaml
    /// </summary>
    public partial class ConsultarGastos : Window {
        public ObservableCollection<GastoTabla> GastosLista { get; } = new ObservableCollection<GastoTabla>();

        /// <summary>
        /// Crea una instancia.
        /// </summary>
        public ConsultarGastos() {
            InitializeComponent();
            DataContext = this;
            CargarGastos();
        }

        /// <summary>
        /// Muestra el panel principal al cerrarse.
        /// </summary>
        /// <param name="e">Evento</param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e) {
            new PanelLiderEvento().Show();
        }

        /// <summary>
        /// Representa un gasto en una tabla.
        /// </summary>
        public struct GastoTabla {
            public string Fecha { get; set; }
            public float Cantidad { get; set; }
            public string Motivo { get; set; }
            public Magistral Magistral { get; set; }
        }

        /// <summary>
        /// Carga los gastos del evento.
        /// </summary>
        private void CargarGastos() {
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    var gastos = sigeaBD.Gasto.Where(gasto => gasto.id_evento == Sesion.Evento.id_evento);
                    foreach (var gasto in gastos) {
                        GastosLista.Add(new GastoTabla {
                            Fecha = gasto.fecha.ToString("dd/MM/yyyy"),
                            Cantidad = float.Parse(gasto.cantidad.ToString()),
                            Motivo = gasto.motivo,
                            Magistral = gasto.Magistral.FirstOrDefault()
                        });
                    }
                }
            } catch (Exception) {
                MessageBox.Show("Error al establecer una conexión.");
                Close();
            }
        }

        /// <summary>
        /// Cierra la ventana.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void VolverButton_Click(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}
