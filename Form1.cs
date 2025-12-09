using DataTransfer.Model;
using DataTransfer.Repository;
using DataTransfer.Service;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Drawing.Printing;
using System.Threading.Tasks;
using static Azure.Core.HttpHeader;


namespace DataTransfer
{
    public partial class FrmVeriEslestirme : Form
    {
        private BaglantiBilgileri kaynak;
        private BaglantiBilgileri hedef;

        private SqlTransferRepository KaynakRepo;
        private SqlTransferRepository HedefRepo;
        private EslestirmeService _eslestirmeService;
        private EslestirmeBilgisi _secilenEslestirme;

        private bool _siralamaYonDurumu = true;

        private FrmBaglantiAc _oncekiForm;


        private Dictionary<string, KolonBilgisi> KaynakKolonlar =
            new Dictionary<string, KolonBilgisi>(StringComparer.OrdinalIgnoreCase);

        private Dictionary<string, KolonBilgisi> HedefKolonlar =
            new Dictionary<string, KolonBilgisi>(StringComparer.OrdinalIgnoreCase);

        public FrmVeriEslestirme(BaglantiBilgileri kaynakBilgi, BaglantiBilgileri hedefBilgi, FrmBaglantiAc oncekiForm)
        {
            InitializeComponent();

            kaynak = kaynakBilgi;
            hedef = hedefBilgi;
            KaynakRepo = new SqlTransferRepository(kaynak);
            HedefRepo = new SqlTransferRepository(hedef);
            _oncekiForm = oncekiForm;

            _eslestirmeService = new EslestirmeService();
            _secilenEslestirme = new EslestirmeBilgisi();

            GridBaslat();

            this.Load += FrmVeriEslestirme_Load;
            GrdEslestirme.DataError += GrdEslestirme_DataError;
        }

        private void GridBaslat()
        {
            GrdEslestirme.AutoGenerateColumns = false;
            GrdEslestirme.Columns.Clear();


            var kolonKaynak = new DataGridViewTextBoxColumn
            {
                Name = "KaynakKolon",
                HeaderText = "Kaynak Kolon",
                ReadOnly = true
            };
            var KaynakTip = new DataGridViewTextBoxColumn
            {
                Name = "KaynakTip",
                HeaderText = "Kaynak Data Tipi",
                ReadOnly = true
            };
            var KaynakUzunluk = new DataGridViewTextBoxColumn
            {
                Name = "KaynakUzunluk",
                HeaderText = "Kaynak Uzunluk",
                ReadOnly = true
            };
            var KaynakNullable = new DataGridViewTextBoxColumn
            {
                Name = "KaynakNullable",
                HeaderText = "Kaynak Boş Geçilebilir",
                ReadOnly = true
            };

            var kolonHedef = new DataGridViewComboBoxColumn
            {
                Name = "HedefKolon",
                HeaderText = "Hedef Kolon",
                FlatStyle = FlatStyle.Flat
            };

            var manuelDegerKolon = new DataGridViewTextBoxColumn
            {
                Name = "ManuelDeger", 
                HeaderText = "Manuel Değer",
                ReadOnly = false, 
                Width = 150
            };
            var mukerrerKolon = new DataGridViewCheckBoxColumn
            {
                Name = "IsUnique",
                HeaderText = "Benzersiz Kolon",
                ReadOnly = true,
                Width = 100,
            };

            var HedefTip = new DataGridViewTextBoxColumn
            {
                Name = "HedefTip",
                HeaderText = "Hedef Data Tipi",
                ReadOnly = true
            };
            var HedefUzunluk = new DataGridViewTextBoxColumn
            {
                Name = "HedefUzunluk",
                HeaderText = "Hedef Uzunluk",
                ReadOnly = true
            };
            var HedefNullable = new DataGridViewTextBoxColumn
            {
                Name = "HedefNullable",
                HeaderText = "Hedef Boş Geçilebilir",
                ReadOnly = true
            };


            var kolonUygunluk = new DataGridViewTextBoxColumn
            {
                Name = "Uygunluk",
                HeaderText = "Uygunluk",
                ReadOnly = true,
            };
            
            //hedef
            var kolonDonusumGerekli = new DataGridViewCheckBoxColumn
            {
                Name = "DonusumGerekli",
                HeaderText = "Dönüşüm Gerekli",
                ReadOnly = false,
                Width = 120,
                Visible=true
            };

            var donusumIslem = new DataGridViewButtonColumn
            {
                Name = "DonusumIslem",
                HeaderText = "Dönüşüm",
                Text = "Ayarla",
                UseColumnTextForButtonValue = true,
                Width = 80,
                Visible = false
            };

            
            var aramaTablo = new DataGridViewComboBoxColumn
            {
                Name = "AramaTablo",
                HeaderText = "Arama Tablosu",
                ReadOnly = false,
                Width = 100,
                Visible = false
            };

            var arananDegerKolon = new DataGridViewComboBoxColumn
            {
                Name = "AramaDegerKolon",
                HeaderText = "Arama Değer Kolonu",
                ReadOnly = false,
                Width = 150,
                FlatStyle = FlatStyle.Flat,
                Visible = false


            };

            var aramaIdKolon = new DataGridViewComboBoxColumn
            {
                Name = "AramaIdKolon",
                HeaderText = "Arama ID Kolon",
                ReadOnly = false,
                Width = 150,
                FlatStyle = FlatStyle.Flat,
                Visible = false
            };

           //kaynak
            var KaynakkolonDonusumGerekli = new DataGridViewCheckBoxColumn
            {
                Name = "KaynakDonusumGerekli",
                HeaderText = "Kaynak Dönüşüm Gerekli",
                ReadOnly = false,
                Width = 120,
                Visible = true
            };

            var KaynakdonusumIslem = new DataGridViewButtonColumn
            {
                Name = "KaynakDonusumIslem",
                HeaderText = "Kaynak Dönüşüm",
                Text = "Ayarla",
                UseColumnTextForButtonValue = true,
                Width = 80,
                Visible = false
            };

            
            var KaynakaramaTablo = new DataGridViewComboBoxColumn
            {
                Name = "KaynakAramaTablo",
                HeaderText = "Kaynak Arama Tablosu",
                ReadOnly = false,
                Width = 100,
                Visible = false
            };

            var KaynakarananDegerKolon = new DataGridViewComboBoxColumn
            {
                Name = "KaynakAramaDegerKolon",
                HeaderText = "Kaynak Arama Değer Kolonu",
                ReadOnly = false,
                Width = 150,
                FlatStyle = FlatStyle.Flat,
                Visible = false


            };

            var KaynakaramaIdKolon = new DataGridViewComboBoxColumn
            {
                Name = "KaynakAramaIdKolon",
                HeaderText = "Kaynak Arama ID Kolon",
                ReadOnly = false,
                Width = 150,
                FlatStyle = FlatStyle.Flat,
                Visible = false
            };

            GrdEslestirme.Columns.AddRange(new DataGridViewColumn[]
            {
                kolonKaynak, KaynakTip, KaynakUzunluk, KaynakNullable, kolonHedef,manuelDegerKolon, mukerrerKolon, HedefTip, HedefUzunluk, HedefNullable, kolonUygunluk,kolonDonusumGerekli,donusumIslem,aramaTablo,arananDegerKolon,aramaIdKolon,KaynakkolonDonusumGerekli,KaynakdonusumIslem,KaynakaramaTablo,KaynakarananDegerKolon,KaynakaramaIdKolon
            });

            GrdEslestirme.AllowUserToAddRows = false;
            
        }


        private async Task TablolarıAgacaYukleAsync()
        {
            try
            {                
                var KaynakTablo = await KaynakRepo.TabloGetirAsync();
                TrwKaynakTablolar.Nodes.Clear();

                foreach (var tabloAd in KaynakTablo.OrderBy(x => x))
                    TrwKaynakTablolar.Nodes.Add(new TreeNode(tabloAd) { Tag = tabloAd });


                var HedefTablo = await HedefRepo.TabloGetirAsync();
                TrwHedefTablolar.Nodes.Clear();

                foreach (var t in HedefTablo.OrderBy(x => x))
                    TrwHedefTablolar.Nodes.Add(new TreeNode(t) { Tag = t });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Tablo yükleme hatası: {ex.Message}");
            }
        }

