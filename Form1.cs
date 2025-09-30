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
                string.IsNullOrWhiteSpace(CmbboxKaynakVeritabani.Text) ||
                string.IsNullOrWhiteSpace(TxtKullan�c�.Text) ||
                string.IsNullOrWhiteSpace(TxtSifre.Text) ||
                string.IsNullOrWhiteSpace(TxtboxHedefSunucu.Text) ||
                string.IsNullOrWhiteSpace(CmbboxHedefVeriTabani.Text))
            //string.IsNullOrWhiteSpace(CmbboxHedefTablo.Text))
            {
                MessageBox.Show("L�tfen t�m ba�lant� bilgilerini doldurun.", "Uyar�", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TestConnectionAsync();

        }
        private async void TestConnectionAsync()
        {
            BtnBaglantiTest.Enabled = false;
            BtnBaglantiTest.Text = "Ba�lant� Testi Yap�l�yor...";

            string kaynakConnection =
               $"Server={TxtboxKaynakSunucu.Text};" +
               $"Database={CmbboxKaynakVeritabani.Text};" +
               $"User Id={TxtKullan�c�.Text};" +
               $"Password={TxtSifre.Text};" +
               $"TrustServerCertificate=True;";

            string hedefConnection =
               $"Server={TxtboxHedefSunucu.Text};" +
               $"Database={CmbboxHedefVeriTabani.Text};" +
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


            string server = TxtboxKaynakSunucu.Text;
            string db = CmbboxKaynakVeritabani.Text;
            string table = CmbboxKaynaktablo.Text;
            string user = TxtKullan�c�.Text;
            string pass = TxtSifre.Text;

            if (string.IsNullOrWhiteSpace(table)

                )
            {
                MessageBox.Show("L�tfen tablo ad�n� girin.", "Uyar�", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            List<string> columns = GetColumns(server, db, table, user, pass);

            DataTable dt = GetTableData(server, db, table, user, pass);





            DataGridViewTextBoxColumn colSelect = new DataGridViewTextBoxColumn();
            colSelect.HeaderText = "Kaynak Kolonlar";
            colSelect.ReadOnly = true;
            GrdKaynak.Columns.Add(colSelect);
            foreach (string col in columns)
            {
                GrdKaynak.Rows.Add(col);
            }
            GrdKaynak.DataSource = dt;

        }
        // kolonlar� listeleme metodu
        private List<string> GetColumns(string server, string db, string table, string user, string pass)
        {
            List<string> columns = new List<string>();


            string connStr = $"Server={server};Database={db};User Id={user};Password={pass};TrustServerCertificate=True;";


            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string sql = @"SELECT COLUMN_NAME 
                       FROM INFORMATION_SCHEMA.COLUMNS 
                       WHERE TABLE_NAME = @Table";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Table", table);

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
        private DataTable GetTableData(string server, string db, string table, string user, string password)
        {

            DataTable dt = new DataTable();//bellek i�erisinde tablo olu�turur
            string connStr = $"Server={server};Database={db};User Id={user};Password={password};TrustServerCertificate=True;";


            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string sqlsorgu = $"SELECT * FROM {table}";
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

        }

        private void CmbboxKaynakVeritabani_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void TxtboxKaynakSunucu_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
    
}
