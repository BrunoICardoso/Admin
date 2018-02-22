using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace ZAdmin_RN.Knewin
{
    public class RequestAPI
    {
        public object JSON { get; private set; }

        public bool Acessar(DadosRequestAPI dados)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://data.knewin.com/news");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{" +
                                        "\"key\":\"" + dados.key + "\"," +
                                        "\"query\":\"" + dados.query.Replace("\"", "\\\"") + "\"," +
                                        "\"offset\":\"0\"," +
                                        "\"defaultOperator\":\"AND\"" +
                                    "}";

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    //dynamic obJSON = JsonConvert.DeserializeObject(result);

                    //if (result.Length > 80 && result != null && result != "")
                        //return true;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }

            return false;
        }        
    }
}
