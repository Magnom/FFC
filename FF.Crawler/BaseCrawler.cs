using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Crawler
{
    public class BaseCrawler : ICrawler
    {
        public Downloader DownloadManager { get; set; }
        public string UrlParse { get; set; }
        

        public static string EliminaAcentos(string texto)
        {

            if (string.IsNullOrEmpty(texto))

                return texto;

            byte[] tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(texto);

            return System.Text.Encoding.UTF8.GetString(tempBytes);

        }

        protected string removeAccents(string txt)
        {
            return txt.Replace('á', 'a').Replace('é', 'e').Replace('í', 'i').Replace('ó', 'o').Replace('ú', 'u')
                .Replace('Á', 'A').Replace('É', 'E').Replace('Í', 'I').Replace('Ó', 'O').Replace('Ú', 'U');
        }

        protected string clearName(string name)
        {

            if (name.IndexOf('(') != -1 && name.IndexOf(')') != -1)
            {
                if ((object)(name.Substring(name.IndexOf('(') + 1, name.IndexOf(')') - name.IndexOf('(') - 1)) is Int32)
                {
                    name = name.Split('(')[0];
                }
            }
            return name.Split('[')[0].Trim();
        }

        public string GetLink(HtmlNode fLink)
        {
            try
            {

                if (fLink != null)
                {
                    return fLink.Attributes.Where(p => p.Name == "href").First().Value;
                }

                return "";
            }
            catch { return ""; }
        }


        public void InitializeDownLoader(string folderPath, string proxyUrl)
        {

            DownloadManager = new Downloader();
            DownloadManager.ProxyUrl = proxyUrl;
            DownloadManager.DestinationForder = folderPath;

        }
        public virtual void Parse(string filePath)
        {
            throw new NotImplementedException();
        }


        public virtual List<string> ParseSearch(string strSearch)
        {
            throw new NotImplementedException();
        }

        public void ParseTorrent(string path)
        {
            //var dictionary = (BEncodedDictionary)BEncodedValue.Decode(File.ReadAllBytes(path));
            //var info = dictionary.Where(p => p.Key.Text == "info").FirstOrDefault();

            //            if (info != null) {
            //              var arrInfo = ((MonoTorrent.BEncoding.BEncodedDictionary)(info.Value));
            //        }
            //var name=info.Value.Where 
            //f.torrentName = ();

            //TODO: PArsear el dictionario
        }
    }
}
