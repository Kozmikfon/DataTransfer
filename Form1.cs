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
                string.IsNullOrWhiteSpace(TxtSifre.Text) ||
                string.IsNullOrWhiteSpace(TxtboxHedefSunucu.Text) ||
                string.IsNullOrWhiteSpace(CmbboxHedefVeriTabani.Text))
                //string.IsNullOrWhiteSpace(CmbboxHedefTablo.Text))
            {
                MessageBox.Show("Lütfen tüm baðlantý bilgilerini doldurun.", "Uyarý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TestConnectionAsync();
           
        }
        private async void TestConnectionAsync()
        {
            BtnBaglantiTest.Enabled = false;
            BtnBaglantiTest.Text = "Baðlantý Testi Yapýlýyor...";

            string kaynakConnection =
                $"Server={TxtboxKaynakSunucu.Text};" +
                $"Database={CmbboxKaynakVeritabani.Text};" +
                $"User Id={TxtKullanýcý.Text};" +
                $"Password={TxtSifre.Text};" +
                $"TrustServerCertificate=True;";

            string hedefConnection =
               $"Server={TxtboxHedefSunucu.Text};" +
               $"Database={CmbboxHedefVeriTabani.Text};" +              
               $"TrustServerCertificate=True;";


            try
            {
                
                using (SqlConnection connKaynak = new SqlConnection(kaynakConnection))
                {
                    await connKaynak.OpenAsync();
                }

                
                using (SqlConnection connHedef = new SqlConnection(hedefConnection))
                {
                    await connHedef.OpenAsync();
                }

                MessageBox.Show("Hem kaynak hem hedef baðlantýsý baþarýlý!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Baðlantý baþarýsýz:\n{ex.Message}");
            }
            finally
            {
                BtnBaglantiTest.Enabled = true;
                BtnBaglantiTest.Text = "Baðlantýyý Test Et";
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
