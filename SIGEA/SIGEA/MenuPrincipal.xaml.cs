﻿using System;
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
    /// Lógica de interacción para MenuPrincipal.xaml
    /// </summary>
    public partial class MenuPrincipal : Window {
        public MenuPrincipal() {
            InitializeComponent();
        }

        private void registrarArticuloButton_Click(object sender, RoutedEventArgs e) {
            new RegistrarArticulo().Show();
        }

        private void RegistrarAutorButton_Click(object sender, RoutedEventArgs e) {
            new RegistrarAutor().Show();
        }
    }
}
