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

        private bool kaynakTestBasarili = false;
        private bool hedefTestBasarili = false;

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

               

                kaynakTestBasarili = await BaglantiTestAsync(KaynakBaglanti);
                hedefTestBasarili = await BaglantiTestAsync(HedefBaglanti);

                if (kaynakTestBasarili && kaynakTestBasarili)
                {
                    LstboxLog.ForeColor = Color.Green;
                    LstboxLog.Items.Add($"[{DateTime.Now:HH:mm:ss}] Kaynak ve Hedef bağlantıları başarıyla açıldı.");



                    MessageBox.Show("Bağlantılar başarılı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Veritabanı comboboxlarını doldur
                    await KaynakVeritabaniComboboxDoldur(KaynakBaglanti);
                    await HedefVeritabaniComboboxDoldur(HedefBaglanti);
                    CmbboxKaynakVeritabani.Enabled = true;
                    CmbboxHedefVeriTabani.Enabled = true;
                    BtnDevam.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Bağlantılardan biri veya her ikisi başarısız.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    BtnDevam.Enabled = false;
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
                    using (var cmd = new SqlCommand("SELECT name FROM sys.databases;", conn))
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            // sadece test amaçlı okuma
                            while (await reader.ReadAsync()) { }
                        }
                    }

                    LstboxLog.ForeColor = Color.Green;
                    LstboxLog.Items.Add($"[{DateTime.Now:HH:mm:ss}] {info.Sunucu} bağlantısı başarılı.");
                }

                return true;
            }
            catch (SqlException ex)
            {
                string mesaj;

                switch (ex.Number)
                {
                    case 18456:
                        mesaj = "SQL kimlik doğrulaması başarısız. Kullanıcı adı veya şifre hatalı.";
                        break;
                    case 4060:
                        mesaj = "Bağlanılmak istenen veritabanına erişim izniniz yok.";
                        break;
                    case 229:
                        mesaj = "Bu kullanıcı, veritabanı nesnelerine erişim yetkisine sahip değil.";
                        break;
                    default:
                        mesaj = ex.Message;
                        break;
                }

                LstboxLog.ForeColor = Color.Red;
                LstboxLog.Items.Add($"[{DateTime.Now:HH:mm:ss}] {info.Sunucu} bağlantı hatası: {mesaj}");
                MessageBox.Show(mesaj, "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            string connStr = $"Server={info.Sunucu};Database=master;User Id={info.Kullanici};Password={info.Sifre};Connect Timeout=10;TrustServerCertificate=True;";

            try
            {
                using (var conn = new SqlConnection(connStr))
                {
                    await conn.OpenAsync();

                    string sql = "SELECT name FROM sys.databases WHERE state = 0 ORDER BY name";
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
            catch (SqlException ex)
            {
                CmbboxKaynakVeritabani.Items.Clear();

                string mesaj = ex.Number switch
                {
                    18456 => "SQL kullanıcı adı veya şifre hatalı.",
                    4060 => "Bu kullanıcı seçilen veritabanına erişemez.",
                    229 => "Veritabanı erişim izniniz yok.",
                    _ => ex.Message
                };

                LstboxLog.ForeColor = Color.Red;
                LstboxLog.Items.Add($"[{DateTime.Now:HH:mm:ss}] ({info.Sunucu}) Hata: {mesaj}");

                MessageBox.Show(mesaj, "Veritabanı Erişim Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            string connStr = $"Server={info.Sunucu};Database=master;User Id={info.Kullanici};Password={info.Sifre};Connect Timeout=10;TrustServerCertificate=True;";

            try
            {
                using (var conn = new SqlConnection(connStr))
                {
                    await conn.OpenAsync();

                    string sql = "SELECT name FROM sys.databases WHERE state = 0 ORDER BY name";
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
            catch (SqlException ex)
            {
                CmbboxHedefVeriTabani.Items.Clear();


                string mesaj = ex.Number switch
                {
                    18456 => "SQL kullanıcı adı veya şifre hatalı.",
                    4060 => "Bu kullanıcı seçilen veritabanına erişemez.",
                    229 => "Veritabanı erişim izniniz yok.",
                    _ => ex.Message
                };

                LstboxLog.ForeColor = Color.Red;
                LstboxLog.Items.Add($"[{DateTime.Now:HH:mm:ss}] ({info.Sunucu}) Hata: {mesaj}");

                MessageBox.Show(mesaj, "Veritabanı Erişim Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            // Bağlantı test edilmeden ilerleme
            if (!kaynakTestBasarili || !hedefTestBasarili)
            {
                MessageBox.Show("Lütfen önce bağlantı testini başarıyla tamamlayın.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Veritabanı seçimi zorunlu
            if (string.IsNullOrWhiteSpace(CmbboxKaynakVeritabani.Text) ||
                string.IsNullOrWhiteSpace(CmbboxHedefVeriTabani.Text))
            {
                MessageBox.Show("Lütfen her iki bağlantı için de veritabanı seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            // 4. FrmVeriEslestirme'yi aç ve bilgileri aktar
            FrmVeriEslestirme frm = new FrmVeriEslestirme(kaynak, hedef,this);
            
            this.Hide();
            frm.Show();
        }

        private void FrmBaglantiAc_Load(object sender, EventArgs e)
        {
            // Başlangıçta devre dışı
            BtnDevam.Enabled = false;
            CmbboxKaynakVeritabani.Enabled = false;
            CmbboxHedefVeriTabani.Enabled = false;
        }
    }
}
