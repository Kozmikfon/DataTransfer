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
    public partial class DonusumEkrani : Form
    {
        public DonusumEkrani()
        {
            InitializeComponent();
        }

        private void GridBaslat()
        {
            GrdDonusum.AutoGenerateColumns = false;
            GrdDonusum.Columns.Clear();

            var kaynakDeger= new DataGridViewTextBoxColumn
            {
                HeaderText = "Kaynak Değer",
                DataPropertyName = "KaynakDeger",
                ReadOnly=true,
                Width = 150
            };
            var hedefAtanacakDeger = new DataGridViewTextBoxColumn
            {
                HeaderText = "Hedef Atanacak Değer",
                DataPropertyName = "HedefAtanacakDeger",
                ReadOnly=true,
                Width = 150
            };
            var durum=new DataGridViewTextBoxColumn
            {
                HeaderText = "Durum",
                DataPropertyName = "Durum",
                ReadOnly=true,
                Width = 100
            };
            var islem=new DataGridViewButtonColumn
            {
                HeaderText = "İşlem",
                Text = "Düzenle",
                UseColumnTextForButtonValue = true,
                Width = 75
                
            };

            GrdDonusum.Columns.AddRange(new DataGridViewColumn[]
            {
                kaynakDeger,
                hedefAtanacakDeger,
                durum,
                islem
            });
        }
        private void DonusumEkrani_Load(object sender, EventArgs e)
        {

        }
    }
}
