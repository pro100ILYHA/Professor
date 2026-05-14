using System;
using System.Drawing;
using System.Windows.Forms;

namespace CollectWordGame
{
    public class RoundResultForm : Form
    {
        public RoundResultForm(string word, int totalScore)
        {
            InitializeComponent(word, totalScore);
        }

        private void InitializeComponent(string word, int totalScore)
        {
            this.Text = "Результат раунда";
            this.Size = new Size(350, 250);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            Label lblCongrats = new Label
            {
                Text = "Поздравляем! Вы отгадали слово:",
                Location = new Point(30, 20),
                Font = new Font("Arial", 12),
                AutoSize = true
            };
            Label lblWord = new Label
            {
                Text = word,
                Location = new Point(30, 50),
                Font = new Font("Arial", 16, FontStyle.Bold),
                AutoSize = true
            };
            Label lblScore = new Label
            {
                Text = $"Общий счёт: {totalScore}",
                Location = new Point(30, 90),
                Font = new Font("Arial", 12),
                AutoSize = true
            };

            Button btnNext = new Button
            {
                Text = "Следующее слово",
                Location = new Point(30, 140),
                Size = new Size(120, 35),
                DialogResult = DialogResult.OK
            };
            Button btnExit = new Button
            {
                Text = "Выйти из игры",
                Location = new Point(170, 140),
                Size = new Size(120, 35),
                DialogResult = DialogResult.Cancel
            };

            Controls.AddRange(new Control[] { lblCongrats, lblWord, lblScore, btnNext, btnExit });

            btnNext.Click += (s, e) => this.Close();
            btnExit.Click += (s, e) => { this.DialogResult = DialogResult.Abort; this.Close(); };
        }
    }
}