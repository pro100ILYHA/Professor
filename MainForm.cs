using System;
using System.Drawing;
using System.Windows.Forms;

namespace CollectWordGame
{
    public class MainForm : Form
    {
        private Button btnPlay, btnDictionary, btnRecords, btnSettings, btnHelp, btnExit;
        private Label lblTitle;

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Собери слово";
            this.Size = new Size(500, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            lblTitle = new Label
            {
                Text = "СОБЕРИ СЛОВО",
                Font = new Font("Arial", 24, FontStyle.Bold),
                Location = new Point(100, 30),
                Size = new Size(300, 50),
                TextAlign = ContentAlignment.MiddleCenter
            };

            btnPlay = CreateButton("Играть", 100, new EventHandler(BtnPlay_Click));
            btnDictionary = CreateButton("Управление словарём", 160, BtnDictionary_Click);
            btnRecords = CreateButton("Рекорды", 220, BtnRecords_Click);
            btnSettings = CreateButton("Настройки", 280, BtnSettings_Click);
            btnHelp = CreateButton("Справка", 340, BtnHelp_Click);
            btnExit = CreateButton("Выход", 400, BtnExit_Click);

            Controls.Add(lblTitle);
            Controls.Add(btnPlay);
            Controls.Add(btnDictionary);
            Controls.Add(btnRecords);
            Controls.Add(btnSettings);
            Controls.Add(btnHelp);
            Controls.Add(btnExit);
        }

        private Button CreateButton(string text, int yPos, EventHandler click)
        {
            Button btn = new Button
            {
                Text = text,
                Location = new Point(150, yPos),
                Size = new Size(200, 40),
                Font = new Font("Arial", 11),
                BackColor = Color.LightSteelBlue,
                UseVisualStyleBackColor = false
            };
            btn.Click += click;   // <-- добавить эту строку
            return btn;
        }

        private void BtnPlay_Click(object sender, EventArgs e)
        {
            GameForm game = new GameForm();
            game.ShowDialog();
        }

        private void BtnDictionary_Click(object sender, EventArgs e)
        {
            DictionaryForm dict = new DictionaryForm();
            dict.ShowDialog();
        }

        private void BtnRecords_Click(object sender, EventArgs e)
        {
            RecordsForm rec = new RecordsForm();
            rec.ShowDialog();
        }

        private void BtnSettings_Click(object sender, EventArgs e)
        {
            SettingsForm sett = new SettingsForm();
            sett.ShowDialog();
        }

        private void BtnHelp_Click(object sender, EventArgs e)
        {
            HelpForm help = new HelpForm();
            help.ShowDialog();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}