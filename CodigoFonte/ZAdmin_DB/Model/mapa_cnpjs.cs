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
    
    public partial class mapa_cnpjs
    {
        public int idcnpj { get; set; }
        public string cnpj { get; set; }
        public Nullable<System.DateTime> datacadastro { get; set; }
        public Nullable<bool> ativo { get; set; }
    }
}