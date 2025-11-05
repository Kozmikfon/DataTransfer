using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;


namespace DataTransfer
{
    public partial class FrmVeriEslestirme : Form
    {
        private BaglantiBilgileri kaynak;
        private BaglantiBilgileri hedef;

        private FrmBaglantiAc _oncekiForm;


        private Dictionary<string, (string DataType, int? Length, bool IsNullable)> KaynakKolonlar =
            new Dictionary<string, (string DataType, int? Length, bool IsNullable)>(StringComparer.OrdinalIgnoreCase); //kaynak kolonların bilgileri tutulmak için sözlük olustutkdu.

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

        private void GridBaslat() // iki tablo arası eşlestiremyi gösteriri
        {
            GrdEslestirme.AutoGenerateColumns = false; //otomaitk kolon oluşturmaaz
            GrdEslestirme.Columns.Clear();

            var kolonKaynak = new DataGridViewTextBoxColumn { Name = "KaynakKolon", HeaderText = "Kaynak kolonlar", ReadOnly = true };
            var kolonHedef = new DataGridViewComboBoxColumn { Name = "HedefKolon", HeaderText = "Hedef kolonlar", FlatStyle = FlatStyle.Flat }; //hedef kolonları comboboxla seçiyorum
            var kolonTip = new DataGridViewTextBoxColumn { Name = "Tip", HeaderText = "DataType", ReadOnly = true };
            var kolonUzunlugu = new DataGridViewTextBoxColumn { Name = "Uzunluk", HeaderText = "Length", ReadOnly = true };
            var kolonNullable = new DataGridViewTextBoxColumn { Name = "Nullable", HeaderText = "Nullable", ReadOnly = true };
            var kolonUygunluk = new DataGridViewTextBoxColumn { Name = "Uygunluk", HeaderText = "Uygunluk", ReadOnly = true };

            GrdEslestirme.Columns.AddRange(new DataGridViewColumn[] { kolonKaynak, kolonHedef, kolonTip, kolonUzunlugu, kolonNullable, kolonUygunluk });
            GrdEslestirme.AllowUserToAddRows = false; // kullanıcı bofş satır ekleyemez
        }




