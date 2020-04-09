using SIGEABD;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace SIGEA {

    public partial class RegistrarAsistente : Window {

        public ObservableCollection<ActividadTabla>
            ActividadesObservableCollection { get; } =
            new ObservableCollection<ActividadTabla>();

        public RegistrarAsistente () {
            InitializeComponent();
            CargarTabla();
        }

        /// <summary>
        /// Metodo que registra al asistente al evento con las distintas actividades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RegistrarButton_Click (object sender, RoutedEventArgs e) {
            if (VerificarCampos() && VerificarDatos()) {
                try {
                    using (SigeaBD sigeaBD = new SigeaBD()) {
                        if (new Asistente {
                            nombre = nombreTextBox.Text,
                            paterno = paternoTextBox.Text,
                            materno = maternoTextBox.Text,
                            correo = correoTextBox.Text,
                            Actividad = (ICollection<Actividad>)
                                        ActividadesObservableCollection.Where(
                                            actividad => actividad.Seleccionado
                                        ).ToList(),
                            Adscripcion = new Adscripcion {
                                nombreDependencia = dependenciaTextBox.Text,
                                direccion = direccionTextBox.Text,
                                telefono = telefonoTextBox.Text,
                                puesto = puestoTextBox.Text
                            }
                        }.Registrar()) {
                            MessageBox.Show("Asistente registrado con exito");
                        } else {
                            MessageBox.Show("Asistente no pudo registrarse");
                        }
                    }
                } catch (DbUpdateException dbUpdateException) {
                    MessageBox.Show("Error al registrar el Asistente.");
                    Console.WriteLine("DbUpdateException@RegistrarButton_Click -> " + dbUpdateException.Message);
                } catch (EntityException entityException) {
                    MessageBox.Show("Error al registrar el Asistente.");
                    Console.WriteLine("EntityException@RegistrarButton_Click -> " + entityException.Message);
                } catch (Exception exception) {
                    MessageBox.Show("Error al registrar el Asistente.");
                    Console.WriteLine("Exception@RegistrarButton_Click -> " + exception.Message);
                }
            }
        }

        /// <summary>
        /// Metodo que cierra la ventana
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelarButton_Click (object sender, RoutedEventArgs e) {
            MenuPrincipal ventanaMenu = new MenuPrincipal();
            ventanaMenu.Show();
            this.Close();
        }

        /// <summary>
        /// Metodo que carga la tabla de las actividades registradas para ese evento
        /// </summary>
        private void CargarTabla () {
            using (SigeaBD sigeaBD = new SigeaBD()) {
                try {
                    var eventoObtenido = sigeaBD.Evento.ToList().Find(
                        evento => evento.id_evento == Sesion.Evento.id_evento);
                    foreach (Actividad actividad in eventoObtenido.Actividad) {
                        ActividadesObservableCollection.Add(new ActividadTabla {
                            Seleccionado = false,
                            Nombre = actividad.nombre,
                            Descripcion = actividad.descripcion
                        });
                    }
                } catch (EntityException) {
                    MessageBox.Show("Error al cargar los proveedores.");
                } catch (Exception) {
                    MessageBox.Show("Error al cargar los proveedores.");
                }
            }
        }

        /// <summary>
        /// Struct para el llenado de las actividades
        /// </summary>
        public struct ActividadTabla {
            public bool Seleccionado { get; set; }
            public string Nombre { get; set; }
            public string Descripcion { get; set; }
        }

        /// <summary>
        /// Metodo que verifica que ningun campo este vacio
        /// </summary>
        /// <returns></returns>
        public Boolean VerificarCampos () {
            if (!string.IsNullOrWhiteSpace(nombreTextBox.Text) &&
                !string.IsNullOrWhiteSpace(paternoTextBox.Text) &&
                !string.IsNullOrWhiteSpace(maternoTextBox.Text) &&
                !string.IsNullOrWhiteSpace(correoTextBox.Text) &&
                !string.IsNullOrWhiteSpace(dependenciaTextBox.Text) &&
                !string.IsNullOrWhiteSpace(direccionTextBox.Text) &&
                !string.IsNullOrWhiteSpace(telefonoTextBox.Text) &&
                !string.IsNullOrWhiteSpace(puestoTextBox.Text)) {
                if (actividadDataGrid.SelectedIndex != -1) {
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

        /// <summary>
        /// Metodo que busca caracteres raros en los datos introducidos
        /// </summary>
        /// <returns></returns>
        private bool VerificarDatos () {
            if (Regex.IsMatch(nombreTextBox.Text, Herramientas.REGEX_SOLO_LETRAS) &&
                Regex.IsMatch(paternoTextBox.Text, Herramientas.REGEX_SOLO_LETRAS) &&
                Regex.IsMatch(maternoTextBox.Text, Herramientas.REGEX_SOLO_LETRAS) &&
                Regex.IsMatch(correoTextBox.Text, Herramientas.REGEX_CORREO) &&
                Regex.IsMatch(dependenciaTextBox.Text, Herramientas.REGEX_SOLO_LETRAS) &&
                Regex.IsMatch(telefonoTextBox.Text, Herramientas.REGEX_SOLO_NUMEROS) &&
                Regex.IsMatch(puestoTextBox.Text, Herramientas.REGEX_SOLO_LETRAS)) {
                return true;
            } else {
                MessageBox.Show("Por favor revise los datos ingresados");
                return false;
            }
        }
    }
}
