using SIGEABD;
using System;
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

                var listaEventos2 = sigeaBD.Evento.Where(evento =>
                evento.Comite.Where(comite =>
                comite.id_organizador == Sesion.Organizador.id_organizador).Count() != 0);

                foreach (Evento evento in listaEventos2) {

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

            var eventoSeleccioando = (DataRowView) eventosDataGrid.SelectedItem;

            if (eventoSeleccioando != null) {

                try {

                    using (SigeaBD sigeaBD = new SigeaBD()) {

                        Sesion.Evento = sigeaBD.Evento.ToList().Find(
                            evento => evento.nombre == eventoSeleccioando[0].ToString());
                    }

                } catch (Exception ex) {

                    Console.WriteLine(ex);
                }

                new Actividades().Show();
                this.Close();
            }

        }
    }
}
