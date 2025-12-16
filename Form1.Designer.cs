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
            label1 = new Label();
            lblkaynak = new Label();
            lblHedef = new Label();
            label2 = new Label();
            label3 = new Label();
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
            TrwHedefTablolar.Location = new Point(0, 21);
            TrwHedefTablolar.Name = "TrwHedefTablolar";
            TrwHedefTablolar.Size = new Size(623, 301);
            TrwHedefTablolar.TabIndex = 0;
            TrwHedefTablolar.AfterSelect += TrwHedefTablolar_AfterSelect;
            // 
            // TrwKaynakTablolar
            // 
            TrwKaynakTablolar.Dock = DockStyle.Left;
            TrwKaynakTablolar.ItemHeight = 18;
            TrwKaynakTablolar.Location = new Point(3, 21);
            TrwKaynakTablolar.Name = "TrwKaynakTablolar";
            TrwKaynakTablolar.Size = new Size(645, 301);
            TrwKaynakTablolar.TabIndex = 0;
            TrwKaynakTablolar.AfterSelect += TrwKaynakTablolar_AfterSelect;
            // 
            // BtnOtomatikEsle
            // 
            BtnOtomatikEsle.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 162);
            BtnOtomatikEsle.Location = new Point(968, 677);
            BtnOtomatikEsle.Name = "BtnOtomatikEsle";
            BtnOtomatikEsle.Size = new Size(191, 90);
            BtnOtomatikEsle.TabIndex = 3;
            BtnOtomatikEsle.Text = "Otomatik Eşleştir";
            BtnOtomatikEsle.UseVisualStyleBackColor = true;
            BtnOtomatikEsle.Click += BtnOtomatikEsle_Click;
            // 
            // BtnStrSil
            // 
            BtnStrSil.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 162);
            BtnStrSil.Location = new Point(1258, 677);
            BtnStrSil.Name = "BtnStrSil";
            BtnStrSil.Size = new Size(191, 87);
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
            GrdEslestirme.Size = new Size(1781, 265);
            GrdEslestirme.TabIndex = 1;
            GrdEslestirme.CellClick += GrdEslestirme_CellClick;
            GrdEslestirme.CellContentClick += GrdEslestirme_CellContentClick;
            GrdEslestirme.CellDoubleClick += GrdEslestirme_CellDoubleClick;
            GrdEslestirme.CellValueChanged += GrdEslestirme_CellValueChanged;
            GrdEslestirme.CurrentCellDirtyStateChanged += GrdEslestirme_CurrentCellDirtyStateChanged;
            GrdEslestirme.EditingControlShowing += GrdEslestirme_EditingControlShowing;
            // 
            // BtnTransferBaslat
            // 
            BtnTransferBaslat.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 162);
            BtnTransferBaslat.Location = new Point(1542, 679);
            BtnTransferBaslat.Name = "BtnTransferBaslat";
            BtnTransferBaslat.Size = new Size(196, 87);
            BtnTransferBaslat.TabIndex = 7;
            BtnTransferBaslat.Text = "Transferi Başlat";
            BtnTransferBaslat.UseVisualStyleBackColor = true;
            BtnTransferBaslat.Click += BtnTransferBaslat_Click;
            // 
            // prgTransfer
            // 
            prgTransfer.Location = new Point(673, 694);
            prgTransfer.Name = "prgTransfer";
            prgTransfer.Size = new Size(200, 73);
            prgTransfer.TabIndex = 8;
            // 
            // lstLog
            // 
            lstLog.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lstLog.FormattingEnabled = true;
            lstLog.ItemHeight = 15;
            lstLog.Location = new Point(10, 805);
            lstLog.Name = "lstLog";
            lstLog.Size = new Size(1743, 139);
            lstLog.TabIndex = 9;
            // 
            // BtnGeri
            // 
            BtnGeri.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 162);
            BtnGeri.Location = new Point(12, 950);
            BtnGeri.Name = "BtnGeri";
            BtnGeri.Size = new Size(105, 49);
            BtnGeri.TabIndex = 11;
            BtnGeri.Text = "Geri";
            BtnGeri.UseVisualStyleBackColor = true;
            BtnGeri.Click += BtnGeri_Click;
            // 
            // BtnFiltreTest
            // 
            BtnFiltreTest.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnFiltreTest.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 162);
            BtnFiltreTest.Location = new Point(489, 71);
            BtnFiltreTest.Name = "BtnFiltreTest";
            BtnFiltreTest.Size = new Size(99, 37);
            BtnFiltreTest.TabIndex = 3;
            BtnFiltreTest.Text = "Test Et";
            BtnFiltreTest.UseVisualStyleBackColor = true;
            BtnFiltreTest.Click += BtnFiltreTest_Click;
            // 
            // TxtFiltreleme
            // 
            TxtFiltreleme.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            TxtFiltreleme.Location = new Point(16, 71);
            TxtFiltreleme.Name = "TxtFiltreleme";
            TxtFiltreleme.Size = new Size(443, 23);
            TxtFiltreleme.TabIndex = 2;
            // 
            // RdoBtnFiltre
            // 
            RdoBtnFiltre.AutoSize = true;
            RdoBtnFiltre.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 162);
            RdoBtnFiltre.Location = new Point(142, 33);
            RdoBtnFiltre.Name = "RdoBtnFiltre";
            RdoBtnFiltre.Size = new Size(110, 24);
            RdoBtnFiltre.TabIndex = 1;
            RdoBtnFiltre.TabStop = true;
            RdoBtnFiltre.Text = "Filtre Uygula";
            RdoBtnFiltre.UseVisualStyleBackColor = true;
            // 
            // RdoBtnTumSatır
            // 
            RdoBtnTumSatır.AutoSize = true;
            RdoBtnTumSatır.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 162);
            RdoBtnTumSatır.Location = new Point(16, 33);
            RdoBtnTumSatır.Name = "RdoBtnTumSatır";
            RdoBtnTumSatır.Size = new Size(107, 24);
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
            GrbBoxFiltreleme.Location = new Point(12, 666);
            GrbBoxFiltreleme.Name = "GrbBoxFiltreleme";
            GrbBoxFiltreleme.Size = new Size(594, 122);
            GrbBoxFiltreleme.TabIndex = 2;
            GrbBoxFiltreleme.TabStop = false;
            GrbBoxFiltreleme.Text = "Filtreleme";
            // 
            // lblTransferSayisi
            // 
            lblTransferSayisi.AutoSize = true;
            lblTransferSayisi.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 162);
            lblTransferSayisi.Location = new Point(1258, 974);
            lblTransferSayisi.Name = "lblTransferSayisi";
            lblTransferSayisi.Size = new Size(133, 25);
            lblTransferSayisi.TabIndex = 12;
            lblTransferSayisi.Text = "Transfer İslemi";
            // 
            // BtnKynkSutunYkle
            // 
            BtnKynkSutunYkle.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 162);
            BtnKynkSutunYkle.Location = new Point(494, 278);
            BtnKynkSutunYkle.Name = "BtnKynkSutunYkle";
            BtnKynkSutunYkle.Size = new Size(148, 41);
            BtnKynkSutunYkle.TabIndex = 16;
            BtnKynkSutunYkle.Text = "Kaynak Sütunları Yükle";
            BtnKynkSutunYkle.UseVisualStyleBackColor = true;
            BtnKynkSutunYkle.Click += BtnKynkSutunYkle_Click;
            // 
            // BtnHdfSutunYkle
            // 
            BtnHdfSutunYkle.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 162);
            BtnHdfSutunYkle.Location = new Point(470, 278);
            BtnHdfSutunYkle.Name = "BtnHdfSutunYkle";
            BtnHdfSutunYkle.Size = new Size(148, 41);
            BtnHdfSutunYkle.TabIndex = 17;
            BtnHdfSutunYkle.Text = "Hedef Sütunları Yükle";
            BtnHdfSutunYkle.UseVisualStyleBackColor = true;
            BtnHdfSutunYkle.Click += BtnHdfSutunYkle_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(BtnKynkSutunYkle);
            groupBox1.Controls.Add(TrwKaynakTablolar);
            groupBox1.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 162);
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
            groupBox2.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 162);
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
            GrdHedefNullable.Location = new Point(1297, 23);
            GrdHedefNullable.Name = "GrdHedefNullable";
            GrdHedefNullable.Size = new Size(234, 306);
            GrdHedefNullable.TabIndex = 20;
            GrdHedefNullable.CellDoubleClick += GrdHedefNullable_CellDoubleClick;
            GrdHedefNullable.ColumnHeaderMouseClick += GrdHedefNullable_ColumnHeaderMouseClick;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 162);
            label1.Location = new Point(1301, 4);
            label1.Name = "label1";
            label1.Size = new Size(81, 17);
            label1.TabIndex = 21;
            label1.Text = "Hedef Kolon";
            // 
            // lblkaynak
            // 
            lblkaynak.AutoSize = true;
            lblkaynak.Font = new Font("Segoe UI", 11.25F);
            lblkaynak.Location = new Point(129, 344);
            lblkaynak.Name = "lblkaynak";
            lblkaynak.Size = new Size(96, 20);
            lblkaynak.TabIndex = 22;
            lblkaynak.Text = "Kaynak Tablo";
            // 
            // lblHedef
            // 
            lblHedef.AutoSize = true;
            lblHedef.Font = new Font("Segoe UI", 11.25F);
            lblHedef.Location = new Point(1155, 344);
            lblHedef.Name = "lblHedef";
            lblHedef.Size = new Size(90, 20);
            lblHedef.TabIndex = 23;
            lblHedef.Text = "Hedef Tablo";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 11.25F);
            label2.Location = new Point(1052, 344);
            label2.Name = "label2";
            label2.Size = new Size(97, 20);
            label2.TabIndex = 24;
            label2.Text = "Hedef Tablo :";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 11.25F);
            label3.Location = new Point(10, 344);
            label3.Name = "label3";
            label3.Size = new Size(103, 20);
            label3.TabIndex = 25;
            label3.Text = "Kaynak Tablo :";
            // 
            // FrmVeriEslestirme
            // 
            AutoScaleMode = AutoScaleMode.Inherit;
            BackgroundImageLayout = ImageLayout.Center;
            ClientSize = new Size(1784, 1011);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(lblHedef);
            Controls.Add(lblkaynak);
            Controls.Add(label1);
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
        private Label label1;
        private Label lblkaynak;
        private Label lblHedef;
        private Label label2;
        private Label label3;
    }

}
