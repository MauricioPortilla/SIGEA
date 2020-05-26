﻿using SIGEABD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SIGEA {
    /// <summary>
    /// Lógica de interacción para RegistrarTrack.xaml
    /// </summary>
    public partial class RegistrarTrack : Window {
        public RegistrarTrack() {
            InitializeComponent();
        }

        private bool VerificarCampos() {
            return string.IsNullOrWhiteSpace(nombreTextBox.Text) &&
                string.IsNullOrWhiteSpace(descripcionTextBox.Text);
        }

        private bool VerificarExistencia() {
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    return sigeaBD.Track.Where(track => track.nombre == nombreTextBox.Text).Count() == 0;
                }
            } catch (Exception) {
                MessageBox.Show("Error al registrar el track.");
                return false;
            }
        }

        private void RegistrarButton_Click(object sender, RoutedEventArgs e) {
            if (!VerificarCampos()) {
                MessageBox.Show("Faltan campos por completar.");
                return;
            }
            if (!VerificarExistencia()) {
                MessageBox.Show("Ya existe un track registrado con este nombre.");
                return;
            }
            Track track = new Track {
                nombre = nombreTextBox.Text,
                descripcion = descripcionTextBox.Text,
                id_evento = Sesion.Evento.id_evento
            };
            try {
                if (track.Registrar()) {
                    MessageBox.Show("Track registrado con éxito.");
                    Close();
                } else {
                    MessageBox.Show("Error al registrar el track.");
                }
            } catch (Exception) {
                MessageBox.Show("Error al registrar el track.");
            }
        }

        private void CancelarButton_Click(object sender, RoutedEventArgs e) {

        }
    }
}
