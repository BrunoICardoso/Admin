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
    
    public partial class tw_posts
    {
        public tw_posts()
        {
            this.promo_tw_posts = new HashSet<promo_tw_posts>();
        }
    
        public int idpost { get; set; }
        public Nullable<int> idperfil { get; set; }
        public string idtwitter { get; set; }
        public string postagem { get; set; }
        public Nullable<System.DateTime> datahora { get; set; }
        public Nullable<bool> is_retweet { get; set; }
        public Nullable<bool> is_reply { get; set; }
        public Nullable<int> qtdretweets { get; set; }
        public Nullable<int> qtdfavoritado { get; set; }
        public string idtwrespondido { get; set; }
        public string urlimagem { get; set; }
        public string nomeimagem { get; set; }
        public bool termoscapturados { get; set; }
    
        public virtual ICollection<promo_tw_posts> promo_tw_posts { get; set; }
        public virtual tw_perfis tw_perfis { get; set; }
    }
}
