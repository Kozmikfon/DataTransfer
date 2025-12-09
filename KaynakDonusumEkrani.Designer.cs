namespace DataTransfer
{
    partial class KaynakDonusumEkrani
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
            GrdKaynakDonusum = new DataGridView();
            BtnKaydet = new Button();
            BtnIptal = new Button();
            GrdUyari = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)GrdKaynakDonusum).BeginInit();
            ((System.ComponentModel.ISupportInitialize)GrdUyari).BeginInit();
            SuspendLayout();
            // 
            // GrdKaynakDonusum
            // 
            GrdKaynakDonusum.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            GrdKaynakDonusum.Location = new Point(62, 52);
            GrdKaynakDonusum.Name = "GrdKaynakDonusum";
            GrdKaynakDonusum.Size = new Size(618, 311);
            GrdKaynakDonusum.TabIndex = 0;
            GrdKaynakDonusum.CellClick += GrdKaynakDonusum_CellClick;
            GrdKaynakDonusum.CellFormatting += GrdKaynakDonusum_CellFormatting;
            // 
            // BtnKaydet
            // 
            BtnKaydet.Location = new Point(193, 417);
            BtnKaydet.Name = "BtnKaydet";
            BtnKaydet.Size = new Size(104, 51);
            BtnKaydet.TabIndex = 1;
            BtnKaydet.Text = "Kaydet";
            BtnKaydet.UseVisualStyleBackColor = true;
            BtnKaydet.Click += BtnKaydet_Click;
            // 
            // BtnIptal
            // 
            BtnIptal.Location = new Point(350, 417);
            BtnIptal.Name = "BtnIptal";
            BtnIptal.Size = new Size(111, 51);
            BtnIptal.TabIndex = 3;
            BtnIptal.Text = "İptal";
            BtnIptal.UseVisualStyleBackColor = true;
            BtnIptal.Click += BtnIptal_Click;
            // 
            // GrdUyari
            // 
            GrdUyari.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            GrdUyari.Location = new Point(734, 52);
            GrdUyari.Name = "GrdUyari";
            GrdUyari.Size = new Size(413, 192);
            GrdUyari.TabIndex = 4;
            // 
            // KaynakDonusumEkrani
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1213, 519);
            Controls.Add(GrdUyari);
            Controls.Add(BtnIptal);
            Controls.Add(BtnKaydet);
            Controls.Add(GrdKaynakDonusum);
            Name = "KaynakDonusumEkrani";
            Text = "KaynakDonusumEkrani";
            Load += KaynakDonusumEkrani_Load;
            ((System.ComponentModel.ISupportInitialize)GrdKaynakDonusum).EndInit();
            ((System.ComponentModel.ISupportInitialize)GrdUyari).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView GrdKaynakDonusum;
        private Button BtnKaydet;
        private Button BtnIptal;
        private DataGridView GrdUyari;
    }
}