using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataTransfer
{
    public partial class FrmBaglantiAc : Form
    {
        public BaglantiBilgileri KaynakBaglanti { get; private set; }
        public BaglantiBilgileri HedefBaglanti { get; private set; }

        public FrmBaglantiAc()
        {
            InitializeComponent();
        }

        private async void BtnBaglantiTest_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtboxKaynakSunucu.Text) ||
                string.IsNullOrWhiteSpace(TxtKullanıcı.Text) ||
                string.IsNullOrWhiteSpace(TxtSifre.Text) ||
                string.IsNullOrWhiteSpace(TxtboxHedefSunucu.Text) ||
                string.IsNullOrWhiteSpace(TxboxHedefKullanici.Text) ||
                string.IsNullOrWhiteSpace(TxboxHedefSifre.Text))
            {
                MessageBox.Show("Lütfen tüm bağlantı bilgilerini doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            BtnBaglantiTest.Enabled = false;
            BtnBaglantiTest.Text = "Bağlantı Testi Yapılıyor...";

            try
            {
                // Kaynak bağlantı bilgileri
                KaynakBaglanti = new BaglantiBilgileri
                {
                    Sunucu = TxtboxKaynakSunucu.Text.Trim(),
                    Kullanici = TxtKullanıcı.Text.Trim(),
                    Sifre = TxtSifre.Text.Trim()
                };

                // Hedef bağlantı bilgileri
                HedefBaglanti = new BaglantiBilgileri
                {
                    Sunucu = TxtboxHedefSunucu.Text.Trim(),
                    Kullanici = TxboxHedefKullanici.Text.Trim(),
                    Sifre = TxboxHedefSifre.Text.Trim()
                };

                bool kaynakDurum = await BaglantiTestAsync(KaynakBaglanti);
                bool hedefDurum = await BaglantiTestAsync(HedefBaglanti);

                if (kaynakDurum && hedefDurum)
                {
                    LstboxLog.ForeColor = Color.Green;
                    LstboxLog.Items.Add($"[{DateTime.Now:HH:mm:ss}] Kaynak ve Hedef bağlantıları başarıyla açıldı.");



                    MessageBox.Show("Bağlantılar başarılı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Veritabanı comboboxlarını doldur
                    await KaynakVeritabaniComboboxDoldur(KaynakBaglanti);
                    await HedefVeritabaniComboboxDoldur(HedefBaglanti);
                }
                else
                {
                    MessageBox.Show("Bağlantılardan biri veya her ikisi başarısız.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                LstboxLog.ForeColor = Color.Red;
                LstboxLog.Items.Add($"[{DateTime.Now:HH:mm:ss}] Hata: {ex.Message}");
                MessageBox.Show($"Bağlantı sırasında hata oluştu:\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                BtnBaglantiTest.Enabled = true;
                BtnBaglantiTest.Text = "Bağlantıyı Test Et";
            }
        }

        private async Task<bool> BaglantiTestAsync(BaglantiBilgileri info)
        {
            string connStr = $"Server={info.Sunucu}; User Id={info.Kullanici}; Password={info.Sifre}; TrustServerCertificate=True;";
            try
            {
                using (var conn = new SqlConnection(connStr))
                {
                    await conn.OpenAsync();
                    LstboxLog.ForeColor = Color.Green;
                    LstboxLog.Items.Add($"[{DateTime.Now:HH:mm:ss}] Bağlantı başarılı: {info.Sunucu}");
                }
                return true;
            }
            catch (Exception ex)
            {
                LstboxLog.ForeColor = Color.Red;
                LstboxLog.Items.Add($"[{DateTime.Now:HH:mm:ss}] Bağlantı başarısız: {info.Sunucu} -> {ex.Message}");
                return false;
            }
        }

        private async Task KaynakVeritabaniComboboxDoldur(BaglantiBilgileri info)
        {
            if (string.IsNullOrWhiteSpace(info.Sunucu) ||
                string.IsNullOrWhiteSpace(info.Kullanici) ||
                string.IsNullOrWhiteSpace(info.Sifre))
            {
                MessageBox.Show("Lütfen sunucu, kullanıcı adı ve şifre bilgilerini doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connStr = $"Server={info.Sunucu};Database=master;User Id={info.Kullanici};Password={info.Sifre};TrustServerCertificate=True;";

            try
            {
                using (var conn = new SqlConnection(connStr))
                {
                    await conn.OpenAsync();

                    string sql = "SELECT NAME FROM sys.databases ORDER BY name";
                    using (var cmd = new SqlCommand(sql, conn))
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        CmbboxKaynakVeritabani.Items.Clear();
                        while (await reader.ReadAsync())
                        {
                            CmbboxKaynakVeritabani.Items.Add(reader["name"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kaynak veritabanları yüklenemedi: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LstboxLog.ForeColor = Color.Red;
                LstboxLog.Items.Add("Kaynak veritabanı yüklenemedi.");
            }
        }


        private async Task HedefVeritabaniComboboxDoldur(BaglantiBilgileri info)
        {
            if (string.IsNullOrWhiteSpace(info.Sunucu) ||
                string.IsNullOrWhiteSpace(info.Kullanici) ||
                string.IsNullOrWhiteSpace(info.Sifre))
            {
                MessageBox.Show("Lütfen sunucu, kullanıcı adı ve şifre bilgilerini doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connStr = $"Server={info.Sunucu};Database=master;User Id={info.Kullanici};Password={info.Sifre};TrustServerCertificate=True;";

            try
            {
                using (var conn = new SqlConnection(connStr))
                {
                    await conn.OpenAsync();

                    string sql = "SELECT NAME FROM sys.databases ORDER BY name";
                    using (var cmd = new SqlCommand(sql, conn))
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        CmbboxHedefVeriTabani.Items.Clear();
                        while (await reader.ReadAsync())
                        {
                            CmbboxHedefVeriTabani.Items.Add(reader["name"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hedef veritabanları yüklenemedi: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LstboxLog.ForeColor = Color.Red;
                LstboxLog.Items.Add("Hedef veritabanı yüklenemedi.");
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

        private void ChkboxHedefSifre_CheckedChanged(object sender, EventArgs e)
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

        // FrmBaglantiAc.cs
        private async void BtnDevam_Click(object sender, EventArgs e)
        {
            // 1. Alanların dolu olduğunu kontrol et
            if (string.IsNullOrWhiteSpace(TxtboxKaynakSunucu.Text) ||
                string.IsNullOrWhiteSpace(TxtKullanıcı.Text) ||
                string.IsNullOrWhiteSpace(TxtSifre.Text) ||
                string.IsNullOrWhiteSpace(CmbboxKaynakVeritabani.Text) ||
                string.IsNullOrWhiteSpace(TxtboxHedefSunucu.Text) ||
                string.IsNullOrWhiteSpace(TxboxHedefKullanici.Text) ||
                string.IsNullOrWhiteSpace(TxboxHedefSifre.Text) ||
                string.IsNullOrWhiteSpace(CmbboxHedefVeriTabani.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. BaglantiBilgilerini oluştur
            BaglantiBilgileri kaynak = new BaglantiBilgileri
            {
                Sunucu = TxtboxKaynakSunucu.Text.Trim(),
                Kullanici = TxtKullanıcı.Text.Trim(),
                Sifre = TxtSifre.Text.Trim(),
                Veritabani = CmbboxKaynakVeritabani.Text.Trim()
            };

            BaglantiBilgileri hedef = new BaglantiBilgileri
            {
                Sunucu = TxtboxHedefSunucu.Text.Trim(),
                Kullanici = TxboxHedefKullanici.Text.Trim(),
                Sifre = TxboxHedefSifre.Text.Trim(),
                Veritabani = CmbboxHedefVeriTabani.Text.Trim()
            };

            // 3. Bağlantıları test et
            bool kaynakBasarili = await BaglantiTestAsync(kaynak);
            bool hedefBasarili = await BaglantiTestAsync(hedef);

            if (!kaynakBasarili || !hedefBasarili)
            {
                MessageBox.Show("Bağlantılardan biri veya her ikisi başarısız.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            await KaynakVeritabaniComboboxDoldur(kaynak);
            await HedefVeritabaniComboboxDoldur(hedef);

            // 4. FrmVeriEslestirme'yi aç ve bilgileri aktar
            FrmVeriEslestirme frm = new FrmVeriEslestirme(kaynak, hedef);
            frm.Show();
            this.Hide();
        }

        private void FrmBaglantiAc_Load(object sender, EventArgs e)
        {

        }
    }
}
