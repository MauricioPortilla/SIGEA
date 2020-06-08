using System;
using System.IO;
using System.Windows;

namespace SIGEA {
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application {
        public static readonly string ARTICULOS_DIRECTORIO = AppDomain.CurrentDomain.BaseDirectory + "/archivos/articulos";

        /// <summary>
        /// Inicializa la aplicación.
        /// </summary>
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
