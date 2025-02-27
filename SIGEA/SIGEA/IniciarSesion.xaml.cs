﻿using SIGEABD;
using System.Data.Entity.Core;
using System.Linq;
using System.Windows;

namespace SIGEA {
    /// <summary>
    /// Lógica de interacción para IniciarSesion.xaml
    /// </summary>
    public partial class IniciarSesion : Window {
        /// <summary>
        /// Crea una instancia.
        /// </summary>
        public IniciarSesion() {
            InitializeComponent();
        }

        /// <summary>
        /// Realiza el proceso de inicio de sesión, comprobando que exista una cuenta en
        /// la base de datos con los datos especificados.
        /// </summary>
        /// <param name="sender">Botón Iniciar sesión</param>
        /// <param name="e">Evento del botón</param>
        private void IniciarSesionButton_Click(object sender, RoutedEventArgs e) {
            if (!VerificarCampos()) {
                MessageBox.Show("Faltan campos por completar.");
                return;
            }
            try {
                string contraseniaCifrada = Herramientas.CifrarConSHA512(contraseniaTextBox.Password);
                Cuenta.IniciarSesion(usuarioTextBox.Text, contraseniaCifrada, (cuentaEncontrada) => {
                    if (cuentaEncontrada != null) {
                        Sesion.Cuenta = cuentaEncontrada;
                        Sesion.Revisor = cuentaEncontrada.Revisor.Count > 0 ? cuentaEncontrada.Revisor.FirstOrDefault() : null;
                        Sesion.Organizador = cuentaEncontrada.Organizador.ToList().FirstOrDefault();
                        if (Sesion.Revisor == null) {
                            new MenuPrincipal().Show();
                        } else {
                            new PanelRevisor().Show();
                        }
                        Close();
                    } else {
                        MessageBox.Show("No existe una cuenta registrada con estos datos.");
                    }
                });
            } catch (EntityException) {
                MessageBox.Show("Error al iniciar sesión.");
            }
        }

        /// <summary>
        /// Verifica que los campos no estén vacíos.
        /// </summary>
        /// <returns>true si los campos están completos; false si no</returns>
        private bool VerificarCampos() {
            return !string.IsNullOrWhiteSpace(usuarioTextBox.Text) &&
                !string.IsNullOrWhiteSpace(contraseniaTextBox.Password);
        }
    }
}
