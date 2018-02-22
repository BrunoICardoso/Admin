using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZAdmin.Utils;

namespace ZAdmin.Controllers
{
    public class ProcessoAPIController : ApiController
    {
        public ViewModel.ProcessoListar ListarProcessosCaptura(ZAdmin_RN.Processo.ProcessoFiltro filtro)
        {

            var processoRN = new ZAdmin_RN.Processo.Processos();
            var promocaoLista = new ViewModel.ProcessoListar();

            //promocaoLista.Processo = processoRN.RetornarListCapturaProcesso(filtro);
            promocaoLista.Processo = processoRN.RetornarListCapturaProcessoElastic(filtro,Configuracoes.ServidorElastic);

            promocaoLista.TotalRegistros = processoRN.TotalRegistros;

            return promocaoLista;
        }

        public List<ZAdmin_RN.Processo.Situacao> GetListaSituacoes()
        {
            var situacaoRN = new ZAdmin_RN.Processo.Situacoes();
            return situacaoRN.RetornaLista();
        }

        public List<ZAdmin_RN.Empresas.Empresa> GetListaEmpresas()
        {
            var empresaRN = new ZAdmin_RN.Empresas.Empresas();
            return empresaRN.RetornaListaNomeDeEmpresas();
        }

        public void GetExcluirProcesso(int idProcesso)
        {
            var processoRN = new ZAdmin_RN.Processo.Processos();
            processoRN.Excluir(idProcesso);
            processoRN.ExcluirElastic(idProcesso, Configuracoes.ServidorElastic);            
        }

        public ZAdmin_RN.Processo.Processo GetProcesso(int idProcesso)
        {
            var RNProcessos = new ZAdmin_RN.Processo.Processos();
            return RNProcessos.RetornaProcesso(idProcesso);
        }

        public List<ZAdmin_RN.Combos.ItemCombo> GetItensComboEstados()
        {
            var RNProcessos = new ZAdmin_RN.Processo.Processos();
            return RNProcessos.RetornaItensComboEstados();
        }

        public List<ZAdmin_RN.Combos.ItemCombo> GetItensComboMunicipios()
        {
            var RNProcessos = new ZAdmin_RN.Processo.Processos();
            return RNProcessos.RetornaItensComboMunicipios();
        }

        public void SalvarPromocao(ZAdmin_RN.Processo.Processo processo)
        {

            var RNProcessos = new ZAdmin_RN.Processo.Processos();

            RNProcessos.AtualizaProcesso(processo);

        }

        public List<ZAdmin_RN.Processo.Arquivo> GetListaArquivos(int idProcesso)
        {
            var RNArquivos = new ZAdmin_RN.Processo.Arquivos();
            return RNArquivos.RetornaLista(idProcesso);
        }

        public void AdicionarDocumento(ZAdmin_RN.Processo.Arquivo arquivo)
        {
            var RNArquivo = new ZAdmin_RN.Processo.Processos();
            RNArquivo.InsertDocumento(arquivo);
        }

        [HttpPost]
        public void ExcluirDocumento(ZAdmin_RN.Processo.Arquivo Arquivo)
        {
            var RNarq = new ZAdmin_RN.Processo.Processos();
            RNarq.DeleteArquivo(Arquivo.idarquivo);
        }

        public List<ZAdmin_RN.Processo.SetorSubsetor> GetSetoresSubsetores(int idProcesso)
        {
            var RNsetores = new ZAdmin_RN.Processo.Processos();
            return RNsetores.RetornaSetoresSubSetores(idProcesso);
        }

        public List<ZAdmin_RN.Processo.Setor> GetSetores()
        {
            var RNsetores = new ZAdmin_RN.Processo.Setores();
            return RNsetores.RetornaSetores();
        }

        public List<ZAdmin_RN.Processo.Subsetor> GetSubsetores(int idSetor)
        {
            var RNsub = new ZAdmin_RN.Processo.Subsetores();
            return RNsub.RetornaSubsetores(idSetor);
        }
        
        public void AdicionarProcessoSetorSubsetor(ZAdmin_RN.Processo.ProcessoSetorSubsetor processoSetor)
        {
            var RN = new ZAdmin_RN.Processo.Processos();
            RN.InsertSetorSubsetor(processoSetor);
        }

        public void ExcluirSetorSubsetor(ZAdmin_RN.Processo.ProcessoSetorSubsetor processoSetor)
        {
            var Rn = new ZAdmin_RN.Processo.Processos();
            Rn.DeleteProcessoSetorSubsetor(processoSetor);
        }


        public List<ZAdmin_RN.Processo.SituacaoMovimento> RetornaSituacoes(int idProcesso) {


            var RNsi = new ZAdmin_RN.Processo.Processos();

            return RNsi.RetornaListarSituacoes(idProcesso);

        }


        public List<ZAdmin_RN.Processo.Situacao> GetComboSituacoes()
        {


            var RNsi = new ZAdmin_RN.Processo.Processos();

            return RNsi.RetornarComboSituacao();

        }


       public void ExcluirSituacao(ZAdmin_RN.Processo.SituacaoMovimento sm)
        {
            var RNs = new ZAdmin_RN.Processo.Processos();

            RNs.DeletaSituacao(sm);
        }


        public void AdicionarMovSituacao(ZAdmin_RN.Processo.SituacaoMovimento sm)
        {
            var RN = new ZAdmin_RN.Processo.Processos();
            RN.InsertMovSituacao(sm);
        }


        



    }
}
