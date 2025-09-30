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
                
                using (SqlConnection connKaynak = new SqlConnection(kaynakConnection))
                {
                    await connKaynak.OpenAsync();
                }

                
                using (SqlConnection connHedef = new SqlConnection(hedefConnection))
                {
                    await connHedef.OpenAsync();
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


        }

        private void GrdHedef_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void GrdKaynak_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
