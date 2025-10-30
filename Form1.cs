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
            Control.CheckForIllegalCrossThreadCalls = false;
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

            BaglantiTestAsync();//baglantı testi
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


                KaynakKolonlar = KolonBilgileriniGetir(server, db, table, user, pass); // kaynakkolonları kolonbilgilerini ile dolduruyorum

                if (!KaynakKolonlar.ContainsKey(sutun))//surun kontrolü yapıyorm
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
            string connstr = ConnOrtak(server, db, user, sifre); //5 adet sorgu iiçn


            using (conn = new SqlConnection(connstr))
            {
                conn.Open();
                string sql = @"SELECT COLUMN_NAME,DATA_TYPE,CHARACTER_MAXIMUM_LENGTH,IS_NULLABLE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME=@TableName ORDER BY ORDINAL_POSITION";

                using (cmd = new SqlCommand(sql, conn))// sorguyu çalıştırdım
                {
                    cmd.Parameters.AddWithValue("@TableName", table);
                    using (reader = cmd.ExecuteReader()) //veriyi okudum
                    {
                        while (reader.Read())
                        {
                            string colName = reader["COLUMN_NAME"].ToString();
                            string dataType = reader["DATA_TYPE"].ToString();
                            int? length = reader["CHARACTER_MAXIMUM_LENGTH"] == DBNull.Value ? null : Convert.ToInt32(reader["CHARACTER_MAXIMUM_LENGTH"]);//sadece string deger okur int datetimeda null gelir
                            bool isNullable = reader["IS_NULLABLE"].ToString() == "YES"; // eger yes ise true döner no ise false döner
                            kolonlar[colName] = (dataType, length, isNullable);
                        }
                    }
                }
            }
            return kolonlar;
        }

        //kolon içeriklerini görme su an kolonları görüntülüyor.
        private DataTable TabloVerileriGetir(string server, string db, string table, string sutun, string user, string sifre)
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
            string connStr = ConnOrtak(server, db, user, sifre);


            using (conn = new SqlConnection(connStr))
            {
                conn.Open();
                string sqlsorgu = $@"SELECT {sutun}  FROM {table}"; //select sutun from tablo seçtiğimşz tablo ve sutuna göre veriiler geliyor.
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
            string connStr = ConnOrtak(server, db, user, sifre);
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
            string connStr = ConnOrtak(server, db, user, sifre);
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

            // Nullable ve length kontrolleri
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
            if (string.IsNullOrWhiteSpace(server) ||
                string.IsNullOrWhiteSpace(db) ||
                string.IsNullOrWhiteSpace(table) ||
                string.IsNullOrWhiteSpace(user) ||
                string.IsNullOrWhiteSpace(sifre))
            {
                MessageBox.Show("Lütfen tüm bağlantı bilgilerini doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            // 2️⃣ Eşleştirme listesini al
            var eslestirmeler = EslestirmeListesi();
            if (eslestirmeler.Count == 0)
            {
                LstboxLog.Items.Add("HATA: Uygun eşleşme bulunamadı.");
                return null;
            }

            // 3️⃣ Sorguda kullanılacak kolon adlarını oluştur
            var kolonAdlari = eslestirmeler.Select(e => e.KaynakKolon).Distinct().ToList();
            string kolonListesi = string.Join(", ", kolonAdlari.Select(c => $"[{c}]"));

            // 4️⃣ Filtre koşulu (seçili satırlara göre)
            string whereKosulu = "";

            if (GrdKaynak.SelectedCells.Count > 0)
            {
                // Seçili hücrelerin kolon adını bul (ilk seçili hücrenin kolonuna göre işlem yapar)
                string kolonAdi = GrdKaynak.Columns[GrdKaynak.SelectedCells[0].ColumnIndex].Name;
                Type tip = GrdKaynak.Columns[GrdKaynak.SelectedCells[0].ColumnIndex].ValueType;

                // Seçilen tüm hücrelerdeki değerleri topla
                var secilenDegerler = new HashSet<string>();
                foreach (DataGridViewCell cell in GrdKaynak.SelectedCells)
                {
                    if (cell.Value == null || cell.Value == DBNull.Value)
                        continue;

                    object deger = cell.Value;
                    string filtreDeger = "";

                    if (tip == typeof(string) || tip == typeof(char))
                    {
                        filtreDeger = $"'{deger.ToString().Replace("'", "''")}'";
                    }
                    else if (tip == typeof(DateTime))
                    {
                        DateTime dtm = Convert.ToDateTime(deger);
                        filtreDeger = $"'{dtm:yyyy-MM-dd HH:mm:ss}'";
                    }
                    else if (tip == typeof(bool))
                    {
                        filtreDeger = (bool)deger ? "1" : "0";
                    }
                    else
                    {
                        // sayısal tiplerde tırnak yok
                        filtreDeger = deger.ToString().Replace(",", ".");
                    }

                    secilenDegerler.Add(filtreDeger);
                }

                // Eğer birden fazla değer varsa IN (...) şeklinde sorgu oluştur
                if (secilenDegerler.Count > 0)
                {
                    string filtre = string.Join(", ", secilenDegerler);
                    whereKosulu = $"WHERE [{kolonAdi}] IN ({filtre})";
                }
            }

            // 5️⃣ Sorguyu oluştur
            string sqlSorgu = $"SELECT {kolonListesi} FROM [{table}] {whereKosulu}";

            // 6️⃣ SQL'den veriyi çek
            DataTable dt = new DataTable();
            string connStr = ConnOrtak(server, db, user, sifre);

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                using (SqlDataAdapter dap = new SqlDataAdapter(sqlSorgu, conn))
                {
                    conn.Open();
                    dap.Fill(dt);
                }

                LstboxLog.Items.Add($"Kaynak tablodan {dt.Rows.Count} satır veri çekildi. (Sorgu: {whereKosulu})");
                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Veri çekme hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LstboxLog.Items.Add($"HATA: {ex.Message}");
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
                
                string KaynakServer = TxtboxKaynakSunucu.Text.Trim();
                string KaynakDB = CmbboxKaynakVeritabani.Text.Trim();
                string KaynakTable = CmbboxKaynaktablo.Text.Trim();
                string KaynakUser = TxtKullanıcı.Text.Trim();
                string KaynakPass = TxtSifre.Text.Trim();

                string HedefServer = TxtboxHedefSunucu.Text.Trim();
                string HedefDB = CmbboxHedefVeriTabani.Text.Trim();
                string HedefTable = CmbboxHedefTablo.Text.Trim();
                string HedefUser = TxboxHedefKullanici.Text.Trim();
                string HedefPass = TxboxHedefSifre.Text.Trim();

               
                DataTable kaynakVeri = await Task.Run(() => 
                TransferVerisiGetir(KaynakServer, KaynakDB, KaynakTable, KaynakUser, KaynakPass)); //veri çekme işlemi

                if (kaynakVeri == null || kaynakVeri.Rows.Count == 0)
                {
                    LstboxLog.Items.Add("HATA: Transfer edilecek veri bulunamadı veya çekilemedi.");
                    return;
                }

                
                string hedefConnStr = ConnOrtak(HedefServer, HedefDB, HedefUser, HedefPass);

                using (SqlConnection hedefConn = new SqlConnection(hedefConnStr))
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(hedefConn)) //toplu veri aktarımı için SqlBulkCopy kullanıyoruz yüksek performans
                {
                    await hedefConn.OpenAsync();
                    bulkCopy.DestinationTableName = HedefTable; //veriin hangi tabloya akrlacağını belirler

                    // Kolon eşleştirmesi
                    foreach (DataGridViewRow row in GrdEslestirme.Rows)//eslestirmedeki satırlar kontrol edilir
                    {
                        if (row.IsNewRow || row.Cells["Uygunluk"].Value?.ToString() != "Uygun")
                            continue;

                       
                        string kaynakAdi = row.Cells[KaynakSutun.Index].Tag?.ToString();
                        string hedefAdi = row.Cells[HedefSutun.Index].Tag?.ToString();

                        if (!string.IsNullOrEmpty(kaynakAdi) && !string.IsNullOrEmpty(hedefAdi))
                        {
                          
                            bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(kaynakAdi, hedefAdi));
                            LstboxLog.Items.Add($"Eşleme eklendi: {kaynakAdi} -> {hedefAdi}");
                        }
                    }

                  
                    if (bulkCopy.ColumnMappings.Count == 0)
                    {
                        throw new InvalidOperationException("Aktarım için geçerli kolon eşleştirmesi bulunamadı.");
                    }

                    
                    await bulkCopy.WriteToServerAsync(kaynakVeri);

                    LstboxLog.ForeColor = Color.Green;
                    LstboxLog.Items.Add($"BAŞARILI: {kaynakVeri.Rows.Count} satır veri başarıyla '{HedefTable}' tablosuna aktarıldı.");
                    MessageBox.Show("Veri transferi başarıyla tamamlandı!", "Başarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                LstboxLog.ForeColor = Color.Red;
                LstboxLog.Items.Add($"KRİTİK HATA: Veri transferi başarısız oldu: {ex.Message}");
                MessageBox.Show($"Veri transferi sırasında bir hata oluştu:\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                BtnTransferBaslat.Enabled = true;
                PrgsbarTransfer.Style = ProgressBarStyle.Blocks;
                PrgsbarTransfer.Visible = false;
                
            }
        }

        private void CkboxSifreGoster_CheckedChanged(object sender, EventArgs e)
        {
            if (CkboxSifreGoster.CheckState == CheckState.Checked)
            {
                TxtSifre.UseSystemPasswordChar = true;
                CkboxSifreGoster.Text = "Şifre Gizle";
            }
            else if (CkboxSifreGoster.CheckState == CheckState.Unchecked)
            {
                TxtSifre.UseSystemPasswordChar = false;
                CkboxSifreGoster.Text = "Şifre Göster";
            }
        }

        private void ChkboxSifre_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkboxHedefSifre.CheckState == CheckState.Checked)
            {
                TxboxHedefSifre.UseSystemPasswordChar = true;
                ChkboxHedefSifre.Text = "Şifre Gizle";
            }
            else if (ChkboxHedefSifre.CheckState == CheckState.Unchecked)
            {
                TxboxHedefSifre.UseSystemPasswordChar = false;
                ChkboxHedefSifre.Text = "Şifre Göster";
            }
        }

        private void BtnGrdTemizle_Click(object sender, EventArgs e)
        {
            GrdEslestirme.Rows.Clear();
        }
    }
}