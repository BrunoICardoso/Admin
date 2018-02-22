using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using ZAdmin.Helpers;
using ZAdmin.ViewModel;
using ZAdmin_RN.Marcas;

namespace ZAdmin.Controllers
{
    public class MarcaAPIController : ApiController
    {
        // GET: api/MarcaAPI
        public MarcaListar Get(int idSetor, int idEmpresa, int pagina, int regporpagina)
        {

            var marcasRN = new ZAdmin_RN.Marcas.Marcas();

            var marcaLista = new MarcaListar();
            marcaLista.Marcas = marcasRN.RetornaMarcas(idSetor, idEmpresa, pagina, regporpagina);

            marcaLista.Empresas = marcasRN.retornaEmpresas();
            marcaLista.Setores = marcasRN.retornaSetores();
            marcaLista.TotalMarcas = marcasRN.TotalMarcas;

            return marcaLista;

        }

        public List<Marca> GetListaNomeDeMarcas()
        {

            var marcasRN = new ZAdmin_RN.Marcas.Marcas();

            return marcasRN.RetornaListaNomeMarcas();

        }

        // GET: api/MarcaAPI/5
        public MarcaCadastro Get(int id)
        {
            var RNMarca = new ZAdmin_RN.Marcas.Marcas();
            var resultadoMarca = RNMarca.RetonaMarca(id);

            var marcaCadastro = new MarcaCadastro();
            marcaCadastro.Marca = resultadoMarca;
            marcaCadastro.Empresas = RNMarca.retornaEmpresas();
            marcaCadastro.Setores = RNMarca.retornaSetores();

            return marcaCadastro;
        }

        public bool GetVerificaCnpjExistente(string cnpjmarca)
        {
            var RNMarcas = new ZAdmin_RN.Marcas.Marcas();

            return RNMarcas.VerificaCnpjExistente(cnpjmarca);
        }

        public bool GetExisteCNPJBancoEditar(string cnpjmarca, int idmarca)
        {
            var RNMarcas = new ZAdmin_RN.Marcas.Marcas();

            return RNMarcas.ExisteCNPJBancoEditar(cnpjmarca, idmarca);
        }
        
        [HttpPost]
        public Mensagem Post(string nomeMarca)
        {
            var RNMarcas = new ZAdmin_RN.Marcas.Marcas();
            var msg = new Mensagem();


            if (RNMarcas.VerificaMarcaExistente(nomeMarca))
            {
                msg.erro = true;
                msg.mensagem = "Marca já cadastrada!";
            }
            else
            {

                msg.erro = false;

                var httpPostedFile = HttpContext.Current.Request.Files["imagemMarca"];
                string jsonMarca = HttpContext.Current.Request.Form[0];

                ZAdmin_RN.Marcas.Marca marca = JsonConvert.DeserializeObject<ZAdmin_RN.Marcas.Marca>(jsonMarca);
                int newidMarca = RNMarcas.Cadastrar(marca);
                marca.idMarca = newidMarca;
                marca.caminhoImagem = "/imagens/padrao.png";

                if (HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    var nomeArquivo = newidMarca + Path.GetExtension(httpPostedFile.FileName);
                    marca.caminhoImagem = "/imagens/marcas/" + nomeArquivo;
                    if (httpPostedFile != null)
                    {
                        var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/imagens/marcas"), nomeArquivo);
                        httpPostedFile.SaveAs(fileSavePath);
                    }
                }
                RNMarcas.AtualizaCaminhoImagem(marca);

            }
            return msg;
        }

        // POST: api/MarcaAPI

        //public async Task<HttpResponseMessage> Post()
        //{
        //    //[FromBody]Marca marca
        //    //var imagemMarca = file.ContentType;
        //    var RNMarcas = new ZAdmin_RN.Marcas.Marcas();
        //    //RNMarcas.Cadastrar(marca);



        //    // Ver se POST é MultiPart? 
        //    if (!Request.Content.IsMimeMultipartContent())
        //    {
        //        throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
        //    }
        //    // Preparar CustomMultipartFormDataStreamProvider para carga de dados
        //    // (veja mais abaixo)

        //    string fileSaveLocation = HttpContext.Current.Server.MapPath("~/imagens");
        //    CustomMultipartFormDataStreamProvider provider = new CustomMultipartFormDataStreamProvider(fileSaveLocation);


        //    List<string> files = new List<string>();
        //    try
        //    {
        //        StringBuilder sb = new StringBuilder(); // Holds the response body

        //        // Read the form data and return an async task.
        //        await Request.Content.ReadAsMultipartAsync(provider);

        //        // This illustrates how to get the form data.
        //        foreach (var key in provider.FormData.AllKeys)
        //        {
        //            foreach (var val in provider.FormData.GetValues(key))
        //            {
        //                sb.Append(string.Format("{0}: {1}\n", key, val));
        //            }
        //        }

        //        // This illustrates how to get the file names for uploaded files.
        //        foreach (var file in provider.FileData)
        //        {
        //            FileInfo fileInfo = new FileInfo(file.LocalFileName);
        //            sb.Append(string.Format("Uploaded file: {0} ({1} bytes)\n", fileInfo.Name, fileInfo.Length));
        //        }
        //        return new HttpResponseMessage()
        //        {
        //            Content = new StringContent(sb.ToString())
        //        };
        //    }
        //    catch (System.Exception e)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
        //    }


        //}


        // PUT: api/EmpresaAPI/5
        //public void Put(int id, [FromBody]Empresa empresa)

        //public void Put(int id, [FromBody]Marca marca)
        //{
        //    var RNMarcas = new ZAdmin_RN.Marcas.Marcas();
        //    marca.idMarca = id;
        //    RNMarcas.Editar(marca);

        //}

        public void Put()
        {
            var RNMarcas = new ZAdmin_RN.Marcas.Marcas();

            var httpPostedFile = HttpContext.Current.Request.Files["ImagemMarca"];
            string jsonMarca = HttpContext.Current.Request.Form[0];

            ZAdmin_RN.Marcas.Marca marca = JsonConvert.DeserializeObject<ZAdmin_RN.Marcas.Marca>(jsonMarca);

            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var nomeArquivo = marca.idMarca + Path.GetExtension(httpPostedFile.FileName);
                marca.caminhoImagem = "/imagens/marcas/" + nomeArquivo;
                if (httpPostedFile != null)
                {
                    var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/imagens/marcas"), nomeArquivo);
                    httpPostedFile.SaveAs(fileSavePath);
                }
            }

            RNMarcas.Editar(marca);
        }

        // DELETE: api/EmpresaAPI/5
        public void Delete(int id)
        {
            var RNMarcas = new ZAdmin_RN.Marcas.Marcas();

            RNMarcas.Deletar(id);

        }


        public List<MarcaExpressao> GetRetornaExpressoes(int idMarca)
        {
            var RNMarca = new ZAdmin_RN.Marcas.MarcaExpressoes();

            return RNMarca.RetornaListaDeExpressoes(idMarca);
        }

        public int salvarExpressao(MarcaExpressao expressao)
        {
            var RNMarca = new ZAdmin_RN.Marcas.MarcaExpressoes();

          return RNMarca.Cadastrar(expressao);
        }

        public void excluirExpressao(int idExpressao)
        {
            var RNMarca = new ZAdmin_RN.Marcas.MarcaExpressoes();

            RNMarca.Excluir(idExpressao);
        }


    }



    public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        public CustomMultipartFormDataStreamProvider(string path) : base(path) { }

        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            return headers.ContentDisposition.FileName.Replace("\"", string.Empty);
        }
    }


}