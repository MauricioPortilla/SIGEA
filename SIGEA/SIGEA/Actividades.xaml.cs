using SIGEABD;
using System;
using System.Data;
using System.Linq;
using System.Windows;


namespace SIGEA {
    /// <summary>
    /// Lógica de interacción para Actividades.xaml
    /// </summary>
    public partial class Actividades : Window {
        public Actividades () {

            InitializeComponent();
            CargarTabla();
        }

        private void CancelarButton_Click (object sender, RoutedEventArgs e) {

        }

        /// <summary>
        /// MEtodo que carga la tabla de actividades
        /// </summary>
        public void CargarTabla () {

            DataTable tablaActividades = new DataTable();

            DataColumn nombre = new DataColumn("Nombre");
            DataColumn tipo = new DataColumn("Tipo");
            DataColumn descripcion = new DataColumn("Descripción");

            tablaActividades.Columns.Add(nombre);
            tablaActividades.Columns.Add(tipo);
            tablaActividades.Columns.Add(descripcion);

            using (SigeaBD sigeaBD = new SigeaBD()) {

                var listaActividades = (from actividad in sigeaBD.Actividad
                                    where actividad.id_evento == Sesion.Evento.id_evento
                                    select actividad).ToList();

                foreach (Actividad actividad in listaActividades) {

                    DataRow fila = tablaActividades.NewRow();

                    fila [0] = actividad.nombre;
                    fila [1] = actividad.tipo;
                    fila [2] = actividad.descripcion;

                    tablaActividades.Rows.Add(fila);
                }

            }

            actividadesDataGrid.ItemsSource = tablaActividades.DefaultView;
        }

        private void MagistralButton_Click (object sender, RoutedEventArgs e) {
            var actividad = (DataRowView) actividadesDataGrid.SelectedItem;

            if (actividad != null) {

                String nombreActividad = actividad [0].ToString();
                RegistrarMagistral magistral = new RegistrarMagistral(nombreActividad);
                magistral.Show();
                this.Close();

            } else {

                MessageBox.Show("Seleccione una actividad");
            }
        }

        private void AsistenteButton_Click (object sender, RoutedEventArgs e) {

            new RegistrarAsistente().Show();
            this.Close();
        }
    }
}
