using DataTransfer.Model;
using DataTransfer.Repository;
using DataTransfer.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataTransfer
{
    public partial class KaynakDonusumEkrani : Form
    {
        private BaglantiBilgileri _kaynakBaglanti;

        private SqlTransferRepository _kaynakRepo;

        private KolonBilgisi _kaynakKolonBilgisi;

        private string _kaynakKolonAdi;
        private string _kaynakTabloAdi;

        private string _aramaTablo;
        private string _aramaDegerKolon;
        private string _aramaIdKolon;

        private DonusumTuru _donusumTipi;
        public Dictionary<string, object> DonusumSozlugu { get; private set; }
        private List<KaynakDonusumSatiri> _donusumListesi;
       

        private BindingSource donusumSatiriBindingSource;
        public KaynakDonusumEkrani(string kaynakKolonAdi, string kaynakTabloAdi,
                                KolonBilgisi kaynakKolonBilgisi, KolonBilgisi hedefKolonBilgisi,
                                string aramaTablo, string aramaDegerKolon, string aramaIdKolon,
                                BaglantiBilgileri kaynakBaglanti,
                                DonusumTuru donusumTipi)
        {
            InitializeComponent();

            _kaynakKolonAdi = kaynakKolonAdi;
            _kaynakTabloAdi = kaynakTabloAdi;

            _kaynakKolonBilgisi = kaynakKolonBilgisi;

            _aramaTablo = aramaTablo;
            _aramaDegerKolon = aramaDegerKolon;
            _aramaIdKolon = aramaIdKolon ?? string.Empty;
            _kaynakBaglanti = kaynakBaglanti;
            _donusumTipi = donusumTipi;

            _kaynakRepo = new SqlTransferRepository(_kaynakBaglanti);
            this.donusumSatiriBindingSource = new System.Windows.Forms.BindingSource();

            DonusumSozlugu = new Dictionary<string, object>();
            _donusumListesi = new List<KaynakDonusumSatiri>();

            this.donusumSatiriBindingSource.DataSource = _donusumListesi;
            GrdKaynakDonusum.DataSource = this.donusumSatiriBindingSource;

            GridBaslat();


        }

        private void GridBaslat()
        {
            GrdKaynakDonusum.AllowUserToAddRows = false;
            GrdKaynakDonusum.AutoGenerateColumns = false;
            GrdKaynakDonusum.Columns.Clear();

            // LookupEslesmeSatiri modeline göre kolonlar
            var kaynakDeger = new DataGridViewTextBoxColumn
            {
                HeaderText = "Kaynak Değer (Orijinal)",
                DataPropertyName = "KaynakDeger",
                ReadOnly = true,
                Width = 180
            };
            var KaynakHedefAtanacakDeger = new DataGridViewTextBoxColumn
            {
                HeaderText = "Kaynaktaki Hedefe Atanacak Değer",
                DataPropertyName = "HedefKaynagaAtanacakDeger",
                Name = "KolonHedefDeger",
                ReadOnly = true,
                Width = 150
            };

            var durum = new DataGridViewTextBoxColumn
            {
                HeaderText = "Eşleşme Durumu",
                DataPropertyName = "Durum",
                ReadOnly = true,
                Width = 120
            };


            var ekleIslemi = new DataGridViewButtonColumn
            {
                Name = "BtnEkle",
                HeaderText = "Aksiyon",
                Text = "Ekle",
                UseColumnTextForButtonValue = true,
                Width = 75
            };

            GrdKaynakDonusum.Columns.AddRange(new DataGridViewColumn[]
            {
                kaynakDeger,
                KaynakHedefAtanacakDeger,
                durum,
                ekleIslemi
            });
        }

        private object KaynakIdIleAciklamaGetir(object idDegeri)
        {
            if (idDegeri == null || idDegeri == DBNull.Value ||
                string.IsNullOrWhiteSpace(_aramaTablo) ||
                string.IsNullOrWhiteSpace(_aramaDegerKolon) ||
                string.IsNullOrWhiteSpace(_aramaIdKolon))
                return null;

            try
            {
                return _kaynakRepo.HedefDegerGetir(
                     _aramaTablo,
                     _aramaIdKolon,
                     idDegeri,
                     _aramaDegerKolon
                 );
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kaynak ID doğrulaması sırasında hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private void OtomatikAramaVeGuncelle()
        {
            foreach (var satır in _donusumListesi)
            {
                if (satır.Durum == "Tamamlandı" || satır.Durum == "Oto Eşleşti") continue;

                // Kaynak değerini alıp, Kaynak Lookup tablosunda ID'sini ara
                object eslesenID = KaynakLookupIDGetir(satır.KaynakDeger);

                if (eslesenID != null && eslesenID != DBNull.Value)
                {
                    // ID'nin karşılığı olan Açıklama değerini çek
                    object aciklamaDegeri = KaynakIdIleAciklamaGetir(eslesenID);

                    satır.EslesenID = eslesenID; // KRİTİK: Veritabanına gidecek ID

                    if (aciklamaDegeri != null && aciklamaDegeri != DBNull.Value)
                    {
                        satır.HedefKaynagaAtanacakDeger = aciklamaDegeri; // UI'a Açıklama
                    }
                    else
                    {
                        satır.HedefKaynagaAtanacakDeger = eslesenID.ToString(); // Açıklama yoksa ID'yi göster
                    }

                    satır.Durum = "Oto Eşleşti";
                }
                else
                {
                    satır.HedefKaynagaAtanacakDeger = null;
                    satır.Durum = "Eşleşme Bulunamadı";
                }
            }
            this.donusumSatiriBindingSource.ResetBindings(false);
            GrdKaynakDonusum.Refresh();
        }

        // Otomatik arama için: Kaynak değerine karşılık gelen Kaynak ID'sini arama tablosundan çeker.
        private object KaynakLookupIDGetir(string kaynakDeger)
        {
            if (string.IsNullOrWhiteSpace(_aramaTablo) || string.IsNullOrWhiteSpace(_aramaIdKolon) || string.IsNullOrWhiteSpace(_aramaDegerKolon))
            {
                return null;
            }

            // SQL: SELECT TOP 1 [ID_KOLONU] FROM [ARAMA_TABLOSU] WHERE [DEĞER_KOLONU] = @kaynakDeger
            string sql = $"SELECT TOP 1 [{_aramaIdKolon}] FROM [{_aramaTablo}] WHERE [{_aramaDegerKolon}] = @kaynakDeger";
            var parameters = new Dictionary<string, object> { { "@kaynakDeger", kaynakDeger } };

            try
            {
                // Repository'nin ExecuteScalar metodunu kullanarak kaynak veritabanında çalıştır.
                return _kaynakRepo.ExecuteScalar(sql, parameters);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kaynak lookup tablosunda değer aranırken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private async void KaynakDonusumEkrani_Load(object sender, EventArgs e)
        {
            await EslesmeVerileriniGosterAsync();
        }

        private async Task EslesmeVerileriniGosterAsync()
        {
            if (string.IsNullOrWhiteSpace(_aramaTablo))
            {
                MessageBox.Show("Kaynak Lookup ayarları eksik veya yapılmamış.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                GrdKaynakDonusum.DataSource = null;
                return;
            }

            GrdKaynakDonusum.SuspendLayout();
            try
            {
                // 1. Repository'de yeni oluşturduğumuz metodu çağırıyoruz.
                // NOT: Bu metot henüz SqlTransferRepository'ye eklenmedi, bu yüzden çalışmayacaktır (aşağıdaki adımda eklememiz gerekecek).
                var yeniListe = await _kaynakRepo.LookupDegerleriCekAsync(
                    _kaynakTabloAdi,
                    _kaynakKolonAdi,
                    _aramaTablo,
                    _aramaDegerKolon,
                    _aramaIdKolon
                );

                if (yeniListe != null && yeniListe.Any())
                {
                    // HATA DÜZELTİLDİ: _eslesmeListesi yerine _donusumListesi kullanıldı.
                    // HATA DÜZELTİLDİ: lookupEslesmeSatiriBindingSource yerine donusumSatiriBindingSource kullanıldı.
                    _donusumListesi.Clear();
                    _donusumListesi.AddRange(yeniListe);
                    this.donusumSatiriBindingSource.ResetBindings(false); // Veri kaynağını yenile
                    OtomatikAramaVeGuncelle();
                }
                else
                {
                    MessageBox.Show("Eşleştirilecek benzersiz değer bulunamadı veya tüm değerler zaten eşleşiyor.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Veri yüklenirken kritik bir hata oluştu: {ex.Message}", "Kritik Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                GrdKaynakDonusum.ResumeLayout();
            }
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            DonusumSozlugu.Clear();
            int basariliEslesmeSayisi = 0;

            foreach (var satır in _donusumListesi)
            {
                if (satır.Durum == "Oto Eşleşti" || satır.Durum == "Manuel Eşleşti" || satır.Durum == "Toplu Eşleşti")
                {

                    object atanacakID = satır.EslesenID;

                    if (atanacakID != null && atanacakID != DBNull.Value)
                    {
                        if (!DonusumSozlugu.ContainsKey(satır.KaynakDeger))
                        {
                            DonusumSozlugu.Add(satır.KaynakDeger, atanacakID);
                            basariliEslesmeSayisi++;
                        }
                    }
                }
            }

            if (basariliEslesmeSayisi == 0)
            {
                MessageBox.Show("Sözlüğe eklenecek geçerli eşleşme bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void GrdKaynakDonusum_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Sadece "BtnEkle" butonuna ve geçerli satıra tıklanıp tıklanmadığını kontrol et
            if (e.RowIndex < 0 || GrdKaynakDonusum.Columns[e.ColumnIndex].Name != "BtnEkle")
                return;

            var satır = _donusumListesi[e.RowIndex];

            // Kullanıcıdan Kaynak Lookup ID'sini girmesini iste
            string idInput = Microsoft.VisualBasic.Interaction.InputBox(
                $"Kaynak Değer: {satır.KaynakDeger}\n\nLütfen eşleşen Kaynak ID'sini girin:",
                "Manuel ID Girişi",
                satır.HedefKaynagaAtanacakDeger?.ToString() ?? string.Empty);

            if (string.IsNullOrWhiteSpace(idInput))
                return;

            // 1. Girilen ID'yi Kaynak veritabanında doğrula ve açıklamasını çek
            object idDegeri = idInput;
            object aciklamaDegeri = KaynakIdIleAciklamaGetir(idDegeri);

            if (aciklamaDegeri != null && aciklamaDegeri != DBNull.Value)
            {
                satır.EslesenID = idDegeri;
                satır.HedefKaynagaAtanacakDeger = aciklamaDegeri;
                satır.Durum = "Manuel Eşleşti";


                this.donusumSatiriBindingSource.ResetItem(e.RowIndex);


                GrdKaynakDonusum.Refresh();

                MessageBox.Show($"Eşleştirme Başarılı: Kaynak ID '{idDegeri}' -> Açıklama: '{aciklamaDegeri}'", "Başarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {

                MessageBox.Show($"Girilen ID '{idDegeri}', Kaynak Lookup Tablosunda bulunamadı veya geçerli değil.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GrdKaynakDonusum_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var satır = GrdKaynakDonusum.Rows[e.RowIndex].DataBoundItem as KaynakDonusumSatiri;

            if (satır != null)
            {
                switch (satır.Durum)
                {
                    case "Oto Eşleşti":
                        e.CellStyle.BackColor = Color.LightGreen;
                        break;
                    case "Manuel Eşleşti":
                        e.CellStyle.BackColor = Color.YellowGreen;
                        break;
                    case "Toplu Eşleşti":
                        e.CellStyle.BackColor = Color.LightSkyBlue;
                        break;
                    case "Eşleşme Bulunamadı":
                        e.CellStyle.BackColor = Color.LightSalmon;
                        break;
                    default:
                        e.CellStyle.BackColor = Color.White;
                        break;
                }
            }
        }

        private void BtnTopluEkle_Click(object sender, EventArgs e)
        {
            var eslesmeyenSatirlar = _donusumListesi.Where(s => s.Durum == "Eşleşme Bulunamadı").ToList();

            if (!eslesmeyenSatirlar.Any())
            {
                MessageBox.Show("Toplu işlem yapılacak eşleşmeyen kayıt bulunamadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            string topluIDInput = Microsoft.VisualBasic.Interaction.InputBox(
                $"Toplam {eslesmeyenSatirlar.Count} adet eşleşmeyen kayıt bulunmaktadır.\n\nBu kayıtlara atanacak Kaynak Lookup ID'sini (örneğin '0' veya 'BILINMIYOR' ID'si) girin:",
                "Toplu ID Girişi",
                string.Empty);

            if (string.IsNullOrWhiteSpace(topluIDInput))
                return;


            object idDegeri = topluIDInput;
            object aciklamaDegeri = KaynakIdIleAciklamaGetir(idDegeri);

            if (aciklamaDegeri == null || aciklamaDegeri == DBNull.Value)
            {
                MessageBox.Show($"Girilen ID '{idDegeri}', Kaynak Lookup Tablosunda bulunamadı veya geçerli değil. Lütfen geçerli bir ID girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            int guncellenenSayi = 0;
            foreach (var satır in eslesmeyenSatirlar)
            {
                satır.EslesenID = idDegeri;
                satır.HedefKaynagaAtanacakDeger = aciklamaDegeri;
                satır.Durum = "Toplu Eşleşti";
                guncellenenSayi++;
            }


            this.donusumSatiriBindingSource.ResetBindings(false);
            GrdKaynakDonusum.Refresh();

            MessageBox.Show($"{guncellenenSayi} adet eşleşmeyen kayıt, başarıyla '{idDegeri}' ID'si ile eşleştirildi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnIptal_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
