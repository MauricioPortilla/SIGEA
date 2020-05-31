using SIGEABD;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using static SIGEA.RegistrarAsistente;

namespace SIGEA {
    /// <summary>
    /// Lógica de interacción para PanelLiderComite.xaml
    /// </summary>
    public partial class PanelLiderComite : Window {
        private bool mostrarMenuPrincipal = true;
        public ObservableCollection<ActividadTabla> ActividadesLista { get; } = new ObservableCollection<ActividadTabla>();
        public ObservableCollection<TareaTabla> TareasLista { get; } = new ObservableCollection<TareaTabla>();

        /// <summary>
        /// Crea una instancia.
        /// </summary>
        public PanelLiderComite() {
            InitializeComponent();
            DataContext = this;
            CargarTablaActividades();
            CargarTablaTareas();
        }

        /// <summary>
        /// Representa una tarea en una tabla.
        /// </summary>
        public struct TareaTabla {
            public Tarea Tarea;
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public string AsignadoA { get; set; }
            public Actividad Actividad { get; set; }
        }

        // <summary>
        /// Muestra el menú principal al cerrarse.
        /// </summary>
        /// <param name="e">Evento</param>
        protected override void OnClosing(CancelEventArgs e) {
            if (mostrarMenuPrincipal) {
                new MenuPrincipal().Show();
            }
        }

        /// <summary>
        /// Método que carga la tabla de actividades
        /// </summary>
        private void CargarTablaActividades() {
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    var listaActividades = sigeaBD.Actividad.AsNoTracking().Where(
                        actividad => actividad.id_evento == Sesion.Evento.id_evento
                    );
                    foreach (Actividad actividad in listaActividades) {
                        ActividadesLista.Add(new ActividadTabla {
                            Actividad = actividad,
                            Nombre = actividad.nombre,
                            Tipo = actividad.tipo,
                            Descripcion = actividad.descripcion
                        });
                    }
                }
            } catch (Exception) {
                MessageBox.Show("Error al establecer una conexión.");
            }
        }

        /// <summary>
        /// Carga las tareas del comité en la tabla.
        /// </summary>
        private void CargarTablaTareas() {
            try {
                using (SigeaBD sigeaBD = new SigeaBD()) {
                    var listaTareas = sigeaBD.Tarea.AsNoTracking().Where(
                        tarea => tarea.id_comite == Sesion.Comite.id_comite
                    );
                    foreach (Tarea tarea in listaTareas) {
                        TareasLista.Add(new TareaTabla {
                            Tarea = tarea,
                            Titulo = tarea.titulo,
                            Descripcion = tarea.descripcion,
                            AsignadoA = tarea.asignadoA,
                            Actividad = tarea.Actividad.FirstOrDefault()
                        });
                    }
                }
            } catch (Exception) {
                MessageBox.Show("Error al establecer una conexión.");
            }
        }

        /// <summary>
        /// Abre una ventana para registrar un artículo.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void RegistrarArticuloButton_Click(object sender, RoutedEventArgs e) {
            mostrarMenuPrincipal = false;
            new RegistrarArticulo().Show();
            this.Close();
        }

        /// <summary>
        /// Abre una ventana para registrar un magistral.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void RegistrarMagistralButton_Click(object sender, RoutedEventArgs e) {
            if (actividadesListView.SelectedItem != null) {
                mostrarMenuPrincipal = false;
                var actividad = (ActividadTabla) actividadesListView.SelectedItem;
                new RegistrarMagistral(actividad.Nombre).Show();
                this.Close();
            } else {
                MessageBox.Show("Seleccione una actividad");
            }
        }

        /// <summary>
        /// Abre una ventana para registrar un asistente.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void RegistrarAsistenteButton_Click(object sender, RoutedEventArgs e) {
            mostrarMenuPrincipal = false;
            new RegistrarAsistente().Show();
            this.Close();
        }

        /// <summary>
        /// Abre una ventana para consultar los artículos.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void ConsultarArticulosButton_Click(object sender, RoutedEventArgs e) {
            mostrarMenuPrincipal = false;
            new ConsultarArticulos().Show();
            Close();
        }

        /// <summary>
        /// Abre una ventana para registrar un autor.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void RegistrarAutorButton_Click(object sender, RoutedEventArgs e) {
            mostrarMenuPrincipal = false;
            new RegistrarAutor().Show();
            Close();
        }

        /// <summary>
        /// Abre una ventana para registrar una tarea.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void RegistrarTareaButton_Click(object sender, RoutedEventArgs e) {
            mostrarMenuPrincipal = false;
            new RegistrarTarea().Show();
            Close();
        }

        /// <summary>
        /// Abre una ventana para modificar la tarea seleccionada.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void ModificarTareasAsignadasButton_Click(object sender, RoutedEventArgs e) {
            if (tareasListView.SelectedIndex != -1) {
                mostrarMenuPrincipal = false;
                new ModificarTarea(((TareaTabla) tareasListView.SelectedItem).Tarea).Show();
                Close();
            } else {
                MessageBox.Show("Selecciona una tarea.");
            }
        }

        /// <summary>
        /// Abre una ventana para registrar un gasto.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void RegistrarGastoButton_Click(object sender, RoutedEventArgs e) {
            mostrarMenuPrincipal = false;
            new RegistrarGasto().Show();
            Close();
        }

        /// <summary>
        /// Abre una ventana para asignar un organizador al comité
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void AsignarOrganizadorAComiteButton_Click(object sender, RoutedEventArgs e) {
            mostrarMenuPrincipal = false;
            new AsignarOrganizadorComite().Show();
            Close();
        }

        /// <summary>
        /// Cierra la ventana.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void RegresarButton_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        /// <summary>
        /// Abre una ventana para registrar el pago de un asistente.
        /// </summary>
        /// <param name="sender">Botón</param>
        /// <param name="e">Evento</param>
        private void RegistrarPagoAsistenteButton_Click(object sender, RoutedEventArgs e) {
            mostrarMenuPrincipal = false;
            new RegistrarPagoAsistente().Show();
            Close();
        }
    }
}
