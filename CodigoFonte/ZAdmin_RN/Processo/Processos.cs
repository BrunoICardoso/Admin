using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAdmin_RN.Processo
{
    public class Processos
    {

        ZAdmin_DB.Model.zeengEntities db = new ZAdmin_DB.Model.zeengEntities();

        private int _totalRegistros;

        public int TotalRegistros
        {
            get { return _totalRegistros; }
            set { _totalRegistros = value; }
        }

        public List<Processo> RetornarListCapturaProcesso(ProcessoFiltro filtro)
        {

            var listaProcesso = (from sp in db.seae_processos

                                 where
                                    sp.dtprocesso >= filtro.dataIni && sp.dtprocesso <= filtro.dataFim &&
                                    sp.interessados.Contains(filtro.palavrasChave) &&
                                    //(
                                    //    filtro.situacao > 0 ?
                                    //    sp.seae_mov_situacao.Where(x => x.idprocesso == sp.idprocesso && x.idsituacao == filtro.situacao).OrderByDescending(x => x.dtsituacao).FirstOrDefault().idsituacao == filtro.situacao :
                                    //    true
                                    //) &&
                                    //sp.premios.Contains(filtro.palavrasChave) &&

                                    //sp.seae_processo_solicitantes.Where(x => x.solicitante.Contains(filtro.palavrasChave)).FirstOrDefault().cnpj != "" &&

                                    //sp.numprocesso == filtro.numProcesso &&

                                    //(
                                    //    from cnpjEmp in db.cnpjempresa
                                    //    join cnpjSolicitante in sp.seae_processo_solicitantes on cnpjEmp.cnpj equals cnpjSolicitante.cnpj
                                    //    where cnpjEmp.idempresa == filtro.empresa
                                    //    select cnpjSolicitante
                                    //).FirstOrDefault().cnpj != "" &&

                                    sp.excluido == false

                                 select new Processo
                                 {
                                     idprocesso = sp.idprocesso,
                                     dtprocesso = sp.dtprocesso,
                                     numprocesso = sp.numprocesso,
                                     interessados = sp.interessados,
                                     numdoc = sp.numdocs,
                                     situacoes = sp.seae_mov_situacao.Select(s => new Situacao() { data = s.dtsituacao, descricao = s.seae_situacoes.descricao, idsituacao = s.idsituacao }),

                                     arquivos = sp.seae_arquivos_proc.Select(x => new Arquivo()
                                     {
                                         idarquivo = x.idarquivo,
                                         numdoc = x.numdoc,
                                         coordenacao = x.coordenacao,
                                         situacao = x.situacao,
                                         link = x.link
                                     }),

                                     setores = sp.seae_processo_setor.Select(s => new Setor() { codsetor = s.seae_setores_proc.codsetor, idsetor = s.idsetor, descricao = s.seae_setores_proc.descricao })

                                 }
                            );

            this.TotalRegistros = listaProcesso.Count();

            if (filtro.ordenacao == "DtRegistroDESC")
                return listaProcesso.OrderByDescending(x => x.dtprocesso).Skip((filtro.pagina - 1) * filtro.qtdRegistros).Take(filtro.qtdRegistros).ToList();
            else if (filtro.ordenacao == "DtRegistroASC")
                return listaProcesso.OrderBy(x => x.dtprocesso).Skip((filtro.pagina - 1) * filtro.qtdRegistros).Take(filtro.qtdRegistros).ToList();
            else if (filtro.ordenacao == "DtSituacaoDESC")
                return listaProcesso.OrderBy(x => x.dtprocesso).Skip((filtro.pagina - 1) * filtro.qtdRegistros).Take(filtro.qtdRegistros).ToList();
            else return listaProcesso.OrderByDescending(x => x.dtprocesso).Skip((filtro.pagina - 1) * filtro.qtdRegistros).Take(filtro.qtdRegistros).ToList();
        }

        public List<ZAdmin_RN.ProcessoElastic.Processos_Seae> RetornarListCapturaProcessoElastic(ProcessoFiltro filtro, string servidorElasstic)
        {
            var node = new Uri(servidorElasstic);
            var settings = new ConnectionSettings(node);
            settings.DisableDirectStreaming(true);
            settings.DefaultIndex("promocoes");

            var client = new ElasticClient(settings);

            if (filtro.dataIni == null)
            {
                filtro.dataIni = new DateTime(1900, 01, 01);
            }

            if (filtro.dataFim == null)
            {
                filtro.dataFim = new DateTime(2200, 01, 01);
            }

            var elastic = client.Search<ZAdmin_RN.ProcessoElastic.Processos_Seae>(s => s
                                    .Size(filtro.qtdRegistros)
                                    .Skip((filtro.pagina - 1) * filtro.qtdRegistros)
                                    //.Take(filtro.qtdRegistros)
                                    .Query(q =>
                                  
                                        (
                                            q.QueryString(qs => qs
                                                                .DefaultField(f => f.numprocesso)
                                                                .Query(filtro.palavrasChave.Replace("/","//"))
                                                                .DefaultOperator(Operator.Or)
                                            )

                                            ||

                                            q.QueryString(qs => qs
                                                            .Fields(f => f
                                                                        .Field("comoparticipar")
                                                                        .Field("interessados")
                                                                        .Field("modalidade")
                                                                        .Field("nome")
                                                                        .Field("premios")
                                                                        .Field("situacaoatual")
                                                                        .Field("arquivos.textoarquivo")
                                                            )
                                                            .Query(filtro.palavrasChave.Replace("/", "//"))
                                                            .DefaultOperator(Operator.And)
                                            )
                                        )
                                    
                                        &&                                    
                                        (filtro.idempresa != 0 ? q.Term("empresas.idempresa", filtro.idempresa) : null)
                                    
                                        &&
                                        q.DateRange(d => d

                                                    .Field(f => f.dtprocesso)
                                                    .GreaterThanOrEquals(string.Format("{0:yyyy-MM-dd}", filtro.dataIni))
                                                    .LessThanOrEquals(string.Format("{0:yyyy-MM-dd}", filtro.dataFim))

                                        )

                                    )
                                    .Sort(x => filtro.ordenacao == "DtRegistroDESC" ?
                                                    x.Descending(c => c.dtprocesso): 
                                                filtro.ordenacao == "DtRegistroASC" ? 
                                                    x.Ascending(c => c.dtprocesso) : 
                                                    x.Descending(c => c.datasituacaoatual)
                                          )

                          );

            //if (filtro.ordenacao == "DtRegistroDESC")
            //    elastic.Documents.OrderByDescending(x => x.dtprocesso);
            //else if (filtro.ordenacao == "DtRegistroASC")
            //    elastic.Documents.OrderBy(x => x.dtprocesso);
            ////else if (filtro.ordenacao == "DtSituacaoDESC")
            ////    elastic.Documents.Select(x => x.situacoes.OrderByDescending(c => c.data));
            //else
            //    elastic.Documents.OrderByDescending(x => x.dtprocesso);

            //.Sort(x => x.Descending(c => c.dtprocesso))
            /*
            if (filtro.ordenacao == "DtRegistroDESC")
                elastic.Sort(x => x.Descending(c => c.dtprocesso));
            else if (filtro.ordenacao == "DtRegistroASC")
                return lista.OrderBy(x => x.dtprocesso).Skip((filtro.pagina - 1) * filtro.qtdRegistros).Take(filtro.qtdRegistros).ToList();
            else if (filtro.ordenacao == "DtSituacaoDESC")
                return lista.OrderByDescending(x => x.situacoes.Select(c => c.data)).Skip((filtro.pagina - 1) * filtro.qtdRegistros).Take(filtro.qtdRegistros).ToList();
                */

            var lista = (
                            from p in elastic.Documents
                            select new ZAdmin_RN.ProcessoElastic.Processos_Seae()
                            {
                                abrangencia_nacional = p.abrangencia_nacional,
                                abrangestados = p.abrangestados,
                                abrangmunicipios = p.abrangmunicipios,
                                arquivos = p.arquivos,
                                comoparticipar = p.comoparticipar,
                                dtprocesso = p.dtprocesso,
                                dtvigenciafim = p.dtvigenciafim,
                                dtvigenciaini = p.dtvigenciaini,
                                empresas = p.empresas,
                                formacontemplacao = p.formacontemplacao,
                                idprocesso = p.idprocesso,
                                interessados = p.interessados,
                                modalidade = p.modalidade,
                                nome = p.nome,
                                numdocs = p.numdocs,
                                numprocesso = p.numprocesso,
                                premios = p.premios,
                                resumo = p.resumo,
                                setores = p.setores,
                                situacaoatual = p.situacaoatual,
                                //situacoes = p.situacoes,
                                solicitantes = p.solicitantes,
                                valortotalpremios = p.valortotalpremios
                            }
                ).ToList();

            this.TotalRegistros = Convert.ToInt32(elastic.Total); // lista.Count();
            return lista;//.Skip((filtro.pagina - 1) * filtro.qtdRegistros).Take(filtro.qtdRegistros).ToList();
        }

        public void Excluir(int idProc)
        {
            try
            {
                var processo = db.seae_processos.Where(x => x.idprocesso == idProc).FirstOrDefault();

                if (processo != null)
                {
                    processo.excluido = true;
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ExcluirElastic(int idProc, string servidorElasstic)
        {
            try
            {
                var node = new Uri(servidorElasstic);
                var settings = new ConnectionSettings(node);
                settings.DisableDirectStreaming(true);
                settings.DefaultIndex("promocoes");

                var client = new ElasticClient(settings);

                client.DeleteByQuery<ZAdmin_RN.ProcessoElastic.Processos_Seae>(q => q
                                                                                   .Query(x => x
                                                                                       .Term("idprocesso", idProc)
                                                                                       )
                                                                                    );
            }
            catch (Exception)
            {
                throw;
            }

        }

        public List<Combos.ItemCombo> RetornaItensComboEstados()
        {
            var estadosDB = (from estado in db.estados
                             orderby estado.nome ascending
                             select new Combos.ItemCombo()
                             {
                                 idItem = estado.idestado,
                                 nome = estado.nome
                             }).ToList();

            return estadosDB;
        }

        public List<Combos.ItemCombo> RetornaItensComboMunicipios()
        {

            var municipiosDB = (from m in db.municipios
                                join e in db.estados on m.idestado equals e.idestado
                                orderby e.uf descending
                                select new Combos.ItemCombo()
                                {
                                    idItem = m.idmunicipio,
                                    nome = m.nome + "/" + e.uf 
                                }).ToList();

            return municipiosDB;
        }

        public Processo RetornaProcesso(int? idProcesso)
        {
            if (idProcesso == null)
                return new Processo();

            var processoDB = (from processo in db.seae_processos
                              where processo.idprocesso == idProcesso &&
                                  processo.excluido == false
                              select new Processo()
                              {
                                  idprocesso = processo.idprocesso,
                                  nome = processo.nome != null ? processo.nome : "",
                                  numprocesso = processo.numprocesso != null ? processo.numprocesso : "",
                                  dtprocesso = processo.dtprocesso,
                                  dtVigenciaIni = processo.dtvigenciaini,
                                  dtVigenciaFim = processo.dtvigenciafim,
                                  abrangenciaNacional = processo.abrangencia_nacional,
                                  valorTotalPremios = processo.valortotalpremios,
                                  modalidade = processo.modalidade != null ? processo.modalidade : "",
                                  formaContemplacao = processo.formacontemplacao != null ? processo.formacontemplacao : "",
                                  interessados = processo.interessados != null ? processo.interessados : "",
                                  resumo = processo.resumo != null ? processo.resumo : "",
                                  comoParticipar = processo.comoparticipar != null ? processo.comoparticipar : "",
                                  premios = processo.premios != null ? processo.premios : "",
                                  empresas = processo.seae_empresa_processos.Select(x => x.idempresa),
                                  estados = processo.seae_abrang_estado.Select(e => e.idestado),
                                  municipios = processo.seae_abrang_municipio.Select(m => m.idmunicipio)
                              }).FirstOrDefault();

            //if(processoDB != null) { }

            return processoDB;

        }

        public void AtualizaProcesso(Processo processo)
        {


            var processoDB = (from p in db.seae_processos
                              where p.idprocesso == processo.idprocesso
                              select p).FirstOrDefault();

            processoDB.nome = processo.nome.Trim();
            processoDB.numprocesso = processo.numprocesso.Trim();
            processoDB.dtprocesso = processo.dtprocesso;
            processoDB.dtvigenciaini = processo.dtVigenciaIni;
            processoDB.dtvigenciafim = processo.dtVigenciaFim;
            processoDB.valortotalpremios = processo.valorTotalPremios;
            processoDB.modalidade = processo.modalidade.Trim();
            processoDB.formacontemplacao = processo.formaContemplacao.Trim();
            processoDB.interessados = processo.interessados.Trim();
            processoDB.resumo = processo.resumo.Trim();
            processoDB.comoparticipar = processo.comoParticipar.Trim();
            processoDB.premios = processo.premios.Trim();
            processoDB.abrangencia_nacional = processo.abrangenciaNacional;
            processoDB.atualizarElastic = true;

            //empresas



            var listaIdEmpresaDB = db.seae_empresa_processos.Where(x => x.idprocesso == processo.idprocesso).Select(y => y.idempresa).ToList();

            var resultadoRemoveu = processo.empresas == null ? listaIdEmpresaDB : listaIdEmpresaDB.Except(processo.empresas);

            var resultadoAdicionou = processo.empresas == null ? listaIdEmpresaDB.Except(listaIdEmpresaDB) : processo.empresas.Except(listaIdEmpresaDB);


            if (resultadoRemoveu.Any())
            {

                var empresasOld = resultadoRemoveu.ToList();

                var resultado = db.seae_empresa_processos.Where(x => empresasOld.Contains(x.idempresa)
                                                                && x.idprocesso == processo.idprocesso)
                                                         .Select(x => x);

                foreach (var empSeae in resultado)
                {
                    db.seae_empresa_processos.Remove(empSeae);
                }


            }

            if (resultadoAdicionou.Any())
            {

                var novasEmpresas = resultadoAdicionou.ToList();


                foreach (var empresa in novasEmpresas)
                {

                    var seaeEmpresaDB = new ZAdmin_DB.Model.seae_empresa_processos();

                    seaeEmpresaDB.idempresa = empresa.Value;
                    seaeEmpresaDB.idprocesso = processo.idprocesso;

                    db.seae_empresa_processos.Add(seaeEmpresaDB);
                }

            }



            //estados


            var listaIdEstadosDB = db.seae_abrang_estado.Where(x => x.idprocesso == processo.idprocesso).Select(y => y.idestado).ToList();

            var resultadoEstadosRemoveu = processo.estados == null ? listaIdEstadosDB : listaIdEstadosDB.Except(processo.estados);

            var resultadoEstadosAdicionou = processo.estados == null ? listaIdEstadosDB.Except(listaIdEstadosDB) : processo.estados.Except(listaIdEstadosDB);

            if (resultadoEstadosRemoveu.Any())
            {

                var estadosOld = resultadoEstadosRemoveu.ToList();

                var resultado = db.seae_abrang_estado.Where(x => estadosOld.Contains(x.idestado)
                                                             && x.idprocesso == processo.idprocesso)
                                                             .Select(x => x);

                foreach (var estadosSeae in resultado)
                {
                    db.seae_abrang_estado.Remove(estadosSeae);
                }


            }

            if (resultadoEstadosAdicionou.Any())
            {

                var novosEstados = resultadoEstadosAdicionou.ToList();

                foreach (var estado in novosEstados)
                {

                    var seaeEstadoDB = new ZAdmin_DB.Model.seae_abrang_estado();

                    seaeEstadoDB.idestado = estado.Value;
                    seaeEstadoDB.idprocesso = processo.idprocesso;

                    db.seae_abrang_estado.Add(seaeEstadoDB);

                }



            }


            //municipios



            var listaIdMunicipiosDB = db.seae_abrang_municipio.Where(x => x.idprocesso == processo.idprocesso).Select(y => y.idmunicipio).ToList();

            var resultadoMunicipiosRemoveu = processo.municipios == null ? listaIdMunicipiosDB : listaIdMunicipiosDB.Except(processo.municipios);

            var resultadoMunicipiosAdicionou = processo.municipios == null ? listaIdMunicipiosDB.Except(listaIdMunicipiosDB) : processo.municipios.Except(listaIdMunicipiosDB);

            if (resultadoMunicipiosRemoveu.Any())
            {

                var municipiosOld = resultadoMunicipiosRemoveu.ToList();

                var resultado = db.seae_abrang_municipio.Where(x => municipiosOld.Contains(x.idmunicipio)
                                                                && x.idprocesso == processo.idprocesso)
                                                                .Select(x => x);

                foreach (var municipioSeae in resultado)
                {
                    db.seae_abrang_municipio.Remove(municipioSeae);
                }

            }

            if (resultadoMunicipiosAdicionou.Any())
            {

                var novosMunicipios = resultadoMunicipiosAdicionou.ToList();

                foreach (var municipio in novosMunicipios)
                {
                    var seaeMunicipioDB = new ZAdmin_DB.Model.seae_abrang_municipio();

                    seaeMunicipioDB.idmunicipio = municipio.Value;
                    seaeMunicipioDB.idprocesso = processo.idprocesso;

                    db.seae_abrang_municipio.Add(seaeMunicipioDB);
                }

            }





            db.SaveChanges();

        }

        public void InsertDocumento(Arquivo arquivo)
        {
            var objArquivo = new ZAdmin_DB.Model.seae_arquivos_proc();
            objArquivo.coordenacao = arquivo.coordenacao.Trim();
            objArquivo.excluido = false;
            objArquivo.idprocesso = arquivo.idprocesso;
            objArquivo.link = arquivo.link.Trim();
            objArquivo.nomearquivo = arquivo.nomearquivo.Trim();
            objArquivo.situacao = arquivo.situacao.Trim();
            objArquivo.textoarquivo = arquivo.textoarquivo.Trim();
            objArquivo.numdoc = arquivo.numdoc.Trim();

            db.seae_arquivos_proc.Add(objArquivo);
            db.SaveChanges();
        }

        public void DeleteArquivo(int idArquivo)
        {
            try
            {
                var arquivo = db.seae_arquivos_proc.Where(x => x.idarquivo == idArquivo).FirstOrDefault();
                if (arquivo != null)
                {
                    arquivo.excluido = true;
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ZAdmin_RN.Processo.SetorSubsetor> RetornaSetoresSubSetores(int idProcesso)
        {
            return (from ps in db.seae_processo_setor
                    join s in db.seae_setores_proc on ps.idsetor equals s.idsetor
                    join sb in db.seae_subsetores_proc on ps.idsubsetor equals sb.idsubsetor
                    orderby s.descricao,sb.descricao ascending
                    where ps.idprocesso == idProcesso
                    
                    select new ZAdmin_RN.Processo.SetorSubsetor()
                    {
                        idsetor = s.idsetor,
                        codsetor = s.codsetor,
                        descricaoSetor = s.descricao,
                        idsubsetor = sb.idsubsetor,
                        codsubsetor = sb.codsubsetor,
                        descricaoSubSetor = sb.descricao,
                        idprocessosetor = ps.idprocessosetor
                    }
                ).ToList();
        }

        public void InsertSetorSubsetor(ZAdmin_RN.Processo.ProcessoSetorSubsetor processoSetor)
        {
            var procSetorSub = new ZAdmin_DB.Model.seae_processo_setor();
            procSetorSub.idprocesso = processoSetor.idprocesso;
            procSetorSub.idsetor = processoSetor.idsetor;
            procSetorSub.idsubsetor = processoSetor.idsubsetor;

            db.seae_processo_setor.Add(procSetorSub);
            db.SaveChanges();
        }

        public void DeleteProcessoSetorSubsetor(ZAdmin_RN.Processo.ProcessoSetorSubsetor processoSetor)
        {
            try
            {
                var processoSetorSub = db.seae_processo_setor.Where(x => x.idprocessosetor == processoSetor.idprocessosetor).FirstOrDefault();
                if(processoSetorSub != null)
                {
                    db.seae_processo_setor.Attach(processoSetorSub);
                    db.seae_processo_setor.Remove(processoSetorSub);
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public List<ZAdmin_RN.Processo.SituacaoMovimento> RetornaListarSituacoes(int idProcesso) {

            var listarsituacao = (from sp in db.seae_processos
                                  join sm in db.seae_mov_situacao on sp.idprocesso equals sm.idprocesso
                                  join ss in db.seae_situacoes on sm.idsituacao equals ss.idsituacao
                                  where sp.idprocesso == idProcesso
                                  select new SituacaoMovimento() {
                                      idmovsituacao = sm.idmovsituacao,
                                      idsituacao = ss.idsituacao,
                                      descricao  = ss.descricao,
                                      idprocesso = sm.idprocesso

                                  }
                                  ).ToList();

            return listarsituacao;

        }



        public List<ZAdmin_RN.Processo.Situacao> RetornarComboSituacao() {

            var combosituacao = (from ss in db.seae_situacoes
                                 orderby ss.descricao ascending
                                 select new Situacao() {

                                     idsituacao = ss.idsituacao,
                                     descricao = ss.descricao

                                  }).ToList();

            return combosituacao;
        }



        public void DeletaSituacao(ZAdmin_RN.Processo.SituacaoMovimento situacao) {

            try
            {

                var proSituacao = db.seae_mov_situacao.Where(x => x.idmovsituacao == situacao.idmovsituacao).FirstOrDefault();

                if (proSituacao != null)
                {

                    db.seae_mov_situacao.Attach(proSituacao);
                    db.seae_mov_situacao.Remove(proSituacao);
                    db.SaveChanges();


                }

            }
            catch (Exception)
            {
                throw;
            }

        }


        public void InsertMovSituacao(ZAdmin_RN.Processo.SituacaoMovimento sm)
        {
            var dbsm = new ZAdmin_DB.Model.seae_mov_situacao();

            dbsm.idprocesso = sm.idprocesso;
            dbsm.idsituacao = sm.idsituacao;
            dbsm.dtsituacao = sm.dtsituacao;
            
            db.seae_mov_situacao.Add(dbsm);
            db.SaveChanges();
        }



    }

}


