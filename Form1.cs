using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;


namespace DataTransfer
{
    public partial class FrmVeriEslestirme : Form
    {
        private BaglantiBilgileri KaynakBilgileri { get; set; }
        private BaglantiBilgileri HedefBilgileri { get; set; }

        private FrmBaglantiAc _oncekiForm;


        public FrmVeriEslestirme(BaglantiBilgileri kaynak, BaglantiBilgileri hedef,FrmBaglantiAc oncekiForm)
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
            KaynakBilgileri = kaynak;
            HedefBilgileri = hedef;
            _oncekiForm = oncekiForm;
            this.Load += Form1_Load;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            BtnEslesmeDogrula.Enabled = false;
            BtnTransferBaslat.Enabled = false;
            //BtnKynkKolonYukle.Enabled = false;
            //BtnHedefKolonYukle.Enabled = false;
            GrdEslestirme.Enabled = false;
            PrgsbarTransfer.Visible = false;

            await KaynakVeriTabanıCombobox(KaynakBilgileri);
            await HedefVeriTabaniCombobox(HedefBilgileri);

            CmbboxKaynakVeritabani.Text = KaynakBilgileri.Veritabani;
            CmbboxHedefVeriTabani.Text = HedefBilgileri.Veritabani;

           

        }


        SqlConnection conn;
        SqlCommand cmd;
        SqlDataAdapter dap;
        SqlDataReader reader;
        DataTable dt;
        Image dbIcon;
        Image dbIcontable;

        private void BtnKynkKolonYukle_Click(object sender, EventArgs e)
        {
            KaynakKolonYukle(KaynakBilgileri);

        }

