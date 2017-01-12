using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFC.Bl;
using HtmlAgilityPack;

namespace FF.Crawler
{
    public class TPBCrawler : BaseCrawler
    {
        private const string BaseUrl = "https://fastpiratebay.co.uk";
        private const string SearchUrl = "https://fastpiratebay.co.uk/s/?q={0}+&page=0&orderby=99";
        

        public Film CurrentFilm { get; set; }        
        public string TmpFolder { get; set; }
        public string UrlBase { get; set; }
        public string PathTorrent { get; set; }

        //const int FileTypeID = 4;

        public TPBCrawler()
        {
            //var fType = CRAWL_FILETYPES.getById(FileTypeID).FirstOrDefault();
            //UrlBase = fType.UrlBase;
            //PathTorrent = fType.DownloadPath;
            //UrlDownload = fType.UrlDownload;

        }


        public List<Object> ParseSearch()
        {


            var strSearch = SearchUrl;
            //var url = string.Format(SearchUrl, HttpUtility.UrlEncode(f.Title.Trim(), Encoding.GetEncoding("ISO-8859-1")).Replace(' ', '+'));
            var url = string.Format(strSearch, removeAccents(clearName(CurrentFilm.Title)).Trim().Replace(' ', '+'));
            var file = DownloadManager.Download(CurrentFilm.Id.ToString(), url);
            var lsUrl = ParseSearchResult(file, CurrentFilm, 1);
            if (lsUrl == null) lsUrl = new List<Object>();

            if (removeAccents(clearName(CurrentFilm.Title)) != CurrentFilm.Title)
            {
                url = string.Format(strSearch, CurrentFilm.Title.Trim().Replace(' ', '+'));
                file = DownloadManager.Download(CurrentFilm.Id.ToString() + "_c", url);
                var lsUrl_o = ParseSearchResult(file, CurrentFilm, 1);
                if (lsUrl_o != null) lsUrl = lsUrl.Concat(lsUrl_o).ToList<Object>();
            }

            if (lsUrl.Count == 0 && CurrentFilm.OriginalTitle != null)
            {
                url = string.Format(strSearch, CurrentFilm.OriginalTitle.Trim().Replace(' ', '+'));
                file = DownloadManager.Download(CurrentFilm.Id.ToString() + "_o", url);
                lsUrl = ParseSearchResult(file, CurrentFilm, 1);
                if (lsUrl == null) lsUrl = new List<Object>();
            }
            if (lsUrl.Count == 0)
            {
                url = string.Format(strSearch, clearName(CurrentFilm.Title).Trim().Replace(' ', '+') + ' ' + CurrentFilm.Year);
                file = DownloadManager.Download(CurrentFilm.Id.ToString() + "_y", url);
                lsUrl = ParseSearchResult(file, CurrentFilm, 100);
                if (lsUrl == null) lsUrl = new List<Object>();
                url = string.Format(strSearch, clearName(CurrentFilm.Title).Trim().Replace(' ', '+') + ' ' + CurrentFilm.GetDirectorName());
                file = DownloadManager.Download(CurrentFilm.Id.ToString() + "_d", url);
                var lsUrl2 = ParseSearchResult(file, CurrentFilm, 100);
                if (lsUrl2 == null) lsUrl2 = new List<Object>();
                lsUrl = lsUrl.Concat(lsUrl2).ToList<Object>();


            }


            return lsUrl;
        }



