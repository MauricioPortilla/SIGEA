﻿using SIGEABD;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace SIGEA {

    public partial class RegistrarAsistente : Window {

        public ObservableCollection<ActividadTabla> ActividadesLista { get; } = new ObservableCollection<ActividadTabla>();
        public ObservableCollection<ActividadTabla> ActividadesSeleccionadasLista { get; } = new ObservableCollection<ActividadTabla>();

        /// <summary>
        /// Crea una instancia.
        /// </summary>
        public RegistrarAsistente() {
            InitializeComponent();
            DataContext = this;
            CargarTabla();
        }

        /// <summary>
        /// Representa una actividad en una tabla.
        /// </summary>
        public struct ActividadTabla {
            public Actividad Actividad;
            public string Nombre { get; set; }
            public string Tipo { get; set; }
            public string Descripcion { get; set; }
        }

        /// <summary>
        /// Muestra el panel principal al cerrarse.
        /// </summary>
        /// <param name="e">Evento</param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e) {
            new PanelLiderComite().Show();
        }

        /// <summary>
        /// Metodo que carga la tabla de las actividades registradas para ese evento
        /// </summary>
        private void CargarTabla() {
            try {
                using(SigeaBD sigeaBD = new SigeaBD()) {
                    var listaActividades = sigeaBD.Actividad.AsNoTracking().Where(
                        actividad => actividad.id_evento == Sesion.Evento.id_evento
                    );
                    foreach(Actividad actividad in listaActividades) {
                        ActividadesLista.Add(new ActividadTabla {
                            Actividad = actividad,
                            Nombre = actividad.nombre,
                            Descripcion = actividad.descripcion,
                            Tipo = actividad.tipo
                        });
                    }
                }
            } catch(Exception) {
                MessageBox.Show("Lo sentimos inténtelo más tarde");
            }
        }

        /// <summary>
        /// Metodo que registra al asistente al evento con las distintas actividades
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void RegistrarButton_Click(object sender, RoutedEventArgs e) {
            if(VerificarCampos() && VerificarDatos() && !VerificarExistencia()) {
                try {
                    Collection<Actividad> actividadesSeleccionadas = new Collection<Actividad>();
                    foreach(var actividadSeleccionada in ActividadesSeleccionadasLista) {
                        actividadesSeleccionadas.Add(actividadSeleccionada.Actividad);
                    }
                    using(SigeaBD sigeaBD = new SigeaBD()) {
                        if(new Asistente {
                            nombre = nombreTextBox.Text,
                            paterno = paternoTextBox.Text,
                            materno = maternoTextBox.Text,
                            correo = correoTextBox.Text,
                            Actividad = actividadesSeleccionadas,
                            Adscripcion = new Adscripcion {
                                nombreDependencia = dependenciaTextBox.Text,
                                direccion = direccionTextBox.Text,
                                telefono = telefonoTextBox.Text,
                                puesto = puestoTextBox.Text
                            },
                            Evento = new Collection<Evento>() {
                                Sesion.Evento
                            }
                        }.Registrar()) {
                            MessageBox.Show("Asistente Registrado con éxito");
                            Close();
                        } else {
                            MessageBox.Show("No se registró el asistente");
                        }
                    }
                } catch(Exception) {
                    MessageBox.Show("Lo sentimos inténtelo más tarde");
                }
            }
        }

        /// <summary>
        /// Metodo que cierra la ventana
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void CancelarButton_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        /// <summary>
        /// Metodo que verifica que ningun campo este vacio
        /// </summary>
        /// <returns>true si no están vacíos; false si lo están</returns>
        public bool VerificarCampos() {
            if(!string.IsNullOrWhiteSpace(nombreTextBox.Text) &&
                !string.IsNullOrWhiteSpace(paternoTextBox.Text) &&
                !string.IsNullOrWhiteSpace(correoTextBox.Text) &&
                !string.IsNullOrWhiteSpace(dependenciaTextBox.Text) &&
                !string.IsNullOrWhiteSpace(direccionTextBox.Text) &&
                !string.IsNullOrWhiteSpace(telefonoTextBox.Text) &&
                !string.IsNullOrWhiteSpace(puestoTextBox.Text)) {
                if(ActividadesSeleccionadasLista.Count > 0) {
                    return true;
                } else {
                    MessageBox.Show("“Por favor llenar todos los campos”");
                    return false;
                }
            } else {
                MessageBox.Show("“Por favor llenar todos los campos”");
                return false;
            }
        }

        /// <summary>
        /// Metodo que busca caracteres raros en los datos introducidos
        /// </summary>
        /// <returns>true si los datos son válidos; false si no</returns>
        private bool VerificarDatos() {
            if(Regex.IsMatch(nombreTextBox.Text, Herramientas.REGEX_SOLO_LETRAS) &&
                Regex.IsMatch(paternoTextBox.Text, Herramientas.REGEX_SOLO_LETRAS) &&
                Regex.IsMatch(maternoTextBox.Text, Herramientas.REGEX_SOLO_LETRAS) &&
                Regex.IsMatch(correoTextBox.Text, Herramientas.REGEX_CORREO) &&
                Regex.IsMatch(dependenciaTextBox.Text, Herramientas.REGEX_SOLO_LETRAS) &&
                Regex.IsMatch(telefonoTextBox.Text, Herramientas.REGEX_SOLO_NUMEROS) &&
                Regex.IsMatch(puestoTextBox.Text, Herramientas.REGEX_SOLO_LETRAS)) {
                return true;
            } else {
                MessageBox.Show("Los datos proporcionados son incorrectos");
                return false;
            }
        }

        /// <summary>
        /// Metodo que verifica la existencia del asistente
        /// </summary>
        /// <returns>true si existe; false si no</returns>
        public bool VerificarExistencia() {
            try {
                using(SigeaBD sigeaBD = new SigeaBD()) {
                    var existenciaAsistente = sigeaBD.Asistente.AsNoTracking().Where(
                        asistente => asistente.Evento.FirstOrDefault(
                            eventoLista => eventoLista.id_evento == Sesion.Evento.id_evento
                        ) != null && asistente.correo == correoTextBox.Text
                    );
                    if(existenciaAsistente.Count() == 0) {
                        return false;
                    } else {
                        MessageBox.Show("Este Asistente ya está registrado en el evento");
                        return true;
                    }
                }
            } catch(Exception) {
                MessageBox.Show("Lo sentimos intentelo mas tarde");
                return false;
            }
        }

        /// <summary>
        /// Metodo que selecciona las actividades
        /// </summary>
        /// <param name="sender">ListView</param>
        /// <param name="e">Evento</param>
        private void actividadListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            if(actividadesListView.SelectedItem != null) {
                var actividadSeleccionada = (ActividadTabla) actividadesListView.SelectedItem;
                ActividadesSeleccionadasLista.Add(actividadSeleccionada);
                ActividadesLista.Remove(actividadSeleccionada);
            } else {
                MessageBox.Show("Seleccione una actividad");
            }
        }

        /// <summary>
        /// Metodo que deselecciona las actividades
        /// </summary>
        /// <param name="sender">ListView</param>
        /// <param name="e">Evento</param>
        private void actividadSeleccionadaListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            if(actividadesSeleccionadasListView.SelectedItem != null) {
                var actividadSeleccionada = (ActividadTabla) actividadesSeleccionadasListView.SelectedItem;
                ActividadesLista.Add(actividadSeleccionada);
                ActividadesSeleccionadasLista.Remove(actividadSeleccionada);
            } else {
                MessageBox.Show("Seleccione una actividad");
            }
        }
    }
}
