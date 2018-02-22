using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZAdmin_RN.Marcas;

namespace ZAdmin_RN.MapaRegistros
{
    public class MapaRegistros
    {

        private int _totalRegistros;

        public int TotalRegistros
        {
            get { return _totalRegistros; }
            set { _totalRegistros = value; }
        }

        public List<MapaRegistro> RetornaListaDeMapaRegistros(FiltroMapa filtro)
        {            
            var db = new ZAdmin_DB.Model.zeengEntities();

            DateTime dataZeroIni = new DateTime(0001, 01, 01, 00, 00, 00);
            DateTime dataFim = new DateTime(2200, 01, 01, 00, 00, 00);

            filtro.dataInicial = filtro.dataInicial != null ? Convert.ToDateTime(string.Format("{0:yyyy/MM/dd 00:00:00}", filtro.dataInicial)) : dataZeroIni;
            filtro.dataFinal = filtro.dataFinal != null ? Convert.ToDateTime(string.Format("{0:yyyy/MM/dd 23:59:59}", filtro.dataFinal)) : dataFim;

            _totalRegistros = (from r in db.mapa_registros
                                join me in db.mapa_registro_empresa on r.idregistro equals me.idregistro into mapaemp
                                from leftMapaEmp in mapaemp.DefaultIfEmpty()                               
                                where (r.excluido == false) &&
                                        ( filtro.numeroRegistro.Trim() != "" ? r.numregistro.Trim().Contains(filtro.numeroRegistro.Trim()) : 1==1 ) &&
                                        ( filtro.nomeMarca.Trim() != "" ?  r.nomeMarca.Trim() == filtro.nomeMarca.Trim() : 1==1) &&
                                        ( filtro.dataInicial != null ? filtro.dataInicial <= r.dataconcessao : r.dataconcessao <= dataZeroIni ) &&
                                        ( filtro.dataFinal != null ? filtro.dataFinal >= r.dataconcessao : r.dataconcessao >= dataFim) &&
                                        (filtro.Estados != null ? filtro.Estados == r.idestado : 1==1) &&
                                        ( filtro.Areas != null ? filtro.Areas == r.idarea : 1==1) &&
                                        ( filtro.Especies != null ? filtro.Especies == r.idespecie : 1==1) &&
                                        ( filtro.Subespecies != null ? filtro.Subespecies == r.idsubespecie : 1==1) &&
                                        ( filtro.Bases != null ? filtro.Bases == r.idbase : 1==1) &&
                                        ( filtro.Caracteristicas != null ? filtro.Caracteristicas == r.idcaracteristica : 1==1) &&
                                        ( filtro.Atributos != null ? filtro.Atributos == r.idatributo : 1==1) &&
                                        ( filtro.Complementos != null ? filtro.Complementos == r.idcomplemento : 1==1) &&
                                        ( filtro.Origens != null ? filtro.Origens == r.idorigem : 1==1) &&
                                        ( filtro.Empresas != null ? leftMapaEmp.idempresa == filtro.Empresas : true )
                                select r).Count();

            var registros = (from r in db.mapa_registros
                             join me in db.mapa_registro_empresa on r.idregistro equals me.idregistro into mapaemp
                             from leftMapaEmp in mapaemp.DefaultIfEmpty()
                             where (r.excluido == false) &&
                                   (filtro.numeroRegistro.Trim() != "" ? r.numregistro.Trim().Contains(filtro.numeroRegistro.Trim()) : 1 == 1) &&
                                     (filtro.nomeMarca.Trim() != "" ? r.nomeMarca.Trim() == filtro.nomeMarca.Trim() : 1 == 1) &&
                                     (filtro.dataInicial != null ? filtro.dataInicial <= r.dataconcessao : r.dataconcessao <= dataZeroIni) &&
                                     (filtro.dataFinal != null ? filtro.dataFinal >= r.dataconcessao : r.dataconcessao >= dataFim) &&
                                     (filtro.Estados != null ? filtro.Estados == r.idestado : 1 == 1) &&
                                     (filtro.Areas != null ? filtro.Areas == r.idarea : 1 == 1) &&
                                     (filtro.Especies != null ? filtro.Especies == r.idespecie : 1 == 1) &&
                                     (filtro.Subespecies != null ? filtro.Subespecies == r.idsubespecie : 1 == 1) &&
                                     (filtro.Bases != null ? filtro.Bases == r.idbase : 1 == 1) &&
                                     (filtro.Caracteristicas != null ? filtro.Caracteristicas == r.idcaracteristica : 1 == 1) &&
                                     (filtro.Atributos != null ? filtro.Atributos == r.idatributo : 1 == 1) &&
                                     (filtro.Complementos != null ? filtro.Complementos == r.idcomplemento : 1 == 1) &&
                                     (filtro.Origens != null ? filtro.Origens == r.idorigem : 1 == 1) &&
                                     (filtro.Empresas != null ? leftMapaEmp.idempresa == filtro.Empresas : true)

                             select r).OrderBy(x => x.nomeMarca).Skip((filtro.pagina - 1) * filtro.qtdRegistros).Take(filtro.qtdRegistros);

            var listaDeRegistros = (from registro in registros
                                    select new MapaRegistro()
                                    {
                                    idRegistro = registro.idregistro,
                                    idEstado = registro.idestado,
                                    idArea = registro.idarea,
                                    idEspecie = registro.idespecie,
                                    idSubEspecie = registro.idsubespecie,
                                    idBase = registro.idbase,
                                    idCaracteristica = registro.idcaracteristica,
                                    idAtributo = registro.idatributo,
                                    idComplemento = registro.idcomplemento,
                                    idOrigem = registro.idorigem,
                                    nomeMarca = registro.nomeMarca,
                                    nomeProduto = registro.nomeProduto,
                                    dataConcessao = registro.dataconcessao,
                                    numeroRegistro = registro.numregistro,
                                    modoAplicacao = registro.modoaplicacao,
                                    nomeEspecie = registro.mapa_especies.nome,
                                    nomeSubespecie = registro.mapa_subespecie.nome,
                                    status = registro.status,
                                    }).ToList();

            return listaDeRegistros;
        }

