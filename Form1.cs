using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using static Azure.Core.HttpHeader;


namespace DataTransfer
{
    public partial class FrmVeriEslestirme : Form
    {
        private BaglantiBilgileri kaynak;
        private BaglantiBilgileri hedef;

        private FrmBaglantiAc _oncekiForm;


        private Dictionary<string, KolonBilgisi> KaynakKolonlar =
            new Dictionary<string, KolonBilgisi>(StringComparer.OrdinalIgnoreCase); //kaynak kolonların bilgilerini tutmak için sözlük olustutkdu.

        private Dictionary<string, KolonBilgisi> HedefKolonlar =
            new Dictionary<string, KolonBilgisi>(StringComparer.OrdinalIgnoreCase);

        public FrmVeriEslestirme(BaglantiBilgileri kaynakBilgi, BaglantiBilgileri hedefBilgi, FrmBaglantiAc oncekiForm)
        {
            InitializeComponent();

            kaynak = kaynakBilgi;
            hedef = hedefBilgi;
            _oncekiForm = oncekiForm;

            GridBaslat();
            this.Load += FrmVeriEslestirme_Load;
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


            GrdEslestirme.Columns.AddRange(new DataGridViewColumn[]
            {
                kolonKaynak, KaynakTip, KaynakUzunluk, KaynakNullable, kolonHedef,mukerrerKolon, HedefTip, HedefUzunluk, HedefNullable, kolonUygunluk
            });

            GrdEslestirme.AllowUserToAddRows = false;
        }



        private async Task TablolarıAgacaYukleAsync() //secilen veritabanındaki tabloları treeview a yükleme
        {
            try
            {
                // kaynak tablolar
                var KaynakTablo = await TabloGetirAsync(kaynak);
                TrwKaynakTablolar.Nodes.Clear(); //treewievdeki düğümleri siler

                foreach (var tabloAd in KaynakTablo.OrderBy(x => x))
                    TrwKaynakTablolar.Nodes.Add(new TreeNode(tabloAd) { Tag = tabloAd }); //treewiew nesnesi kontrolüne ekleme işlemi alfabetik sıraya göre


                var HedefTablo = await TabloGetirAsync(hedef);
                TrwHedefTablolar.Nodes.Clear();

                foreach (var t in HedefTablo.OrderBy(x => x))
                    TrwHedefTablolar.Nodes.Add(new TreeNode(t) { Tag = t });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Tablo yükleme hatası: {ex.Message}");
            }
        }

        #region TabloGetirme
        private async Task<List<string>> TabloGetirAsync(BaglantiBilgileri info)
        {
            var list = new List<string>();
            string connStr = $"Server={info.Sunucu};Database={info.Veritabani};User Id={info.Kullanici};Password={info.Sifre};TrustServerCertificate=True;";

            string sql = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE' AND TABLE_NAME NOT IN ('__EFMigrationsHistory','sysdiagrams') ORDER BY TABLE_NAME";
            try
            {
                using (var conn = new SqlConnection(connStr))
                using (var cmd = new SqlCommand(sql, conn))
                {
                    await conn.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(reader.GetString(0));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lstLog.Items.Add($"Tablo Yükleme hatası ({info.Sunucu}): {ex.Message}");
            }
            return list;
        }
        #endregion


        private async void TrwKaynakTablolar_AfterSelect(object sender, TreeViewEventArgs e)//tablo seçme işlemi
        {
            if (e.Node?.Tag is string tablo && !string.IsNullOrWhiteSpace(tablo))
                lstLog.Items.Add($"Kaynak tablo seçildi: {tablo}");
        }

        private async void TrwHedefTablolar_AfterSelect(object sender, TreeViewEventArgs e)
        {

            if (e.Node?.Tag is string tablo && !string.IsNullOrWhiteSpace(tablo)) // node.tag? tablo adı
                lstLog.Items.Add($"Hedef tablo seçildi: {tablo}");
        }

        private void HedefKolonSecildi(object? sender, EventArgs e) //gridde hedef kolon seçilince kolon bilgileri doldurmak için
        {
            if (sender is ComboBox combo && GrdEslestirme.CurrentCell != null)//sender combobox
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

                // Uygunluk kontrolünü güncelle
                KontrolEt(row);
            }
        }

