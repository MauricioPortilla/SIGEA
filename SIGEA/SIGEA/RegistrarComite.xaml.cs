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

        public ObservableCollection<Organizador> OrganizadoresObservableCollection { get; } = 
            new ObservableCollection<Organizador>();

        public RegistrarComite () {
            InitializeComponent();
            CargarComboBox();
        }

        private void CancelarButton_Click (object sender, RoutedEventArgs e) {

        }

        private void RegistrarButton_Click (object sender, RoutedEventArgs e) {
            if (VerificarCampos() && VerificarDatos() && VerificarExistencia()) {
                try {
                    OrganizadoresObservableCollection.Add((Organizador) organizadorComboBox.SelectedItem);
                    using (SigeaBD sigeaBD = new SigeaBD()) {
                        if (new Comite {
                            nombre = nombreTextBox.Text,
                            responsabilidades = responsabilidadesTextBlock.Text,
                            Evento = (Evento) eventoComboBox.SelectedItem,
                            Organizadores = OrganizadoresObservableCollection 
                        }.Registrar()) {
                            MessageBox.Show("El Evento se registro correctamente");
                        } else {
                            MessageBox.Show("El Evento no se pudo registrar");
                        }
                    }
                } catch (Exception exception) {
                    MessageBox.Show("Error al registrar el evento.");
                    Console.WriteLine("Exception@RegistrarButton_Click -> " + exception.Message);
                }
            }
        }

        private void CargarComboBox () {
            using (SigeaBD sigeaBD = new SigeaBD()) {
                try {
                    foreach (Evento evento in sigeaBD.Evento.ToList()) {
                        eventoComboBox.Items.Add(evento);
                    }
                } catch (EntityException entityException) {
                    MessageBox.Show("Error al cargar los eventos.");
                    Console.WriteLine("EntityException@RegistrarArticulo->CargarEventos() -> " + entityException.Message);
                }
                try {
                    foreach (Organizador organizador in sigeaBD.Organizador.ToList()) {
                        organizadorComboBox.Items.Add(organizador);
                    }
                } catch (EntityException entityException) {
                    MessageBox.Show("Error al cargar los eventos.");
                    Console.WriteLine("EntityException@RegistrarArticulo->CargarEventos() -> " + entityException.Message);
                }
            }
        }

        public Boolean VerificarCampos () {
            if (!string.IsNullOrWhiteSpace(nombreTextBox.Text) &&
                !string.IsNullOrWhiteSpace(responsabilidadesTextBlock.Text)) {
                if (eventoComboBox.SelectedItem != null &&
                    organizadorComboBox.SelectedItem != null ) {
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

        private bool VerificarDatos () {
            if (Regex.IsMatch(nombreTextBox.Text, Herramientas.REGEX_SOLO_LETRAS) &&
                Regex.IsMatch(responsabilidadesTextBlock.Text, Herramientas.REGEX_SOLO_LETRAS)) {
                return true;
            } else {
                MessageBox.Show("Por favor revise los datos ingresados");
                return false;
            }
        }

        public Boolean VerificarExistencia () {
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    var comiteOptenido = sigeaBD.Comite.ToList().Find(
                        comite => comite.nombre == nombreTextBox.Text);
                    if (comiteOptenido == null) {
                        return true;
                    } else {
                        MessageBox.Show("Ya esta registrado el comté");
                        return false;
                    }
                }
            } catch (Exception e) {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}