        public MapaRegistro RetornaMapaRegistro(int idRegistro)
        {

            var db = new ZAdmin_DB.Model.zeengEntities();

            var registroDB = (from registro in db.mapa_registros
                              where registro.idregistro == idRegistro
                              select new MapaRegistro()
                              {
                                  cnpj = registro.cnpj,
                                  idRegistro = registro.idregistro,
                                  idArea = registro.idarea,
                                  idAtributo = registro.idatributo,
                                  idBase = registro.idbase,
                                  idCaracteristica = registro.idcaracteristica,
                                  idComplemento = registro.idcomplemento,
                                  idEspecie = registro.idespecie,
                                  idEstado = registro.idestado,
                                  idOrigem = registro.idorigem,
                                  idSubEspecie = registro.idsubespecie,
                                  dataConcessao = registro.dataconcessao,
                                  modoAplicacao = registro.modoaplicacao,
                                  nomeMarca = registro.nomeMarca,
                                  nomeProduto = registro.nomeProduto,
                                  numeroRegistro = registro.numregistro,
                                  status = registro.status
                              }).FirstOrDefault();


            registroDB.listaEmpresas = db.mapa_registro_empresa.Where(me => me.idregistro == registroDB.idRegistro).Select(v => v.idempresa.Value).ToList();
            

            return registroDB;
        }

        public void Cadastrar(MapaRegistro registro)
        {

            var db = new ZAdmin_DB.Model.zeengEntities();

            var registroDB = new ZAdmin_DB.Model.mapa_registros();

            registroDB.cnpj = registro.cnpj.Trim();
            registroDB.idestado = registro.idEstado;
            registroDB.idarea = registro.idArea;
            registroDB.idespecie = registro.idEspecie;
            registroDB.idsubespecie = registro.idSubEspecie;
            registroDB.idbase = registro.idBase;
            registroDB.idcaracteristica = registro.idCaracteristica;
            registroDB.idatributo = registro.idAtributo;
            registroDB.idcomplemento = registro.idComplemento;
            registroDB.idorigem = registro.idOrigem;
            registroDB.nomeMarca = registro.nomeMarca != null ? registro.nomeMarca.Trim() : null;
            registroDB.nomeProduto = registro.nomeProduto != null ? registro.nomeProduto.Trim() : null;
            registroDB.dataconcessao = registro.dataConcessao;
            registroDB.numregistro = registro.numeroRegistro != null ? registro.numeroRegistro.Trim() : null;
            registroDB.modoaplicacao = registro.modoAplicacao != null ? registro.modoAplicacao.Trim() : null;
            registroDB.status = registro.status != null ? registro.status.Trim() : null;
            registroDB.excluido = false;

            db.mapa_registros.Add(registroDB);
            db.SaveChanges();

            
            //mapa empresa

            var idRegistro = registroDB.idregistro;
            
            if (registro.listaEmpresas != null)
            {
                foreach (var idEmpresa in registro.listaEmpresas)
                {

                    var mapaRegistroEmpresa = new ZAdmin_DB.Model.mapa_registro_empresa();

                    mapaRegistroEmpresa.dataregistro = DateTime.Now;
                    mapaRegistroEmpresa.idempresa = idEmpresa;
                    mapaRegistroEmpresa.idregistro = idRegistro;

                    db.mapa_registro_empresa.Add(mapaRegistroEmpresa);
                    db.SaveChanges();

                }
            }

            
        }