        private void KaynakSutunBilgileriGetir(Dictionary<string, KolonBilgisi> kaynakKolon)//dictionayden gelen kolon bilgilerini gride yükleme kaynak kolonları ama
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
                if (cell == null) continue;
                cell.Items.Clear();
                foreach (var h in hedefKolonIsimleri)
                    cell.Items.Add(h);
            }
        }

        private async Task<Dictionary<string, KolonBilgisi>> KolonBilgileriniGetirAsync(BaglantiBilgileri info, string tabloAdi)
        {
            var kolonlar = new Dictionary<string, KolonBilgisi>(StringComparer.OrdinalIgnoreCase);

            if (info == null || string.IsNullOrWhiteSpace(info.Sunucu) || string.IsNullOrWhiteSpace(tabloAdi))
                return kolonlar;

            string connStr = $"Server={info.Sunucu};Database={info.Veritabani};User Id={info.Kullanici};Password={info.Sifre};TrustServerCertificate=True;";

            //  INFORMATION_SCHEMA ile sistem tablolarını (sys.indexes) birleştirerek Primary Key ve Unique Index bilgisini tek bir IsUnique alanı olarak çeker.
            
            string sql = $@"
            SELECT 
                C.COLUMN_NAME, 
                C.DATA_TYPE, 
                C.CHARACTER_MAXIMUM_LENGTH, 
                C.IS_NULLABLE,
                ISNULL(INDEXES.IsUnique, 0) AS IsUnique -- PK ve Unique Index'leri tek bir alanda toplar
            FROM INFORMATION_SCHEMA.COLUMNS C
            JOIN sys.tables T ON T.name = C.TABLE_NAME AND T.name = @TableName
            JOIN sys.schemas S ON S.schema_id = T.schema_id
            LEFT JOIN (
                SELECT 
                    COL.name AS COLUMN_NAME, 
                    MAX(CAST(I.is_unique_constraint AS INT)) AS IsUnique
                FROM sys.indexes I
                JOIN sys.index_columns IC ON I.object_id = IC.object_id AND I.index_id = IC.index_id
                JOIN sys.columns COL ON COL.object_id = I.object_id AND COL.column_id = IC.column_id
                WHERE I.object_id = OBJECT_ID(@TableName) AND (I.is_unique_constraint = 1 OR I.is_primary_key = 1)
                GROUP BY COL.name
            ) AS INDEXES ON INDEXES.COLUMN_NAME = C.COLUMN_NAME
            WHERE C.TABLE_NAME = @TableName
            ORDER BY C.ORDINAL_POSITION";

            try
            {
                using (var conn = new SqlConnection(connStr))
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@TableName", tabloAdi);
                    await conn.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            string KolonIsmi = reader["COLUMN_NAME"].ToString();
                            string tip = reader["DATA_TYPE"].ToString();

                            
                            int? uzunluk = reader["CHARACTER_MAXIMUM_LENGTH"] == DBNull.Value ? null : (int?)Convert.ToInt32(reader["CHARACTER_MAXIMUM_LENGTH"]);
                            bool isNullable = reader["IS_NULLABLE"].ToString().Equals("YES", StringComparison.OrdinalIgnoreCase);                           
                            bool isUnique = Convert.ToBoolean(reader["IsUnique"]);
                            
                            kolonlar[KolonIsmi] = new KolonBilgisi
                            {
                                DataType = tip,
                                Length = uzunluk,
                                IsNullable = isNullable,
                                IsUnique = isUnique // Benzersiz alan
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
               
                MessageBox.Show($"Kolon bilgisi alınamadı: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return kolonlar;
        }



        private void KontrolEt(DataGridViewRow row)
        {
            try
            {
                bool benzersizAlanCheck = (bool)(row.Cells["IsUnique"].Value ?? false);

               
                if (benzersizAlanCheck && row.Cells["Uygunluk"].Style.ForeColor != Color.Red)
                {
                    row.Tag = "ONAYLANDI";
                }

                
                if (row.Tag != null && row.Tag.ToString() == "ONAYLANDI")
                {
                    row.Cells["Uygunluk"].Value = "Uygun";
                    row.Cells["Uygunluk"].Style.ForeColor = Color.Blue;
                    return;
                }

                var kaynakKolon = row.Cells["KaynakKolon"].Value?.ToString();
                var hedefKolon = row.Cells["HedefKolon"].Value?.ToString();

                if (string.IsNullOrWhiteSpace(kaynakKolon) || string.IsNullOrWhiteSpace(hedefKolon))
                {
                    row.Cells["Uygunluk"].Value = "";
                    return;
                }

                if (!KaynakKolonlar.TryGetValue(kaynakKolon, out var KaynakBilgi) ||
                    !HedefKolonlar.TryGetValue(hedefKolon, out var HedefBilgi))
                {
                    row.Cells["Uygunluk"].Value = "Kolon Bilgisi Eksik";
                    row.Cells["Uygunluk"].Style.ForeColor = Color.Red;
                    return;
                }

                List<string> mesajlar = new List<string>();

                bool kritikHata = false;    
                bool uyariGerekli = false;   

                // 1. Nullable Kontrolü hhedef Null olamaz ama Kaynak Null geliyorsa
                if (!HedefBilgi.IsNullable && KaynakBilgi.IsNullable)
                {
                    mesajlar.Add("Hedef NULL kabul etmiyor");
                    kritikHata = true;
                }

                // 2. Metinsel Dönüşümler (nvarchar <-> char vb.)
                string[] metinselTipler = { "nvarchar", "nchar", "varchar", "char", "text", "ntext" };
                bool kaynakMetin = metinselTipler.Contains(KaynakBilgi.DataType.ToLower());
                bool hedefMetin = metinselTipler.Contains(HedefBilgi.DataType.ToLower());

                if (kaynakMetin && hedefMetin)
                {
                    // Tip ismi farklıysa (örn: nvarchar -> nchar)
                    if (!string.Equals(KaynakBilgi.DataType, HedefBilgi.DataType, StringComparison.OrdinalIgnoreCase))
                    {
                        mesajlar.Add($"Tip Dönüşümü ({KaynakBilgi.DataType}->{HedefBilgi.DataType})");
                        uyariGerekli = true; // Bu bir uyarıd onaylanırsa geçilir
                    }

                    // Uzunluk Kontrolü
                    if (KaynakBilgi.Length.HasValue && HedefBilgi.Length.HasValue)
                    {
                        // -1  değerlerini int.MaxValuee dönüştürerek mantıksal karşılaştırma yapıyoruz
                        long kaynakLen = KaynakBilgi.Length.Value == -1 ? int.MaxValue : KaynakBilgi.Length.Value;
                        long hedefLen = HedefBilgi.Length.Value == -1 ? int.MaxValue : HedefBilgi.Length.Value;

                        
                        if (hedefLen < kaynakLen)
                        {
                            // Ekranda kullanıcıya -1 yerine "MAX" göstermek için string hazırlıyoruz
                            string kStr = KaynakBilgi.Length.Value == -1 ? "MAX" : KaynakBilgi.Length.Value.ToString();
                            string hStr = HedefBilgi.Length.Value == -1 ? "MAX" : HedefBilgi.Length.Value.ToString();

                            mesajlar.Add($"Kırpılma Riski ({kStr}->{hStr})");
                            uyariGerekli = true; // Onay gerektiren turuncu rengi tetikler.
                        }
                        else if (hedefLen > kaynakLen)
                        {
                            
                            string kStr = KaynakBilgi.Length.Value == -1 ? "MAX" : KaynakBilgi.Length.Value.ToString();
                            string hStr = HedefBilgi.Length.Value == -1 ? "MAX" : HedefBilgi.Length.Value.ToString();

                         
                            mesajlar.Add($" ({kStr}->{hStr})");
                        }
                        
                    }
                }

               
                else if (SayiKontrolu(KaynakBilgi.DataType) && SayiKontrolu(HedefBilgi.DataType))
                {
                    bool kaynakOndalik = OndalikliTip(KaynakBilgi.DataType);
                    bool hedefTam = TamSayiliTip(HedefBilgi.DataType);

                    
                    if (kaynakOndalik && hedefTam) 
                    {
                        
                        mesajlar.Add("Ondalık -> Tam Sayı (Veri Kaybı Riski)");

                        kritikHata = false; 
                        uyariGerekli = true; 
                    }                   
                }
                else
                {                  
                    string [] tarihTipleri = { "date", "datetime", "datetime2", "smalldatetime", "time" };
                    bool kaynakTarih = tarihTipleri.Contains(KaynakBilgi.DataType.ToLower());
                    bool hedefTarih = tarihTipleri.Contains(HedefBilgi.DataType.ToLower());

                    
                    if ((SayiKontrolu(KaynakBilgi.DataType) || kaynakTarih) && hedefMetin)
                    {
                        //  int varchar veya datetime nvarchar
                        mesajlar.Add($"UYUŞMAZLIK: {KaynakBilgi.DataType} -> {HedefBilgi.DataType}");
                        kritikHata = true;
                    }
                    //  Metinsel bir alanın sayısal veya tarihsel bir alana dönüştürülmemesi
                    else if (kaynakMetin && (SayiKontrolu(HedefBilgi.DataType) || hedefTarih))
                    {
                        //  nvarchar  int
                        mesajlar.Add($"UYUŞMAZLIK: {KaynakBilgi.DataType} -> {HedefBilgi.DataType} (Tip Çakışması)");
                        kritikHata = true;
                    }
                    
                    else if (!string.Equals(KaynakBilgi.DataType, HedefBilgi.DataType, StringComparison.OrdinalIgnoreCase))
                    {
                        mesajlar.Add($"Alakasız Tip Uyuşmazlığı: {KaynakBilgi.DataType} -> {HedefBilgi.DataType}");
                        uyariGerekli = true; 
                    }
                }

                
                if (kritikHata)
                {
                    row.Cells["Uygunluk"].Value = string.Join(", ", mesajlar);
                    row.Cells["Uygunluk"].Style.ForeColor = Color.Red;
                    row.Tag = null; 
                }
                else if (uyariGerekli)
                {
                   
                    row.Cells["Uygunluk"].Value = "ONAY GEREKİYOR: " + string.Join(", ", mesajlar);
                    row.Cells["Uygunluk"].Style.ForeColor = Color.DarkOrange;
                    row.Tag = null; 
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




        private List<(string KaynakKolon, string HedefKolon)> EslestirmeListesi() //kolon eşleşme listesini döndürüyor
        {
            var liste = new List<(string KaynakKolon, string HedefKolon)>();

            foreach (DataGridViewRow row in GrdEslestirme.Rows)
            {
                KontrolEt(row);

                if (row.IsNewRow) continue;

                string durum = row.Cells["Uygunluk"].Value?.ToString();

               
                bool onayli = (row.Tag?.ToString() == "ONAYLANDI");

                if (!onayli)
                    continue; 

                string kaynak = row.Cells["KaynakKolon"].Value?.ToString();
                string hedef = row.Cells["HedefKolon"].Value?.ToString();

                if (!string.IsNullOrWhiteSpace(kaynak) && !string.IsNullOrWhiteSpace(hedef))
                {
                    liste.Add((kaynak, hedef));
                }
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




        private void BtnOtomatikEsle_Click(object sender, EventArgs e) // İsim Bazlı Eşleme (Düzeltilmiş)
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

                // Eşleme olsun veya olmasın, uygunluğu kontrol et
                KontrolEt(row);
            }

            lstLog.Items.Add("Eşleme tamamlandı.");
        }


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

        private void GrdEslestirme_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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
                MessageBox.Show("Tüm satırlar seçili filtre testi için gerekmiyor");
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
                string connStr = $"Server={kaynak.Sunucu};Database={kaynak.Veritabani}; User Id={kaynak.Kullanici};Password={kaynak.Sifre};TrustServerCertificate=True;";

                string sql = $"SELECT COUNT(1) FROM [{tablo}] WHERE {where}";
                using (var conn = new SqlConnection(connStr))
                using (var cmd = new SqlCommand(sql, conn))
                {
                    await conn.OpenAsync();
                    var deger = await cmd.ExecuteScalarAsync();//sator degerini döndürüyor
                    MessageBox.Show($"Filtre testi başarılı: {deger} satır döndü.");
                    lstLog.Items.Add($"Filtre testi başarılı: {deger} satır döndü. {where}");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Test hatası: {ex.Message}");
                lstLog.Items.Add($"Filtre hatası: {ex.Message}");
            }
        }




        private string ConnectionString(BaglantiBilgileri b) =>

            $"Server={b.Sunucu};Database={b.Veritabani};User Id={b.Kullanici};Password={b.Sifre};TrustServerCertificate=True;";


        private DataTable DataTableGetir(string connStr, string tablo, List<string> kolonlar, string kosul = "")//sql den veri çeker datatable döndürür belirli tablo ve kolonlardan
        {
            string kolonListe = string.Join(", ", kolonlar.Select(c => $"[{c}]"));//[ad]

            string sql = $"SELECT {kolonListe} FROM [{tablo}]";

            if (!string.IsNullOrWhiteSpace(kosul)) //eger koşul varsa sornadan ekelem yaparım
                sql += " WHERE " + kosul;

            var dt = new DataTable();

            using (var conn = new SqlConnection(connStr))
            using (var da = new SqlDataAdapter(sql, conn))
            {
                da.Fill(dt);
            }
            return dt;
        }


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


        private void GrdEslestirme_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var row = GrdEslestirme.Rows[e.RowIndex];
            KontrolEt(row);
        }

        private async void FrmVeriEslestirme_Load(object sender, EventArgs e)
        {
            lstLog.Items.Add("Tablolar yükleniyor");
            await TablolarıAgacaYukleAsync();
            lstLog.Items.Add("Tablolar yüklendi");
            RdoBtnTumSatır.Checked = true;
        }


        private async void BtnTransferBaslat_Click(object sender, EventArgs e)
        {
            if (TrwKaynakTablolar.SelectedNode == null || TrwHedefTablolar.SelectedNode == null)
            {
                MessageBox.Show("Kaynak ve hedef tablo seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var eslesmeler = EslestirmeListesi(); //kolon eşleşmeya yapar

            if (eslesmeler.Count == 0)
            {
                MessageBox.Show("Uygun kolon eşleşmesi bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string kaynakTablo = TrwKaynakTablolar.SelectedNode.Tag.ToString();
            string hedefTablo = TrwHedefTablolar.SelectedNode.Tag.ToString();


            foreach (var eslesme in eslesmeler)
            {
                var kaynakBilgi = KaynakKolonlar[eslesme.KaynakKolon];
                var hedefBilgi = HedefKolonlar[eslesme.HedefKolon];

                // Kaynak ondalık, Hedef tam sayı ise tehlike var demektir.
                if (OndalikliTip(kaynakBilgi.DataType) && TamSayiliTip(hedefBilgi.DataType))
                {
                    LogEkle($"Veri kaybı ön kontrolü: {eslesme.KaynakKolon} ({kaynakBilgi.DataType}) -> {eslesme.HedefKolon} ({hedefBilgi.DataType})");

                    long ondalikSatirSayisi = await OndalikSayiKontroluAsync(kaynak, kaynakTablo, eslesme.KaynakKolon);

                    if (ondalikSatirSayisi > 0)
                    {
                        LogEkle($"KRİTİK HATA: {eslesme.KaynakKolon} kolonunda {ondalikSatirSayisi} adet ondalık veri bulundu.");
                        MessageBox.Show(
                            $"Aktarım İPTAL EDİLDİ.\n\n" +
                            $"{eslesme.KaynakKolon} kolonunda ondalık değerler mevcuttur. " +
                            $"Hedef kolon ({eslesme.HedefKolon}) tam sayı tipindedir. Bu durum geri dönülemez veri kaybına yol açar.",
                            "Kritik Veri Kaybı Engellendi",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);

                        BtnTransferBaslat.Enabled = true;
                        return; // Kritik hata varsa aktarımı durdur
                    }
                }
            }



            BtnTransferBaslat.Enabled = false;
            prgTransfer.Value = 0;
            prgTransfer.Style = ProgressBarStyle.Continuous;
            LogEkle("Transfer işlemi başlatılıyor...");

            try
            {
                string kaynakConnStr = ConnectionString(kaynak);
                string hedefConnStr = ConnectionString(hedef);
                var kolonlar = eslesmeler.Select(x => x.KaynakKolon).ToList();


                DataTable kaynakVeri = await Task.Run(() => //kaynak veriyi datatable ile çekiyorum daha sonra bunu onizeme formunda göstericem
                    DataTableGetir(kaynakConnStr, kaynakTablo, kolonlar, TxtFiltreleme.Text)
                );


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

        // TİP KONOTROLÜ
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

        private async Task<long> OndalikSayiKontroluAsync(BaglantiBilgileri info,string tabloAdi,string kolonAdi) 
        {
            string connStr= ConnectionString(info);
            string sql= $@"SELECT COUNT(1) FROM [{tabloAdi}] WHERE [{kolonAdi}] IS NOT NULL AND FLOOR([{kolonAdi}]) <> [{kolonAdi}]
           {(!string.IsNullOrWhiteSpace(TxtFiltreleme.Text) ? " AND " + TxtFiltreleme.Text : "")}";

            try
            {
                using (var conn = new SqlConnection(connStr))
                using (var cmd = new SqlCommand(sql, conn))
                {
                    await conn.OpenAsync();
                    var result = await cmd.ExecuteScalarAsync();
                    return Convert.ToInt64(result);
                }
            }
            catch (Exception ex)
            {
                LogEkle($"Ondalık kontrolü sırasında hata: {ex.Message}");
                return long.MaxValue;
            }

        }

        private async Task TransferSatiriKontrolu(BaglantiBilgileri kaynak,BaglantiBilgileri hedef,string kaynakTablo,string hedefTablo,
            List<(string KaynakKolon, string HedefKolon)> eslesmeler,
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
                    benzersizAlan = (bool)(checkCell.Value ?? false); //benzersiz alanları unıque olduğunu kontrol et
                }

                bool uniqueTanimlama = HedefKolonlar.ContainsKey(hedefKolon) && HedefKolonlar[hedefKolon].IsUnique; //hedef kolonun unıque tanımlaması var mı

                if (benzersizAlan || uniqueTanimlama)
                {
                    benzersizKolon.Add(hedefKolon);
                }
            }

            
            benzersizKolon = benzersizKolon.Distinct().ToList();

            bool unıqueKontrolu = benzersizKolon.Any();

            // baglantı ve Transaction

            using var conn = new SqlConnection(hedefConnStr);
            await conn.OpenAsync();
            using var transaction = conn.BeginTransaction();

            int toplam = kaynakVeri.Rows.Count;
            int aktarılan = 0;
            int atlanan = 0;
            int islenen = 0;

            LogEkle($"Mükerrer Kontrol Kolonları (Meta/Manuel): {string.Join(", ", benzersizKolon)} (Sayısı: {benzersizKolon.Count})");

            foreach (DataRow row in kaynakVeri.Rows)
            {
                try
                {
                    bool satirUyumlu = true;

                    var degerEkle = new Dictionary<string, object>();//hedef kolona eklenecek değerler


                    foreach (var (kaynakKolon, hedefKolon) in eslesmeler) //kolon ciftleri uzeronde donuyorum
                    {
                        var KaynakBilgi = KaynakKolonlar[kaynakKolon];
                        var HedefBilgi = HedefKolonlar[hedefKolon];

                        object val = row[kaynakKolon];

                        
                        if ((val == null || val == DBNull.Value))
                        {
                            if (!HedefBilgi.IsNullable)
                            {
                                LogEkle($"Satır Atlandı: '{hedefKolon}' kolonu NULL olamaz.");
                                satirUyumlu = false;
                                break;
                            }
                            degerEkle[hedefKolon] = DBNull.Value;
                            continue;
                        }

                        
                        if (MetinselTip(KaynakBilgi.DataType) && MetinselTip(HedefBilgi.DataType))
                        {
                            string sVal = val.ToString();

                            if (HedefBilgi.Length.HasValue && HedefBilgi.Length.Value != -1 && sVal.Length > HedefBilgi.Length.Value)
                            {
                                sVal = sVal.Substring(0, HedefBilgi.Length.Value); //kırpma islemi
                            }
                            degerEkle[hedefKolon] = sVal; //kırpılmıs str degeri ekle
                        }
                        
                        else if (SayiKontrolu(KaynakBilgi.DataType) && SayiKontrolu(HedefBilgi.DataType))
                        {
                            if (OndalikliTip(KaynakBilgi.DataType) && TamSayiliTip(HedefBilgi.DataType))
                            {
                                decimal dVal = Convert.ToDecimal(val);
                                degerEkle[hedefKolon] = (long)Math.Truncate(dVal);
                            }
                            else
                            {
                                degerEkle[hedefKolon] = val;//oldugu gibi ekle
                            }
                        }                        
                        else
                        {
                            degerEkle[hedefKolon] = val;
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
                            if (degerEkle.TryGetValue(hedefKolon, out object checkValue))
                            {
                                if (checkValue == DBNull.Value)
                                {
                                    kosulKontrolu.Add($"[{hedefKolon}] IS NULL");
                                }
                                else
                                {
                                    string parametreAdi = $"@{hedefKolon}_Check";
                                    object sonKontrolDeger = checkValue;

                                    
                                    if (checkValue is string s)
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

                    // --- INSERT İŞLEMİ ---
                    string cols = string.Join(",", degerEkle.Keys.Select(k => $"[{k}]"));
                    string parametreAdlari = string.Join(",", degerEkle.Keys.Select(k => $"@{k}"));
                    string insertSql = $"INSERT INTO [{hedefTablo}] ({cols}) VALUES ({parametreAdlari})";

                    using (var cmdInsert = new SqlCommand(insertSql, conn, transaction))
                    {
                        foreach (var kvp in degerEkle)
                        {
                            cmdInsert.Parameters.AddWithValue($"@{kvp.Key}", kvp.Value ?? DBNull.Value);
                        }
                        await cmdInsert.ExecuteNonQueryAsync();
                    }

                    aktarılan++;
                    islenen++;

                    if (islenen % 10 == 0) ProgresGuncelle(islenen, toplam, aktarılan, atlanan);

                }
                catch (Exception ex)
                {
                    islenen++;
                    atlanan++;
                    LogEkle($"Satır Hatası: {ex.Message}");
                    ProgresGuncelle(islenen, toplam, aktarılan, atlanan);
                }
            }

            transaction.Commit();
            ProgresGuncelle(toplam, toplam, aktarılan, atlanan);
            LogEkle($"Transfer Bitti. Aktarılan: {aktarılan}, Atlanan: {atlanan}");
            MessageBox.Show($"İşlem Tamamlandı.\nAktarılan: {aktarılan}\nAtlanan: {atlanan}", "Sonuç", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }





        private void GrdEslestirme_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (GrdEslestirme.CurrentCell.ColumnIndex == GrdEslestirme.Columns["HedefKolon"].Index)
            {
                if (e.Control is ComboBox combo)
                {
                    combo.SelectedIndexChanged -= HedefKolonSecildi;
                    combo.SelectedIndexChanged += HedefKolonSecildi;
                }
            }
        }



        private void GrdEslestirme_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = GrdEslestirme.Rows[e.RowIndex];
            if (e.ColumnIndex == GrdEslestirme.Columns["IsUnique"].Index)
            {
                var cell = row.Cells["IsUnique"] as DataGridViewCheckBoxCell;

                if (cell != null)
                {
                    // ReadOnly olsa bile değerini tersine çeviriyoruz (Manuel seçim)
                    bool currentValue = (bool)(cell.Value ?? false);
                    cell.Value = !currentValue;

                    // Değişikliği hemen uygula
                    GrdEslestirme.CommitEdit(DataGridViewDataErrorContexts.Commit);

                    // Bu seçim, satırın uygunluk durumunu etkileyebilir, bu yüzden kontrol et
                    KontrolEt(row);
                }
                return; // İşlem bitti
            }

            string durum = row.Cells["Uygunluk"].Value?.ToString();

            
            if (string.IsNullOrEmpty(durum) || durum == "Uygun") return;

            
            if (row.Cells["Uygunluk"].Style.ForeColor == Color.Red)
            {
                MessageBox.Show("Bu hata kritiktir ve onaylanarak geçilemez. Lütfen kolon eşleşmesini değiştirin.",
                                "Kritik Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            
            if (durum.StartsWith("ONAY GEREKİYOR"))
            {
                string uyariMesaji = durum.Replace("ONAY GEREKİYOR: ", "");

                DialogResult result = MessageBox.Show(
                    $"Bu eşleşmede şu uyarılar var:\n\n{uyariMesaji}\n\n" +
                    "Aktarım sırasında veri kaybı veya kırpılma olabilir. Bunu kabul edip onaylıyor musunuz?",
                    "Onay İsteği",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    
                    row.Tag = "ONAYLANDI";
                    KontrolEt(row); 
                }
            }
        }

        private async void BtnKynkSutunYkle_Click(object sender, EventArgs e)
        {
            try
            {
                if (TrwKaynakTablolar.SelectedNode?.Tag is string kaynakTablo && !string.IsNullOrWhiteSpace(kaynakTablo))
                {
                    lstLog.Items.Add($"Kaynak kolonlar yükleniyor: {kaynakTablo}...");
                    KaynakKolonlar = await KolonBilgileriniGetirAsync(kaynak, kaynakTablo);
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

        private async void BtnHdfSutunYkle_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in GrdEslestirme.Rows)
                {
                    if (row.Cells["HedefKolon"] is DataGridViewComboBoxCell combo)
                    {
                        combo.Items.Clear();
                        combo.Value = null;
                    }
                }

                if (TrwHedefTablolar.SelectedNode?.Tag is string hedefTablo && !string.IsNullOrWhiteSpace(hedefTablo))
                {
                    lstLog.Items.Add($"Hedef kolonlar yükleniyor: {hedefTablo}...");
                    HedefKolonlar = await KolonBilgileriniGetirAsync(hedef, hedefTablo);
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
    }
}