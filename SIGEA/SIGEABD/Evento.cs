//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SIGEABD
{
    using System;
    using System.Collections.Generic;
    
    public partial class Evento
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Evento()
        {
            this.Actividad = new HashSet<Actividad>();
            this.Gasto = new HashSet<Gasto>();
            this.Track = new HashSet<Track>();
            this.Comite = new HashSet<Comite>();
        }
    
        public int id_evento { get; set; }
        public int id_organizador { get; set; }
        public string nombre { get; set; }
        public string tipo { get; set; }
        public System.DateTime fechaInicio { get; set; }
        public System.DateTime fechaFin { get; set; }
        public double cuota { get; set; }
        public string sede { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Actividad> Actividad { get; set; }
        public virtual Organizador Organizador { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Gasto> Gasto { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Track> Track { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comite> Comite { get; set; }
    }
}
