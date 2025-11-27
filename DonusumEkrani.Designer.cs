namespace DataTransfer
{
    partial class DonusumEkrani
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
            BtnDonusumKaydet = new Button();
            BtnDonusumIptal = new Button();
            lblBilgi = new Label();
            BtnEkle = new Button();
            GrdDonusum = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)GrdDonusum).BeginInit();
            SuspendLayout();
            // 
            // BtnDonusumKaydet
            // 
            BtnDonusumKaydet.Location = new Point(401, 569);
            BtnDonusumKaydet.Name = "BtnDonusumKaydet";
            BtnDonusumKaydet.Size = new Size(119, 51);
            BtnDonusumKaydet.TabIndex = 0;
            BtnDonusumKaydet.Text = "Kaydet";
            BtnDonusumKaydet.UseVisualStyleBackColor = true;
            BtnDonusumKaydet.Click += BtnDonusumKaydet_Click;
            // 
            // BtnDonusumIptal
            // 
            BtnDonusumIptal.Location = new Point(560, 569);
            BtnDonusumIptal.Name = "BtnDonusumIptal";
            BtnDonusumIptal.Size = new Size(119, 51);
            BtnDonusumIptal.TabIndex = 1;
            BtnDonusumIptal.Text = "İptal";
            BtnDonusumIptal.UseVisualStyleBackColor = true;
            BtnDonusumIptal.Click += BtnDonusumIptal_Click;
            // 
            // lblBilgi
            // 
            lblBilgi.AutoSize = true;
            lblBilgi.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 162);
            lblBilgi.Location = new Point(95, 31);
            lblBilgi.Name = "lblBilgi";
            lblBilgi.Size = new Size(50, 20);
            lblBilgi.TabIndex = 2;
            lblBilgi.Text = "label1";
            // 
            // BtnEkle
            // 
            BtnEkle.Location = new Point(252, 569);
            BtnEkle.Name = "BtnEkle";
            BtnEkle.Size = new Size(119, 51);
            BtnEkle.TabIndex = 3;
            BtnEkle.Text = "Ekle";
            BtnEkle.UseVisualStyleBackColor = true;
            // 
            // GrdDonusum
            // 
            GrdDonusum.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            GrdDonusum.Location = new Point(95, 83);
            GrdDonusum.Name = "GrdDonusum";
            GrdDonusum.Size = new Size(799, 450);
            GrdDonusum.TabIndex = 4;
            GrdDonusum.CellContentClick += GrdDonusum_CellContentClick;
            // 
            // DonusumEkrani
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(964, 703);
            Controls.Add(GrdDonusum);
            Controls.Add(BtnEkle);
            Controls.Add(lblBilgi);
            Controls.Add(BtnDonusumIptal);
            Controls.Add(BtnDonusumKaydet);
            Name = "DonusumEkrani";
            Text = "Veri Dönüşüm Ekranı";
            Load += DonusumEkrani_Load;
            ((System.ComponentModel.ISupportInitialize)GrdDonusum).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button BtnDonusumKaydet;
        private Button BtnDonusumIptal;
        private Label lblBilgi;
        private Button BtnEkle;
        private DataGridView GrdDonusum;
    }
}