        private async Task TablolarıAgacaYukleAsync()
        {
            try
            {
                // kaynak tablolar
                var KaynakTablo = await TabloGetirAsync(kaynak);
                TrwKaynakTablolar.Nodes.Clear();
                foreach (var t in KaynakTablo.OrderBy(x => x))
                    TrwKaynakTablolar.Nodes.Add(new TreeNode(t) { Tag = t }); //treewiew nesnesine kontrolüne ekleme işlemi


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

        private async Task<List<string>> TabloGetirAsync(BaglantiBilgileri info)
        {
            var list = new List<string>();
            string connStr = $"Server={info.Sunucu};Database={info.Veritabani ?? "master"};User Id={info.Kullanici};Password={info.Sifre};TrustServerCertificate=True;";

            string sql = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE' ORDER BY TABLE_NAME";
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
                lstLog.Items.Add($"Tablo Yukleme hatası ({info.Sunucu}): {ex.Message}");
            }
            return list;
        }

        private async void TrwKaynakTablolar_AfterSelect(object sender, TreeViewEventArgs e)//tablo seçilince kolonları yükle
        {
            if (e.Node == null)
                return;

            string tablo = e.Node.Tag?.ToString();

            if (string.IsNullOrWhiteSpace(tablo))
                return;

            lstLog.Items.Add($"Kaynak Tablolar: {tablo}");

            KaynakKolonlar = await KolonBilgileriniGetirAsync(kaynak, tablo); //treeview kolon yüklenince kaynak kolonlar sözlüğüne atılıyor
            KaynakSutunBilgileriGetir(KaynakKolonlar);
        }

        private async void TrwHedefTablolar_AfterSelect(object sender, TreeViewEventArgs e)//seçilen tabloya göre hedef kolonları yükle 
        {

            if (e.Node == null)
                return;

            string tablo = e.Node.Tag?.ToString();

            if (string.IsNullOrWhiteSpace(tablo))
                return;

            lstLog.Items.Add($"Hedef tablolar: {tablo}");
            HedefKolonlar = await KolonBilgileriniGetirAsync(hedef, tablo);

            HedefGuncelle(HedefKolonlar.Keys.ToList());//
        }

        private void HedefKolonSecildi(object? sender, EventArgs e)
        {
            if (sender is ComboBox combo && GrdEslestirme.CurrentCell != null)
            {
                var secilen = combo.SelectedItem?.ToString();
                if (string.IsNullOrWhiteSpace(secilen)) return;

                var row = GrdEslestirme.CurrentRow;
                if (row == null) return;

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
                //row.Cells["Tip"].Value = kaynak.Value.DataType;
                //row.Cells["Uzunluk"].Value = kaynak.Value.Length?.ToString() ?? "";
                //row.Cells["Nullable"].Value = kaynak.Value.IsNullable ? "YES" : "NO"; //kaynak tablo bilgileri
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

                if (!KaynakKolonlar.TryGetValue(kaynakKolon, out var kInfo))
                {
                    row.Cells["Uygunluk"].Value = "Kaynak yok";
                    row.Cells["Uygunluk"].Style.ForeColor = Color.Red;
                    return;
                }

                if (!HedefKolonlar.TryGetValue(hedefKolon, out var hInfo))
                {
                    row.Cells["Uygunluk"].Value = "Hedef yok";
                    row.Cells["Uygunluk"].Style.ForeColor = Color.Red;
                    return;
                }

                // data type kontrol (basit eşitlik)
                if (!string.Equals(kInfo.DataType, hInfo.DataType, StringComparison.OrdinalIgnoreCase))
                {
                    row.Cells["Uygunluk"].Value = $"Uyumsuz tip ({kInfo.DataType}->{hInfo.DataType})";
                    row.Cells["Uygunluk"].Style.ForeColor = Color.Red;
                    return;
                }

                // nullable hedef NOT NULL ise hata
                if (!hInfo.IsNullable && kInfo.IsNullable)
                {
                    // kaynak nullable ama hedef not null
                    row.Cells["Uygunluk"].Value = "Hedef Null olamaz";
                    row.Cells["Uygunluk"].Style.ForeColor = Color.OrangeRed;
                    return;
                }

                // length kontrol (varsa)
                if (kInfo.Length.HasValue && hInfo.Length.HasValue && kInfo.Length > hInfo.Length)
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



        private List<(string KaynakKolon, string HedefKolon)> EslestirmeListesi()
        {
            var liste = new List<(string KaynakKolon, string HedefKolon)>();
            foreach (DataGridViewRow row in GrdEslestirme.Rows)
            {
                KontrolEt(row);
                if (row.IsNewRow) continue;
                if (row.Cells["Uygunluk"].Value?.ToString() != "Uygun") continue;

                string k = row.Cells["KaynakKolon"].Value?.ToString();
                string h = row.Cells["HedefKolon"].Value?.ToString();
                if (!string.IsNullOrWhiteSpace(k) && !string.IsNullOrWhiteSpace(h))
                {
                    liste.Add((k, h));
                }
            }
            return liste;
        }


        private void BtnGeri_Click(object sender, EventArgs e)
        {
            _oncekiForm.Show();
            this.Close();
        }





        private void BtnSutunYkle_Click(object sender, EventArgs e)
        {

            if (TrwKaynakTablolar.SelectedNode != null)
            {
                var n = TrwKaynakTablolar.SelectedNode.Tag?.ToString();
                if (!string.IsNullOrWhiteSpace(n)) TrwKaynakTablolar_AfterSelect(this, new TreeViewEventArgs(TrwKaynakTablolar.SelectedNode));
            }
            if (TrwHedefTablolar.SelectedNode != null)
            {
                var n = TrwHedefTablolar.SelectedNode.Tag?.ToString();
                if (!string.IsNullOrWhiteSpace(n)) TrwHedefTablolar_AfterSelect(this, new TreeViewEventArgs(TrwHedefTablolar.SelectedNode));
            }
        }

        private void BtnOtomatikEsle_Click(object sender, EventArgs e)// isim bazlı
        {
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
                string connStr = $"Server={kaynak.Sunucu};Database={kaynak.Veritabani};User Id={kaynak.Kullanici};Password={kaynak.Sifre};TrustServerCertificate=True;";

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


        private DataTable GetDataTable(string connStr, string tablo, List<string> kolonlar, string where = "")
        {
            string kolList = string.Join(", ", kolonlar.Select(c => $"[{c}]"));
            string sql = $"SELECT {kolList} FROM [{tablo}]";
            if (!string.IsNullOrWhiteSpace(where))
                sql += " WHERE " + where;

            var dt = new DataTable();
            using (var conn = new SqlConnection(connStr))
            using (var da = new SqlDataAdapter(sql, conn))
            {
                da.Fill(dt);
            }
            return dt;
        }


        private async Task ProgresBar(string connStr, string hedefTablo, DataTable dt, List<(string KaynakKolon, string HedefKolon)> eslesmeler)
        {
            using (var conn = new SqlConnection(connStr))
            using (var bulk = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, null))
            {
                await conn.OpenAsync();
                bulk.DestinationTableName = hedefTablo;
                bulk.BatchSize = 1000;

                foreach (var (kcol, hcol) in eslesmeler)
                {
                    if (dt.Columns.Contains(kcol))
                        bulk.ColumnMappings.Add(kcol, hcol);
                }

                // İlerleme eventi
                bulk.SqlRowsCopied += (s, e) =>
                {
                    int progress = (int)((e.RowsCopied / (double)dt.Rows.Count) * 100);
                    ProgresGuncelle(progress);
                };
                bulk.NotifyAfter = 500;

                await bulk.WriteToServerAsync(dt);
            }
        }

        private void ProgresGuncelle(int degisim)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<int>(ProgresGuncelle), degisim);
                return;
            }

            prgTransfer.Value = Math.Min(degisim, 100);
            LogEkle($"%{degisim} tamamlandı...");
        }

