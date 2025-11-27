using DataTransfer.Model;
using DataTransfer.Repository;
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
        private BaglantiBilgileri _kaynakBaglanti;
        private BaglantiBilgileri _hedefBaglanti;
        private SqlTransferRepository _kaynakRepo;

        private readonly string _kaynakTabloadi;
        private readonly string _kaynakKolonAdi;
        private readonly string _aramaTablo;
        private readonly string _aramaDegerKolon;
        private readonly string _aramaIdKolon;

        public Dictionary<string, object> DonusumSozlugu { get; private set; }

        private List<DonusumSatiri> _donusumListesi;


        public DonusumEkrani(string kaynakKolonAdi, string kaynakTabloAdi, string aramaTablo, string aramaDegerKolon, string aramaIdKolon, BaglantiBilgileri kaynakBaglanti, BaglantiBilgileri hedefBaglanti)
        {
            InitializeComponent();

            _kaynakKolonAdi = kaynakKolonAdi;
            _aramaTablo = aramaTablo;
            _aramaDegerKolon = aramaDegerKolon;
            _aramaIdKolon = aramaIdKolon;
            _kaynakBaglanti = kaynakBaglanti;
            _hedefBaglanti = hedefBaglanti;
            _kaynakTabloadi = kaynakTabloAdi;
            _kaynakRepo = new SqlTransferRepository(_kaynakBaglanti);

            DonusumSozlugu = new Dictionary<string, object>();
            GridBaslat();
        }

        private void GridBaslat()
        {
            GrdDonusum.AutoGenerateColumns = false;
            GrdDonusum.Columns.Clear();

            var kaynakDeger = new DataGridViewTextBoxColumn
            {
                HeaderText = "Kaynak Değer",
                DataPropertyName = "KaynakDeger",
                ReadOnly = true,
                Width = 150
            };
            var hedefAtanacakDeger = new DataGridViewTextBoxColumn
            {
                HeaderText = "Hedef Atanacak Değer",
                DataPropertyName = "HedefAtanacakDeger",
                ReadOnly = true,
                Width = 150
            };
            var durum = new DataGridViewTextBoxColumn
            {
                HeaderText = "Durum",
                DataPropertyName = "Durum",
                ReadOnly = true,
                Width = 100
            };
            var islem = new DataGridViewButtonColumn
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


        private async void DonusumEkrani_Load(object sender, EventArgs e)
        {
            DataTable kaynakdegerler = KaynakDegerleriCek(_kaynakKolonAdi);

            if (kaynakdegerler != null && kaynakdegerler.Rows.Count > 0)
            {
                _donusumListesi.Clear();
                foreach (DataRow row in kaynakdegerler.Rows)
                {
                    object kaynakDeger = row[0] ?? DBNull.Value;

                    var donusumSatiri = new DonusumSatiri
                    {
                        KaynakDeger = kaynakDeger.ToString(),
                        HedefAtanacakDeger = null,
                        Durum = "Beklemede"
                    };
                    _donusumListesi.Add(donusumSatiri);
                }
                GrdDonusum.DataSource = null;
                GrdDonusum.DataSource = _donusumListesi;
            }
            else
            {
                MessageBox.Show("Kaynak kolonda eşleştirilecek benzersiz değer bulunamadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            OtomatikAramaVeGuncelle();
        }
        private DataTable KaynakDegerleriCek(string kolonadi)
        {
            try
            {

                string kaynakTabloAdi = _kaynakTabloadi;

                string sql = $"SELECT DISTINCT [{kolonadi}] FROM [{kaynakTabloAdi}]";

                // 🌟 GÜNCELLEME: DbHelper yerine Repository kullanıldı
                return _kaynakRepo.DataTableCalistir(sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kaynak değerler çekilirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private object HedefDegeriAra(string kaynakDeger)
        {
            try
            {
                string sql = $"SELECT TOP 1 [{_aramaIdKolon}] FROM [{_aramaTablo}] WHERE [{_aramaDegerKolon}] = @kaynakDeger";

                using var hedefRepo = new SqlTransferRepository(_hedefBaglanti);

                var parameters = new Dictionary<string, object>
                {
                    { "@kaynakDeger", kaynakDeger }
                };
                return hedefRepo.ExecuteScalar(sql, parameters);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hedef değer aranırken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        

        private void OtomatikAramaVeGuncelle()
        {

            foreach (var satır in _donusumListesi)
            {
                if (satır.Durum == "Tamamlandı" || satır.Durum == "Oto Eşleşti") continue;

                object hedefID = HedefDegeriAra(satır.KaynakDeger);

                if (hedefID != null && hedefID != DBNull.Value)
                {
                    satır.HedefAtanacakDeger = hedefID;
                    satır.Durum = "Oto Eşleşti";
                }
                else
                {
                    satır.HedefAtanacakDeger = null;
                    satır.Durum = "Eşleşme Bulunamadı"; 
                }
            }

            GrdDonusum.DataSource = null;
            GrdDonusum.DataSource = _donusumListesi;
            GrdDonusum.Refresh();
        }

        private void GrdDonusum_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3 && e.RowIndex >= 0)
            {
                var satir = _donusumListesi[e.RowIndex];
                string yeniDeger = Microsoft.VisualBasic.Interaction.InputBox(
                     $"Kaynak Değer: '{satir.KaynakDeger}' için Hedef ID'yi girin.",
                     "Manuel Eşleme",
                     satir.HedefAtanacakDeger?.ToString() ?? string.Empty);

                if (!string.IsNullOrWhiteSpace(yeniDeger))
                {
                    if (long.TryParse(yeniDeger, out long idDegeri))
                    {
                        satir.HedefAtanacakDeger = idDegeri;
                        satir.Durum = "Manuel eşleşti";
                        GrdDonusum.RefreshEdit();

                    }
                    else
                    {
                        MessageBox.Show("Lütfen geçerli bir sayısal ID değeri girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnDonusumKaydet_Click(object sender, EventArgs e)
        {
            bool eksikEslestirmeVar = _donusumListesi.Any(d => d.Durum != "Oto Eşleşti" && d.Durum != "Manuel Eşleşti" && d.Durum != "Tamamlandı" && d.Durum != "NULL Değer");

            if (eksikEslestirmeVar)
            {
                DialogResult result = MessageBox.Show("Bazı değerler için eşleştirme tamamlanmadı. Yine de kaydetmek istiyor musunuz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                {
                    return;
                }
            }

            DonusumSozlugu.Clear();
            foreach (var satir in _donusumListesi)
            {
                DonusumSozlugu.Add(satir.KaynakDeger, satir.HedefAtanacakDeger);
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnDonusumIptal_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
