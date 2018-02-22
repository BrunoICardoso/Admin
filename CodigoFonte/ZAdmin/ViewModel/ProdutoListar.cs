using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZAdmin_RN.Produtos;

namespace ZAdmin.ViewModel
{
    public class ProdutoListar
    {

        public List<Produto> Produtos { get; set; }
        
        public List<ZAdmin_RN.Marcas.Marca> Marcas { get; set; }
        public List<ZAdmin_RN.Empresas.Empresa> Empresas { get; set; }
        public int TotalProdutos { get; set; }    
    
    }
}