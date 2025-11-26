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
            SuspendLayout();
            // 
            // BtnDonusumKaydet
            // 
            BtnDonusumKaydet.Location = new Point(265, 485);
            BtnDonusumKaydet.Name = "BtnDonusumKaydet";
            BtnDonusumKaydet.Size = new Size(119, 51);
            BtnDonusumKaydet.TabIndex = 0;
            BtnDonusumKaydet.Text = "Kaydet";
            BtnDonusumKaydet.UseVisualStyleBackColor = true;
            // 
            // BtnDonusumIptal
            // 
            BtnDonusumIptal.Location = new Point(424, 485);
            BtnDonusumIptal.Name = "BtnDonusumIptal";
            BtnDonusumIptal.Size = new Size(119, 51);
            BtnDonusumIptal.TabIndex = 1;
            BtnDonusumIptal.Text = "İptal";
            BtnDonusumIptal.UseVisualStyleBackColor = true;
            // 
            // DonusumEkrani
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(900, 576);
            Controls.Add(BtnDonusumIptal);
            Controls.Add(BtnDonusumKaydet);
            Name = "DonusumEkrani";
            Text = "Veri Dönüşüm Ekranı";
            ResumeLayout(false);
        }

        #endregion

        private Button BtnDonusumKaydet;
        private Button BtnDonusumIptal;
    }
}