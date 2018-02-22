using System.Collections.Generic;
using System.Web.Http;
using ZAdmin.Helpers;
using ZAdmin.ViewModel;
using ZAdmin_RN.Clientes;

namespace ZAdmin.Controllers
{
    public class ClienteAPIController : ApiController
    {

        // GET: api/ClienteAPI
        public ClienteListar Get(int pagina, int regporpagina)
        {

            var RNClientes = new ZAdmin_RN.Clientes.Clientes();
            var clientes = RNClientes.RetornaListaDeClientes(pagina, regporpagina);

            var ClienteListar = new ClienteListar();

            ClienteListar.Clientes = clientes;
            ClienteListar.TotalClientes = RNClientes.TotalClientes;

            return ClienteListar;
        }

        public ClienteCadastro Get(int id)
        {
            var RNCliente = new ZAdmin_RN.Clientes.Clientes();
            var resultado = RNCliente.RetornaCliente(id);
            var cadCliente = new ClienteCadastro();
            cadCliente.cliente = resultado;
            cadCliente.Estados = RNCliente.RetornaListaDeEstados();
            return cadCliente;
        }

        public bool GetVerificaCnpjClienteExistente(string cnpjCliente)
        {
            var RNClientes = new ZAdmin_RN.Clientes.Clientes();
            return RNClientes.VerificaClienteExistente(cnpjCliente);
        }

        public bool GetExisteCNPJBancoEditar(string cnpjcliente, int idcliente)
        {
            var RNCliente = new ZAdmin_RN.Clientes.Clientes();

            return RNCliente.ExisteCNPJBancoEditar(cnpjcliente, idcliente);
        }

        public List<Estado> GetRetornaEstados()
        {
            var clientesRN = new ZAdmin_RN.Clientes.Clientes();
            return clientesRN.RetornaListaDeEstados();

        }

        [HttpPost]
        public Mensagem Post(Cliente cliente)
        {
            var RNClientes = new ZAdmin_RN.Clientes.Clientes();

            var msg = new Mensagem();
            


            if (RNClientes.VerificaClienteExistente(cliente.cnpj))
            {
                msg.erro = true;
                msg.mensagem = "Cliente já cadastrado!";

            }
            else
            {

                string resultadoValidaCliente = ValidaCliente(cliente);
                if (string.IsNullOrEmpty(resultadoValidaCliente))
                {
                    RNClientes.Cadastrar(cliente);
                    msg.mensagem = "Cliente cadastrado!";
                    msg.erro = false;
                }
                else
                {
                    msg.mensagem = resultadoValidaCliente;
                }
                
            }

            return msg;
        }

        public string ValidaCliente(Cliente cliente)
        {

            List<string> lista = new List<string>();


            if (string.IsNullOrEmpty(cliente.nome))
            {
                lista.Add("Nome");
            }

            if (string.IsNullOrEmpty(cliente.cnpj))
            {
                lista.Add("Cnpj");
            }
            if (string.IsNullOrEmpty(cliente.endereco))
            {
                lista.Add("Endereço");
            }
            if (string.IsNullOrEmpty(cliente.cep))
            {
                lista.Add("Cep");
            }

            if (cliente.idestado == null)
            {
                lista.Add("Estado");
            }
            if (string.IsNullOrEmpty(cliente.cidade))
            {
                lista.Add("Cidade");
            }
            //if (string.IsNullOrEmpty(cliente.inscricaoEstadual))
            //{
            //    lista.Add("Inscricao Estadual");
            //}
            //if (string.IsNullOrEmpty(cliente.inscricaoMunicipal))
            //{
            //    lista.Add("Inscricao Municipal");
            //}

            return string.Join(", ", lista);
        }
 
        [HttpPost]
        public void Put(int id, Cliente cliente)
        {
            var RNClientes = new ZAdmin_RN.Clientes.Clientes();

            cliente.idcliente = id;

            RNClientes.Editar(cliente);
        }

        public void Delete(int id)
        {
            var RNClientes = new ZAdmin_RN.Clientes.Clientes();

            RNClientes.Deletar(id);
        }

        [HttpGet]
        public List<ZAdmin_RN.Combos.ItemCombo> tagEmpresasCliente(int idCliente)
        {
            var clientes = new ZAdmin_RN.Clientes.Clientes();
            return clientes.EmpresasCliente(idCliente);
        }

    }

}
