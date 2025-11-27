using DataTransfer.Model;
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

        private readonly string _kaynakKolonAdi;
        private readonly string _aramaTablo;
        private readonly string _aramaDegerKolon;
        private readonly string _aramaIdKolon;

        public Dictionary<string, object> DonusumSozlugu { get; private set; }

        private List<DonusumSatiri> _donusumListesi;


        public DonusumEkrani()
        {
            InitializeComponent();

            _kaynakKolonAdi=_kaynakKolonAdi;
            _aramaTablo=_aramaTablo;
            _aramaDegerKolon=_aramaDegerKolon;
            _aramaIdKolon=_aramaIdKolon;

            DonusumSozlugu = new Dictionary<string, object>();
            GridBaslat();
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
            _donusumListesi = new List<DonusumSatiri>();
            GrdDonusum.DataSource = _donusumListesi;
        }
        private void DonusumEkrani_Load(object sender, EventArgs e)
        {

        }
    }
}
