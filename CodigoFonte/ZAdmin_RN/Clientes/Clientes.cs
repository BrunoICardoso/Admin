using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAdmin_RN.Clientes
{
    public class Clientes
    {

        private int _totalClientes;

        public int TotalClientes
        {
            get { return _totalClientes; }
            set { _totalClientes = value; }
        }

        public List<Cliente> RetornaListaDeClientes(int pagina, int qtdPorPagina)
        {

            var db = new ZAdmin_DB.Model.zeengEntities();

            _totalClientes = (from c in db.clientes
                              where c.excluido == false
                              select c).Count();

            var clientes = (from c in db.clientes
                            where c.excluido == false
                            select c).OrderBy(x => x.nome).Skip((pagina - 1) * qtdPorPagina).Take(qtdPorPagina);

            var listaDeClientes = (from c in clientes
                                   select new Cliente()
                                   {
                                       idcliente = c.idclientes,
                                       nome = c.nome,
                                       celular = c.celular,
                                       cidade = c.cidade,
                                       endereco = c.endereco,
                                       nomecontato = c.nomecontato,
                                       site = c.site,
                                       telefone = c.telefone,
                                       idestado = c.idestado,
                                       cnpj = c.cnpj,
                                       email = c.email,
                                       cep = c.cep,
                                       inscricaoEstadual = c.inscestadual,
                                       inscricaoMunicipal = c.inscmunicial,
                                       nomeEstado = c.estados.nome
                                   }).ToList();

            return listaDeClientes;
        }

        public Cliente RetornaCliente(int id)
        {
            var db = new ZAdmin_DB.Model.zeengEntities();

            var clienteDB = (from cliente in db.clientes
                             where (cliente.excluido == false) && (cliente.idclientes == id)
                             select new Cliente()
                             {
                                 idcliente = cliente.idclientes,
                                 nome = cliente.nome,
                                 celular = cliente.celular,
                                 cidade = cliente.cidade,
                                 endereco = cliente.endereco,
                                 idestado = cliente.idestado,
                                 nomecontato = cliente.nomecontato,
                                 site = cliente.site,
                                 telefone = cliente.telefone,
                                 cnpj = cliente.cnpj,
                                 email = cliente.email,
                                 cep = cliente.cep,
                                 inscricaoEstadual = cliente.inscestadual,
                                 inscricaoMunicipal = cliente.inscmunicial,
                                 quantidadeEmpresas = cliente.qtdempresas == null ? 0 : (int)cliente.qtdempresas,
                                 vertodasempresas = cliente.vertodasempresas,
                                 dataExpiracao = cliente.dtexpiracaolicenca
                             }).FirstOrDefault();


            var clienteVertentes = db.cliente_vertentes.Where(x => x.idcliente == clienteDB.idcliente).Select(x => x).FirstOrDefault();

            if (clienteVertentes != null)
            {

                var listaVert = new List<string>();

                if (clienteVertentes.redessociais == true)
                {
                    //clienteDB.vertentes.Add("redessocias");
                    listaVert.Add("redessocias");
                }
                if (clienteVertentes.produtos == true)
                {
                    //clienteDB.vertentes.Add("produtos");
                    listaVert.Add("produtos");
                }
                if (clienteVertentes.promocoes == true)
                {
                    //clienteDB.vertentes.Add("promocoes");
                    listaVert.Add("promocoes");
                }
                if (clienteVertentes.presencaonline == true)
                {
                    //clienteDB.vertentes.Add("presencaonline");
                    listaVert.Add("presencaonline");
                }
                if (clienteVertentes.noticias == true)
                {
                    //clienteDB.vertentes.Add("noticias");
                    listaVert.Add("noticias");
                }

                clienteDB.vertentes = new List<string>();
                clienteDB.vertentes.AddRange(listaVert);

            }


            return clienteDB;
        }

        public void Cadastrar(Cliente cliente)
        {

            if (cliente.vertentes.Any())
            {

                var db = new ZAdmin_DB.Model.zeengEntities();

                var clienteDB = new ZAdmin_DB.Model.clientes();

                clienteDB.nome = cliente.nome;
                clienteDB.site = cliente.site;
                clienteDB.telefone = cliente.telefone != null ? cliente.telefone.Trim() : null;
                clienteDB.nomecontato = cliente.nomecontato != null ? cliente.nomecontato.Trim() : null;
                clienteDB.endereco = cliente.endereco != null ? cliente.endereco.Trim() : null;
                clienteDB.celular = cliente.celular != null ? cliente.celular.Trim() : null;
                clienteDB.cidade = cliente.cidade != null ? cliente.cidade.Trim() : null;
                clienteDB.idestado = cliente.idestado;
                clienteDB.email = cliente.email != null ? cliente.email.Trim() : null;
                clienteDB.cnpj = cliente.cnpj != null ? cliente.cnpj.Trim() : null;
                clienteDB.cep = cliente.cep != null ? cliente.cep.Trim() : null;
                clienteDB.inscmunicial = cliente.inscricaoMunicipal != null ? cliente.inscricaoMunicipal.Trim() : null;
                clienteDB.inscestadual = cliente.inscricaoEstadual != null ? cliente.inscricaoEstadual.Trim() : null;
                clienteDB.excluido = false;
                clienteDB.qtdempresas = cliente.quantidadeEmpresas;
                clienteDB.vertodasempresas = cliente.quantidadeEmpresas <= 0 ? true : false;
                clienteDB.dtexpiracaolicenca = cliente.dataExpiracao;
                db.clientes.Add(clienteDB);

                db.SaveChanges();


                var clienteVertentes = new ZAdmin_DB.Model.cliente_vertentes();
                clienteVertentes.idcliente = clienteDB.idclientes;
                clienteVertentes.produtos = cliente.vertentes.Contains("produtos") ? true : false;
                clienteVertentes.presencaonline = cliente.vertentes.Contains("presencaonline") ? true : false;
                clienteVertentes.promocoes = cliente.vertentes.Contains("promocoes") ? true : false;
                clienteVertentes.noticias = cliente.vertentes.Contains("noticias") ? true : false;
                clienteVertentes.redessociais = cliente.vertentes.Contains("redessocias") ? true : false;

                db.cliente_vertentes.Add(clienteVertentes);

                db.SaveChanges();

                if (cliente.quantidadeEmpresas > 0)
                {
                    cliente.vertodasempresas = false;
                    foreach (var empresa in cliente.listaEmpresas)
                    {
                        var clienteEmpresa = new ZAdmin_DB.Model.cliente_empresas();
                        clienteEmpresa.idempresa = empresa;
                        clienteEmpresa.idcliente = clienteDB.idclientes;
                        clienteEmpresa.dtcadastro = DateTime.Now;
                        clienteEmpresa.excluido = false;
                        db.cliente_empresas.Add(clienteEmpresa);
                        db.SaveChanges();
                    }
                }

            }

        }

        public void Editar(Cliente cliente)
        {
            if (cliente.vertentes.Any())
            {
                var db = new ZAdmin_DB.Model.zeengEntities();

                var clienteDB = (from c in db.clientes
                                 where c.idclientes == cliente.idcliente
                                 select c).FirstOrDefault();

                clienteDB.nome = cliente.nome != null ? cliente.nome.Trim() : null;
                clienteDB.idestado = cliente.idestado;
                clienteDB.nomecontato = cliente.nomecontato != null ? cliente.nomecontato.Trim() : null;
                clienteDB.site = cliente.site != null ? cliente.site.Trim() : null;
                clienteDB.telefone = cliente.telefone != null ? cliente.telefone.Trim() : null;
                clienteDB.cidade = cliente.cidade != null ? cliente.cidade.Trim() : null;
                clienteDB.celular = cliente.celular != null ? cliente.celular.Trim() : null;
                clienteDB.endereco = cliente.endereco != null ? cliente.endereco.Trim() : null;
                clienteDB.email = cliente.email != null ? cliente.email.Trim() : null;
                clienteDB.cnpj = cliente.cnpj != null ? cliente.cnpj.Trim() : null;
                clienteDB.cep = cliente.cep != null ? cliente.cep.Trim() : null;
                clienteDB.inscestadual = cliente.inscricaoEstadual != null ? cliente.inscricaoEstadual.Trim() : null;
                clienteDB.inscmunicial = cliente.inscricaoMunicipal != null ? cliente.inscricaoMunicipal.Trim() : null;
                clienteDB.qtdempresas = cliente.quantidadeEmpresas;
                clienteDB.vertodasempresas = cliente.quantidadeEmpresas <= 0 ? true : false;
                clienteDB.dtexpiracaolicenca = cliente.dataExpiracao;
                db.SaveChanges();

                var clienteVertente = db.cliente_vertentes.Where(x => x.idcliente == clienteDB.idclientes).Select(x => x).FirstOrDefault();
                if (clienteVertente != null)
                {
                    clienteVertente.produtos = cliente.vertentes.Contains("produtos") ? true : false;
                    clienteVertente.presencaonline = cliente.vertentes.Contains("presencaonline") ? true : false;
                    clienteVertente.promocoes = cliente.vertentes.Contains("promocoes") ? true : false;
                    clienteVertente.noticias = cliente.vertentes.Contains("noticias") ? true : false;
                    clienteVertente.redessociais = cliente.vertentes.Contains("redessocias") ? true : false;
                    db.SaveChanges();
                }
                else
                {

                    var clienteVertentes = new ZAdmin_DB.Model.cliente_vertentes();
                    clienteVertentes.idcliente = clienteDB.idclientes;
                    clienteVertentes.produtos = cliente.vertentes.Contains("produtos") ? true : false;
                    clienteVertentes.presencaonline = cliente.vertentes.Contains("presencaonline") ? true : false;
                    clienteVertentes.promocoes = cliente.vertentes.Contains("promocoes") ? true : false;
                    clienteVertentes.noticias = cliente.vertentes.Contains("noticias") ? true : false;
                    clienteVertentes.redessociais = cliente.vertentes.Contains("redessocias") ? true : false;

                    db.cliente_vertentes.Add(clienteVertentes);

                    db.SaveChanges();

                }

                if (cliente.quantidadeEmpresas > 0)
                {
                    cliente.vertodasempresas = false;
                    var listaEmpresasCliente = db.cliente_empresas.Where(x => x.idcliente == clienteDB.idclientes && x.excluido == false).Select(x => x.idempresa.Value).ToList();
                    cliente.listaEmpresas = cliente.listaEmpresas == null ? new List<int>() : cliente.listaEmpresas;
                    var empresasAdicionar = cliente.listaEmpresas.Except(listaEmpresasCliente);
                    var empresasDeletar = listaEmpresasCliente.Except(cliente.listaEmpresas);

                    if (empresasAdicionar.Any())
                    {
                        foreach (var empresa in empresasAdicionar)
                        {
                            var clienteEmpresa = new ZAdmin_DB.Model.cliente_empresas();
                            clienteEmpresa.idempresa = empresa;
                            clienteEmpresa.idcliente = clienteDB.idclientes;
                            clienteEmpresa.dtcadastro = DateTime.Now;
                            clienteEmpresa.excluido = false;
                            db.cliente_empresas.Add(clienteEmpresa);
                            db.SaveChanges();
                        }
                    }

                    if (empresasDeletar.Any())
                    {
                        foreach (var empresa in empresasDeletar)
                        {
                            var empresaDB = db.cliente_empresas.Where(x => x.excluido == false && x.idempresa == empresa && x.idcliente == cliente.idcliente).Select(x => x).FirstOrDefault();
                            empresaDB.excluido = true;
                            db.SaveChanges();
                        }
                    }
                }
               


            }
        }

        public void Deletar(int idcliente)
        {
            var db = new ZAdmin_DB.Model.zeengEntities();

            var clienteDB = (from c in db.clientes
                             where c.idclientes == idcliente
                             select c).FirstOrDefault();
            if (clienteDB != null)
            {
                clienteDB.excluido = true;
                db.SaveChanges();
            }
        }

        public List<Estado> RetornaListaDeEstados()
        {
            var db = new ZAdmin_DB.Model.zeengEntities();

            var listaEstados = (from e in db.estados
                                orderby e.nome
                                select new Estado()
                                {
                                    idestado = e.idestado,
                                    nome = e.nome,
                                    uf = e.uf
                                }).ToList();

            return listaEstados;
        }

        public bool VerificaClienteExistente(string cnpj)
        {
            bool resultado;

            var db = new ZAdmin_DB.Model.zeengEntities();

            resultado = (from c in db.clientes
                         where (c.excluido == false) && (c.cnpj.Equals(cnpj))
                         select c).Any();

            return resultado;
        }

        public bool ExisteCNPJBancoEditar(string cnpjcliente, int idcliente)
        {
            bool resultado;
            var db = new ZAdmin_DB.Model.zeengEntities();

            var verificacnpj = (from cnpj in db.clientes
                                where cnpj.idclientes == idcliente && cnpj.cnpj.Equals(cnpjcliente)
                                select cnpj);


            if (verificacnpj.Any())
            {
                resultado = true;
            }
            else
            {
                resultado = false;
            }

            return resultado;
        }

        public List<Combos.ItemCombo> EmpresasCliente(int idCliente)
        {

            var db = new ZAdmin_DB.Model.zeengEntities();
            var cliente = db.clientes.Where(x => x.idclientes == idCliente).Select(x => x).FirstOrDefault();
            
            if(cliente.vertodasempresas == false) { 
            var empresas = (from empresa in db.cliente_empresas
                            join emp in db.empresas on empresa.idempresa equals emp.idempresa
                            where empresa.idcliente == idCliente && empresa.excluido == false
                            select new Combos.ItemCombo()
                            {
                                idItem = (int)empresa.idempresa,
                                nome = emp.nome
                            }).ToList();
                return empresas;

            }else
            {
                return null;
            }


        }

    }
}
