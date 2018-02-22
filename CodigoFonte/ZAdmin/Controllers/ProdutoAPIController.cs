using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ZAdmin.Helpers;
using ZAdmin.ViewModel;
using ZAdmin_RN.Empresas;
using ZAdmin_RN.Marcas;
using ZAdmin_RN.Produtos;

namespace ZAdmin.Controllers
{
    public class ProdutoAPIController : ApiController
    {

        //GET: api/ProdutoAPI
        public ProdutoListar Get(int idEmpresa, int idMarca, int pagina, int qtdporpagina)
        {
            var produtosRN = new ZAdmin_RN.Produtos.Produtos();

            var produtosLista = new ProdutoListar();

            produtosLista.Produtos = produtosRN.RetornaProdutos(idEmpresa, idMarca, pagina, qtdporpagina);
            produtosLista.Marcas = produtosRN.RetornaMarcas();
            produtosLista.Empresas = produtosRN.RetornaEmpresas();
            produtosLista.TotalProdutos = produtosRN.TotalProdutos;

            return produtosLista;
        }
        
        public List<Empresa> GetEmpresas()
        {
            var produtosRN = new ZAdmin_RN.Produtos.Produtos();
            return produtosRN.RetornaEmpresas();
        }

        public List<Marca> GetMarcas(int idempresa)
        {
            var produtosRN = new ZAdmin_RN.Produtos.Produtos();
            return produtosRN.RetornaMarcas(idempresa);
        }

        public List<Produto> GetListaNomeDeProdutos()
        {
            var produtosRN = new ZAdmin_RN.Produtos.Produtos();

            return produtosRN.RetornaListaNomeProdutos();

        }
        
        public ProdutoCadastro Get(int id)
        {

            var RNProduto = new ZAdmin_RN.Produtos.Produtos();
            var resultadoProduto = RNProduto.RetornaProduto(id);

            var produtoCadastrado = new ProdutoCadastro();

            produtoCadastrado.Produto = resultadoProduto;
            produtoCadastrado.Empresas = RNProduto.RetornaEmpresas();
            produtoCadastrado.Marcas = RNProduto.RetornaMarcas(resultadoProduto.idempresa);

            return produtoCadastrado;
        }
        
        [HttpPost]
        public Mensagem Post(string nomeProduto, int idmarcaProduto)
        {
            var RNProdutos = new ZAdmin_RN.Produtos.Produtos();
            var msg = new Mensagem();



            if (RNProdutos.VerificaProdutoExistente(nomeProduto, idmarcaProduto))
            {
                msg.erro = true;
                msg.mensagem = "Produto já cadastrado!";
            }
            else
            {
                msg.erro = false;

                var httpPostedFile = HttpContext.Current.Request.Files["imagemProduto"];
                string jsonProduto = HttpContext.Current.Request.Form[0];

                ZAdmin_RN.Produtos.Produto produto = JsonConvert.DeserializeObject<ZAdmin_RN.Produtos.Produto>(jsonProduto);
                int idProdutoCadastrado = RNProdutos.Cadastrar(produto);
                produto.idproduto = idProdutoCadastrado;
                produto.caminhoimagem = "/imagens/padrao.png";

                
                if (HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    var nomeArquivo = idProdutoCadastrado + Path.GetExtension(httpPostedFile.FileName);
                    produto.caminhoimagem = "/imagens/produtos/" + nomeArquivo;
                    if (httpPostedFile != null)
                    {
                        var caminhoSalvarArquivo = Path.Combine(HttpContext.Current.Server.MapPath("~/imagens/produtos"), nomeArquivo);

                        httpPostedFile.SaveAs(caminhoSalvarArquivo);
                    }
                }
                RNProdutos.AtualizaCaminhoImagem(produto);
            }
            return msg;
        }

        // PUT: api/ProdutoAPI/5
        //[FromBody]Produto produto
        public void Put()
        {
            var httpPostedFile = HttpContext.Current.Request.Files["imagemProduto"];
            string jsonProduto = HttpContext.Current.Request.Form[0];

            ZAdmin_RN.Produtos.Produto produto = JsonConvert.DeserializeObject<ZAdmin_RN.Produtos.Produto>(jsonProduto);

            var RNProdutos = new ZAdmin_RN.Produtos.Produtos();
        
            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var nomeArquivo = produto.idproduto + Path.GetExtension(httpPostedFile.FileName);
                produto.caminhoimagem = "/imagens/produtos/" + nomeArquivo;
                if (httpPostedFile != null)
                {
                    var caminhoSalvarArquivo = Path.Combine(HttpContext.Current.Server.MapPath("~/imagens/produtos"), nomeArquivo);

                    httpPostedFile.SaveAs(caminhoSalvarArquivo);
                }
            }
            RNProdutos.Editar(produto);

        }

        public void Delete(int id)
        {
            var RNProdutos = new ZAdmin_RN.Produtos.Produtos();
            RNProdutos.Deletar(id);
        }


    }



}
