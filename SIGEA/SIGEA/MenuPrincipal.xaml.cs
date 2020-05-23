using SIGEABD;
using System.Data;
using System.Linq;
using System.Windows;

namespace SIGEA {
    public partial class MenuPrincipal : Window {

        public MenuPrincipal () {

            InitializeComponent();
            CargarTabla();
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
        public void CargarTabla () {

            DataTable tablaEventos = new DataTable();

            DataColumn nombre = new DataColumn("Nombre");
            DataColumn sede = new DataColumn("Sede");
            DataColumn fechaInicio = new DataColumn("Fecha de Inicio");
            DataColumn fechaFin = new DataColumn("Fecha de FIn");

            tablaEventos.Columns.Add(nombre);
            tablaEventos.Columns.Add(sede);
            tablaEventos.Columns.Add(fechaInicio);
            tablaEventos.Columns.Add(fechaFin);

            using (SigeaBD sigeaBD = new SigeaBD()) {

                var listaEventos = (from evento in sigeaBD.Evento
                                    where evento.id_organizador == Sesion.Organizador.id_organizador
                                    select evento).ToList();

                foreach (Evento evento in listaEventos) {

                    DataRow fila = tablaEventos.NewRow();

                    fila [0] = evento.nombre;
                    fila [1] = evento.sede;
                    fila [2] = evento.fechaInicio.ToShortDateString();
                    fila [3] = evento.fechaFin.Date.ToShortDateString();

                    tablaEventos.Rows.Add(fila);
                }
            }

            eventosDataGrid.ItemsSource = tablaEventos.DefaultView;
        }


        private void PruebaButton_Click (object sender, RoutedEventArgs e) {
            /*if (eventosDataGrid.SelectedItem != null) {
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
            }*/
        }
    }
}