        public void Editar(MapaRegistro registro)
        {

            var db = new ZAdmin_DB.Model.zeengEntities();

            var registroDB = (from r in db.mapa_registros
                              where r.idregistro == registro.idRegistro
                              select r).FirstOrDefault();

            registroDB.cnpj = registro.cnpj;
            registroDB.idestado = registro.idEstado;
            registroDB.idarea = registro.idArea;
            registroDB.idespecie = registro.idEspecie;
            registroDB.idsubespecie = registro.idSubEspecie;
            registroDB.idbase = registro.idBase;
            registroDB.idcaracteristica = registro.idCaracteristica;
            registroDB.idatributo = registro.idAtributo;
            registroDB.idcomplemento = registro.idComplemento;
            registroDB.idorigem = registro.idOrigem;
            registroDB.nomeMarca = registro.nomeMarca != null ? registro.nomeMarca.Trim() : null;
            registroDB.nomeProduto = registro.nomeProduto != null ? registro.nomeProduto.Trim() : null;
            registroDB.dataconcessao = registro.dataConcessao;
            registroDB.numregistro = registro.numeroRegistro != null ? registro.numeroRegistro.Trim() : null;
            registroDB.modoaplicacao = registro.modoAplicacao != null ? registro.modoAplicacao.Trim() : null;
            registroDB.status = registro.status != null ? registro.status.Trim() : null;


            var listaIdEmpresaMapaDB = db.mapa_registro_empresa.Where(me => me.idregistro == registro.idRegistro).Select(item => item.idempresa.Value).ToList();

            var resultadoRemoveuEmpresa = registro.listaEmpresas == null ? listaIdEmpresaMapaDB : listaIdEmpresaMapaDB.Except(registro.listaEmpresas);

            var resultadoAdicionouEmpresa = registro.listaEmpresas == null ? listaIdEmpresaMapaDB.Except(listaIdEmpresaMapaDB) : registro.listaEmpresas.Except(listaIdEmpresaMapaDB);


            if (resultadoRemoveuEmpresa.Any())
            {

                var listaOld = resultadoRemoveuEmpresa.ToList();

                var resultado = db.mapa_registro_empresa.Where(mp => listaOld.Contains(mp.idempresa.Value) && mp.idregistro == registro.idRegistro).Select(res => res);

                foreach(var empresaMapa in resultado)
                {
                    db.mapa_registro_empresa.Remove(empresaMapa);
                }

            }

            if (resultadoAdicionouEmpresa.Any())
            {

                var novasEmpresas = resultadoAdicionouEmpresa.ToList();


                foreach(var empresa in novasEmpresas)
                {

                    var mapaEmpresasDB = new ZAdmin_DB.Model.mapa_registro_empresa();

                    mapaEmpresasDB.idempresa = empresa;
                    mapaEmpresasDB.idregistro = registro.idRegistro;
                    mapaEmpresasDB.dataregistro = DateTime.Now;

                    db.mapa_registro_empresa.Add(mapaEmpresasDB);
                    
                }
                
            }
            
           db.SaveChanges();
        }

        public void Deletar(int idRegistro)
        {

            var db = new ZAdmin_DB.Model.zeengEntities();

            var registroDB = (from r in db.mapa_registros
                              where r.idregistro == idRegistro
                              select r).FirstOrDefault();

            if (registroDB != null)
            {
                registroDB.excluido = true;
                db.SaveChanges();
            }

        }

        public List<ItemCombo> RetornaListaDeMarcas()
        {
            var db = new ZAdmin_DB.Model.zeengEntities();

            var listaDeMarcas = (from marca in db.marcas
                                 select new ItemCombo()
                                 {
                                     nome = marca.nome,
                                     idItem = marca.idmarca
                                 }).ToList();

            return listaDeMarcas;
        }

