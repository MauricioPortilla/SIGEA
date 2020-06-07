using SIGEABD;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace SIGEA {

    public partial class RegistrarComite : Window {
        private List<Organizador> organizadores = new List<Organizador>();
        public ObservableCollection<OrganizadorTabla> OrganizadoresLista { get; } = new ObservableCollection<OrganizadorTabla>();

        /// <summary>
        /// Crea una instancia.
        /// </summary>
        public RegistrarComite() {
            InitializeComponent();
            DataContext = this;
            CargarOrganizadores();
            CargarComboBox();
        }

        /// <summary>
        /// Muestra el panel principal al cerrarse.
        /// </summary>
        /// <param name="e">Evento</param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e) {
            new PanelLiderEvento().Show();
        }

        /// <summary>
        /// Representa un Organizador en una tabla.
        /// </summary>
        public struct OrganizadorTabla {
            public Organizador Organizador;
            public string Nombre { get; set; }
            public string Paterno { get; set; }
            public string Materno { get; set; }
        }

        /// <summary>
        /// Carga los organizadores de la base de datos y los coloca en una lista.
        /// </summary>
        private void CargarOrganizadores() {
            try {
                using(SigeaBD sigeaBD = new SigeaBD()) {
                    organizadores = sigeaBD.Organizador.AsNoTracking().Where(
                        organizador => organizador.id_organizador != Sesion.Organizador.id_organizador
                    ).ToList();
                }
            } catch(Exception) {
                MessageBox.Show("Lo sentimos inténtelo más tarde");
            }
        }

        /// <summary>
        /// Metodo que carga el combo con los organizadores
        /// </summary>
        private void CargarComboBox() {
            foreach(Organizador organizador in organizadores) {
                organizadorComboBox.Items.Add(organizador);
            }
        }

        /// <summary>
        /// Limpia la lista de organizadores de la tabla y la vuelve a llenar exceptuando
        /// el organizador seleccionado en el combo box.
        /// </summary>
        /// <param name="sender">ComboBox</param>
        /// <param name="e">Evento</param>
        private void organizadorComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
            OrganizadoresLista.Clear();
            foreach(var organizador in organizadores) {
                if(organizador.id_organizador == (organizadorComboBox.SelectedItem as Organizador).id_organizador) {
                    continue;
                }
                OrganizadoresLista.Add(new OrganizadorTabla {
                    Organizador = organizador,
                    Nombre = organizador.nombre,
                    Paterno = organizador.paterno,
                    Materno = organizador.materno
                });
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
        /// Metodo que registra el comite en el sistema
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void RegistrarButton_Click(object sender, RoutedEventArgs e) {
            if(VerificarCampos() && VerificarDatos() && VerificarExistencia()) {
                Collection<Organizador> organizadoresSeleccionados = new Collection<Organizador>();
                foreach(OrganizadorTabla organizadorTabla in organizadoresListView.SelectedItems) {
                    organizadoresSeleccionados.Add(organizadorTabla.Organizador);
                }
                try {
                    var organizador = (Organizador) organizadorComboBox.SelectedItem;
                    using(SigeaBD sigeaBD = new SigeaBD()) {
                        if(new Comite {
                            nombre = nombreTextBox.Text,
                            responsabilidades = responsabilidadesTextBox.Text,
                            id_evento = Sesion.Evento.id_evento,
                            id_organizador = organizador.id_organizador,
                            Organizadores = organizadoresSeleccionados
                        }.Registrar()) {
                            MessageBox.Show("Comité registrado con éxito");
                            Close();
                        } else {
                            MessageBox.Show("No se regristró el comité");
                        }
                    }
                } catch(Exception) {
                    MessageBox.Show("Lo sentimos inténtelo más tarde");
                }
            }
        }

        /// <summary>
        /// Metodo que verifica que ningun campo este vacio
        /// </summary>
        /// <returns>true si están completos; false si no</returns>
        public Boolean VerificarCampos() {
            if(!string.IsNullOrWhiteSpace(nombreTextBox.Text) &&
                !string.IsNullOrWhiteSpace(responsabilidadesTextBox.Text)) {
                if(organizadorComboBox.SelectedItem != null) {
                    return true;
                } else {
                    MessageBox.Show("Debe seleccionar al menos una actividad.");
                    return false;
                }
            } else {
                MessageBox.Show("Por favor llenar todos los campos");
                return false;
            }
        }

        /// <summary>
        /// Metodo que busca caracteres raros en los datos introducidos
        /// </summary>
        /// <returns>true si son válidos; false si no</returns>
        private bool VerificarDatos() {
            if(Regex.IsMatch(nombreTextBox.Text, Herramientas.REGEX_SOLO_LETRAS)) {
                return true;
            } else {
                MessageBox.Show("Los datos proporcionados son incorrectos");
                return false;
            }
        }

        /// <summary>
        /// Metodo que verifica la existencia del comite
        /// </summary>
        /// <returns>true si existe; false si no</returns>
        public Boolean VerificarExistencia() {
            try {
                using(SigeaBD sigeaBD = new SigeaBD()) {
                    var comiteOptenido = sigeaBD.Comite.ToList().Find(
                        comite => comite.nombre == nombreTextBox.Text &&
                        comite.Evento.id_evento == Sesion.Evento.id_evento
                    );
                    if(comiteOptenido == null) {
                        return true;
                    } else {
                        MessageBox.Show("Este Comité ya está registrado en el evento");
                        return false;
                    }
                }
            } catch(Exception) {
                MessageBox.Show("Lo sentimos inténtelo más tarde");
                return false;
            }
        }
    }
}
