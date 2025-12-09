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
        private KolonBilgisi _hedefKolonBilgisi;

        private KolonBilgisi _kaynakAramaIdKolonBilgisi;

        private KaynakDonusumUyarıBilgisi _kaynakUyariBilgisi;
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
                                 KolonBilgisi kaynakKolonBilgisi, KolonBilgisi hedefKolonBilgisi,KolonBilgisi kaynakAramaIdKolonBilgisi,KaynakDonusumUyarıBilgisi kaynakUyariBilgisi,
                                 string aramaTablo, string aramaDegerKolon, string aramaIdKolon,
                                 BaglantiBilgileri kaynakBaglanti,
                                 DonusumTuru donusumTipi)
        {
            InitializeComponent();

            _kaynakKolonAdi = kaynakKolonAdi;
            _kaynakTabloAdi = kaynakTabloAdi;

            _kaynakKolonBilgisi = kaynakKolonBilgisi;
            _hedefKolonBilgisi = hedefKolonBilgisi;
            _kaynakAramaIdKolonBilgisi = kaynakAramaIdKolonBilgisi;

            _kaynakUyariBilgisi = kaynakUyariBilgisi;

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
            UyariGridi();


        }

        private void UyariGridi()
        {
            GrdUyari.AllowUserToAddRows = false;
            GrdUyari.AutoGenerateColumns = false;
            GrdUyari.Columns.Clear();

            var uyariMesaji = new DataGridViewTextBoxColumn
            {
                HeaderText = "Uyarı Mesajı",
                Name = "Uyari",
                DataPropertyName = "Uyari",
                ReadOnly = true,
                Width = 300
            };
            var kaynakTip = new DataGridViewTextBoxColumn
            {
                HeaderText = "Kaynak Tipi",
                Name ="KaynakTipi",
                DataPropertyName = "KaynakTipi",
                ReadOnly = true,
                Width = 120
            };
            var kaynakUzunluk = new DataGridViewTextBoxColumn
            {
                HeaderText = "Kaynak Uzunluk",
                Name ="KaynakUzunluk",
                DataPropertyName = "KaynakUzunluk",
                ReadOnly = true,
                Width = 100
            };

            var hedefTip = new DataGridViewTextBoxColumn
            {
                HeaderText = "Hedef Tipi",
                Name = "HedefTipi",
                DataPropertyName = "HedefTipi",
                ReadOnly = true,
                Width = 120
            };
            var hedefUzunluk = new DataGridViewTextBoxColumn
            {
                HeaderText = "Hedef Uzunluk",
                Name = "HedefUzunluk",
                DataPropertyName = "HedefUzunluk",
                ReadOnly = true,
                Width = 100
            };
            GrdUyari.Columns.AddRange(new DataGridViewColumn[]
            {
                uyariMesaji,
                kaynakTip,
                kaynakUzunluk,
                hedefTip,
                hedefUzunluk
            });

        }

        private void GridBaslat()
        {
            GrdKaynakDonusum.AllowUserToAddRows = false;
            GrdKaynakDonusum.AutoGenerateColumns = false;
            GrdKaynakDonusum.Columns.Clear();

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


            GrdKaynakDonusum.Columns.AddRange(new DataGridViewColumn[]
            {
                kaynakDeger,
                KaynakHedefAtanacakDeger,
                durum,
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

                object eslesenID = KaynakLookupIDGetir(satır.KaynakDeger);

                if (eslesenID != null && eslesenID != DBNull.Value)
                {
                    object aciklamaDegeri = KaynakIdIleAciklamaGetir(eslesenID);

                    satır.EslesenID = eslesenID; 

                    if (aciklamaDegeri != null && aciklamaDegeri != DBNull.Value)
                    {
                        satır.HedefKaynagaAtanacakDeger = aciklamaDegeri;
                    }
                    else
                    {
                        satır.HedefKaynagaAtanacakDeger = eslesenID.ToString();
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

        private object KaynakLookupIDGetir(string kaynakDeger)
        {
            if (string.IsNullOrWhiteSpace(_aramaTablo) || string.IsNullOrWhiteSpace(_aramaIdKolon) || string.IsNullOrWhiteSpace(_aramaDegerKolon))
            {
                return null;
            }

            
            string sql = $"SELECT TOP 1 [{_aramaIdKolon}] FROM [{_aramaTablo}] WHERE [{_aramaDegerKolon}] = @kaynakDeger";
            var parameters = new Dictionary<string, object> { { "@kaynakDeger", kaynakDeger } };

            try
            {
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
           
            UyariGridiniDoldur();
            
        }

        private void UyariGridiniDoldur()
        {
            if (_kaynakUyariBilgisi == null) return;

            GrdUyari.Rows.Clear();

            int rowIndex = GrdUyari.Rows.Add();
            DataGridViewRow row = GrdUyari.Rows[rowIndex];

            row.Cells["Uyari"].Value = _kaynakUyariBilgisi.UyariMesaji;
            row.Cells["KaynakTipi"].Value = _kaynakUyariBilgisi.KaynakTipi;
            row.Cells["KaynakUzunluk"].Value = _kaynakUyariBilgisi.KaynakUzunluk.HasValue && _kaynakUyariBilgisi.KaynakUzunluk.Value == int.MaxValue ? "MAX" : _kaynakUyariBilgisi.KaynakUzunluk?.ToString();
            row.Cells["HedefTipi"].Value = _kaynakUyariBilgisi.HedefTipi;
            row.Cells["HedefUzunluk"].Value = _kaynakUyariBilgisi.HedefUzunluk.HasValue && _kaynakUyariBilgisi.HedefUzunluk.Value == int.MaxValue ? "MAX" : _kaynakUyariBilgisi.HedefUzunluk?.ToString();

            if (_kaynakUyariBilgisi.UyariMesaji.StartsWith("🔴"))
            {
                row.DefaultCellStyle.BackColor = Color.LightCoral;
                row.DefaultCellStyle.ForeColor = Color.Black;
            }
            else if (_kaynakUyariBilgisi.UyariMesaji.StartsWith("🟡"))
            {
                row.DefaultCellStyle.BackColor = Color.LightYellow;
                row.DefaultCellStyle.ForeColor = Color.DarkOrange;
            }
            else
            {
                row.DefaultCellStyle.BackColor = Color.LightGreen;
                row.DefaultCellStyle.ForeColor = Color.DarkGreen;
            }

            GrdUyari.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
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
                var yeniListe = await _kaynakRepo.LookupDegerleriCekAsync(
                    _kaynakTabloAdi,
                    _kaynakKolonAdi,
                    _aramaTablo,
                    _aramaDegerKolon,
                    _aramaIdKolon
                );

                if (yeniListe != null && yeniListe.Any())
                {
                    _donusumListesi.Clear();
                    _donusumListesi.AddRange(yeniListe);
                    this.donusumSatiriBindingSource.ResetBindings(false);
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
            var tamamlanmisDurumlar=new List<string> { "Oto Eşleşti", "Manuel Eşleşti", "Toplu Eşleşti" };

            bool eksikEslestirmeVar = _donusumListesi.Any(satir => !satir.Durum.Equals("Null Değer", StringComparison.OrdinalIgnoreCase) &&
            !tamamlanmisDurumlar.Any(durum => satir.Durum.Equals(durum, StringComparison.OrdinalIgnoreCase)));

            if (eksikEslestirmeVar)
            {
                MessageBox.Show("Tüm kayıtların eşleştirilmesi gerekmektedir. Lütfen eksik eşleşmeleri tamamlayın.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DonusumSozlugu.Clear();

            foreach (var satir in _donusumListesi)
            {
                if (satir.HedefKaynagaAtanacakDeger!=null&&
                    !string.IsNullOrWhiteSpace(satir.HedefKaynagaAtanacakDeger.ToString()))
                {
                    if (tamamlanmisDurumlar.Any(durum=>satir.Durum.Equals(durum,StringComparison.OrdinalIgnoreCase)))
                    {
                        if (!DonusumSozlugu.ContainsKey(satir.KaynakDeger)) 
                        {
                            DonusumSozlugu.Add(satir.KaynakDeger, satir.HedefKaynagaAtanacakDeger);
                        }

                    }

                }
            }
            this.DialogResult=DialogResult.OK;
            this.Close();
        }



        private void GrdKaynakDonusum_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || GrdKaynakDonusum.Columns[e.ColumnIndex].Name != "BtnEkle")
                return;

            var satır = _donusumListesi[e.RowIndex];

            if (satır.Durum != "Eşleşme Bulunamadı")
            {
                MessageBox.Show("Bu kayıt zaten eşleşmiş durumda. Yeni kayıt ekleme işlemi sadece eşleşme bulunamayan kayıtlar için yapılabilir.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show($"Kaynak Değer: '{satır.KaynakDeger}' için Kaynak Lookup tablosuna yeni bir kayıt eklemek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No)
                return;

           
            int yeniID = 0;
            

            SqlTransferRepository tekilRepo = null;

            try
            {
                tekilRepo = new SqlTransferRepository(_kaynakBaglanti);
                yeniID = YeniKayitEkleVeIDDon(satır.KaynakDeger, tekilRepo);

            }
            catch (Exception ex)
            {
            }
            finally
            {
                tekilRepo?.Dispose();
            }

            if (yeniID > 0)
            {
                object idDegeri = yeniID;
                object aciklamaDegeri = KaynakIdIleAciklamaGetir(idDegeri);

                if (aciklamaDegeri != null && aciklamaDegeri != DBNull.Value)
                {
                    satır.EslesenID = idDegeri;
                    satır.HedefKaynagaAtanacakDeger = aciklamaDegeri;
                    satır.Durum = "Toplu Eşleşti"; 

                    this.donusumSatiriBindingSource.ResetItem(e.RowIndex);
                    GrdKaynakDonusum.Refresh();

                    MessageBox.Show($"Yeni kayıt başarıyla eklendi: Kaynak ID '{idDegeri}' -> Açıklama: '{aciklamaDegeri}'", "Başarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    
                    MessageBox.Show($"Yeni eklenen Kayıt ID '{idDegeri}' için açıklama değeri çekilemedi. Kayıt eklenmiş olabilir, ancak UI güncellenemedi.", "Hata/Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show($"Yeni kayıt eklenemedi. Detaylar için önceki hata mesajlarını kontrol edin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        //private void BtnTopluEkle_Click(object sender, EventArgs e)
        //{
        //    TopluEslesmeyenleriEkle();
        //}

        //private void TopluEslesmeyenleriEkle()
        //{
        //    var eklenecekSatirlar = _donusumListesi.Where(s => s.Durum == "Eşleşme Bulunamadı").ToList();
        //    int toplamEklenecekSatir = eklenecekSatirlar.Count;

        //    if (toplamEklenecekSatir == 0)
        //    {
        //        MessageBox.Show("Eşleşme bulunamayan kayıt yok.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        return;
        //    }
        //    DialogResult result = MessageBox.Show($"{toplamEklenecekSatir} adet eşleşme bulunamayan kayıt var. Bu kayıtlar için yeni kayıt eklemek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

        //    if (result == DialogResult.No)
        //    {
        //        return;
        //    }

        //    GrdKaynakDonusum.SuspendLayout();
        //    this.donusumSatiriBindingSource.RaiseListChangedEvents = false;

        //    int basariliEklemeSayisi = 0;
        //    // ⭐️ Rollback için kaynakRepo nesnesini döngü dışına al
        //    SqlTransferRepository kaynakRepo = null;

        //    try
        //    {
        //        kaynakRepo = new SqlTransferRepository(_kaynakBaglanti);
        //        kaynakRepo.BeginTransaction(); // İşlemi Başlat

        //        foreach (var satır in eklenecekSatirlar)
        //        {
        //            // YeniKayitEkleVeIDDon metoduna transaction'lı kaynakRepo'yu gönderiyoruz
        //            int yeniID = YeniKayitEkleVeIDDon(satır.KaynakDeger, kaynakRepo);
        //            if (yeniID > 0)
        //            {
        //                satır.EslesenID = yeniID;
        //                // KaynakIdIleAciklamaGetir transaction'sız repo kullanıyor, 
        //                // ancak bu tekil ekleme commit edilince hemen ardından çekilecektir (çoğu SQL sunucuda).
        //                satır.HedefKaynagaAtanacakDeger = KaynakIdIleAciklamaGetir(yeniID);
        //                satır.Durum = "Toplu Eşleşti";
        //                basariliEklemeSayisi++;
        //            }
        //            // else durumunda YeniKayitEkleVeIDDon içinde hata mesajı gösterildi.
        //        }
        //        kaynakRepo.CommitTransaction(); // İşlemi Onayla
        //    }
        //    catch (Exception ex)
        //    {
        //        try
        //        {
        //            kaynakRepo?.RollbackTransaction();
        //        }
        //        catch (Exception rollbackEx)
        //        {
        //            MessageBox.Show($"Kayıt eklenirken hata oluştu ve işlem geri alınamadı (Rollback Hatası: {rollbackEx.Message}). Veri tutarsızlığı olabilir. Lütfen DBA ile iletişime geçin.", "Kritik Veri Bütünlüğü Hatası", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        //        }
        //        MessageBox.Show($"Kayıt eklenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        basariliEklemeSayisi = 0;

        //    }
        //    finally
        //    {
               
        //        kaynakRepo?.Dispose();

        //        this.donusumSatiriBindingSource.RaiseListChangedEvents = true;
        //        this.donusumSatiriBindingSource.ResetBindings(false);
        //        GrdKaynakDonusum.ResumeLayout();
        //        if (basariliEklemeSayisi > 0)
        //        {
        //            MessageBox.Show($"{basariliEklemeSayisi} adet kayıt Kaynak Lookup tablosuna eklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        }
        //        else if (toplamEklenecekSatir > 0 && basariliEklemeSayisi == 0)
        //        {
        //            MessageBox.Show("Toplu kayıt ekleme işlemi tamamlandı ancak hiçbir kayıt başarılı eklenemedi. Detaylar için hata mesajlarını kontrol edin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        }
        //    }
        //}

        private void BtnIptal_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private object GetVarsayilanDeger(string veriTipi, string kolonAdi)
        {

            switch (veriTipi.ToLower())
            {
                case "int":
                case "bigint":
                case "smallint":
                case "tinyint":
                case "decimal":
                case "numeric":
                case "money":
                    return 0; 

                case "bit":

                    return 1;

                case "uniqueidentifier":
                    return Guid.NewGuid(); 

                case "char":
                case "varchar":
                case "nchar":
                case "nvarchar":
                case "text":
                case "ntext":
                case "sysname": 
                                
                    return "DefaultUser";

                case "datetime":
                case "date":
                case "smalldatetime":
                case "datetime2":
                   
                    return DateTime.Now;

                default:
                    
                    throw new InvalidOperationException($"'{kolonAdi}' kolonu için desteklenmeyen zorunlu veri tipi: {veriTipi}. Manuel değer atanmalı.");
            }
        }
        private int YeniKayitEkleVeIDDon(string yeniDEger, SqlTransferRepository kaynakRepo)
        {
            try
            {
               
                List<ZorunluKolonBilgisi> zorunluKolonlar = _kaynakRepo.ZorunluKolonlariCek(_aramaTablo, _aramaIdKolon);
                List<string> kolonlar = new List<string> { $"[{_aramaDegerKolon}]" };
                List<string> parametreler = new List<string> { "@yenideger" };

                var sqlparameters = new Dictionary<string, object>
                {
                    { "@yenideger", yeniDEger }
                };

                int paramIndex = 1;

                foreach (var kolonBilgisi in zorunluKolonlar)
                {
                    string kolonAdi = kolonBilgisi.KolonAdi;
                    string veriTipi = kolonBilgisi.veriTipi;

                    if (kolonAdi.Equals(_aramaDegerKolon, StringComparison.OrdinalIgnoreCase) 
                        || kolonAdi.Equals(_aramaIdKolon,StringComparison.OrdinalIgnoreCase))
                     
                    {
                        continue;
                    }
                    string paramName = $"@p{paramIndex}";
                    kolonlar.Add($"[{kolonAdi}]");
                    parametreler.Add(paramName);

                    object varsayılanDeger = GetVarsayilanDeger(veriTipi, kolonAdi);
                    sqlparameters.Add(paramName, varsayılanDeger);
                    paramIndex++;

                }

                string kolonListesi = string.Join(", ", kolonlar);
                string parametreListesi = string.Join(", ", parametreler);
                string insertSql = $"INSERT INTO [{_aramaTablo}] ({kolonListesi}) VALUES ({parametreListesi}); SELECT SCOPE_IDENTITY();";

               
                object yeniId = kaynakRepo.ExecuteScalar(insertSql, sqlparameters);

                if (yeniId != null && yeniId != DBNull.Value)
                {                  
                    return Convert.ToInt32(yeniId);
                }
                return 0;
            }
            catch (Exception ex)
            {               
                MessageBox.Show($"Yeni kayıt eklenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);            
                throw; 
            }
        }
    }
}