        private void LogEkle(string mesaj)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(LogEkle), mesaj);
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
                MessageBox.Show("Kaynak ve hedef tablo seçin.");
                return;
            }

            var eslesmeler = EslestirmeListesi();
            if (eslesmeler.Count == 0)
            {
                MessageBox.Show("Uygun eşleşme yok.");
                return;
            }

            string kaynakTablo = TrwKaynakTablolar.SelectedNode.Tag.ToString();
            string hedefTablo = TrwHedefTablolar.SelectedNode.Tag.ToString();

            BtnTransferBaslat.Enabled = false;
            prgTransfer.Value = 0;
            prgTransfer.Style = ProgressBarStyle.Continuous;
            LogEkle("Transfer başlıyor...");

            try
            {
                var kaynakKolonList = eslesmeler.Select(e2 => e2.KaynakKolon).ToList();
                string kaynakConnStr = ConnectionString(kaynak);
                string hedefConnStr = ConnectionString(hedef);

                // 1️⃣ Kaynak veriyi çek
                DataTable kaynakVeri = await Task.Run(() =>
                    GetDataTable(kaynakConnStr, kaynakTablo, kaynakKolonList, TxtFiltreleme.Text)
                );

                if (kaynakVeri == null || kaynakVeri.Rows.Count == 0)
                {
                    LogEkle("Kaynakta veri yok - transfer iptal.");
                    MessageBox.Show("Kaynakta veri yok.");
                    return;
                }

                // 2️⃣ Hedef veriyi çek
                var hedefKolonList = eslesmeler.Select(x => x.HedefKolon).ToList();
                DataTable hedefVeri = await Task.Run(() =>
                    GetDataTable(hedefConnStr, hedefTablo, hedefKolonList)
                );

                // 3️⃣ Yeni kayıtları bul
                var yeniDt = kaynakVeri.Clone();
                var hedefSet = new HashSet<string>(
                    hedefVeri.AsEnumerable().Select(r => string.Join("|", hedefKolonList.Select(c => r[c]?.ToString() ?? "")))
                );

                int total = kaynakVeri.Rows.Count;
                int processed = 0;

                foreach (DataRow kr in kaynakVeri.Rows)
                {
                    string key = string.Join("|", kaynakKolonList.Select(c => kr[c]?.ToString() ?? ""));
                    if (!hedefSet.Contains(key))
                        yeniDt.ImportRow(kr);

                    processed++;

                    if (processed % 100 == 0 || processed == total)
                    {
                        int progress = (int)((processed / (double)total) * 100);
                        ProgresGuncelle(progress);
                    }
                }

                if (yeniDt.Rows.Count == 0)
                {
                    LogEkle("Yeni kayıt yok.");
                    MessageBox.Show("Yeni aktarılacak kayıt yok.");
                    return;
                }


                await ProgresBar(hedefConnStr, hedefTablo, yeniDt, eslesmeler);

                LogEkle($"Transfer başarılı: {yeniDt.Rows.Count} satır aktarıldı.");
                MessageBox.Show($"Transfer tamamlandı. {yeniDt.Rows.Count} satır aktarıldı.");

            }
            catch (Exception ex)
            {
                LogEkle($"Transfer hatası: {ex.Message}");
                MessageBox.Show($"Transfer hatası: {ex.Message}");
            }
            finally
            {
                prgTransfer.Value = 100;
                BtnTransferBaslat.Enabled = true;

                await Task.Delay(500);
                prgTransfer.Value = 0;

            }
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
    }
}