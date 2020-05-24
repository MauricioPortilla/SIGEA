using SIGEABD;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace SIGEA {

    public partial class RegistrarAsistente : Window {

        public DataTable tablaActividades = new DataTable();
        public DataTable tablaActividadesSeleccionadas = new DataTable();

        public RegistrarAsistente () {

            InitializeComponent();
            DataContext = this;
            CargarTabla();
        }

        /// <summary>
        /// Metodo que registra al asistente al evento con las distintas actividades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RegistrarButton_Click (object sender, RoutedEventArgs e) {
            if (VerificarCampos() && VerificarDatos()) {
                try {
                    using (SigeaBD sigeaBD = new SigeaBD()) {
                        if (new Asistente {
                            nombre = nombreTextBox.Text,
                            paterno = paternoTextBox.Text,
                            materno = maternoTextBox.Text,
                            correo = correoTextBox.Text,
                            Actividad = 
                            Adscripcion = new Adscripcion {
                                nombreDependencia = dependenciaTextBox.Text,
                                direccion = direccionTextBox.Text,
                                telefono = telefonoTextBox.Text,
                                puesto = puestoTextBox.Text
                            }
                        }.Registrar()) {
                            MessageBox.Show("Asistente registrado con exito");
                        } else {
                            MessageBox.Show("Asistente no pudo registrarse");
                        }
                    }
                } catch (Exception exception) {
                    MessageBox.Show("Error al registrar el Asistente.");
                    Console.WriteLine("Exception@RegistrarButton_Click -> " + exception.Message);
                }
            }
        }

        /// <summary>
        /// Metodo que cierra la ventana
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelarButton_Click (object sender, RoutedEventArgs e) {

            new Actividades().Show();
            this.Close();
        }

        /// <summary>
        /// Metodo que carga la tabla de las actividades registradas para ese evento
        /// </summary>
        private void CargarTabla () {

            DataColumn nombre = new DataColumn("Nombre");
            DataColumn tipo = new DataColumn("Tipo");
            DataColumn descripcion = new DataColumn("Descripción");

            tablaActividades.Columns.Add(nombre);
            tablaActividades.Columns.Add(tipo);
            tablaActividades.Columns.Add(descripcion);

            DataColumn nombre2 = new DataColumn("Nombre");
            DataColumn tipo2 = new DataColumn("Tipo");
            DataColumn descripcion2 = new DataColumn("Descripción");

            tablaActividadesSeleccionadas.Columns.Add(nombre2);
            tablaActividadesSeleccionadas.Columns.Add(tipo2);
            tablaActividadesSeleccionadas.Columns.Add(descripcion2);

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

            actividadDataGrid.ItemsSource = tablaActividades.DefaultView;
        }

        /// <summary>
        /// Metodo que verifica que ningun campo este vacio
        /// </summary>
        /// <returns></returns>
        public Boolean VerificarCampos () {

            if (!string.IsNullOrWhiteSpace(nombreTextBox.Text) &&
                !string.IsNullOrWhiteSpace(paternoTextBox.Text) &&
                !string.IsNullOrWhiteSpace(maternoTextBox.Text) &&
                !string.IsNullOrWhiteSpace(correoTextBox.Text) &&
                !string.IsNullOrWhiteSpace(dependenciaTextBox.Text) &&
                !string.IsNullOrWhiteSpace(direccionTextBox.Text) &&
                !string.IsNullOrWhiteSpace(telefonoTextBox.Text) &&
                !string.IsNullOrWhiteSpace(puestoTextBox.Text)) {

                if (actividadDataGrid.SelectedIndex != -1) {

                    return true;

                } else {

                    MessageBox.Show("Debe seleccionar al menos una actividad.");
                    return false;
                }

            } else {

                MessageBox.Show("Debe llenar todos los campos.");
                return false;
            }
        }

        /// <summary>
        /// Metodo que busca caracteres raros en los datos introducidos
        /// </summary>
        /// <returns></returns>
        private bool VerificarDatos () {

            if (Regex.IsMatch(nombreTextBox.Text, Herramientas.REGEX_SOLO_LETRAS) &&
                Regex.IsMatch(paternoTextBox.Text, Herramientas.REGEX_SOLO_LETRAS) &&
                Regex.IsMatch(maternoTextBox.Text, Herramientas.REGEX_SOLO_LETRAS) &&
                Regex.IsMatch(correoTextBox.Text, Herramientas.REGEX_CORREO) &&
                Regex.IsMatch(dependenciaTextBox.Text, Herramientas.REGEX_SOLO_LETRAS) &&
                Regex.IsMatch(telefonoTextBox.Text, Herramientas.REGEX_SOLO_NUMEROS) &&
                Regex.IsMatch(puestoTextBox.Text, Herramientas.REGEX_SOLO_LETRAS)) {

                return true;

            } else {

                MessageBox.Show("Por favor revise los datos ingresados");
                return false;
            }
        }

        /// <summary>
        /// Metodo que verifica la existencia del asistente
        /// </summary>
        /// <returns></returns>
        public bool VerificarExistencia () {

            try{

                using (SigeaBD sigeaBD = new SigeaBD()) {

                    var asistentes = sigeaBD.Asistente.Where(
                        asistente => asistente.Evento.FirstOrDefault(
                            eventoLista => eventoLista.id_evento == Sesion.Evento.id_evento
                        ) != null
                    );

                    if (asistentes != null) {

                        return true;

                    } else {

                        MessageBox.Show("El asistente ya esta registrado");
                        return false;
                    }
                }

            } catch (Exception e){

                MessageBox.Show("Lo sentimos intentelo mas tarde");
                return false;
            }
        }

        /// <summary>
        /// Metodo que selecciona las actividades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void actividadDataGrid_MouseDoubleClick (object sender, System.Windows.Input.MouseButtonEventArgs e) {

            var actividadSeleccionada = (DataRowView) actividadDataGrid.SelectedItem;

            if (actividadSeleccionada != null) {

                DataRow fila = tablaActividadesSeleccionadas.NewRow();

                fila [0] = actividadSeleccionada [0].ToString();
                fila [1] = actividadSeleccionada [1].ToString();
                fila [2] = actividadSeleccionada [2].ToString();

                tablaActividadesSeleccionadas.Rows.Add(fila);

                actividadSeleccionadaDataGrid.ItemsSource = tablaActividadesSeleccionadas.DefaultView;

            } else {

                MessageBox.Show("Seleccione una actividad");
            }
        }

        /// <summary>
        /// Metodo que deselecciona las actividades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void actividadSeleccionadaDataGrid_MouseDoubleClick (object sender, System.Windows.Input.MouseButtonEventArgs e) {

            var actividadSeleccionada = (DataRowView) actividadSeleccionadaDataGrid.SelectedItem;

            if (actividadSeleccionada != null) {

                DataRow fila = tablaActividades.NewRow();

                fila [0] = actividadSeleccionada [0].ToString();
                fila [1] = actividadSeleccionada [1].ToString();
                fila [2] = actividadSeleccionada [2].ToString();

                tablaActividades.Rows.Add(fila);

                actividadDataGrid.ItemsSource = tablaActividades.DefaultView;

            } else {

                MessageBox.Show("Seleccione una actividad");
            }
        }
    }
}
