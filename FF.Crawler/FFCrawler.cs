using FFC.Bl;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FF.Crawler
{
    public class FFCrawler : BaseCrawler
    {
        public FFC.Bl.Film CurrentFilm { get; set; }
        public List<FFC.Bl.Film> CurrentFilmList { get; set; }

        public string CurrentCrawlUrl { get; set; }
        public string TmpFolder { get; set; }

        const string UrlBase = "http://www.filmaffinity.com";

        public List<FFC.Bl.Film> ParseUserList(string urlList) {

            var file = string.Empty;
            var currPage =1;

            CurrentFilmList = new List<Film>();
            do
            {
                
                file = DownloadManager.Download("userList_" + currPage , urlList + "&page=" + currPage);
                if (file != "") {
                    ParseUserListResult(file);
                }
                currPage++;
            }
            while (file != "");
            

            return CurrentFilmList;

        }

        public override List<string> ParseSearch(string strSearch)
        {
            
            string result = string.Empty;
            var file = string.Empty;
            CurrentCrawlUrl = CurrentFilm.GetFilmUrl();
            var txtBuscar = clearName(CurrentFilm.Title).Trim();
            
            file = DownloadManager.Download(CurrentFilm.Id.ToString(), string.Format(strSearch, removeAccents(txtBuscar)));
            result = ParseSearchResult(file, CurrentFilm);
            
            if (CurrentCrawlUrl == null)
            {
                System.IO.File.Delete(TmpFolder + CurrentFilm.Id + ".htm");
                if (txtBuscar.Contains("ii"))
                {
                    txtBuscar = txtBuscar.Replace("iii", " 3");
                    txtBuscar = txtBuscar.Replace("ii", " 2");
                    var url = string.Format(strSearch, HttpUtility.UrlEncode(txtBuscar.Trim(), Encoding.GetEncoding("ISO-8859-1")).Replace(' ', '+'));

                    DownloadManager.Download(CurrentFilm.Id.ToString(), url);

                    result = ParseSearchResult(file, CurrentFilm);
                    

                }
            }
            if (CurrentCrawlUrl == null)
            {
                System.IO.File.Delete(TmpFolder + CurrentFilm.Id + ".htm");
                txtBuscar = txtBuscar.Replace("III", " Parte 3");
                txtBuscar = txtBuscar.Replace("II", " Parte 2");
                var url = string.Format(strSearch, HttpUtility.UrlEncode(txtBuscar.Trim(), Encoding.GetEncoding("ISO-8859-1")).Replace(' ', '+'));
                DownloadManager.Download(CurrentFilm.Id.ToString(), url);

                result = ParseSearchResult(file, CurrentFilm);
                

            }
            if (CurrentCrawlUrl == null)
            {
                System.IO.File.Delete(TmpFolder + CurrentFilm.Id + ".htm");
                txtBuscar = removeAccents(txtBuscar);

                var url = string.Format(strSearch, HttpUtility.UrlEncode(txtBuscar.Trim(), Encoding.GetEncoding("ISO-8859-1")).Replace(' ', '+'));

                DownloadManager.Download(CurrentFilm.Id.ToString(), url);                
                result = ParseSearchResult(file, CurrentFilm);
                

            }
            
            return new List<string>() { result };
           
        }

        private void ParseUserListResult(string filePath)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.Load(filePath, Encoding.Default);

            var d = doc.DocumentNode.SelectNodes(string.Format("//*[contains(@class,'{0}')]", "movies_list"));
            if (d != null)
            {
                d = d[0].SelectNodes("child::*");
                foreach (var q in d)
                {
                    var currFilm = new Film();
                    //foreach (var n in q.ChildNodes)
                    //{
                        var n = q.SelectNodes(string.Format("//*[contains(@class,'{0}')]", "mc-title"));
                    if (n != null) {
                        currFilm.Title = n[0].InnerText;
                    }
                        
                    //}

                    CurrentFilmList.Add(currFilm);

                }

            }
        }

        private string ParseSearchResult(string filePath, FFC.Bl.Film f)
        {
            try
            {

                HtmlDocument doc = new HtmlDocument();
                doc.Load(filePath, Encoding.Default);
                string result = string.Empty;

                //var t = doc.DocumentNode.SelectNodes(string.Format("//*[contains(@class,'{0}')]", "mc-info-container"));
                if (doc.DocumentNode.SelectNodes("//title")[0].InnerText.IndexOf("Búsqueda avanzada") == -1)
                {

                    if (doc.DocumentNode.SelectNodes("//title")[0].InnerText.IndexOf("B&uacute;squeda de &quot;") != -1)
                    {
                        var d = doc.DocumentNode.SelectNodes(string.Format("//*[contains(@class,'{0}')]", "mc-info-container"));
                        if (d != null)
                        {
                            foreach (var n in d)
                            {
                                var txt = n.InnerText;

                                if (
                                        (txt.IndexOf(f.Year) != -1 ||
                                        txt.IndexOf((Int32.Parse(f.Year) - 1).ToString()) != -1 ||
                                        txt.IndexOf((Int32.Parse(f.Year) + 1).ToString()) != -1
                                   ) && (removeAccents(txt.Replace("\r", "").Replace("\n", "")).IndexOf(removeAccents(clearName(f.GetDirectorName())), StringComparison.InvariantCultureIgnoreCase)) != -1)
                                {

                                    //var dwn = new DownLoader();
                                    //dwn.ProxyUrl = Properties.Settings.Default.URLProxy;
                                    //dwn.DestinationForder = PathFiles;

                                    //dwn.Download(f.ID.ToString(), UrlBase + n.SelectNodes("child::*")[0].FirstChild.Attributes[0].Value);
                                    result = UrlBase + n.SelectNodes("child::*")[0].FirstChild.Attributes[0].Value;
                                    //cr.LastDownload = DateTime.Now;
                                    //cr.path = PathFiles  + f.ID + ".htm";                                
                                }

                            }
                        }
                    }
                    else
                    {

                        var mainTitle = doc.DocumentNode.SelectNodes(string.Format("//*[contains(@id,'{0}')]", "main-title"))[0];

                        if (!(mainTitle.InnerText == "Búsqueda avanzada"))
                        {
                            //if (!System.IO.File.Exists(PathFiles + @"\" + f.ID + ".htm")) System.IO.File.Copy(filePath, PathFiles + @"\" + f.ID + ".htm");

                            result = UrlBase + mainTitle.FirstChild.Attributes[0].Value;
                            //cr.path = PathFiles + @"\" + f.ID + ".htm";
                            //cr.LastDownload = DateTime.Now;                    
                        }

                    }
                    return result;
                }
                else
                {
                    return "";
                }

            }
            catch (Exception e)
            {

                // throw;
                return "";
            }


        }
        public override void Parse(string cr)
        {
            try
            {
                var filePath = DownloadManager.Download(CurrentFilm.Id.ToString(), cr);

                HtmlDocument doc = new HtmlDocument();
                doc.Load(filePath, Encoding.Default);

                FFC.Bl.Film.Campos currentField = FFC.Bl.Film.Campos.tituloOriginal;
                bool procesField = false;

                var d = doc.DocumentNode.SelectNodes(string.Format("//*[contains(@class,'{0}')]", "movie-info"));
                if (d != null)
                {
                    foreach (var q in d)
                    {
                        foreach (var n in q.ChildNodes)
                        {
                            if (n.InnerText == "T&iacute;tulo original") { currentField = FFC.Bl.Film.Campos.tituloOriginal; procesField = true; continue; }
                            if (n.InnerText == "M&uacute;sica") { currentField = FFC.Bl.Film.Campos.musica; procesField = true; continue; }
                            if (n.InnerText == "Fotograf&iacute;a") { currentField = FFC.Bl.Film.Campos.fotografia; procesField = true; continue; }
                            if (n.InnerText == "Productora") { currentField = FFC.Bl.Film.Campos.productora; procesField = true; continue; }
                            if (n.InnerText == "G&eacute;nero") { currentField = FFC.Bl.Film.Campos.genero; procesField = true; continue; }
                            if (n.InnerText == "Premios") { currentField = FFC.Bl.Film.Campos.premios; procesField = true; continue; }

                            if (n.InnerText == "Director") { currentField = FFC.Bl.Film.Campos.director; procesField = true; continue; }

                            if (n.InnerText == "Gui&oacute;n") { currentField = FFC.Bl.Film.Campos.guion; procesField = true; continue; }
                            if (n.InnerText == "Duraci&oacute;n") { currentField = FFC.Bl.Film.Campos.duracion; procesField = true; continue; }


                            if (procesField && n.InnerText.Trim() != "")
                            {
                                var valorInsertar = n.InnerText;
                                if (currentField == FFC.Bl.Film.Campos.duracion)
                                {
                                    if (CurrentFilm.Duration == 0)
                                    {
                                        valorInsertar = valorInsertar.Replace("&nbsp;min.", "").Trim();
                                        CurrentFilm.SetFieldValue(currentField, valorInsertar, n.InnerHtml);
                                    }
                                }
                                else
                                {
                                    CurrentFilm.SetFieldValue(currentField, valorInsertar, n.InnerHtml);
                                }
                                procesField = false;
                            }

                        }
                    }
                }

                var r = doc.DocumentNode.SelectNodes(string.Format("//*[contains(@itemprop,'{0}')]", "ratingValue"));
                if (r != null)
                {
                    CurrentFilm.GlobalRatting = Double.Parse(r[0].InnerText);
                }


                var nCaratula = doc.DocumentNode.SelectNodes(string.Format("//*[contains(@id,'{0}')]", "movie-main-image-container"));
                if (nCaratula != null)
                {
                    //f.UrlImagen = nCaratula[0].FirstChild.NextSibling.Attributes[1].Value;
                    var urlCaratula = nCaratula[0].FirstChild.NextSibling.Attributes.Where(p => p.Name == "href").FirstOrDefault();
                    if (urlCaratula != null) CurrentFilm.UrlImagen = urlCaratula.Value;
                    else
                    {
                        urlCaratula = nCaratula[0].FirstChild.NextSibling.Attributes.Where(p => p.Name == "src").FirstOrDefault();
                        if (urlCaratula != null) CurrentFilm.UrlImagen = urlCaratula.Value;
                    }

                    if (CurrentFilm.UrlImagen != "")
                    {
                        WebClient webclient = new WebClient();
                        using (Stream stream = webclient.OpenRead(CurrentFilm.UrlImagen))
                        {
                            byte[] array;
                            using (var ms = new MemoryStream())
                            {
                                stream.CopyTo(ms);
                                array = ms.ToArray();
                            }

                            /*var cov = CurrentFilm.getCover();
                            cov.Imagen = array;
                            cov.Update();*/
                        }

                    }

                }

                var tabs = doc.DocumentNode.SelectNodes(string.Format("//*[@class='{0}']", "ntabs"));
                if (tabs != null)
                {
                    foreach (var tab in tabs[0].ChildNodes)
                    {
                       /* if (tab.InnerText.Contains("Trailers"))
                        {
                            var crImageUrl = CurrentFilm.getFilmAffinityVideoUrl();
                            crImageUrl.Url = UrlBase + tab.FirstChild.Attributes[0].Value;
                            crImageUrl.NumberItems = (int?)Int32.Parse(tab.InnerText.Replace("Trailers", "").Replace("&nbsp;", "").Replace("[", "").Replace("]", ""));
                            crImageUrl.Update();
                        }

                        if (tab.InnerText.Contains("Im&aacute;genes"))
                        {
                            var crVideoUrl = CurrentFilm.getFilmAffinityImageUrl();
                            crVideoUrl.Url = UrlBase + tab.FirstChild.Attributes[0].Value;
                            crVideoUrl.NumberItems = (int?)Int32.Parse(tab.InnerText.Replace("Im&aacute;genes", "").Replace("&nbsp;", "").Replace("[", "").Replace("]", ""));
                            crVideoUrl.Update();
                        }*/

                    }
                }


                //CurrentFilm.Update();
                //cr.Update();
                //cr.LastRead = DateTime.Now;

            }
            catch (Exception e)
            {

                throw;
            }
        }
    }

}
