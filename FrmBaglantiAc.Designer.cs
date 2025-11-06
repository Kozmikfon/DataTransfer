namespace DataTransfer
{
    partial class FrmBaglantiAc
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            GrbboxKaynak = new GroupBox();
            CmbboxKaynakVeritabani = new ComboBox();
            lblKynkVeri = new Label();
            CkboxSifreGoster = new CheckBox();
            TxtSifre = new TextBox();
            label2 = new Label();
            TxtKullanıcı = new TextBox();
            label1 = new Label();
            LblKynkSunucu = new Label();
            TxtboxKaynakSunucu = new TextBox();
            GrbboxHedef = new GroupBox();
            LblHdfVeri = new Label();
            CmbboxHedefVeriTabani = new ComboBox();
            ChkboxHedefSifre = new CheckBox();
            TxboxHedefSifre = new TextBox();
            label4 = new Label();
            TxboxHedefKullanici = new TextBox();
            LblHedefKullanici = new Label();
            TxtboxHedefSunucu = new TextBox();
            LblHdfSunucu = new Label();
            BtnBaglantiTest = new Button();
            LstboxLog = new ListBox();
            BtnDevam = new Button();
            PrgsbarBaglanti = new ProgressBar();
            GrbboxKaynak.SuspendLayout();
            GrbboxHedef.SuspendLayout();
            SuspendLayout();
            // 
            // GrbboxKaynak
            // 
            GrbboxKaynak.BackColor = SystemColors.ButtonFace;
            GrbboxKaynak.Controls.Add(CmbboxKaynakVeritabani);
            GrbboxKaynak.Controls.Add(lblKynkVeri);
            GrbboxKaynak.Controls.Add(CkboxSifreGoster);
            GrbboxKaynak.Controls.Add(TxtSifre);
            GrbboxKaynak.Controls.Add(label2);
            GrbboxKaynak.Controls.Add(TxtKullanıcı);
            GrbboxKaynak.Controls.Add(label1);
            GrbboxKaynak.Controls.Add(LblKynkSunucu);
            GrbboxKaynak.Controls.Add(TxtboxKaynakSunucu);
            GrbboxKaynak.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 162);
            GrbboxKaynak.Location = new Point(12, 12);
            GrbboxKaynak.Name = "GrbboxKaynak";
            GrbboxKaynak.Size = new Size(354, 226);
            GrbboxKaynak.TabIndex = 17;
            GrbboxKaynak.TabStop = false;
            GrbboxKaynak.Text = "Kaynak";
            // 
            // CmbboxKaynakVeritabani
            // 
            CmbboxKaynakVeritabani.FormattingEnabled = true;
            CmbboxKaynakVeritabani.Location = new Point(129, 160);
            CmbboxKaynakVeritabani.Name = "CmbboxKaynakVeritabani";
            CmbboxKaynakVeritabani.Size = new Size(205, 23);
            CmbboxKaynakVeritabani.TabIndex = 25;
            CmbboxKaynakVeritabani.SelectedIndexChanged += CmbboxKaynakVeritabani_SelectedIndexChanged;
            // 
            // lblKynkVeri
            // 
            lblKynkVeri.AutoSize = true;
            lblKynkVeri.Font = new Font("Segoe UI", 11.25F);
            lblKynkVeri.Location = new Point(10, 163);
            lblKynkVeri.Name = "lblKynkVeri";
            lblKynkVeri.Size = new Size(87, 20);
            lblKynkVeri.TabIndex = 24;
            lblKynkVeri.Text = "Veri tabanı :";
            // 
            // CkboxSifreGoster
            // 
            CkboxSifreGoster.AutoSize = true;
            CkboxSifreGoster.Location = new Point(129, 135);
            CkboxSifreGoster.Name = "CkboxSifreGoster";
            CkboxSifreGoster.Size = new Size(86, 19);
            CkboxSifreGoster.TabIndex = 1;
            CkboxSifreGoster.Text = "Şifre Göster";
            CkboxSifreGoster.UseVisualStyleBackColor = true;
            CkboxSifreGoster.CheckedChanged += CkboxSifreGoster_CheckedChanged;
            // 
            // TxtSifre
            // 
            TxtSifre.Location = new Point(128, 103);
            TxtSifre.Multiline = true;
            TxtSifre.Name = "TxtSifre";
            TxtSifre.PasswordChar = '*';
            TxtSifre.Size = new Size(205, 23);
            TxtSifre.TabIndex = 23;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 162);
            label2.Location = new Point(8, 106);
            label2.Name = "label2";
            label2.Size = new Size(46, 20);
            label2.TabIndex = 22;
            label2.Text = "Şifre :";
            // 
            // TxtKullanıcı
            // 
            TxtKullanıcı.Location = new Point(128, 65);
            TxtKullanıcı.Name = "TxtKullanıcı";
            TxtKullanıcı.Size = new Size(206, 23);
            TxtKullanıcı.TabIndex = 21;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 162);
            label1.Location = new Point(6, 68);
            label1.Name = "label1";
            label1.Size = new Size(99, 20);
            label1.TabIndex = 20;
            label1.Text = "Kullanıcı Adı :";
            // 
            // LblKynkSunucu
            // 
            LblKynkSunucu.AutoSize = true;
            LblKynkSunucu.Font = new Font("Segoe UI", 11.25F);
            LblKynkSunucu.Location = new Point(6, 29);
            LblKynkSunucu.Name = "LblKynkSunucu";
            LblKynkSunucu.Size = new Size(63, 20);
            LblKynkSunucu.TabIndex = 1;
            LblKynkSunucu.Text = "Sunucu :";
            // 
            // TxtboxKaynakSunucu
            // 
            TxtboxKaynakSunucu.Location = new Point(129, 26);
            TxtboxKaynakSunucu.Name = "TxtboxKaynakSunucu";
            TxtboxKaynakSunucu.Size = new Size(205, 23);
            TxtboxKaynakSunucu.TabIndex = 7;
            // 
            // GrbboxHedef
            // 
            GrbboxHedef.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            GrbboxHedef.BackColor = SystemColors.ButtonFace;
            GrbboxHedef.Controls.Add(LblHdfVeri);
            GrbboxHedef.Controls.Add(CmbboxHedefVeriTabani);
            GrbboxHedef.Controls.Add(ChkboxHedefSifre);
            GrbboxHedef.Controls.Add(TxboxHedefSifre);
            GrbboxHedef.Controls.Add(label4);
            GrbboxHedef.Controls.Add(TxboxHedefKullanici);
            GrbboxHedef.Controls.Add(LblHedefKullanici);
            GrbboxHedef.Controls.Add(TxtboxHedefSunucu);
            GrbboxHedef.Controls.Add(LblHdfSunucu);
            GrbboxHedef.Location = new Point(414, 12);
            GrbboxHedef.Name = "GrbboxHedef";
            GrbboxHedef.Size = new Size(366, 226);
            GrbboxHedef.TabIndex = 18;
            GrbboxHedef.TabStop = false;
            GrbboxHedef.Text = "Hedef";
            // 
            // LblHdfVeri
            // 
            LblHdfVeri.AutoSize = true;
            LblHdfVeri.Font = new Font("Segoe UI", 11.25F);
            LblHdfVeri.Location = new Point(11, 167);
            LblHdfVeri.Name = "LblHdfVeri";
            LblHdfVeri.Size = new Size(88, 20);
            LblHdfVeri.TabIndex = 23;
            LblHdfVeri.Text = "Veri Tabanı :";
            // 
            // CmbboxHedefVeriTabani
            // 
            CmbboxHedefVeriTabani.FormattingEnabled = true;
            CmbboxHedefVeriTabani.Location = new Point(124, 164);
            CmbboxHedefVeriTabani.Name = "CmbboxHedefVeriTabani";
            CmbboxHedefVeriTabani.Size = new Size(202, 23);
            CmbboxHedefVeriTabani.TabIndex = 24;
            CmbboxHedefVeriTabani.SelectedIndexChanged += CmbboxHedefVeriTabani_SelectedIndexChanged;
            // 
            // ChkboxHedefSifre
            // 
            ChkboxHedefSifre.AutoSize = true;
            ChkboxHedefSifre.Location = new Point(124, 139);
            ChkboxHedefSifre.Name = "ChkboxHedefSifre";
            ChkboxHedefSifre.Size = new Size(86, 19);
            ChkboxHedefSifre.TabIndex = 22;
            ChkboxHedefSifre.Text = "Şifre Göster";
            ChkboxHedefSifre.UseVisualStyleBackColor = true;
            ChkboxHedefSifre.CheckedChanged += ChkboxHedefSifre_CheckedChanged;
            // 
            // TxboxHedefSifre
            // 
            TxboxHedefSifre.Location = new Point(123, 106);
            TxboxHedefSifre.Multiline = true;
            TxboxHedefSifre.Name = "TxboxHedefSifre";
            TxboxHedefSifre.PasswordChar = '*';
            TxboxHedefSifre.Size = new Size(203, 23);
            TxboxHedefSifre.TabIndex = 19;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 162);
            label4.Location = new Point(10, 109);
            label4.Name = "label4";
            label4.Size = new Size(46, 20);
            label4.TabIndex = 18;
            label4.Text = "Şifre :";
            // 
            // TxboxHedefKullanici
            // 
            TxboxHedefKullanici.Location = new Point(122, 65);
            TxboxHedefKullanici.Name = "TxboxHedefKullanici";
            TxboxHedefKullanici.Size = new Size(204, 23);
            TxboxHedefKullanici.TabIndex = 17;
            // 
            // LblHedefKullanici
            // 
            LblHedefKullanici.AutoSize = true;
            LblHedefKullanici.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 162);
            LblHedefKullanici.Location = new Point(7, 68);
            LblHedefKullanici.Name = "LblHedefKullanici";
            LblHedefKullanici.Size = new Size(99, 20);
            LblHedefKullanici.TabIndex = 16;
            LblHedefKullanici.Text = "Kullanıcı Adı :";
            // 
            // TxtboxHedefSunucu
            // 
            TxtboxHedefSunucu.Location = new Point(121, 29);
            TxtboxHedefSunucu.Name = "TxtboxHedefSunucu";
            TxtboxHedefSunucu.Size = new Size(205, 23);
            TxtboxHedefSunucu.TabIndex = 10;
            // 
            // LblHdfSunucu
            // 
            LblHdfSunucu.AutoSize = true;
            LblHdfSunucu.Font = new Font("Segoe UI", 11.25F);
            LblHdfSunucu.Location = new Point(7, 32);
            LblHdfSunucu.Name = "LblHdfSunucu";
            LblHdfSunucu.Size = new Size(63, 20);
            LblHdfSunucu.TabIndex = 4;
            LblHdfSunucu.Text = "Sunucu :";
            // 
            // BtnBaglantiTest
            // 
            BtnBaglantiTest.BackColor = Color.White;
            BtnBaglantiTest.Location = new Point(414, 260);
            BtnBaglantiTest.Name = "BtnBaglantiTest";
            BtnBaglantiTest.Size = new Size(106, 52);
            BtnBaglantiTest.TabIndex = 19;
            BtnBaglantiTest.Text = "Bağlantı Test Et";
            BtnBaglantiTest.UseVisualStyleBackColor = false;
            BtnBaglantiTest.Click += BtnBaglantiTest_Click;
            // 
            // LstboxLog
            // 
            LstboxLog.FormattingEnabled = true;
            LstboxLog.ItemHeight = 15;
            LstboxLog.Location = new Point(12, 260);
            LstboxLog.Name = "LstboxLog";
            LstboxLog.Size = new Size(354, 139);
            LstboxLog.TabIndex = 20;
            // 
            // BtnDevam
            // 
            BtnDevam.Location = new Point(685, 260);
            BtnDevam.Name = "BtnDevam";
            BtnDevam.Size = new Size(95, 52);
            BtnDevam.TabIndex = 21;
            BtnDevam.Text = "İlerle";
            BtnDevam.UseVisualStyleBackColor = true;
            BtnDevam.Click += BtnDevam_Click;
            // 
            // PrgsbarBaglanti
            // 
            PrgsbarBaglanti.Location = new Point(526, 307);
            PrgsbarBaglanti.Name = "PrgsbarBaglanti";
            PrgsbarBaglanti.Size = new Size(150, 29);
            PrgsbarBaglanti.Style = ProgressBarStyle.Continuous;
            PrgsbarBaglanti.TabIndex = 22;
            PrgsbarBaglanti.Visible = false;
            // 
            // FrmBaglantiAc
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(795, 430);
            Controls.Add(PrgsbarBaglanti);
            Controls.Add(BtnDevam);
            Controls.Add(LstboxLog);
            Controls.Add(BtnBaglantiTest);
            Controls.Add(GrbboxHedef);
            Controls.Add(GrbboxKaynak);
            Name = "FrmBaglantiAc";
            Text = "Bağlan";
            Load += FrmBaglantiAc_Load;
            GrbboxKaynak.ResumeLayout(false);
            GrbboxKaynak.PerformLayout();
            GrbboxHedef.ResumeLayout(false);
            GrbboxHedef.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox GrbboxKaynak;
        private TextBox TxtSifre;
        private Label label2;
        private TextBox TxtKullanıcı;
        private Label label1;
        private Label LblKynkSunucu;
        private TextBox TxtboxKaynakSunucu;
        private CheckBox CkboxSifreGoster;
        private GroupBox GrbboxHedef;
        private CheckBox ChkboxHedefSifre;
        private TextBox TxboxHedefSifre;
        private Label label4;
        private TextBox TxboxHedefKullanici;
        private Label LblHedefKullanici;
        private TextBox TxtboxHedefSunucu;
        private Label LblHdfSunucu;
        private Button BtnBaglantiTest;
        private ComboBox CmbboxKaynakVeritabani;
        private Label lblKynkVeri;
        private Label LblHdfVeri;
        private ComboBox CmbboxHedefVeriTabani;
        private ListBox LstboxLog;
        private Button BtnDevam;
        private ProgressBar PrgsbarBaglanti;
    }
}