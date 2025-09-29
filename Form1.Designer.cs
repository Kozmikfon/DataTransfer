namespace DataTransfer
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            BtnBaglantiTest = new Button();
            LblKynkSunucu = new Label();
            lblKynkVeri = new Label();
            LblKynkTablo = new Label();
            LblHdfSunucu = new Label();
            LblHdfVeri = new Label();
            LblHdfTablo = new Label();
            textBox1 = new TextBox();
            comboBox1 = new ComboBox();
            comboBox2 = new ComboBox();
            textBox2 = new TextBox();
            comboBox3 = new ComboBox();
            comboBox4 = new ComboBox();
            BtnVeriAktarim = new Button();
            button1 = new Button();
            groupBox1 = new GroupBox();
            DataGrdvwKaynak = new DataGridView();
            LblKynkSutun = new Label();
            BtnKynkKolonEslestir = new Button();
            groupBox2 = new GroupBox();
            DataGrdvwHedef = new DataGridView();
            LblHdfSutun = new Label();
            button3 = new Button();
            groupBox3 = new GroupBox();
            groupBox4 = new GroupBox();
            listBox1 = new ListBox();
            panel1 = new Panel();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)DataGrdvwKaynak).BeginInit();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)DataGrdvwHedef).BeginInit();
            groupBox3.SuspendLayout();
            groupBox4.SuspendLayout();
            SuspendLayout();
            // 
            // BtnBaglantiTest
            // 
            BtnBaglantiTest.Location = new Point(351, 17);
            BtnBaglantiTest.Name = "BtnBaglantiTest";
            BtnBaglantiTest.Size = new Size(118, 34);
            BtnBaglantiTest.TabIndex = 0;
            BtnBaglantiTest.Text = "Bağlantı Test Et";
            BtnBaglantiTest.UseVisualStyleBackColor = true;
            // 
            // LblKynkSunucu
            // 
            LblKynkSunucu.AutoSize = true;
            LblKynkSunucu.Font = new Font("Segoe UI", 11.25F);
            LblKynkSunucu.Location = new Point(8, 55);
            LblKynkSunucu.Name = "LblKynkSunucu";
            LblKynkSunucu.Size = new Size(63, 20);
            LblKynkSunucu.TabIndex = 1;
            LblKynkSunucu.Text = "Sunucu :";
            // 
            // lblKynkVeri
            // 
            lblKynkVeri.AutoSize = true;
            lblKynkVeri.Font = new Font("Segoe UI", 11.25F);
            lblKynkVeri.Location = new Point(9, 103);
            lblKynkVeri.Name = "lblKynkVeri";
            lblKynkVeri.Size = new Size(87, 20);
            lblKynkVeri.TabIndex = 2;
            lblKynkVeri.Text = "Veri tabanı :";
            lblKynkVeri.Click += lblKynkVeri_Click;
            // 
            // LblKynkTablo
            // 
            LblKynkTablo.AutoSize = true;
            LblKynkTablo.Font = new Font("Segoe UI", 11.25F);
            LblKynkTablo.Location = new Point(9, 149);
            LblKynkTablo.Name = "LblKynkTablo";
            LblKynkTablo.Size = new Size(79, 20);
            LblKynkTablo.TabIndex = 3;
            LblKynkTablo.Text = "Tablo Adı :";
            // 
            // LblHdfSunucu
            // 
            LblHdfSunucu.AutoSize = true;
            LblHdfSunucu.Font = new Font("Segoe UI", 11.25F);
            LblHdfSunucu.Location = new Point(15, 57);
            LblHdfSunucu.Name = "LblHdfSunucu";
            LblHdfSunucu.Size = new Size(63, 20);
            LblHdfSunucu.TabIndex = 4;
            LblHdfSunucu.Text = "Sunucu :";
            // 
            // LblHdfVeri
            // 
            LblHdfVeri.AutoSize = true;
            LblHdfVeri.Font = new Font("Segoe UI", 11.25F);
            LblHdfVeri.Location = new Point(17, 103);
            LblHdfVeri.Name = "LblHdfVeri";
            LblHdfVeri.Size = new Size(88, 20);
            LblHdfVeri.TabIndex = 5;
            LblHdfVeri.Text = "Veri Tabanı :";
            LblHdfVeri.Click += LblHdfVeri_Click;
            // 
            // LblHdfTablo
            // 
            LblHdfTablo.AutoSize = true;
            LblHdfTablo.Font = new Font("Segoe UI", 11.25F);
            LblHdfTablo.Location = new Point(17, 149);
            LblHdfTablo.Name = "LblHdfTablo";
            LblHdfTablo.Size = new Size(79, 20);
            LblHdfTablo.TabIndex = 6;
            LblHdfTablo.Text = "Tablo Adı :";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(136, 55);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(127, 23);
            textBox1.TabIndex = 7;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(136, 100);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(127, 23);
            comboBox1.TabIndex = 8;
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(136, 146);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(127, 23);
            comboBox2.TabIndex = 9;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(130, 56);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(129, 23);
            textBox2.TabIndex = 10;
            // 
            // comboBox3
            // 
            comboBox3.FormattingEnabled = true;
            comboBox3.Location = new Point(130, 100);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new Size(128, 23);
            comboBox3.TabIndex = 11;
            // 
            // comboBox4
            // 
            comboBox4.FormattingEnabled = true;
            comboBox4.Location = new Point(130, 146);
            comboBox4.Name = "comboBox4";
            comboBox4.Size = new Size(128, 23);
            comboBox4.TabIndex = 12;
            // 
            // BtnVeriAktarim
            // 
            BtnVeriAktarim.Location = new Point(193, 17);
            BtnVeriAktarim.Name = "BtnVeriAktarim";
            BtnVeriAktarim.Size = new Size(121, 42);
            BtnVeriAktarim.TabIndex = 14;
            BtnVeriAktarim.Text = "Tablo Eşleşmesini Doğrula";
            BtnVeriAktarim.UseVisualStyleBackColor = true;
            BtnVeriAktarim.Click += BtnVeriAktarim_Click;
            // 
            // button1
            // 
            button1.Location = new Point(59, 17);
            button1.Name = "button1";
            button1.Size = new Size(118, 45);
            button1.TabIndex = 15;
            button1.Text = "Veri Transferini Başlat";
            button1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(DataGrdvwKaynak);
            groupBox1.Controls.Add(LblKynkSutun);
            groupBox1.Controls.Add(BtnKynkKolonEslestir);
            groupBox1.Controls.Add(LblKynkSunucu);
            groupBox1.Controls.Add(lblKynkVeri);
            groupBox1.Controls.Add(LblKynkTablo);
            groupBox1.Controls.Add(comboBox2);
            groupBox1.Controls.Add(textBox1);
            groupBox1.Controls.Add(comboBox1);
            groupBox1.Dock = DockStyle.Left;
            groupBox1.Location = new Point(0, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(296, 667);
            groupBox1.TabIndex = 16;
            groupBox1.TabStop = false;
            groupBox1.Text = "Kaynak";
            // 
            // DataGrdvwKaynak
            // 
            DataGrdvwKaynak.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DataGrdvwKaynak.Location = new Point(23, 307);
            DataGrdvwKaynak.Name = "DataGrdvwKaynak";
            DataGrdvwKaynak.Size = new Size(240, 174);
            DataGrdvwKaynak.TabIndex = 19;
            // 
            // LblKynkSutun
            // 
            LblKynkSutun.AutoSize = true;
            LblKynkSutun.Location = new Point(23, 276);
            LblKynkSutun.Name = "LblKynkSutun";
            LblKynkSutun.Size = new Size(72, 15);
            LblKynkSutun.TabIndex = 18;
            LblKynkSutun.Text = "datagridvew";
            // 
            // BtnKynkKolonEslestir
            // 
            BtnKynkKolonEslestir.Location = new Point(27, 216);
            BtnKynkKolonEslestir.Name = "BtnKynkKolonEslestir";
            BtnKynkKolonEslestir.Size = new Size(216, 29);
            BtnKynkKolonEslestir.TabIndex = 10;
            BtnKynkKolonEslestir.Text = "Kolonları Yükle";
            BtnKynkKolonEslestir.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(DataGrdvwHedef);
            groupBox2.Controls.Add(LblHdfSutun);
            groupBox2.Controls.Add(button3);
            groupBox2.Controls.Add(comboBox3);
            groupBox2.Controls.Add(comboBox4);
            groupBox2.Controls.Add(textBox2);
            groupBox2.Controls.Add(LblHdfVeri);
            groupBox2.Controls.Add(LblHdfSunucu);
            groupBox2.Controls.Add(LblHdfTablo);
            groupBox2.Dock = DockStyle.Right;
            groupBox2.Location = new Point(812, 0);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(296, 667);
            groupBox2.TabIndex = 17;
            groupBox2.TabStop = false;
            groupBox2.Text = "Hedef";
            groupBox2.Enter += groupBox2_Enter;
            // 
            // DataGrdvwHedef
            // 
            DataGrdvwHedef.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DataGrdvwHedef.Location = new Point(19, 307);
            DataGrdvwHedef.Name = "DataGrdvwHedef";
            DataGrdvwHedef.Size = new Size(240, 174);
            DataGrdvwHedef.TabIndex = 15;
            // 
            // LblHdfSutun
            // 
            LblHdfSutun.AutoSize = true;
            LblHdfSutun.Location = new Point(19, 276);
            LblHdfSutun.Name = "LblHdfSutun";
            LblHdfSutun.Size = new Size(75, 15);
            LblHdfSutun.TabIndex = 14;
            LblHdfSutun.Text = "datagirdview";
            // 
            // button3
            // 
            button3.Location = new Point(39, 216);
            button3.Name = "button3";
            button3.Size = new Size(219, 26);
            button3.TabIndex = 13;
            button3.Text = "Kolonları Yükle";
            button3.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(BtnBaglantiTest);
            groupBox3.Controls.Add(BtnVeriAktarim);
            groupBox3.Controls.Add(button1);
            groupBox3.Dock = DockStyle.Bottom;
            groupBox3.Location = new Point(296, 538);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(516, 129);
            groupBox3.TabIndex = 20;
            groupBox3.TabStop = false;
            groupBox3.Text = "groupBox3";
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(panel1);
            groupBox4.Controls.Add(listBox1);
            groupBox4.Dock = DockStyle.Fill;
            groupBox4.Location = new Point(296, 0);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(516, 538);
            groupBox4.TabIndex = 21;
            groupBox4.TabStop = false;
            groupBox4.Text = "groupBox4";
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(71, 22);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(334, 184);
            listBox1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Location = new Point(0, 241);
            panel1.Name = "panel1";
            panel1.Size = new Size(516, 291);
            panel1.TabIndex = 1;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1108, 667);
            Controls.Add(groupBox4);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Name = "Form1";
            Text = "Veri Aktarımı";
            Load += Form1_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)DataGrdvwKaynak).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)DataGrdvwHedef).EndInit();
            groupBox3.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Button BtnBaglantiTest;
        private Label LblKynkSunucu;
        private Label lblKynkVeri;
        private Label LblKynkTablo;
        private Label LblHdfSunucu;
        private Label LblHdfVeri;
        private Label LblHdfTablo;
        private TextBox textBox1;
        private ComboBox comboBox1;
        private ComboBox comboBox2;
        private TextBox textBox2;
        private ComboBox comboBox3;
        private ComboBox comboBox4;
        private Button BtnVeriAktarim;
        private Button button1;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Button BtnKynkKolonEslestir;
        private Button button3;
        private DataGridView DataGrdvwKaynak;
        private Label LblKynkSutun;
        private DataGridView DataGrdvwHedef;
        private Label LblHdfSutun;
        private GroupBox groupBox3;
        private GroupBox groupBox4;
        private Panel panel1;
        private ListBox listBox1;
    }
}
