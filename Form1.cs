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


        private Dictionary<string, (string DataType, int? Length, bool IsNullable)> KaynakKolonlar =
            new Dictionary<string, (string DataType, int? Length, bool IsNullable)>(StringComparer.OrdinalIgnoreCase); //kaynak kolonların bilgilerini tutmak için sözlük olustutkdu.

        private Dictionary<string, (string DataType, int? Length, bool IsNullable)> HedefKolonlar =
            new Dictionary<string, (string DataType, int? Length, bool IsNullable)>(StringComparer.OrdinalIgnoreCase);

        public FrmVeriEslestirme(BaglantiBilgileri kaynakBilgi, BaglantiBilgileri hedefBilgi, FrmBaglantiAc oncekiForm)
        {
            InitializeComponent();

            kaynak = kaynakBilgi;
            hedef = hedefBilgi;
            _oncekiForm = oncekiForm;

            GridBaslat();
            this.Load += FrmVeriEslestirme_Load;
        }

        private void GridBaslat() // iki tablo arası eşlestiremyi gösterir
        {
            GrdEslestirme.AutoGenerateColumns = false; //otomaitk kolon oluşturmaaz
            GrdEslestirme.Columns.Clear();

            var kolonKaynak = new DataGridViewTextBoxColumn 
            { 
                Name = "KaynakKolon",
                HeaderText = "Kaynak kolonlar",
                ReadOnly = true 
            };
            var KaynakkolonTip = new DataGridViewTextBoxColumn
            {
                Name = "Tip",
                HeaderText = "Kaynak Data Tipi",
                ReadOnly = true
            };
            var KaynakkolonUzunlugu = new DataGridViewTextBoxColumn
            {
                Name = "Uzunluk",
                HeaderText = "Kaynak Uzunluk",
                ReadOnly = true
            };
            var KaynakkolonNullable = new DataGridViewTextBoxColumn
            {
                Name = "Nullable",
                HeaderText = "Kaynak Boş Geçilebilir",
                ReadOnly = true
            };


            var kolonHedef = new DataGridViewComboBoxColumn 
            { 
                Name = "HedefKolon",
                HeaderText = "Hedef kolonlar",
                FlatStyle = FlatStyle.Flat 
            }; 
            var kolonTip = new DataGridViewTextBoxColumn 
            { 
                Name = "Tip",
                HeaderText = "Hedef Data Tipi",
                ReadOnly = true 
            };
            var kolonUzunlugu = new DataGridViewTextBoxColumn 
            {
                Name = "Uzunluk", 
                HeaderText = "Hedef Uzunluk",
                ReadOnly = true
            };
            var kolonNullable = new DataGridViewTextBoxColumn
            { 
                Name = "Nullable", 
                HeaderText = "Hedef Boş Geçilebilir",
                ReadOnly = true 
            };
            var kolonUygunluk = new DataGridViewTextBoxColumn 
            { 
                Name = "Uygunluk",
                HeaderText = "Uygunluk",
                ReadOnly = true 
            };

            GrdEslestirme.Columns.AddRange(new DataGridViewColumn[] { kolonKaynak,KaynakkolonTip,KaynakkolonUzunlugu,KaynakkolonNullable ,kolonHedef, kolonTip, kolonUzunlugu, kolonNullable, kolonUygunluk });
            GrdEslestirme.AllowUserToAddRows = false; // kullanıcı bofş satır ekleyemez
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
                    row.Cells["Tip"].Value = bilgi.DataType;
                    row.Cells["Uzunluk"].Value = bilgi.Length?.ToString() ?? "";
                    row.Cells["Nullable"].Value = bilgi.IsNullable ? "YES" : "NO";
                }

                // Uygunluk kontrolünü güncelle
                KontrolEt(row);
            }
        }

        private void KaynakSutunBilgileriGetir(Dictionary<string, (string DataType, int? Length, bool IsNullable)> kaynakKolon)//dictionayden gelen kolon bilgilerini gride yükleme kaynak kolonları ama
        {
            GrdEslestirme.Rows.Clear();
            foreach (var kaynak in kaynakKolon)
            {
                int satır = GrdEslestirme.Rows.Add();
                var row = GrdEslestirme.Rows[satır];

               
                row.Cells["KaynakKolon"].Value = kaynak.Key;              
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

        private async Task<Dictionary<string, (string DataType, int? Length, bool IsNullable)>> KolonBilgileriniGetirAsync(BaglantiBilgileri info, string tabloAdi)
        {
            var kolonlar = new Dictionary<string, (string DataType, int? Length, bool IsNullable)>(StringComparer.OrdinalIgnoreCase);

            if (info == null || string.IsNullOrWhiteSpace(info.Sunucu) || string.IsNullOrWhiteSpace(tabloAdi))
                return kolonlar;

            string connStr = $"Server={info.Sunucu};Database={info.Veritabani};User Id={info.Kullanici};Password={info.Sifre};TrustServerCertificate=True;";

            string sql = @"SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, IS_NULLABLE
                       FROM INFORMATION_SCHEMA.COLUMNS
                       WHERE TABLE_NAME = @TableName
                       ORDER BY ORDINAL_POSITION";

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
                            kolonlar[KolonIsmi] = (tip, uzunluk, isNullable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lstLog.Items.Add($"Kolon alinamadi: {ex.Message}");
                MessageBox.Show($"Kolon bilgisi alınamadı: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return kolonlar;
        }



        private void GrdEslestirme_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // hızlı kontrol trigger
            if (e.RowIndex >= 0)
                KontrolEt(GrdEslestirme.Rows[e.RowIndex]);
        }



        private void KontrolEt(DataGridViewRow row)
        {
            try
            {
                var kaynakKolon = row.Cells["KaynakKolon"].Value?.ToString();
                var hedefKolon = row.Cells["HedefKolon"].Value?.ToString();
                if (string.IsNullOrWhiteSpace(kaynakKolon) || string.IsNullOrWhiteSpace(hedefKolon))
                {
                    row.Cells["Uygunluk"].Value = "";
                    return;
                }

                if (!KaynakKolonlar.TryGetValue(kaynakKolon, out var KaynakBilgi))
                {
                    row.Cells["Uygunluk"].Value = "Kaynak yok";
                    row.Cells["Uygunluk"].Style.ForeColor = Color.Red;
                    return;
                }

                if (!HedefKolonlar.TryGetValue(hedefKolon, out var HedefBilgi))
                {
                    row.Cells["Uygunluk"].Value = "Hedef yok";
                    row.Cells["Uygunluk"].Style.ForeColor = Color.Red;
                    return;
                }

                // data type kontrol
                if (!string.Equals(KaynakBilgi.DataType, HedefBilgi.DataType, StringComparison.OrdinalIgnoreCase))
                {
                    row.Cells["Uygunluk"].Value = $"Uyumsuz tip ({KaynakBilgi.DataType}->{HedefBilgi.DataType})";
                    row.Cells["Uygunluk"].Style.ForeColor = Color.Red;
                    return;
                }

                // nullable hedef NOT NULL ise hata
                if (!HedefBilgi.IsNullable && KaynakBilgi.IsNullable) //hedef no nullable=false ve kaynak yes ise
                {
                    // kaynak nullable ama hedef not null
                    row.Cells["Uygunluk"].Value = "Hedef Null olamaz";
                    row.Cells["Uygunluk"].Style.ForeColor = Color.OrangeRed;
                    return;
                }

                // length kontrol
                if (KaynakBilgi.Length.HasValue && HedefBilgi.Length.HasValue && KaynakBilgi.Length > HedefBilgi.Length) // nvarchar varchar nchar
                {
                    row.Cells["Uygunluk"].Value = "Uzunluk Aşıyor";
                    row.Cells["Uygunluk"].Style.ForeColor = Color.Orange;
                    return;
                }

                row.Cells["Uygunluk"].Value = "Uygun";
                row.Cells["Uygunluk"].Style.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                lstLog.Items.Add($"Kontrol hata: {ex.Message}");
            }
        }



        private List<(string KaynakKolon, string HedefKolon)> EslestirmeListesi()//kolon eşleşme listesini döndürüyor
        {
            var liste = new List<(string KaynakKolon, string HedefKolon)>();

            foreach (DataGridViewRow row in GrdEslestirme.Rows)
            {
                KontrolEt(row);

                if (row.IsNewRow) 
                    continue;

                if (row.Cells["Uygunluk"].Value?.ToString() != "Uygun")
                    continue;

                string kaynak = row.Cells["KaynakKolon"].Value?.ToString();
                string hedef = row.Cells["HedefKolon"].Value?.ToString();

                if (!string.IsNullOrWhiteSpace(kaynak) && !string.IsNullOrWhiteSpace(hedef)) //boş olmayan kolonları listeye ekle
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


        private async void BtnSutunYkle_Click(object sender, EventArgs e)
        {
            try
            {
                // Kaynak tablo seçiliyse
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

        private void BtnOtomatikEsle_Click(object sender, EventArgs e)// isim bazlı
        {

            foreach (DataGridViewRow row in GrdEslestirme.Rows)
            {
                if (row.Cells["HedefKolon"] is DataGridViewComboBoxCell comboCell)
                {
                    comboCell.Items.Clear();
                    comboCell.Items.AddRange(HedefKolonlar.Keys.ToArray());
                }
            }

            var hedefList = HedefKolonlar.Keys.ToList();
            foreach (DataGridViewRow row in GrdEslestirme.Rows)
            {
                var source = row.Cells["KaynakKolon"].Value?.ToString();

                if (string.IsNullOrWhiteSpace(source))
                    continue;

                var match = hedefList.FirstOrDefault(h => h.Equals(source, StringComparison.OrdinalIgnoreCase));

                if (match != null)
                {
                    var cell = row.Cells["HedefKolon"] as DataGridViewComboBoxCell;
                    if (cell != null)
                    {

                        if (!cell.Items.Contains(match))
                            cell.Items.Add(match);

                        cell.Value = match;
                    }
                }
                KontrolEt(row);

                if (!string.IsNullOrEmpty(match) && HedefKolonlar.TryGetValue(match, out var hInfo))
                {
                    row.Cells["Tip"].Value = hInfo.DataType;
                    row.Cells["Uzunluk"].Value = hInfo.Length?.ToString() ?? "";
                    row.Cells["Nullable"].Value = hInfo.IsNullable ? "YES" : "NO";
                }

            }
            lstLog.Items.Add("Eşleme tamamlandı.");


        }

        private void BtnStrEkle_Click(object sender, EventArgs e)
        {
            GrdEslestirme.Rows.Add();
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

            string kaynakTablo = TrwKaynakTablolar.SelectedNode.Tag.ToString();//treeviewde seçilen tablo adları alır
            string hedefTablo = TrwHedefTablolar.SelectedNode.Tag.ToString();

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


        private async Task TransferSatiriKontrolu(BaglantiBilgileri kaynak,BaglantiBilgileri hedef,string kaynakTablo,string hedefTablo,
            List<(string KaynakKolon, string HedefKolon)> eslesmeler, DataTable kaynakVeri)//hedefteki verinin satır satır kontrolu
        { 
        
            string hedefConnStr = ConnectionString(hedef);

            using var conn = new SqlConnection(hedefConnStr);
            await conn.OpenAsync();

            int toplam = kaynakVeri.Rows.Count;
            int aktarılan = 0;
            int atlanan = 0;
            int islenen = 0;

            foreach (DataRow row in kaynakVeri.Rows)
            {
                
                string kosul = string.Join(" AND ", eslesmeler.Select(x => $"[{x.HedefKolon}] = @{x.HedefKolon}"));//eşleşen kolonları alır ve koşul oluşturur

                string kontrolSql = $"SELECT COUNT(1) FROM [{hedefTablo}] WHERE {kosul}";//hedefte var mı kontrolü

                using (var cmd = new SqlCommand(kontrolSql, conn))
                {
                    foreach (var (kaynakKolon, hedefKolon) in eslesmeler)
                        cmd.Parameters.AddWithValue($"@{hedefKolon}", row[kaynakKolon] ?? DBNull.Value);

                    int sonuc = (int) await cmd.ExecuteScalarAsync();
                    if (sonuc > 0)
                    {
                        atlanan++;
                        islenen++;
                        ProgresGuncelle(islenen, toplam, aktarılan, atlanan);
                        continue;
                    }
                }

                
                string kolonList = string.Join(",", eslesmeler.Select(x => $"[{x.HedefKolon}]"));// Hedef tabloya ekleyeceğimiz kolonları virgülle birleştirir:
                string parametreList = string.Join(",", eslesmeler.Select(x => $"@{x.HedefKolon}"));//SQL parametrelerini oluşturur:

                string insertSql = $"INSERT INTO [{hedefTablo}] ({kolonList}) VALUES ({parametreList})"; 

                using (var insertCmd = new SqlCommand(insertSql, conn))
                {
                    foreach (var (kaynakKolon, hedefKolon) in eslesmeler)
                        insertCmd.Parameters.AddWithValue($"@{hedefKolon}", row[kaynakKolon] ?? DBNull.Value);

                    await insertCmd.ExecuteNonQueryAsync();
                }

                aktarılan++;
                islenen++;

                
                if (islenen % 10 == 0 || islenen == toplam)
                    ProgresGuncelle(islenen, toplam, aktarılan, atlanan);
            }

            
            ProgresGuncelle(toplam, toplam, aktarılan, atlanan);
            LogEkle($"Transfer tamamlandı: {aktarılan} yeni kayıt eklendi, {atlanan} kayıt zaten mevcuttu.");
            MessageBox.Show(
                $"Transfer tamamlandı.\nYeni kayıt: {aktarılan}\nZaten mevcut: {atlanan}",
                "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information
            );
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

        private async void BtnHedefSutunYkle_Click(object sender, EventArgs e)
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

        private async Task UI_Guncelle()
        {

        }
    }
}