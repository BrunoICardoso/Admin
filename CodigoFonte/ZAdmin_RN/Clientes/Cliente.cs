using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAdmin_RN.Clientes
{
    public class Cliente
    {
        public int idcliente { get; set; }
        public int? idestado { get; set; }
        public string nome { get; set; }
        public string site { get; set; }
        public string endereco { get; set; }
        public string cidade { get; set; }
        public string telefone { get; set; }
        public string celular { get; set; }
        public string nomecontato { get; set; }
        public string cnpj { get; set; }
        public string email { get; set; }
        public string cep { get; set; }
        public string inscricaoEstadual { get; set; }
        public string inscricaoMunicipal { get; set; }
        public bool? vertodasempresas { get; set; }
        public string nomeEstado { get; set; }
        public List<string> vertentes { get; set; }
        public int quantidadeEmpresas { get; set; }
        public IEnumerable<int> listaEmpresas { get; set; }
        public DateTime? dataExpiracao { get; set; }
        //public Estado estado { get; set; }



    }

}
