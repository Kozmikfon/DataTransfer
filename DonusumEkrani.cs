using DataTransfer.Model;
using DataTransfer.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.SymbolStore;
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
        private SqlTransferRepository _hedefRepo;

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

        private List<HedefDonusumSatiri> _donusumListesi;


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
            _hedefRepo= new SqlTransferRepository(_hedefBaglanti);

            DonusumSozlugu = new Dictionary<string, object>();

            _donusumListesi = new List<HedefDonusumSatiri>();


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
                Text = "Ekle",
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
                using var hedefRepo = new SqlTransferRepository(_hedefBaglanti);
        
                KolonBilgisi hedefKolonBilgisi = hedefRepo.GetKolonBilgisi(_aramaTablo, _aramaDegerKolon);
                string? hedefTip = hedefKolonBilgisi?.DataType.ToLower();

                object? arananParametreDegeri = null;
                if (!string.IsNullOrWhiteSpace(kaynakDeger))
                {
                    arananParametreDegeri =_hedefRepo.DinamikTipDonusumu(kaynakDeger, hedefTip);

                }
                else
                {
                    arananParametreDegeri = DBNull.Value;
                }

                if (arananParametreDegeri == null && !string.IsNullOrWhiteSpace(kaynakDeger))
                {
                    throw new InvalidOperationException($"'{kaynakDeger}' değeri, hedef tip ({hedefTip}) için uygun formata dönüştürülemedi.");
                }


                string sql = $"SELECT TOP 1 [{_aramaIdKolon}] FROM [{_aramaTablo}] WHERE [{_aramaDegerKolon}] = @kaynakDeger";

                var parameters = new Dictionary<string, object>
                {
                    { "@kaynakDeger", arananParametreDegeri ?? DBNull.Value }
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

        private int YeniKayitEkleVeIDDon(string yeniDeger, SqlTransferRepository hedefRepo)
        {
            try
            { 
                KolonBilgisi hedefKolonBilgisi = hedefRepo.GetKolonBilgisi(_aramaTablo, _aramaDegerKolon);
                string? hedefTip= hedefKolonBilgisi?.DataType.ToLower();
                object arananParametreDegeri=hedefRepo.DinamikTipDonusumu(yeniDeger, hedefTip);

                if (arananParametreDegeri==null && !string.IsNullOrWhiteSpace(yeniDeger))
                {
                    throw new InvalidOperationException($"'{yeniDeger}' değeri, hedef tip ({hedefTip}) için uygun formata dönüştürülemedi.");
                }

                List<ZorunluKolonBilgisi> zorunluKolonlar = hedefRepo.ZorunluKolonlariCek(_aramaTablo, _aramaIdKolon);

                List<string> kolonlar = new List<string> { $"[{_aramaDegerKolon}]" };
                List<string> parametreler = new List<string> { "@yeniDeger" };

                var sqlParameters = new Dictionary<string, object>
                {
                    { "@yeniDeger", arananParametreDegeri??DBNull.Value }
                };


                int paramIndex = 1;

                foreach (var kolonBilgisi in zorunluKolonlar)
                {
                    string kolonAdi = kolonBilgisi.KolonAdi;
                    string veriTipi = kolonBilgisi.veriTipi;


                    if (kolonAdi.Equals(_aramaDegerKolon, StringComparison.OrdinalIgnoreCase) ||
                        kolonAdi.Equals(_aramaIdKolon,    StringComparison.OrdinalIgnoreCase)) 
                    {
                        continue;
                    }


                    string paramName = $"@p{paramIndex++}";
                    kolonlar.Add($"[{kolonAdi}]");
                    parametreler.Add(paramName);

                    object varsayilanDeger = GetVarsayilanDeger(veriTipi, kolonAdi);
                    sqlParameters.Add(paramName, varsayilanDeger);
                }

                string kolonListesi = string.Join(", ", kolonlar);
                string parametreListesi = string.Join(", ", parametreler);
                string insertSql = $"INSERT INTO [{_aramaTablo}] ({kolonListesi}) VALUES ({parametreListesi}); SELECT SCOPE_IDENTITY();";

                object newId = hedefRepo.ExecuteScalar(insertSql, sqlParameters);

                if (newId != null && newId != DBNull.Value)
                {
                    return Convert.ToInt32(newId);
                }

                return 0;
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show($"Zorunlu alan atama hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
            catch (Exception ex)
            {
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

                    foreach (DataRow row in kaynakdegerler.Rows)
                    {
                        object? kaynakDeger = null;

                        if (kaynakKolonIndex < row.Table.Columns.Count)
                        {
                            kaynakDeger = row[kaynakKolonIndex];
                        }

                        string? kaynakDegerString = kaynakDeger == DBNull.Value || kaynakDeger == null
                                                        ? string.Empty
                                                        : kaynakDeger.ToString();

                        var donusumSatiri = new HedefDonusumSatiri
                        {
                            KaynakDeger = kaynakDegerString,
                            HedefAtanacakDeger = null,
                            Durum = "Beklemede"
                        };
                        _donusumListesi.Add(donusumSatiri);
                    }


                    this.donusumSatiriBindingSource.ResetBindings(false);

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
                GrdDonusum.ResumeLayout();              
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


                if (row.DataBoundItem is HedefDonusumSatiri satir)
                {

                    bool yeniKayitEklendi = false;


                    if (!yeniKayitEklendi)
                    {
                        string yeniDeger = Microsoft.VisualBasic.Interaction.InputBox(
                            $"Kaynak Değer: '{satir.KaynakDeger}' için Hedef ID'yi girin.\n" +
                            $"(ID'yi değiştirmek veya {this._aramaTablo} tablosunda doğrulamak istiyorsanız)",
                            "Manuel Eşleme ve Doğrulama",
                            satir.HedefAtanacakDeger?.ToString() ?? string.Empty);

                        if (!string.IsNullOrWhiteSpace(yeniDeger))
                        {
                            if (long.TryParse(yeniDeger, out long idDegeri))
                            {

                                object aciklamaResult = HedefIdIleArananGetir(idDegeri);

                                if (aciklamaResult != null && aciklamaResult != DBNull.Value)
                                {

                                    string? aciklama = aciklamaResult.ToString();
                                    satir.HedefAtanacakDeger = idDegeri;


                                    satir.Durum = $"Manuel Eşleşti ({aciklama})";
                                }
                                else
                                {

                                    MessageBox.Show($"Girilen ID ({idDegeri}) Hedef Tablo ({_aramaTablo})'da bulunamadı.", "ID Doğrulama Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
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

                if (((DataGridView)sender).Rows[e.RowIndex].DataBoundItem is HedefDonusumSatiri satir)
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

        private void BtnTopluKayit_Click(object sender, EventArgs e)
        {
            TopluEslesmeyenleriEkle();
        }

        private void TopluEslesmeyenleriEkle()
        {
            var eklenecekSatirlar = _donusumListesi.Where(s => s.Durum == "Eşleşme Bulunamadı" && !string.IsNullOrWhiteSpace(s.KaynakDeger)).ToList();

            int toplamEklenecekSatir = eklenecekSatirlar.Count;

            if (toplamEklenecekSatir == 0)
            {
                MessageBox.Show("Yeni kayıt eklenmesi gereken satır bulunamadı veya Kaynak Değerleri boş.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show(
                $"{toplamEklenecekSatir} adet eşleşmeyen kayıt için Hedef Tabloya ({_aramaTablo}) otomatik kayıt eklemek istediğinizden emin misiniz? Bu işlem geri alınamaz (Transaction başarısız olursa geri alınır).",
                "Toplu Kayıt Ekleme Onayı",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.No)
            {
                return;
            }

            GrdDonusum.SuspendLayout();
            this.donusumSatiriBindingSource.RaiseListChangedEvents = false;

            int basariliEklemeSayisi = 0;

            try
            {
                using var hedefRepo = new SqlTransferRepository(_hedefBaglanti);
                hedefRepo.BeginTransaction(); 

                foreach (var satir in eklenecekSatirlar)
                {
                    int yeniId = YeniKayitEkleVeIDDon(satir.KaynakDeger, hedefRepo);

                    if (yeniId > 0)
                    {
                        satir.HedefAtanacakDeger = yeniId;
                        satir.Durum = "Yeni Kayıt Eklendi";
                        basariliEklemeSayisi++;
                    }
                    else
                    {
                        satir.Durum = "Ekleme Başarısız";
                    }
                }

                hedefRepo.CommitTransaction(); 
            }
            catch (Exception ex)
            {
                
                try
                {
                    new SqlTransferRepository(_hedefBaglanti).RollbackTransaction();
                }
                catch (Exception rollbackEx)
                {
                    MessageBox.Show($"Kayıt eklenirken hata oluştu ve işlem geri alınamadı (Rollback Hatası: {rollbackEx.Message}). Veri tutarsızlığı olabilir. Lütfen DBA ile iletişime geçin.", "Kritik Veri Bütünlüğü Hatası", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                   
                }

                MessageBox.Show($"Toplu kayıt eklenirken bir hata oluştu. İşlem geri alındı. Hata: {ex.Message}", "Toplu Ekleme Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                basariliEklemeSayisi = 0; 
            }
            finally
            {
                this.donusumSatiriBindingSource.RaiseListChangedEvents = true;
                GrdDonusum.ResumeLayout();
                this.donusumSatiriBindingSource.ResetBindings(false);

                if (basariliEklemeSayisi > 0)
                {
                    MessageBox.Show($"{basariliEklemeSayisi} adet kayıt başarıyla eklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (toplamEklenecekSatir > 0 && basariliEklemeSayisi == 0)
                { 
                }
            }
        }

        private object HedefIdIleArananGetir(object idDegeri)
        {
            if (string.IsNullOrWhiteSpace(_aramaTablo) || string.IsNullOrWhiteSpace(_aramaDegerKolon) || string.IsNullOrWhiteSpace(_aramaIdKolon))
            {
                return null;
            }

            try
            {

                using var hedefRepo = new SqlTransferRepository(_hedefBaglanti);

                return hedefRepo.HedefDegerGetir(
                    _aramaTablo,
                    _aramaIdKolon,
                    idDegeri,
                    _aramaDegerKolon
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hedef açıklama aranırken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

       
    }
}
