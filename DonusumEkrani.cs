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

        private KolonBilgisi _kaynakKolonBilgisi;
        private KolonBilgisi _hedefKolonBilgisi;

        private string _kaynakTabloadi;
        private string _kaynakKolonAdi;
        private string _aramaTablo;
        private string _aramaDegerKolon;
        private string _aramaIdKolon;

        private string _hedefKolonAdi;
        private DonusumTuru _donusumTipi;

        public Dictionary<string, object> DonusumSozlugu { get; private set; }

        private List<DonusumSatiri> _donusumListesi;


        public DonusumEkrani(string kaynakKolonAdi, string kaynakTabloAdi, string hedefKolonAdi, KolonBilgisi kaynakKolonBilgisi, KolonBilgisi hedefKolonBilgisi, string aramaTablo, string aramaDegerKolon, string aramaIdKolon,
                            BaglantiBilgileri kaynakBaglanti,
                                BaglantiBilgileri hedefBaglanti,
                            DonusumTuru donusumTipi)
        {
            InitializeComponent();

            _kaynakKolonAdi = kaynakKolonAdi;
            _kaynakTabloadi = kaynakTabloAdi;
            _hedefKolonAdi = hedefKolonAdi;
            _kaynakKolonBilgisi = kaynakKolonBilgisi;
            _hedefKolonBilgisi = hedefKolonBilgisi;


            _aramaTablo = aramaTablo;
            _aramaDegerKolon = aramaDegerKolon;
            _aramaIdKolon = aramaIdKolon ?? string.Empty;

            _kaynakBaglanti = kaynakBaglanti;
            _hedefBaglanti = hedefBaglanti;
            _donusumTipi = donusumTipi;

            _kaynakRepo = new SqlTransferRepository(_kaynakBaglanti);


            DonusumSozlugu = new Dictionary<string, object>();

            _donusumListesi = new List<DonusumSatiri>();

            
            this.donusumSatiriBindingSource.DataSource = _donusumListesi;
            GrdDonusum.DataSource = this.donusumSatiriBindingSource;

            GridBaslat();
        }

        private void GridBaslat()
        {
            GrdDonusum.AllowUserToAddRows = false;

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
            
            
            //GrdDonusum.DataSource = _donusumListesi;
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
            if (string.IsNullOrWhiteSpace(_aramaTablo) ||
                string.IsNullOrWhiteSpace(_aramaIdKolon) ||
                string.IsNullOrWhiteSpace(_aramaDegerKolon))
            {

                return null;
            }

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


            // Veri listesi (List<T>) zaten referans olarak bağlı olduğu için sadece yenileme yeterli.
            GrdDonusum.Refresh();

            GrdDonusum.Invalidate(true);
        }



        private void BtnDonusumKaydet_Click(object sender, EventArgs e)
        {
            var tamamlanmisDurumlar = new List<string>
    {
        "Oto Eşleşti",
        "Manuel Eşleşti",
        "Tamamlandı",
        "Yeni Kayıt Eklendi" 
       
    };

            
            bool eksikEslestirmeVar = _donusumListesi.Any(satir =>
                !satir.Durum.Equals("NULL Değer", StringComparison.OrdinalIgnoreCase) &&
                !tamamlanmisDurumlar.Any(durum => satir.Durum.Equals(durum, StringComparison.OrdinalIgnoreCase))
            );

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
              
                if (satir.HedefAtanacakDeger != null &&
                    !string.IsNullOrWhiteSpace(satir.HedefAtanacakDeger.ToString()))
                {
                   
                    if (tamamlanmisDurumlar.Any(durum => satir.Durum.Equals(durum, StringComparison.OrdinalIgnoreCase)))
                    {
                        
                        if (!DonusumSozlugu.ContainsKey(satir.KaynakDeger))
                        {
                            DonusumSozlugu.Add(satir.KaynakDeger, satir.HedefAtanacakDeger);
                        }
                    }
                }
                
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
                using var hedefRepo = new SqlTransferRepository(_hedefBaglanti);

                List<ZorunluKolonBilgisi> zorunluKolonlar = hedefRepo.ZorunluKolonlariCek(_aramaTablo, _aramaIdKolon);

                // 1. Alan adları ve parametre adları listeleri
                List<string> kolonlar = new List<string> { $"[{_aramaDegerKolon}]" };
                List<string> parametreler = new List<string> { "@yeniDeger" };

                var sqlParameters = new Dictionary<string, object>
        {
            { "@yeniDeger", yeniDeger }
        };

               
                int paramIndex = 1;
               
                foreach (var kolonBilgisi in zorunluKolonlar)
                {
                    string kolonAdi = kolonBilgisi.KolonAdi;
                    string veriTipi = kolonBilgisi.veriTipi;

                   
                    if (kolonAdi.Equals(_aramaDegerKolon, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }
                   

                    string paramName = $"@p{paramIndex++}";
                    kolonlar.Add($"[{kolonAdi}]");
                    parametreler.Add(paramName);

                    // Veri tipine göre uygun varsayılan değeri atama
                    object varsayilanDeger = GetVarsayilanDeger(veriTipi, kolonAdi);
                    sqlParameters.Add(paramName, varsayilanDeger);
                }

                // ... (Geri kalan INSERT SQL oluşturma ve çalıştırma mantığı aynı kalır)
                string kolonListesi = string.Join(", ", kolonlar);
                string parametreListesi = string.Join(", ", parametreler);
                string insertSql = $"INSERT INTO [{_aramaTablo}] ({kolonListesi}) VALUES ({parametreListesi}); SELECT SCOPE_IDENTITY();";

                object newId = hedefRepo.ExecuteScalar(insertSql, sqlParameters);

                // ... (Başarılı ID dönüşü veya hata yakalama)
                if (newId != null && newId != DBNull.Value)
                {
                    return Convert.ToInt32(newId);
                }

                return 0;
            }
            catch (InvalidOperationException ex)
            {
                // Spesifik olarak atanmamış tip hatası
                MessageBox.Show($"Zorunlu alan atama hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
            catch (Exception ex)
            {
                // Genel SQL veya bağlantı hatası
                MessageBox.Show($"Yeni kayıt eklenirken kritik bir hata oluştu: {ex.Message}", "Kritik Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
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
                    return 0; // Sayısal tipler

                case "bit":
                    
                    return 1;

                case "uniqueidentifier":
                    return Guid.NewGuid(); // GUID tipleri

                case "char":
                case "varchar":
                case "nchar":
                case "nvarchar":
                case "text":
                case "ntext":
                case "sysname": // SQL Server'ın özel string tipi
                                // Zorunlu string alanlar için standart bir değer
                    return "DefaultUser";

                case "datetime":
                case "date":
                case "smalldatetime":
                case "datetime2":
                    // Zorunlu tarih alanları için geçerli zamanı atama
                    return DateTime.Now;

                default:
                    // Hata fırlatma mantığı korunur.
                    throw new InvalidOperationException($"'{kolonAdi}' kolonu için desteklenmeyen zorunlu veri tipi: {veriTipi}. Manuel değer atanmalı.");
            }
        }



        private void DonusumEkrani_Load(object sender, EventArgs e)
        {
            GrdDonusum.SuspendLayout();

            try
            {
                DataTable kaynakdegerler = KaynakDegerleriCek(_kaynakKolonAdi);

                if (kaynakdegerler != null && kaynakdegerler.Rows.Count > 0)
                {
                    if (kaynakdegerler.Columns.Count == 0)
                    {
                        MessageBox.Show("Kaynak değerler çekilirken bir hata oluştu veya tablo boş sütun döndürdü. Lütfen seçilen tablo ve kolon adlarını kontrol edin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    _donusumListesi.Clear();
                    int kaynakKolonIndex = 0;

                    // Veriler Listeye Dolduruluyor
                    foreach (DataRow row in kaynakdegerler.Rows)
                    {
                        object kaynakDeger = null;

                        if (kaynakKolonIndex < row.Table.Columns.Count)
                        {
                            kaynakDeger = row[kaynakKolonIndex];
                        }

                        string kaynakDegerString = kaynakDeger == DBNull.Value || kaynakDeger == null
                                                        ? string.Empty
                                                        : kaynakDeger.ToString();

                        var donusumSatiri = new DonusumSatiri
                        {
                            KaynakDeger = kaynakDegerString,
                            HedefAtanacakDeger = null,
                            Durum = "Beklemede"
                        };
                        _donusumListesi.Add(donusumSatiri);
                    }

                    // --------------------------------------------------------------------------------
                    // ÖNEMLİ DEĞİŞİKLİK: BindingSource üzerinden veri yenileniyor. 
                    // Bu, _donusumListesi'ndeki yeni verileri GrdDonusum'a güvenle iletir.
                    this.donusumSatiriBindingSource.ResetBindings(false);
                    // --------------------------------------------------------------------------------

                    // Otomatik aramayı çizim olayları durmuşken yap
                    OtomatikAramaVeGuncelle();
                }
                else
                {
                    MessageBox.Show("Kaynak kolonda eşleştirilecek benzersiz değer bulunamadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Veri yüklenirken kritik bir hata oluştu: {ex.Message}", "Kritik Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // UI Güncellemelerini ve Olayları Yeniden Başlat
                GrdDonusum.ResumeLayout();
                // Tüm UI'ın yeniden çizilmesini zorla (gerekirse)
                GrdDonusum.Refresh();
            }

        }

        private void GrdDonusum_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || e.RowIndex >= GrdDonusum.Rows.Count)
            {
                return; 
            }
            
            if (e.ColumnIndex == 3)
            {
                
                GrdDonusum.EndEdit();
                GrdDonusum.CommitEdit(DataGridViewDataErrorContexts.Commit);

                var row = GrdDonusum.Rows[e.RowIndex];

                
                if (row.DataBoundItem is DonusumSatiri satir)
                {

                    bool yeniKayitEklendi = false;

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
                            int yeniID = YeniKayitEkleVeIDDon(satir.KaynakDeger);

                            if (yeniID > 0)
                            {
                                satir.HedefAtanacakDeger = yeniID;
                                satir.Durum = "Yeni Kayıt Eklendi";
                                yeniKayitEklendi = true;
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

                    // Yeni kayıt eklenmediyse ve kullanıcı "Düzenle" butonuna bastıysa manuel eşleme ekranını aç.
                    if (!yeniKayitEklendi)
                    {
                        string yeniDeger = Microsoft.VisualBasic.Interaction.InputBox(
                            $"Kaynak Değer: '{satir.KaynakDeger}' için Hedef ID'yi girin.\n" +
                            $"(ID'yi değiştirmek veya manuel eşleme yapmak istiyorsanız)",
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
                    }

                    GrdDonusum.RefreshEdit();
                    GrdDonusum.InvalidateRow(e.RowIndex);
                }
            }
        }

        private void GrdDonusum_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex == GrdDonusum.NewRowIndex)
            {
                return;
            }
            if (e.RowIndex >= ((DataGridView)sender).Rows.Count || ((DataGridView)sender).Rows[e.RowIndex].DataBoundItem == null)
            {
                return;
            }

            if (e.ColumnIndex == 2)
            {
                
                if (((DataGridView)sender).Rows[e.RowIndex].DataBoundItem is DonusumSatiri satir)
                {
                    if (satir.Durum == "Eşleşme Bulunamadı")
                    {
                        e.CellStyle.BackColor = Color.LightCoral;
                    }
                    else if (satir.Durum == "Oto Eşleşti" || satir.Durum == "Manuel Eşleşti")
                    {
                        e.CellStyle.BackColor = Color.LightGreen;
                    }
                }
            }

        }
    }
}
