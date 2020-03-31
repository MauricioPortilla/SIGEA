using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SIGEA {
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application {
        public static readonly string ARTICULOS_DIRECTORIO = AppDomain.CurrentDomain.BaseDirectory + "/archivos/articulos";

        public App() {
            CrearDirectorios();
        }

        /// <summary>
        /// Crea un directorio de artículos en la carpeta de archivos en la raíz del programa
        /// por si no existe.
        /// </summary>
        private void CrearDirectorios() {
            if (!Directory.Exists(ARTICULOS_DIRECTORIO)) {
                Directory.CreateDirectory(ARTICULOS_DIRECTORIO);
            }
        }
    }
}
