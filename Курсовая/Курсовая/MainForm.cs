using System;
using System.Drawing;
using System.Windows.Forms;
using WordGuessGame.Classes;

namespace WordGuessGame
{
    public partial class MainForm : Form
    {
        private GameSettings settings;

        public MainForm()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void InitializeComponent()
        {
            this.Text = "Угадай слово - Главное меню";
            this.Size = new Size(400, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormClosing += MainForm_FormClosing;

            Button btnStartGame = new Button();
            btnStartGame.Text = "Начать игру";
            btnStartGame.Size = new Size(200, 40);
            btnStartGame.Location = new Point(85, 60);
            btnStartGame.Click += BtnStartGame_Click;

            Button btnDictionary = new Button();
            btnDictionary.Text = "Работа со словарём";
            btnDictionary.Size = new Size(200, 40);
            btnDictionary.Location = new Point(85, 120);
            btnDictionary.Click += BtnDictionary_Click;

            Button btnSettings = new Button();
            btnSettings.Text = "Настройки";
            btnSettings.Size = new Size(200, 40);
            btnSettings.Location = new Point(85, 180);
            btnSettings.Click += BtnSettings_Click;

            Button btnHelp = new Button();
            btnHelp.Text = "Справка";
            btnHelp.Size = new Size(200, 40);
            btnHelp.Location = new Point(85, 240);
            btnHelp.Click += BtnHelp_Click;

            Button btnExit = new Button();
            btnExit.Text = "Выход";
            btnExit.Size = new Size(200, 40);
            btnExit.Location = new Point(85, 300);
            btnExit.Click += BtnExit_Click;

            this.Controls.AddRange(new Control[] { btnStartGame, btnDictionary, btnSettings, btnHelp, btnExit });
        }

        private void LoadSettings()
        {
            settings = SettingsManager.LoadSettings();
            SettingsManager.ApplyTheme(this, settings);
        }

        private void BtnStartGame_Click(object sender, EventArgs e)
        {
            GameForm gameForm = new GameForm(settings);
            gameForm.ShowDialog();
            settings = SettingsManager.LoadSettings();
            SettingsManager.ApplyTheme(this, settings);
        }

        private void BtnDictionary_Click(object sender, EventArgs e)
        {
            DictionaryForm dictForm = new DictionaryForm(settings);
            dictForm.ShowDialog();
            SettingsManager.ApplyTheme(this, settings);
        }

        private void BtnSettings_Click(object sender, EventArgs e)
        {
            SettingsForm settingsForm = new SettingsForm(settings);
            settingsForm.ShowDialog();
            settings = SettingsManager.LoadSettings();
            SettingsManager.ApplyTheme(this, settings);
        }

        private void BtnHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Правила игры:\n\n" +
                "1. Компьютер загадывает слово из словаря (с учётом выбранной темы)\n" +
                "2. Вы вводите слово и нажимаете «Проверить»\n" +
                "3. Программа показывает, сколько букв из вашего слова есть в загаданном\n" +
                "4. За каждое совпадение начисляются очки\n\n" +
                "ПОДСКАЗКИ:\n" +
                "• 1-я подсказка: длина слова (штраф -5 очков)\n" +
                "• 2-я подсказка: тема слова (штраф -5 очков)\n" +
                "• 3-я подсказка: первая буква (штраф -5 очков)\n" +
                "• Очки могут уходить в минус!\n\n" +
                "Цель — набрать как можно больше очков и побить рекорд!\n\n" +
                "Версия 2.0\nРазработчик: Драмбович М.Н.",
                "Справка",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Вы уверены, что хотите выйти?",
                "Выход",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SettingsManager.SaveSettings(settings);
        }
    }
}