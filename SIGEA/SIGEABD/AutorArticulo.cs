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
    
    public partial class AutorArticulo
    {
        public int id_autor { get; set; }
        public int id_articulo { get; set; }
        public System.DateTime fecha { get; set; }
    
        public virtual Articulo Articulo { get; set; }
        public virtual Autor Autor { get; set; }
    }
}
