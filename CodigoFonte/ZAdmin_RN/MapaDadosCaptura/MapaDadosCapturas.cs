using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZAdmin_RN.MapaRegistros;

namespace ZAdmin_RN.MapaDadosCaptura
{
    public class MapaDadosCapturas
    {

        ZAdmin_DB.Model.zeengEntities db = new ZAdmin_DB.Model.zeengEntities();

        private int _totalCapturas;

        public int TotalCapturas
        {
            get { return _totalCapturas; }
            set { _totalCapturas = value; }
        }

        public List<MapaDadosCaptura> RetornaListaDeDadosMapaCaptura(FiltroCaptura filtro)
        {

            var listaCnpjs = (from cnpj in db.cnpjempresa
                              where cnpj.idempresa == filtro.idEmpresa
                              select cnpj.cnpj).ToList();

            if (listaCnpjs.Any() || filtro.idEmpresa == 0)
            {

                var capturas = (from c in db.mapa_dadoscaptura
                                where
                                    c.excluido == false &&
                                    (
                                        string.IsNullOrEmpty(filtro.pesquisafiltro) ||
                                        (
                                            c.area.Contains(filtro.pesquisafiltro) ||
                                            c.area.ToLower() == filtro.pesquisafiltro.ToLower() ||

                                            c.atributo.Contains(filtro.pesquisafiltro) ||
                                            c.atributo.ToLower() == filtro.pesquisafiltro.ToLower() ||

                                            c.@base.Contains(filtro.pesquisafiltro) ||
                                            c.@base.ToLower() == filtro.pesquisafiltro.ToLower() ||

                                            c.caracteristica.Contains(filtro.pesquisafiltro) ||
                                            c.caracteristica.ToLower() == filtro.pesquisafiltro.ToLower() ||

                                            c.cnpj.Contains(filtro.pesquisafiltro) ||
                                            c.cnpj.ToLower() == filtro.pesquisafiltro.ToLower() ||

                                            c.complemento.Contains(filtro.pesquisafiltro) ||
                                            c.complemento.ToLower() == filtro.pesquisafiltro.ToLower() ||

                                            c.especie.Contains(filtro.pesquisafiltro) ||
                                            c.especie.ToLower() == filtro.pesquisafiltro.ToLower() ||

                                            c.estabelecimento.Contains(filtro.pesquisafiltro) ||
                                            c.estabelecimento.ToLower() == filtro.pesquisafiltro.ToLower() ||

                                            c.marca.Contains(filtro.pesquisafiltro) ||
                                            c.marca.ToLower() == filtro.pesquisafiltro.ToLower() ||

                                            c.modoaplicacao.Contains(filtro.pesquisafiltro) ||
                                            c.modoaplicacao.ToLower() == filtro.pesquisafiltro.ToLower() ||

                                            c.origem.Contains(filtro.pesquisafiltro) ||
                                            c.origem.ToLower() == filtro.pesquisafiltro.ToLower() ||

                                            c.produto.Contains(filtro.pesquisafiltro) ||
                                            c.produto.ToLower() == filtro.pesquisafiltro.ToLower() ||

                                            c.registro.Contains(filtro.pesquisafiltro) ||
                                            c.registro.ToLower() == filtro.pesquisafiltro.ToLower() ||

                                            c.subespecie.Contains(filtro.pesquisafiltro) ||
                                            c.subespecie.ToLower() == filtro.pesquisafiltro.ToLower()

                                            )
                                   )
                                    && (filtro.status != "T" ? c.status == filtro.status: true)
                                    && (filtro.captura != null && filtro.captura != 0 ? c.idmapacaptura == filtro.captura : c.idmapacaptura > 0)

                                    && (listaCnpjs.Any(r => c.cnpj.Contains(r)) || listaCnpjs.Count() == 0)

                                    && ((filtro.dataInicial == null || c.dtdataconcessao >= filtro.dataInicial) &&
                                       (filtro.dataFinal == null || c.dtdataconcessao <= filtro.dataFinal))

                                select c).ToList();

        


                if (filtro.vinculo == "V")
                {
                    capturas = (
                            from c in capturas
                            join r in db.mapa_registros on c.registro equals r.numregistro
                            select c
                        ).ToList();

                }
                else if (filtro.vinculo == "NV")
                {
                    capturas = (
                            from c in capturas
                            join r in db.mapa_registros on c.registro equals r.numregistro
                            where r.numregistro != c.registro
                            select c
                        ).ToList();
                }

                _totalCapturas = capturas.Select(s => s.iddadoscaptura).Count();
                var pagcapturas = capturas.OrderBy(x => x.produto).Skip((filtro.pagina - 1) * filtro.qtdRegistros).Take(filtro.qtdRegistros).ToList();
                //_totalCapturas = pagcapturas.Count();


                var listaCapturas = (from c in pagcapturas
                                     select new MapaDadosCaptura()
                                     {
                                         idDadosCaputura = c.iddadoscaptura,
                                         idmaoacaptura = c.idmapacaptura,
                                         dataHoraCaptura = c.datahoracaptura,
                                         uf = c.uf,
                                         area = c.area,
                                         especie = c.especie,
                                         subEspecie = c.subespecie,
                                         @base = c.@base,
                                         caracteristica = c.caracteristica,
                                         atributo = c.atributo,
                                         complemento = c.complemento,
                                         estabelecimento = c.estabelecimento,
                                         cnpj = c.cnpj,
                                         produto = c.produto,
                                         dataConcessao = DateTime.ParseExact(c.dataconcessao, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                                         registro = c.registro,
                                         marca = c.marca,
                                         origem = c.origem,
                                         modoAplicacao = c.modoaplicacao,
                                         status = c.status

                                     }).ToList();


                var max = pagcapturas.Max(x => x.idmapacaptura);
                int idcapturaanterior = Convert.ToInt32((pagcapturas.Where(c => c.idmapacaptura < max).OrderByDescending(c => c.idmapacaptura).FirstOrDefault()));

                var listaCapturasAnterior = (from c in pagcapturas
                                             where c.idmapacaptura == idcapturaanterior

                                             select new MapaDadosCapturaAnterior()
                                             {
                                                 idDadosCaputura = c.iddadoscaptura,
                                                 idmaoacaptura = c.idmapacaptura,
                                                 dataHoraCaptura = c.datahoracaptura,
                                                 uf = c.uf,
                                                 area = c.area,
                                                 especie = c.especie,
                                                 subEspecie = c.subespecie,
                                                 @base = c.@base,
                                                 caracteristica = c.caracteristica,
                                                 atributo = c.atributo,
                                                 complemento = c.complemento,
                                                 estabelecimento = c.estabelecimento,
                                                 cnpj = c.cnpj,
                                                 produto = c.produto,
                                                 dataConcessao = DateTime.ParseExact(c.dataconcessao, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                                                 registro = c.registro,
                                                 marca = c.marca,
                                                 origem = c.origem,
                                                 modoAplicacao = c.modoaplicacao,
                                                 status = c.status
                                             }).ToList();


                if (listaCapturasAnterior.Count() != 0)
                {
                    for (int i = 0; i < listaCapturas.Count(); i++)
                    {

                        if (listaCapturas[i].area != listaCapturasAnterior[i].area ||
                            listaCapturas[i].uf != listaCapturasAnterior[i].uf ||
                            listaCapturas[i].subEspecie != listaCapturasAnterior[i].subEspecie ||
                            listaCapturas[i].@base != listaCapturasAnterior[i].@base ||
                            listaCapturas[i].caracteristica != listaCapturasAnterior[i].caracteristica ||
                            listaCapturas[i].atributo != listaCapturasAnterior[i].atributo ||
                            listaCapturas[i].complemento != listaCapturasAnterior[i].complemento ||
                            listaCapturas[i].estabelecimento != listaCapturasAnterior[i].estabelecimento ||
                            listaCapturas[i].cnpj != listaCapturasAnterior[i].cnpj ||
                            listaCapturas[i].produto != listaCapturasAnterior[i].produto ||
                            listaCapturas[i].dataConcessao != listaCapturasAnterior[i].dataConcessao ||
                            listaCapturas[i].marca != listaCapturasAnterior[i].marca ||
                            listaCapturas[i].origem != listaCapturasAnterior[i].origem ||
                            listaCapturas[i].modoAplicacao != listaCapturasAnterior[i].modoAplicacao ||
                            listaCapturas[i].status != listaCapturasAnterior[i].status ||
                            listaCapturas[i].registro != listaCapturasAnterior[i].registro)
                        {
                            listaCapturas[i].StatusMapa = true;
                        }
                        else
                        {

                            listaCapturas[i].StatusMapa = false;
                        }

                    }
                }


                for (int i = 0; i < listaCapturas.Count(); i++)
                {

                    listaCapturas[i].ValidarRegistro = (from r in db.mapa_registros.ToList()
                                                        where r.numregistro == listaCapturas[i].registro
                                                        select r.numregistro.Count() != 0 ? true : false).FirstOrDefault();
                }


                return listaCapturas;


            }
            else
            {
                _totalCapturas = 0;
                return null;
            }

        }

        public MapaDadosCaptura RetornaCaptura(int idCaptura)
        {

            var capturaDB = (from captura in db.mapa_dadoscaptura
                             where captura.excluido == false &&
                                    captura.iddadoscaptura == idCaptura
                             select new MapaDadosCaptura()
                             {
                                 idDadosCaputura = captura.iddadoscaptura,
                                 uf = captura.uf,
                                 dataHoraCaptura = captura.datahoracaptura,
                                 area = captura.area,
                                 especie = captura.especie,
                                 subEspecie = captura.subespecie,
                                 @base = captura.@base,
                                 caracteristica = captura.caracteristica,
                                 atributo = captura.atributo,
                                 complemento = captura.complemento,
                                 estabelecimento = captura.estabelecimento,
                                 cnpj = captura.cnpj,
                                 produto = captura.produto,
                                 dataConcessao = captura.dtdataconcessao,
                                 registro = captura.registro,
                                 marca = captura.marca,
                                 origem = captura.origem,
                                 modoAplicacao = captura.modoaplicacao,
                                 status = captura.status,

                                 //listaDeStatus = (captura.mapa_statusdadoscaptura.Select(
                                 //                 x => new StatusDadosCaptura()
                                 //                 {
                                 //                     data = x.data,
                                 //                     idMapaDadosCaputura = x.iddadoscaptura,
                                 //                     idStatusCaptura = x.iddadoscaptura,
                                 //                     idUsuario = x.idusuario,
                                 //                     status = x.status
                                 //                 }))

                             }).FirstOrDefault();


            return capturaDB;
        }

        public void SalvaCapturaParaRegitros(MapaDadosCapturaImportado mapacaptura)
        {

            //MARCA
            if (mapacaptura.idMarca == null)
            {
                var marca = (from m in db.marcas
                             where m.nome.ToLower() == mapacaptura.nomemarca.ToLower()
                             select m).FirstOrDefault();
                if (marca != null)
                {
                    mapacaptura.idMarca = marca.idmarca;
                }
            }


            //ESTADO
            if (mapacaptura.idEstado == null)
            {
                var estado = (from e in db.estados
                              where e.uf.ToLower() == mapacaptura.uf.ToLower()
                              select e).FirstOrDefault();

                mapacaptura.idEstado = estado.idestado;
            }

            //AREA
            if (mapacaptura.idArea == null)
            {
                var area = (from m in db.mapa_areas
                            where m.nome.ToLower() == mapacaptura.nomearea.ToLower()
                            select m).FirstOrDefault();

                if (area == null)
                {
                    var a = new ZAdmin_DB.Model.mapa_areas()
                    {
                        nome = mapacaptura.nomearea,
                        excluido = false
                    };
                    db.mapa_areas.Add(a);
                    db.SaveChanges();
                    mapacaptura.idArea = a.idarea;
                }
                else
                {
                    mapacaptura.idArea = area.idarea;
                }
            }

            //ESPECIE
            if (mapacaptura.idEspecie == null)
            {
                var especie = (from e in db.mapa_especies
                               where e.nome.ToLower() == mapacaptura.nomeEspecie.ToLower()
                               select e).FirstOrDefault();

                if (especie == null)
                {
                    var especieDB = new ZAdmin_DB.Model.mapa_especies()
                    {
                        nome = mapacaptura.nomeEspecie,
                        excluido = false
                    };
                    db.mapa_especies.Add(especieDB);
                    db.SaveChanges();
                    mapacaptura.idEspecie = especieDB.idespecie;
                }
                else
                {
                    mapacaptura.idEspecie = especie.idespecie;
                }

            }

            //SUBESPECIE
            if (mapacaptura.idSubEspecie == null)
            {
                var subespecie = (from s in db.mapa_subespecie
                                  where s.nome.ToLower() == mapacaptura.nomeSubespecie.ToLower()
                                  select s).FirstOrDefault();

                if (subespecie == null)
                {
                    var subespecieDB = new ZAdmin_DB.Model.mapa_subespecie()
                    {
                        nome = mapacaptura.nomeSubespecie,
                        excluido = false
                    };
                    db.mapa_subespecie.Add(subespecieDB);
                    db.SaveChanges();
                    mapacaptura.idSubEspecie = subespecieDB.idsubespecie;
                }
                else
                {
                    mapacaptura.idSubEspecie = subespecie.idsubespecie;
                }
            }

            //BASE 
            if (mapacaptura.idBase == null)
            {
                var basecaptura = (from b in db.mapa_base
                                   where b.nome.ToLower() == mapacaptura.nomebase.ToLower()
                                   select b).FirstOrDefault();

                if (basecaptura == null)
                {
                    var baseDB = new ZAdmin_DB.Model.mapa_base()
                    {
                        nome = mapacaptura.nomebase,
                        excluido = false
                    };
                    db.mapa_base.Add(baseDB);
                    db.SaveChanges();
                    mapacaptura.idBase = baseDB.idbase;
                }
                else
                {
                    mapacaptura.idBase = basecaptura.idbase;
                }
            }

            //CARACTERISTICA
            if (mapacaptura.idCaracteristica == null)
            {
                var caracteristica = (from c in db.mapa_caracteristica
                                      where c.nome.ToLower() == mapacaptura.nomecaracteristica.ToLower()
                                      select c).FirstOrDefault();

                if (caracteristica == null)
                {
                    var caracteristicaDB = new ZAdmin_DB.Model.mapa_caracteristica()
                    {
                        nome = mapacaptura.nomecaracteristica,
                        excluido = false
                    };
                    db.mapa_caracteristica.Add(caracteristicaDB);
                    db.SaveChanges();
                    mapacaptura.idCaracteristica = caracteristicaDB.idcaracteristica;

                }
                else
                {
                    mapacaptura.idCaracteristica = caracteristica.idcaracteristica;
                }
            }

            //ATRIBUTO
            if (mapacaptura.idAtributo == null)
            {
                var atributo = (from a in db.mapa_atributo
                                where a.nome.ToLower() == mapacaptura.nomeatributo.ToLower()
                                select a).FirstOrDefault();

                if (atributo == null)
                {
                    var atributoDB = new ZAdmin_DB.Model.mapa_atributo()
                    {
                        nome = mapacaptura.nomeatributo,
                        excluido = false
                    };
                    db.mapa_atributo.Add(atributoDB);
                    db.SaveChanges();
                    mapacaptura.idAtributo = atributoDB.idatributo;

                }
                else
                {
                    mapacaptura.idAtributo = atributo.idatributo;
                }
            }

            //COMPLEMENTO
            if (mapacaptura.idComplemento == null)
            {
                var complemento = (from c in db.mapa_complemento
                                   where c.nome.ToLower() == mapacaptura.nomecomplemento.ToLower()
                                   select c).FirstOrDefault();

                if (complemento == null)
                {
                    var complementoDB = new ZAdmin_DB.Model.mapa_complemento()
                    {
                        nome = mapacaptura.nomecomplemento,
                        excluido = false
                    };

                    db.mapa_complemento.Add(complementoDB);
                    db.SaveChanges();
                    mapacaptura.idComplemento = complementoDB.idcomplemento;

                }
                else
                {
                    mapacaptura.idComplemento = complemento.idcomplemento;
                }
            }

            //ORIGEM
            if (mapacaptura.idOrigem == null)
            {
                var origem = (from o in db.mapa_origens
                              where o.nome.ToLower() == mapacaptura.nomeorigem.ToLower()
                              select o).FirstOrDefault();

                if (origem == null)
                {
                    var origemDB = new ZAdmin_DB.Model.mapa_origens()
                    {
                        nome = mapacaptura.nomeorigem,
                        excluido = false
                    };

                    db.mapa_origens.Add(origemDB);
                    db.SaveChanges();
                    mapacaptura.idOrigem = origemDB.idorigem;

                }
                else
                {
                    mapacaptura.idOrigem = origem.idorigem;
                }
            }

            //MAPAREGISTRO
            var mapaReg = new ZAdmin_DB.Model.mapa_registros()
            {
                cnpj = mapacaptura.cnpj.Trim(),
                idestado = mapacaptura.idEstado,
                idarea = mapacaptura.idArea,
                idespecie = mapacaptura.idEspecie,
                idsubespecie = mapacaptura.idSubEspecie,
                idbase = mapacaptura.idBase,
                idcaracteristica = mapacaptura.idCaracteristica,
                idatributo = mapacaptura.idAtributo,
                idcomplemento = mapacaptura.idComplemento,
                idorigem = mapacaptura.idOrigem,
                nomeMarca = (mapacaptura.nomemarca != null ? mapacaptura.nomemarca.Trim() : null),
                nomeProduto = (mapacaptura.nomeProduto != null ? mapacaptura.nomeProduto.Trim() : null),
                dataconcessao = mapacaptura.dataConcessao,
                numregistro = (mapacaptura.numeroRegistro != null ? mapacaptura.numeroRegistro.Trim() : null),
                modoaplicacao = (mapacaptura.modoAplicacao != null ? mapacaptura.modoAplicacao.Trim() : null),
                status = (mapacaptura.status != null ? mapacaptura.status.Trim() : null),
                excluido = false
            };

            db.mapa_registros.Add(mapaReg);
            db.SaveChanges();


            //empresas
            var idRegistro = mapaReg.idregistro;

            if (mapacaptura.listaEmpresas != null && mapacaptura.listaEmpresas.Count > 0)
            {
                foreach (var idEmpresa in mapacaptura.listaEmpresas)
                {
                    var mapaEmpresaDB = new ZAdmin_DB.Model.mapa_registro_empresa();

                    mapaEmpresaDB.dataregistro = DateTime.Now;
                    mapaEmpresaDB.idempresa = idEmpresa;
                    mapaEmpresaDB.idregistro = idRegistro;

                    db.mapa_registro_empresa.Add(mapaEmpresaDB);

                    db.SaveChanges();
                }
            }

            //cnpjempresa
            if (mapacaptura.listaEmpresas != null && mapacaptura.listaEmpresas.Count > 0)
            {
                foreach (var idEmpresa in mapacaptura.listaEmpresas)
                {
                    var achouEmpresaCNPJ = db.cnpjempresa.Where(x => x.idempresa == idEmpresa && x.cnpj.Trim() == mapacaptura.cnpj.Trim()).FirstOrDefault();
                    if (achouEmpresaCNPJ == null)
                    {
                        var cnpjEmpresa = new ZAdmin_DB.Model.cnpjempresa();
                        cnpjEmpresa.cnpj = mapacaptura.cnpj.Trim();
                        cnpjEmpresa.idempresa = idEmpresa;

                        db.cnpjempresa.Add(cnpjEmpresa);
                        db.SaveChanges();
                    }
                }
            }
        }

        public MapaDadosCapturaImportado RetornaCapturaParaRegitros(int idDadosCaptura)
        {

            MapaDadosCapturaImportado mapacaptura = (from m in db.mapa_dadoscaptura.ToList()
                                                     where m.iddadoscaptura == idDadosCaptura
                                                     select new MapaDadosCapturaImportado()
                                                     {
                                                         idDadosCaputura = idDadosCaptura,
                                                         cnpj = m.cnpj,
                                                         //dataConcessao = Convert.ToDateTime(m.dtdataconcessao),
                                                         dataConcessao = Convert.ToDateTime(m.dataconcessao),
                                                         modoAplicacao = m.modoaplicacao,
                                                         nomearea = m.area,
                                                         nomeatributo = m.atributo,
                                                         nomebase = m.@base,
                                                         nomecaracteristica = m.caracteristica,
                                                         nomecomplemento = m.complemento,
                                                         nomeEspecie = m.especie,
                                                         nomemarca = m.marca,
                                                         nomeorigem = m.origem,
                                                         nomeProduto = m.produto,
                                                         nomeSubespecie = m.subespecie,
                                                         numeroRegistro = m.registro,
                                                         status = m.status,
                                                         uf = m.uf

                                                     }).SingleOrDefault();

            if (mapacaptura != null)
            {

                var idEmpresa = (from c in db.cnpjempresa
                                 where c.cnpj == mapacaptura.cnpj
                                 select c.idempresa).FirstOrDefault();

                if (idEmpresa != null)
                {
                    mapacaptura.idEmpresa = idEmpresa;
                }

                //MARCA
                var marca = (from m in db.marcas
                             where m.nome.ToLower() == mapacaptura.nomemarca.ToLower()
                             select m).FirstOrDefault();

                if (marca != null)
                {
                    mapacaptura.idMarca = marca.idmarca;
                    mapacaptura.nomemarca = marca.nome;
                }

                //ESTADO
                var estado = (from e in db.estados
                              where e.uf.ToLower() == mapacaptura.uf.ToLower()
                              select e).FirstOrDefault();

                if (estado != null)
                {
                    mapacaptura.idEstado = estado.idestado;
                }

                //AREA
                if (mapacaptura.idArea == null)
                {
                    var area = (from m in db.mapa_areas
                                where m.nome.ToLower() == mapacaptura.nomearea.ToLower()
                                select m).FirstOrDefault();

                    if (area != null)
                    {
                        mapacaptura.idArea = area.idarea;
                        mapacaptura.nomearea = area.nome;
                    }
                }

                //ESPECIE
                if (mapacaptura.idEspecie == null)
                {
                    var especie = (from e in db.mapa_especies
                                   where e.nome.ToLower() == mapacaptura.nomeEspecie.ToLower()
                                   select e).FirstOrDefault();

                    if (especie != null)
                    {
                        mapacaptura.idEspecie = especie.idespecie;
                        mapacaptura.nomeEspecie = especie.nome;
                    }

                }

                //SUBESPECIE
                if (mapacaptura.idSubEspecie == null)
                {
                    var subespecie = (from s in db.mapa_subespecie
                                      where s.nome.ToLower() == mapacaptura.nomeSubespecie.ToLower()
                                      select s).FirstOrDefault();

                    if (subespecie != null)
                    {
                        mapacaptura.idSubEspecie = subespecie.idsubespecie;
                    }
                }

                //BASE 
                if (mapacaptura.idBase == null)
                {
                    var basecaptura = (from b in db.mapa_base
                                       where b.nome.ToLower() == mapacaptura.nomebase.ToLower()
                                       select b).FirstOrDefault();

                    if (basecaptura != null)
                    {
                        mapacaptura.idBase = basecaptura.idbase;
                    }

                }

                //CARACTERISTICA
                if (mapacaptura.idCaracteristica == null)
                {
                    var caracteristica = (from c in db.mapa_caracteristica
                                          where c.nome.ToLower() == mapacaptura.nomecaracteristica.ToLower()
                                          select c).FirstOrDefault();

                    if (caracteristica != null)
                    {
                        mapacaptura.idCaracteristica = caracteristica.idcaracteristica;
                    }
                }


                //ATRIBUTO
                if (mapacaptura.idAtributo == null)
                {
                    var atributo = (from a in db.mapa_atributo
                                    where a.nome.ToLower() == mapacaptura.nomeatributo.ToLower()
                                    select a).FirstOrDefault();

                    if (atributo != null)
                    {
                        mapacaptura.idAtributo = atributo.idatributo;
                    }
                }

                //COMPLEMENTO
                if (mapacaptura.idComplemento == null)
                {
                    var complemento = (from c in db.mapa_complemento
                                       where c.nome.ToLower() == mapacaptura.nomecomplemento.ToLower()
                                       select c).FirstOrDefault();

                    if (complemento != null)
                    {
                        mapacaptura.idComplemento = complemento.idcomplemento;
                    }
                }


                //ORIGEM
                if (mapacaptura.idOrigem == null)
                {
                    var origem = (from o in db.mapa_origens
                                  where o.nome.ToLower() == mapacaptura.nomeorigem.ToLower()
                                  select o).FirstOrDefault();

                    if (origem != null)
                    {
                        mapacaptura.idOrigem = origem.idorigem;
                    }

                }
                return mapacaptura;
            }
            else
            {
                return null;
            }

        }

        public void Editar(MapaDadosCaptura captura)
        {
            var db = new ZAdmin_DB.Model.zeengEntities();

            var capturaDB = (from c in db.mapa_dadoscaptura
                             where c.iddadoscaptura == captura.idDadosCaputura
                             select c).FirstOrDefault();

            capturaDB.uf = captura.uf != null ? captura.uf.Trim() : null;
            capturaDB.area = captura.area != null ? captura.area.Trim() : null;
            capturaDB.especie = captura.especie != null ? captura.especie.Trim() : null;
            capturaDB.subespecie = captura.subEspecie != null ? captura.subEspecie.Trim() : null;
            capturaDB.@base = captura.@base != null ? captura.@base.Trim() : null;
            capturaDB.caracteristica = captura.caracteristica != null ? captura.caracteristica.Trim() : null;
            capturaDB.atributo = captura.atributo != null ? captura.atributo.Trim() : null;
            capturaDB.complemento = captura.complemento != null ? captura.complemento.Trim() : null;
            capturaDB.cnpj = captura.cnpj != null ? captura.cnpj.Trim() : null;
            capturaDB.produto = captura.produto != null ? captura.produto.Trim() : null;
            capturaDB.dtdataconcessao = captura.dataConcessao;
            capturaDB.registro = captura.registro != null ? captura.registro.Trim() : null;
            capturaDB.marca = captura.marca != null ? captura.marca.Trim() : null;
            capturaDB.origem = captura.origem != null ? captura.origem.Trim() : null;
            capturaDB.modoaplicacao = captura.modoAplicacao != null ? captura.modoAplicacao.Trim() : null;
            capturaDB.status = captura.status != null ? captura.status.Trim() : null;

            db.SaveChanges();

            //cnpjempresa
            if (captura.listaEmpresas != null && captura.listaEmpresas.Count > 0)
            {
                foreach (var idEmpresa in captura.listaEmpresas)
                {
                    var achouEmpresaCNPJ = db.cnpjempresa.Where(x => x.idempresa == idEmpresa && x.cnpj.Trim() == captura.cnpj.Trim()).FirstOrDefault();
                    if (achouEmpresaCNPJ == null)
                    {
                        var cnpjEmpresa = new ZAdmin_DB.Model.cnpjempresa();
                        cnpjEmpresa.cnpj = captura.cnpj.Trim();
                        cnpjEmpresa.idempresa = idEmpresa;

                        db.cnpjempresa.Add(cnpjEmpresa);
                        db.SaveChanges();
                    }
                }
            }
        }

        public void Deletar(int idCaptura)
        {

            var capturaDB = (from captura in db.mapa_dadoscaptura
                             where captura.iddadoscaptura == idCaptura
                             select captura).FirstOrDefault();

            if (capturaDB != null)
            {
                capturaDB.excluido = true;
                db.SaveChanges();
            }

        }

        public List<ItemCombo> RetornaListaDeEstados()
        {

            var estados = (from estado in db.estados
                           select new ItemCombo()
                           {
                               idItem = estado.idestado,
                               nome = estado.uf
                           }).ToList();

            return estados;

        }

        public List<ItemCombo> RetornaListaDeEmpresas()
        {
            var empresas = (from empresa in db.empresas
                            orderby empresa.nome ascending
                            select new ItemCombo()
                            {
                                idItem = empresa.idempresa,
                                nome = empresa.nome
                            }).ToList();

            return empresas;

        }

        public List<ItemCombo> RetornaListaDeMarcas(int idEmpresa)
        {
            var marcas = (from marca in db.marcas
                          where marca.idempresa == idEmpresa || idEmpresa == 0

                          select new ItemCombo()
                          {
                              idItem = marca.idmarca,
                              nome = marca.nome
                          }).ToList();

            return marcas;
        }

        public List<ItemCombo> RetornaListaMarcasByEmpresas(List<int> Ids)
        {
            var marcas = new List<ItemCombo>();
            if (Ids != null)
                marcas = db.marcas.Where(x => Ids.Contains(x.idempresa.Value) && x.excluido == false).Select(c => new ItemCombo() { idItem = c.idmarca, nome = c.nome }).ToList();
            else
                marcas = db.marcas.Where(x => x.excluido == false).Select(c => new ItemCombo() { idItem = c.idmarca, nome = c.nome }).ToList();

            return marcas;
        }

        public FiltroMapaRegistro RetornaCombos()
        {

            var filtro = new FiltroMapaRegistro();

            filtro.Estados = (from estado in db.estados
                              where estado.excluido == false
                              select new ItemCombo()
                              {
                                  idItem = estado.idestado,
                                  nome = estado.uf
                              }).ToList();

            filtro.Areas = (from area in db.mapa_areas
                            where area.excluido == false
                            select new ItemCombo()
                            {
                                idItem = area.idarea,
                                nome = area.nome
                            }).ToList();

            filtro.Especies = (from especie in db.mapa_especies
                               where especie.excluido == false
                               select new ItemCombo()
                               {
                                   idItem = especie.idespecie,
                                   nome = especie.nome
                               }).ToList();

            filtro.Subespecies = (from subespecie in db.mapa_subespecie
                                  where subespecie.excluido == false
                                  select new ItemCombo()
                                  {
                                      idItem = subespecie.idsubespecie,
                                      nome = subespecie.nome
                                  }).ToList();

            filtro.Bases = (from @base in db.mapa_base
                            where @base.excluido == false
                            select new ItemCombo()
                            {
                                idItem = @base.idbase,
                                nome = @base.nome
                            }).ToList();

            filtro.Caracteristicas = (from caracteristica in db.mapa_caracteristica
                                      where caracteristica.excluido == false
                                      select new ItemCombo()
                                      {
                                          idItem = caracteristica.idcaracteristica,
                                          nome = caracteristica.nome
                                      }).ToList();

            filtro.Atributos = (from atributo in db.mapa_atributo
                                where atributo.excluido == false
                                select new ItemCombo()
                                {
                                    idItem = atributo.idatributo,
                                    nome = atributo.nome
                                }).ToList();

            filtro.Complementos = (from complemento in db.mapa_complemento
                                   where complemento.excluido == false
                                   select new ItemCombo()
                                   {
                                       idItem = complemento.idcomplemento,
                                       nome = complemento.nome
                                   }).ToList();

            filtro.Origens = (from origem in db.mapa_origens
                              where origem.excluido == false
                              select new ItemCombo()
                              {
                                  idItem = origem.idorigem,
                                  nome = origem.nome
                              }).ToList();

            filtro.Marcas = (from marca in db.marcas
                             where marca.excluido == false
                             select new ItemCombo()
                             {
                                 idItem = marca.idmarca,
                                 nome = marca.nome
                             }).ToList();

            filtro.Empresas = (from empresa in db.empresas
                               where empresa.excluido == false
                               select new ItemCombo()
                               {
                                   idItem = empresa.idempresa,
                                   nome = empresa.nome
                               }).ToList();

            return filtro;
        }

        public int RetornaAssociacaoCnpjEmpresaMapa(MapaRegistro filtro)
        {
            var existe = db.cnpjempresa.Where(x => x.cnpj.Trim() == filtro.cnpj.Trim() && filtro.listaEmpresas.Contains(x.idempresa)).ToList();
            if (existe.Count() < filtro.listaEmpresas.Count())
                return 0;

            return 1;
        }
    }
}
