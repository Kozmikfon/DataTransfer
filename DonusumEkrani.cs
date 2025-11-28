using DataTransfer.Model;
using DataTransfer.Repository;
using DataTransfer.Service;
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
        private string _hedefKolonAdi;
        private DonusumTuru _donusumTipi;

        public Dictionary<string, object> DonusumSozlugu { get; private set; }

        private List<DonusumSatiri> _donusumListesi;


        public DonusumEkrani(string kaynakKolonAdi, string kaynakTabloAdi, string aramaTablo, string aramaDegerKolon, string aramaIdKolon, BaglantiBilgileri kaynakBaglanti, BaglantiBilgileri hedefBaglanti,string hedefKolonAdi,DonusumTuru donusumTipi)
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

            this._hedefKolonAdi = _hedefKolonAdi;
            this._donusumTipi= donusumTipi;

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



        private DataTable KaynakDegerleriCek(string kolonadi)
        {
            try
            {

                string kaynakTabloAdi = _kaynakTabloadi;

                string sql = $"SELECT DISTINCT [{kolonadi}] FROM [{kaynakTabloAdi}]";

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

        private int YeniKayitEkleVeIDDon(string yeniDeger)
        {
            try
            {
                string insertSql = $"INSERT INTO [{_aramaTablo}] ([{_aramaDegerKolon}]) VALUES (@yeniDeger); SELECT SCOPE_IDENTITY();";

                using var hedefRepo = new SqlTransferRepository(_hedefBaglanti);

                var parameters = new Dictionary<string, object>
        {
            { "@yeniDeger", yeniDeger }
        };

                object newId = hedefRepo.ExecuteScalar(insertSql, parameters);

                if (newId != null && newId != DBNull.Value)
                {
                    return Convert.ToInt32(newId);
                }

                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Yeni kayıt eklenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }

        private void GrdDonusum_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3 && e.RowIndex >= 0)
            {
                // 🚨 KRİTİK GÜNCELLEME: Listenin sınırları dışında bir indekse erişimi engelle.
                // Ayrıca, GrdDonusum.AllowUserToAddRows = true ise, en alttaki boş satırı atla.
                if (e.RowIndex >= _donusumListesi.Count || e.RowIndex == GrdDonusum.NewRowIndex)
                {
                    return; // Geçersiz satıra tıklandı, işlemi sonlandır.
                }

                // Artık 'e.RowIndex' kesinlikle _donusumListesi'nin sınırları içindedir.
                var satir = _donusumListesi[e.RowIndex];

                // Sadece "Eşleşme Bulunamadı" durumları için ek seçenek sun
                if (satir.Durum == "Eşleşme Bulunamadı")
                {
                    DialogResult secim = MessageBox.Show(
                        $"Kaynak Değer: '{satir.KaynakDeger}' için eşleşme bulunamadı.\n\n" +
                        $"Hedef Tablo ({_aramaTablo})'ya bu değeri kullanarak yeni kayıt eklemek ister misiniz?",
                        "Eşleşme Seçeneği",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Question);

                    if (secim == DialogResult.Yes)
                    {
                        // YENİ KAYIT EKLEME İŞLEMİ
                        int yeniID = YeniKayitEkleVeIDDon(satir.KaynakDeger);

                        if (yeniID > 0)
                        {
                            satir.HedefAtanacakDeger = yeniID;
                            satir.Durum = "Yeni Kayıt Eklendi";
                        }
                        else
                        {
                            MessageBox.Show("Kayıt ekleme başarısız oldu. Manuel eşlemeye geçiliyor.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else if (secim == DialogResult.Cancel)
                    {
                        return;
                    }
                }

                // MANUEL ID GİRİŞİ (Mevcut mantık)
                string yeniDeger = Microsoft.VisualBasic.Interaction.InputBox(
                    $"Kaynak Değer: '{satir.KaynakDeger}' için Hedef ID'yi girin.\n" +
                    $"(Yeni kayıt eklenmediyse veya ID'yi değiştirmek istiyorsanız)",
                    "Manuel Eşleme",
                    satir.HedefAtanacakDeger?.ToString() ?? string.Empty);

                if (!string.IsNullOrWhiteSpace(yeniDeger))
                {
                    if (long.TryParse(yeniDeger, out long idDegeri))
                    {
                        satir.HedefAtanacakDeger = idDegeri;
                        satir.Durum = "Manuel Eşleşti";
                    }
                    else
                    {
                        MessageBox.Show("Lütfen geçerli bir sayısal ID değeri girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                // Grid'i güncelle
                GrdDonusum.RefreshEdit();
            }
        }

        private void DonusumEkrani_Load(object sender, EventArgs e)
        {
            DataTable kaynakdegerler = KaynakDegerleriCek(_kaynakKolonAdi);

            if (kaynakdegerler != null && kaynakdegerler.Rows.Count > 0)
            {
                if (kaynakdegerler.Columns.Count == 0)
                {
                    MessageBox.Show("Kaynak değerler çekilirken bir hata oluştu veya tablo boş sütun döndürdü. Lütfen seçilen tablo ve kolon adlarını (özellikle 'space' gibi karakterleri) kontrol edin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _donusumListesi.Clear();
                foreach (DataRow row in kaynakdegerler.Rows)
                {
                    // Bu satır artık güvenlidir:
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
    }
}
