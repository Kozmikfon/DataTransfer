using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataTransfer
{
    public partial class FrmVeriOnizleme : Form
    {
        public bool Onaylandi { get; set; } = false;
        public FrmVeriOnizleme(DataTable veri, string tabloAdi)
        {
            InitializeComponent();
            this.Text = $"Önizleme - {tabloAdi}";

            // Bilgi etiketi
            LblBilgi.Text = $"Toplam {veri.Rows.Count} satır, {veri.Columns.Count} kolon bulunuyor.";

            // Sadece ilk 50 satırı gösterelim
            DataTable gosterim = veri.Clone();
            int max = Math.Min(50, veri.Rows.Count);
            for (int i = 0; i < max; i++)
                gosterim.ImportRow(veri.Rows[i]);

            GrdOnizleme.DataSource = gosterim;
        }

        private void FrmVeriOnizleme_Load(object sender, EventArgs e)
        {

        }

        private void BtnOnayla_Click(object sender, EventArgs e)
        {
            Onaylandi = true;
            this.Close();
        }

        private void BtnIptal_Click(object sender, EventArgs e)
        {
            Onaylandi = false;
            this.Close();
        }

        private void GrdOnizleme_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