        private async void TrwKaynakTablolar_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node?.Tag is string tablo && !string.IsNullOrWhiteSpace(tablo))
                lstLog.Items.Add($"Kaynak tablo seçildi: {tablo}");
        }

        private async void TrwHedefTablolar_AfterSelect(object sender, TreeViewEventArgs e)
        {

            if (e.Node?.Tag is string tablo && !string.IsNullOrWhiteSpace(tablo)) 
                lstLog.Items.Add($"Hedef tablo seçildi: {tablo}");
        }

        private void HedefKolonSecildi(object? sender, EventArgs e) 
        {
            if (sender is ComboBox combo && GrdEslestirme.CurrentCell != null)
            {
                var secilen = combo.SelectedItem?.ToString();
                if (string.IsNullOrWhiteSpace(secilen))
                    return;

                var row = GrdEslestirme.CurrentRow;
                if (row == null)
                    return;

                if (HedefKolonlar.TryGetValue(secilen, out var bilgi))
                {
                    row.Cells["HedefTip"].Value = bilgi.DataType;
                    row.Cells["HedefUzunluk"].Value = bilgi.Length?.ToString() ?? "";
                    row.Cells["HedefNullable"].Value = bilgi.IsNullable ? "YES" : "NO";
                }               
                GridKontrolEt(row);
            }
        }

        private void KaynakSutunBilgileriGetir(Dictionary<string, KolonBilgisi> kaynakKolon)
        {
            GrdEslestirme.Rows.Clear();
            foreach (var kaynak in kaynakKolon)
            {
                int satır = GrdEslestirme.Rows.Add();
                var row = GrdEslestirme.Rows[satır];


                row.Cells["KaynakKolon"].Value = kaynak.Key;

                row.Cells["KaynakTip"].Value = kaynak.Value.DataType;
                row.Cells["KaynakUzunluk"].Value = kaynak.Value.Length?.ToString() ?? "";
                row.Cells["KaynakNullable"].Value = kaynak.Value.IsNullable ? "YES" : "NO";

                row.Cells["Uygunluk"].Value = "";
            }
        }


        private void HedefGuncelle(List<string> hedefKolonIsimleri) //hedef kolon comboxolarına yükleme için kolon adları tutuluyor.
        {
            foreach (DataGridViewRow row in GrdEslestirme.Rows)
            {
                var cell = row.Cells["HedefKolon"] as DataGridViewComboBoxCell;

                if (cell == null) 
                    continue;

                cell.Items.Clear();

                foreach (var hedef in hedefKolonIsimleri)
                    cell.Items.Add(hedef);
            }
        }



        private void GridKontrolEt(DataGridViewRow row)
        {
            string mevcutTagString = row.Tag?.ToString();
            bool dahaOnceOnaylandi = mevcutTagString == "ONAYLANDI";

            try
            {
                bool benzersizAlanCheck = (bool)(row.Cells["IsUnique"].Value ?? false);

                var kaynakKolon = row.Cells["KaynakKolon"].Value?.ToString();
                var hedefKolon = row.Cells["HedefKolon"].Value?.ToString();
                string manuelDeger = row.Cells["ManuelDeger"].Value?.ToString();

                bool isManuelGiris = kaynakKolon == "(MANUEL GİRİŞ)";

                KolonBilgisi KaynakBilgi = null;
                KolonBilgisi HedefBilgi = null;



                if (string.IsNullOrWhiteSpace(hedefKolon) || (!isManuelGiris && string.IsNullOrWhiteSpace(kaynakKolon)))
                {
                    row.Cells["Uygunluk"].Value = "";
                    row.Cells["Uygunluk"].Style.ForeColor = Color.Empty;
                    row.Tag = null;
                    return;
                }

                if (!isManuelGiris &&
                    (!KaynakKolonlar.TryGetValue(kaynakKolon, out KaynakBilgi) ||
                     !HedefKolonlar.TryGetValue(hedefKolon, out HedefBilgi)))
                {
                    row.Cells["Uygunluk"].Value = "Kolon Bilgisi Eksik";
                    row.Cells["Uygunluk"].Style.ForeColor = Color.Red;
                    row.Tag = null;
                    return;
                }

                EslestirmeSonucu sonuc = new EslestirmeSonucu();

                if (isManuelGiris)
                {
                    if (string.IsNullOrWhiteSpace(manuelDeger))
                    {
                        sonuc.KritikHataVar = false;
                        sonuc.UyariGerekli = true;
                        sonuc.Mesajlar.Add("MANUEL DEĞER GİRİLMELİ");
                        row.Tag = null;
                    }

                    else if (!dahaOnceOnaylandi)
                    {
                        sonuc.KritikHataVar = false;
                        sonuc.UyariGerekli = true;
                        sonuc.Mesajlar.Add("MANUEL GİRİŞ ONAYI BEKLENİYOR");
                        row.Tag = null;
                    }
                    else
                    {

                        sonuc.KritikHataVar = false;
                        sonuc.UyariGerekli = false;
                        row.Tag = "ONAYLANDI";
                    }
                }


                else 
                {

                    if (HedefBilgi == null)
                    {
                        HedefKolonlar.TryGetValue(hedefKolon, out HedefBilgi);
                    }

                    string aramaTablosuAdi = row.Cells["AramaTablo"].Value?.ToString();
                    string aramaDegerKolon = row.Cells["AramaDegerKolon"].Value?.ToString();
                    string aramaIdKolon = row.Cells["AramaIdKolon"].Value?.ToString();

                    bool aramaTanimliMi = !string.IsNullOrWhiteSpace(aramaTablosuAdi);

                    if (aramaTanimliMi)
                    {
                        if (string.IsNullOrWhiteSpace(aramaDegerKolon))
                        {
                            sonuc.KritikHataVar = true;
                            sonuc.Mesajlar.Add("Arama Tablosu seçildi, ancak Değer Kolonu zorunludur.");
                        }
                        if (string.IsNullOrWhiteSpace(aramaIdKolon))
                        {
                            sonuc.KritikHataVar = true;
                            sonuc.Mesajlar.Add("Arama Tablosu seçildi, ancak ID Kolonu zorunludur.");
                        }
                    }

                    string KaynakaramaTablosuAdi = row.Cells["KaynakAramaTablo"].Value?.ToString();
                    string KaynakaramaDegerKolon = row.Cells["KaynakAramaDegerKolon"].Value?.ToString();
                    string KaynakaramaIdKolon = row.Cells["KaynakAramaIdKolon"].Value?.ToString();

                    bool KaynakaramaTanimliMi = !string.IsNullOrWhiteSpace(KaynakaramaTablosuAdi);

                    if (KaynakaramaTanimliMi)
                    {
                        if (string.IsNullOrWhiteSpace(KaynakaramaDegerKolon))
                        {
                            sonuc.KritikHataVar = true;
                            sonuc.Mesajlar.Add("Kaynak Arama Tablosu seçildi, ancak Değer Kolonu zorunludur.");
                        }
                        if (string.IsNullOrWhiteSpace(KaynakaramaIdKolon))
                        {
                            sonuc.KritikHataVar = true;
                            sonuc.Mesajlar.Add("Kaynak Arama Tablosu seçildi, ancak ID Kolonu zorunludur.");
                        }
                    }

                    bool hedefDonusumGerekli = (bool)(row.Cells["DonusumGerekli"].Value ?? false);
                    bool kaynakDonusumGerekli = (bool)(row.Cells["KaynakDonusumGerekli"].Value ?? false);

                    if (!sonuc.KritikHataVar)
                    {
                        
                        bool hedefLookupAktif = hedefDonusumGerekli && !string.IsNullOrWhiteSpace(row.Cells["AramaTablo"].Value?.ToString());
                        bool kaynakLookupAktif = kaynakDonusumGerekli && !string.IsNullOrWhiteSpace(row.Cells["KaynakAramaTablo"].Value?.ToString());

                                                
                        sonuc = _eslestirmeService.KontrolEt(
                            KaynakBilgi,
                            HedefBilgi,
                            kaynakKolon,
                            hedefLookupAktif, 
                            kaynakLookupAktif 
                        );
                    }

                    if (dahaOnceOnaylandi && !sonuc.KritikHataVar)
                    {
                        row.Tag = "ONAYLANDI";
                    }
                    else
                    {

                        row.Tag = sonuc;
                    }


                    bool isNullHata = sonuc.KritikHataVar && sonuc.Mesajlar.Any(m => m.Contains("Hedef NULL kabul etmiyor"));

                    if (isNullHata)
                    {

                        if (HedefBilgi != null && !HedefBilgi.IsNullable && !string.IsNullOrWhiteSpace(manuelDeger))
                        {

                            sonuc.KritikHataVar = false;
                            sonuc.UyariGerekli = true;
                            sonuc.Mesajlar.RemoveAll(m => m.Contains("Hedef NULL kabul etmiyor"));
                            sonuc.Mesajlar.Add("Kaynaktaki NULL değerler için Manuel Değer ('" + manuelDeger + "') kullanılacaktır.");

                            if (dahaOnceOnaylandi)
                            {
                                row.Tag = "ONAYLANDI";
                            }
                            else
                            {
                                row.Tag = sonuc;
                            }
                        }
                    }

                }


                if (row.Tag != null && row.Tag.ToString() == "ONAYLANDI" && !sonuc.KritikHataVar)
                {
                    row.Cells["Uygunluk"].Value = "Uygun";
                    row.Cells["Uygunluk"].Style.ForeColor = Color.Blue;
                    return;
                }
                //sonuc.Mesajlar.RemoveAll(m => m.Contains("ONAY GEREKİYOR:"));

                if (sonuc.KritikHataVar && sonuc.Mesajlar.Contains("Uygun Değil"))
                {
                    lstLog.Items.Add("Ondalık -> Tam Sayı (Veri Kaybı Riski)");
                }

                if (sonuc.KritikHataVar)
                {
                    row.Cells["Uygunluk"].Value = string.Join(", ", sonuc.Mesajlar);
                    row.Cells["Uygunluk"].Style.ForeColor = Color.Red;
                    row.Tag = null;
                }

                else if (sonuc.UyariGerekli)
                {
                    if (isManuelGiris)
                    {
                        row.Cells["Uygunluk"].Value = string.Join(", ", sonuc.Mesajlar);
                    }
                    else
                    {                        
                        row.Cells["Uygunluk"].Value = "ONAY GEREKİYOR: " + string.Join(", ", sonuc.Mesajlar);
                    }

                    row.Cells["Uygunluk"].Style.ForeColor = Color.DarkOrange;                    
                }
                else
                {
                    row.Cells["Uygunluk"].Value = "Uygun";
                    row.Cells["Uygunluk"].Style.ForeColor = Color.Green;
                    row.Tag = "ONAYLANDI";
                }

            }

            catch (Exception ex)
            {

                row.Cells["Uygunluk"].Value = "Hata";
                lstLog.Items.Add("Kontrol Hatası: " + ex.Message);

            }
        }




        private List<EslestirmeBilgisi> EslestirmeListesi()
        {
            var liste = new List<EslestirmeBilgisi>();

            foreach (DataGridViewRow row in GrdEslestirme.Rows)
            {
                if (row.IsNewRow)
                    continue;

                string kaynak = row.Cells["KaynakKolon"].Value?.ToString();
                string hedef = row.Cells["HedefKolon"].Value?.ToString();

                
                if (string.IsNullOrWhiteSpace(kaynak) || string.IsNullOrWhiteSpace(hedef))
                {
                    continue;
                }

                bool onayliString = row.Tag?.ToString() == "ONAYLANDI";
                EslestirmeSonucu sonuc = row.Tag as EslestirmeSonucu;              

                bool uygunluk = onayliString || (sonuc != null && sonuc.EslestirmeUygun);

                if (!uygunluk)
                {
                    continue;
                }

                
                string manuelDeger = row.Cells["ManuelDeger"].Value?.ToString();

                if (sonuc == null)
                {
                    sonuc = new EslestirmeSonucu() { EslestirmeUygun = true };
                }

                liste.Add(new EslestirmeBilgisi
                {
                    KaynakKolon = kaynak,
                    HedefKolon = hedef,
                    ManuelDeger = manuelDeger,

                    Sonuc = sonuc
                });
            }
            return liste;
        }

        #region Geri
        private void BtnGeri_Click(object sender, EventArgs e)
        {
            _oncekiForm.Show();
            this.Close();
        }
        #endregion

        #region OtomatikEsle

        private void BtnOtomatikEsle_Click(object sender, EventArgs e) 
        {
            if (GrdEslestirme.Columns["HedefKolon"] is DataGridViewComboBoxColumn comboCol)
            {
                comboCol.Items.Clear();
                comboCol.Items.AddRange(HedefKolonlar.Keys.ToArray());
            }

            var hedefList = HedefKolonlar.Keys.ToList();


            foreach (DataGridViewRow row in GrdEslestirme.Rows)
            {
                var sourceName = row.Cells["KaynakKolon"].Value?.ToString();

                if (string.IsNullOrWhiteSpace(sourceName))
                    continue;


                var matchName = hedefList.FirstOrDefault(h => h.Equals(sourceName, StringComparison.OrdinalIgnoreCase));

                if (matchName != null)
                {

                    row.Cells["HedefKolon"].Value = matchName;

                    if (HedefKolonlar.TryGetValue(matchName, out var hInfo))
                    {
                        row.Cells["HedefTip"].Value = hInfo.DataType;
                        row.Cells["HedefUzunluk"].Value = hInfo.Length?.ToString() ?? "";
                        row.Cells["HedefNullable"].Value = hInfo.IsNullable ? "YES" : "NO";
                    }
                }                
                GridKontrolEt(row);
            }

            lstLog.Items.Add("Eşleme tamamlandı.");
        }
        #endregion

        private void BtnStrSil_Click(object sender, EventArgs e)
        {
            if (GrdEslestirme.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow r in GrdEslestirme.SelectedRows)
                    GrdEslestirme.Rows.Remove(r);
            }
            else if (GrdEslestirme.CurrentCell != null)
            {
                GrdEslestirme.Rows.RemoveAt(GrdEslestirme.CurrentCell.RowIndex);
            }
        }



        private async void BtnFiltreTest_Click(object sender, EventArgs e)
        {
            if (TrwKaynakTablolar.SelectedNode == null)
            {
                MessageBox.Show("Önce kaynak tablo seçin.");
                return;
            }

            string tablo = TrwKaynakTablolar.SelectedNode.Tag.ToString();

            if (RdoBtnTumSatır.Checked)
            {
                MessageBox.Show("Tüm satırlar seçili, filtre testi için gerekmiyor.");
                return;
            }

            string where = TxtFiltreleme.Text.Trim();
            if (string.IsNullOrWhiteSpace(where))
            {
                MessageBox.Show("Filtre boş.");
                return;
            }

            try
            {
                int satırSayısı = await KaynakRepo.SatirSayisiGetirAsync(tablo, where);

                MessageBox.Show($"Filtre testi başarılı: {satırSayısı} satır döndü.");
                lstLog.Items.Add($"Filtre testi başarılı: {satırSayısı} satır döndü. WHERE: {where}");
            }
            catch (Exception ex)
            {
                // Repository'den fırlatılan detaylı hatayı yakala
                MessageBox.Show($"Test hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lstLog.Items.Add($"Filtre hatası: {ex.Message}");
            }
        }




        private string ConnectionString(BaglantiBilgileri b) =>

            $"Server={b.Sunucu};Database={b.Veritabani};User Id={b.Kullanici};Password={b.Sifre};TrustServerCertificate=True;";





        private void ProgresGuncelle(int islenen, int toplam, int aktarılan, int atlanan)
        {
            if (prgTransfer.InvokeRequired)
            {
                prgTransfer.Invoke(new Action(() => { ProgresGuncelle(islenen, toplam, aktarılan, atlanan); }));//new Action<int, int, int, int>(ProgresGuncelle), islenen, toplam, aktarılan, atlanan
                return;
            }

            int progress = (int)((islenen / (double)toplam) * 100);
            prgTransfer.Value = Math.Min(progress, 100);

            lblTransferSayisi.Text = $"{progress}%  |  {aktarılan} eklendi  |  {atlanan} atlandı";
        }



        private void LogEkle(string mesaj)
        {
            if (lstLog.InvokeRequired)// uı threade böyle bir işim var lstloga ulaşmak izin istemek
            {
                lstLog.Invoke(new Action(() => { LogEkle(mesaj); }));
                return;
            }

            lstLog.Items.Add($"[{DateTime.Now:HH:mm:ss}] {mesaj}");
            lstLog.TopIndex = lstLog.Items.Count - 1;
        }


        private async void GrdEslestirme_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = GrdEslestirme.Rows[e.RowIndex];
            var currentColumn = GrdEslestirme.Columns[e.ColumnIndex];

            // 1. Durum: Kaynak veya Hedef Kolon Seçimi Değişimi
            if (currentColumn.Name == "KaynakKolon" || currentColumn.Name == "HedefKolon")
            {
                row.Tag = null;
                row.Cells["Uygunluk"].Value = "";
                row.Cells["Uygunluk"].Style.ForeColor = Color.Empty;
                row.Cells["ManuelDeger"].Value = DBNull.Value;
                GridKontrolEt(row);
            }
            // 2. Durum: Manuel Değer Değişimi
            else if (currentColumn.Name == "ManuelDeger")
            {
                row.Tag = null;
                GridKontrolEt(row);
            }
            // 3. Durum: Hedef Arama Tablosu Değişimi
            else if (currentColumn.Name == "AramaTablo")
            {
                row.Tag = null;
                row.Cells["Uygunluk"].Value = "";
                row.Cells["Uygunluk"].Style.ForeColor = Color.Empty;
                GridKontrolEt(row);
            }
            // 4. Durum: Kaynak Arama Tablosu Değişimi
            else if (currentColumn.Name == "KaynakAramaTablo")
            {
                row.Tag = null;
                row.Cells["Uygunluk"].Value = "";
                row.Cells["Uygunluk"].Style.ForeColor = Color.Empty;
                GridKontrolEt(row);
            }
            // 💡 5. DURUM: Kaynak Dönüşüm Gerekli Checkbox Değişimi (Tablo Yükleme Tetikleyicisi) 
            else if (currentColumn.Name == "KaynakDonusumGerekli")
            {
                bool donusumGerekli = (bool)(row.Cells["KaynakDonusumGerekli"].Value ?? false);

                if (donusumGerekli)
                {
                    try
                    {
                        // Tablo Adlarını Kaynak Repo'dan çek
                        List<string> tabloAdlari = await KaynakRepo.TabloAdlariniGetirAsync();

                        // Hücreye Yükle
                        if (row.Cells["KaynakAramaTablo"] is DataGridViewComboBoxCell tabloCell)
                        {
                            tabloCell.Items.Clear();
                            tabloCell.Items.AddRange(tabloAdlari.ToArray());
                        }

                        row.Cells["KaynakAramaDegerKolon"].Value = null;
                        row.Cells["KaynakAramaIdKolon"].Value = null;
                       
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Kaynak Tablo Adları yüklenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    
                    row.Cells["KaynakAramaTablo"].Value = null;
                    row.Cells["KaynakAramaDegerKolon"].Value = null;
                    row.Cells["KaynakAramaIdKolon"].Value = null;
                }
                
                GridKontrolEt(row);
            }
           
            else if (currentColumn.Name == "DonusumGerekli")
            {
                
                GridKontrolEt(row);
            }
        }

        private async void FrmVeriEslestirme_Load(object sender, EventArgs e)
        {
            lstLog.Items.Add("Tablolar yükleniyor");

            // Asenkron işi başlat, ancak sonucu bekleme (Load olayının tamamlanması için)
            LoadAsyncData();

            RdoBtnTumSatır.Checked = true;
        }
        private async void LoadAsyncData() // Burada async void kalabilir, ancak try-catch zorunludur.
        {
            try
            {
                await TablolarıAgacaYukleAsync();
                lstLog.Items.Add("Tablolar yüklendi");

                // 💡 EK GÜVENLİK: Tablolar yüklendikten sonra Grid'in UI'ını hemen yenile
                GrdEslestirme.Invalidate(true);
            }
            catch (Exception ex)
            {
                // ASYNC VOID İÇİN KRİTİK: Hataları mutlaka yakalayın
                MessageBox.Show($"Veri yükleme sırasında kritik hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnTransferBaslat_Click(object sender, EventArgs e)
        {


            if (TrwKaynakTablolar.SelectedNode == null || TrwHedefTablolar.SelectedNode == null)
            {
                MessageBox.Show("Kaynak ve hedef tablo seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var eslesmeler = EslestirmeListesi(); // Kolon eşleşmelerini getirir

            if (eslesmeler.Count == 0)
            {
                MessageBox.Show("Uygun kolon eşleşmesi bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var benzersizKolonlar = BenzersizKolonlariGetir();
            if (!benzersizKolonlar.Any())
            {

                DialogResult result = MessageBox.Show(
                    "Mükerrer (Benzersiz) kayıt kontrolü için herhangi bir alan seçilmedi. İptal edip benzersiz alan seçmek için 'Evet', kontrolsüz devam etmek için 'Hayır' tuşuna basın.",
                    "Mükerrer Kontrol Uyarısı",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);


                if (result == DialogResult.Yes)
                {
                    LogEkle("Kullanıcı benzersiz alan seçmek için işlemi iptal etti.");
                    return;
                }

            }
            BtnTransferBaslat.Enabled = false;
            prgTransfer.Value = 0;
            prgTransfer.Style = ProgressBarStyle.Continuous;
            LogEkle("Transfer işlemi başlatılıyor...");

            try
            {

                string kaynakTablo = TrwKaynakTablolar.SelectedNode.Tag.ToString();
                string hedefTablo = TrwHedefTablolar.SelectedNode.Tag.ToString();

                var kolonlar = eslesmeler.Select(x => x.KaynakKolon).ToList();

                DataTable kaynakVeri = await Task.Run(() =>
                {

                    return KaynakRepo.VeriGetir(kaynakTablo, eslesmeler, TxtFiltreleme.Text);
                });


                if (kaynakVeri.Rows.Count == 0)
                {
                    LogEkle("Kaynakta aktarılacak veri bulunamadı.");
                    MessageBox.Show("Kaynakta veri bulunamadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }


                using (var onizlemeForm = new FrmVeriOnizleme(kaynakVeri))
                {
                    onizlemeForm.ShowDialog();
                    if (!onizlemeForm.Onaylandi)
                    {
                        LogEkle("Kullanıcı aktarımı iptal etti.");
                        return;
                    }
                }


                await TransferSatiriKontrolu(kaynak, hedef, kaynakTablo, hedefTablo, eslesmeler, kaynakVeri);
            }
            catch (Exception ex)
            {
                LogEkle($"Transfer hatası: {ex.Message}");
                MessageBox.Show($"Transfer hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                BtnTransferBaslat.Enabled = true;
                prgTransfer.Value = 100;
                await Task.Delay(300);
                prgTransfer.Value = 0;
            }
        }


        private bool SayiKontrolu(string dataType)
        {
            string[] numericTypes = { "int", "bigint", "smallint", "tinyint", "decimal", "numeric", "float", "real" };
            return numericTypes.Contains(dataType.ToLower());
        }

        private bool TamSayiliTip(string dataType)
        {
            string[] intTypes = { "int", "bigint", "smallint", "tinyint" };
            return intTypes.Contains(dataType.ToLower());
        }

        private bool OndalikliTip(string dataType)
        {
            string[] decimalTypes = { "decimal", "numeric", "float", "real" };
            return decimalTypes.Contains(dataType.ToLower());
        }

        private bool MetinselTip(string dataType)
        {
            string[] textTypes = { "nvarchar", "varchar", "nchar", "char" };
            return textTypes.Contains(dataType.ToLower());
        }


        #region TransferKontrolu
        private async Task TransferSatiriKontrolu(BaglantiBilgileri kaynak, BaglantiBilgileri hedef, string kaynakTablo, string hedefTablo,
            List<EslestirmeBilgisi> eslesmeler,
            DataTable kaynakVeri)
        {
            string hedefConnStr = ConnectionString(hedef);

            var benzersizKolon = new List<string>();

            foreach (DataGridViewRow row in GrdEslestirme.Rows)
            {

                if (row.Tag?.ToString() != "ONAYLANDI")
                    continue;

                string hedefKolon = row.Cells["HedefKolon"].Value?.ToString();

                if (string.IsNullOrWhiteSpace(hedefKolon))
                    continue;

                bool benzersizAlan = false;

                if (row.Cells["IsUnique"] is DataGridViewCheckBoxCell checkCell)
                {
                    benzersizAlan = (bool)(checkCell.Value ?? false); 
                }

                bool uniqueTanimlama = HedefKolonlar.ContainsKey(hedefKolon) && HedefKolonlar[hedefKolon].IsUnique; //hedef kolonun unıque tanımlaması var mı

                if (benzersizAlan || uniqueTanimlama)
                {
                    benzersizKolon.Add(hedefKolon);
                }
            }


            benzersizKolon = benzersizKolon.Distinct().ToList();

            bool unıqueKontrolu = benzersizKolon.Any();

            

            using var conn = new SqlConnection(hedefConnStr);
            await conn.OpenAsync();
            using var transaction = conn.BeginTransaction();

            int toplam = kaynakVeri.Rows.Count;
            int aktarılan = 0;
            int atlanan = 0;
            int islenen = 0;

            LogEkle($"Mükerrer Kontrol Kolonları : {string.Join(", ", benzersizKolon)} (Sayısı: {benzersizKolon.Count})");

            foreach (DataRow row in kaynakVeri.Rows)
            {
                try
                {
                    bool satirUyumlu = true;

                    var hedefDegerEkle = new Dictionary<string, object>();

                    var eslestirmeler = EslestirmeListesi();

                    foreach (var eslestirme in eslestirmeler)
                    {
                        var kaynakKolon = eslestirme.KaynakKolon;
                        var hedefKolon = eslestirme.HedefKolon;

                        if (kaynakKolon == "(MANUEL GİRİŞ)")
                        {
                            
                            var hedefBilgi = HedefKolonlar[hedefKolon];
                            string manuelDeger = eslestirme.ManuelDeger;

                            if (string.IsNullOrWhiteSpace(manuelDeger))
                            {

                                if (!hedefBilgi.IsNullable)
                                {
                                    LogEkle($"Satır Atlandı: '{hedefKolon}' kolonu (Manuel Giriş) NULL olamaz ve değer boş.");
                                    satirUyumlu = false;
                                    break;
                                }
                                hedefDegerEkle[hedefKolon] = DBNull.Value;
                            }
                            else
                            {

                                hedefDegerEkle[hedefKolon] = manuelDeger;
                            }
                            continue;
                        }


                        var KaynakBilgi = KaynakKolonlar[kaynakKolon];
                        var HedefBilgi = HedefKolonlar[hedefKolon];

                        object val = row[hedefKolon];


                        if ((val == null || val == DBNull.Value))
                        {
                            
                            if (!HedefBilgi.IsNullable && !string.IsNullOrWhiteSpace(eslestirme.ManuelDeger))
                            {
                                
                                val = eslestirme.ManuelDeger;
                                LogEkle($"Bilgi: '{hedefKolon}' kolonu (Kaynak: {kaynakKolon}) NULL geldi, Hedef NULL kabul etmediği için manuel değer ('{eslestirme.ManuelDeger}') atandı.");
                            }
                            
                            else if (!HedefBilgi.IsNullable)
                            {
                               
                                LogEkle($"Satır Atlandı: '{hedefKolon}' kolonu NULL olamaz ve manuel varsayılan değer tanımlanmamış.");
                                satirUyumlu = false;
                                break;
                            }
                            
                            else
                            {
                               
                                hedefDegerEkle[hedefKolon] = DBNull.Value;
                                continue;
                            }
                        }


                        if (MetinselTip(KaynakBilgi.DataType) && MetinselTip(HedefBilgi.DataType))
                        {
                            string sVal = val.ToString();

                            if (HedefBilgi.Length.HasValue && HedefBilgi.Length.Value != -1 && sVal.Length > HedefBilgi.Length.Value)
                            {
                                sVal = sVal.Substring(0, HedefBilgi.Length.Value);
                            }
                            hedefDegerEkle[hedefKolon] = sVal; 
                        }

                        else if (SayiKontrolu(KaynakBilgi.DataType) && SayiKontrolu(HedefBilgi.DataType))
                        {
                            if (OndalikliTip(KaynakBilgi.DataType) && TamSayiliTip(HedefBilgi.DataType))
                            {
                                decimal dVal = Convert.ToDecimal(val);
                                hedefDegerEkle[hedefKolon] = (long)Math.Truncate(dVal);
                            }
                            else
                            {
                                hedefDegerEkle[hedefKolon] = val;
                            }
                        }

                        else
                        {
                            hedefDegerEkle[hedefKolon] = val;
                        }
                    }

                    if (!satirUyumlu)
                    {
                        atlanan++;
                        islenen++;
                        ProgresGuncelle(islenen, toplam, aktarılan, atlanan);
                        continue;
                    }


                    if (unıqueKontrolu)
                    {
                        var kosulKontrolu = new List<string>();
                        var parametreKontrolu = new Dictionary<string, object>();

                        foreach (var hedefKolon in benzersizKolon)
                        {
                            if (hedefDegerEkle.TryGetValue(hedefKolon, out object nihaiDeger))
                            {
                                if (nihaiDeger == DBNull.Value)
                                {
                                    kosulKontrolu.Add($"[{hedefKolon}] IS NULL");
                                }
                                else
                                {
                                    string parametreAdi = $"@{hedefKolon}_Check";
                                    object sonKontrolDeger = nihaiDeger;


                                    if (nihaiDeger is string s)
                                    {
                                        sonKontrolDeger = s.Trim();
                                    }

                                    kosulKontrolu.Add($"[{hedefKolon}] = {parametreAdi}");
                                    parametreKontrolu.Add(parametreAdi, sonKontrolDeger);
                                }
                            }
                        }

                        if (kosulKontrolu.Any())
                        {
                            string kosul = string.Join(" AND ", kosulKontrolu);
                            string kontrolSql = $"SELECT COUNT(1) FROM [{hedefTablo}] WHERE {kosul}";

                            using (var cmd = new SqlCommand(kontrolSql, conn, transaction))
                            {
                                foreach (var kvp in parametreKontrolu)
                                {
                                    cmd.CommandTimeout = 300;
                                    cmd.Parameters.AddWithValue(kvp.Key, kvp.Value);
                                }

                                int count = (int)await cmd.ExecuteScalarAsync();

                                if (count > 0)
                                {
                                    atlanan++;
                                    islenen++;
                                    LogEkle($"Satır Atlandı: Benzersiz kaydı mevcut. Anahtar: {string.Join(", ", benzersizKolon)}");
                                    ProgresGuncelle(islenen, toplam, aktarılan, atlanan);
                                    continue;
                                }
                            }
                        }
                    }

                    
                    string cols = string.Join(",", hedefDegerEkle.Keys.Select(k => $"[{k}]"));
                    string parametreAdlari = string.Join(",", hedefDegerEkle.Keys.Select(k => $"@{k}"));
                    string insertSql = $"INSERT INTO [{hedefTablo}] ({cols}) VALUES ({parametreAdlari})";

                    using (var cmdInsert = new SqlCommand(insertSql, conn, transaction))
                    {
                        cmdInsert.CommandTimeout = 300;
                        foreach (var kvp in hedefDegerEkle)
                        {
                            cmdInsert.Parameters.AddWithValue($"@{kvp.Key}", kvp.Value ?? DBNull.Value);
                        }
                        await cmdInsert.ExecuteNonQueryAsync();
                    }

                    aktarılan++;
                    islenen++;

                    if (islenen % 1 == 0) ProgresGuncelle(islenen, toplam, aktarılan, atlanan);

                }
                catch (Exception ex)
                {
                    islenen++;
                    atlanan++;
                    LogEkle($"Satır Hatası: {ex.Message}");
                    ProgresGuncelle(islenen, toplam, aktarılan, atlanan);
                }
            }

            try
            {
                transaction.Commit();
                LogEkle("Tüm transfer işlemleri başarıyla Commit edildi.");
            }
            catch (Exception ex)
            {

                try
                {
                    transaction.Rollback();
                    LogEkle($"KRİTİK HATA: Commit başarısız oldu. İşlem geri alındı (Rollback). Hata: {ex.Message}");
                }
                catch (Exception rollbackEx)
                {
                    LogEkle($"KRİTİK HATA: Rollback başarısız oldu. Veritabanı tutarsız olabilir. Hata: {rollbackEx.Message}");
                }

                MessageBox.Show($"Aktarım Commit edilirken KRİTİK HATA oluştu ve işlem geri alındı/başarısız oldu.\n{ex.Message}", "Kritik Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);

                aktarılan = 0;
                atlanan = toplam;
            }


            ProgresGuncelle(toplam, toplam, aktarılan, atlanan);
            LogEkle($"Transfer Bitti. Aktarılan: {aktarılan}, Atlanan: {atlanan}");
            MessageBox.Show($"İşlem Tamamlandı.\nAktarılan: {aktarılan}\nAtlanan: {atlanan}", "Sonuç", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion



        #region BenzersizKontroluEklemeCikarma
        private List<string> BenzersizKolonlariGetir()
        {
            var benzersizKolon = new List<string>();

            foreach (DataGridViewRow row in GrdEslestirme.Rows)
            {
                if (row.Tag?.ToString() != "ONAYLANDI")
                    continue;
                string hedefKolon = row.Cells["HedefKolon"].Value?.ToString();
                if (string.IsNullOrWhiteSpace(hedefKolon)) continue;

                bool benzersizAlan = (row.Cells["IsUnique"] as DataGridViewCheckBoxCell)?.Value as bool? ?? false;
                bool uniqueTanimlama = HedefKolonlar.ContainsKey(hedefKolon) && HedefKolonlar[hedefKolon].IsUnique;

                if (benzersizAlan || uniqueTanimlama)
                {
                    benzersizKolon.Add(hedefKolon);
                }
            }

            return benzersizKolon.Distinct().ToList();
        }
        #endregion

        private void GrdEslestirme_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is ComboBox combo)
            {
                combo.SelectedIndexChanged -= HedefKolonSecildi;
                combo.SelectedIndexChanged -= AramaTablosu_SelectedIndexChanged;
                combo.SelectedIndexChanged -= KaynakAramaTablosu_SelectedIndexChanged;

                var currentColumnName = GrdEslestirme.CurrentCell.OwningColumn.Name;

                // 1. Hedef Kolon Seçimi Kontrolü
                if (currentColumnName == "HedefKolon")
                {
                    combo.SelectedIndexChanged += HedefKolonSecildi;
                }
                else if (currentColumnName == "AramaTablo")
                {
                    combo.SelectedIndexChanged += AramaTablosu_SelectedIndexChanged;
                }
                else if (currentColumnName == "KaynakAramaTablo")
                {
                    combo.SelectedIndexChanged += KaynakAramaTablosu_SelectedIndexChanged;

                }
            }
        }

        private async void AramaTablosu_SelectedIndexChanged(object sender, EventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox == null) return;


            var currentRow = GrdEslestirme.CurrentRow;
            string secilenTabloAdi = comboBox.SelectedItem.ToString();


            List<string> kolonAdlari = await HedefRepo.KolonAdlariniGetirAsync(secilenTabloAdi);


            if (currentRow.Cells["AramaDegerKolon"] is DataGridViewComboBoxCell degerKolonCell)
            {
                degerKolonCell.Items.Clear();
                degerKolonCell.Items.AddRange(kolonAdlari.ToArray());
            }

            if (currentRow.Cells["AramaIdKolon"] is DataGridViewComboBoxCell idKolonCell)
            {
                idKolonCell.Items.Clear();
                idKolonCell.Items.AddRange(kolonAdlari.ToArray());
            }
            currentRow.Cells["AramaTablo"].Value = secilenTabloAdi;


            lstLog.Items.Add($"{secilenTabloAdi} tablosunun kolonları Arama Kolonlarına yüklendi.");
        }


        private async void KaynakAramaTablosu_SelectedIndexChanged(object sender, EventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox == null) return;


            var currentRow = GrdEslestirme.CurrentRow;
            string secilenTabloAdi = comboBox.SelectedItem.ToString();


            List<string> kolonAdlari = await KaynakRepo.KolonAdlariniGetirAsync(secilenTabloAdi);


            if (currentRow.Cells["KaynakAramaDegerKolon"] is DataGridViewComboBoxCell degerKolonCell)
            {
                degerKolonCell.Items.Clear();
                degerKolonCell.Items.AddRange(kolonAdlari.ToArray());


            }


            if (currentRow.Cells["KaynakAramaIdKolon"] is DataGridViewComboBoxCell idKolonCell)
            {
                idKolonCell.Items.Clear();
                idKolonCell.Items.AddRange(kolonAdlari.ToArray());

            }
            currentRow.Cells["KaynakAramaTablo"].Value = secilenTabloAdi;


            lstLog.Items.Add($"{secilenTabloAdi} tablosunun kolonları Kaynak Arama Kolonlarına yüklendi.");
        }

        private void GrdEslestirme_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var row = GrdEslestirme.Rows[e.RowIndex];
            string durum = row.Cells["Uygunluk"].Value?.ToString();
            string kaynakKolon = row.Cells["KaynakKolon"].Value?.ToString();
            string hedefKolon = row.Cells["HedefKolon"].Value?.ToString();

            
            if (e.ColumnIndex == GrdEslestirme.Columns["IsUnique"].Index)
            {
                var cell = row.Cells["IsUnique"] as DataGridViewCheckBoxCell;
                if (cell != null)
                {
                    bool currentValue = (bool)(cell.Value ?? false);
                    cell.Value = !currentValue;
                    GrdEslestirme.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    GridKontrolEt(row);
                }
                return;  
            }


            if (row.Tag?.ToString() == "ONAYLANDI")
            {
                return;
            }


            if (string.IsNullOrEmpty(durum) || durum == "Uygun")
                return;


            if (kaynakKolon == "(MANUEL GİRİŞ)" && durum?.Contains("ONAYI BEKLENİYOR") == true)
            {
                string manuelDeger = row.Cells["ManuelDeger"].Value?.ToString();

                if (string.IsNullOrWhiteSpace(manuelDeger))
                {
                    MessageBox.Show($"'{hedefKolon}' kolonu için **Manuel Değer** girilmeden onaylayamazsınız.",
                                      "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }


                DialogResult result = MessageBox.Show(
                    $"'{hedefKolon}' hedef kolonu için sabit değer olarak **'{manuelDeger}'** atanmıştır.\n\n" +
                    "Bu eşleşmeyi transfer için onaylıyor musunuz?",
                    "Manuel Giriş Onayı",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    row.Tag = "ONAYLANDI";
                    GridKontrolEt(row);
                }
                return;
            }


            if (row.Cells["Uygunluk"].Style.ForeColor == Color.Red)
            {
                MessageBox.Show("Bu hata kritiktir ve onaylanarak geçilemez. Lütfen kolon eşleşmesini değiştirin.",
                                  "Kritik Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            if (durum.StartsWith("ONAY GEREKİYOR"))
            {
                string uyariMesaji = durum.Replace("ONAY GEREKİYOR: ", "");


                if (uyariMesaji.Contains("Dönüşüm Gerekli") || uyariMesaji.Contains("Format Dönüşümü Gerekli"))
                {
                    DonusumEkraniAc(e.RowIndex);
                    return;
                }

                DialogResult result = MessageBox.Show(
                    $"Bu eşleşmede şu uyarılar var:\n\n{uyariMesaji}\n\n" +
                    "Aktarım sırasında veri kaybı veya kırpılma olabilir. Bunu kabul edip onaylıyor musunuz?",
                    "Onay İsteği",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    row.Tag = "ONAYLANDI";
                    GridKontrolEt(row);
                }
            }
        }


        #region KaynakSutunEkle
        private async void BtnKynkSutunYkle_Click(object sender, EventArgs e)
        {
            try
            {
                if (TrwKaynakTablolar.SelectedNode?.Tag is string kaynakTablo && !string.IsNullOrWhiteSpace(kaynakTablo))
                {
                    lstLog.Items.Add($"Kaynak kolonlar yükleniyor: {kaynakTablo}...");
                    lblkaynak.Text = kaynakTablo;
                    KaynakKolonlar = await KaynakRepo.KolonBilgileriniGetirAsync(kaynakTablo);
                    KaynakSutunBilgileriGetir(KaynakKolonlar);
                    lstLog.Items.Add($"Kaynak kolonlar yüklendi ({KaynakKolonlar.Count} adet).");
                }
                else
                {
                    lstLog.Items.Add("Kaynak tablo seçilmedi.");
                }
            }
            catch (Exception ex)
            {
                lstLog.Items.Add($"Kaynak kolon yükleme hatası: {ex.Message}");

            }
        }
        #endregion


        #region HedefIsNullable
        private void HedefKolonDetaylariniGrideDoldur()
        {

            var detayListesi = HedefKolonlar
                .OrderBy(kvp => kvp.Key)
                .Select(kvp => new
                {
                    KolonAdi = kvp.Key,
                    NullOzelik = kvp.Value.IsNullable ? "YES" : "NO"
                })
                .ToList();


            GrdHedefNullable.DataSource = null;
            GrdHedefNullable.DataSource = detayListesi;

            GrdHedefNullable.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
        }
        #endregion


        #region GrdHedefNullable_CellDoubleClick
        private void GrdHedefNullable_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex < 0 || GrdHedefNullable.Columns.Count == 0)
                return;

            var hedefKolonRow = GrdHedefNullable.Rows[e.RowIndex];


            string hedefKolonAdi = hedefKolonRow.Cells["KolonAdi"].Value?.ToString();

            if (string.IsNullOrWhiteSpace(hedefKolonAdi))
                return;


            if (GrdEslestirme.Rows.Cast<DataGridViewRow>()
                .Any(r => r.Cells["HedefKolon"].Value?.ToString() == hedefKolonAdi))
            {
                MessageBox.Show($"'{hedefKolonAdi}' hedef kolonu zaten eşleştirilmiş.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            int newRowIndex = GrdEslestirme.Rows.Add();
            var newRow = GrdEslestirme.Rows[newRowIndex];

            newRow.Cells["KaynakKolon"].Value = "(MANUEL GİRİŞ)";
            newRow.Cells["KaynakTip"].Value = "manueltip";
            newRow.Cells["KaynakUzunluk"].Value = "";
            newRow.Cells["KaynakNullable"].Value = "YES";



            newRow.Cells["HedefKolon"].Value = hedefKolonAdi;

            GrdEslestirme.CommitEdit(DataGridViewDataErrorContexts.Commit);

            if (HedefKolonlar.TryGetValue(hedefKolonAdi, out var hedefBilgi))
            {
                newRow.Cells["HedefTip"].Value = hedefBilgi.DataType;
                newRow.Cells["HedefUzunluk"].Value = hedefBilgi.Length?.ToString() ?? "";
                newRow.Cells["HedefNullable"].Value = hedefBilgi.IsNullable ? "YES" : "NO";
                newRow.Cells["IsUnique"].Value = hedefBilgi.IsUnique;
            }

            newRow.Cells["ManuelDeger"].Style.BackColor = Color.LightYellow;

            GridKontrolEt(newRow);


            GrdEslestirme.CurrentCell = newRow.Cells["ManuelDeger"];
            GrdEslestirme.BeginEdit(true);
            GrdHedefNullable.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
        }
        #endregion



        #region GrdEslestirme_DataError
        private void GrdEslestirme_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            
            if (e.Exception is ArgumentException && e.Exception.Message.Contains("DataGridViewComboBoxCell"))
            {

                e.ThrowException = false;
                e.Cancel = true;

            }
        }
        #endregion



        private void GrdHedefNullable_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (GrdHedefNullable.Columns[e.ColumnIndex].Name == "NullOzelik")
            {

                var detayListesi = HedefKolonlar.Select(kvp => new
                {
                    KolonAdi = kvp.Key,
                    NullOzelik = kvp.Value.IsNullable ? "YES" : "NO"
                });


                if (_siralamaYonDurumu)
                {
                    detayListesi = detayListesi.OrderBy(d => d.NullOzelik);
                }
                else
                {
                    detayListesi = detayListesi.OrderByDescending(d => d.NullOzelik);
                }

                _siralamaYonDurumu = !_siralamaYonDurumu;

                GrdHedefNullable.DataSource = detayListesi.ToList();
                GrdHedefNullable.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            }
        }


        private async void BtnHdfSutunYkle_Click(object sender, EventArgs e)
        {
            try
            {
                if (TrwHedefTablolar.SelectedNode?.Tag is string hedefTablo && !string.IsNullOrWhiteSpace(hedefTablo))
                {
                    lstLog.Items.Add($"Hedef kolonlar yükleniyor: {hedefTablo}...");
                    lblHedef.Text = hedefTablo;


                    HedefKolonlar = await HedefRepo.KolonBilgileriniGetirAsync(hedefTablo);

                    HedefKolonDetaylariniGrideDoldur();

                    if (GrdEslestirme.Columns["HedefKolon"] is DataGridViewComboBoxColumn comboCol)
                    {

                        comboCol.Items.Clear();
                        comboCol.Items.AddRange(HedefKolonlar.Keys.ToArray());
                    }

                    var hedefTablolar = await HedefRepo.TabloGetirAsync();

                    if (GrdEslestirme.Columns["AramaTablo"] is DataGridViewComboBoxColumn aramaTabloComboCol)
                    {
                        aramaTabloComboCol.Items.Clear();

                        if (hedefTablolar.Any())
                        {
                            aramaTabloComboCol.Items.AddRange(hedefTablolar.ToArray());
                            lstLog.Items.Add($"Arama tabloları yüklendi ({hedefTablolar.Count} adet).");
                        }
                    }


                    HedefGuncelle(HedefKolonlar.Keys.ToList());
                    lstLog.Items.Add($"Hedef kolonlar yüklendi ({HedefKolonlar.Count} adet).");
                }
                else
                {
                    lstLog.Items.Add("Hedef tablo seçilmedi.");
                }
            }
            catch (Exception ex)
            {
                lstLog.Items.Add($"Hedef kolon yükleme hatası: {ex.Message}");
            }
        }




        private void DonusumEkraniAc(int rowIndex)
        {
            var row = GrdEslestirme.Rows[rowIndex];

            if (row.Tag is not EslestirmeSonucu)
            {
                GridKontrolEt(row);
            }

            if (row.Tag is not EslestirmeSonucu sonuc)
            {
                MessageBox.Show("Önce eşleştirme kontrolünü yapın (GridKontrolEt çalıştırılmalı).", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            string kaynakTabloAdi = TrwKaynakTablolar.SelectedNode?.Tag as string;
            string hedefTabloAdi = TrwHedefTablolar.SelectedNode?.Tag as string;

            string kaynakKolonAdi = row.Cells["KaynakKolon"].Value?.ToString();
            string hedefKolonAdi = row.Cells["HedefKolon"].Value?.ToString();

            string aramaTablo = row.Cells["AramaTablo"].Value?.ToString() ?? string.Empty;
            string aramaDegerKolon = row.Cells["AramaDegerKolon"].Value?.ToString() ?? string.Empty;
            string aramaIdKolon = row.Cells["AramaIdKolon"].Value?.ToString() ?? string.Empty;

            bool donusumGerekli = (bool)(row.Cells["DonusumGerekli"].Value ?? false);

            
            if (string.IsNullOrWhiteSpace(kaynakTabloAdi) || string.IsNullOrWhiteSpace(hedefTabloAdi))
            {
                MessageBox.Show("Kaynak veya hedef tablo seçmelisiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!donusumGerekli)
            {
                MessageBox.Show("Bu kolon için Dönüşüm Gerekli kutucuğunu işaretlemelisiniz.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (string.IsNullOrWhiteSpace(hedefKolonAdi))
            {
                MessageBox.Show("Önce bir Hedef Kolon seçmelisiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            KolonBilgisi kaynakKolonBilgisi = null;
            KolonBilgisi hedefKolonBilgisi = null;

            if (!string.IsNullOrWhiteSpace(kaynakKolonAdi) && KaynakKolonlar.ContainsKey(kaynakKolonAdi))
            {
                kaynakKolonBilgisi = KaynakKolonlar[kaynakKolonAdi];
            }
            if (!string.IsNullOrWhiteSpace(hedefKolonAdi) && HedefKolonlar.ContainsKey(hedefKolonAdi))
            {
                hedefKolonBilgisi = HedefKolonlar[hedefKolonAdi];
            }

            if (kaynakKolonAdi != "(MANUEL GİRİŞ)" && (kaynakKolonBilgisi == null || hedefKolonBilgisi == null))
            {
                MessageBox.Show("Kolon bilgisi (tip, uzunluk vb.) bulunamadı. Lütfen kolonları yeniden yükleyin.", "Kritik Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(aramaTablo) ||
                 string.IsNullOrWhiteSpace(aramaDegerKolon) ||
                 string.IsNullOrWhiteSpace(aramaIdKolon))
            {
                MessageBox.Show("Dönüşüm işlemi için 'Arama Tablosu', 'Arama Değer Kolonu' ve 'Arama ID Kolonu' alanlarının Grid'de seçilmiş olması zorunludur. Lütfen doldurunuz.", "Eşleştirme Eksik", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            try
            {
                var donusumForm = new DonusumEkrani(
                    kaynakKolonAdi: kaynakKolonAdi,
                    kaynakTabloAdi: kaynakTabloAdi,
                    hedefKolonAdi: hedefKolonAdi,
                    kaynakKolonBilgisi: kaynakKolonBilgisi,
                    hedefKolonBilgisi: hedefKolonBilgisi,

                    aramaTablo: aramaTablo,
                    aramaDegerKolon: aramaDegerKolon,
                    aramaIdKolon: aramaIdKolon,


                    kaynakBaglanti: kaynak,
                    hedefBaglanti: hedef,
                    donusumTipi: sonuc.DonusumTipi
                );

                if (donusumForm.ShowDialog() == DialogResult.OK)
                {
                    Dictionary<string, object> yeniSozluk = donusumForm.DonusumSozlugu;
                    sonuc.DonusumSozlugu = yeniSozluk;
                    sonuc.DonusumTipi = DonusumTuru.LookupEslestirme;
                    sonuc.EslestirmeUygun = true;

                    row.Cells["DonusumGerekli"].Value = true;
                    row.Cells["Uygunluk"].Value = "Uygun (Dönüşümlü)";
                    row.Cells["Uygunluk"].Style.ForeColor = Color.DarkGreen;
                    row.Tag = sonuc;
                    MessageBox.Show("Dönüşüm ayarları kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Dönüşüm ekranı açılırken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void GrdEslestirme_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == GrdEslestirme.Columns["DonusumIslem"].Index && e.RowIndex >= 0)
            {
                GrdEslestirme.EndEdit();
                GrdEslestirme.CommitEdit(DataGridViewDataErrorContexts.Commit);


                if (GrdEslestirme.InvokeRequired)
                {
                    GrdEslestirme.Invoke(new Action(() => DonusumEkraniAc(e.RowIndex)));
                }
                else
                {
                    DonusumEkraniAc(e.RowIndex);
                }
            }
            else if (e.ColumnIndex == GrdEslestirme.Columns["KaynakDonusumIslem"].Index && e.RowIndex >= 0)
            {
                GrdEslestirme.EndEdit();
                GrdEslestirme.CommitEdit(DataGridViewDataErrorContexts.Commit);
                if (GrdEslestirme.InvokeRequired)
                {
                    GrdEslestirme.Invoke(new Action(() => KaynakDonusumEkraniAc(e.RowIndex)));
                }
                else
                {
                    KaynakDonusumEkraniAc(e.RowIndex);
                }
            }
        }



        private void AyarlariGuncelle(int rowIndex, bool enable)
        {
            var row = GrdEslestirme.Rows[rowIndex];

            row.Cells["AramaTablo"].ReadOnly = !enable;
            row.Cells["AramaDegerKolon"].ReadOnly = !enable;
            row.Cells["AramaIdKolon"].ReadOnly = !enable;

            Color arkaPlanRengi = enable ? Color.White : Color.LightGray;

            row.Cells["AramaTablo"].Style.BackColor = arkaPlanRengi;
            row.Cells["AramaDegerKolon"].Style.BackColor = arkaPlanRengi;
            row.Cells["AramaIdKolon"].Style.BackColor = arkaPlanRengi;

            if (enable)
            {
                row.Cells["DonusumIslem"].Style.BackColor = Color.LightSteelBlue;
            }
            else
            {
                row.Cells["DonusumIslem"].Style.BackColor = Color.LightGray;
            }
        }

        private void KaynakAyarlariGuncelle(int rowIndex, bool enable)
        {
            var row = GrdEslestirme.Rows[rowIndex];

            row.Cells["KaynakAramaTablo"].ReadOnly = !enable;
            row.Cells["KaynakAramaDegerKolon"].ReadOnly = !enable;
            row.Cells["KaynakAramaIdKolon"].ReadOnly = !enable;

            Color arkaPlanRengi = enable ? Color.White : Color.LightGray;

            row.Cells["KaynakAramaTablo"].Style.BackColor = arkaPlanRengi;
            row.Cells["KaynakAramaDegerKolon"].Style.BackColor = arkaPlanRengi;
            row.Cells["KaynakAramaIdKolon"].Style.BackColor = arkaPlanRengi;

            if (enable)
            {
                row.Cells["KaynakDonusumIslem"].Style.BackColor = Color.LightSteelBlue;
            }
            else
            {
                row.Cells["KaynakDonusumIslem"].Style.BackColor = Color.LightGray;
            }
        }

        private void KaynakDonusumEkraniAc(int rowIndex)
        {
            var row = GrdEslestirme.Rows[rowIndex];

            if (row.Tag is not EslestirmeSonucu)
            {
                GridKontrolEt(row);
            }

            if (row.Tag is not EslestirmeSonucu sonuc)
            {
                MessageBox.Show("Önce eşleştirme kontrolünü yapın (GridKontrolEt çalıştırılmalı).", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string kaynakTabloAdi = TrwKaynakTablolar.SelectedNode?.Tag as string;
            string hedefTabloAdi = TrwHedefTablolar.SelectedNode?.Tag as string;

            string kaynakKolonAdi = row.Cells["KaynakKolon"].Value?.ToString();
            string hedefKolonAdi = row.Cells["HedefKolon"].Value?.ToString();

            string KaynakaramaTablo = row.Cells["KaynakAramaTablo"].Value?.ToString() ?? string.Empty;
            string KaynakaramaDegerKolon = row.Cells["KaynakAramaDegerKolon"].Value?.ToString() ?? string.Empty;
            string KaynakaramaIdKolon = row.Cells["KaynakAramaIdKolon"].Value?.ToString() ?? string.Empty;

            bool donusumGerekli = (bool)(row.Cells["KaynakDonusumGerekli"].Value ?? false);

            if (string.IsNullOrWhiteSpace(kaynakTabloAdi))
            {
                MessageBox.Show("Kaynak tablo seçmelisiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!donusumGerekli)
            {
                MessageBox.Show("Bu kolon için Dönüşüm Gerekli kutucuğunu işaretlemelisiniz.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (string.IsNullOrWhiteSpace(hedefKolonAdi))
            {
                MessageBox.Show("Önce bir Hedef Kolon seçmelisiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(KaynakaramaTablo) ||
                string.IsNullOrWhiteSpace(KaynakaramaDegerKolon) ||
                string.IsNullOrWhiteSpace(KaynakaramaIdKolon))
            {
                MessageBox.Show("Kaynak Dönüşüm işlemi için 'Arama Tablosu', 'Arama Değer Kolonu' ve 'Arama ID Kolonu' alanlarının Grid'de seçilmiş olması zorunludur. Lütfen doldurunuz.", "Eşleştirme Eksik", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            KolonBilgisi kaynakKolonBilgisi = null;
            KolonBilgisi hedefKolonBilgisi = null;

            if (!string.IsNullOrWhiteSpace(kaynakKolonAdi) && KaynakKolonlar.ContainsKey(kaynakKolonAdi))
            {
                kaynakKolonBilgisi = KaynakKolonlar[kaynakKolonAdi];
            }
            if (!string.IsNullOrWhiteSpace(hedefKolonAdi) && HedefKolonlar.ContainsKey(hedefKolonAdi))
            {
                hedefKolonBilgisi = HedefKolonlar[hedefKolonAdi];
            }

            if (kaynakKolonAdi != "(MANUEL GİRİŞ)" && (kaynakKolonBilgisi == null || hedefKolonBilgisi == null))
            {
                MessageBox.Show("Kolon bilgisi (tip, uzunluk vb.) bulunamadı. Lütfen kolonları yeniden yükleyin.", "Kritik Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            
            KolonBilgisi kaynakAramaDegerKolonBilgisi = null;
            KolonBilgisi kaynakAramaIDKolonBilgisi = null;
            try
            {

                kaynakAramaIDKolonBilgisi = KaynakRepo.GetKolonBilgisi(KaynakaramaTablo, KaynakaramaIdKolon);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kaynak Lookup Değer Kolonu ({KaynakaramaIdKolon}) bilgisi çekilirken hata oluştu: {ex.Message}", "Kritik Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            long? kaynakLookupLen = null;
            long? hedefLen = null;

            if (kaynakAramaIDKolonBilgisi.Length.HasValue)
            {
                kaynakLookupLen = kaynakAramaIDKolonBilgisi.Length.Value == -1 ? int.MaxValue : kaynakAramaIDKolonBilgisi.Length.Value;
            }
            if (hedefKolonBilgisi.Length.HasValue)
            {
                hedefLen = hedefKolonBilgisi.Length.Value == -1 ? int.MaxValue : hedefKolonBilgisi.Length.Value;
            }

            KaynakDonusumUyarıBilgisi uyariNesnesi = new KaynakDonusumUyarıBilgisi
            {
                KontrolEdilenAlan = KaynakaramaDegerKolon,
                KaynakTipi = kaynakAramaIDKolonBilgisi.DataType,
                KaynakUzunluk = kaynakLookupLen,
                HedefTipi = hedefKolonBilgisi.DataType,
                HedefUzunluk = hedefLen
            };


            if (hedefKolonBilgisi.IsStringBased && kaynakLookupLen.HasValue && hedefLen.HasValue && hedefLen < kaynakLookupLen)
            {
                uyariNesnesi.UyariMesaji = $"KRİTİK RİSK: Veri Kırpılması Riski! Kaynak Uzunluk ({kaynakLookupLen}) Hedef Uzunluk ({hedefLen})'ten büyüktür.";
            }
            else if (hedefKolonBilgisi.IsStringBased && !kaynakAramaIDKolonBilgisi.IsStringBased)
            {
                uyariNesnesi.UyariMesaji = $" BİLGİ: Tip Dönüşümü Gerekli. Kaynak Tipi ({kaynakAramaIDKolonBilgisi.DataType}) string hedefe dönüştürülecektir.";
            }
            else
            {
                uyariNesnesi.UyariMesaji = "Uygun Kritik  hata yok";
            }
            try
            {
                
                var kaynakDonusumForm = new KaynakDonusumEkrani(
                    kaynakKolonAdi: kaynakKolonAdi,
                    kaynakTabloAdi: kaynakTabloAdi,
                    kaynakKolonBilgisi: kaynakKolonBilgisi,
                    hedefKolonBilgisi: hedefKolonBilgisi,
                    kaynakAramaIdKolonBilgisi: kaynakAramaIDKolonBilgisi,
                    kaynakUyariBilgisi: uyariNesnesi,

                    aramaTablo: KaynakaramaTablo,
                    aramaDegerKolon: KaynakaramaDegerKolon,
                    aramaIdKolon: KaynakaramaIdKolon,

                    kaynakBaglanti: kaynak,
                    donusumTipi: sonuc.DonusumTipi
                );

                if (kaynakDonusumForm.ShowDialog() == DialogResult.OK)
                {
                    Dictionary<string, object> yeniSozluk = kaynakDonusumForm.DonusumSozlugu;

                    
                    sonuc.DonusumSozlugu = yeniSozluk;
                    sonuc.DonusumTipi = DonusumTuru.LookupEslestirme;
                    sonuc.EslestirmeUygun = true;

                    
                    row.Cells["KaynakDonusumGerekli"].Value = true;
                    row.Cells["Uygunluk"].Value = "Uygun (Kaynak Dönüşümlü)";
                    row.Cells["Uygunluk"].Style.ForeColor = Color.DarkGreen;
                    row.Tag = sonuc; 
                    MessageBox.Show("Kaynak Dönüşüm ayarları kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kaynak Dönüşüm ekranı açılırken kritik hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GrdEslestirme_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (GrdEslestirme.CurrentCell.ColumnIndex == GrdEslestirme.Columns["KaynakDonusumGerekli"].Index)
            {
                GrdEslestirme.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
            
            else if (GrdEslestirme.CurrentCell.ColumnIndex == GrdEslestirme.Columns["DonusumGerekli"].Index)
            {
                GrdEslestirme.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void GrdEslestirme_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && GrdEslestirme.Columns[e.ColumnIndex].Name == "DonusumGerekli")
            {
                var cell = GrdEslestirme.Rows[e.RowIndex].Cells["DonusumGerekli"];     
                bool yeniDurum;

                if (cell.EditedFormattedValue is bool editedValue)
                {
                    yeniDurum = editedValue;
                }
                else
                {
     
                    bool currentValue = cell.Value as bool? ?? false;
                    yeniDurum = !currentValue;
                }

                GrdEslestirme.CommitEdit(DataGridViewDataErrorContexts.Commit);

                if (yeniDurum)
                {
                    GrdEslestirme.Columns["DonusumIslem"].Visible = true;
                    GrdEslestirme.Columns["AramaTablo"].Visible = true;
                    GrdEslestirme.Columns["AramaDegerKolon"].Visible = true;
                    GrdEslestirme.Columns["AramaIdKolon"].Visible = true;
                }
                else 
                {
                    GrdEslestirme.Columns["DonusumIslem"].Visible = false;
                    GrdEslestirme.Columns["AramaTablo"].Visible = false;
                    GrdEslestirme.Columns["AramaDegerKolon"].Visible = false;
                    GrdEslestirme.Columns["AramaIdKolon"].Visible = false;
                }

                AyarlariGuncelle(e.RowIndex, yeniDurum);
            }
            else if (e.RowIndex >= 0 && GrdEslestirme.Columns[e.ColumnIndex].Name == "KaynakDonusumGerekli")
            {
                var cell = GrdEslestirme.Rows[e.RowIndex].Cells["KaynakDonusumGerekli"];
                bool yeniDurum;
                if (cell.EditedFormattedValue is bool editedValue)
                {
                    yeniDurum = editedValue;
                }
                else
                {
                    bool currentValue = cell.Value as bool? ?? false;
                    yeniDurum = !currentValue;
                }
                GrdEslestirme.CommitEdit(DataGridViewDataErrorContexts.Commit);
                if (yeniDurum)
                {
                    GrdEslestirme.Columns["KaynakDonusumIslem"].Visible = true;
                    GrdEslestirme.Columns["KaynakAramaTablo"].Visible = true;
                    GrdEslestirme.Columns["KaynakAramaDegerKolon"].Visible = true;
                    GrdEslestirme.Columns["KaynakAramaIdKolon"].Visible = true;
                }
                else
                {
                    GrdEslestirme.Columns["KaynakDonusumIslem"].Visible = false;
                    GrdEslestirme.Columns["KaynakAramaTablo"].Visible = false;
                    GrdEslestirme.Columns["KaynakAramaDegerKolon"].Visible = false;
                    GrdEslestirme.Columns["KaynakAramaIdKolon"].Visible = false;
                }
                KaynakAyarlariGuncelle(e.RowIndex, yeniDurum);

            }
        }
    }
}