        public List<ItemCombo> RetornListaDeMarcasDeUmaEmpresa(int idEmpresa)
        {
            var db = new ZAdmin_DB.Model.zeengEntities();

            var listaDeMarcas = (from marca in db.marcas
                                 where marca.idempresa == idEmpresa || idEmpresa == 0
                                 select new ItemCombo()
                                 {
                                     nome = marca.nome,
                                     idItem = marca.idmarca
                                 }).ToList();

            return listaDeMarcas;
        }

        public FiltroMapaRegistro RetornaCombosMapaRegistro()
        {
            var db = new ZAdmin_DB.Model.zeengEntities();

            var filtro = new FiltroMapaRegistro();

            filtro.Estados = (from estado in db.estados
                              where estado.excluido == false
                              orderby estado.nome
                              select new ItemCombo()
                              {
                                  idItem = estado.idestado,
                                  nome = estado.nome
                              }).ToList();

            filtro.Areas = (from area in db.mapa_areas
                            where area.excluido == false
                            orderby area.nome ascending
                            select new ItemCombo()
                            {
                                idItem = area.idarea,
                                nome = area.nome
                            }).ToList();

            filtro.Especies = (from especie in db.mapa_especies
                               where especie.excluido == false
                               orderby especie.nome ascending
                               select new ItemCombo()
                               {
                                   idItem = especie.idespecie,
                                   nome = especie.nome
                               }).ToList();

            filtro.Subespecies = (from subespecie in db.mapa_subespecie
                                  where subespecie.excluido == false
                                  orderby subespecie.nome ascending
                                  select new ItemCombo()
                                  {
                                      idItem = subespecie.idsubespecie,
                                      nome = subespecie.nome
                                  }).ToList();

            filtro.Bases = (from @base in db.mapa_base
                            where @base.excluido == false
                            orderby @base.nome ascending
                            select new ItemCombo()
                            {
                                idItem = @base.idbase,
                                nome = @base.nome
                            }).ToList();

            filtro.Caracteristicas = (from caracteristica in db.mapa_caracteristica
                                      where caracteristica.excluido == false
                                      orderby caracteristica.nome ascending
                                      select new ItemCombo()
                                      {
                                          idItem = caracteristica.idcaracteristica,
                                          nome = caracteristica.nome
                                      }).ToList();

            filtro.Atributos = (from atributo in db.mapa_atributo
                                where atributo.excluido == false
                                orderby atributo.nome ascending
                                select new ItemCombo()
                                {
                                    idItem = atributo.idatributo,
                                    nome = atributo.nome
                                }).ToList();

            filtro.Complementos = (from complemento in db.mapa_complemento
                                   where complemento.excluido == false
                                   orderby complemento.nome ascending
                                   select new ItemCombo()
                                   {
                                       idItem = complemento.idcomplemento,
                                       nome = complemento.nome
                                   }).ToList();

            filtro.Origens = (from origem in db.mapa_origens
                              where origem.excluido == false
                              orderby origem.nome ascending
                              select new ItemCombo()
                              {
                                  idItem = origem.idorigem,
                                  nome = origem.nome
                              }).ToList();

            filtro.Marcas = (from marca in db.marcas
                             where marca.excluido == false
                             orderby marca.nome ascending
                             select new ItemCombo()
                             {
                                 idItem = marca.idmarca,
                                 nome = marca.nome
                             }).ToList();

            filtro.Empresas = (from empresa in db.empresas
                               where empresa.excluido == false
                               orderby empresa.nome ascending
                               select new ItemCombo()
                               {
                                   idItem = empresa.idempresa,
                                   nome = empresa.nome
                               }).ToList();

            return filtro;
        }

        public int VerificaDuplicidadeRegistroEmpresa(FiltroDadosMapaRegistro filtro)
        {
            var db = new ZAdmin_DB.Model.zeengEntities();

            var existeDuplicidade = (from r in db.mapa_registros
                                        join e in db.mapa_registro_empresa on r.idregistro equals e.idregistro
                                        where r.numregistro == filtro.numeroRegistro 
                                        && filtro.idEmpresas.Contains(e.idempresa)
                                        select r
                                    ).FirstOrDefault();

            if (existeDuplicidade != null)
                return 1;
            else
                return 0;
        }

    }

    public class FiltroDadosMapaRegistro
    {
        public string numeroRegistro { get; set; }
        public List<int?> idEmpresas { get; set; }

    }

}
