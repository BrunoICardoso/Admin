using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAdmin_RN.Produtos
{
    public class Produto
    {
        
        public int idproduto { get; set; }
        public int? idmarca { get; set; }
        public int? idempresa { get; set; }
        public string marcanome { get; set; }
        public string nome { get; set; }
        public string descricao { get; set; }
        public string urlsite { get; set; }
        public string caminhoimagem { get; set; }

        public IEnumerable<RedeSocial> redessocias { get; set; }
    }
}
