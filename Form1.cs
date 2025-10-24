using Microsoft.Data.SqlClient;
using System.Data;


namespace DataTransfer
{
    public partial class FrmVeriEslestirme : Form
    {
        public FrmVeriEslestirme()
        {

            InitializeComponent();

            dbIcon = Properties.Resources.database;
            dbIcontable = Properties.Resources.table;

            //veritabanları
            CmbboxKaynakVeritabani.DrawMode = DrawMode.OwnerDrawFixed;
            CmbboxKaynakVeritabani.ItemHeight = 20;


            CmbboxKaynakVeritabani.DrawItem += CmbboxKaynakVeritabani_DrawItem;

            CmbboxHedefVeriTabani.DrawMode = DrawMode.OwnerDrawFixed;
            CmbboxHedefVeriTabani.ItemHeight = 20;
            CmbboxHedefVeriTabani.DrawItem += CmbboxHedefVeriTabani_DrawItem;

            //tablolar
            CmbboxKaynaktablo.DrawMode = DrawMode.OwnerDrawFixed;
            CmbboxKaynaktablo.ItemHeight = 22;
            CmbboxKaynaktablo.DrawItem += CmbboxKaynaktablo_DrawItem;

            CmbboxHedefTablo.DrawMode = DrawMode.OwnerDrawFixed;
            CmbboxHedefTablo.ItemHeight = 22;
            CmbboxHedefTablo.DrawItem += CmbboxHedefTablo_DrawItem;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BtnEslesmeDogrula.Enabled = false;
            BtnTransferBaslat.Enabled = false;
            BtnKynkKolonYukle.Enabled = false;
            BtnHedefKolonYukle.Enabled = false;
            GrdEslestirme.Enabled = false;
            PrgsbarTransfer.Visible = false;

        }


        SqlConnection conn;
        SqlCommand cmd;
        SqlDataAdapter dap;
        SqlDataReader reader;
        DataTable dt;
        Image dbIcon;
        Image dbIcontable;
        
