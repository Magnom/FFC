using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FF.Crawler
{
    public class Downloader
    {
        public string ProxyUrl { get; set; }
        public string DestinationForder { get; set; }
        public bool UseConsole { get; set; }
        public bool DownloadFiles { get; set; }

        private WebClient Cliente { get; set; }

        public Downloader()
        {
            DownloadFiles = true;
        }

        public int DownloadRange(string filename, string url, int IdMin, int IdMax)
        {
            int downLoaded = 0;
            if (Cliente == null)
            {
                Cliente = new WebClient();
                WebProxy wP = new WebProxy();
                CredentialCache cc = new CredentialCache();
                Uri UrlP = new Uri(ProxyUrl);
                wP.Address = UrlP;
                Cliente.Proxy = wP;
            }

            for (int i = IdMin; i <= IdMax; i++)
            {
                if (!System.IO.File.Exists(DestinationForder + filename + i + ".htm"))
                {
                    Uri Url = new Uri(url + i);
                    Cliente.DownloadFile(Url, filename + "actores" + i + ".htm");
                }
                downLoaded++;
                if (UseConsole) Console.WriteLine("Descargado {0}", i);
            }

            return downLoaded;

        }

        public string DownloadWithPost(string filename, string url, string postData)
        {
            if (!System.IO.File.Exists(DestinationForder + filename + ".html"))
            {

                if (Cliente == null)
                {
                    Cliente = new WebClient();
                    if (ProxyUrl != null)
                    {
                        WebProxy wP = new WebProxy();
                        CredentialCache cc = new CredentialCache();
                        Uri UrlP = new Uri(ProxyUrl);
                        wP.Address = UrlP;
                        Cliente.Proxy = wP;
                    }
                }

                Cliente.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                string HtmlResult = Cliente.UploadString(url, postData);

                System.IO.File.WriteAllText(DestinationForder + filename + ".html", HtmlResult);


            }

            return DestinationForder + filename + ".html";
        }

        public string Download(string filename, string url, bool putExt = true)
        {
            try
            {
                if (!System.IO.File.Exists(DestinationForder + filename + (putExt ? ".htm" : "")))
                {
                    if (Cliente == null)
                    {
                        Cliente = new WebClient();
                        if (!String.IsNullOrEmpty(ProxyUrl))
                        {
                            WebProxy wP = new WebProxy();
                            CredentialCache cc = new CredentialCache();
                            Uri UrlP = new Uri(ProxyUrl);
                            wP.Address = UrlP;
                            Cliente.Proxy = wP;
                        }
                    }

                    string result = DestinationForder + filename + (putExt ? ".htm" : "");

                    if (!System.IO.File.Exists(result))
                    {
                        Uri Url = new Uri(url);
                        Cliente.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                        if (DownloadFiles)
                        {
                            Cliente.DownloadFile(Url, DestinationForder + filename + (putExt ? ".htm" : ""));
                        }
                        else
                        {
                            result = "";
                        }

                    }

                    return result;
                }
                else {
                    return DestinationForder + filename + (putExt ? ".htm" : "");
                }
            }
            catch (Exception Ex) {
                return "";
            }

        }

    }
}
