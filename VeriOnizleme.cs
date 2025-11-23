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
        private DataTable _veri;

        public bool Onaylandi { get; private set; } = false;
        public FrmVeriOnizleme(DataTable veri)
        {
            InitializeComponent();
            _veri = veri;
        }

        private void FrmVeriOnizleme_Load(object sender, EventArgs e)
        {
            GrdOnizleme.DataSource = _veri;
            LblBilgi.Text = $"Toplam {_veri.Rows.Count} kayıt görüntüleniyor.";
        }

        private void BtnOnayla_Click(object sender, EventArgs e)
        {
           Onaylandi = true;
           Close();
        }

        private void BtnIptal_Click(object sender, EventArgs e)
        {
            Onaylandi = false;
            Close();
        }

        private void GrdOnizleme_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
