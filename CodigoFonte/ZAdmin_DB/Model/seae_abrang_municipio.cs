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
    
    public partial class seae_abrang_municipio
    {
        public int idabrangmunicipio { get; set; }
        public Nullable<int> idprocesso { get; set; }
        public Nullable<int> idmunicipio { get; set; }
    
        public virtual seae_processos seae_processos { get; set; }
        public virtual municipios municipios { get; set; }
    }
}