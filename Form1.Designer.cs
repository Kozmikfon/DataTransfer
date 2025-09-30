namespace DataTransfer
{
    partial class FrmVeriEslestirme
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            BtnBaglantiTest = new Button();
            LblKynkSunucu = new Label();
            lblKynkVeri = new Label();
            LblKynkTablo = new Label();
            LblHdfSunucu = new Label();
            LblHdfVeri = new Label();
            LblHdfTablo = new Label();
            TxtboxKaynakSunucu = new TextBox();
            CmbboxKaynakVeritabani = new ComboBox();
            CmbboxKaynaktablo = new ComboBox();
            TxtboxHedefSunucu = new TextBox();
            CmbboxHedefVeriTabani = new ComboBox();
            CmbboxHedefTablo = new ComboBox();
            BtnDogrula = new Button();
            BtnTransferBaslat = new Button();
            GrbboxKaynak = new GroupBox();
            TxtSifre = new TextBox();
            label2 = new Label();
            TxtKullanıcı = new TextBox();
            label1 = new Label();
            GrdKaynak = new DataGridView();
            LblKynkSutun = new Label();
            BtnKynkKolonYukle = new Button();
            GrbboxHedef = new GroupBox();
            GrdHedef = new DataGridView();
            LblHdfSutun = new Label();
            BtnHedefKolonYukle = new Button();
            GrbboxButon = new GroupBox();
            GrbboxEslesmeLog = new GroupBox();
            LstboxEslesmeLog = new ListBox();
            GrbboxKaynak.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GrdKaynak).BeginInit();
            GrbboxHedef.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GrdHedef).BeginInit();
            GrbboxButon.SuspendLayout();
            GrbboxEslesmeLog.SuspendLayout();
            SuspendLayout();
            // 
            // BtnBaglantiTest
            // 
            BtnBaglantiTest.Location = new Point(85, 23);
            BtnBaglantiTest.Name = "BtnBaglantiTest";
            BtnBaglantiTest.Size = new Size(146, 56);
            BtnBaglantiTest.TabIndex = 0;
            BtnBaglantiTest.Text = "Bağlantı Test Et";
            BtnBaglantiTest.UseVisualStyleBackColor = true;
            BtnBaglantiTest.Click += BtnBaglantiTest_Click;
            // 
            // LblKynkSunucu
            // 
            LblKynkSunucu.AutoSize = true;
            LblKynkSunucu.Font = new Font("Segoe UI", 11.25F);
            LblKynkSunucu.Location = new Point(12, 56);
            LblKynkSunucu.Name = "LblKynkSunucu";
            LblKynkSunucu.Size = new Size(63, 20);
            LblKynkSunucu.TabIndex = 1;
            LblKynkSunucu.Text = "Sunucu :";
            // 
            // lblKynkVeri
            // 
            lblKynkVeri.AutoSize = true;
            lblKynkVeri.Font = new Font("Segoe UI", 11.25F);
            lblKynkVeri.Location = new Point(9, 103);
            lblKynkVeri.Name = "lblKynkVeri";
            lblKynkVeri.Size = new Size(87, 20);
            lblKynkVeri.TabIndex = 2;
            lblKynkVeri.Text = "Veri tabanı :";
            lblKynkVeri.Click += lblKynkVeri_Click;
            // 
            // LblKynkTablo
            // 
            LblKynkTablo.AutoSize = true;
            LblKynkTablo.Font = new Font("Segoe UI", 11.25F);
            LblKynkTablo.Location = new Point(9, 149);
            LblKynkTablo.Name = "LblKynkTablo";
            LblKynkTablo.Size = new Size(79, 20);
            LblKynkTablo.TabIndex = 3;
            LblKynkTablo.Text = "Tablo Adı :";
            // 
            // LblHdfSunucu
            // 
            LblHdfSunucu.AutoSize = true;
            LblHdfSunucu.Font = new Font("Segoe UI", 11.25F);
            LblHdfSunucu.Location = new Point(15, 57);
            LblHdfSunucu.Name = "LblHdfSunucu";
            LblHdfSunucu.Size = new Size(63, 20);
            LblHdfSunucu.TabIndex = 4;
            LblHdfSunucu.Text = "Sunucu :";
            // 
            // LblHdfVeri
            // 
            LblHdfVeri.AutoSize = true;
            LblHdfVeri.Font = new Font("Segoe UI", 11.25F);
            LblHdfVeri.Location = new Point(17, 103);
            LblHdfVeri.Name = "LblHdfVeri";
            LblHdfVeri.Size = new Size(88, 20);
            LblHdfVeri.TabIndex = 5;
            LblHdfVeri.Text = "Veri Tabanı :";
            LblHdfVeri.Click += LblHdfVeri_Click;
            // 
            // LblHdfTablo
            // 
            LblHdfTablo.AutoSize = true;
            LblHdfTablo.Font = new Font("Segoe UI", 11.25F);
            LblHdfTablo.Location = new Point(17, 149);
            LblHdfTablo.Name = "LblHdfTablo";
            LblHdfTablo.Size = new Size(79, 20);
            LblHdfTablo.TabIndex = 6;
            LblHdfTablo.Text = "Tablo Adı :";
            // 
            // TxtboxKaynakSunucu
            // 
            TxtboxKaynakSunucu.Location = new Point(136, 55);
            TxtboxKaynakSunucu.Name = "TxtboxKaynakSunucu";
            TxtboxKaynakSunucu.Size = new Size(127, 23);
            TxtboxKaynakSunucu.TabIndex = 7;
            TxtboxKaynakSunucu.TextChanged += TxtboxKaynakSunucu_TextChanged;
            // 
            // CmbboxKaynakVeritabani
            // 
            CmbboxKaynakVeritabani.FormattingEnabled = true;
            CmbboxKaynakVeritabani.Location = new Point(136, 100);
            CmbboxKaynakVeritabani.Name = "CmbboxKaynakVeritabani";
            CmbboxKaynakVeritabani.Size = new Size(127, 23);
            CmbboxKaynakVeritabani.TabIndex = 8;
            CmbboxKaynakVeritabani.SelectedIndexChanged += CmbboxKaynakVeritabani_SelectedIndexChanged;
            // 
            // CmbboxKaynaktablo
            // 
            CmbboxKaynaktablo.FormattingEnabled = true;
            CmbboxKaynaktablo.Location = new Point(136, 146);
            CmbboxKaynaktablo.Name = "CmbboxKaynaktablo";
            CmbboxKaynaktablo.Size = new Size(127, 23);
            CmbboxKaynaktablo.TabIndex = 9;
            // 
            // TxtboxHedefSunucu
            // 
            TxtboxHedefSunucu.Location = new Point(130, 56);
            TxtboxHedefSunucu.Name = "TxtboxHedefSunucu";
            TxtboxHedefSunucu.Size = new Size(129, 23);
            TxtboxHedefSunucu.TabIndex = 10;
            // 
            // CmbboxHedefVeriTabani
            // 
            CmbboxHedefVeriTabani.FormattingEnabled = true;
            CmbboxHedefVeriTabani.Location = new Point(130, 100);
            CmbboxHedefVeriTabani.Name = "CmbboxHedefVeriTabani";
            CmbboxHedefVeriTabani.Size = new Size(128, 23);
            CmbboxHedefVeriTabani.TabIndex = 11;
            // 
            // CmbboxHedefTablo
            // 
            CmbboxHedefTablo.FormattingEnabled = true;
            CmbboxHedefTablo.Location = new Point(130, 146);
            CmbboxHedefTablo.Name = "CmbboxHedefTablo";
            CmbboxHedefTablo.Size = new Size(128, 23);
            CmbboxHedefTablo.TabIndex = 12;
            // 
            // BtnDogrula
            // 
            BtnDogrula.Location = new Point(184, 100);
            BtnDogrula.Name = "BtnDogrula";
            BtnDogrula.Size = new Size(121, 42);
            BtnDogrula.TabIndex = 14;
            BtnDogrula.Text = "Tablo Eşleşmesini Doğrula";
            BtnDogrula.UseVisualStyleBackColor = true;
            BtnDogrula.Click += BtnVeriAktarim_Click;
            // 
            // BtnTransferBaslat
            // 
            BtnTransferBaslat.Location = new Point(28, 99);
            BtnTransferBaslat.Name = "BtnTransferBaslat";
            BtnTransferBaslat.Size = new Size(118, 45);
            BtnTransferBaslat.TabIndex = 15;
            BtnTransferBaslat.Text = "Veri Transferini Başlat";
            BtnTransferBaslat.UseVisualStyleBackColor = true;
            // 
            // GrbboxKaynak
            // 
            GrbboxKaynak.Controls.Add(TxtSifre);
            GrbboxKaynak.Controls.Add(label2);
            GrbboxKaynak.Controls.Add(TxtKullanıcı);
            GrbboxKaynak.Controls.Add(label1);
            GrbboxKaynak.Controls.Add(GrdKaynak);
            GrbboxKaynak.Controls.Add(LblKynkSutun);
            GrbboxKaynak.Controls.Add(BtnKynkKolonYukle);
            GrbboxKaynak.Controls.Add(LblKynkSunucu);
            GrbboxKaynak.Controls.Add(lblKynkVeri);
            GrbboxKaynak.Controls.Add(LblKynkTablo);
            GrbboxKaynak.Controls.Add(CmbboxKaynaktablo);
            GrbboxKaynak.Controls.Add(TxtboxKaynakSunucu);
            GrbboxKaynak.Controls.Add(CmbboxKaynakVeritabani);
            GrbboxKaynak.Dock = DockStyle.Left;
            GrbboxKaynak.Location = new Point(0, 0);
            GrbboxKaynak.Name = "GrbboxKaynak";
            GrbboxKaynak.Size = new Size(489, 667);
            GrbboxKaynak.TabIndex = 16;
            GrbboxKaynak.TabStop = false;
            GrbboxKaynak.Text = "Kaynak";
            GrbboxKaynak.Enter += GrbboxKaynak_Enter;
            // 
            // TxtSifre
            // 
            TxtSifre.Location = new Point(135, 233);
            TxtSifre.Name = "TxtSifre";
            TxtSifre.Size = new Size(128, 23);
            TxtSifre.TabIndex = 23;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 162);
            label2.Location = new Point(12, 236);
            label2.Name = "label2";
            label2.Size = new Size(46, 20);
            label2.TabIndex = 22;
            label2.Text = "Şifre :";
            // 
            // TxtKullanıcı
            // 
            TxtKullanıcı.Location = new Point(135, 189);
            TxtKullanıcı.Name = "TxtKullanıcı";
            TxtKullanıcı.Size = new Size(128, 23);
            TxtKullanıcı.TabIndex = 21;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 162);
            label1.Location = new Point(12, 192);
            label1.Name = "label1";
            label1.Size = new Size(99, 20);
            label1.TabIndex = 20;
            label1.Text = "Kullanıcı Adı :";
            // 
            // GrdKaynak
            // 
            GrdKaynak.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            GrdKaynak.Location = new Point(23, 397);
            GrdKaynak.Name = "GrdKaynak";
            GrdKaynak.Size = new Size(449, 239);
            GrdKaynak.TabIndex = 19;
            GrdKaynak.CellContentClick += GrdKaynak_CellContentClick;
            // 
            // LblKynkSutun
            // 
            LblKynkSutun.AutoSize = true;
            LblKynkSutun.Location = new Point(24, 345);
            LblKynkSutun.Name = "LblKynkSutun";
            LblKynkSutun.Size = new Size(72, 15);
            LblKynkSutun.TabIndex = 18;
            LblKynkSutun.Text = "datagridvew";
            // 
            // BtnKynkKolonYukle
            // 
            BtnKynkKolonYukle.Location = new Point(37, 296);
            BtnKynkKolonYukle.Name = "BtnKynkKolonYukle";
            BtnKynkKolonYukle.Size = new Size(216, 29);
            BtnKynkKolonYukle.TabIndex = 10;
            BtnKynkKolonYukle.Text = "Kolonları Yükle";
            BtnKynkKolonYukle.UseVisualStyleBackColor = true;
            BtnKynkKolonYukle.Click += BtnKynkKolonYukle_Click;
            // 
            // GrbboxHedef
            // 
            GrbboxHedef.Controls.Add(GrdHedef);
            GrbboxHedef.Controls.Add(LblHdfSutun);
            GrbboxHedef.Controls.Add(BtnHedefKolonYukle);
            GrbboxHedef.Controls.Add(CmbboxHedefVeriTabani);
            GrbboxHedef.Controls.Add(CmbboxHedefTablo);
            GrbboxHedef.Controls.Add(TxtboxHedefSunucu);
            GrbboxHedef.Controls.Add(LblHdfVeri);
            GrbboxHedef.Controls.Add(LblHdfSunucu);
            GrbboxHedef.Controls.Add(LblHdfTablo);
            GrbboxHedef.Dock = DockStyle.Right;
            GrbboxHedef.Location = new Point(802, 0);
            GrbboxHedef.Name = "GrbboxHedef";
            GrbboxHedef.Size = new Size(306, 667);
            GrbboxHedef.TabIndex = 17;
            GrbboxHedef.TabStop = false;
            GrbboxHedef.Text = "Hedef";
            // 
            // GrdHedef
            // 
            GrdHedef.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            GrdHedef.Location = new Point(30, 462);
            GrdHedef.Name = "GrdHedef";
            GrdHedef.Size = new Size(354, 174);
            GrdHedef.TabIndex = 15;
            GrdHedef.CellContentClick += GrdHedef_CellContentClick;
            // 
            // LblHdfSutun
            // 
            LblHdfSutun.AutoSize = true;
            LblHdfSutun.Location = new Point(19, 340);
            LblHdfSutun.Name = "LblHdfSutun";
            LblHdfSutun.Size = new Size(75, 15);
            LblHdfSutun.TabIndex = 14;
            LblHdfSutun.Text = "datagirdview";
            // 
            // BtnHedefKolonYukle
            // 
            BtnHedefKolonYukle.Location = new Point(39, 216);
            BtnHedefKolonYukle.Name = "BtnHedefKolonYukle";
            BtnHedefKolonYukle.Size = new Size(219, 26);
            BtnHedefKolonYukle.TabIndex = 13;
            BtnHedefKolonYukle.Text = "Kolonları Yükle";
            BtnHedefKolonYukle.UseVisualStyleBackColor = true;
            BtnHedefKolonYukle.Click += BtnHedefKolonYukle_Click;
            // 
            // GrbboxButon
            // 
            GrbboxButon.Controls.Add(BtnBaglantiTest);
            GrbboxButon.Controls.Add(BtnDogrula);
            GrbboxButon.Controls.Add(BtnTransferBaslat);
            GrbboxButon.Dock = DockStyle.Top;
            GrbboxButon.Location = new Point(489, 0);
            GrbboxButon.Name = "GrbboxButon";
            GrbboxButon.Size = new Size(313, 224);
            GrbboxButon.TabIndex = 20;
            GrbboxButon.TabStop = false;
            // 
            // GrbboxEslesmeLog
            // 
            GrbboxEslesmeLog.Controls.Add(LstboxEslesmeLog);
            GrbboxEslesmeLog.Dock = DockStyle.Bottom;
            GrbboxEslesmeLog.Location = new Point(489, 216);
            GrbboxEslesmeLog.Name = "GrbboxEslesmeLog";
            GrbboxEslesmeLog.Size = new Size(313, 451);
            GrbboxEslesmeLog.TabIndex = 21;
            GrbboxEslesmeLog.TabStop = false;
            GrbboxEslesmeLog.Text = "groupBox4";
            // 
            // LstboxEslesmeLog
            // 
            LstboxEslesmeLog.FormattingEnabled = true;
            LstboxEslesmeLog.ItemHeight = 15;
            LstboxEslesmeLog.Location = new Point(6, 22);
            LstboxEslesmeLog.Name = "LstboxEslesmeLog";
            LstboxEslesmeLog.Size = new Size(292, 169);
            LstboxEslesmeLog.TabIndex = 0;
            LstboxEslesmeLog.SelectedIndexChanged += LstboxEslesmeLog_SelectedIndexChanged;
            // 
            // FrmVeriEslestirme
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1108, 667);
            Controls.Add(GrbboxEslesmeLog);
            Controls.Add(GrbboxButon);
            Controls.Add(GrbboxHedef);
            Controls.Add(GrbboxKaynak);
            Name = "FrmVeriEslestirme";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Veri Aktarımı";
            Load += Form1_Load;
            GrbboxKaynak.ResumeLayout(false);
            GrbboxKaynak.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GrdKaynak).EndInit();
            GrbboxHedef.ResumeLayout(false);
            GrbboxHedef.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GrdHedef).EndInit();
            GrbboxButon.ResumeLayout(false);
            GrbboxEslesmeLog.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Button BtnBaglantiTest;
        private Label LblKynkSunucu;
        private Label lblKynkVeri;
        private Label LblKynkTablo;
        private Label LblHdfSunucu;
        private Label LblHdfVeri;
        private Label LblHdfTablo;
        private TextBox TxtboxKaynakSunucu;
        private ComboBox CmbboxKaynakVeritabani;
        private ComboBox CmbboxKaynaktablo;
        private TextBox TxtboxHedefSunucu;
        private ComboBox CmbboxHedefVeriTabani;
        private ComboBox CmbboxHedefTablo;
        private Button BtnDogrula;
        private Button BtnTransferBaslat;
        private GroupBox GrbboxKaynak;
        private GroupBox GrbboxHedef;
        private Button BtnKynkKolonYukle;
        private Button BtnHedefKolonYukle;
        private DataGridView GrdKaynak;
        private Label LblKynkSutun;
        private DataGridView GrdHedef;
        private Label LblHdfSutun;
        private GroupBox GrbboxButon;
        private GroupBox GrbboxEslesmeLog;
        private ListBox LstboxEslesmeLog;
        private TextBox TxtSifre;
        private Label label2;
        private TextBox TxtKullanıcı;
        private Label label1;
    }
}
