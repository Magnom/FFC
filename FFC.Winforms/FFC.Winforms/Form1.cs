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

namespace FFC.Winforms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var crwl = new FFCrawler();
            crwl.InitializeDownLoader(crwl.TmpFolder, null);
            crwl.TmpFolder = "C:\\tmpFiles\\";

            crwl.InitializeDownLoader(crwl.TmpFolder, "");
            crwl.ParseUserList(textBox1.Text);
        }
    }
}
