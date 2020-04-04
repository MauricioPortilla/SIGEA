using System;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIGEABD;

namespace SIGEA.Pruebas {
    [TestClass]
    public class RegistrarComitePruebas {

        public ObservableCollection<Evento>
            EventosObservableCollection { get; } =
            new ObservableCollection<Evento>();

        public ObservableCollection<Organizador>
            OrganizadoresObservableCollection { get; } =
            new ObservableCollection<Organizador>();

        [TestMethod]
        public void RegistrarComitePrueba () {
            EventosObservableCollection.Add(new Evento {
                nombre = "Escuela de Verano IS",
                sede = "Facultad de estadistica e Informatica",
                cuota = 900.00,
                fechaInicio = new DateTime(25 / 6 / 2018),
                fechaFin = new DateTime(2 / 7 / 2018)
            });
            OrganizadoresObservableCollection.Add(new Organizador {
                nombre = "",
                paterno = "",
                materno = "",
                telefono = "2283085075",
                correo = "juancasu_900@hotmail.com"
            });
            Comite comite = new Comite {
                nombre = "Comite de Ventas",
                responsabilidades = "Llevar las ventas de la tienda de recuerdos del evento",
                Evento = EventosObservableCollection,
                Organizador = OrganizadoresObservableCollection
            };
            Assert.IsTrue(comite.Registrar());
        }
    }
}
