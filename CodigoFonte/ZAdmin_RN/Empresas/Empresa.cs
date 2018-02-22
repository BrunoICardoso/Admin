using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAdmin_RN.Empresas
{
    public class Empresa
    {

        public int idempresa { get; set; }
        public int idsetor { get; set; }
        public string setorNome { get; set; }
        public string nome { get; set; }
        public string descricao { get; set; }
        public string urlsite { get; set; }
        public string caminhoImagem { get; set; }

        public IEnumerable<Cnpj> cnpjs { get; set; }
        public IEnumerable<RedeSocial> redessociais { get; set; }
        public List<Setor> setores { get; set; }

    }

    public class Setor
    {
        public int idsetor { get; set; }
        public string nome { get; set; }
    }


}
