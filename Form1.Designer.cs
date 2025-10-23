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
            components = new System.ComponentModel.Container();
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
            BtnEslesmeDogrula = new Button();
            BtnTransferBaslat = new Button();
            GrbboxKaynak = new GroupBox();
            CkboxSifreGoster = new CheckBox();
            CmboxKaynakSutun = new ComboBox();
            TxtSifre = new TextBox();
            label3 = new Label();
            label2 = new Label();
            TxtKullanıcı = new TextBox();
            label1 = new Label();
            GrdKaynak = new DataGridView();
            BtnKynkKolonYukle = new Button();
            GrbboxHedef = new GroupBox();
            ChkboxHedefSifre = new CheckBox();
            CmboxHedefSutun = new ComboBox();
            TxboxHedefSifre = new TextBox();
            label4 = new Label();
            TxboxHedefKullanici = new TextBox();
            LblHedefSutun = new Label();
            LblHedefKullanici = new Label();
            GrdHedef = new DataGridView();
            BtnHedefKolonYukle = new Button();
            GrbboxButon = new GroupBox();
            LstboxLog = new ListBox();
            GrbboxEslesmeLog = new GroupBox();
            PrgsbarTransfer = new ProgressBar();
            GrdEslestirme = new DataGridView();
            KaynakSutun = new DataGridViewButtonColumn();
            HedefSutun = new DataGridViewTextBoxColumn();
            Uygunluk = new DataGridViewTextBoxColumn();
            Sil = new DataGridViewButtonColumn();
            timer1 = new System.Windows.Forms.Timer(components);
            GrbboxKaynak.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GrdKaynak).BeginInit();
            GrbboxHedef.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GrdHedef).BeginInit();
            GrbboxButon.SuspendLayout();
            GrbboxEslesmeLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GrdEslestirme).BeginInit();
            SuspendLayout();
            // 
            // BtnBaglantiTest
            // 
            BtnBaglantiTest.BackColor = Color.White;
            BtnBaglantiTest.Location = new Point(19, 16);
            BtnBaglantiTest.Name = "BtnBaglantiTest";
            BtnBaglantiTest.Size = new Size(146, 42);
            BtnBaglantiTest.TabIndex = 0;
            BtnBaglantiTest.Text = "Bağlantı Test Et";
            BtnBaglantiTest.UseVisualStyleBackColor = false;
            BtnBaglantiTest.Click += BtnBaglantiTest_Click;
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
            // lblKynkVeri
            // 
            lblKynkVeri.AutoSize = true;
            lblKynkVeri.Font = new Font("Segoe UI", 11.25F);
            lblKynkVeri.Location = new Point(0, 168);
            lblKynkVeri.Name = "lblKynkVeri";
            lblKynkVeri.Size = new Size(87, 20);
            lblKynkVeri.TabIndex = 2;
            lblKynkVeri.Text = "Veri tabanı :";
            // 
            // LblKynkTablo
            // 
            LblKynkTablo.AutoSize = true;
            LblKynkTablo.Font = new Font("Segoe UI", 11.25F);
            LblKynkTablo.Location = new Point(1, 214);
            LblKynkTablo.Name = "LblKynkTablo";
            LblKynkTablo.Size = new Size(79, 20);
            LblKynkTablo.TabIndex = 3;
            LblKynkTablo.Text = "Tablo Adı :";
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
            // LblHdfVeri
            // 
            LblHdfVeri.AutoSize = true;
            LblHdfVeri.Font = new Font("Segoe UI", 11.25F);
            LblHdfVeri.Location = new Point(10, 170);
            LblHdfVeri.Name = "LblHdfVeri";
            LblHdfVeri.Size = new Size(88, 20);
            LblHdfVeri.TabIndex = 5;
            LblHdfVeri.Text = "Veri Tabanı :";
            // 
            // LblHdfTablo
            // 
            LblHdfTablo.AutoSize = true;
            LblHdfTablo.Font = new Font("Segoe UI", 11.25F);
            LblHdfTablo.Location = new Point(9, 213);
            LblHdfTablo.Name = "LblHdfTablo";
            LblHdfTablo.Size = new Size(79, 20);
            LblHdfTablo.TabIndex = 6;
            LblHdfTablo.Text = "Tablo Adı :";
            // 
            // TxtboxKaynakSunucu
            // 
            TxtboxKaynakSunucu.Location = new Point(129, 26);
            TxtboxKaynakSunucu.Name = "TxtboxKaynakSunucu";
            TxtboxKaynakSunucu.Size = new Size(127, 23);
            TxtboxKaynakSunucu.TabIndex = 7;
            // 
            // CmbboxKaynakVeritabani
            // 
            CmbboxKaynakVeritabani.FormattingEnabled = true;
            CmbboxKaynakVeritabani.Location = new Point(128, 165);
            CmbboxKaynakVeritabani.Name = "CmbboxKaynakVeritabani";
            CmbboxKaynakVeritabani.Size = new Size(127, 23);
            CmbboxKaynakVeritabani.TabIndex = 8;
            CmbboxKaynakVeritabani.DrawItem += CmbboxKaynakVeritabani_DrawItem;
            CmbboxKaynakVeritabani.SelectedIndexChanged += CmbboxKaynakVeritabani_SelectedIndexChanged;
            // 
            // CmbboxKaynaktablo
            // 
            CmbboxKaynaktablo.FormattingEnabled = true;
            CmbboxKaynaktablo.Location = new Point(128, 211);
            CmbboxKaynaktablo.Name = "CmbboxKaynaktablo";
            CmbboxKaynaktablo.Size = new Size(127, 23);
            CmbboxKaynaktablo.TabIndex = 9;
            CmbboxKaynaktablo.DrawItem += CmbboxKaynaktablo_DrawItem;
            CmbboxKaynaktablo.SelectedIndexChanged += CmbboxKaynaktablo_SelectedIndexChanged;
            // 
            // TxtboxHedefSunucu
            // 
            TxtboxHedefSunucu.Location = new Point(121, 29);
            TxtboxHedefSunucu.Name = "TxtboxHedefSunucu";
            TxtboxHedefSunucu.Size = new Size(129, 23);
            TxtboxHedefSunucu.TabIndex = 10;
            // 
            // CmbboxHedefVeriTabani
            // 
            CmbboxHedefVeriTabani.FormattingEnabled = true;
            CmbboxHedefVeriTabani.Location = new Point(123, 167);
            CmbboxHedefVeriTabani.Name = "CmbboxHedefVeriTabani";
            CmbboxHedefVeriTabani.Size = new Size(129, 23);
            CmbboxHedefVeriTabani.TabIndex = 11;
            CmbboxHedefVeriTabani.DrawItem += CmbboxHedefVeriTabani_DrawItem;
            CmbboxHedefVeriTabani.SelectedIndexChanged += CmbboxHedefVeriTabani_SelectedIndexChanged;
            // 
            // CmbboxHedefTablo
            // 
            CmbboxHedefTablo.FormattingEnabled = true;
            CmbboxHedefTablo.Location = new Point(122, 210);
            CmbboxHedefTablo.Name = "CmbboxHedefTablo";
            CmbboxHedefTablo.Size = new Size(129, 23);
            CmbboxHedefTablo.TabIndex = 12;
            CmbboxHedefTablo.DrawItem += CmbboxHedefTablo_DrawItem;
            CmbboxHedefTablo.SelectedIndexChanged += CmbboxHedefTablo_SelectedIndexChanged;
            // 
            // BtnEslesmeDogrula
            // 
            BtnEslesmeDogrula.Location = new Point(302, 273);
            BtnEslesmeDogrula.Name = "BtnEslesmeDogrula";
            BtnEslesmeDogrula.Size = new Size(134, 52);
            BtnEslesmeDogrula.TabIndex = 14;
            BtnEslesmeDogrula.Text = "Tablo Eşleşmesini Doğrula";
            BtnEslesmeDogrula.UseVisualStyleBackColor = true;
            BtnEslesmeDogrula.Click += BtnEslesmeDogrula_Click;
            // 
            // BtnTransferBaslat
            // 
            BtnTransferBaslat.Location = new Point(116, 273);
            BtnTransferBaslat.Name = "BtnTransferBaslat";
            BtnTransferBaslat.Size = new Size(123, 52);
            BtnTransferBaslat.TabIndex = 15;
            BtnTransferBaslat.Text = "Veri Transferini Başlat";
            BtnTransferBaslat.UseVisualStyleBackColor = true;
            BtnTransferBaslat.Click += BtnTransferBaslat_Click;
            // 
            // GrbboxKaynak
            // 
            GrbboxKaynak.BackColor = SystemColors.GradientInactiveCaption;
            GrbboxKaynak.Controls.Add(CkboxSifreGoster);
            GrbboxKaynak.Controls.Add(CmbboxKaynakVeritabani);
            GrbboxKaynak.Controls.Add(CmboxKaynakSutun);
            GrbboxKaynak.Controls.Add(TxtSifre);
            GrbboxKaynak.Controls.Add(CmbboxKaynaktablo);
            GrbboxKaynak.Controls.Add(label3);
            GrbboxKaynak.Controls.Add(label2);
            GrbboxKaynak.Controls.Add(LblKynkTablo);
            GrbboxKaynak.Controls.Add(TxtKullanıcı);
            GrbboxKaynak.Controls.Add(lblKynkVeri);
            GrbboxKaynak.Controls.Add(label1);
            GrbboxKaynak.Controls.Add(GrdKaynak);
            GrbboxKaynak.Controls.Add(BtnKynkKolonYukle);
            GrbboxKaynak.Controls.Add(LblKynkSunucu);
            GrbboxKaynak.Controls.Add(TxtboxKaynakSunucu);
            GrbboxKaynak.Dock = DockStyle.Left;
            GrbboxKaynak.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 162);
            GrbboxKaynak.Location = new Point(0, 0);
            GrbboxKaynak.Name = "GrbboxKaynak";
            GrbboxKaynak.Size = new Size(267, 686);
            GrbboxKaynak.TabIndex = 16;
            GrbboxKaynak.TabStop = false;
            GrbboxKaynak.Text = "Kaynak";
            // 
            // CkboxSifreGoster
            // 
            CkboxSifreGoster.AutoSize = true;
            CkboxSifreGoster.Location = new Point(130, 132);
            CkboxSifreGoster.Name = "CkboxSifreGoster";
            CkboxSifreGoster.Size = new Size(86, 19);
            CkboxSifreGoster.TabIndex = 1;
            CkboxSifreGoster.Text = "Şifre Göster";
            CkboxSifreGoster.UseVisualStyleBackColor = true;
            CkboxSifreGoster.CheckedChanged += CkboxSifreGoster_CheckedChanged;
            // 
            // CmboxKaynakSutun
            // 
            CmboxKaynakSutun.FormattingEnabled = true;
            CmboxKaynakSutun.Location = new Point(128, 254);
            CmboxKaynakSutun.Name = "CmboxKaynakSutun";
            CmboxKaynakSutun.Size = new Size(127, 23);
            CmboxKaynakSutun.TabIndex = 26;
            // 
            // TxtSifre
            // 
            TxtSifre.Location = new Point(129, 103);
            TxtSifre.Name = "TxtSifre";
            TxtSifre.Size = new Size(128, 23);
            TxtSifre.TabIndex = 23;
            TxtSifre.TextChanged += TxtSifre_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 162);
            label3.Location = new Point(1, 257);
            label3.Name = "label3";
            label3.Size = new Size(80, 20);
            label3.TabIndex = 25;
            label3.Text = "Sütun Adı :";
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
            TxtKullanıcı.Size = new Size(128, 23);
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
            // GrdKaynak
            // 
            GrdKaynak.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            GrdKaynak.Location = new Point(12, 332);
            GrdKaynak.Name = "GrdKaynak";
            GrdKaynak.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GrdKaynak.Size = new Size(232, 310);
            GrdKaynak.TabIndex = 19;
            GrdKaynak.CellClick += GrdKaynak_CellClick;
            // 
            // BtnKynkKolonYukle
            // 
            BtnKynkKolonYukle.Location = new Point(25, 283);
            BtnKynkKolonYukle.Name = "BtnKynkKolonYukle";
            BtnKynkKolonYukle.Size = new Size(216, 29);
            BtnKynkKolonYukle.TabIndex = 10;
            BtnKynkKolonYukle.Text = "Kolonları Yükle";
            BtnKynkKolonYukle.UseVisualStyleBackColor = true;
            BtnKynkKolonYukle.Click += BtnKynkKolonYukle_Click;
            // 
            // GrbboxHedef
            // 
            GrbboxHedef.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            GrbboxHedef.BackColor = SystemColors.GradientInactiveCaption;
            GrbboxHedef.Controls.Add(ChkboxHedefSifre);
            GrbboxHedef.Controls.Add(CmboxHedefSutun);
            GrbboxHedef.Controls.Add(TxboxHedefSifre);
            GrbboxHedef.Controls.Add(LblHdfVeri);
            GrbboxHedef.Controls.Add(label4);
            GrbboxHedef.Controls.Add(TxboxHedefKullanici);
            GrbboxHedef.Controls.Add(LblHedefSutun);
            GrbboxHedef.Controls.Add(LblHedefKullanici);
            GrbboxHedef.Controls.Add(GrdHedef);
            GrbboxHedef.Controls.Add(LblHdfTablo);
            GrbboxHedef.Controls.Add(BtnHedefKolonYukle);
            GrbboxHedef.Controls.Add(CmbboxHedefTablo);
            GrbboxHedef.Controls.Add(TxtboxHedefSunucu);
            GrbboxHedef.Controls.Add(LblHdfSunucu);
            GrbboxHedef.Controls.Add(CmbboxHedefVeriTabani);
            GrbboxHedef.Dock = DockStyle.Right;
            GrbboxHedef.Location = new Point(835, 0);
            GrbboxHedef.Name = "GrbboxHedef";
            GrbboxHedef.Size = new Size(284, 686);
            GrbboxHedef.TabIndex = 17;
            GrbboxHedef.TabStop = false;
            GrbboxHedef.Text = "Hedef";
            // 
            // ChkboxHedefSifre
            // 
            ChkboxHedefSifre.AutoSize = true;
            ChkboxHedefSifre.Location = new Point(124, 135);
            ChkboxHedefSifre.Name = "ChkboxHedefSifre";
            ChkboxHedefSifre.Size = new Size(86, 19);
            ChkboxHedefSifre.TabIndex = 22;
            ChkboxHedefSifre.Text = "Şifre Göster";
            ChkboxHedefSifre.UseVisualStyleBackColor = true;
            ChkboxHedefSifre.CheckedChanged += ChkboxSifre_CheckedChanged;
            // 
            // CmboxHedefSutun
            // 
            CmboxHedefSutun.FormattingEnabled = true;
            CmboxHedefSutun.Location = new Point(121, 254);
            CmboxHedefSutun.Name = "CmboxHedefSutun";
            CmboxHedefSutun.Size = new Size(130, 23);
            CmboxHedefSutun.TabIndex = 21;
            // 
            // TxboxHedefSifre
            // 
            TxboxHedefSifre.Location = new Point(123, 106);
            TxboxHedefSifre.Name = "TxboxHedefSifre";
            TxboxHedefSifre.Size = new Size(130, 23);
            TxboxHedefSifre.TabIndex = 19;
            TxboxHedefSifre.TextChanged += TxboxHedefSifre_TextChanged;
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
            TxboxHedefKullanici.Size = new Size(129, 23);
            TxboxHedefKullanici.TabIndex = 17;
            // 
            // LblHedefSutun
            // 
            LblHedefSutun.AutoSize = true;
            LblHedefSutun.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 162);
            LblHedefSutun.Location = new Point(10, 257);
            LblHedefSutun.Name = "LblHedefSutun";
            LblHedefSutun.Size = new Size(80, 20);
            LblHedefSutun.TabIndex = 20;
            LblHedefSutun.Text = "Sütun Adı :";
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
            // GrdHedef
            // 
            GrdHedef.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            GrdHedef.Location = new Point(21, 332);
            GrdHedef.Name = "GrdHedef";
            GrdHedef.Size = new Size(218, 310);
            GrdHedef.TabIndex = 15;
            GrdHedef.CellClick += GrdHedef_CellClick;
            // 
            // BtnHedefKolonYukle
            // 
            BtnHedefKolonYukle.Location = new Point(30, 283);
            BtnHedefKolonYukle.Name = "BtnHedefKolonYukle";
            BtnHedefKolonYukle.Size = new Size(219, 26);
            BtnHedefKolonYukle.TabIndex = 13;
            BtnHedefKolonYukle.Text = "Kolonları Yükle";
            BtnHedefKolonYukle.UseVisualStyleBackColor = true;
            BtnHedefKolonYukle.Click += BtnHedefKolonYukle_Click;
            // 
            // GrbboxButon
            // 
            GrbboxButon.BackColor = SystemColors.Control;
            GrbboxButon.Controls.Add(LstboxLog);
            GrbboxButon.Controls.Add(BtnBaglantiTest);
            GrbboxButon.Dock = DockStyle.Top;
            GrbboxButon.Location = new Point(267, 0);
            GrbboxButon.Name = "GrbboxButon";
            GrbboxButon.Size = new Size(568, 250);
            GrbboxButon.TabIndex = 20;
            GrbboxButon.TabStop = false;
            // 
            // LstboxLog
            // 
            LstboxLog.FormattingEnabled = true;
            LstboxLog.ItemHeight = 15;
            LstboxLog.Location = new Point(19, 65);
            LstboxLog.Name = "LstboxLog";
            LstboxLog.Size = new Size(533, 154);
            LstboxLog.TabIndex = 0;
            // 
            // GrbboxEslesmeLog
            // 
            GrbboxEslesmeLog.Controls.Add(PrgsbarTransfer);
            GrbboxEslesmeLog.Controls.Add(GrdEslestirme);
            GrbboxEslesmeLog.Controls.Add(BtnTransferBaslat);
            GrbboxEslesmeLog.Controls.Add(BtnEslesmeDogrula);
            GrbboxEslesmeLog.Dock = DockStyle.Bottom;
            GrbboxEslesmeLog.Location = new Point(267, 256);
            GrbboxEslesmeLog.Name = "GrbboxEslesmeLog";
            GrbboxEslesmeLog.Size = new Size(568, 430);
            GrbboxEslesmeLog.TabIndex = 21;
            GrbboxEslesmeLog.TabStop = false;
            GrbboxEslesmeLog.Text = "groupBox4";
            // 
            // PrgsbarTransfer
            // 
            PrgsbarTransfer.Location = new Point(116, 375);
            PrgsbarTransfer.Name = "PrgsbarTransfer";
            PrgsbarTransfer.Size = new Size(135, 23);
            PrgsbarTransfer.TabIndex = 17;
            // 
            // GrdEslestirme
            // 
            GrdEslestirme.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            GrdEslestirme.Columns.AddRange(new DataGridViewColumn[] { KaynakSutun, HedefSutun, Uygunluk, Sil });
            GrdEslestirme.Location = new Point(19, 38);
            GrdEslestirme.Name = "GrdEslestirme";
            GrdEslestirme.Size = new Size(534, 200);
            GrdEslestirme.TabIndex = 16;
            GrdEslestirme.CellClick += GrdEslestirme_CellClick;
            GrdEslestirme.CellValidated += GrdEslestirme_CellValidated;
            GrdEslestirme.CellValidating += GrdEslestirme_CellValidating;
            GrdEslestirme.CellValueChanged += GrdEslestirme_CellValueChanged;
            GrdEslestirme.CurrentCellDirtyStateChanged += GrdEslestirme_CurrentCellDirtyStateChanged;
            // 
            // KaynakSutun
            // 
            KaynakSutun.HeaderText = "Kaynak Sütunlar";
            KaynakSutun.Name = "KaynakSutun";
            KaynakSutun.Resizable = DataGridViewTriState.True;
            KaynakSutun.SortMode = DataGridViewColumnSortMode.Automatic;
            KaynakSutun.Width = 160;
            // 
            // HedefSutun
            // 
            HedefSutun.HeaderText = "Hedef Sütunlar";
            HedefSutun.Name = "HedefSutun";
            HedefSutun.Resizable = DataGridViewTriState.True;
            HedefSutun.Width = 160;
            // 
            // Uygunluk
            // 
            Uygunluk.HeaderText = "Uygunluk";
            Uygunluk.Name = "Uygunluk";
            Uygunluk.Resizable = DataGridViewTriState.True;
            // 
            // Sil
            // 
            Sil.HeaderText = "Sil";
            Sil.Name = "Sil";
            Sil.Resizable = DataGridViewTriState.True;
            Sil.SortMode = DataGridViewColumnSortMode.Automatic;
            Sil.Width = 70;
            // 
            // FrmVeriEslestirme
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoValidate = AutoValidate.EnablePreventFocusChange;
            BackgroundImageLayout = ImageLayout.Center;
            ClientSize = new Size(1119, 686);
            Controls.Add(GrbboxEslesmeLog);
            Controls.Add(GrbboxButon);
            Controls.Add(GrbboxHedef);
            Controls.Add(GrbboxKaynak);
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
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
            ((System.ComponentModel.ISupportInitialize)GrdEslestirme).EndInit();
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
        private Button BtnEslesmeDogrula;
        private Button BtnTransferBaslat;
        private GroupBox GrbboxKaynak;
        private GroupBox GrbboxHedef;
        private Button BtnKynkKolonYukle;
        private Button BtnHedefKolonYukle;
        private DataGridView GrdKaynak;
        private DataGridView GrdHedef;
        private GroupBox GrbboxButon;
        private GroupBox GrbboxEslesmeLog;
        private ListBox LstboxLog;
        private TextBox TxtSifre;
        private Label label2;
        private TextBox TxtKullanıcı;
        private Label label1;
        private ComboBox CmboxKaynakSutun;
        private Label label3;
        private Label LblHedefSutun;
        private TextBox TxboxHedefSifre;
        private Label label4;
        private TextBox TxboxHedefKullanici;
        private Label LblHedefKullanici;
        private ComboBox CmboxHedefSutun;
        private DataGridView GrdEslestirme;
        private DataGridViewButtonColumn KaynakSutun;
        private DataGridViewTextBoxColumn HedefSutun;
        private DataGridViewTextBoxColumn Uygunluk;
        private DataGridViewButtonColumn Sil;
        private CheckBox CkboxSifreGoster;
        private CheckBox ChkboxHedefSifre;
        private ProgressBar PrgsbarTransfer;
        private System.Windows.Forms.Timer timer1;
    }
}
