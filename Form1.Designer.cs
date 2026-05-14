namespace lr4
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        private void InitializeComponent()
        {
            this.button2 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.расчетToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сантиметрыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.дециметрыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.километрыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.метрыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.справкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(222, 188);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(91, 23);
            this.button2.TabIndex = 0;
            this.button2.Text = "Перевести";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(285, 130);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 26);
            this.textBox1.TabIndex = 4;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(141, 133);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(133, 19);
            this.label6.TabIndex = 5;
            this.label6.Text = "Введите значение:";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.расчетToolStripMenuItem,
            this.справкаToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(615, 24);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // расчетToolStripMenuItem
            // 
            this.расчетToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.сантиметрыToolStripMenuItem,
            this.дециметрыToolStripMenuItem,
            this.километрыToolStripMenuItem,
            this.метрыToolStripMenuItem});
            this.расчетToolStripMenuItem.Name = "расчетToolStripMenuItem";
            this.расчетToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.расчетToolStripMenuItem.Text = "Расчет";
            // 
            // сантиметрыToolStripMenuItem
            // 
            this.сантиметрыToolStripMenuItem.Name = "сантиметрыToolStripMenuItem";
            this.сантиметрыToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.сантиметрыToolStripMenuItem.Text = "Сантиметры (см³)";
            this.сантиметрыToolStripMenuItem.Click += new System.EventHandler(this.сантиметрыToolStripMenuItem_Click);
            // 
            // дециметрыToolStripMenuItem
            // 
            this.дециметрыToolStripMenuItem.Name = "дециметрыToolStripMenuItem";
            this.дециметрыToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.дециметрыToolStripMenuItem.Text = "Дециметры (дм³)";
            this.дециметрыToolStripMenuItem.Click += new System.EventHandler(this.дециметрыToolStripMenuItem_Click);
            // 
            // километрыToolStripMenuItem
            // 
            this.километрыToolStripMenuItem.Name = "километрыToolStripMenuItem";
            this.километрыToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.километрыToolStripMenuItem.Text = "Километры (км³)";
            this.километрыToolStripMenuItem.Click += new System.EventHandler(this.километрыToolStripMenuItem_Click);
            // 
            // метрыToolStripMenuItem
            // 
            this.метрыToolStripMenuItem.Name = "метрыToolStripMenuItem";
            this.метрыToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.метрыToolStripMenuItem.Text = "Метры (м³)";
            this.метрыToolStripMenuItem.Click += new System.EventHandler(this.метрыToolStripMenuItem_Click);
            // 
            // справкаToolStripMenuItem
            // 
            this.справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
            this.справкаToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.справкаToolStripMenuItem.Text = "Справка";
            this.справкаToolStripMenuItem.Click += new System.EventHandler(this.справкаToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButton3,
            this.toolStripButton4});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(615, 28);
            this.toolStrip1.TabIndex = 7;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.CheckOnClick = true;
            this.toolStripButton1.Font = new System.Drawing.Font("Ravie", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripButton1.ForeColor = System.Drawing.Color.DarkGreen;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(111, 25);
            this.toolStripButton1.Text = "Сантиметры";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.CheckOnClick = true;
            this.toolStripButton2.Font = new System.Drawing.Font("Ravie", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.toolStripButton2.ForeColor = System.Drawing.Color.Crimson;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(104, 25);
            this.toolStripButton2.Text = "Дециметры";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.CheckOnClick = true;
            this.toolStripButton3.Font = new System.Drawing.Font("Ravie", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.toolStripButton3.ForeColor = System.Drawing.Color.DarkCyan;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(66, 25);
            this.toolStripButton3.Text = "Метры";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.CheckOnClick = true;
            this.toolStripButton4.Font = new System.Drawing.Font("Ravie", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.toolStripButton4.ForeColor = System.Drawing.Color.DarkBlue;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(104, 25);
            this.toolStripButton4.Text = "Километры";
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(128, 251);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 19);
            this.label4.TabIndex = 8;
            this.label4.Text = "Результат:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(243, 251);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(25, 19);
            this.label5.TabIndex = 9;
            this.label5.Text = "—";
            // 
            // Form1
            // 
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(615, 328);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Перевод кубических единиц объёма";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripMenuItem расчетToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сантиметрыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem справкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem дециметрыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem километрыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem метрыToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}