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
            BtnOtomatikEsle = new Button();
            BtnStrSil = new Button();
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
            BtnKynkSutunYkle = new Button();
            BtnHdfSutunYkle = new Button();
            groupBox1 = new GroupBox();
            groupBox2 = new GroupBox();
            GrdHedefNullable = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)GrdEslestirme).BeginInit();
            GrbBoxFiltreleme.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GrdHedefNullable).BeginInit();
            SuspendLayout();
            // 
            // TrwHedefTablolar
            // 
            TrwHedefTablolar.Dock = DockStyle.Right;
            TrwHedefTablolar.Location = new Point(0, 19);
            TrwHedefTablolar.Name = "TrwHedefTablolar";
            TrwHedefTablolar.Size = new Size(623, 303);
            TrwHedefTablolar.TabIndex = 0;
            TrwHedefTablolar.AfterSelect += TrwHedefTablolar_AfterSelect;
            // 
            // TrwKaynakTablolar
            // 
            TrwKaynakTablolar.Dock = DockStyle.Left;
            TrwKaynakTablolar.ItemHeight = 18;
            TrwKaynakTablolar.Location = new Point(3, 19);
            TrwKaynakTablolar.Name = "TrwKaynakTablolar";
            TrwKaynakTablolar.Size = new Size(645, 303);
            TrwKaynakTablolar.TabIndex = 0;
            TrwKaynakTablolar.AfterSelect += TrwKaynakTablolar_AfterSelect;
            // 
            // BtnOtomatikEsle
            // 
            BtnOtomatikEsle.Location = new Point(1124, 370);
            BtnOtomatikEsle.Name = "BtnOtomatikEsle";
            BtnOtomatikEsle.Size = new Size(156, 65);
            BtnOtomatikEsle.TabIndex = 3;
            BtnOtomatikEsle.Text = "Otomatik eşleştir";
            BtnOtomatikEsle.UseVisualStyleBackColor = true;
            BtnOtomatikEsle.Click += BtnOtomatikEsle_Click;
            // 
            // BtnStrSil
            // 
            BtnStrSil.Location = new Point(1124, 472);
            BtnStrSil.Name = "BtnStrSil";
            BtnStrSil.Size = new Size(153, 66);
            BtnStrSil.TabIndex = 2;
            BtnStrSil.Text = "Satır Sil";
            BtnStrSil.UseVisualStyleBackColor = true;
            BtnStrSil.Click += BtnStrSil_Click;
            // 
            // GrdEslestirme
            // 
            GrdEslestirme.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            GrdEslestirme.Location = new Point(0, 370);
            GrdEslestirme.Name = "GrdEslestirme";
            GrdEslestirme.Size = new Size(1118, 283);
            GrdEslestirme.TabIndex = 1;
            GrdEslestirme.CellContentClick += GrdEslestirme_CellContentClick;
            GrdEslestirme.CellDoubleClick += GrdEslestirme_CellDoubleClick;
            GrdEslestirme.CellValueChanged += GrdEslestirme_CellValueChanged;
            GrdEslestirme.EditingControlShowing += GrdEslestirme_EditingControlShowing;
            // 
            // BtnTransferBaslat
            // 
            BtnTransferBaslat.Location = new Point(1124, 580);
            BtnTransferBaslat.Name = "BtnTransferBaslat";
            BtnTransferBaslat.Size = new Size(156, 73);
            BtnTransferBaslat.TabIndex = 7;
            BtnTransferBaslat.Text = "Transferi Başlat";
            BtnTransferBaslat.UseVisualStyleBackColor = true;
            BtnTransferBaslat.Click += BtnTransferBaslat_Click;
            // 
            // prgTransfer
            // 
            prgTransfer.Location = new Point(624, 682);
            prgTransfer.Name = "prgTransfer";
            prgTransfer.Size = new Size(200, 73);
            prgTransfer.TabIndex = 8;
            // 
            // lstLog
            // 
            lstLog.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lstLog.FormattingEnabled = true;
            lstLog.ItemHeight = 15;
            lstLog.Location = new Point(12, 770);
            lstLog.Name = "lstLog";
            lstLog.Size = new Size(1560, 124);
            lstLog.TabIndex = 9;
            // 
            // BtnGeri
            // 
            BtnGeri.Location = new Point(12, 903);
            BtnGeri.Name = "BtnGeri";
            BtnGeri.Size = new Size(88, 49);
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
            GrbBoxFiltreleme.Controls.Add(RdoBtnTumSatır);
            GrbBoxFiltreleme.Controls.Add(RdoBtnFiltre);
            GrbBoxFiltreleme.Controls.Add(TxtFiltreleme);
            GrbBoxFiltreleme.Controls.Add(BtnFiltreTest);
            GrbBoxFiltreleme.Location = new Point(12, 659);
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
            lblTransferSayisi.Location = new Point(1068, 931);
            lblTransferSayisi.Name = "lblTransferSayisi";
            lblTransferSayisi.Size = new Size(111, 21);
            lblTransferSayisi.TabIndex = 12;
            lblTransferSayisi.Text = "Transfer İslemi";
            // 
            // BtnKynkSutunYkle
            // 
            BtnKynkSutunYkle.Location = new Point(491, 278);
            BtnKynkSutunYkle.Name = "BtnKynkSutunYkle";
            BtnKynkSutunYkle.Size = new Size(148, 41);
            BtnKynkSutunYkle.TabIndex = 16;
            BtnKynkSutunYkle.Text = "Kaynak Sütun Yükle";
            BtnKynkSutunYkle.UseVisualStyleBackColor = true;
            BtnKynkSutunYkle.Click += BtnKynkSutunYkle_Click;
            // 
            // BtnHdfSutunYkle
            // 
            BtnHdfSutunYkle.Location = new Point(470, 278);
            BtnHdfSutunYkle.Name = "BtnHdfSutunYkle";
            BtnHdfSutunYkle.Size = new Size(148, 41);
            BtnHdfSutunYkle.TabIndex = 17;
            BtnHdfSutunYkle.Text = "Hedef Sütun Yükle";
            BtnHdfSutunYkle.UseVisualStyleBackColor = true;
            BtnHdfSutunYkle.Click += BtnHdfSutunYkle_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(BtnKynkSutunYkle);
            groupBox1.Controls.Add(TrwKaynakTablolar);
            groupBox1.Location = new Point(0, 4);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(648, 325);
            groupBox1.TabIndex = 18;
            groupBox1.TabStop = false;
            groupBox1.Text = "Kaynak Tablolar";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(BtnHdfSutunYkle);
            groupBox2.Controls.Add(TrwHedefTablolar);
            groupBox2.Location = new Point(654, 4);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(626, 325);
            groupBox2.TabIndex = 19;
            groupBox2.TabStop = false;
            groupBox2.Text = "Hedef Tablolar";
            // 
            // GrdHedefNullable
            // 
            GrdHedefNullable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            GrdHedefNullable.Location = new Point(1292, 23);
            GrdHedefNullable.Name = "GrdHedefNullable";
            GrdHedefNullable.Size = new Size(229, 341);
            GrdHedefNullable.TabIndex = 20;
            // 
            // FrmVeriEslestirme
            // 
            AutoScaleMode = AutoScaleMode.Inherit;
            BackgroundImageLayout = ImageLayout.Center;
            ClientSize = new Size(1584, 961);
            Controls.Add(GrdHedefNullable);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(lblTransferSayisi);
            Controls.Add(BtnGeri);
            Controls.Add(BtnStrSil);
            Controls.Add(BtnOtomatikEsle);
            Controls.Add(lstLog);
            Controls.Add(prgTransfer);
            Controls.Add(BtnTransferBaslat);
            Controls.Add(GrbBoxFiltreleme);
            Controls.Add(GrdEslestirme);
            MinimizeBox = false;
            MinimumSize = new Size(600, 400);
            Name = "FrmVeriEslestirme";
            Text = "Veri Transferi";
            Load += FrmVeriEslestirme_Load;
            ((System.ComponentModel.ISupportInitialize)GrdEslestirme).EndInit();
            GrbBoxFiltreleme.ResumeLayout(false);
            GrbBoxFiltreleme.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)GrdHedefNullable).EndInit();
            ResumeLayout(false);
            PerformLayout();


        }
        private TreeView TrwHedefTablolar;
        private TreeView TrwKaynakTablolar;
       // private SplitContainer splitContainerMain;
        private DataGridView GrdEslestirme;
        private Button BtnTransferBaslat;
        private ProgressBar prgTransfer;
        private ListBox lstLog;
        private Button BtnGeri;
        private Button BtnOtomatikEsle;
        private Button BtnStrSil;
        private Button BtnFiltreTest;
        private TextBox TxtFiltreleme;
        private RadioButton RdoBtnFiltre;
        private RadioButton RdoBtnTumSatır;
        private GroupBox GrbBoxFiltreleme;
        private Label lblTransferSayisi;
        private Button BtnKynkSutunYkle;
        private Button BtnHdfSutunYkle;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private DataGridView GrdHedefNullable;
    }

}
