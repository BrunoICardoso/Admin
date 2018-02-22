using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAdmin_RN.Marcas
{
    public class Marca
    {

        public int idMarca { get; set; }
        public int? idEmpresa { get; set; }
        public string EmpresaNome { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string UrlSite { get; set; }
        public string caminhoImagem { get; set; }

        public IEnumerable<Cnpj> cnpjs { get; set; }
        public IEnumerable<RedeSocial> RedesSociais { get; set; }

    }
}
