using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
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

        private void LstboxEslesmeLog_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void PnlEslestirme_Paint(object sender, PaintEventArgs e)
        {

        }



        private void BtnBaglantiTest_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtboxKaynakSunucu.Text) ||
                string.IsNullOrWhiteSpace(TxboxHedefKullanici.Text) ||
                string.IsNullOrWhiteSpace(TxboxHedefSifre.Text) ||
                string.IsNullOrWhiteSpace(TxtKullan�c�.Text) ||
                string.IsNullOrWhiteSpace(TxtSifre.Text) ||
                string.IsNullOrWhiteSpace(TxtboxHedefSunucu.Text))

            {
                MessageBox.Show("L�tfen t�m ba�lant� bilgilerini doldurun.", "Uyar�", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TestConnectionAsync();//baglant� testi
            VeriTaban�Combobox();//veritaban� combobox doldurma


        }
        private async void TestConnectionAsync()
        {
            BtnBaglantiTest.Enabled = false;
            BtnBaglantiTest.Text = "Ba�lant� Testi Yap�l�yor...";

            string kaynakConnection =
               $"Server={TxtboxKaynakSunucu.Text};" +
               $"User Id={TxtKullan�c�.Text};" +
               $"Password={TxtSifre.Text};" +
               $"TrustServerCertificate=True;";

            string hedefConnection =
               $"Server={TxtboxHedefSunucu.Text};" +
               $"User Id={TxboxHedefKullanici.Text};" +
               $"Password={TxboxHedefSifre.Text};" +
               $"TrustServerCertificate=True;";


            try
            {

                SqlConnection connKaynak = new SqlConnection(kaynakConnection);
                if (connKaynak.State == ConnectionState.Closed)
                {
                    await connKaynak.OpenAsync();
                }

                SqlConnection connHedef = new SqlConnection(hedefConnection);
                if (connHedef.State == ConnectionState.Closed)
                {
                    await connHedef.OpenAsync();
                }
                else
                {
                    MessageBox.Show("Ba�lant� zaten a��k.");
                }

                MessageBox.Show("Hem kaynak hem hedef ba�lant�s� ba�ar�l�!");

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ba�lant� ba�ar�s�z:\n{ex.Message}");
            }
            finally
            {
                BtnBaglantiTest.Enabled = true;
                BtnBaglantiTest.Text = "Ba�lant�y� Test Et";
            }

        }


        private void BtnKynkKolonYukle_Click(object sender, EventArgs e)
        {

            try
            {
                string server = TxtboxKaynakSunucu.Text;
                string db = CmbboxKaynakVeritabani.Text;
                string table = CmbboxKaynaktablo.Text;
                string user = TxtKullan�c�.Text;
                string pass = TxtSifre.Text;
                string sutun = CmboxKaynakSutun.Text;

                if (string.IsNullOrWhiteSpace(table) || (string.IsNullOrWhiteSpace(sutun)))
                {
                    MessageBox.Show("L�tfen tablo ve s�tun ad�n� girin.", "Uyar�", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                List<string> columns = GetColumns(server, db, table, sutun, user, pass);

                DataTable dt = GetTableData(server, db, table, sutun, user, pass);

                DataGridViewTextBoxColumn colSelect = new DataGridViewTextBoxColumn();
                colSelect.HeaderText = "Kaynak Kolonlar";
                colSelect.ReadOnly = true;
                GrdKaynak.Columns.Add(colSelect);

                DataRow row=dt.NewRow();
                
                dt.Rows.Add(row);
                GrdKaynak.Columns.Clear();
                GrdKaynak.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata olu�tu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }

        }
        // kolonlar� listeleme metodu
        private List<string> GetColumns(string server, string db, string table, string sutun, string user, string pass)
        {
            List<string> columns = new List<string>();


            string connStr = $"Server={server};Database={db};User Id={user};Password={pass};TrustServerCertificate=True;";


            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

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
        //kolon i�eriklerini g�rme
        private DataTable GetTableData(string server, string db, string table, string sutun, string user, string password)
        {

            DataTable dt = new DataTable();//bellek i�erisinde tablo olu�turur
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

        private void GrdHedef_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void GrdKaynak_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void BtnHedefKolonYukle_Click(object sender, EventArgs e)
        {
            try
            {
                string server = TxtboxHedefSunucu.Text;
                string db=TxboxHedefKullanici.Text;

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void CmbboxKaynakVeritabani_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabloDoldur();
        }

        private void TxtboxKaynakSunucu_TextChanged(object sender, EventArgs e)
        {


        }

        private void BtnVeritabaniGetir_Click(object sender, EventArgs e)
        {

        }

        //veritaban� combobox doldurma
        private void VeriTaban�Combobox()
        {
            string server = TxtboxKaynakSunucu.Text;
            string user = TxtKullan�c�.Text;
            string pass = TxtSifre.Text;
            if (string.IsNullOrWhiteSpace(server) ||
                string.IsNullOrWhiteSpace(user) ||
                string.IsNullOrWhiteSpace(pass))
            {
                MessageBox.Show("L�tfen sunucu, kullan�c� ad� ve �ifre bilgilerini doldurun.", "Uyar�", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                //tablo combobox doldurma
                //MessageBox.Show("Veritabanlar� ba�ar�yla y�klendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Hata olu�tu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void TabloDoldur()
        {
            string server = TxtboxKaynakSunucu.Text;
            string db = CmbboxKaynakVeritabani.Text;
            string user = TxtKullan�c�.Text;
            string pass = TxtSifre.Text;
            if (string.IsNullOrWhiteSpace(server) ||
                string.IsNullOrWhiteSpace(db) ||
                string.IsNullOrWhiteSpace(user) ||
                string.IsNullOrWhiteSpace(pass))
            {
                MessageBox.Show("L�tfen sunucu, veritaban�, kullan�c� ad� ve �ifre bilgilerini doldurun.", "Uyar�", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                //MessageBox.Show("Tablolar ba�ar�yla y�klendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata olu�tu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void SutunDoldur()
        {
            string server = TxtboxKaynakSunucu.Text;
            string db = CmbboxKaynakVeritabani.Text;
            string table = CmbboxKaynaktablo.Text;
            string user = TxtKullan�c�.Text;
            string pass = TxtSifre.Text;
            if (string.IsNullOrWhiteSpace(server) ||
                string.IsNullOrWhiteSpace(db) ||
                string.IsNullOrWhiteSpace(table) ||
                string.IsNullOrWhiteSpace(user) ||
                string.IsNullOrWhiteSpace(pass))
            {
                MessageBox.Show("L�tfen sunucu, veritaban�, tablo, kullan�c� ad� ve �ifre bilgilerini doldurun.", "Uyar�", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                //MessageBox.Show("S�tunlar ba�ar�yla y�klendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata olu�tu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            SutunDoldur();
        }
    }

}
