using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZAdmin.ViewModel
{
    public class ProdutoCadastro
    {

        public ZAdmin_RN.Produtos.Produto Produto { get; set; }

        
        public List<ZAdmin_RN.Marcas.Marca> Marcas { get; set; }
        public List<ZAdmin_RN.Empresas.Empresa> Empresas { get; set; }
        

    }



}