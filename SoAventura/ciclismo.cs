//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SoAventura
{
    using System;
    using System.Collections.Generic;
    
    public partial class ciclismo
    {
        public int Id_Evento { get; set; }
        public int ano { get; set; }
        public Nullable<int> distancia { get; set; }
    
        public virtual Evento_Desportivo Evento_Desportivo { get; set; }
    }
}