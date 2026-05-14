using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WordGuessGame.Classes;

namespace WordGuessGame
{
    public partial class GameForm : Form
    {
        private WordDictionary dictionary;
        private GameLogic gameLogic;
        private GameSettings settings;
        private int currentBestScore;
        private string protocolPath = "Data/game_protocol.txt";

        // Элементы управления
        private Label lblStatus;
        private Label lblSecretWord;
        private Label lblAttempts;
        private Label lblWordLength;
        private Label lblTheme;
        private Label lblScore;
        private Label lblBestScore;
        private Label lblResult;
        private Label lblResultValue;
        private TextBox txtWordInput;
        private Button btnCheck;
        private Button btnNewGame;
        private Button btnClearHistory;
        private Button btnBackToMenu;
        private Button btnHint;
        private ListBox lstHistory;

        public GameForm(GameSettings settings)
        {
            this.settings = settings;
            dictionary = new WordDictionary();
            gameLogic = new GameLogic();
            currentBestScore = SettingsManager.LoadBestScore();
            InitializeComponent();
            SettingsManager.ApplyTheme(this, settings);
            LoadDictionaryAndStartGame();
        }

        private void InitializeComponent()
        {
            this.Text = "Угадай слово - Игра";
            this.Size = new Size(750, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormClosing += GameForm_FormClosing;

            // Информационная панель
            lblStatus = new Label();
            lblStatus.Text = "Статус: Загрузка...";
            lblStatus.Location = new Point(20, 20);
            lblStatus.Size = new Size(200, 25);

            lblSecretWord = new Label();
            lblSecretWord.Text = "Загаданное слово: ???";
            lblSecretWord.Location = new Point(250, 20);
            lblSecretWord.Size = new Size(250, 25);
            lblSecretWord.Font = new Font("Consolas", 10, FontStyle.Bold);

            lblAttempts = new Label();
            lblAttempts.Text = "Попытки: 0";
            lblAttempts.Location = new Point(520, 20);
            lblAttempts.Size = new Size(120, 25);

            lblWordLength = new Label();
            lblWordLength.Text = "Длина слова: ?";
            lblWordLength.Location = new Point(20, 55);
            lblWordLength.Size = new Size(150, 25);

            lblTheme = new Label();
            lblTheme.Text = "Тема: ?";
            lblTheme.Location = new Point(250, 55);
            lblTheme.Size = new Size(200, 25);

            lblScore = new Label();
            lblScore.Text = "Очки: 0";
            lblScore.Location = new Point(520, 55);
            lblScore.Size = new Size(150, 25);
            lblScore.Font = new Font("Arial", 10, FontStyle.Bold);

            lblBestScore = new Label();
            lblBestScore.Text = "Рекорд: " + currentBestScore;
            lblBestScore.Location = new Point(600, 20);
            lblBestScore.Size = new Size(130, 25);
            lblBestScore.Font = new Font("Arial", 10, FontStyle.Bold);

            // Игровая область
            Label lblPrompt = new Label();
            lblPrompt.Text = "Введите слово из словаря:";
            lblPrompt.Location = new Point(20, 100);
            lblPrompt.Size = new Size(200, 25);

            txtWordInput = new TextBox();
            txtWordInput.Location = new Point(20, 130);
            txtWordInput.Size = new Size(200, 25);
            txtWordInput.MaxLength = 30;
            txtWordInput.KeyPress += TxtWordInput_KeyPress;
            txtWordInput.TextChanged += TxtWordInput_TextChanged;

            btnCheck = new Button();
            btnCheck.Text = "Проверить";
            btnCheck.Location = new Point(230, 128);
            btnCheck.Size = new Size(100, 30);
            btnCheck.Click += BtnCheck_Click;

            btnHint = new Button();
            btnHint.Text = "Подсказка (-5 очков)";
            btnHint.Location = new Point(340, 128);
            btnHint.Size = new Size(180, 30);
            btnHint.Click += BtnHint_Click;

            lblResult = new Label();
            lblResult.Text = "Результат:";
            lblResult.Location = new Point(20, 170);
            lblResult.Size = new Size(80, 25);

            lblResultValue = new Label();
            lblResultValue.Text = "—";
            lblResultValue.Location = new Point(100, 170);
            lblResultValue.Size = new Size(100, 25);
            lblResultValue.Font = new Font("Arial", 12, FontStyle.Bold);

            // История ходов
            Label lblHistory = new Label();
            lblHistory.Text = "История ходов:";
            lblHistory.Location = new Point(20, 210);
            lblHistory.Size = new Size(150, 25);

            lstHistory = new ListBox();
            lstHistory.Location = new Point(20, 240);
            lstHistory.Size = new Size(690, 280);
            lstHistory.Font = new Font("Consolas", 10);

            // Панель управления
            btnNewGame = new Button();
            btnNewGame.Text = "Новая игра";
            btnNewGame.Location = new Point(20, 540);
            btnNewGame.Size = new Size(120, 35);
            btnNewGame.Click += BtnNewGame_Click;

            btnClearHistory = new Button();
            btnClearHistory.Text = "Очистить историю";
            btnClearHistory.Location = new Point(150, 540);
            btnClearHistory.Size = new Size(120, 35);
            btnClearHistory.Click += BtnClearHistory_Click;

            btnBackToMenu = new Button();
            btnBackToMenu.Text = "Вернуться в меню";
            btnBackToMenu.Location = new Point(590, 540);
            btnBackToMenu.Size = new Size(120, 35);
            btnBackToMenu.Click += BtnBackToMenu_Click;

            this.Controls.AddRange(new Control[] {
                lblStatus, lblSecretWord, lblAttempts, lblWordLength, lblTheme,
                lblScore, lblBestScore, lblPrompt, txtWordInput, btnCheck, btnHint,
                lblResult, lblResultValue, lblHistory, lstHistory,
                btnNewGame, btnClearHistory, btnBackToMenu
            });
        }

        private void LoadDictionaryAndStartGame()
        {
            bool loaded = dictionary.LoadDictionary();
            if (!loaded || dictionary.Count == 0)
            {
                MessageBox.Show("Не удалось загрузить словарь. Игра недоступна.\n\n" +
                    "Убедитесь, что файл Data/words.txt существует и имеет формат:\n" +
                    "слово|тема (каждое слово на отдельной строке)\n\n" +
                    "Пример:\nкот|животные\nсобака|животные\nкомпьютер|техника",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                txtWordInput.Enabled = false;
                btnCheck.Enabled = false;
                btnHint.Enabled = false;
                btnNewGame.Enabled = false;
                lblStatus.Text = "Статус: Ошибка загрузки словаря";
                lblStatus.ForeColor = Color.Red;
                return;
            }

            StartNewGame();
        }

        private void StartNewGame()
        {
            if (!dictionary.IsLoaded) return;

            int minLength, maxLength;
            SettingsManager.GetWordLengthByDifficulty(settings.Difficulty, out minLength, out maxLength);

            WordEntry secretEntry = dictionary.GetRandomWord(minLength, maxLength);

            if (secretEntry == null)
            {
                MessageBox.Show("Нет слов подходящей длины для выбранной сложности.",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            gameLogic.StartNewGame(secretEntry.Word, secretEntry.Theme);

            // Обновление интерфейса
            UpdateHintDisplay();
            lblAttempts.Text = "Попытки: " + gameLogic.Attempts;
            lblScore.Text = "Очки: " + gameLogic.Score;
            UpdateScoreColor();
            lblResultValue.Text = "—";
            txtWordInput.Text = "";
            txtWordInput.Enabled = true;
            btnCheck.Enabled = true;
            btnHint.Enabled = true;
            lstHistory.Items.Clear();
            lblStatus.Text = "Статус: Игра активна";
            lblStatus.ForeColor = SystemColors.ControlText;
            UpdateBestScoreDisplay();

            WriteToProtocol("[НОВАЯ ИГРА] Загадано слово длиной " + secretEntry.Word.Length + " (тема: " + secretEntry.Theme + ")");
        }

        private void UpdateHintDisplay()
        {
            // Длина
            if (gameLogic.LengthRevealed)
                lblWordLength.Text = "Длина слова: " + gameLogic.SecretWordLength;
            else
                lblWordLength.Text = "Длина слова: ?";

            // Тема
            if (gameLogic.ThemeRevealed)
                lblTheme.Text = "Тема: " + gameLogic.SecretTheme;
            else
                lblTheme.Text = "Тема: ?";

            // Загаданное слово с открытыми буквами
            string revealedWord = gameLogic.GetRevealedWord();
            if (revealedWord.Contains('?'))
                lblSecretWord.Text = "Загаданное слово: " + revealedWord;
            else
                lblSecretWord.Text = "Загаданное слово: " + revealedWord + " (ВСЕ БУКВЫ ОТКРЫТЫ!)";
        }

        private void UpdateScoreColor()
        {
            if (gameLogic.Score < 0)
                lblScore.ForeColor = Color.Red;
            else if (gameLogic.Score > 0)
                lblScore.ForeColor = Color.Green;
            else
                lblScore.ForeColor = SystemColors.ControlText;
        }

        private void UpdateBestScoreDisplay()
        {
            currentBestScore = SettingsManager.LoadBestScore();
            lblBestScore.Text = "Рекорд: " + currentBestScore;

            if (gameLogic.Score > currentBestScore && currentBestScore > 0)
                lblBestScore.ForeColor = Color.Red;
            else
                lblBestScore.ForeColor = Color.Blue;
        }

        private void BtnCheck_Click(object sender, EventArgs e)
        {
            string userWord = txtWordInput.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(userWord))
            {
                MessageBox.Show("Введите слово!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!dictionary.ContainsWord(userWord))
            {
                lblStatus.Text = "Статус: Слово не найдено в словаре";
                lblStatus.ForeColor = Color.Red;
                return;
            }

            lblStatus.Text = "Статус: Игра активна";
            lblStatus.ForeColor = SystemColors.ControlText;

            int matches = gameLogic.CheckWord(userWord);

            lblResultValue.Text = matches.ToString();
            lblAttempts.Text = "Попытки: " + gameLogic.Attempts;
            lblScore.Text = "Очки: " + gameLogic.Score;
            UpdateScoreColor();

            // Обновление истории
            lstHistory.Items.Clear();
            foreach (string entry in gameLogic.History)
            {
                lstHistory.Items.Add(entry);
            }
            lstHistory.TopIndex = lstHistory.Items.Count - 1;

            // Проверка рекорда
            if (gameLogic.Score > currentBestScore)
            {
                currentBestScore = gameLogic.Score;
                SettingsManager.SaveBestScore(currentBestScore);
                UpdateBestScoreDisplay();
                MessageBox.Show("Новый рекорд! " + currentBestScore + " очков!", "Поздравляем!",
                    MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }

            txtWordInput.Text = "";
            WriteToProtocol("Ввод: " + userWord + " | Совпадений: " + matches + " | Очки: " + gameLogic.Score);
        }

        private void BtnHint_Click(object sender, EventArgs e)
        {
            if (!gameLogic.CanUseHint()) return;

            // Предупреждение если очков мало (но всё равно можно использовать)
            if (gameLogic.Score < 5 && gameLogic.Score > -50)
            {
                DialogResult result = MessageBox.Show(
                    "У вас " + gameLogic.Score + " очков. Подсказка стоит 5 очков.\n" +
                    "После использования очки станут " + (gameLogic.Score - 5) + ".\nПродолжить?",
                    "Подтверждение",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (result != DialogResult.Yes)
                    return;
            }

            string hintResult = gameLogic.UseHint();

            lblScore.Text = "Очки: " + gameLogic.Score;
            UpdateScoreColor();
            UpdateHintDisplay();

            // Обновление истории
            lstHistory.Items.Clear();
            foreach (string entry in gameLogic.History)
            {
                lstHistory.Items.Add(entry);
            }
            lstHistory.TopIndex = lstHistory.Items.Count - 1;

            // Определяем следующую подсказку для сообщения
            string nextHint = "";
            if (!gameLogic.LengthRevealed)
                nextHint = "\n\nСледующая подсказка: длина слова.";
            else if (!gameLogic.ThemeRevealed)
                nextHint = "\n\nСледующая подсказка: тема слова.";
            else if (gameLogic.RevealedLettersCount < gameLogic.SecretWordLength)
                nextHint = "\n\nСледующая подсказка: следующая буква (" + (gameLogic.RevealedLettersCount + 1) + "-я).";
            else
                nextHint = "\n\nВсе подсказки использованы (слово полностью открыто).";

            MessageBox.Show("Подсказка: " + hintResult + "\nШтраф: -5 очков\nТекущие очки: " + gameLogic.Score + nextHint,
                "Подсказка", MessageBoxButtons.OK,
                gameLogic.Score < 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);

            WriteToProtocol("[ПОДСКАЗКА] " + hintResult + " | Очки: " + gameLogic.Score);
        }

        private void BtnNewGame_Click(object sender, EventArgs e)
        {
            StartNewGame();
        }

        private void BtnClearHistory_Click(object sender, EventArgs e)
        {
            lstHistory.Items.Clear();
            gameLogic.History.Clear();
        }

        private void BtnBackToMenu_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TxtWordInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            char[] russianLetters = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя".ToCharArray();

            if (e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Enter)
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    btnCheck.PerformClick();
                }
                return;
            }

            if (!char.IsLetter(e.KeyChar) || Array.IndexOf(russianLetters, char.ToLower(e.KeyChar)) == -1)
            {
                e.Handled = true;
            }
        }

        private void TxtWordInput_TextChanged(object sender, EventArgs e)
        {
            txtWordInput.Text = txtWordInput.Text.ToLower();
            txtWordInput.Select(txtWordInput.Text.Length, 0);
        }

        private void WriteToProtocol(string message)
        {
            try
            {
                string directory = Path.GetDirectoryName(protocolPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                string logEntry = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " | " + message;
                File.AppendAllText(protocolPath, logEntry + Environment.NewLine);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Ошибка записи протокола: " + ex.Message);
            }
        }

        private void GameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            WriteToProtocol("[ЗАВЕРШЕНИЕ ИГРЫ] Итоговый счёт: " + gameLogic.Score);
        }
    }
}