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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SIGEABD;

namespace SIGEA {
    /// <summary>
    /// Lógica de interacción para InicioSesion.xaml
    /// </summary>
    public partial class InicioSesion : Window {
        public InicioSesion() {
            InitializeComponent();
        }

        private void IniciarSesionButton_Click(object sender, RoutedEventArgs e) {
            if (!verificarCampos()) {
                MessageBox.Show("Faltan campos por completar.");
                return
            } else if (!verificarDatos()) {
                MessageBox.Show("Debes ingresar datos válidos.");
                return;
            }

        }

        private bool verificarCampos() {
            return !string.IsNullOrWhiteSpace(usuarioTextBox.Text) &&
                !string.IsNullOrWhiteSpace(contraseniaTextBox.Text);
        }

        private bool verificarDatos() {
            return true;
        }
    }
}
