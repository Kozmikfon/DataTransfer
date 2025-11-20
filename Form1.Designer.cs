namespace DataTransfer
{
    partial class FrmVeriEslestirme
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

        private void InitializeComponent()
        {
            TrwHedefTablolar = new TreeView();
            TrwKaynakTablolar = new TreeView();
            BtnSutunYkle = new Button();
            BtnHedefSutunYkle = new Button();
            BtnOtomatikEsle = new Button();
            BtnStrSil = new Button();
            BtnStrEkle = new Button();
            GrdEslestirme = new DataGridView();
            BtnTransferBaslat = new Button();
            prgTransfer = new ProgressBar();
            lstLog = new ListBox();
            BtnGeri = new Button();
            BtnFiltreTest = new Button();
            TxtFiltreleme = new TextBox();
            RdoBtnFiltre = new RadioButton();
            RdoBtnTumSatır = new RadioButton();
            GrbBoxFiltreleme = new GroupBox();
            lblTransferSayisi = new Label();
            label2 = new Label();
            lblKaynak = new Label();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)GrdEslestirme).BeginInit();
            GrbBoxFiltreleme.SuspendLayout();
            SuspendLayout();
            // 
            // TrwHedefTablolar
            // 
            TrwHedefTablolar.Location = new Point(597, 24);
            TrwHedefTablolar.Name = "TrwHedefTablolar";
            TrwHedefTablolar.Size = new Size(589, 256);
            TrwHedefTablolar.TabIndex = 0;
            TrwHedefTablolar.AfterSelect += TrwHedefTablolar_AfterSelect;
            // 
            // TrwKaynakTablolar
            // 
            TrwKaynakTablolar.ItemHeight = 18;
            TrwKaynakTablolar.Location = new Point(0, 24);
            TrwKaynakTablolar.Name = "TrwKaynakTablolar";
            TrwKaynakTablolar.Size = new Size(591, 256);
            TrwKaynakTablolar.TabIndex = 0;
            TrwKaynakTablolar.AfterSelect += TrwKaynakTablolar_AfterSelect;
            // 
            // BtnSutunYkle
            // 
            BtnSutunYkle.Location = new Point(466, 286);
            BtnSutunYkle.Name = "BtnSutunYkle";
            BtnSutunYkle.Size = new Size(125, 29);
            BtnSutunYkle.TabIndex = 12;
            BtnSutunYkle.Text = "Kaynak Sütun Yükle";
            BtnSutunYkle.UseVisualStyleBackColor = true;
            BtnSutunYkle.Click += BtnSutunYkle_Click;
            // 
            // BtnHedefSutunYkle
            // 
            BtnHedefSutunYkle.Location = new Point(1041, 286);
            BtnHedefSutunYkle.Name = "BtnHedefSutunYkle";
            BtnHedefSutunYkle.Size = new Size(137, 29);
            BtnHedefSutunYkle.TabIndex = 12;
            BtnHedefSutunYkle.Text = "Hedef Sutun Yükle";
            BtnHedefSutunYkle.UseVisualStyleBackColor = true;
            BtnHedefSutunYkle.Click += BtnHedefSutunYkle_Click;
            // 
            // BtnOtomatikEsle
            // 
            BtnOtomatikEsle.Location = new Point(1025, 342);
            BtnOtomatikEsle.Name = "BtnOtomatikEsle";
            BtnOtomatikEsle.Size = new Size(156, 65);
            BtnOtomatikEsle.TabIndex = 3;
            BtnOtomatikEsle.Text = "Otomatik eşleştir";
            BtnOtomatikEsle.UseVisualStyleBackColor = true;
            BtnOtomatikEsle.Click += BtnOtomatikEsle_Click;
            // 
            // BtnStrSil
            // 
            BtnStrSil.Location = new Point(1025, 487);
            BtnStrSil.Name = "BtnStrSil";
            BtnStrSil.Size = new Size(153, 66);
            BtnStrSil.TabIndex = 2;
            BtnStrSil.Text = "Satır Sil";
            BtnStrSil.UseVisualStyleBackColor = true;
            BtnStrSil.Click += BtnStrSil_Click;
            // 
            // BtnStrEkle
            // 
            BtnStrEkle.Location = new Point(1025, 413);
            BtnStrEkle.Name = "BtnStrEkle";
            BtnStrEkle.Size = new Size(156, 68);
            BtnStrEkle.TabIndex = 1;
            BtnStrEkle.Text = "Satır ekle";
            BtnStrEkle.UseVisualStyleBackColor = true;
            BtnStrEkle.Click += BtnStrEkle_Click;
            // 
            // GrdEslestirme
            // 
            GrdEslestirme.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            GrdEslestirme.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            GrdEslestirme.Location = new Point(0, 321);
            GrdEslestirme.Name = "GrdEslestirme";
            GrdEslestirme.Size = new Size(1019, 232);
            GrdEslestirme.TabIndex = 1;
            GrdEslestirme.CellContentClick += GrdEslestirme_CellContentClick;
            GrdEslestirme.CellDoubleClick += GrdEslestirme_CellDoubleClick;
            GrdEslestirme.CellValueChanged += GrdEslestirme_CellValueChanged;
            GrdEslestirme.EditingControlShowing += GrdEslestirme_EditingControlShowing;
            // 
            // BtnTransferBaslat
            // 
            BtnTransferBaslat.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnTransferBaslat.Location = new Point(1025, 582);
            BtnTransferBaslat.Name = "BtnTransferBaslat";
            BtnTransferBaslat.Size = new Size(156, 73);
            BtnTransferBaslat.TabIndex = 7;
            BtnTransferBaslat.Text = "Transferi Başlat";
            BtnTransferBaslat.UseVisualStyleBackColor = true;
            BtnTransferBaslat.Click += BtnTransferBaslat_Click;
            // 
            // prgTransfer
            // 
            prgTransfer.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            prgTransfer.Location = new Point(624, 582);
            prgTransfer.Name = "prgTransfer";
            prgTransfer.Size = new Size(200, 73);
            prgTransfer.TabIndex = 8;
            // 
            // lstLog
            // 
            lstLog.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lstLog.FormattingEnabled = true;
            lstLog.ItemHeight = 15;
            lstLog.Location = new Point(12, 670);
            lstLog.Name = "lstLog";
            lstLog.Size = new Size(1110, 124);
            lstLog.TabIndex = 9;
            // 
            // BtnGeri
            // 
            BtnGeri.Location = new Point(12, 830);
            BtnGeri.Name = "BtnGeri";
            BtnGeri.Size = new Size(75, 23);
            BtnGeri.TabIndex = 11;
            BtnGeri.Text = "Geri";
            BtnGeri.UseVisualStyleBackColor = true;
            BtnGeri.Click += BtnGeri_Click;
            // 
            // BtnFiltreTest
            // 
            BtnFiltreTest.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnFiltreTest.Location = new Point(474, 45);
            BtnFiltreTest.Name = "BtnFiltreTest";
            BtnFiltreTest.Size = new Size(99, 37);
            BtnFiltreTest.TabIndex = 3;
            BtnFiltreTest.Text = "test et";
            BtnFiltreTest.UseVisualStyleBackColor = true;
            BtnFiltreTest.Click += BtnFiltreTest_Click;
            // 
            // TxtFiltreleme
            // 
            TxtFiltreleme.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            TxtFiltreleme.Location = new Point(16, 58);
            TxtFiltreleme.Name = "TxtFiltreleme";
            TxtFiltreleme.Size = new Size(428, 23);
            TxtFiltreleme.TabIndex = 2;
            // 
            // RdoBtnFiltre
            // 
            RdoBtnFiltre.AutoSize = true;
            RdoBtnFiltre.Location = new Point(126, 33);
            RdoBtnFiltre.Name = "RdoBtnFiltre";
            RdoBtnFiltre.Size = new Size(91, 19);
            RdoBtnFiltre.TabIndex = 1;
            RdoBtnFiltre.TabStop = true;
            RdoBtnFiltre.Text = "Filtre Uygula";
            RdoBtnFiltre.UseVisualStyleBackColor = true;
            // 
            // RdoBtnTumSatır
            // 
            RdoBtnTumSatır.AutoSize = true;
            RdoBtnTumSatır.Location = new Point(16, 33);
            RdoBtnTumSatır.Name = "RdoBtnTumSatır";
            RdoBtnTumSatır.Size = new Size(89, 19);
            RdoBtnTumSatır.TabIndex = 0;
            RdoBtnTumSatır.TabStop = true;
            RdoBtnTumSatır.Text = "Tüm Satırlar";
            RdoBtnTumSatır.UseVisualStyleBackColor = true;
            // 
            // GrbBoxFiltreleme
            // 
            GrbBoxFiltreleme.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            GrbBoxFiltreleme.Controls.Add(RdoBtnTumSatır);
            GrbBoxFiltreleme.Controls.Add(RdoBtnFiltre);
            GrbBoxFiltreleme.Controls.Add(TxtFiltreleme);
            GrbBoxFiltreleme.Controls.Add(BtnFiltreTest);
            GrbBoxFiltreleme.Location = new Point(12, 559);
            GrbBoxFiltreleme.Name = "GrbBoxFiltreleme";
            GrbBoxFiltreleme.Size = new Size(579, 96);
            GrbBoxFiltreleme.TabIndex = 2;
            GrbBoxFiltreleme.TabStop = false;
            GrbBoxFiltreleme.Text = "Filtreleme";
            // 
            // lblTransferSayisi
            // 
            lblTransferSayisi.AutoSize = true;
            lblTransferSayisi.Font = new Font("Segoe UI", 12F);
            lblTransferSayisi.Location = new Point(852, 815);
            lblTransferSayisi.Name = "lblTransferSayisi";
            lblTransferSayisi.Size = new Size(19, 21);
            lblTransferSayisi.TabIndex = 12;
            lblTransferSayisi.Text = "0";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F);
            label2.Location = new Point(983, 815);
            label2.Name = "label2";
            label2.Size = new Size(139, 21);
            label2.TabIndex = 13;
            label2.Text = "veri transfer edildi.";
            // 
            // lblKaynak
            // 
            lblKaynak.AutoSize = true;
            lblKaynak.Location = new Point(0, 6);
            lblKaynak.Name = "lblKaynak";
            lblKaynak.Size = new Size(90, 15);
            lblKaynak.TabIndex = 14;
            lblKaynak.Text = "Kaynak Tablolar";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(597, 6);
            label1.Name = "label1";
            label1.Size = new Size(84, 15);
            label1.TabIndex = 15;
            label1.Text = "Hedef Tablolar";
            // 
            // FrmVeriEslestirme
            // 
            AutoScaleMode = AutoScaleMode.Inherit;
            BackgroundImageLayout = ImageLayout.Center;
            ClientSize = new Size(1184, 861);
            Controls.Add(label1);
            Controls.Add(lblKaynak);
            Controls.Add(TrwKaynakTablolar);
            Controls.Add(BtnHedefSutunYkle);
            Controls.Add(BtnSutunYkle);
            Controls.Add(TrwHedefTablolar);
            Controls.Add(label2);
            Controls.Add(lblTransferSayisi);
            Controls.Add(BtnGeri);
            Controls.Add(BtnStrSil);
            Controls.Add(BtnOtomatikEsle);
            Controls.Add(BtnStrEkle);
            Controls.Add(lstLog);
            Controls.Add(prgTransfer);
            Controls.Add(BtnTransferBaslat);
            Controls.Add(GrbBoxFiltreleme);
            Controls.Add(GrdEslestirme);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmVeriEslestirme";
            Text = "Veri Transferi";
            Load += FrmVeriEslestirme_Load;
            ((System.ComponentModel.ISupportInitialize)GrdEslestirme).EndInit();
            GrbBoxFiltreleme.ResumeLayout(false);
            GrbBoxFiltreleme.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }
        private TreeView TrwHedefTablolar;
        private TreeView TrwKaynakTablolar;
        private Button BtnSutunYkle;
       // private SplitContainer splitContainerMain;
        private DataGridView GrdEslestirme;
        private Button BtnTransferBaslat;
        private ProgressBar prgTransfer;
        private ListBox lstLog;
        private Button BtnGeri;
        private Button BtnOtomatikEsle;
        private Button BtnStrSil;
        private Button BtnStrEkle;
        private Button BtnFiltreTest;
        private TextBox TxtFiltreleme;
        private RadioButton RdoBtnFiltre;
        private RadioButton RdoBtnTumSatır;
        private GroupBox GrbBoxFiltreleme;
        private Button BtnHedefSutunYkle;
        private Label lblTransferSayisi;
        private Label label2;
        private Label lblKaynak;
        private Label label1;
    }

}
