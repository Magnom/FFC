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
            dataGridView1.DataSource = CurrList.Select(p => new { Id = p.Id, Title = p.Title, Año = p.Year, Url = p.UrlFilmaffinity, TorrentCount = p.TorrentLinksCount, ELinkCount = p.ELinksCount }).OrderBy(p=>p.Title).ToList();

            dataGridView1.Show();
        }

        private void LoadList()
        {

            CurrList = FFCrawler.LoadFromFilmAffinity(textBox1.Text);

            foreach (var f in CurrList)
            {
                LoadTorrents(f, false);
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadList();
            BindList();
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
                v.Mostrar(CurrFilm.UrlFilmaffinity);
            }
            
        }

        public List<Object> LoadTorrents(Film f, bool downloadFiles)
        {
            return TPBCrawler.LoadTorrents(f, downloadFiles);

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
   

}
