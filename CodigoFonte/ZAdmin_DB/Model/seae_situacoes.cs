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
    
    public partial class seae_situacoes
    {
        public seae_situacoes()
        {
            this.seae_mov_situacao = new HashSet<seae_mov_situacao>();
        }
    
        public int idsituacao { get; set; }
        public string descricao { get; set; }
    
        public virtual ICollection<seae_mov_situacao> seae_mov_situacao { get; set; }
    }
}
