using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic.ApplicationServices;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing.Imaging;
using System.Xml.Serialization;

namespace DataTransfer
{
    public partial class FrmVeriEslestirme : Form
    {
        public FrmVeriEslestirme()
        {

            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BtnEslesmeDogrula.Enabled = false;
            BtnTransferBaslat.Enabled = false;
            BtnKynkKolonYukle.Enabled = false;
            BtnHedefKolonYukle.Enabled = false;
            GrdEslestirme.Enabled=false;

        }

        SqlConnection connHedef;
        SqlConnection connKaynak;
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataAdapter dap;
        SqlDataReader reader;
        DataTable dt;

        private void BtnBaglantiTest_Click(object sender, EventArgs e)
        {
            GrdEslestirme.Rows.Clear();
            //CmbboxKaynakVeritabani.Items.Clear();
            //CmbboxHedefVeriTabani.Items.Clear();
            //CmbboxKaynaktablo.Items.Clear();
            //CmbboxHedefTablo.Items.Clear(); 
            //CmboxKaynakSutun.Items.Clear();
            //CmboxHedefSutun.Items.Clear();


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

            TestConnectionAsync(connHedef,connKaynak);//baglantı testi
            KaynakVeriTabanıCombobox();//veritabanı combobox doldurma
            HedefVeriTabaniCombobox();//hedef veritabanı combobox doldurma


        }
        private async void TestConnectionAsync(SqlConnection connHedef, SqlConnection connKaynak)
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
                BtnTransferBaslat.Enabled = true;
                BtnKynkKolonYukle.Enabled = true;
                BtnHedefKolonYukle.Enabled = true;
                GrdEslestirme.Enabled = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bağlantı başarısız:\n{ex.Message}");
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
                    return;
                }


                //KolonYukle(server, db, table, sutun, user, pass);
                List<string> columns = KolonlarıGetir(server, db, table, sutun, user, pass);
                dt = TabloVerileriGetir(server, db, table, sutun, user, pass);
                DataGridViewTextBoxColumn colSelect = new DataGridViewTextBoxColumn();
                colSelect.HeaderText = "Hedef Kolonlar";
                colSelect.ReadOnly = true;


