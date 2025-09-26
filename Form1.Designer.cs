namespace DataTransfer
{
    partial class Form1
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
            this.LblKaynakSunucu = new System.Windows.Forms.Label();
            this.TxtKaynakSunucu = new System.Windows.Forms.TextBox();
            this.LblKaynakDatabase = new System.Windows.Forms.Label();
            this.LblKaynakSutun = new System.Windows.Forms.Label();
            this.LblKaynakTablo = new System.Windows.Forms.Label();
            this.CbxKaynakDatabase = new System.Windows.Forms.ComboBox();
            this.CbxKaynakTablo = new System.Windows.Forms.ComboBox();
            this.CbxHedefTablo = new System.Windows.Forms.ComboBox();
            this.CbxHedefDatabase = new System.Windows.Forms.ComboBox();
            this.LblHedefTablo = new System.Windows.Forms.Label();
            this.LblHedefSutun = new System.Windows.Forms.Label();
            this.LblHedefDatabase = new System.Windows.Forms.Label();
            this.TxtHedefSunucu = new System.Windows.Forms.TextBox();
            this.LblHedefSunucu = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.BtnBaglanti = new System.Windows.Forms.Button();
            this.BtnDogrula = new System.Windows.Forms.Button();
            this.BtnTransferBaslat = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.treeView2 = new System.Windows.Forms.TreeView();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // LblKaynakSunucu
            // 
            this.LblKaynakSunucu.AutoSize = true;
            this.LblKaynakSunucu.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.LblKaynakSunucu.Location = new System.Drawing.Point(46, 46);
            this.LblKaynakSunucu.Name = "LblKaynakSunucu";
            this.LblKaynakSunucu.Size = new System.Drawing.Size(119, 18);
            this.LblKaynakSunucu.TabIndex = 0;
            this.LblKaynakSunucu.Text = "Kaynak Sunucu :";
            // 
            // TxtKaynakSunucu
            // 
            this.TxtKaynakSunucu.Location = new System.Drawing.Point(171, 43);
            this.TxtKaynakSunucu.Name = "TxtKaynakSunucu";
            this.TxtKaynakSunucu.Size = new System.Drawing.Size(146, 21);
            this.TxtKaynakSunucu.TabIndex = 1;
            // 
            // LblKaynakDatabase
            // 
            this.LblKaynakDatabase.AutoSize = true;
            this.LblKaynakDatabase.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.LblKaynakDatabase.Location = new System.Drawing.Point(46, 97);
            this.LblKaynakDatabase.Name = "LblKaynakDatabase";
            this.LblKaynakDatabase.Size = new System.Drawing.Size(89, 18);
            this.LblKaynakDatabase.TabIndex = 2;
            this.LblKaynakDatabase.Text = "Veri Tabanı :";
            // 
            // LblKaynakSutun
            // 
            this.LblKaynakSutun.AutoSize = true;
            this.LblKaynakSutun.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.LblKaynakSutun.Location = new System.Drawing.Point(15, 40);
            this.LblKaynakSutun.Name = "LblKaynakSutun";
            this.LblKaynakSutun.Size = new System.Drawing.Size(167, 18);
            this.LblKaynakSutun.TabIndex = 3;
            this.LblKaynakSutun.Text = "Kaynak Tablo Sütünları :";
            this.LblKaynakSutun.Click += new System.EventHandler(this.LblKaynakSutun_Click);
            // 
            // LblKaynakTablo
            // 
            this.LblKaynakTablo.AutoSize = true;
            this.LblKaynakTablo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.LblKaynakTablo.Location = new System.Drawing.Point(46, 153);
            this.LblKaynakTablo.Name = "LblKaynakTablo";
            this.LblKaynakTablo.Size = new System.Drawing.Size(77, 18);
            this.LblKaynakTablo.TabIndex = 4;
            this.LblKaynakTablo.Text = "Tablo Adı :";
            // 
            // CbxKaynakDatabase
            // 
            this.CbxKaynakDatabase.FormattingEnabled = true;
            this.CbxKaynakDatabase.Location = new System.Drawing.Point(171, 97);
            this.CbxKaynakDatabase.Name = "CbxKaynakDatabase";
            this.CbxKaynakDatabase.Size = new System.Drawing.Size(146, 23);
            this.CbxKaynakDatabase.TabIndex = 5;
            this.CbxKaynakDatabase.SelectedIndexChanged += new System.EventHandler(this.CbxKaynakDatabase_SelectedIndexChanged);
            // 
            // CbxKaynakTablo
            // 
            this.CbxKaynakTablo.FormattingEnabled = true;
            this.CbxKaynakTablo.Location = new System.Drawing.Point(171, 152);
            this.CbxKaynakTablo.Name = "CbxKaynakTablo";
            this.CbxKaynakTablo.Size = new System.Drawing.Size(146, 23);
            this.CbxKaynakTablo.TabIndex = 6;
            // 
            // CbxHedefTablo
            // 
            this.CbxHedefTablo.FormattingEnabled = true;
            this.CbxHedefTablo.Location = new System.Drawing.Point(705, 153);
            this.CbxHedefTablo.Name = "CbxHedefTablo";
            this.CbxHedefTablo.Size = new System.Drawing.Size(146, 23);
            this.CbxHedefTablo.TabIndex = 13;
            this.CbxHedefTablo.SelectedIndexChanged += new System.EventHandler(this.comboBox3_SelectedIndexChanged);
            // 
            // CbxHedefDatabase
            // 
            this.CbxHedefDatabase.FormattingEnabled = true;
            this.CbxHedefDatabase.Location = new System.Drawing.Point(705, 97);
            this.CbxHedefDatabase.Name = "CbxHedefDatabase";
            this.CbxHedefDatabase.Size = new System.Drawing.Size(146, 23);
            this.CbxHedefDatabase.TabIndex = 12;
            this.CbxHedefDatabase.SelectedIndexChanged += new System.EventHandler(this.comboBox4_SelectedIndexChanged);
            // 
            // LblHedefTablo
            // 
            this.LblHedefTablo.AutoSize = true;
            this.LblHedefTablo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.LblHedefTablo.Location = new System.Drawing.Point(580, 153);
            this.LblHedefTablo.Name = "LblHedefTablo";
            this.LblHedefTablo.Size = new System.Drawing.Size(77, 18);
            this.LblHedefTablo.TabIndex = 11;
            this.LblHedefTablo.Text = "Tablo Adı :";
            this.LblHedefTablo.Click += new System.EventHandler(this.label1_Click);
            // 
            // LblHedefSutun
            // 
            this.LblHedefSutun.AutoSize = true;
            this.LblHedefSutun.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.LblHedefSutun.Location = new System.Drawing.Point(437, 42);
            this.LblHedefSutun.Name = "LblHedefSutun";
            this.LblHedefSutun.Size = new System.Drawing.Size(157, 18);
            this.LblHedefSutun.TabIndex = 10;
            this.LblHedefSutun.Text = "Hedef Tablo Sütünları :";
            this.LblHedefSutun.Click += new System.EventHandler(this.label2_Click);
            // 
            // LblHedefDatabase
            // 
            this.LblHedefDatabase.AutoSize = true;
            this.LblHedefDatabase.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.LblHedefDatabase.Location = new System.Drawing.Point(580, 97);
            this.LblHedefDatabase.Name = "LblHedefDatabase";
            this.LblHedefDatabase.Size = new System.Drawing.Size(89, 18);
            this.LblHedefDatabase.TabIndex = 9;
            this.LblHedefDatabase.Text = "Veri Tabanı :";
            this.LblHedefDatabase.Click += new System.EventHandler(this.label3_Click);
            // 
            // TxtHedefSunucu
            // 
            this.TxtHedefSunucu.Location = new System.Drawing.Point(705, 43);
            this.TxtHedefSunucu.Name = "TxtHedefSunucu";
            this.TxtHedefSunucu.Size = new System.Drawing.Size(146, 21);
            this.TxtHedefSunucu.TabIndex = 8;
            this.TxtHedefSunucu.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // LblHedefSunucu
            // 
            this.LblHedefSunucu.AutoSize = true;
            this.LblHedefSunucu.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.LblHedefSunucu.Location = new System.Drawing.Point(580, 46);
            this.LblHedefSunucu.Name = "LblHedefSunucu";
            this.LblHedefSunucu.Size = new System.Drawing.Size(109, 18);
            this.LblHedefSunucu.TabIndex = 14;
            this.LblHedefSunucu.Text = "Hedef Sunucu :";
            this.LblHedefSunucu.Click += new System.EventHandler(this.LblHedefSunucu_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.treeView2);
            this.groupBox1.Controls.Add(this.treeView1);
            this.groupBox1.Controls.Add(this.LblKaynakSutun);
            this.groupBox1.Controls.Add(this.LblHedefSutun);
            this.groupBox1.Location = new System.Drawing.Point(49, 258);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(819, 296);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // BtnBaglanti
            // 
            this.BtnBaglanti.Location = new System.Drawing.Point(18, 31);
            this.BtnBaglanti.Name = "BtnBaglanti";
            this.BtnBaglanti.Size = new System.Drawing.Size(125, 23);
            this.BtnBaglanti.TabIndex = 16;
            this.BtnBaglanti.Text = "Baglantı test et";
            this.BtnBaglanti.UseVisualStyleBackColor = true;
            // 
            // BtnDogrula
            // 
            this.BtnDogrula.Location = new System.Drawing.Point(18, 104);
            this.BtnDogrula.Name = "BtnDogrula";
            this.BtnDogrula.Size = new System.Drawing.Size(125, 23);
            this.BtnDogrula.TabIndex = 17;
            this.BtnDogrula.Text = "Eslesmeyi Dogrula";
            this.BtnDogrula.UseVisualStyleBackColor = true;
            // 
            // BtnTransferBaslat
            // 
            this.BtnTransferBaslat.Location = new System.Drawing.Point(18, 177);
            this.BtnTransferBaslat.Name = "BtnTransferBaslat";
            this.BtnTransferBaslat.Size = new System.Drawing.Size(125, 41);
            this.BtnTransferBaslat.TabIndex = 18;
            this.BtnTransferBaslat.Text = "Veri Transferini baslat";
            this.BtnTransferBaslat.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 254);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 15);
            this.label1.TabIndex = 19;
            this.label1.Text = "Hata";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.BtnDogrula);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.BtnBaglanti);
            this.groupBox2.Controls.Add(this.BtnTransferBaslat);
            this.groupBox2.Location = new System.Drawing.Point(943, 46);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(169, 292);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "groupBox2";
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(18, 72);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(345, 207);
            this.treeView1.TabIndex = 0;
            // 
            // treeView2
            // 
            this.treeView2.Location = new System.Drawing.Point(440, 72);
            this.treeView2.Name = "treeView2";
            this.treeView2.Size = new System.Drawing.Size(330, 207);
            this.treeView2.TabIndex = 1;
            this.treeView2.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView2_AfterSelect);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(76, 252);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 15);
            this.label2.TabIndex = 20;
            this.label2.Text = "label2";
            this.label2.Click += new System.EventHandler(this.label2_Click_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1141, 576);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.LblHedefSunucu);
            this.Controls.Add(this.CbxHedefTablo);
            this.Controls.Add(this.CbxHedefDatabase);
            this.Controls.Add(this.LblHedefTablo);
            this.Controls.Add(this.LblHedefDatabase);
            this.Controls.Add(this.TxtHedefSunucu);
            this.Controls.Add(this.CbxKaynakTablo);
            this.Controls.Add(this.CbxKaynakDatabase);
            this.Controls.Add(this.LblKaynakTablo);
            this.Controls.Add(this.LblKaynakDatabase);
            this.Controls.Add(this.TxtKaynakSunucu);
            this.Controls.Add(this.LblKaynakSunucu);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Veri Aktarımı";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LblKaynakSunucu;
        private System.Windows.Forms.TextBox TxtKaynakSunucu;
        private System.Windows.Forms.Label LblKaynakDatabase;
        private System.Windows.Forms.Label LblKaynakSutun;
        private System.Windows.Forms.Label LblKaynakTablo;
        private System.Windows.Forms.ComboBox CbxKaynakDatabase;
        private System.Windows.Forms.ComboBox CbxKaynakTablo;
        private System.Windows.Forms.ComboBox CbxHedefTablo;
        private System.Windows.Forms.ComboBox CbxHedefDatabase;
        private System.Windows.Forms.Label LblHedefTablo;
        private System.Windows.Forms.Label LblHedefSutun;
        private System.Windows.Forms.Label LblHedefDatabase;
        private System.Windows.Forms.TextBox TxtHedefSunucu;
        private System.Windows.Forms.Label LblHedefSunucu;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button BtnBaglanti;
        private System.Windows.Forms.Button BtnDogrula;
        private System.Windows.Forms.Button BtnTransferBaslat;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TreeView treeView2;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Label label2;
    }
}

