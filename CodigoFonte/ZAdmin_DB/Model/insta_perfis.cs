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
    
    public partial class insta_perfis
    {
        public insta_perfis()
        {
            this.insta_posts = new HashSet<insta_posts>();
        }
    
        public int idperfil { get; set; }
        public Nullable<int> idempresa { get; set; }
        public Nullable<int> idmarca { get; set; }
        public string nome { get; set; }
        public string idinstagram { get; set; }
        public string username { get; set; }
        public string sobre { get; set; }
        public string website { get; set; }
        public string urlimagem { get; set; }
        public string nomeimagem { get; set; }
        public Nullable<System.DateTime> datahoraultimopost { get; set; }
        public Nullable<System.DateTime> ultimaatualizacao { get; set; }
        public string link { get; set; }
    
        public virtual empresas empresas { get; set; }
        public virtual ICollection<insta_posts> insta_posts { get; set; }
        public virtual marcas marcas { get; set; }
    }
}