        private List<object> ParseSearchResult(string filePath, Film f, int maxResultPages)
        {
            try
            {

                var found = 0;
                var result = new List<object>();
                HtmlDocument doc = new HtmlDocument();
                doc.Load(filePath, Encoding.Default);



                if (doc.DocumentNode.SelectNodes(string.Format("//a[contains(@href,'{0}')]", @"/search/")).Count > (maxResultPages + 9))
                {
                }
                else
                {
                    var d = doc.GetElementbyId("searchResult");

                    if (d != null)
                    {

                        foreach (var n in d.SelectNodes(string.Format("//*[contains(@class,'{0}')]", "detName")))
                        {
                            var txt = n.InnerText;
                            var urlPirateBay = UrlBase + n.SelectNodes(string.Format("./*[contains(@class,'{0}')]", "detLink"))[0].Attributes[0].Value;
                            var descFile = n.SelectNodes(string.Format("./*[contains(@class,'{0}')]", "detLink"))[0].InnerText;
                            var urlTorrent = "http:" + n.NextSibling.NextSibling.NextSibling.NextSibling.Attributes[0].Value;


                            //f.AddLinkFile(FILMS.LinkTypes.torrent, descFile, urlPirateBay, urlTorrent);
                            found++;
                            result.Add(new { Url = BaseUrl + urlPirateBay, Title = txt, Description = descFile, UrlTorrnet = urlTorrent });
                        }

                    }

                }

                return result;
            }
            catch (Exception e)
            {
                return null;
                //throw;
            }


        }
        /*public override void Parse()
        {
            var file = DownloadManager.Download(CurrentFilm.Id + "_" + cr.Url.Split('/').Last(), cr.Url);
            ParseFilmFile(cr.path, CurrentFilm, cr);
            
        }*/
        /*
        public void ParseFilmFile(string filePath, FF.DataModel.FILMS f, FF.DataModel.CRAWL_URLS cr)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.Load(filePath, Encoding.Default);

            //if (doc.DocumentNode.InnerHtml.Contains(f.GetDirectorName()) || doc.DocumentNode.InnerHtml.Contains(f.Year))
            //{
            var l = f.GetFilmLink(cr.Url);

            if (doc.DocumentNode.InnerHtml.Contains(f.GetDirectorName()) && doc.DocumentNode.InnerHtml.Contains(f.Year == null ? "" : f.Year))
            {
                l.State = 2;
            }
            else
            {
                l.State = 4;
            }





            //get file
            var fLink = doc.DocumentNode.SelectNodes(string.Format("//*[contains(@title,'{0}')]", "Torrent File"));
            var lnkValue = GetLink(doc, string.Format("//*[contains(@title,'{0}')]", "Torrent File"));
            if (lnkValue != "")
            {
                var foder = string.Format("{0}{1}", PathTorrent, f.ID);
                if (!System.IO.Directory.Exists(foder)) System.IO.Directory.CreateDirectory(foder);
                foder += "\\";
                var pathAntes = DownloadManager.DestinationForder;
                DownloadManager.DestinationForder = PathTorrent + @"\" + f.ID + @"\";

                //no descargamos los torrents
                //var file = DownloadManager.Download(f.ID + "_" + lnkValue.Split('/').Last(), "http:" + lnkValue, false);
                //DownloadManager.DestinationForder = pathAntes;
                cr.torrentPath = f.ID + "_" + lnkValue.Split('/').Last();
                l.torrentLink = lnkValue;
                cr.Update();
                l.torrentName = lnkValue.Split('/').Last();
                //ParseTorrent(string.Concat(PathTorrent, f.ID, @"\",f.ID , "_", l.torrentName), l);
            }


            l.Update();

            //read data from link
            bool procesField = false;
            FF.DataModel.FILM_LINKS.Campos currentField = FF.DataModel.FILM_LINKS.Campos.AudioLanguage;
            var d = doc.GetElementbyId("details");
            if (d != null)
            {
                //var d1 = d.SelectNodes(string.Format("./*[contains(@class,'{0}')]", "col1"));
                //if (d1 != null)
                //{
                foreach (var cols in d.ChildNodes)
                {
                    foreach (var n in cols.ChildNodes)
                    {
                        if (n.InnerText == "Size:") { currentField = FF.DataModel.FILM_LINKS.Campos.Size; procesField = true; continue; }
                        if (n.InnerText == "Spoken language(s):") { currentField = FF.DataModel.FILM_LINKS.Campos.AudioLanguage; procesField = true; continue; }
                        if (n.InnerText == "Texted language(s):") { currentField = FF.DataModel.FILM_LINKS.Campos.TextLanguage; procesField = true; continue; }
                        if (n.InnerText == "Seeders:") { currentField = FF.DataModel.FILM_LINKS.Campos.Seeders; procesField = true; continue; }
                        if (n.InnerText == "Leechers:") { currentField = FF.DataModel.FILM_LINKS.Campos.Leechers; procesField = true; continue; }
                        if (n.InnerText == "Info Hash:") { currentField = FF.DataModel.FILM_LINKS.Campos.hash; procesField = true; continue; }


                        if (procesField && n.InnerText.Trim() != "" && n.InnerText.Trim() != "&nbsp;")
                        {
                            var valorInsertar = n.InnerText;

                            l.SetFieldValue(currentField, valorInsertar, n.InnerHtml);

                            procesField = false;
                        }
                    }
                }
                // }
            }

            l.Update();

            //}
            //else
            //{
            //    var l = f.GetFilmLink(cr.Url);
            //    l.State = 3;
            //    l.Update();
            //}
        }

        public string GetLink(HtmlDocument doc, string xPath)
        {
            try
            {
                var fLink = doc.DocumentNode.SelectNodes(xPath);
                if (fLink != null)
                {
                    return fLink.First().Attributes.Where(p => p.Name == "href").First().Value;
                }

                return "";
            }
            catch { return ""; }
        }

        public FF.DataModel.CRAWL_URLS GetCrawlFromUrl(string url)
        {
            return FF.DataModel.CRAWL_URLS.getByIdFilm(CurrentFilm.ID, "4").Where(p => p.Url == url).FirstOrDefault();
        }
        */
    }
}
