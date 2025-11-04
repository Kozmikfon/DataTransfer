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

            ProgresBarBaslat();

            try
            {
                ProgresBarGuncelle(10);
                KaynakBaglanti = new BaglantiBilgileri
                {
                    Sunucu = TxtboxKaynakSunucu.Text.Trim(),
                    Kullanici = TxtKullanıcı.Text.Trim(),
                    Sifre = TxtSifre.Text.Trim()
                };

                HedefBaglanti = new BaglantiBilgileri
                {
                    Sunucu = TxtboxHedefSunucu.Text.Trim(),
                    Kullanici = TxboxHedefKullanici.Text.Trim(),
                    Sifre = TxboxHedefSifre.Text.Trim()
                };

                ProgresBarGuncelle(50);

                kaynakTestBasarili = await BaglantiTestAsync(KaynakBaglanti);
                ProgresBarGuncelle(55);

                hedefTestBasarili = await BaglantiTestAsync(HedefBaglanti);
                ProgresBarGuncelle(75);

                if (kaynakTestBasarili && hedefTestBasarili)
                {
                    ProgresBarGuncelle(85);

                    await KaynakVeritabaniComboboxDoldur(KaynakBaglanti);
                    ProgresBarGuncelle(92);

                    await HedefVeritabaniComboboxDoldur(HedefBaglanti);
                    ProgresBarGuncelle(100);

                    ProgresBarBitir(true);

                    LstboxLog.ForeColor = Color.Green;
                    LogEkle($"[{DateTime.Now:HH:mm:ss}] Kaynak ve Hedef bağlantıları başarıyla açıldı.");

                    MessageBox.Show("Bağlantılar başarılı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    CmbboxKaynakVeritabani.Enabled = true;
                    CmbboxHedefVeriTabani.Enabled = true;
                    BtnDevam.Enabled = true;
                }
                else
                {
                    ProgresBarBitir(false);
                    MessageBox.Show("Bağlantılardan biri veya her ikisi başarısız.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    BtnDevam.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                ProgresBarBitir(false);
                LstboxLog.ForeColor = Color.Red;
                LogEkle($"[{DateTime.Now:HH:mm:ss}] Hata: {ex.Message}");
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
                    LogEkle($"[{DateTime.Now:HH:mm:ss}] {info.Sunucu} bağlantısı başarılı.");
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
                LogEkle($"[{DateTime.Now:HH:mm:ss}] {info.Sunucu} bağlantı hatası: {mesaj}");
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

                string mesaj = ex is SqlException sqlEx ? sqlEx.Number switch
                {
                    18456 => "SQL kullanıcı adı veya şifre hatalı.",
                    4060 => "Bu kullanıcı seçilen veritabanına erişemez.",
                    229 => "Veritabanı erişim izniniz yok.",
                    _ => sqlEx.Message
                } : ex.Message;

                LstboxLog.ForeColor = Color.Red;
                LogEkle($"[{DateTime.Now:HH:mm:ss}] ({info.Sunucu}) Hata: {mesaj}");

                MessageBox.Show(mesaj, "Veritabanı Erişim Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
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


                string mesaj = ex is SqlException sqlEx ? sqlEx.Number switch
                {
                    18456 => "SQL kullanıcı adı veya şifre hatalı.",
                    4060 => "Bu kullanıcı seçilen veritabanına erişemez.",
                    229 => "Veritabanı erişim izniniz yok.",
                    _ => sqlEx.Message
                } : ex.Message;

                LstboxLog.ForeColor = Color.Red;
                LogEkle($"[{DateTime.Now:HH:mm:ss}] ({info.Sunucu}) Hata: {mesaj}");

                MessageBox.Show(mesaj, "Veritabanı Erişim Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
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
            try
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

                // Bağlantı bilgilerini oluştur
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

                // Yeni formu güvenli şekilde aç
                FrmVeriEslestirme frm = null;

                try
                {
                    frm = new FrmVeriEslestirme(kaynak, hedef, this);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"FrmVeriEslestirme oluşturulurken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    this.Hide();
                    frm.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Yeni form açılırken hata oluştu:\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Show(); // Eğer hata olursa geri getir
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"İşlem sırasında beklenmeyen bir hata oluştu:\n{ex.Message}", "Kritik Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void FrmBaglantiAc_Load(object sender, EventArgs e)
        {
            // Başlangıçta devre dışı
            BtnDevam.Enabled = false;
            CmbboxKaynakVeritabani.Enabled = false;
            CmbboxHedefVeriTabani.Enabled = false;
        }

        private async void CmbboxKaynakVeritabani_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedDb = CmbboxKaynakVeritabani.Text;
            if (string.IsNullOrWhiteSpace(selectedDb)) return;

            try
            {
                // Kullanıcının seçtiği veritabanına erişimi var mı kontrol et
                string connStr = $"Server={TxtboxKaynakSunucu.Text.Trim()};Database={selectedDb};User Id={TxtKullanıcı.Text.Trim()};Password={TxtSifre.Text.Trim()};Connect Timeout=5;TrustServerCertificate=True;";

                using (var conn = new SqlConnection(connStr))
                {
                    await conn.OpenAsync();

                    // Basit bir test sorgusu çalıştıralım
                    using (var cmd = new SqlCommand("SELECT TOP 1 name FROM sys.tables", conn))
                    {
                        await cmd.ExecuteScalarAsync();
                    }

                    LstboxLog.ForeColor = Color.Green;
                    LogEkle($"[{DateTime.Now:HH:mm:ss}] '{selectedDb}' veritabanına erişim doğrulandı.");
                }
            }
            catch (SqlException ex)
            {
                string mesaj = ex.Number switch
                {
                    4060 => "Bu veritabanına erişim izniniz yok.",
                    18456 => "Kullanıcı adı veya şifre hatalı.",
                    229 => "Veritabanı nesnelerine erişim yetkiniz yok.",
                    _ => $"SQL Hatası ({ex.Number}): {ex.Message}"
                };

                LstboxLog.ForeColor = Color.Red;
                LogEkle($"[{DateTime.Now:HH:mm:ss}] '{selectedDb}' erişim hatası: {mesaj}");
                MessageBox.Show(mesaj, "Erişim Engellendi", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Hatalı seçim durumunda geri al
                CmbboxKaynakVeritabani.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Beklenmeyen hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CmbboxKaynakVeritabani.SelectedIndex = -1;
            }
        }

        private async void CmbboxHedefVeriTabani_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedDb = CmbboxHedefVeriTabani.Text;
            if (string.IsNullOrWhiteSpace(selectedDb)) return;

            try
            {
                string connStr = $"Server={TxtboxHedefSunucu.Text.Trim()};Database={selectedDb};User Id={TxboxHedefKullanici.Text.Trim()};Password={TxboxHedefSifre.Text.Trim()};Connect Timeout=5;TrustServerCertificate=True;";
                using (var conn = new SqlConnection(connStr))
                {
                    await conn.OpenAsync();
                    using (var cmd = new SqlCommand("SELECT TOP 1 name FROM sys.tables", conn))
                    {
                        await cmd.ExecuteScalarAsync();
                    }

                    LstboxLog.ForeColor = Color.Green;
                    LogEkle($"[{DateTime.Now:HH:mm:ss}] '{selectedDb}' veritabanına erişim doğrulandı.");
                }
            }
            catch (SqlException ex)
            {
                string mesaj = ex.Number switch
                {
                    4060 => "Bu veritabanına erişim izniniz yok.",
                    18456 => "Kullanıcı adı veya şifre hatalı.",
                    229 => "Veritabanı nesnelerine erişim yetkiniz yok.",
                    _ => $"SQL Hatası ({ex.Number}): {ex.Message}"
                };

                LstboxLog.ForeColor = Color.Red;
                LogEkle($"[{DateTime.Now:HH:mm:ss}] '{selectedDb}' erişim hatası: {mesaj}");
                MessageBox.Show(mesaj, "Erişim Engellendi", MessageBoxButtons.OK, MessageBoxIcon.Error);

                CmbboxHedefVeriTabani.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Beklenmeyen hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CmbboxHedefVeriTabani.SelectedIndex = -1;
            }
        }
        private void ProgresBarBaslat()
        {
            PrgsbarBaglanti.Visible = true;
            PrgsbarBaglanti.Value = 0;
            PrgsbarBaglanti.ForeColor = Color.DodgerBlue;
            PrgsbarBaglanti.Refresh();
        }
        private void ProgresBarGuncelle(int value)
        {
            if (value < 0)
                value = 0;

            if (value > 100)
                value = 100;

            PrgsbarBaglanti.Value = value;
            PrgsbarBaglanti.Refresh();

        }
        private void ProgresBarBitir(bool basarili)
        {
            PrgsbarBaglanti.Value = 100;
            PrgsbarBaglanti.ForeColor = Color.Green;

            PrgsbarBaglanti.Refresh();
            Task.Delay(500).Wait();
            PrgsbarBaglanti.Visible = false;
        }

        private void LogEkle(string mesaj)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(LogEkle), mesaj);
                return;
            }

            LogEkle($"[{DateTime.Now:HH:mm:ss}] {mesaj}");
            LstboxLog.TopIndex = LstboxLog.Items.Count - 1; //scroll otomatik alta
        }
    }
}
