using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAdmin_RN.Utils
{
    public class Captura
    {
        
        public bool GeraImagemUrl(string url, string nomeArquivo, string dirExecutavel, string dirArquivos)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.WorkingDirectory = dirArquivos; //@"C:\Users\diogo.souza\Desktop";
            startInfo.FileName = dirExecutavel + "/" + "wkhtmltoimage.exe"; //@"C:\Program Files\wkhtmltopdf\bin\wkhtmltoimage.exe";
            startInfo.Arguments = url + " " + dirArquivos + "/promocoes/" + nomeArquivo;
            //startInfo.Arguments = url + " C:\\Sites\\Imagens\\promocoes\\" + nomeArquivo;
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            if (process.ExitCode == 0)
                return true;
            else
                return false;
        }
    }
}
