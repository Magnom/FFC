using FF.Crawler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FFC.Bl;

namespace FFC.Winforms
{
    public partial class List : Form
    {
        public List<Film> CurrList { get; set; }
        public Film CurrFilm { get; set; }

        public List()
        {
            InitializeComponent();
        }

        private void BindList()
        {
            dataGridView1.DataSource = CurrList.Select(p => new { Id = p.Id, Title = p.Title, Año = p.Year, Url = p.UrlFilmaffinity, TorrentCount = p.TorrentLinksCount, ELinkCount = p.ELinksCount })
                .ToList();

            dataGridView1.Show();
        }

        private void LoadList()
        {
            var crwl = new FFCrawler();
            crwl.InitializeDownLoader(crwl.TmpFolder, null);
            crwl.TmpFolder = "C:\\tmpFiles\\";
            crwl.InitializeDownLoader(crwl.TmpFolder, "");
            CurrList = crwl.ParseUserList(textBox1.Text);

            foreach (var f in CurrList)
            {
               // LoadTorrents(f, false);
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadList();
            BindList();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            

            //
        }
            
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                CurrFilm = Film.GetById(CurrList,
                    (int)
                        dataGridView1.CurrentRow.DataBoundItem.GetType()
                            .GetProperty("Id")
                            .GetValue(dataGridView1.CurrentRow.DataBoundItem, null));
                var v = new dlgBrowser();
                v.Mostrar("http://www.filmaffinity.es" + CurrFilm.UrlFilmaffinity);
            }
            
        }

        public List<Object> LoadTorrents(Film f, bool downloadFiles)
        {
            var crwl = new TPBCrawler();
            crwl.InitializeDownLoader(crwl.TmpFolder, null);
            crwl.TmpFolder = "C:\\tmpFiles\\Torrent\\";

            crwl.InitializeDownLoader(crwl.TmpFolder, "");
            crwl.DownloadManager.DownloadFiles = downloadFiles;
            crwl.CurrentFilm = f;
            var urlsTorrent = crwl.ParseSearch();

            f.TorrentLinks = urlsTorrent;
            f.TorrentLinksCount = urlsTorrent.Count;

            return urlsTorrent;

        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                CurrFilm = Film.GetById(CurrList,
                    (int)
                        dataGridView1.CurrentRow.DataBoundItem.GetType()
                            .GetProperty("Id")
                            .GetValue(dataGridView1.CurrentRow.DataBoundItem, null));


                var urlsTorrent = LoadTorrents(CurrFilm, true);
                
                BindList();

                var dlgLink = new dlgLinkShow();
                dlgLink.Mostrar(urlsTorrent, CurrFilm);

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }

    public class DeleteCell : DataGridViewButtonCell
    {
        Image del = Image.FromFile("..\\..\\img\\delete.ico");
        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, elementState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
            graphics.DrawImage(del, cellBounds);
        }
    }

    public class DeleteColumn : DataGridViewButtonColumn
    {
        public DeleteColumn()
        {
            this.CellTemplate = new DeleteCell();
            this.Width = 20;
            //set other options here 
        }
    }

}
