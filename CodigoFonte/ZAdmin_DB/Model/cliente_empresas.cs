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
    
    public partial class cliente_empresas
    {
        public int idclienteempresa { get; set; }
        public Nullable<int> idcliente { get; set; }
        public Nullable<int> idempresa { get; set; }
        public Nullable<System.DateTime> dtcadastro { get; set; }
        public Nullable<bool> excluido { get; set; }
    
        public virtual empresas empresas { get; set; }
        public virtual clientes clientes { get; set; }
    }
}
