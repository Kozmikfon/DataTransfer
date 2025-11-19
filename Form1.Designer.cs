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
            TrwHedefTablolar.Size = new Size(589, 256);
            TrwHedefTablolar.TabIndex = 0;
            TrwHedefTablolar.AfterSelect += TrwHedefTablolar_AfterSelect;
            // 
            // TrwKaynakTablolar
            // 
            TrwKaynakTablolar.Dock = DockStyle.Fill;
            TrwKaynakTablolar.ItemHeight = 18;
            TrwKaynakTablolar.Location = new Point(0, 0);
            TrwKaynakTablolar.Name = "TrwKaynakTablolar";
            TrwKaynakTablolar.Size = new Size(591, 256);
            TrwKaynakTablolar.TabIndex = 0;
            TrwKaynakTablolar.AfterSelect += TrwKaynakTablolar_AfterSelect;
            // 
            // BtnSutunYkle
            // 
            BtnSutunYkle.Location = new Point(486, 224);
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
            splitContainerMain.Panel2.Controls.Add(BtnHedefSutunYkle);
            splitContainerMain.Panel2.Controls.Add(TrwHedefTablolar);
            splitContainerMain.Size = new Size(1184, 256);
            splitContainerMain.SplitterDistance = 591;
            splitContainerMain.TabIndex = 0;
            // 
            // BtnHedefSutunYkle
            // 
            BtnHedefSutunYkle.Location = new Point(483, 224);
            BtnHedefSutunYkle.Name = "BtnHedefSutunYkle";
            BtnHedefSutunYkle.Size = new Size(103, 29);
            BtnHedefSutunYkle.TabIndex = 12;
            BtnHedefSutunYkle.Text = "Sutun Yükle";
            BtnHedefSutunYkle.UseVisualStyleBackColor = true;
            BtnHedefSutunYkle.Click += BtnHedefSutunYkle_Click;
            // 
            // BtnOtomatikEsle
            // 
            BtnOtomatikEsle.Location = new Point(985, 284);
            BtnOtomatikEsle.Name = "BtnOtomatikEsle";
            BtnOtomatikEsle.Size = new Size(156, 48);
            BtnOtomatikEsle.TabIndex = 3;
            BtnOtomatikEsle.Text = "Otomatik eşleştir";
            BtnOtomatikEsle.UseVisualStyleBackColor = true;
            BtnOtomatikEsle.Click += BtnOtomatikEsle_Click;
            // 
            // BtnStrSil
            // 
            BtnStrSil.Location = new Point(985, 425);
            BtnStrSil.Name = "BtnStrSil";
            BtnStrSil.Size = new Size(156, 40);
            BtnStrSil.TabIndex = 2;
            BtnStrSil.Text = "Satır Sil";
            BtnStrSil.UseVisualStyleBackColor = true;
            BtnStrSil.Click += BtnStrSil_Click;
            // 
            // BtnStrEkle
            // 
            BtnStrEkle.Location = new Point(985, 360);
            BtnStrEkle.Name = "BtnStrEkle";
            BtnStrEkle.Size = new Size(156, 38);
            BtnStrEkle.TabIndex = 1;
            BtnStrEkle.Text = "Satır ekle";
            BtnStrEkle.UseVisualStyleBackColor = true;
            BtnStrEkle.Click += BtnStrEkle_Click;
            // 
            // GrdEslestirme
            // 
            GrdEslestirme.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            GrdEslestirme.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            GrdEslestirme.Location = new Point(12, 272);
            GrdEslestirme.Name = "GrdEslestirme";
            GrdEslestirme.Size = new Size(967, 272);
            GrdEslestirme.TabIndex = 1;
            GrdEslestirme.CellContentClick += GrdEslestirme_CellContentClick;
            GrdEslestirme.CellValueChanged += GrdEslestirme_CellValueChanged;
            GrdEslestirme.EditingControlShowing += GrdEslestirme_EditingControlShowing;
            // 
            // BtnTransferBaslat
            // 
            BtnTransferBaslat.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnTransferBaslat.Location = new Point(985, 582);
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
            // FrmVeriEslestirme
            // 
            AutoScaleMode = AutoScaleMode.Inherit;
            BackgroundImageLayout = ImageLayout.Center;
            ClientSize = new Size(1184, 861);
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
            Controls.Add(splitContainerMain);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
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
            PerformLayout();
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
        private Button BtnHedefSutunYkle;
        private Label lblTransferSayisi;
        private Label label2;
    }

}
