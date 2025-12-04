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
            BtnTopluEkle = new Button();
            BtnIptal = new Button();
            ((System.ComponentModel.ISupportInitialize)GrdKaynakDonusum).BeginInit();
            SuspendLayout();
            // 
            // GrdKaynakDonusum
            // 
            GrdKaynakDonusum.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            GrdKaynakDonusum.Location = new Point(130, 52);
            GrdKaynakDonusum.Name = "GrdKaynakDonusum";
            GrdKaynakDonusum.Size = new Size(599, 325);
            GrdKaynakDonusum.TabIndex = 0;
            GrdKaynakDonusum.CellClick += GrdKaynakDonusum_CellClick;
            GrdKaynakDonusum.CellFormatting += GrdKaynakDonusum_CellFormatting;
            // 
            // BtnKaydet
            // 
            BtnKaydet.Location = new Point(185, 444);
            BtnKaydet.Name = "BtnKaydet";
            BtnKaydet.Size = new Size(104, 51);
            BtnKaydet.TabIndex = 1;
            BtnKaydet.Text = "Kaydet";
            BtnKaydet.UseVisualStyleBackColor = true;
            BtnKaydet.Click += BtnKaydet_Click;
            // 
            // BtnTopluEkle
            // 
            BtnTopluEkle.Location = new Point(498, 444);
            BtnTopluEkle.Name = "BtnTopluEkle";
            BtnTopluEkle.Size = new Size(127, 51);
            BtnTopluEkle.TabIndex = 2;
            BtnTopluEkle.Text = "Eşleşmeyenleri Ekle";
            BtnTopluEkle.UseVisualStyleBackColor = true;
            BtnTopluEkle.Click += BtnTopluEkle_Click;
            // 
            // BtnIptal
            // 
            BtnIptal.Location = new Point(342, 444);
            BtnIptal.Name = "BtnIptal";
            BtnIptal.Size = new Size(111, 51);
            BtnIptal.TabIndex = 3;
            BtnIptal.Text = "İptal";
            BtnIptal.UseVisualStyleBackColor = true;
            BtnIptal.Click += BtnIptal_Click;
            // 
            // KaynakDonusumEkrani
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(874, 547);
            Controls.Add(BtnIptal);
            Controls.Add(BtnTopluEkle);
            Controls.Add(BtnKaydet);
            Controls.Add(GrdKaynakDonusum);
            Name = "KaynakDonusumEkrani";
            Text = "KaynakDonusumEkrani";
            Load += KaynakDonusumEkrani_Load;
            ((System.ComponentModel.ISupportInitialize)GrdKaynakDonusum).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView GrdKaynakDonusum;
        private Button BtnKaydet;
        private Button BtnTopluEkle;
        private Button BtnIptal;
    }
}