using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic.ApplicationServices;
using System.Data;
using System.Drawing.Imaging;

namespace DataTransfer
{
    public partial class FrmVeriEslestirme : Form
    {
        public FrmVeriEslestirme()
        {

            InitializeComponent();

        }

        private void LblHdfVeri_Click(object sender, EventArgs e)
        {

        }

        private void lblKynkVeri_Click(object sender, EventArgs e)
        {

        }

        private void BtnVeriAktarim_Click(object sender, EventArgs e)
        {

        }

        private void GrbBoxSutunlar_Enter(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }



        private void GrbboxKaynak_Enter(object sender, EventArgs e)
        {

        }


        private void BtnBaglantiTest_Click(object sender, EventArgs e)
        {
            LstboxLog.Items.Clear();
            if (string.IsNullOrWhiteSpace(TxtboxKaynakSunucu.Text) ||
                string.IsNullOrWhiteSpace(TxboxHedefKullanici.Text) ||
                string.IsNullOrWhiteSpace(TxboxHedefSifre.Text) ||
                string.IsNullOrWhiteSpace(TxtKullanýcý.Text) ||
                string.IsNullOrWhiteSpace(TxtSifre.Text) ||
                string.IsNullOrWhiteSpace(TxtboxHedefSunucu.Text))

            {
                MessageBox.Show("Lütfen tüm baðlantý bilgilerini doldurun.", "Uyarý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TestConnectionAsync();//baglantý testi
            KaynakVeriTabanýCombobox();//veritabaný combobox doldurma
            HedefVeriTabaniCombobox();//hedef veritabaný combobox doldurma


        }
        private async void TestConnectionAsync()
        {
            BtnBaglantiTest.Enabled = false;
            BtnBaglantiTest.Text = "Baðlantý Testi Yapýlýyor...";

            string kaynakConnection =
               $"Server={TxtboxKaynakSunucu.Text};" +
               $"User Id={TxtKullanýcý.Text};" +
               $"Password={TxtSifre.Text};" +
               $"TrustServerCertificate=True;";

            string hedefConnection =
               $"Server={TxtboxHedefSunucu.Text};" +
               $"User Id={TxboxHedefKullanici.Text};" +
               $"Password={TxboxHedefSifre.Text};" +
               $"TrustServerCertificate=True;";


            try
            {
                SqlConnection connHedef = new SqlConnection(hedefConnection);
                if (connHedef.State == ConnectionState.Closed)
                {
                    await connHedef.OpenAsync();
                }

                SqlConnection connKaynak = new SqlConnection(kaynakConnection);
                if (connKaynak.State == ConnectionState.Closed)
                {
                    await connKaynak.OpenAsync();
                }


                else
                {
                    MessageBox.Show("Baðlantý zaten açýk.");
                }

                MessageBox.Show("Hem kaynak hem hedef baðlantýsý baþarýlý!");

                if (connKaynak.State == ConnectionState.Open && connHedef.State == ConnectionState.Open)
                {
                    LstboxLog.ForeColor = Color.Green;
                    LstboxLog.Items.Add("Baðlantý baþarýlý þekilde oluþtu.");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Baðlantý baþarýsýz:\n{ex.Message}");
                LstboxLog.ForeColor = Color.Red;
                LstboxLog.Items.Add("Baðlantý baþarýsýz.");
            }
            finally
            {
                BtnBaglantiTest.Enabled = true;
                BtnBaglantiTest.Text = "Baðlantýyý Test Et";
            }

        }




        private void BtnKynkKolonYukle_Click(object sender, EventArgs e)
        {

            try
            {
                string server = TxtboxKaynakSunucu.Text;
                string db = CmbboxKaynakVeritabani.Text;
                string table = CmbboxKaynaktablo.Text;
                string user = TxtKullanýcý.Text;
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
                    MessageBox.Show("Lütfen tablo ve sütun adýný girin.", "Uyarý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                //KolonYukle(server, db, table, sutun, user, pass);
                List<string> columns = GetColumns(server, db, table, sutun, user, pass);
                DataTable dt = GetTableData(server, db, table, sutun, user, pass);
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
                MessageBox.Show($"Hata oluþtu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        // kolonlarý listeleme metodu
        private List<string> GetColumns(string server, string db, string table, string sutun, string user, string pass)
        {
            if (string.IsNullOrWhiteSpace(server)
                || string.IsNullOrWhiteSpace(db)
                || string.IsNullOrWhiteSpace(table)
                || string.IsNullOrWhiteSpace(sutun)
                || string.IsNullOrWhiteSpace(user)
                || string.IsNullOrWhiteSpace(pass)
                )
            {
                MessageBox.Show("Lütfen tüm baðlantý bilgilerini doldurun.", "Uyarý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            List<string> columns = new List<string>();
            string connStr = $"Server={server};Database={db};User Id={user};Password={pass};TrustServerCertificate=True;";


            using (SqlConnection conn = new SqlConnection(connStr))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                string sql = @"SELECT COLUMN_NAME 
                       FROM INFORMATION_SCHEMA.COLUMNS 
                       WHERE TABLE_NAME = @TableName AND COLUMN_NAME= @ColumnName";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
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
            }

            return columns;
        }
        //kolon içeriklerini görme
        private DataTable GetTableData(string server, string db, string table, string sutun, string user, string password)
        {
            if (string.IsNullOrWhiteSpace(server) ||
                string.IsNullOrWhiteSpace(db) ||
                string.IsNullOrWhiteSpace(table) ||
                string.IsNullOrWhiteSpace(sutun) ||
                string.IsNullOrWhiteSpace(user) ||
                string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Lütfen tüm baðlantý bilgilerini doldurun.", "Uyarý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            DataTable dt = new DataTable();//bellek içerisinde tablo oluþturur
            string connStr = $"Server={server};Database={db};User Id={user};Password={password};TrustServerCertificate=True;";


            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string sqlsorgu = $" SELECT [{sutun}] FROM [{table}]";
                using (SqlDataAdapter da = new SqlDataAdapter(sqlsorgu, conn))
                {
                    da.Fill(dt);
                }
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
                    MessageBox.Show("Lütfen tablo ve sütun adýný girin.", "Uyarý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }



                //KolonYukle(server, db, table, sutun, user, pass);
                List<string> columns = GetColumns(server, db, table, sutun, user, pass);
                DataTable dt = GetTableData(server, db, table, sutun, user, pass);
                DataGridViewTextBoxColumn colSelect = new DataGridViewTextBoxColumn();
                colSelect.HeaderText = "Hedef Kolonlar";
                colSelect.ReadOnly = true;


                GrdHedef.Columns.Add(colSelect);
                DataRow row = dt.NewRow();
                dt.Rows.Add(row);
                GrdHedef.Columns.Clear();
                GrdHedef.DataSource = dt;

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void CmbboxKaynakVeritabani_SelectedIndexChanged(object sender, EventArgs e)
        {
            KaynakTabloDoldur();
        }

        private void TxtboxKaynakSunucu_TextChanged(object sender, EventArgs e)
        {


        }

        private void BtnVeritabaniGetir_Click(object sender, EventArgs e)
        {

        }

        //veritabaný combobox doldurma
        private void KaynakVeriTabanýCombobox()
        {
            string server = TxtboxKaynakSunucu.Text;
            string user = TxtKullanýcý.Text;
            string pass = TxtSifre.Text;
            if (string.IsNullOrWhiteSpace(server) ||
                string.IsNullOrWhiteSpace(user) ||
                string.IsNullOrWhiteSpace(pass))
            {
                MessageBox.Show("Lütfen sunucu, kullanýcý adý ve þifre bilgilerini doldurun.", "Uyarý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                using (SqlConnection connection = new SqlConnection(connStr))
                {
                    connection.Open();
                    string sql = "SELECT NAME FROM sys.databases ORDER BY name";
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        CmbboxKaynakVeritabani.Items.Clear();
                        while (reader.Read())
                        {
                            CmbboxKaynakVeritabani.Items.Add(reader["name"].ToString());
                        }
                    }
                }
                //LstboxLog.ForeColor = Color.Green;
                //LstboxLog.Items.Add("Kaynak Veritabanlarý baþarýyla yüklendi.");
                //tablo combobox doldurma
                //MessageBox.Show("Veritabanlarý baþarýyla yüklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Hata oluþtu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LstboxLog.ForeColor = Color.Red;
                LstboxLog.Items.Add("Veritabaný yüklenemedi.");
                return;
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
                MessageBox.Show("Lütfen sunucu, kullanýcý adý ve þifre bilgilerini doldurun.", "Uyarý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                using (SqlConnection connection = new SqlConnection(connStr))
                {
                    connection.Open();
                    string sql = "SELECT NAME FROM sys.databases ORDER BY name";
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        CmbboxHedefVeriTabani.Items.Clear();
                        while (reader.Read())
                        {
                            CmbboxHedefVeriTabani.Items.Add(reader["name"].ToString());
                        }
                    }
                }
                //LstboxLog.ForeColor = Color.Green;
                //LstboxLog.Items.Add(" Hedef Veritabanlarý baþarýyla yüklendi.");
                //tablo combobox doldurma
                //MessageBox.Show("Veritabanlarý baþarýyla yüklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluþtu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LstboxLog.ForeColor = Color.Red;
                LstboxLog.Items.Add("Veritabaný yüklenemedi.");
                return;
            }
        }
        private void KaynakTabloDoldur()
        {
            string server = TxtboxKaynakSunucu.Text;
            string db = CmbboxKaynakVeritabani.Text;
            string user = TxtKullanýcý.Text;
            string pass = TxtSifre.Text;
            if (string.IsNullOrWhiteSpace(server) ||
                string.IsNullOrWhiteSpace(db) ||
                string.IsNullOrWhiteSpace(user) ||
                string.IsNullOrWhiteSpace(pass))
            {
                MessageBox.Show("Lütfen sunucu, veritabaný, kullanýcý adý ve þifre bilgilerini doldurun.", "Uyarý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string connStr = $"Server={server};Database={db};User Id={user};Password={pass};TrustServerCertificate=True;";
            try
            {
                using (SqlConnection connection = new SqlConnection(connStr))
                {
                    connection.Open();
                    string sql = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' ORDER BY TABLE_NAME";
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        CmbboxKaynaktablo.Items.Clear();
                        while (reader.Read())
                        {
                            CmbboxKaynaktablo.Items.Add(reader["TABLE_NAME"].ToString());
                        }
                    }
                }
                //MessageBox.Show("Tablolar baþarýyla yüklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluþtu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Lütfen sunucu, veritabaný, kullanýcý adý ve þifre bilgilerini doldurun.", "Uyarý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string connStr = $"Server={server};Database={db};User Id={user};Password={pass};TrustServerCertificate=True;";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string sql = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' ORDER BY TABLE_NAME";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        CmbboxHedefTablo.Items.Clear();
                        while (reader.Read())
                        {
                            CmbboxHedefTablo.Items.Add(reader["TABLE_NAME"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Hata oluþtu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void KaynakSutunDoldur()
        {
            string server = TxtboxKaynakSunucu.Text;
            string db = CmbboxKaynakVeritabani.Text;
            string table = CmbboxKaynaktablo.Text;
            string user = TxtKullanýcý.Text;
            string pass = TxtSifre.Text;
            if (string.IsNullOrWhiteSpace(server) ||
                string.IsNullOrWhiteSpace(db) ||
                string.IsNullOrWhiteSpace(table) ||
                string.IsNullOrWhiteSpace(user) ||
                string.IsNullOrWhiteSpace(pass))
            {
                MessageBox.Show("Lütfen sunucu, veritabaný, tablo, kullanýcý adý ve þifre bilgilerini doldurun.", "Uyarý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string connStr = $"Server={server};Database={db};User Id={user};Password={pass};TrustServerCertificate=True;";
            try
            {
                using (SqlConnection connection = new SqlConnection(connStr))
                {
                    connection.Open();
                    string sql = @"SELECT COLUMN_NAME 
                       FROM INFORMATION_SCHEMA.COLUMNS 
                       WHERE TABLE_NAME = @TableName
                       ORDER BY ORDINAL_POSITION";
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@TableName", table);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            CmboxKaynakSutun.Items.Clear();
                            while (reader.Read())
                            {
                                CmboxKaynakSutun.Items.Add(reader["COLUMN_NAME"].ToString());
                            }
                        }
                    }
                }
                //LstboxLog.ForeColor = Color.Green;
                //LstboxLog.Items.Add("Kaynak Sütunlar baþarýyla yüklendi.");
                //MessageBox.Show("Sütunlar baþarýyla yüklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluþtu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LstboxLog.ForeColor = Color.Red;
                LstboxLog.Items.Add("Sütunlar yüklenemedi.");
                return;
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
                MessageBox.Show("Lütfen sunucu, veritabaný, tablo, kullanýcý adý ve þifre bilgilerini doldurun.", "Uyarý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string connStr = $"Server={server};Database={db};User Id={user};Password={pass};TrustServerCertificate=True;";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string sql = @"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME=@TableName ORDER BY ORDINAL_POSITION";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@TableName", table);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            CmboxHedefSutun.Items.Clear();
                            while (reader.Read())
                            {
                                CmboxHedefSutun.Items.Add(reader["COLUMN_NAME"].ToString());
                            }
                        }
                    }

                }
                //LstboxLog.ForeColor = Color.Green;
                //LstboxLog.Items.Add("Hedef Sütunlar baþarýyla yüklendi.");
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Hata oluþtu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LstboxLog.ForeColor = Color.Red;
                LstboxLog.Items.Add("Sütunlar yüklenemedi.");
                return;
            }
        }

        private void GrbboxHedef_Enter(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

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
        private string secilenKaynakDeger = null;
        private void GrdKaynak_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                secilenKaynakDeger = GrdKaynak.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();
                int newRowIndex = GrdEslestirme.Rows.Add();

                GrdEslestirme.Rows[newRowIndex].Cells[KaynakSutun.Index].Value = secilenKaynakDeger;
                AktifSatirIndex = newRowIndex; //satýrý kaydet
            }
        }

        private void GrdHedef_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!AktifSatirIndex.HasValue)
            {
                MessageBox.Show("Önce Kaynak deðeri seçin", "Uyarý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && AktifSatirIndex.HasValue)
            {
                string secilenHedefDeger = GrdHedef.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();

                GrdEslestirme.Rows[AktifSatirIndex.Value].Cells[HedefSutun.Index].Value = secilenHedefDeger;
                AktifSatirIndex = null; //eþleþtirme tamamlandýktan sonra sýfýrla
                secilenKaynakDeger = null; //seçilen kaynak deðeri sýfýrla
            }
        }

        private void CmboxHedefSutun_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void GrdEslestirme_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && GrdEslestirme.Columns[e.ColumnIndex].Name == "Sil")
            {
                string kaynak = GrdEslestirme.Rows[e.RowIndex].Cells[KaynakSutun.Index].Value?.ToString();
                string hedef = GrdEslestirme.Rows[e.RowIndex].Cells[HedefSutun.Index].Value?.ToString();
                if ((!string.IsNullOrWhiteSpace(kaynak) && !string.IsNullOrWhiteSpace(hedef)) || (!string.IsNullOrWhiteSpace(kaynak) ) || (!string.IsNullOrWhiteSpace(hedef)))
                {
                    DialogResult result = MessageBox.Show("Bu eþleþmeyi silmek istiyor musunuz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        GrdEslestirme.Rows.RemoveAt(e.RowIndex);
                    }
                }
                else
                {
                    MessageBox.Show("Silinecek eþleþme bulunamadý.", "Uyarý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void GrdHedef_MouseHover(object sender, EventArgs e)
        {

        }
    }

}
