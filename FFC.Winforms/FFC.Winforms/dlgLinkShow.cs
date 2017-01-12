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
    public partial class dlgLinkShow : Form
    {
        public dlgLinkShow()
        {
            InitializeComponent();
        }

        public void Mostrar(Object urlsTorrent, Film currFilm)
        {
            Text = currFilm.Title;
            dataGridView1.DataSource = urlsTorrent;
            Show();


        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
               
                var v = new dlgBrowser();
                v.Mostrar(                    (string)
                        dataGridView1.CurrentRow.DataBoundItem.GetType()
                            .GetProperty("Url")
                            .GetValue(dataGridView1.CurrentRow.DataBoundItem, null));
            }
        }
        
    }
}
