using Microsoft.Data.SqlClient;
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
                string.IsNullOrWhiteSpace(TxtKullanýcý.Text) ||
                string.IsNullOrWhiteSpace(TxtSifre.Text))
            {
                MessageBox.Show("Lütfen tüm baðlantý bilgilerini doldurun.", "Uyarý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TestConnectionAsync();
           
        }
        private async void TestConnectionAsync()
        {
            
            string connectionString =
               $"Server={TxtboxKaynakSunucu.Text};" +
               $"Database={CmbboxKaynakVeritabani.Text};" +
               $"User Id={TxtKullanýcý.Text};" +
               $"Password={TxtSifre.Text};" +
               $"TrustServerCertificate=True;";

            try
            {
                using (SqlConnection baglanti = new SqlConnection(connectionString))
                {
                    await baglanti.OpenAsync();
                    MessageBox.Show("Baðlantý baþarýlý!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($" Baðlantý baþarýsýz:\n{ex.Message}");
            }
            finally
            {
                BtnBaglantiTest.Enabled = true;
                BtnBaglantiTest.Text = "Baðlantý Testi yapýlýyor";
            }
            
        }


        private void BtnKynkKolonYukle_Click(object sender, EventArgs e)
        {


        }

        private void GrdHedef_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void GrdKaynak_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
