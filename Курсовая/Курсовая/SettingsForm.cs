using System;
using System.Drawing;
using System.Windows.Forms;
using WordGuessGame.Classes;

namespace WordGuessGame
{
    public partial class SettingsForm : Form
    {
        private GameSettings settings;
        private ComboBox cboTheme;
        private RadioButton rbEasy;
        private RadioButton rbMedium;
        private RadioButton rbHard;
        private Button btnSave;
        private Button btnCancel;
        private Button btnResetRecord;

        public SettingsForm(GameSettings settings)
        {
            this.settings = settings;
            InitializeComponent();
            LoadCurrentSettings();
        }

        private void InitializeComponent()
        {
            this.Text = "Настройки";
            this.Size = new Size(350, 350);
            this.StartPosition = FormStartPosition.CenterScreen;

            Label lblTheme = new Label();
            lblTheme.Text = "Цветовая тема:";
            lblTheme.Location = new Point(20, 20);
            lblTheme.Size = new Size(120, 25);

            cboTheme = new ComboBox();
            cboTheme.Location = new Point(150, 18);
            cboTheme.Size = new Size(150, 25);
            cboTheme.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTheme.Items.AddRange(new string[] { "Светлая", "Тёмная" });

            GroupBox grpDifficulty = new GroupBox();
            grpDifficulty.Text = "Сложность (длина слова)";
            grpDifficulty.Location = new Point(20, 60);
            grpDifficulty.Size = new Size(280, 100);

            rbEasy = new RadioButton();
            rbEasy.Text = "Лёгкая (3-5 букв)";
            rbEasy.Location = new Point(10, 25);
            rbEasy.Size = new Size(150, 25);

            rbMedium = new RadioButton();
            rbMedium.Text = "Средняя (4-7 букв)";
            rbMedium.Location = new Point(10, 50);
            rbMedium.Size = new Size(150, 25);

            rbHard = new RadioButton();
            rbHard.Text = "Сложная (6-10 букв)";
            rbHard.Location = new Point(10, 75);
            rbHard.Size = new Size(150, 25);

            grpDifficulty.Controls.AddRange(new Control[] { rbEasy, rbMedium, rbHard });

            btnResetRecord = new Button();
            btnResetRecord.Text = "Сбросить рекорд";
            btnResetRecord.Location = new Point(20, 180);
            btnResetRecord.Size = new Size(150, 35);
            btnResetRecord.Click += BtnResetRecord_Click;

            btnSave = new Button();
            btnSave.Text = "Сохранить";
            btnSave.Location = new Point(50, 240);
            btnSave.Size = new Size(100, 35);
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button();
            btnCancel.Text = "Отмена";
            btnCancel.Location = new Point(180, 240);
            btnCancel.Size = new Size(100, 35);
            btnCancel.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] { lblTheme, cboTheme, grpDifficulty, btnResetRecord, btnSave, btnCancel });
        }

        private void LoadCurrentSettings()
        {
            cboTheme.SelectedItem = settings.Theme;

            if (settings.Difficulty == Difficulty.Easy)
                rbEasy.Checked = true;
            else if (settings.Difficulty == Difficulty.Medium)
                rbMedium.Checked = true;
            else
                rbHard.Checked = true;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (cboTheme.SelectedItem != null)
                settings.Theme = cboTheme.SelectedItem.ToString();

            if (rbEasy.Checked)
                settings.Difficulty = Difficulty.Easy;
            else if (rbMedium.Checked)
                settings.Difficulty = Difficulty.Medium;
            else
                settings.Difficulty = Difficulty.Hard;

            SettingsManager.SaveSettings(settings);
            MessageBox.Show("Настройки сохранены.", "Успех",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void BtnResetRecord_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Вы уверены, что хотите сбросить рекорд?",
                "Сброс рекорда",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                SettingsManager.SaveBestScore(0);
                MessageBox.Show("Рекорд сброшен.", "Сброс выполнен",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}