        private void BtnBaglantiTest_Click(object sender, EventArgs e)
        {
            GrdEslestirme.Rows.Clear();

            LstboxLog.Items.Clear();
            if (string.IsNullOrWhiteSpace(TxtboxKaynakSunucu.Text) ||
                string.IsNullOrWhiteSpace(TxboxHedefKullanici.Text) ||
                string.IsNullOrWhiteSpace(TxboxHedefSifre.Text) ||
                string.IsNullOrWhiteSpace(TxtKullanıcı.Text) ||
                string.IsNullOrWhiteSpace(TxtSifre.Text) ||
                string.IsNullOrWhiteSpace(TxtboxHedefSunucu.Text))

            {
                MessageBox.Show("Lütfen tüm bağlantı bilgilerini doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            BaglantiTestAsync(); //baglantı testi
            KaynakVeriTabanıCombobox();//veritabanı combobox doldurma
            HedefVeriTabaniCombobox();//hedef veritabanı combobox doldurma


        }
        private async Task<bool> BaglantiTestAsync()
        {
            BtnBaglantiTest.Enabled = false;
            BtnBaglantiTest.Text = "Bağlantı Testi Yapılıyor...";

            string KaynakSorgu = $"Server={TxtboxKaynakSunucu.Text}; User Id={TxtKullanıcı.Text}; Password={TxtSifre.Text}; TrustServerCertificate=True;";
            string HedefSorgu = $"Server={TxtboxHedefSunucu.Text}; User Id={TxboxHedefKullanici.Text}; Password={TxboxHedefSifre.Text}; TrustServerCertificate=True;";

            try
            {
                using (var KaynakBaglanti = new SqlConnection(KaynakSorgu))
                using (var HedefBaglanti = new SqlConnection(HedefSorgu))
                {
                    await KaynakBaglanti.OpenAsync();
                    await HedefBaglanti.OpenAsync();

                    
                    LstboxLog.ForeColor = Color.Green;
                    LstboxLog.Items.Add($"Kaynak ve hedef bağlantıları başarıyla açıldı.");
                }

                
                BtnEslesmeDogrula.Enabled = true;
                BtnKynkKolonYukle.Enabled = true;
                BtnHedefKolonYukle.Enabled = true;
                GrdEslestirme.Enabled = true;

                MessageBox.Show("Bağlantı Oluşturuldu!");
                return true;
            }
            catch (Exception ex)
            {
                LstboxLog.ForeColor = Color.Red;
                LstboxLog.Items.Add($"[{DateTime.Now:HH:mm:ss}] Bağlantı başarısız: {ex.Message}");
                MessageBox.Show($"Bağlantı başarısız:\n{ex.Message}");
                return false;
            }
            finally
            {
                BtnBaglantiTest.Enabled = true;
                BtnBaglantiTest.Text = "Bağlantıyı Test Et";
            }
        }



        private void BtnKynkKolonYukle_Click(object sender, EventArgs e)
        {
            KaynakKolonYukle();

        }

        private bool KaynakKolonYukle()
        {
            try
            {
                string server = TxtboxKaynakSunucu.Text.Trim();
                string db = CmbboxKaynakVeritabani.Text.Trim();
                string table = CmbboxKaynaktablo.Text.Trim();
                string user = TxtKullanıcı.Text.Trim();
                string pass = TxtSifre.Text.Trim();
                string sutun = CmboxKaynakSutun.Text.Trim();

                
                if (string.IsNullOrWhiteSpace(server) ||
                    string.IsNullOrWhiteSpace(db) ||
                    string.IsNullOrWhiteSpace(table) ||
                    string.IsNullOrWhiteSpace(user) ||
                    string.IsNullOrWhiteSpace(pass) ||
                    string.IsNullOrWhiteSpace(sutun))
                {
                    MessageBox.Show("Lütfen tablo, sütun ve bağlantı bilgilerini eksiksiz girin.",
                                    "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                // Kolon bilgilerini dictionary içn dolduruyoruz
                KaynakKolonlar = KolonBilgileriniGetir(server, db, table, user, pass);

                if (!KaynakKolonlar.ContainsKey(sutun))
                {
                    MessageBox.Show($"Seçilen sütun '{sutun}' kaynak tabloda bulunamadı.",
                                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                // DataTable oluştur ve Grid'e bağla
                DataTable dt = TabloVerileriGetir(server, db, table, sutun, user, pass);
                GrdKaynak.Columns.Clear();
                GrdKaynak.DataSource = dt;


                foreach (DataGridViewColumn col in GrdKaynak.Columns)
                {
                    col.Tag = col.DataPropertyName;
                }


                LstboxLog.Items.Add($"Kaynak Tablosu '{table}' yüklendi. Kolonlar:");
                foreach (var kol in KaynakKolonlar.Keys)
                {
                    LstboxLog.Items.Add(kol);
                }

                return true;
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"SQL Hatası: {sqlEx.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Beklenmeyen Hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }


        }

    

        // kolon bilgilerini getiriyor 
        private Dictionary<string, (string DataType, int? Length, bool IsNullable)> KolonBilgileriniGetir(string server, string db, string table, string user, string sifre)
        {
            var kolonlar = new Dictionary<string, (string DataType, int? Length, bool IsNullable)>(); //boş bir sozluk olusturdum.
            string connstr = ConnOrtak(server,db,user,sifre); // 5 adet sorgu


            using (conn = new SqlConnection(connstr))
            {
                conn.Open();
                string sql = @"SELECT COLUMN_NAME,DATA_TYPE,CHARACTER_MAXIMUM_LENGTH,IS_NULLABLE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME=@TableName ORDER BY ORDINAL_POSITION";

                using (cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@TableName", table);
                    using (reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string colName = reader["COLUMN_NAME"].ToString();
                            string dataType = reader["DATA_TYPE"].ToString();
                            int? length = reader["CHARACTER_MAXIMUM_LENGTH"] == DBNull.Value ? null : Convert.ToInt32(reader["CHARACTER_MAXIMUM_LENGTH"]);//sadece string deger okur int datetimeda null gelir
                            bool isNullable = reader["IS_NULLABLE"].ToString() == "YES";
                            kolonlar[colName] = (dataType, length, isNullable);
                        }
                    }
                }
            }
            return kolonlar;
        }

        //kolon içeriklerini görme su an kolonları görüntülüyor.
        private DataTable TabloVerileriGetir(string server, string db, string table, string sutun,string user, string sifre)
        {
            if (string.IsNullOrWhiteSpace(server) ||
                string.IsNullOrWhiteSpace(db) ||
                string.IsNullOrWhiteSpace(table) ||
                string.IsNullOrWhiteSpace(sutun) ||
                string.IsNullOrWhiteSpace(user) ||
                string.IsNullOrWhiteSpace(sifre))
            {
                MessageBox.Show("Lütfen tüm bağlantı bilgilerini doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            dt = new DataTable();//bellek içerisinde tablo oluşturuyoruz sanal
            string connStr = ConnOrtak(server,db,user,sifre);


            using (conn = new SqlConnection(connStr))
            {
                conn.Open();
                string sqlsorgu = $@"SELECT {sutun}  FROM {table}";
                dap = new SqlDataAdapter(sqlsorgu, conn);
                dap.Fill(dt);
            }
            return dt;
        }



        private void BtnHedefKolonYukle_Click(object sender, EventArgs e)
        {
            HedefKolonYükle();
        }

        private bool HedefKolonYükle()
        {
            try
            {
                string server = TxtboxHedefSunucu.Text.Trim();
                string db = CmbboxHedefVeriTabani.Text.Trim();
                string table = CmbboxHedefTablo.Text.Trim();
                string user = TxboxHedefKullanici.Text.Trim();
                string pass = TxboxHedefSifre.Text.Trim();
                string sutun = CmboxHedefSutun.Text.Trim();

                // Boş alan kontrolü
                if (string.IsNullOrWhiteSpace(server) ||
                    string.IsNullOrWhiteSpace(db) ||
                    string.IsNullOrWhiteSpace(table) ||
                    string.IsNullOrWhiteSpace(user) ||
                    string.IsNullOrWhiteSpace(pass) ||
                    string.IsNullOrWhiteSpace(sutun))
                {
                    MessageBox.Show("Lütfen tablo, sütun ve bağlantı bilgilerini eksiksiz girin.",
                                    "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                // Kolon bilgilerini dictionary'e al
                HedefKolonlar = KolonBilgileriniGetir(server, db, table, user, pass);

                if (!HedefKolonlar.ContainsKey(sutun))
                {
                    MessageBox.Show($"Seçilen sütun '{sutun}' hedef tabloda bulunamadı.",
                                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                // DataTable oluştur ve Grid'e bağla
                DataTable dt = TabloVerileriGetir(server, db, table, sutun, user, pass);
                GrdHedef.Columns.Clear();
                GrdHedef.DataSource = dt;

                // Grid kolon tag'lerini ayarla (gerçek SQL kolon adı)
                foreach (DataGridViewColumn col in GrdHedef.Columns)
                {
                    col.Tag = col.DataPropertyName;
                }

                // Log ekle
                LstboxLog.Items.Add($"Hedef Tablosu '{table}' yüklendi. Kolonlar:");
                foreach (var kol in HedefKolonlar.Keys)
                {
                    LstboxLog.Items.Add(kol);
                }

                return true;
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"SQL Hatası: {sqlEx.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Beklenmeyen Hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        

        private void CmbboxKaynakVeritabani_SelectedIndexChanged(object sender, EventArgs e)
        {

            KaynakTabloDoldur();
        }


        //veritabanı combobox doldurma
        private void KaynakVeriTabanıCombobox()
        {
            string server = TxtboxKaynakSunucu.Text;
            string user = TxtKullanıcı.Text;
            string pass = TxtSifre.Text;
            if (string.IsNullOrWhiteSpace(server) ||
                string.IsNullOrWhiteSpace(user) ||
                string.IsNullOrWhiteSpace(pass))
            {
                MessageBox.Show("Lütfen sunucu, kullanıcı adı ve şifre bilgilerini doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string connStr =
                $"Server={server};" +
                $"Database=master;" +
                $"User Id={user};" +
                $"Password={pass};" +
                $"TrustServerCertificate=True;";

            try
            {
                using (conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string sql = "SELECT NAME FROM sys.databases ORDER BY name";
                    cmd = new SqlCommand(sql, conn);
                    reader = cmd.ExecuteReader();
                    CmbboxKaynakVeritabani.Items.Clear();
                    while (reader.Read())
                    {
                        CmbboxKaynakVeritabani.Items.Add(reader["name"].ToString());
                    }

                }

            }
            catch (Exception ex)
            {

                MessageBox.Show($"Hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LstboxLog.ForeColor = Color.Red;
                LstboxLog.Items.Add("Veritabanı yüklenemedi.");
                return;
            }
            finally
            {
                conn.Close();
            }

        }
        private void HedefVeriTabaniCombobox()
        {
            string server = TxtboxHedefSunucu.Text;
            string user = TxboxHedefKullanici.Text;
            string pass = TxboxHedefSifre.Text;
            if (string.IsNullOrWhiteSpace(server) ||
                string.IsNullOrWhiteSpace(user) ||
                string.IsNullOrWhiteSpace(pass))
            {
                MessageBox.Show("Lütfen sunucu, kullanıcı adı ve şifre bilgilerini doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string connStr =
                $"Server={server};" +
                $"Database=master;" +
                $"User Id={user};" +
                $"Password={pass};" +
                $"TrustServerCertificate=True;";
            try
            {
                using (conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string sql = "SELECT NAME FROM sys.databases ORDER BY name"; //sys.databases her veritabanı hakkında bilgiler tutar ***alfabetik sıraya göre orde by name***
                    cmd = new SqlCommand(sql, conn);
                    reader = cmd.ExecuteReader();
                    CmbboxHedefVeriTabani.Items.Clear();
                    while (reader.Read())
                    {
                        CmbboxHedefVeriTabani.Items.Add(reader["name"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LstboxLog.ForeColor = Color.Red;
                LstboxLog.Items.Add("Veritabanı yüklenemedi.");
                return;
            }
            finally
            {
                conn.Close();
            }
        }
        private void KaynakTabloDoldur()
        {
            string server = TxtboxKaynakSunucu.Text;
            string db = CmbboxKaynakVeritabani.Text;
            string user = TxtKullanıcı.Text;
            string sifre = TxtSifre.Text;
            if (string.IsNullOrWhiteSpace(server) ||
                string.IsNullOrWhiteSpace(db) ||
                string.IsNullOrWhiteSpace(user) ||
                string.IsNullOrWhiteSpace(sifre))
            {
                MessageBox.Show("Lütfen sunucu, veritabanı, kullanıcı adı ve şifre bilgilerini doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string connStr = ConnOrtak(server,db,user,sifre);
            try
            {
                using (conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string sql = "SELECT TABLE_NAME, TABLE_TYPE FROM INFORMATION_SCHEMA.TABLES ORDER BY TABLE_NAME";
                    cmd = new SqlCommand(sql, conn);
                    reader = cmd.ExecuteReader();
                    CmbboxKaynaktablo.Items.Clear();
                    while (reader.Read())
                    {
                        CmbboxKaynaktablo.Items.Add(reader["TABLE_NAME"].ToString());
                    }

                    


                }
                //MessageBox.Show("Tablolar başarıyla yüklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }

        }
        private void HedefTabloDoldur()
        {
            string server = TxtboxHedefSunucu.Text;
            string db = CmbboxHedefVeriTabani.Text;
            string user = TxboxHedefKullanici.Text;
            string sifre = TxboxHedefSifre.Text;
            if (string.IsNullOrWhiteSpace(server) ||
                string.IsNullOrWhiteSpace(db) ||
                string.IsNullOrWhiteSpace(user) ||
                string.IsNullOrWhiteSpace(sifre))
            {
                MessageBox.Show("Lütfen sunucu, veritabanı, kullanıcı adı ve şifre bilgilerini doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string connStr = ConnOrtak(server,db,user,sifre);
            try
            {
                using (conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string sql = "SELECT TABLE_NAME, TABLE_TYPE FROM INFORMATION_SCHEMA.TABLES ORDER BY TABLE_NAME";
                    cmd = new SqlCommand(sql, conn);
                    reader = cmd.ExecuteReader();
                    CmbboxHedefTablo.Items.Clear();
                    while (reader.Read())
                    {
                        CmbboxHedefTablo.Items.Add(reader["TABLE_NAME"].ToString());
                    }

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
        }
        private void KaynakSutunDoldur()
        {
            string server = TxtboxKaynakSunucu.Text;
            string db = CmbboxKaynakVeritabani.Text;
            string table = CmbboxKaynaktablo.Text;
            string user = TxtKullanıcı.Text;
            string sifre = TxtSifre.Text;
            if (string.IsNullOrWhiteSpace(server) ||
                string.IsNullOrWhiteSpace(db) ||
                string.IsNullOrWhiteSpace(table) ||
                string.IsNullOrWhiteSpace(user) ||
                string.IsNullOrWhiteSpace(sifre))
            {
                MessageBox.Show("Lütfen sunucu, veritabanı, tablo, kullanıcı adı ve şifre bilgilerini doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string connStr = ConnOrtak(server,db,user,sifre);
            try
            {
                using (conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string sql = @"SELECT COLUMN_NAME 
                       FROM INFORMATION_SCHEMA.COLUMNS 
                       WHERE TABLE_NAME = @TableName
                       ORDER BY ORDINAL_POSITION";

                    cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@TableName", table);
                    reader = cmd.ExecuteReader();
                    CmboxKaynakSutun.Items.Clear();
                    while (reader.Read())
                    {
                        CmboxKaynakSutun.Items.Add(reader["COLUMN_NAME"].ToString());
                    }


                }
                //LstboxLog.ForeColor = Color.Green;
                //LstboxLog.Items.Add("Kaynak Sütunlar başarıyla yüklendi.");
                //MessageBox.Show("Sütunlar başarıyla yüklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LstboxLog.ForeColor = Color.Red;
                LstboxLog.Items.Add("Sütunlar yüklenemedi.");
                return;
            }
            finally
            {
                conn.Close();
            }
        }
        private string ConnOrtak(string server,string db,string user,string sifre)
        {
            string connstr= $"Server={server};Database={db};User Id={user};Password={sifre};TrustServerCertificate=True;";
            return connstr;
        }
        private void HedefSutunDoldur()
        {
            string server = TxtboxHedefSunucu.Text;
            string db = CmbboxHedefVeriTabani.Text;
            string table = CmbboxHedefTablo.Text;
            string user = TxboxHedefKullanici.Text;
            string sifre = TxboxHedefSifre.Text;
            if (string.IsNullOrWhiteSpace(server) ||
                string.IsNullOrWhiteSpace(db) ||
                string.IsNullOrWhiteSpace(table) ||
                string.IsNullOrWhiteSpace(user) ||
                string.IsNullOrWhiteSpace(sifre))
            {
                MessageBox.Show("Lütfen sunucu, veritabanı, tablo, kullanıcı adı ve şifre bilgilerini doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string connStr = ConnOrtak(server,db,user,sifre);
            try
            {
                using (conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string sql = @"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME=@TableName ORDER BY ORDINAL_POSITION";
                    cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@TableName", table);
                    reader = cmd.ExecuteReader();
                    CmboxHedefSutun.Items.Clear();
                    while (reader.Read())
                    {
                        CmboxHedefSutun.Items.Add(reader["COLUMN_NAME"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LstboxLog.ForeColor = Color.Red;
                LstboxLog.Items.Add("Sütunlar yüklenemedi.");
                return;
            }
            finally
            {
                conn.Close();
            }
        }

        private void CmbboxKaynaktablo_SelectedIndexChanged(object sender, EventArgs e)
        {
            KaynakSutunDoldur();
        }

        private void CmbboxHedefTablo_SelectedIndexChanged(object sender, EventArgs e)
        {
            HedefSutunDoldur();
        }

        private void CmbboxHedefVeriTabani_SelectedIndexChanged(object sender, EventArgs e)
        {
            HedefTabloDoldur();
        }


        private int? AktifSatirIndex = null;
        private object? secilenKaynakDeger = null;

        // Kaynak hücre seçildiğinde
        private void GrdKaynak_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            secilenKaynakDeger = GrdKaynak.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

            int newRowIndex = GrdEslestirme.Rows.Add();
            GrdEslestirme.Rows[newRowIndex].Cells[KaynakSutun.Index].Value = secilenKaynakDeger;

            // SQL kolon adı olarak Tag’i kaydet
            GrdEslestirme.Rows[newRowIndex].Cells[KaynakSutun.Index].Tag =
                GrdKaynak.Columns[e.ColumnIndex].Tag ?? GrdKaynak.Columns[e.ColumnIndex].Name;

            AktifSatirIndex = newRowIndex;
            LstboxLog.Items.Add($"Eşleme için kaynak seçildi: {secilenKaynakDeger}");
        }

        // Hedef hücre seçildiğinde
        private void GrdHedef_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (!AktifSatirIndex.HasValue)
            {
                MessageBox.Show("Önce kaynak hücreyi seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var secilenHedefDeger = GrdHedef.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();
            if (string.IsNullOrEmpty(secilenHedefDeger))
                return;

            var row = GrdEslestirme.Rows[AktifSatirIndex.Value];
            row.Cells[HedefSutun.Index].Value = secilenHedefDeger;

            row.Cells[HedefSutun.Index].Tag =
                GrdHedef.Columns[e.ColumnIndex].Tag ?? GrdHedef.Columns[e.ColumnIndex].Name;

            // Kolon tip kontrolü
            KontrolEt(row);

            AktifSatirIndex = null;
            if (row.Cells["Uygunluk"].Value?.ToString() == "Uygun")
            {
                LstboxLog.Items.Add($"Eşleme tamamlandı: {secilenKaynakDeger} -> {secilenHedefDeger}");
            }
            else
            {
                LstboxLog.Items.Add("Eşleme tamamlanmadı");
            }
        }



        Dictionary<string, (string DataType, int? length, bool IsNullable)> KaynakKolonlar =
            new Dictionary<string, (string DataType, int? length, bool IsNullable)>(StringComparer.OrdinalIgnoreCase);
        Dictionary<string, (string DataType, int? length, bool IsNullable)> HedefKolonlar =
            new Dictionary<string, (string DataType, int? length, bool IsNullable)>(StringComparer.OrdinalIgnoreCase);



        private void KontrolEt(DataGridViewRow row)
        {
            string kaynakDeger = row.Cells[KaynakSutun.Index].Value?.ToString().Trim();
            string hedefDeger = row.Cells[HedefSutun.Index].Value?.ToString().Trim();

            if (string.IsNullOrEmpty(kaynakDeger) || string.IsNullOrEmpty(hedefDeger))
                return;

            
            string kaynakKolonAdi = row.Cells[KaynakSutun.Index].Tag?.ToString();
            string hedefKolonAdi = row.Cells[HedefSutun.Index].Tag?.ToString();

            if (string.IsNullOrEmpty(kaynakKolonAdi) || string.IsNullOrEmpty(hedefKolonAdi))
                return;

            if (!KaynakKolonlar.TryGetValue(kaynakKolonAdi, out var KaynakInfo))
            {
                LstboxLog.Items.Add($"UYARI: Kaynak kolon '{kaynakKolonAdi}' Dictionary’de bulunamadı!");
                row.Cells["Uygunluk"].Value = "Kaynak kolon yok";
                row.Cells["Uygunluk"].Style.ForeColor = Color.Red;
                return;
            }

            if (!HedefKolonlar.TryGetValue(hedefKolonAdi, out var HedefInfo))
            {
                LstboxLog.Items.Add($"UYARI: Hedef kolon '{hedefKolonAdi}' Dictionary’de bulunamadı!");
                row.Cells["Uygunluk"].Value = "Hedef kolon yok";
                row.Cells["Uygunluk"].Style.ForeColor = Color.Red;
                return;
            }

            // Tip kontrolü
            if (KaynakInfo.DataType != HedefInfo.DataType)
            {
                row.Cells["Uygunluk"].Value = "Uyumsuz tip";
                row.Cells["Uygunluk"].Style.ForeColor = Color.Red;
                LstboxLog.Items.Add($"Veri tipleri uyuşmuyor: Kaynak({KaynakInfo.DataType}) - Hedef({HedefInfo.DataType})");
                return;
            }

            // Nullable ve length kontrolleri
            if (!HedefInfo.IsNullable && KaynakInfo.IsNullable)
            {
                row.Cells["Uygunluk"].Value = "Hedef NULL olamaz!";
                row.Cells["Uygunluk"].Style.ForeColor = Color.OrangeRed;
                LstboxLog.Items.Add($"UYARI: {hedefKolonAdi} boş geçilemez ama {kaynakKolonAdi} NULL olabilir.");
                return;
            }

            if (HedefInfo.length.HasValue && KaynakInfo.length.HasValue && KaynakInfo.length > HedefInfo.length)
            {
                row.Cells["Uygunluk"].Value = "Uzunluk aşıyor.";
                row.Cells["Uygunluk"].Style.ForeColor = Color.Orange;
                LstboxLog.Items.Add($"UYARI: {kaynakKolonAdi} ({KaynakInfo.length}) hedef kolon ({HedefInfo.length}) boyutunu aşıyor");
                return;
            }

            // Her şey uyumlu
            row.Cells["Uygunluk"].Value = "Uygun";
            row.Cells["Uygunluk"].Style.ForeColor = Color.Green;
        }



        //silme işlemi
        private void GrdEslestirme_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            //silme islemi
            if (e.RowIndex >= 0 && GrdEslestirme.Columns[e.ColumnIndex].Name == "Sil")
            {
                object kaynak = GrdEslestirme.Rows[e.RowIndex].Cells[KaynakSutun.Index].Value;
                object hedef = GrdEslestirme.Rows[e.RowIndex].Cells[HedefSutun.Index].Value;

                if (kaynak == null && hedef == null)
                {
                    MessageBox.Show("Silinecek eşleşme bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult result = MessageBox.Show("Bu eşleşmeyi silmek istiyor musunuz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    GrdEslestirme.Rows.RemoveAt(e.RowIndex);
                }
            }
        }


        private void GrdEslestirme_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {

        }

        private void BtnEslesmeDogrula_Click(object sender, EventArgs e)
        {
            BtnTransferBaslat.Enabled = true;
        }


        private void GrdEslestirme_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return; // başlık veya geçersiz hücre varsa çık
            var row = GrdEslestirme.Rows[e.RowIndex];
            KontrolEt(row);
        }

        private void TxtSifre_TextChanged(object sender, EventArgs e)
        {
            TxtSifre.PasswordChar = '\u25CF';
        }

        private void TxboxHedefSifre_TextChanged(object sender, EventArgs e)
        {
            TxboxHedefSifre.PasswordChar = '\u25CF';
        }

        private void GrdEslestirme_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            var row = GrdEslestirme.Rows[e.RowIndex];
            KontrolEt(row);
        }

        private void CmbboxKaynakVeritabani_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
            {
                return;
            }
            e.DrawBackground();
            e.Graphics.DrawImage(dbIcon, e.Bounds.Left, e.Bounds.Top, 18, 18);

            string text = CmbboxKaynakVeritabani.Items[e.Index].ToString();
            e.Graphics.DrawString(text, e.Font, Brushes.Black, e.Bounds.Left + 25, e.Bounds.Top + 2);
            e.DrawFocusRectangle();
        }

        private void CmbboxHedefVeriTabani_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
            {
                return;
            }
            e.DrawBackground();
            e.Graphics.DrawImage(dbIcon, e.Bounds.Left, e.Bounds.Top, 18, 18);
            string text = CmbboxHedefVeriTabani.Items[e.Index].ToString();
            e.Graphics.DrawString(text, e.Font, Brushes.Black, e.Bounds.Left + 25, e.Bounds.Top + 2);
            e.DrawFocusRectangle();
        }

        private void CmbboxKaynaktablo_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
            {
                return;
            }
            e.DrawBackground();
            e.Graphics.DrawImage(dbIcontable, e.Bounds.Left, e.Bounds.Top, 18, 18);
            string text = CmbboxKaynaktablo.Items[e.Index].ToString();
            e.Graphics.DrawString(text, e.Font, Brushes.Black, e.Bounds.Left + 25, e.Bounds.Top + 1);
            e.DrawFocusRectangle();
        }

        private void CmbboxHedefTablo_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
            {
                return;
            }
            e.DrawBackground();
            e.Graphics.DrawImage(dbIcontable, e.Bounds.Left, e.Bounds.Top, 18, 18);
            string text = CmbboxHedefTablo.Items[e.Index].ToString();
            e.Graphics.DrawString(text, e.Font, Brushes.Black, e.Bounds.Left + 25, e.Bounds.Top + 1);
            e.DrawFocusRectangle();
        }

        private void GrdEslestirme_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (GrdEslestirme.IsCurrentCellDirty)
            {
                GrdEslestirme.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        //kaynkatan select ile oku hedefe insert olarak yaz
        private void BtnTransferBaslat_Click(object sender, EventArgs e)
        {
            try
            {
                string kaynakTablo = CmbboxKaynaktablo.Text.Trim();
                string hedefTablo = CmbboxHedefTablo.Text.Trim();

                if (string.IsNullOrWhiteSpace(kaynakTablo) || string.IsNullOrWhiteSpace(hedefTablo))
                {
                    MessageBox.Show("Lütfen kaynak ve hedef tablo seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Eşleştirmeleri al (sadece 'Uygun' olanlar)
                var eslesmeler = new List<(string KaynakKolon, string HedefKolon)>();
                foreach (DataGridViewRow row in GrdEslestirme.Rows)
                {
                    if (row.Cells["Uygunluk"].Value?.ToString() == "Uygun")
                    {
                        string kaynakKolon = row.Cells[KaynakSutun.Index].Tag?.ToString();
                        string hedefKolon = row.Cells[HedefSutun.Index].Tag?.ToString();

                        if (!string.IsNullOrEmpty(kaynakKolon) && !string.IsNullOrEmpty(hedefKolon))
                            eslesmeler.Add((kaynakKolon, hedefKolon));
                    }
                }

                if (eslesmeler.Count == 0)
                {
                    MessageBox.Show("Hiç uygun kolon eşleştirmesi bulunamadı!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Seçili satırları al
                if (GrdKaynak.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Lütfen transfer etmek için en az bir satır seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string safeHedefTablo = "[" + hedefTablo.Replace("]", "]]") + "]";

                using (SqlConnection connHedef = new SqlConnection(GetConnStrHedef()))
                {
                    connHedef.Open();
                    using (SqlTransaction tran = connHedef.BeginTransaction())
                    {
                        int toplamAktarilan = 0;

                        foreach (DataGridViewRow kaynakRow in GrdKaynak.SelectedRows)
                        {
                            string insertSql = $"INSERT INTO {safeHedefTablo} " +
                                $"({string.Join(", ", eslesmeler.Select(x => $"[{x.HedefKolon.Replace("]", "]]")}]"))}) " +
                                $"VALUES ({string.Join(", ", eslesmeler.Select((x, i) => "@p" + i))})";

                            using (SqlCommand cmd = new SqlCommand(insertSql, connHedef, tran))
                            {
                                for (int i = 0; i < eslesmeler.Count; i++)
                                {
                                    string kaynakKolon = eslesmeler[i].KaynakKolon;

                                    // 🔹 Kolon gerçekten gridde var mı kontrol et
                                    if (!GrdKaynak.Columns.Contains(kaynakKolon))
                                    {
                                        LstboxLog.Items.Add($"Uyarı: {kaynakKolon} kolonu kaynak gridde bulunamadı, atlandı.");
                                        cmd.Parameters.AddWithValue("@p" + i, DBNull.Value);
                                        continue;
                                    }

                                    object value = kaynakRow.Cells[kaynakKolon]?.Value ?? DBNull.Value;
                                    cmd.Parameters.AddWithValue("@p" + i, value);
                                }

                                toplamAktarilan += cmd.ExecuteNonQuery();
                            }
                        }


                        tran.Commit();
                        LstboxLog.Items.Add($"{toplamAktarilan} satır başarıyla transfer edildi.");
                        MessageBox.Show($"{toplamAktarilan} satır başarıyla transfer edildi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                LstboxLog.Items.Add($"SQL Hatası: {sqlEx.Message}");
                MessageBox.Show($"SQL Hatası:\n{sqlEx.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                LstboxLog.Items.Add($"Hata: {ex.Message}");
                MessageBox.Show($"Hata:\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private string GetConnStrKaynak()
        {
            return $"Server={TxtboxKaynakSunucu.Text};" +
                   $"Database={CmbboxKaynakVeritabani.Text};" +
                   $"User Id={TxtKullanıcı.Text};" +
                   $"Password={TxtSifre.Text};" +
                   "TrustServerCertificate=True;";
        }

      
        private string GetConnStrHedef()
        {
            return $"Server={TxtboxHedefSunucu.Text};" +
                   $"Database={CmbboxHedefVeriTabani.Text};" +
                   $"User Id={TxboxHedefKullanici.Text};" +
                   $"Password={TxboxHedefSifre.Text};" +
                   "TrustServerCertificate=True;";
        }


        private void CkboxSifreGoster_CheckedChanged(object sender, EventArgs e)
        {
            if (CkboxSifreGoster.Checked)
            {
                TxtSifre.PasswordChar = '\0';
            }
            else
            {
                TxtSifre.PasswordChar = '\u25CF';
            }
        }

        private void ChkboxSifre_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkboxHedefSifre.Checked)
            {
                TxboxHedefSifre.PasswordChar = '\0';
            }
            else
            {
                TxboxHedefSifre.PasswordChar = '\u25CF';
            }
        }
    }
}