using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Testes
{
    [TestClass]
    
    public class TestesPromocoes
    {

        [TestMethod]
        public void AssociaNoticiaPromocao() {

            var rnPromo = new ZAdmin_RN.Promocao.PromocaoNoticia("http://172.16.0.85:9200","dev");

            rnPromo.AssociaPromocaoNoticia(12, 60288027, 1);

        }
    }
}
