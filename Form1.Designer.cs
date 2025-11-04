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
            splitContainerMain = new SplitContainer();
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
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).BeginInit();
            splitContainerMain.Panel1.SuspendLayout();
            splitContainerMain.Panel2.SuspendLayout();
            splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GrdEslestirme).BeginInit();
            GrbBoxFiltreleme.SuspendLayout();
            SuspendLayout();
            // 
            // TrwHedefTablolar
            // 
            TrwHedefTablolar.Dock = DockStyle.Fill;
            TrwHedefTablolar.Location = new Point(0, 0);
            TrwHedefTablolar.Name = "TrwHedefTablolar";
            TrwHedefTablolar.Size = new Size(518, 200);
            TrwHedefTablolar.TabIndex = 0;
            TrwHedefTablolar.AfterSelect += TrwHedefTablolar_AfterSelect;
            // 
            // TrwKaynakTablolar
            // 
            TrwKaynakTablolar.Dock = DockStyle.Fill;
            TrwKaynakTablolar.Location = new Point(0, 0);
            TrwKaynakTablolar.Name = "TrwKaynakTablolar";
            TrwKaynakTablolar.Size = new Size(478, 200);
            TrwKaynakTablolar.TabIndex = 0;
            TrwKaynakTablolar.AfterSelect += TrwKaynakTablolar_AfterSelect;
            // 
            // BtnSutunYkle
            // 
            BtnSutunYkle.Location = new Point(373, 166);
            BtnSutunYkle.Name = "BtnSutunYkle";
            BtnSutunYkle.Size = new Size(102, 29);
            BtnSutunYkle.TabIndex = 12;
            BtnSutunYkle.Text = "Sütun Yükle";
            BtnSutunYkle.UseVisualStyleBackColor = true;
            BtnSutunYkle.Click += BtnSutunYkle_Click;
            // 
            // splitContainerMain
            // 
            splitContainerMain.Dock = DockStyle.Top;
            splitContainerMain.Location = new Point(0, 0);
            splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            splitContainerMain.Panel1.Controls.Add(BtnSutunYkle);
            splitContainerMain.Panel1.Controls.Add(TrwKaynakTablolar);
            // 
            // splitContainerMain.Panel2
            // 
            splitContainerMain.Panel2.Controls.Add(BtnOtomatikEsle);
            splitContainerMain.Panel2.Controls.Add(BtnStrSil);
            splitContainerMain.Panel2.Controls.Add(BtnStrEkle);
            splitContainerMain.Panel2.Controls.Add(TrwHedefTablolar);
            splitContainerMain.Size = new Size(1000, 200);
            splitContainerMain.SplitterDistance = 478;
            splitContainerMain.TabIndex = 0;
            // 
            // BtnOtomatikEsle
            // 
            BtnOtomatikEsle.Location = new Point(323, 125);
            BtnOtomatikEsle.Name = "BtnOtomatikEsle";
            BtnOtomatikEsle.Size = new Size(112, 72);
            BtnOtomatikEsle.TabIndex = 3;
            BtnOtomatikEsle.Text = "Otomatik eşleştir";
            BtnOtomatikEsle.UseVisualStyleBackColor = true;
            BtnOtomatikEsle.Click += BtnOtomatikEsle_Click;
            // 
            // BtnStrSil
            // 
            BtnStrSil.Location = new Point(445, 164);
            BtnStrSil.Name = "BtnStrSil";
            BtnStrSil.Size = new Size(70, 33);
            BtnStrSil.TabIndex = 2;
            BtnStrSil.Text = "Satır Sil";
            BtnStrSil.UseVisualStyleBackColor = true;
            BtnStrSil.Click += BtnStrSil_Click;
            // 
            // BtnStrEkle
            // 
            BtnStrEkle.Location = new Point(441, 125);
            BtnStrEkle.Name = "BtnStrEkle";
            BtnStrEkle.Size = new Size(74, 33);
            BtnStrEkle.TabIndex = 1;
            BtnStrEkle.Text = "Satır ekle";
            BtnStrEkle.UseVisualStyleBackColor = true;
            BtnStrEkle.Click += BtnStrEkle_Click;
            // 
            // GrdEslestirme
            // 
            GrdEslestirme.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            GrdEslestirme.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            GrdEslestirme.Location = new Point(12, 215);
            GrdEslestirme.Name = "GrdEslestirme";
            GrdEslestirme.Size = new Size(976, 251);
            GrdEslestirme.TabIndex = 1;
            GrdEslestirme.CellContentClick += GrdEslestirme_CellContentClick;
            GrdEslestirme.CellValueChanged += GrdEslestirme_CellValueChanged;
            // 
            // BtnTransferBaslat
            // 
            BtnTransferBaslat.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnTransferBaslat.Location = new Point(832, 479);
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
            prgTransfer.Location = new Point(618, 479);
            prgTransfer.Name = "prgTransfer";
            prgTransfer.Size = new Size(200, 73);
            prgTransfer.TabIndex = 8;
            // 
            // lstLog
            // 
            lstLog.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lstLog.FormattingEnabled = true;
            lstLog.ItemHeight = 15;
            lstLog.Location = new Point(12, 560);
            lstLog.Name = "lstLog";
            lstLog.Size = new Size(976, 94);
            lstLog.TabIndex = 9;
            // 
            // BtnGeri
            // 
            BtnGeri.Location = new Point(12, 675);
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
            BtnFiltreTest.Location = new Point(506, 47);
            BtnFiltreTest.Name = "BtnFiltreTest";
            BtnFiltreTest.Size = new Size(88, 27);
            BtnFiltreTest.TabIndex = 3;
            BtnFiltreTest.Text = "test et";
            BtnFiltreTest.UseVisualStyleBackColor = true;
            BtnFiltreTest.Click += BtnFiltreTest_Click;
            // 
            // TxtFiltreleme
            // 
            TxtFiltreleme.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            TxtFiltreleme.Location = new Point(16, 50);
            TxtFiltreleme.Name = "TxtFiltreleme";
            TxtFiltreleme.Size = new Size(470, 23);
            TxtFiltreleme.TabIndex = 2;
            // 
            // RdoBtnFiltre
            // 
            RdoBtnFiltre.AutoSize = true;
            RdoBtnFiltre.Location = new Point(111, 28);
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
            RdoBtnTumSatır.Location = new Point(16, 28);
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
            GrbBoxFiltreleme.Location = new Point(12, 472);
            GrbBoxFiltreleme.Name = "GrbBoxFiltreleme";
            GrbBoxFiltreleme.Size = new Size(600, 80);
            GrbBoxFiltreleme.TabIndex = 2;
            GrbBoxFiltreleme.TabStop = false;
            GrbBoxFiltreleme.Text = "Filtreleme";
            // 
            // FrmVeriEslestirme
            // 
            AutoScaleMode = AutoScaleMode.Inherit;
            BackgroundImageLayout = ImageLayout.Center;
            ClientSize = new Size(1000, 705);
            Controls.Add(BtnGeri);
            Controls.Add(lstLog);
            Controls.Add(prgTransfer);
            Controls.Add(BtnTransferBaslat);
            Controls.Add(GrbBoxFiltreleme);
            Controls.Add(GrdEslestirme);
            Controls.Add(splitContainerMain);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "FrmVeriEslestirme";
            Text = "Veri Transferi";
            Load += FrmVeriEslestirme_Load;
            splitContainerMain.Panel1.ResumeLayout(false);
            splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).EndInit();
            splitContainerMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)GrdEslestirme).EndInit();
            GrbBoxFiltreleme.ResumeLayout(false);
            GrbBoxFiltreleme.PerformLayout();
            ResumeLayout(false);
        }
        private TreeView TrwHedefTablolar;
        private TreeView TrwKaynakTablolar;
        private Button BtnSutunYkle;
        private SplitContainer splitContainerMain;
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
    }

}
