using SIGEABD;
using System;
using System.Data;
using System.Linq;
using System.Windows;

namespace SIGEA {

    /// <summary>
    /// Lógica de interacción para Tareas.xaml
    /// </summary>
    public partial class Tareas : Window {

        public Tareas () {
            InitializeComponent();
        }

        /// <summary>
        /// Metodo acción del boton modificar tarea
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ModificarButton_Click (object sender, RoutedEventArgs e) {

            //var tareaSeleccionada = (DataRowView) tareasDataGrid.SelectedItem;

            //if (tareaSeleccionada != null) {

            //    using (SigeaBD sigeaBD = new SigeaBD()) {

            //        Sesion.Tarea = sigeaBD.Tarea.ToList().Find(
            //            tarea => tarea.titulo == tareaSeleccionada[0].ToString());
            //    }

            //    new ModificarTarea().Show();
            //    this.Close();

            //} else {

            //    MessageBox.Show("Selecciona una tarea");
            //}
        }

        /// <summary>
        /// Metodo que carga la tabla de Tareas
        /// </summary>
        public void CargarTabla () {

            DataTable tablaTareas = new DataTable();

            DataColumn titulo = new DataColumn("Titulo");
            DataColumn descripciom = new DataColumn("Descripción");

            tablaTareas.Columns.Add(titulo);
            tablaTareas.Columns.Add(descripciom);

            try {

                using (SigeaBD sigeaBD = new SigeaBD()) {

                    var listaTareas = (from tarea in sigeaBD.Tarea
                                       where tarea.id_comite == Sesion.Comite.id_comite
                                       select tarea).ToList();

                    foreach (Tarea tarea in listaTareas) {

                        DataRow fila = tablaTareas.NewRow();

                        fila [0] = tarea.titulo;
                        fila [1] = tarea.descripcion;

                        tablaTareas.Rows.Add(fila);
                    }
                }

            } catch (Exception) {

                MessageBox.Show("Lo sentimos intentelo mas tarde");
            }

            tareasDataGrid.ItemsSource = tablaTareas.DefaultView;
        }
    }
}
