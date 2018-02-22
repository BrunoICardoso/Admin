using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAdmin_RN.Promocao
{
    public class Promocao
    {
     
        public int idPromocao { get; set; }
        public int? idOrgaoregulador { get; set; }
        public int? idModalidade { get; set; }
        public string nome { get; set; }
        public string certificadoAutorizacao { get; set; }
        public string outrosInteressados { get; set; }
        public bool? abrangenciaNacional { get; set; }
        public DateTime? dtCadastro { get; set; }
        public DateTime? dtVigenciaIni { get; set; }
        public DateTime? dtVigenciaFim { get; set; }
        public string linkSitePromocao { get; set; }
        public string linkFacebook { get; set; }
        public string linkInstagram { get; set; }
        public string linkTwitter { get; set; }
        public string linkYoutube { get; set; }
        public string mecanicaPromo { get; set; }
        public string produtosParticipantes { get; set; }
        public string premiosPromo { get; set; }
        public decimal? valorPremio { get; set; }
        public string linkRegulamento { get; set; }
        public string textoRegulamento { get; set; }
        public bool excluido { get; set; }
        public string numeroProcesso { get; set; }
        public int idProcesso { get; set; }
        public int idRedeSocial { get; set; }
        public string nomeRedeSocial { get; set; }
        public int idNoticia { get; set; }
        public int fontePesquisa { get; set; }
        public IEnumerable<PromocaoEmpresa> Empresas { get; set; }
        public IEnumerable<PromocaoEstado> Estados { get; set; }
        public IEnumerable<PromocaoMunicipio> Municipios { get; set; }
        public IEnumerable<PromocaoArquivoSeae> ArquivoSeae { get; set; }
        public List<int> ListaEmpresas { get; set; }
        public List<int> ListaEstados { get; set; }
        public List<int> ListaMunicipios { get; set; }


    }

}