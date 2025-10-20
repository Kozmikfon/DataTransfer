using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic.ApplicationServices;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing.Imaging;
using System.IO.MemoryMappedFiles;
using System.Net;
using System.Windows.Forms;
using System.Xml.Serialization;
using static System.ComponentModel.Design.ObjectSelectorEditor;

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

        }

        SqlConnection connHedef;
        SqlConnection connKaynak;
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

            BaglantiTestAsync(connHedef, connKaynak);//baglantı testi
            KaynakVeriTabanıCombobox();//veritabanı combobox doldurma
            HedefVeriTabaniCombobox();//hedef veritabanı combobox doldurma


        }
        private async void BaglantiTestAsync(SqlConnection connHedef, SqlConnection connKaynak)
        {

            BtnBaglantiTest.Enabled = false;
            BtnBaglantiTest.Text = "Bağlantı Testi Yapılıyor...";


            string kaynakConnection =
               $"Server={TxtboxKaynakSunucu.Text};" +
               $"User Id={TxtKullanıcı.Text};" +
               $"Password={TxtSifre.Text};" +
               $"TrustServerCertificate=True;";

            string hedefConnection =
               $"Server={TxtboxHedefSunucu.Text};" +
               $"User Id={TxboxHedefKullanici.Text};" +
               $"Password={TxboxHedefSifre.Text};" +
               $"TrustServerCertificate=True;";


            try
            {
                connHedef = new SqlConnection(hedefConnection);
                if (connHedef.State == ConnectionState.Closed)
                {
                    await connHedef.OpenAsync();
                }

                connKaynak = new SqlConnection(kaynakConnection);
                if (connKaynak.State == ConnectionState.Closed)
                {
                    await connKaynak.OpenAsync();
                }


                else
                {
                    MessageBox.Show("Bağlantı zaten açık.");
                }

                MessageBox.Show("Bağlantı Oluşturuldu!");

                if (connKaynak.State == ConnectionState.Open && connHedef.State == ConnectionState.Open)
                {
                    LstboxLog.ForeColor = Color.Green;
                    LstboxLog.Items.Add("Bağlantı başarılı şekilde oluştu.");
                }
                BtnEslesmeDogrula.Enabled = true;
                BtnKynkKolonYukle.Enabled = true;
                BtnHedefKolonYukle.Enabled = true;
                GrdEslestirme.Enabled = true;
               

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bağlantı başarısız:\n {ex.Message}");
                LstboxLog.ForeColor = Color.Red;
                LstboxLog.Items.Add("Bağlantı başarısız.");
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
                string server = TxtboxKaynakSunucu.Text;
                string db = CmbboxKaynakVeritabani.Text;
                string table = CmbboxKaynaktablo.Text;
                string user = TxtKullanıcı.Text;
                string pass = TxtSifre.Text;
                string sutun = CmboxKaynakSutun.Text;

                if (string.IsNullOrWhiteSpace(table) ||
                    (string.IsNullOrWhiteSpace(sutun)) ||
                    (string.IsNullOrWhiteSpace(server)) ||
                    (string.IsNullOrWhiteSpace(db)) ||
                    (string.IsNullOrWhiteSpace(user)) ||
                    (string.IsNullOrWhiteSpace(pass))
                    )
                {
                    MessageBox.Show("Lütfen tablo ve sütun adını girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }


                KaynakKolonlar = KolonBilgileriniGetir(server, db, table, user, pass); // bilgileri dictionary'e atıyorum
                dt = TabloVerileriGetir(server, db, table,sutun, user, pass); // datatable içerisinde sanal tablo oluşturup tablo kolonlarını alıyroum. satılar sutunlar
                GrdKaynak.Columns.Clear();
                GrdKaynak.DataSource = dt;

                foreach (DataGridViewColumn Kolon in GrdKaynak.Columns)
                {
                    Kolon.Tag = Kolon.Name;
                }
                LstboxLog.Items.Add($"Kaynak Tablosu: '{table}' yuklendi. Kolonlar:");
                foreach (var item in KaynakKolonlar.Keys)
                {
                    LstboxLog.Items.Add("" + item);
                }



            }
            catch (Exception ex)
            {

                MessageBox.Show($"Hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return true;


        }

        // kolonları listeleme metodu
        private List<string> KolonlarıGetir(string server, string db, string table, string sutun, string user, string pass)
        {

            if (string.IsNullOrWhiteSpace(server)
                || string.IsNullOrWhiteSpace(db)
                || string.IsNullOrWhiteSpace(table)
                || string.IsNullOrWhiteSpace(sutun)
                || string.IsNullOrWhiteSpace(user)
                || string.IsNullOrWhiteSpace(pass)
                )
            {
                MessageBox.Show("Lütfen tüm bağlantı bilgilerini doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            List<string> kolonlar = new List<string>();
            string connStr = $"Server={server};Database={db};User Id={user};Password={pass};TrustServerCertificate=True;";


            using (conn = new SqlConnection(connStr))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                string sql = @"SELECT COLUMN_NAME,DATA_TYPE 
                       FROM INFORMATION_SCHEMA.COLUMNS 
                       WHERE TABLE_NAME = @TableName AND COLUMN_NAME= @ColumnName";

                cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@TableName", table);
                cmd.Parameters.AddWithValue("@ColumnName", sutun);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        kolonlar.Add(reader["COLUMN_NAME"].ToString());

                    }
                }
            }
            return kolonlar;
        }

        // kolon bilgilerini getiriyor 
        private Dictionary<string, (string DataType, int? Length, bool IsNullable)> KolonBilgileriniGetir(string server, string db, string table, string user, string pass)
        {
            var kolonlar = new Dictionary<string, (string DataType, int? Length, bool IsNullable)>(); //boş bir sozluk olusturdum.
            string connstr = $"Server={server};Database={db};User Id={user};Password={pass};TrustServerCertificate=True;";
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
        private DataTable TabloVerileriGetir(string server, string db, string table, string sutun,string user, string password)
        {
            if (string.IsNullOrWhiteSpace(server) ||
                string.IsNullOrWhiteSpace(db) ||
                string.IsNullOrWhiteSpace(table) ||
                string.IsNullOrWhiteSpace(sutun) ||
                string.IsNullOrWhiteSpace(user) ||
                string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Lütfen tüm bağlantı bilgilerini doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            dt = new DataTable();//bellek içerisinde tablo oluşturuyoruz sanal
            string connStr = $"Server={server};Database={db};User Id={user};Password={password};TrustServerCertificate=True;";


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
                string server = TxtboxHedefSunucu.Text;
                string db = CmbboxHedefVeriTabani.Text;
                string table = CmbboxHedefTablo.Text;
                string sutun = CmboxHedefSutun.Text;
                string user = TxboxHedefKullanici.Text;
                string pass = TxboxHedefSifre.Text;

                if (string.IsNullOrWhiteSpace(table) ||
                    //string.IsNullOrWhiteSpace(sutun) ||
                    string.IsNullOrWhiteSpace(server) ||
                    string.IsNullOrWhiteSpace(db) ||
                    string.IsNullOrWhiteSpace(user) ||
                    string.IsNullOrWhiteSpace(pass)
                    )
                {
                    MessageBox.Show("Lütfen tablo ve sütun adını girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }


                HedefKolonlar = KolonBilgileriniGetir(server, db, table, user, pass);
                dt = TabloVerileriGetir(server, db, table, sutun,user, pass);
                GrdHedef.Columns.Clear();
                GrdHedef.DataSource = dt;
                foreach (DataGridViewColumn Kolon in GrdHedef.Columns)
                {
                    Kolon.Tag = Kolon.Name;
                }
                LstboxLog.Items.Add($"Hedef kolonlar:'{table}' yuklendi Kolonlar: ");
                foreach (var item in HedefKolonlar.Keys)
                {
                    LstboxLog.Items.Add(" " + item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata", ex.Message);
            }

            return true;
        }
        private List<(string KaynakKolon, string HedefKolon)> EslemeListesi()
        {
            var eslestirme = new List<(string, string)>();
            foreach (DataGridViewRow item in GrdEslestirme.Rows)
            {
                if (item.IsNewRow)
                {
                    continue;
                }
                var kaynak = item.Cells[KaynakSutun.Index].Value?.ToString().Trim();
                var hedef = item.Cells[HedefSutun.Index].Value?.ToString().Trim();
                if (!string.IsNullOrEmpty(kaynak) && !string.IsNullOrEmpty(hedef))
                {
                    eslestirme.Add((kaynak, hedef));

                }
            }
            return eslestirme;
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
            string pass = TxtSifre.Text;
            if (string.IsNullOrWhiteSpace(server) ||
                string.IsNullOrWhiteSpace(db) ||
                string.IsNullOrWhiteSpace(user) ||
                string.IsNullOrWhiteSpace(pass))
            {
                MessageBox.Show("Lütfen sunucu, veritabanı, kullanıcı adı ve şifre bilgilerini doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string connStr = $"Server={server};Database={db};User Id={user};Password={pass};TrustServerCertificate=True;";
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
            string pass = TxboxHedefSifre.Text;
            if (string.IsNullOrWhiteSpace(server) ||
                string.IsNullOrWhiteSpace(db) ||
                string.IsNullOrWhiteSpace(user) ||
                string.IsNullOrWhiteSpace(pass))
            {
                MessageBox.Show("Lütfen sunucu, veritabanı, kullanıcı adı ve şifre bilgilerini doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string connStr = $"Server={server};Database={db};User Id={user};Password={pass};TrustServerCertificate=True;";
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
            string pass = TxtSifre.Text;
            if (string.IsNullOrWhiteSpace(server) ||
                string.IsNullOrWhiteSpace(db) ||
                string.IsNullOrWhiteSpace(table) ||
                string.IsNullOrWhiteSpace(user) ||
                string.IsNullOrWhiteSpace(pass))
            {
                MessageBox.Show("Lütfen sunucu, veritabanı, tablo, kullanıcı adı ve şifre bilgilerini doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string connStr = $"Server={server};Database={db};User Id={user};Password={pass};TrustServerCertificate=True;";
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
        private void HedefSutunDoldur()
        {
            string server = TxtboxHedefSunucu.Text;
            string db = CmbboxHedefVeriTabani.Text;
            string table = CmbboxHedefTablo.Text;
            string user = TxboxHedefKullanici.Text;
            string pass = TxboxHedefSifre.Text;
            if (string.IsNullOrWhiteSpace(server) ||
                string.IsNullOrWhiteSpace(db) ||
                string.IsNullOrWhiteSpace(table) ||
                string.IsNullOrWhiteSpace(user) ||
                string.IsNullOrWhiteSpace(pass))
            {
                MessageBox.Show("Lütfen sunucu, veritabanı, tablo, kullanıcı adı ve şifre bilgilerini doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string connStr = $"Server={server};Database={db};User Id={user};Password={pass};TrustServerCertificate=True;";
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

        private void GrdKaynak_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            // Seçilen hücrenin değeri
            secilenKaynakDeger = GrdKaynak.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();
            //if (object.IsNullOrEmpty(secilenKaynakDeger))
            //    return;

            // Yeni eşleme satırı ekle
            int newRowIndex = GrdEslestirme.Rows.Add();
            GrdEslestirme.Rows[newRowIndex].Cells[KaynakSutun.Index].Value = secilenKaynakDeger;

            // Hücrenin ait olduğu kolon bilgisini gizli tut
            GrdEslestirme.Rows[newRowIndex].Cells[KaynakSutun.Index].Tag = GrdKaynak.Columns[e.ColumnIndex].Name;

            AktifSatirIndex = newRowIndex;
            LstboxLog.Items.Add($"Eşleme için kaynak veri seçildi: {secilenKaynakDeger}");
        }

        // Hedef hücreye tıklama
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

            // Hücrenin ait olduğu kolon bilgisini gizli tut
            row.Cells[HedefSutun.Index].Tag = GrdHedef.Columns[e.ColumnIndex].Name;

            // Kolon bilgileri ile kontroller
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




        // Kontrol fonksiyonu (artık veriye göre eşleme)
        // Kontrol metodu (güncel kolon tag’lerini kullanacak)
        private void KontrolEt(DataGridViewRow row)
        {
            string kaynakDeger = row.Cells[KaynakSutun.Index].Value?.ToString().Trim();
            string hedefDeger = row.Cells[HedefSutun.Index].Value?.ToString().Trim();

            if (string.IsNullOrEmpty(kaynakDeger) || string.IsNullOrEmpty(hedefDeger))
                return;

            // Kaynak kolon adını Tag'den al
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

        private void BtnTransferBaslat_Click(object sender, EventArgs e)
        {
            try
            {
                string kaynakServer = TxtboxKaynakSunucu.Text;
                string kaynakDb = CmbboxKaynakVeritabani.Text;
                string kaynakUser = TxtKullanıcı.Text;
                string kaynakPass = TxtSifre.Text;

                string hedefServer = TxtboxHedefSunucu.Text;
                string hedefDb = CmbboxHedefVeriTabani.Text;
                string hedefUser = TxboxHedefKullanici.Text;
                string hedefPass = TxboxHedefSifre.Text;

                string kaynakTablo = CmbboxKaynaktablo.Text;
                string hedefTablo = CmbboxHedefTablo.Text;

                if (string.IsNullOrWhiteSpace(kaynakTablo) || string.IsNullOrWhiteSpace(hedefTablo))
                {
                    MessageBox.Show("Lütfen kaynak ve hedef tablo seçin!");
                    return;
                }

                // Kolon eşleştirmeleri
                List<(string kaynakKolon, string hedefKolon)> eslesmeler = new List<(string, string)>();
                foreach (DataGridViewRow row in GrdEslestirme.Rows)
                {
                    if (row.Cells["KaynakSutun"].Value != null && row.Cells["HedefSutun"].Value != null)
                    {
                        string kaynak = row.Cells["KaynakSutun"].Value.ToString();
                        string hedef = row.Cells["HedefSutun"].Value.ToString();

                        // KontrolEt metoduyla uyumluluk kontrolü
                        KontrolEt(row);
                        if (row.Cells["Uygunluk"].Value?.ToString() == "Uygun")
                        {
                            eslesmeler.Add((kaynak, hedef));
                        }
                        else
                        {
                            LstboxLog.Items.Add($"Eşleme atlandı: {kaynak} -> {hedef} (Uygun değil)");
                        }
                    }
                }

                if (eslesmeler.Count == 0)
                {
                    MessageBox.Show("Hiç uygun kolon eşleştirmesi yok!");
                    return;
                }

                // Transfer edilecek veri: kullanıcı seçmiş olduğu hücre veya tüm tablo
                DataGridViewCell secilenHucre = GrdKaynak.CurrentCell;
                if (secilenHucre == null)
                {
                    MessageBox.Show("Lütfen transfer için bir hücre seçin!");
                    return;
                }

                object secilenDeger = secilenHucre.Value;
                string secilenKolon = GrdKaynak.Columns[secilenHucre.ColumnIndex].Name;

                // Kaynak ve hedef bağlantıları
                string connStrKaynak = $"Server={kaynakServer};Database={kaynakDb};User Id={kaynakUser};Password={kaynakPass};TrustServerCertificate=True;";
                string connStrHedef = $"Server={hedefServer};Database={hedefDb};User Id={hedefUser};Password={hedefPass};TrustServerCertificate=True;";

                using (SqlConnection connKaynak = new SqlConnection(connStrKaynak))
                using (SqlConnection connHedef = new SqlConnection(connStrHedef))
                {
                    connKaynak.Open();
                    connHedef.Open();

                    // Dinamik kolon listesi
                    string KaynakListesi = string.Join(",", eslesmeler.Select(x => $"[{x.kaynakKolon}]"));
                    string HedefListesi = string.Join(",", eslesmeler.Select(x => $"[{x.hedefKolon}]"));


                    // Parameter kullanarak veri bazlı insert
                    string sql = $"INSERT INTO {hedefTablo} ({HedefListesi}) " +
                                 $"SELECT {KaynakListesi} FROM {kaynakTablo} " +
                                 $"WHERE [{secilenKolon}] = @SecilenDeger";

                    using (SqlCommand cmd = new SqlCommand(sql, connHedef))
                    {
                        cmd.Parameters.AddWithValue("@SecilenDeger", secilenDeger ?? DBNull.Value);
                        int satirSayisi = cmd.ExecuteNonQuery();
                        MessageBox.Show($"{satirSayisi} satır transfer edildi.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}");
            }            
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