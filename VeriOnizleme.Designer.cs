namespace DataTransfer
{
    partial class FrmVeriOnizleme
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
            GrdOnizleme = new DataGridView();
            BtnOnayla = new Button();
            BtnIptal = new Button();
            LblBilgi = new Label();
            ((System.ComponentModel.ISupportInitialize)GrdOnizleme).BeginInit();
            SuspendLayout();
            // 
            // GrdOnizleme
            // 
            GrdOnizleme.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            GrdOnizleme.Location = new Point(53, 69);
            GrdOnizleme.Name = "GrdOnizleme";
            GrdOnizleme.Size = new Size(1217, 541);
            GrdOnizleme.TabIndex = 0;
            GrdOnizleme.CellContentClick += GrdOnizleme_CellContentClick;
            // 
            // BtnOnayla
            // 
            BtnOnayla.Location = new Point(132, 660);
            BtnOnayla.Name = "BtnOnayla";
            BtnOnayla.Size = new Size(88, 39);
            BtnOnayla.TabIndex = 1;
            BtnOnayla.Text = "Onayla";
            BtnOnayla.UseVisualStyleBackColor = true;
            BtnOnayla.Click += BtnOnayla_Click;
            // 
            // BtnIptal
            // 
            BtnIptal.Location = new Point(226, 660);
            BtnIptal.Name = "BtnIptal";
            BtnIptal.Size = new Size(88, 39);
            BtnIptal.TabIndex = 2;
            BtnIptal.Text = "İptal";
            BtnIptal.UseVisualStyleBackColor = true;
            BtnIptal.Click += BtnIptal_Click;
            // 
            // LblBilgi
            // 
            LblBilgi.AutoSize = true;
            LblBilgi.Location = new Point(53, 30);
            LblBilgi.Name = "LblBilgi";
            LblBilgi.Size = new Size(38, 15);
            LblBilgi.TabIndex = 3;
            LblBilgi.Text = "label1";
            // 
            // FrmVeriOnizleme
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1338, 732);
            Controls.Add(LblBilgi);
            Controls.Add(BtnIptal);
            Controls.Add(BtnOnayla);
            Controls.Add(GrdOnizleme);
            Name = "FrmVeriOnizleme";
            Text = "VeriOnizleme";
            Load += FrmVeriOnizleme_Load;
            ((System.ComponentModel.ISupportInitialize)GrdOnizleme).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView GrdOnizleme;
        private Button BtnOnayla;
        private Button BtnIptal;
        private Label LblBilgi;
    }
}