        private async Task<bool> KaynakKolonYukle(BaglantiBilgileri baglantiBilgileri)
        {
            try
            {
                string server = baglantiBilgileri.Sunucu;
                string db = baglantiBilgileri.Veritabani;
                string table = CmbboxKaynaktablo.Text.Trim();


                string sutun = CmboxKaynakSutun.Text;

                if (string.IsNullOrWhiteSpace(server) ||
                    string.IsNullOrWhiteSpace(db) ||
                    string.IsNullOrWhiteSpace(table) ||

                    string.IsNullOrWhiteSpace(sutun))
                {
                    MessageBox.Show("Lütfen tablo, sütun ve bağlantı bilgilerini eksiksiz girin.",
                                    "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                // Kolon bilgilerini asenkron olarak çekiyoruz
                KaynakKolonlar = await KolonBilgileriniGetirAsync(KaynakBilgileri, table);

                if (KaynakKolonlar == null || !KaynakKolonlar.ContainsKey(sutun))
                {
                    MessageBox.Show($"Seçilen sütun '{sutun}' kaynak tabloda bulunamadı.",
                                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                // DataTable oluştur ve Grid'e bağla (asenkron şekilde)
                DataTable dt = await TabloVerileriGetirAsync(KaynakBilgileri, table, sutun); //baglantı tablo sutun

                if (dt == null)
                {
                    MessageBox.Show("Tablo verileri alınamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                // Grid'i doldur
                GrdKaynak.Columns.Clear();
                GrdKaynak.DataSource = dt;

                foreach (DataGridViewColumn col in GrdKaynak.Columns)
                    col.Tag = col.DataPropertyName;

                // Log kaydı
                LstboxLog.Items.Add($"Kaynak Tablosu '{table}' yüklendi. Kolonlar:");
                foreach (var kol in KaynakKolonlar.Keys)
                    LstboxLog.Items.Add(kol);

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
        private async Task<Dictionary<string, (string DataType, int? Length, bool IsNullable)>> KolonBilgileriniGetirAsync(BaglantiBilgileri baglanti, string tabloAdi)
        {
            var kolonlar = new Dictionary<string, (string DataType, int? Length, bool IsNullable)>(StringComparer.OrdinalIgnoreCase);

            if (baglanti == null ||
                string.IsNullOrWhiteSpace(baglanti.Sunucu) ||
                string.IsNullOrWhiteSpace(baglanti.Veritabani) ||
                string.IsNullOrWhiteSpace(baglanti.Kullanici) ||
                string.IsNullOrWhiteSpace(baglanti.Sifre) ||
                string.IsNullOrWhiteSpace(tabloAdi))
            {
                MessageBox.Show("Kolon bilgilerini almak için gerekli bilgiler eksik.",
                                "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return kolonlar;
            }

            string connStr = $"Server={baglanti.Sunucu};Database={baglanti.Veritabani};User Id={baglanti.Kullanici};Password={baglanti.Sifre};TrustServerCertificate=True;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    await conn.OpenAsync();

                    string sql = @"
                SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, IS_NULLABLE
                FROM INFORMATION_SCHEMA.COLUMNS
                WHERE TABLE_NAME = @TableName
                ORDER BY ORDINAL_POSITION";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@TableName", tabloAdi);

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                string colName = reader["COLUMN_NAME"].ToString();
                                string dataType = reader["DATA_TYPE"].ToString();

                                int? length = reader["CHARACTER_MAXIMUM_LENGTH"] == DBNull.Value
                                    ? null
                                    : Convert.ToInt32(reader["CHARACTER_MAXIMUM_LENGTH"]);

                                bool isNullable = reader["IS_NULLABLE"].ToString().Equals("YES", StringComparison.OrdinalIgnoreCase);

                                kolonlar[colName] = (dataType, length, isNullable);
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"SQL Hatası (KolonBilgileriniGetirAsync): {sqlEx.Message}",
                                "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Beklenmeyen hata (KolonBilgileriniGetirAsync): {ex.Message}",
                                "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return kolonlar;
        }


        //kolon içeriklerini görme su an kolonları görüntülüyor.
        private async Task<DataTable> TabloVerileriGetirAsync(BaglantiBilgileri baglanti, string tabloAdi, string sutunAdi)
        {
            if (baglanti == null ||
                string.IsNullOrWhiteSpace(baglanti.Sunucu) ||
                string.IsNullOrWhiteSpace(baglanti.Veritabani) ||
                string.IsNullOrWhiteSpace(baglanti.Kullanici) ||
                string.IsNullOrWhiteSpace(baglanti.Sifre) ||
                string.IsNullOrWhiteSpace(tabloAdi) ||
                string.IsNullOrWhiteSpace(sutunAdi))
            {
                MessageBox.Show("Lütfen tüm bağlantı ve tablo bilgilerini doldurun.",
                                "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            DataTable dt = new DataTable();

            string connStr = $"Server={baglanti.Sunucu};Database={baglanti.Veritabani};User Id={baglanti.Kullanici};Password={baglanti.Sifre};TrustServerCertificate=True;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    await conn.OpenAsync();

                    // Parametreli sorgu ile SQL Injection riski yok
                    string sqlSorgu = $@"SELECT [{sutunAdi}] FROM [{tabloAdi}]";

                    using (SqlCommand cmd = new SqlCommand(sqlSorgu, conn))
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Veri alınırken hata oluştu:\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return dt;
        }




        private void BtnHedefKolonYukle_Click(object sender, EventArgs e)
        {
            HedefKolonYükle(HedefBilgileri);
        }

        private async Task<bool> HedefKolonYükle(BaglantiBilgileri hedefBaglanti)
        {
            try
            {
                string db = hedefBaglanti.Veritabani;
                string table = CmbboxHedefTablo.Text.Trim();
                string sutun = CmboxHedefSutun.Text.Trim();

                // Boş alan kontrolü
                if (string.IsNullOrWhiteSpace(db) ||
                    string.IsNullOrWhiteSpace(table) ||
                    string.IsNullOrWhiteSpace(sutun))
                {
                    MessageBox.Show("Lütfen tablo, sütun ve bağlantı bilgilerini eksiksiz girin.",
                                    "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                // Kolon bilgilerini dictionary olarak al
                HedefKolonlar = await KolonBilgileriniGetirAsync(hedefBaglanti, table);

                if (!HedefKolonlar.ContainsKey(sutun))
                {
                    MessageBox.Show($"Seçilen sütun '{sutun}' hedef tabloda bulunamadı.",
                                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                // DataTable oluştur ve grid'e bağla
                DataTable dt = await TabloVerileriGetirAsync(hedefBaglanti, table, sutun);

                GrdHedef.Columns.Clear();
                GrdHedef.DataSource = dt;

                // Grid kolon tag'lerini ayarla (gerçek SQL kolon adı)
                foreach (DataGridViewColumn col in GrdHedef.Columns)
                {
                    col.Tag = col.DataPropertyName;
                }

                // Log ekle
                LstboxLog.Items.Add($"[{DateTime.Now:HH:mm:ss}] Hedef Tablo '{table}' yüklendi. Kolonlar:");
                foreach (var kol in HedefKolonlar.Keys)
                {
                    LstboxLog.Items.Add($"  - {kol}");
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



        private async void CmbboxKaynakVeritabani_SelectedIndexChanged(object sender, EventArgs e)
        {

            KaynakTabloDoldur(KaynakBilgileri);
            string selectedDb = CmbboxKaynakVeritabani.Text;
            if (string.IsNullOrWhiteSpace(selectedDb)) return;

            try
            {
                // Kullanıcının seçtiği veritabanına erişimi var mı kontrol et
                string connStr = $"Server={KaynakBilgileri.Sunucu};Database={selectedDb};User Id={KaynakBilgileri.Kullanici};Password={KaynakBilgileri.Sifre};Connect Timeout=5;TrustServerCertificate=True;";

                using (var conn = new SqlConnection(connStr))
                {
                    await conn.OpenAsync();

                    // Basit bir test sorgusu çalıştıralım
                    using (var cmd = new SqlCommand("SELECT TOP 1 name FROM sys.tables", conn))
                    {
                        await cmd.ExecuteScalarAsync();
                    }

                    LstboxLog.ForeColor = Color.Green;
                    LstboxLog.Items.Add($"[{DateTime.Now:HH:mm:ss}] '{selectedDb}' veritabanına erişim doğrulandı.");
                }
            }
            catch (SqlException ex)
            {
                string mesaj = ex.Number switch
                {
                    4060 => "Bu veritabanına erişim izniniz yok.",
                    18456 => "Kullanıcı adı veya şifre hatalı.",
                    229 => "Veritabanı nesnelerine erişim yetkiniz yok.",
                    _ => $"SQL Hatası ({ex.Number}): {ex.Message}"
                };

                LstboxLog.ForeColor = Color.Red;
                LstboxLog.Items.Add($"[{DateTime.Now:HH:mm:ss}] '{selectedDb}' erişim hatası: {mesaj}");
                MessageBox.Show(mesaj, "Erişim Engellendi", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Hatalı seçim durumunda geri al
                CmbboxKaynakVeritabani.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Beklenmeyen hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CmbboxKaynakVeritabani.SelectedIndex = -1;
            }


        }


        //veritabanı combobox doldurma
        private async Task KaynakVeriTabanıCombobox(BaglantiBilgileri baglantiBilgileri)
        {
            string server = baglantiBilgileri.Sunucu;
            string user = baglantiBilgileri.Kullanici;
            string pass = baglantiBilgileri.Sifre;
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
        private async Task HedefVeriTabaniCombobox(BaglantiBilgileri baglantiBilgileri)
        {
            string server = baglantiBilgileri.Sunucu;
            string user = baglantiBilgileri.Kullanici;
            string sifre = baglantiBilgileri.Sifre;
            
            string connStr =
                $"Server={server};" +
                $"Database=master;" +
                $"User Id={user};" +
                $"Password={sifre};" +
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
        private void KaynakTabloDoldur(BaglantiBilgileri baglantiBilgileri)
        {
            string server = baglantiBilgileri.Sunucu;
            string db = baglantiBilgileri.Veritabani;
            string user = baglantiBilgileri.Kullanici;
            string sifre = baglantiBilgileri.Sifre;
            //string sifre = TxtSifre.Text;
            if (
                string.IsNullOrWhiteSpace(db))


            {
                MessageBox.Show("Lütfen sunucu, veritabanı, kullanıcı adı ve şifre bilgilerini doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var conn = new SqlConnection(
                    $"Server={baglantiBilgileri.Sunucu};Database={baglantiBilgileri.Veritabani};User Id={baglantiBilgileri.Kullanici};Password={baglantiBilgileri.Sifre};TrustServerCertificate=True;"))
                {
                    conn.Open();
                    string sql = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE' ORDER BY TABLE_NAME";
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
        private void HedefTabloDoldur(BaglantiBilgileri baglantiBilgileri)
        {
            string server = baglantiBilgileri.Sunucu;
            string db = baglantiBilgileri.Veritabani;
            string user = baglantiBilgileri.Kullanici;
            string sifre = baglantiBilgileri.Sifre;
            //string db = CmbboxHedefVeriTabani.Text;

            if (
                string.IsNullOrWhiteSpace(db))

            {
                MessageBox.Show("Lütfen sunucu, veritabanı, kullanıcı adı ve şifre bilgilerini doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var conn = new SqlConnection(
                    $"Server={baglantiBilgileri.Sunucu};Database={baglantiBilgileri.Veritabani};User Id={baglantiBilgileri.Kullanici};Password={baglantiBilgileri.Sifre};TrustServerCertificate=True;"))
                {
                    conn.Open();
                    string sql = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE' ORDER BY TABLE_NAME";
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
        private void KaynakSutunDoldur(BaglantiBilgileri baglantiBilgileri)
        {
            string server = baglantiBilgileri.Sunucu;
            string db = baglantiBilgileri.Veritabani;
            string table = CmbboxKaynaktablo.Text;
            string user = baglantiBilgileri.Kullanici;
            string sifre = baglantiBilgileri.Sifre;
            if (string.IsNullOrWhiteSpace(server) ||
                string.IsNullOrWhiteSpace(db) ||
                string.IsNullOrWhiteSpace(table) ||
                string.IsNullOrWhiteSpace(user) ||
                string.IsNullOrWhiteSpace(sifre))
            {
                MessageBox.Show("Lütfen sunucu, veritabanı, tablo, kullanıcı adı ve şifre bilgilerini doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string connStr = ConnOrtak(server, db, user, sifre);
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
        private string ConnOrtak(string server, string db, string user, string sifre)
        {
            string connstr = $"Server={server};Database={db};User Id={user};Password={sifre};TrustServerCertificate=True;";
            return connstr;
        }
        private void HedefSutunDoldur(BaglantiBilgileri baglantiBilgileri)
        {
            string server = baglantiBilgileri.Sunucu;
            string db = baglantiBilgileri.Veritabani;
            string table = CmbboxHedefTablo.Text;
            string user = baglantiBilgileri.Kullanici;
            string sifre = baglantiBilgileri.Sifre;

            if (string.IsNullOrWhiteSpace(server) ||
                string.IsNullOrWhiteSpace(db) ||
                string.IsNullOrWhiteSpace(table) ||
                string.IsNullOrWhiteSpace(user) ||
                string.IsNullOrWhiteSpace(sifre))
            {
                MessageBox.Show("Lütfen sunucu, veritabanı, tablo, kullanıcı adı ve şifre bilgilerini doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //server,db,user,sifre
            string connStr = ConnOrtak(server, db, user, sifre);
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
            KaynakSutunDoldur(KaynakBilgileri);
        }

        private void CmbboxHedefTablo_SelectedIndexChanged(object sender, EventArgs e)
        {
            HedefSutunDoldur(HedefBilgileri);
        }

        private async void CmbboxHedefVeriTabani_SelectedIndexChanged(object sender, EventArgs e)
        {
            HedefTabloDoldur(HedefBilgileri);
            string selectedDb = CmbboxHedefVeriTabani.Text;
            if (string.IsNullOrWhiteSpace(selectedDb)) return;

            try
            {
                string connStr = $"Server={HedefBilgileri.Sunucu};Database={selectedDb};User Id={HedefBilgileri.Kullanici};Password={HedefBilgileri.Sifre};Connect Timeout=5;TrustServerCertificate=True;";
                using (var conn = new SqlConnection(connStr))
                {
                    await conn.OpenAsync();
                    using (var cmd = new SqlCommand("SELECT TOP 1 name FROM sys.tables", conn))
                    {
                        await cmd.ExecuteScalarAsync();
                    }

                    LstboxLog.ForeColor = Color.Green;
                    LstboxLog.Items.Add($"[{DateTime.Now:HH:mm:ss}] '{selectedDb}' veritabanına erişim doğrulandı.");
                }
            }
            catch (SqlException ex)
            {
                string mesaj = ex.Number switch
                {
                    4060 => "Bu veritabanına erişim izniniz yok.",
                    18456 => "Kullanıcı adı veya şifre hatalı.",
                    229 => "Veritabanı nesnelerine erişim yetkiniz yok.",
                    _ => $"SQL Hatası ({ex.Number}): {ex.Message}"
                };

                LstboxLog.ForeColor = Color.Red;
                LstboxLog.Items.Add($"[{DateTime.Now:HH:mm:ss}] '{selectedDb}' erişim hatası: {mesaj}");
                MessageBox.Show(mesaj, "Erişim Engellendi", MessageBoxButtons.OK, MessageBoxIcon.Error);

                CmbboxHedefVeriTabani.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Beklenmeyen hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CmbboxHedefVeriTabani.SelectedIndex = -1;
            }
        }


        private int? AktifSatirIndex = null;//grdkaynak ve hedeften seçtiğimiz satırın indeksini tutmak için bir sonraki adımda hedef kolonun seçeçği yeri gösterir
        private object? secilenKaynakDeger = null; //secilenkaynaktaki bilgileri tutumak object tğrü yerine farklı bir tür tercih edilmeli "runtimede yük" grdkaynaktaki seçilen hücrenin değerini tutmakn için

        // Kaynak hücre seçildiğinde
        private void GrdKaynak_CellClick(object sender, DataGridViewCellEventArgs e) //tıklanıldığında teetiklenen
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) //boş satır vvya sutun
                return;

            secilenKaynakDeger = GrdKaynak.Rows[e.RowIndex].Cells[e.ColumnIndex].Value; //tıklanılan hucrenin degeri secilenakaynakdegere atıldı.


            int YeniBosSatir = GrdEslestirme.Rows.Add(); //yeni boş satır olusturdum
            GrdEslestirme.Rows[YeniBosSatir].Cells[KaynakSutun.Index].Value = secilenKaynakDeger;//yenisatırda seçilen kaynaktaki değeri atadım. grdeslestirme deki kaynak sutununa secilenkaynakdegeri atadım.


            // verinin meta bilgisi saklanıyor "tag"
            GrdEslestirme.Rows[YeniBosSatir].Cells[KaynakSutun.Index].Tag = GrdKaynak.Columns[e.ColumnIndex].Tag ?? GrdKaynak.Columns[e.ColumnIndex].Name;


            AktifSatirIndex = YeniBosSatir; //kaynak seçildi hedef bekleniyor  durmuna geçidi
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

            var row = GrdEslestirme.Rows[AktifSatirIndex.Value]; //satırı indeksi aldım
            row.Cells[HedefSutun.Index].Value = secilenHedefDeger;//satır kolonuna seçilen hedef değeri atadım

            row.Cells[HedefSutun.Index].Tag = GrdHedef.Columns[e.ColumnIndex].Tag ?? GrdHedef.Columns[e.ColumnIndex].Name;//grdeslestirmedeki hedefsutun sutununa, grdhedefteki secilen verinin tagın atadım

            // Kolon tip kontrolü
            KontrolEt(row); //satır üzerinde kontrol etme işlemi hem satırt hem kontrol etme işlemi

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
            new Dictionary<string, (string DataType, int? length, bool IsNullable)>(StringComparer.OrdinalIgnoreCase);//kaynaktaki tüm kolonların istenilen bilgilerini tutacak sozluk

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


            if (!KaynakKolonlar.TryGetValue(kaynakKolonAdi, out var KaynakInfo)) //kaynakkolonadi burada anahtar olarak kulanılıyor. buradki kaynakınfo artık tip uzunluk ve karakter nullabilite bilgilerini tutuyor kaynakınfo tuple görevini almıştır
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



            // Tip kontrolünu 
            if (KaynakInfo.DataType != HedefInfo.DataType)
            {
                row.Cells["Uygunluk"].Value = "Uyumsuz tip";
                row.Cells["Uygunluk"].Style.ForeColor = Color.Red;
                LstboxLog.Items.Add($"Veri tipleri uyuşmuyor: Kaynak({KaynakInfo.DataType}) - Hedef({HedefInfo.DataType})");
                return;
            }

            // Nullable kontrolleri
            if (HedefInfo.IsNullable) // hedef not null ise boş geçilemez yes
            {
                row.Cells["Uygunluk"].Value = "boş geçilemez";
                row.Cells["Uygunluk"].Style.ForeColor = Color.OrangeRed;
                LstboxLog.Items.Add($"UYARI: {hedefKolonAdi} boş geçilemez");
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



        private List<(string KaynakKolon, string HedefKolon)> EslestirmeListesi()
        {
            var liste = new List<(string KaynakKolon, string HedefKolon)>();

            foreach (DataGridViewRow row in GrdEslestirme.Rows)
            {
                if (row.IsNewRow)
                    continue;

                if (row.Cells["Uygunluk"].Value?.ToString() != "Uygun")
                    continue;

                string kaynakKolon = row.Cells[KaynakSutun.Index].Tag?.ToString();
                string hedefKolon = row.Cells[HedefSutun.Index].Tag?.ToString();

                if (!string.IsNullOrEmpty(kaynakKolon) && !string.IsNullOrEmpty(hedefKolon))
                    liste.Add((kaynakKolon, hedefKolon));
            }

            return liste;
        }


        //kolon içeriklerini görme
        private DataTable TransferVerisiGetir(string server, string db, string table, string user, string sifre)
        {
            // 1️⃣ Bağlantı kontrolleri
            if (string.IsNullOrWhiteSpace(server) || string.IsNullOrWhiteSpace(db) ||
                string.IsNullOrWhiteSpace(table) || string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(sifre))
            {
                MessageBox.Show("Lütfen tüm bağlantı bilgilerini doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            // 2️⃣ Eşleştirme listesi
            var eslestirmeler = EslestirmeListesi();
            if (eslestirmeler.Count == 0)
            {
                LstboxLog.Items.Add("HATA: Uygun eşleşme bulunamadı.");
                return null;
            }

            var kolonAdlari = eslestirmeler.Select(e => e.KaynakKolon).Distinct().ToList();
            if (kolonAdlari.Count == 0)
            {
                LstboxLog.Items.Add("HATA: Eşleşen kaynak kolon bulunamadı.");
                return null;
            }

            string kolonListesi = string.Join(", ", kolonAdlari.Select(c => $"[{c}]"));

            // 3️⃣ Satır bazlı seçim
            bool seciliSatirVar = GrdKaynak.SelectedRows.Count > 0 || GrdKaynak.SelectedCells.Count > 0;
            var seciliRowIndexes = new HashSet<int>();

            if (seciliSatirVar)
            {
                foreach (DataGridViewRow row in GrdKaynak.SelectedRows)
                    seciliRowIndexes.Add(row.Index);

                foreach (DataGridViewCell cell in GrdKaynak.SelectedCells)
                    seciliRowIndexes.Add(cell.RowIndex);

                // RowCount dışındakileri temizle
                seciliRowIndexes.RemoveWhere(i => i < 0 || i >= GrdKaynak.Rows.Count);
            }

            // Grid kolon eşleştirme (kaynak kolon -> grid index)
            var gridColumnMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < GrdKaynak.Columns.Count; i++)
            {
                var col = GrdKaynak.Columns[i];
                if (!string.IsNullOrEmpty(col.DataPropertyName) && kolonAdlari.Contains(col.DataPropertyName, StringComparer.OrdinalIgnoreCase))
                    gridColumnMap[col.DataPropertyName] = i;
                else if (!string.IsNullOrEmpty(col.Name) && kolonAdlari.Contains(col.Name, StringComparer.OrdinalIgnoreCase))
                    gridColumnMap[col.Name] = i;
            }

            if (gridColumnMap.Count == 0)
            {
                MessageBox.Show("Kaynak kolonlar Grid'de bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            // Satır filtreleri (opsiyonel)
            var satirKosullari = new List<string>();
            if (seciliSatirVar)
            {
                foreach (int rowIndex in seciliRowIndexes)
                {
                    var gridRow = GrdKaynak.Rows[rowIndex];
                    if (gridRow.IsNewRow) continue;

                    var parcaKosullar = new List<string>();
                    foreach (var kaynakKolon in kolonAdlari)
                    {
                        if (!gridColumnMap.TryGetValue(kaynakKolon, out int colIndex))
                            continue;

                        object val = gridRow.Cells[colIndex].Value;
                        if (val == null || val == DBNull.Value)
                        {
                            parcaKosullar.Add($"[{kaynakKolon}] IS NULL");
                            continue;
                        }

                        Type tip = val.GetType();
                        string literal;
                        if (tip == typeof(string) || tip == typeof(char))
                            literal = $"'{val.ToString().Replace("'", "''")}'";
                        else if (tip == typeof(DateTime))
                            literal = $"'{Convert.ToDateTime(val):yyyy-MM-dd HH:mm:ss}'";
                        else if (tip == typeof(bool))
                            literal = (bool)val ? "1" : "0";
                        else
                            literal = val.ToString().Replace(",", ".");

                        parcaKosullar.Add($"[{kaynakKolon}] = {literal}");
                    }

                    if (parcaKosullar.Count > 0)
                        satirKosullari.Add("(" + string.Join(" AND ", parcaKosullar) + ")");
                }
            }

            // 6️⃣ WHERE cümlesi
            string whereKosulu = seciliSatirVar && satirKosullari.Count > 0
                ? "WHERE " + string.Join(" OR ", satirKosullari)
                : ""; // seçim yoksa tüm tablo

            // 7️⃣ Son SQL
            string sqlSorgu = $"SELECT {kolonListesi} FROM [{table}] {whereKosulu}";
            LstboxLog.Items.Add($"Çalıştırılan sorgu: {sqlSorgu}");

            // 8️⃣ Data çek
            var dt = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnOrtak(server, db, user, sifre)))
                using (SqlDataAdapter dap = new SqlDataAdapter(sqlSorgu, conn))
                {
                    conn.Open();
                    dap.Fill(dt);
                }

                LstboxLog.Items.Add($"Kaynak tablodan {dt.Rows.Count} satır veri çekildi.");
                return dt;
            }
            catch (Exception ex)
            {
                LstboxLog.Items.Add($"HATA: {ex.Message}");
                MessageBox.Show($"Veri çekme hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }












        private async void BtnTransferBaslat_Click(object sender, EventArgs e)
        {
            BtnTransferBaslat.Enabled = false;
            PrgsbarTransfer.Visible = true;
            PrgsbarTransfer.Style = ProgressBarStyle.Marquee;
            LstboxLog.Items.Add("Veri transferi başlatılıyor...");

            try
            {
                // 1️⃣ Kaynak ve hedef bilgileri
                string KaynakServer = KaynakBilgileri.Sunucu;
                string KaynakDB = CmbboxKaynakVeritabani.Text.Trim();
                string KaynakTable = CmbboxKaynaktablo.Text.Trim();
                string KaynakUser =KaynakBilgileri.Kullanici;
                string KaynakPass = KaynakBilgileri.Sifre;

                string HedefServer = HedefBilgileri.Sunucu;
                string HedefDB = CmbboxHedefVeriTabani.Text.Trim();
                string HedefTable = CmbboxHedefTablo.Text.Trim();
                string HedefUser = HedefBilgileri.Kullanici;
                string HedefPass = HedefBilgileri.Sifre;

                // 2️⃣ Eşleştirme listesi
                var eslestirmeler = EslestirmeListesi();
                if (eslestirmeler == null || eslestirmeler.Count == 0)
                {
                    MessageBox.Show("Aktarım için uygun kolon eşleştirmesi bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 3️⃣ Kaynak veriyi çek
                DataTable kaynakVeri = await Task.Run(() =>
                    TransferVerisiGetir(KaynakServer, KaynakDB, KaynakTable, KaynakUser, KaynakPass));

                if (kaynakVeri == null || kaynakVeri.Rows.Count == 0)
                {
                    LstboxLog.Items.Add("HATA: Transfer edilecek veri bulunamadı veya çekilemedi.");
                    MessageBox.Show("Transfer edilecek veri bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                //  Önizleme formu
                using (FrmVeriOnizleme frm = new FrmVeriOnizleme(kaynakVeri, KaynakTable))
                {
                    frm.ShowDialog();
                    if (!frm.Onaylandi)
                    {
                        LstboxLog.Items.Add("Kullanıcı transfer işlemini iptal etti.");
                        MessageBox.Show("Veri transferi iptal edildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                //  Hedef tabloya bağlan
                string hedefConnStr = ConnOrtak(HedefServer, HedefDB, HedefUser, HedefPass);
                DataTable hedefVeri = new DataTable();

                using (SqlConnection hedefConn = new SqlConnection(hedefConnStr))
                {
                    await hedefConn.OpenAsync();

                    //  Hedef tablo mevcut kayıtları çek (mükerrer kontrolü için)
                    string hedefKolonListesi = string.Join(", ", eslestirmeler.Select(e => $"[{e.HedefKolon}]"));
                    string sqlHedef = $"SELECT {hedefKolonListesi} FROM [{HedefTable}]";
                    using (SqlDataAdapter dap = new SqlDataAdapter(sqlHedef, hedefConn))
                    {
                        dap.Fill(hedefVeri);
                    }
                }

                //  Kaynak ile hedefi karşılaştır, sadece yeni kayıtları al mükerrer kayıt kontrolü
                DataTable yeniKaynakVeri = kaynakVeri.Clone(); // kolon yapısını koru
                foreach (DataRow kaynakRow in kaynakVeri.Rows)
                {
                    bool kayitVarMi = false;

                    foreach (DataRow hedefRow in hedefVeri.Rows)
                    {
                        bool ayniMi = true;
                        for (int i = 0; i < eslestirmeler.Count; i++)
                        {
                            var kaynakVal = kaynakRow[eslestirmeler[i].KaynakKolon];
                            var hedefVal = hedefRow[eslestirmeler[i].HedefKolon];
                            if (!object.Equals(kaynakVal, hedefVal))
                            {
                                ayniMi = false;
                                break;
                            }
                        }
                        if (ayniMi)
                        {
                            kayitVarMi = true;
                            break;
                        }
                    }

                    if (!kayitVarMi)
                        yeniKaynakVeri.Rows.Add(kaynakRow.ItemArray);
                }

                if (yeniKaynakVeri.Rows.Count == 0)
                {
                    LstboxLog.Items.Add("Hedef tabloda zaten mevcut tüm kayıtlar atlandı. Transfer yapılmadı.");
                    MessageBox.Show("Transfer edilecek yeni kayıt bulunamadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 8️⃣ BulkCopy ile yeni verileri aktar
                using (SqlConnection hedefConn = new SqlConnection(hedefConnStr))
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(hedefConn))
                {
                    await hedefConn.OpenAsync();
                    bulkCopy.DestinationTableName = HedefTable;

                    bulkCopy.ColumnMappings.Clear();
                    foreach (var (kaynakKolon, hedefKolon) in eslestirmeler)
                    {
                        if (!yeniKaynakVeri.Columns.Contains(kaynakKolon)) continue;
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(kaynakKolon, hedefKolon));
                    }

                    await bulkCopy.WriteToServerAsync(yeniKaynakVeri);
                }

                LstboxLog.ForeColor = Color.Green;
                LstboxLog.Items.Add($"BAŞARILI: {yeniKaynakVeri.Rows.Count} satır '{HedefTable}' tablosuna aktarıldı.");
                MessageBox.Show("Veri transferi başarıyla tamamlandı!", "Başarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                LstboxLog.ForeColor = Color.Red;
                LstboxLog.Items.Add($"KRİTİK HATA: {ex.Message}");
                MessageBox.Show($"Veri transferi sırasında bir hata oluştu:\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                BtnTransferBaslat.Enabled = true;
                PrgsbarTransfer.Style = ProgressBarStyle.Blocks;
                PrgsbarTransfer.Visible = false;
                LstboxLog.ForeColor = Color.Black;
            }
        }

      

        private void BtnGrdTemizle_Click(object sender, EventArgs e)
        {
            GrdEslestirme.Rows.Clear();
        }

        private void BtnGeriBaglanti_Click(object sender, EventArgs e)
        {
            _oncekiForm.Show();
            this.Close();
        }

        private void GrbboxButon_Enter(object sender, EventArgs e)
        {

        }

        private void FrmVeriEslestirme_Load(object sender, EventArgs e)
        {

        }

        private void GrbboxKaynak_Enter(object sender, EventArgs e)
        {

        }
    }
}