//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ZAdmin_DB.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class mapa_registro_empresa
    {
        public int idregistroempresa { get; set; }
        public Nullable<int> idregistro { get; set; }
        public Nullable<int> idempresa { get; set; }
        public Nullable<System.DateTime> dataregistro { get; set; }
    
        public virtual empresas empresas { get; set; }
        public virtual mapa_registros mapa_registros { get; set; }
    }
}
