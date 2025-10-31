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
            lblKynkVeri = new Label();
            LblKynkTablo = new Label();
            LblHdfVeri = new Label();
            LblHdfTablo = new Label();
            CmbboxKaynakVeritabani = new ComboBox();
            CmbboxKaynaktablo = new ComboBox();
            CmbboxHedefVeriTabani = new ComboBox();
            CmbboxHedefTablo = new ComboBox();
            BtnEslesmeDogrula = new Button();
            BtnTransferBaslat = new Button();
            GrbboxKaynak = new GroupBox();
            CmboxKaynakSutun = new ComboBox();
            label3 = new Label();
            GrdKaynak = new DataGridView();
            BtnKynkKolonYukle = new Button();
            GrbboxHedef = new GroupBox();
            CmboxHedefSutun = new ComboBox();
            LblHedefSutun = new Label();
            GrdHedef = new DataGridView();
            BtnHedefKolonYukle = new Button();
            GrbboxButon = new GroupBox();
            LstboxLog = new ListBox();
            GrbboxEslesmeLog = new GroupBox();
            BtnGeriBaglanti = new Button();
            BtnGrdTemizle = new Button();
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
            // lblKynkVeri
            // 
            lblKynkVeri.AutoSize = true;
            lblKynkVeri.Font = new Font("Segoe UI", 11.25F);
            lblKynkVeri.Location = new Point(6, 22);
            lblKynkVeri.Name = "lblKynkVeri";
            lblKynkVeri.Size = new Size(87, 20);
            lblKynkVeri.TabIndex = 2;
            lblKynkVeri.Text = "Veri tabanı :";
            // 
            // LblKynkTablo
            // 
            LblKynkTablo.AutoSize = true;
            LblKynkTablo.Font = new Font("Segoe UI", 11.25F);
            LblKynkTablo.Location = new Point(7, 68);
            LblKynkTablo.Name = "LblKynkTablo";
            LblKynkTablo.Size = new Size(79, 20);
            LblKynkTablo.TabIndex = 3;
            LblKynkTablo.Text = "Tablo Adı :";
            // 
            // LblHdfVeri
            // 
            LblHdfVeri.AutoSize = true;
            LblHdfVeri.Font = new Font("Segoe UI", 11.25F);
            LblHdfVeri.Location = new Point(14, 19);
            LblHdfVeri.Name = "LblHdfVeri";
            LblHdfVeri.Size = new Size(88, 20);
            LblHdfVeri.TabIndex = 5;
            LblHdfVeri.Text = "Veri Tabanı :";
            // 
            // LblHdfTablo
            // 
            LblHdfTablo.AutoSize = true;
            LblHdfTablo.Font = new Font("Segoe UI", 11.25F);
            LblHdfTablo.Location = new Point(13, 62);
            LblHdfTablo.Name = "LblHdfTablo";
            LblHdfTablo.Size = new Size(79, 20);
            LblHdfTablo.TabIndex = 6;
            LblHdfTablo.Text = "Tablo Adı :";
            // 
            // CmbboxKaynakVeritabani
            // 
            CmbboxKaynakVeritabani.FormattingEnabled = true;
            CmbboxKaynakVeritabani.Location = new Point(134, 19);
            CmbboxKaynakVeritabani.Name = "CmbboxKaynakVeritabani";
            CmbboxKaynakVeritabani.Size = new Size(127, 23);
            CmbboxKaynakVeritabani.TabIndex = 8;
            CmbboxKaynakVeritabani.DrawItem += CmbboxKaynakVeritabani_DrawItem;
            CmbboxKaynakVeritabani.SelectedIndexChanged += CmbboxKaynakVeritabani_SelectedIndexChanged;
            // 
            // CmbboxKaynaktablo
            // 
            CmbboxKaynaktablo.FormattingEnabled = true;
            CmbboxKaynaktablo.Location = new Point(134, 65);
            CmbboxKaynaktablo.Name = "CmbboxKaynaktablo";
            CmbboxKaynaktablo.Size = new Size(127, 23);
            CmbboxKaynaktablo.TabIndex = 9;
            CmbboxKaynaktablo.DrawItem += CmbboxKaynaktablo_DrawItem;
            CmbboxKaynaktablo.SelectedIndexChanged += CmbboxKaynaktablo_SelectedIndexChanged;
            // 
            // CmbboxHedefVeriTabani
            // 
            CmbboxHedefVeriTabani.FormattingEnabled = true;
            CmbboxHedefVeriTabani.Location = new Point(127, 16);
            CmbboxHedefVeriTabani.Name = "CmbboxHedefVeriTabani";
            CmbboxHedefVeriTabani.Size = new Size(129, 23);
            CmbboxHedefVeriTabani.TabIndex = 11;
            CmbboxHedefVeriTabani.DrawItem += CmbboxHedefVeriTabani_DrawItem;
            CmbboxHedefVeriTabani.SelectedIndexChanged += CmbboxHedefVeriTabani_SelectedIndexChanged;
            // 
            // CmbboxHedefTablo
            // 
            CmbboxHedefTablo.FormattingEnabled = true;
            CmbboxHedefTablo.Location = new Point(126, 59);
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
            GrbboxKaynak.Controls.Add(CmbboxKaynakVeritabani);
            GrbboxKaynak.Controls.Add(CmboxKaynakSutun);
            GrbboxKaynak.Controls.Add(CmbboxKaynaktablo);
            GrbboxKaynak.Controls.Add(label3);
            GrbboxKaynak.Controls.Add(LblKynkTablo);
            GrbboxKaynak.Controls.Add(lblKynkVeri);
            GrbboxKaynak.Controls.Add(GrdKaynak);
            GrbboxKaynak.Controls.Add(BtnKynkKolonYukle);
            GrbboxKaynak.Dock = DockStyle.Left;
            GrbboxKaynak.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 162);
            GrbboxKaynak.Location = new Point(0, 0);
            GrbboxKaynak.Name = "GrbboxKaynak";
            GrbboxKaynak.Size = new Size(267, 686);
            GrbboxKaynak.TabIndex = 16;
            GrbboxKaynak.TabStop = false;
            GrbboxKaynak.Text = "Kaynak";
            GrbboxKaynak.Enter += GrbboxKaynak_Enter;
            // 
            // CmboxKaynakSutun
            // 
            CmboxKaynakSutun.FormattingEnabled = true;
            CmboxKaynakSutun.Location = new Point(134, 108);
            CmboxKaynakSutun.Name = "CmboxKaynakSutun";
            CmboxKaynakSutun.Size = new Size(127, 23);
            CmboxKaynakSutun.TabIndex = 26;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 162);
            label3.Location = new Point(7, 111);
            label3.Name = "label3";
            label3.Size = new Size(80, 20);
            label3.TabIndex = 25;
            label3.Text = "Sütun Adı :";
            // 
            // GrdKaynak
            // 
            GrdKaynak.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            GrdKaynak.Location = new Point(18, 199);
            GrdKaynak.Name = "GrdKaynak";
            GrdKaynak.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GrdKaynak.Size = new Size(232, 310);
            GrdKaynak.TabIndex = 19;
            GrdKaynak.CellClick += GrdKaynak_CellClick;
            // 
            // BtnKynkKolonYukle
            // 
            BtnKynkKolonYukle.Location = new Point(34, 151);
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
            GrbboxHedef.Controls.Add(CmboxHedefSutun);
            GrbboxHedef.Controls.Add(LblHdfVeri);
            GrbboxHedef.Controls.Add(LblHedefSutun);
            GrbboxHedef.Controls.Add(GrdHedef);
            GrbboxHedef.Controls.Add(LblHdfTablo);
            GrbboxHedef.Controls.Add(BtnHedefKolonYukle);
            GrbboxHedef.Controls.Add(CmbboxHedefTablo);
            GrbboxHedef.Controls.Add(CmbboxHedefVeriTabani);
            GrbboxHedef.Dock = DockStyle.Right;
            GrbboxHedef.Location = new Point(835, 0);
            GrbboxHedef.Name = "GrbboxHedef";
            GrbboxHedef.Size = new Size(284, 686);
            GrbboxHedef.TabIndex = 17;
            GrbboxHedef.TabStop = false;
            GrbboxHedef.Text = "Hedef";
            // 
            // CmboxHedefSutun
            // 
            CmboxHedefSutun.FormattingEnabled = true;
            CmboxHedefSutun.Location = new Point(125, 103);
            CmboxHedefSutun.Name = "CmboxHedefSutun";
            CmboxHedefSutun.Size = new Size(130, 23);
            CmboxHedefSutun.TabIndex = 21;
            // 
            // LblHedefSutun
            // 
            LblHedefSutun.AutoSize = true;
            LblHedefSutun.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 162);
            LblHedefSutun.Location = new Point(14, 106);
            LblHedefSutun.Name = "LblHedefSutun";
            LblHedefSutun.Size = new Size(80, 20);
            LblHedefSutun.TabIndex = 20;
            LblHedefSutun.Text = "Sütun Adı :";
            // 
            // GrdHedef
            // 
            GrdHedef.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            GrdHedef.Location = new Point(38, 168);
            GrdHedef.Name = "GrdHedef";
            GrdHedef.Size = new Size(218, 310);
            GrdHedef.TabIndex = 15;
            GrdHedef.CellClick += GrdHedef_CellClick;
            // 
            // BtnHedefKolonYukle
            // 
            BtnHedefKolonYukle.Location = new Point(34, 132);
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
            GrbboxButon.Dock = DockStyle.Top;
            GrbboxButon.Location = new Point(267, 0);
            GrbboxButon.Name = "GrbboxButon";
            GrbboxButon.Size = new Size(568, 250);
            GrbboxButon.TabIndex = 20;
            GrbboxButon.TabStop = false;
            GrbboxButon.Enter += GrbboxButon_Enter;
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
            GrbboxEslesmeLog.Controls.Add(BtnGeriBaglanti);
            GrbboxEslesmeLog.Controls.Add(BtnGrdTemizle);
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
            // BtnGeriBaglanti
            // 
            BtnGeriBaglanti.Location = new Point(19, 375);
            BtnGeriBaglanti.Name = "BtnGeriBaglanti";
            BtnGeriBaglanti.Size = new Size(75, 23);
            BtnGeriBaglanti.TabIndex = 18;
            BtnGeriBaglanti.Text = "Geri";
            BtnGeriBaglanti.UseVisualStyleBackColor = true;
            BtnGeriBaglanti.Click += BtnGeriBaglanti_Click;
            // 
            // BtnGrdTemizle
            // 
            BtnGrdTemizle.Location = new Point(444, 244);
            BtnGrdTemizle.Name = "BtnGrdTemizle";
            BtnGrdTemizle.Size = new Size(108, 23);
            BtnGrdTemizle.TabIndex = 1;
            BtnGrdTemizle.Text = "Temizle";
            BtnGrdTemizle.UseVisualStyleBackColor = true;
            BtnGrdTemizle.Click += BtnGrdTemizle_Click;
            // 
            // PrgsbarTransfer
            // 
            PrgsbarTransfer.Location = new Point(319, 363);
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
            GrdEslestirme.CellValueChanged += GrdEslestirme_CellValueChanged;
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
            Load += FrmVeriEslestirme_Load;
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
        private Label lblKynkVeri;
        private Label LblKynkTablo;
        private Label LblHdfVeri;
        private Label LblHdfTablo;
        private ComboBox CmbboxKaynakVeritabani;
        private ComboBox CmbboxKaynaktablo;
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
        private ComboBox CmboxKaynakSutun;
        private Label label3;
        private Label LblHedefSutun;
        private ComboBox CmboxHedefSutun;
        private DataGridView GrdEslestirme;
        private DataGridViewButtonColumn KaynakSutun;
        private DataGridViewTextBoxColumn HedefSutun;
        private DataGridViewTextBoxColumn Uygunluk;
        private DataGridViewButtonColumn Sil;
        private ProgressBar PrgsbarTransfer;
        private System.Windows.Forms.Timer timer1;
        private Button BtnGrdTemizle;
        private Button BtnGeriBaglanti;
    }
}
