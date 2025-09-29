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
            GroupBox GrbBoxSutunlar;
            LblHdfSutun = new Label();
            LblKynkSutun = new Label();
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
            dataGridView1 = new DataGridView();
            dataGridView2 = new DataGridView();
            groupBox1 = new GroupBox();
            GrbBoxSutunlar = new GroupBox();
            GrbBoxSutunlar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // GrbBoxSutunlar
            // 
            GrbBoxSutunlar.Controls.Add(dataGridView2);
            GrbBoxSutunlar.Controls.Add(dataGridView1);
            GrbBoxSutunlar.Controls.Add(LblHdfSutun);
            GrbBoxSutunlar.Controls.Add(LblKynkSutun);
            GrbBoxSutunlar.Location = new Point(55, 312);
            GrbBoxSutunlar.Name = "GrbBoxSutunlar";
            GrbBoxSutunlar.Size = new Size(883, 306);
            GrbBoxSutunlar.TabIndex = 13;
            GrbBoxSutunlar.TabStop = false;
            GrbBoxSutunlar.Text = "Tablolar";
            GrbBoxSutunlar.Enter += GrbBoxSutunlar_Enter;
            // 
            // LblHdfSutun
            // 
            LblHdfSutun.AutoSize = true;
            LblHdfSutun.Location = new Point(491, 37);
            LblHdfSutun.Name = "LblHdfSutun";
            LblHdfSutun.Size = new Size(124, 15);
            LblHdfSutun.TabIndex = 1;
            LblHdfSutun.Text = "Hedef Tablo Sütünları:";
            // 
            // LblKynkSutun
            // 
            LblKynkSutun.AutoSize = true;
            LblKynkSutun.Location = new Point(32, 31);
            LblKynkSutun.Name = "LblKynkSutun";
            LblKynkSutun.Size = new Size(130, 15);
            LblKynkSutun.TabIndex = 0;
            LblKynkSutun.Text = "Kaynak Tablo Sütünları:";
            // 
            // BtnBaglantiTest
            // 
            BtnBaglantiTest.Location = new Point(866, 55);
            BtnBaglantiTest.Name = "BtnBaglantiTest";
            BtnBaglantiTest.Size = new Size(121, 23);
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
            LblKynkSunucu.Size = new Size(114, 20);
            LblKynkSunucu.TabIndex = 1;
            LblKynkSunucu.Text = "Kaynak Sunucu :";
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
            LblHdfSunucu.Location = new Point(451, 56);
            LblHdfSunucu.Name = "LblHdfSunucu";
            LblHdfSunucu.Size = new Size(108, 20);
            LblHdfSunucu.TabIndex = 4;
            LblHdfSunucu.Text = "Hedef Sunucu :";
            // 
            // LblHdfVeri
            // 
            LblHdfVeri.AutoSize = true;
            LblHdfVeri.Font = new Font("Segoe UI", 11.25F);
            LblHdfVeri.Location = new Point(453, 103);
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
            LblHdfTablo.Location = new Point(453, 149);
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
            textBox2.Location = new Point(565, 52);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(129, 23);
            textBox2.TabIndex = 10;
            // 
            // comboBox3
            // 
            comboBox3.FormattingEnabled = true;
            comboBox3.Location = new Point(566, 100);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new Size(128, 23);
            comboBox3.TabIndex = 11;
            // 
            // comboBox4
            // 
            comboBox4.FormattingEnabled = true;
            comboBox4.Location = new Point(566, 146);
            comboBox4.Name = "comboBox4";
            comboBox4.Size = new Size(128, 23);
            comboBox4.TabIndex = 12;
            // 
            // BtnVeriAktarim
            // 
            BtnVeriAktarim.Location = new Point(866, 169);
            BtnVeriAktarim.Name = "BtnVeriAktarim";
            BtnVeriAktarim.Size = new Size(121, 42);
            BtnVeriAktarim.TabIndex = 14;
            BtnVeriAktarim.Text = "Tablo Eşleşmesini Doğrula";
            BtnVeriAktarim.UseVisualStyleBackColor = true;
            BtnVeriAktarim.Click += BtnVeriAktarim_Click;
            // 
            // button1
            // 
            button1.Location = new Point(866, 100);
            button1.Name = "button1";
            button1.Size = new Size(118, 45);
            button1.TabIndex = 15;
            button1.Text = "Veri Transferini Başlat";
            button1.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(35, 70);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(240, 150);
            dataGridView1.TabIndex = 2;
            // 
            // dataGridView2
            // 
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Location = new Point(494, 68);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.Size = new Size(240, 150);
            dataGridView2.TabIndex = 3;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(comboBox4);
            groupBox1.Controls.Add(LblKynkSunucu);
            groupBox1.Controls.Add(lblKynkVeri);
            groupBox1.Controls.Add(LblKynkTablo);
            groupBox1.Controls.Add(LblHdfSunucu);
            groupBox1.Controls.Add(comboBox3);
            groupBox1.Controls.Add(LblHdfVeri);
            groupBox1.Controls.Add(textBox2);
            groupBox1.Controls.Add(LblHdfTablo);
            groupBox1.Controls.Add(comboBox2);
            groupBox1.Controls.Add(textBox1);
            groupBox1.Controls.Add(comboBox1);
            groupBox1.Location = new Point(55, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(731, 261);
            groupBox1.TabIndex = 16;
            groupBox1.TabStop = false;
            groupBox1.Text = "groupBox1";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1047, 616);
            Controls.Add(groupBox1);
            Controls.Add(button1);
            Controls.Add(BtnVeriAktarim);
            Controls.Add(GrbBoxSutunlar);
            Controls.Add(BtnBaglantiTest);
            Name = "Form1";
            Text = "Veri Aktarımı";
            Load += Form1_Load;
            GrbBoxSutunlar.ResumeLayout(false);
            GrbBoxSutunlar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
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
        private GroupBox GrbBoxSutunlar;
        private Label LblHdfSutun;
        private Label LblKynkSutun;
        private Button BtnVeriAktarim;
        private Button button1;
        private DataGridView dataGridView2;
        private DataGridView dataGridView1;
        private GroupBox groupBox1;
    }
}