                GrdKaynak.Columns.Add(colSelect);
                DataRow row = dt.NewRow();
                dt.Rows.Add(row);
                GrdKaynak.Columns.Clear();
                GrdKaynak.DataSource = dt;
               
            }
            catch (Exception ex)
            {
                
                MessageBox.Show($"Hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

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

            List<string> columns = new List<string>();
            string connStr = $"Server={server};Database={db};User Id={user};Password={pass};TrustServerCertificate=True;";
            
           
            using (conn = new SqlConnection(connStr))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                string sql = @"SELECT COLUMN_NAME 
                       FROM INFORMATION_SCHEMA.COLUMNS 
                       WHERE TABLE_NAME = @TableName AND COLUMN_NAME= @ColumnName";

                cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@TableName", table);
                cmd.Parameters.AddWithValue("@ColumnName", sutun);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        columns.Add(reader["COLUMN_NAME"].ToString());
                    }
                }
            }
              return columns;
        }
        //kolon içeriklerini görme
        private DataTable TabloVerileriGetir(string server, string db, string table, string sutun, string user, string password)
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


            using (conn=new SqlConnection(connStr))
            {
                conn.Open();
                string sqlsorgu = $" SELECT [{sutun}] FROM [{table}]";
                dap= new SqlDataAdapter(sqlsorgu, conn);
                dap.Fill(dt);
                
            }
            return dt;
        }



        private void BtnHedefKolonYukle_Click(object sender, EventArgs e)
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
                    string.IsNullOrWhiteSpace(sutun) ||
                    string.IsNullOrWhiteSpace(server) ||
                    string.IsNullOrWhiteSpace(db) ||
                    string.IsNullOrWhiteSpace(user) ||
                    string.IsNullOrWhiteSpace(pass)
                    )
                {
                    MessageBox.Show("Lütfen tablo ve sütun adını girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }



                //KolonYukle(server, db, table, sutun, user, pass);
                List<string> columns = KolonlarıGetir(server, db, table, sutun, user, pass);
                dt = TabloVerileriGetir(server, db, table, sutun, user, pass);
                DataGridViewTextBoxColumn colSelect = new DataGridViewTextBoxColumn();
                colSelect.HeaderText = "Hedef Kolonlar";
                colSelect.ReadOnly = true;


                GrdHedef.Columns.Add(colSelect);
                DataRow row = dt.NewRow();
                dt.Rows.Add(row);
                GrdHedef.Columns.Clear();
                GrdHedef.DataSource = dt;

            }
            catch (Exception ex)
            {

                MessageBox.Show("Hata", ex.Message);
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
                //LstboxLog.ForeColor = Color.Green;
                //LstboxLog.Items.Add("Kaynak Veritabanları başarıyla yüklendi.");
                //tablo combobox doldurma
                //MessageBox.Show("Veritabanları başarıyla yüklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    string sql = "SELECT NAME FROM sys.databases ORDER BY name";
                    cmd = new SqlCommand(sql, conn);
                    reader = cmd.ExecuteReader();
                    CmbboxHedefVeriTabani.Items.Clear();
                    while (reader.Read())
                    {
                        CmbboxHedefVeriTabani.Items.Add(reader["name"].ToString());
                    }

                }
                //LstboxLog.ForeColor = Color.Green;
                //LstboxLog.Items.Add(" Hedef Veritabanları başarıyla yüklendi.");
                //tablo combobox doldurma
                //MessageBox.Show("Veritabanları başarıyla yüklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    string sql = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' ORDER BY TABLE_NAME";
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
                using (conn=new SqlConnection(connStr))
                {
                    conn.Open();
                    string sql = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' ORDER BY TABLE_NAME";
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
                using (conn=new SqlConnection(connStr))
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
                //LstboxLog.ForeColor = Color.Green;
                //LstboxLog.Items.Add("Hedef Sütunlar başarıyla yüklendi.");
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
        //private void IsNullableKOntrolEt()
        //{
            
        //    if (conn.State==ConnectionState.Closed)
        //    {

        //        conn.Open(); 
        //        string sql= @"SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, IS_NULLABLE
        //                    FROM INFORMATION_SCHEMA.COLUMNS
        //                    WHERE TABLE_NAME = @TableName";
        //        cmd = new SqlCommand(sql, conn);
        //        cmd.Parameters.AddWithValue("@Table_Name",table);
        //    }
            
        //}



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
        private object secilenKaynakDeger = null;

        private void GrdKaynak_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {

                secilenKaynakDeger = GrdKaynak.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                int newRowIndex = GrdEslestirme.Rows.Add();

                GrdEslestirme.Rows[newRowIndex].Cells[KaynakSutun.Index].Value = secilenKaynakDeger;
                AktifSatirIndex = newRowIndex; //satırı kaydet
                LstboxLog.Items.Add("kaynak" + secilenKaynakDeger.GetType());
            }
        }

        private void GrdHedef_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!AktifSatirIndex.HasValue)
            {
                MessageBox.Show("Önce Kaynak değeri seçin", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && AktifSatirIndex.HasValue)
            {
                var secilenHedefDeger = GrdHedef.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                GrdEslestirme.Rows[AktifSatirIndex.Value].Cells[HedefSutun.Index].Value = secilenHedefDeger;
                AktifSatirIndex = null; //eşleştirme tamamlandıktan sonra sıfırla
                secilenKaynakDeger = null; //seçilen kaynak değeri sıfırla
                LstboxLog.Items.Add("hedef" + secilenHedefDeger.GetType());
            }
        }

        Dictionary<string,(object DataType,int length,bool IsNullable)> KaynakKolonlar = 
            new Dictionary<string, (object DataType, int length, bool IsNullable)>();
        Dictionary<string,(object DataType,int length,bool IsNullable)> HedefKolonlar = 
            new Dictionary<string, (object DataType, int length, bool IsNullable)>();



        //şu an sadece tip kontrolü yapıyor
        private void KontrolEt(DataGridViewRow row)
        {
          
            object kaynakDeger = GrdEslestirme.Rows[GrdEslestirme.CurrentCell.RowIndex].Cells[KaynakSutun.Index].Value;
            object hedefDeger = GrdEslestirme.Rows[GrdEslestirme.CurrentCell.RowIndex].Cells[HedefSutun.Index].Value;

            if (kaynakDeger != null && hedefDeger != null) // her iki değerde dolu ise
            {
                if (kaynakDeger.GetType() != hedefDeger.GetType())
                {                    
                    row.Cells["Uygunluk"].Value = "Uyumsuz tip";
                    row.Cells["Uygunluk"].Style.ForeColor = Color.Red;
                    LstboxLog.Items.Add("Veri tipleri uyuşmuyor." + "\n Kaynak Deger: " + kaynakDeger.GetType() + "\n hedef deger: " + hedefDeger.GetType());
                    LstboxLog.ForeColor = Color.Red;
                    MessageBox.Show("Seçilen kaynak ve hedef değerlerin veri tipleri uyuşmuyor. Lütfen uygun değerleri seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    row.Cells["Uygunluk"].Style.ForeColor = Color.Green;
                    row.Cells["Uygunluk"].Value = "Uyumlu tip";
                }
               
            }
            
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

        }

        private void GrdEslestirme_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

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
            KontrolEt(GrdEslestirme.Rows[GrdEslestirme.CurrentCell.RowIndex]);
            
        }
    }

}
