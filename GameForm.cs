using System;
using System.Drawing;
using System.Windows.Forms;

namespace CollectWordGame
{
    public class GameForm : Form
    {
        private Label lblTopic, lblAnagram, lblScore, lblRound, lblTimer;
        private TextBox txtAnswer;
        private Button btnCheck, btnHint, btnSkip, btnExit;
        private Timer gameTimer;
        private GameSession session;
        private DictionaryManager dictionary;
        private SettingsManager settings;
        private RecordManager records;

        public GameForm()
        {
            InitializeComponent();
            dictionary = new DictionaryManager();
            settings = new SettingsManager();
            records = new RecordManager();
            session = new GameSession(dictionary, settings);
            session.StartNewSession();
            LoadNextWord();
        }

        private void InitializeComponent()
        {
            this.Text = "Игра - Собери слово";
            this.Size = new Size(600, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            lblTopic = new Label { Location = new Point(30, 20), Font = new Font("Arial", 14, FontStyle.Bold), Size = new Size(300, 30), Text = "Тема:" };
            lblAnagram = new Label { Location = new Point(30, 60), Font = new Font("Consolas", 24, FontStyle.Bold), Size = new Size(350, 50), Text = "АНАГРАММА" };
            lblScore = new Label { Location = new Point(450, 20), Font = new Font("Arial", 12), Size = new Size(120, 25), Text = "Очки: 0" };
            lblRound = new Label { Location = new Point(450, 50), Font = new Font("Arial", 12), Size = new Size(120, 25), Text = "Раунд: 0/10" };
            lblTimer = new Label { Location = new Point(30, 250), Font = new Font("Arial", 12), Size = new Size(150, 25), Text = "Время: 30" };

            txtAnswer = new TextBox { Location = new Point(30, 130), Font = new Font("Arial", 14), Size = new Size(200, 30) };
            btnCheck = new Button { Text = "Проверить", Location = new Point(250, 130), Size = new Size(100, 30) };
            btnCheck.Click += BtnCheck_Click;

            btnHint = new Button { Text = "Подсказка", Location = new Point(30, 190), Size = new Size(100, 35) };
            btnHint.Click += BtnHint_Click;
            btnSkip = new Button { Text = "Пропустить", Location = new Point(150, 190), Size = new Size(100, 35) };
            btnSkip.Click += BtnSkip_Click;
            btnExit = new Button { Text = "Выйти в меню", Location = new Point(270, 190), Size = new Size(120, 35) };
            btnExit.Click += (s, e) => { gameTimer?.Stop(); this.Close(); };

            gameTimer = new Timer { Interval = 1000 };
            gameTimer.Tick += GameTimer_Tick;

            Controls.AddRange(new Control[] { lblTopic, lblAnagram, lblScore, lblRound, lblTimer,
                txtAnswer, btnCheck, btnHint, btnSkip, btnExit });
        }

        private void LoadNextWord()
        {
            if (!session.NextRound())
            {
                EndGame();
                return;
            }

            lblTopic.Text = "Тема: " + session.CurrentTopic;
            lblAnagram.Text = session.CurrentAnagram;
            lblRound.Text = $"Раунд: {session.CurrentRound}/{session.GetTotalRounds()}";
            lblScore.Text = $"Очки: {session.Score}";
            txtAnswer.Clear();
            session.TimeLeft = settings.TimerSeconds;
            lblTimer.Text = "Время: " + session.TimeLeft;
            gameTimer.Start();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            session.TimeLeft--;
            lblTimer.Text = "Время: " + session.TimeLeft;
            if (session.TimeLeft <= 0)
            {
                gameTimer.Stop();
                session.SkipRound();
                MessageBox.Show("Время истекло!", "Информация");
                LoadNextWord();
            }
        }

        private void BtnCheck_Click(object sender, EventArgs e)
        {
            gameTimer.Stop();
            if (session.CheckAnswer(txtAnswer.Text))
            {
                PlayCorrectSound();
                RoundResultForm resultForm = new RoundResultForm(session.CurrentWord, session.Score);
                resultForm.ShowDialog();

                // Обновляем счёт на форме
                lblScore.Text = $"Очки: {session.Score}";
                LoadNextWord();
            }
            else
            {
                MessageBox.Show("Неправильно! Попробуйте ещё раз.", "Ошибка");
                gameTimer.Start(); // продолжить таймер
                txtAnswer.Focus();
            }
        }

        private void BtnHint_Click(object sender, EventArgs e)
        {
            char? hint = session.UseHint();
            if (hint != null)
            {
                txtAnswer.Text = hint.ToString();
                txtAnswer.Focus();
                txtAnswer.SelectionStart = txtAnswer.Text.Length;
                MessageBox.Show($"Подсказка: первая буква '{hint}'. Очки за раунд снижены вдвое.");
            }
            else
            {
                MessageBox.Show("Подсказки закончились.");
            }
        }

        private void BtnSkip_Click(object sender, EventArgs e)
        {
            gameTimer.Stop();
            session.SkipRound();
            LoadNextWord();
        }

        private void EndGame()
        {
            gameTimer.Stop();
            GameResultForm resultForm = new GameResultForm(session, records, settings);
            resultForm.ShowDialog();
            this.Close();
        }

        private void PlayCorrectSound()
        {
            try
            {
                // Простой звуковой сигнал (или загрузите свой .wav)
                System.Media.SystemSounds.Beep.Play();
            }
            catch { /* игнорируем ошибки звука */ }
        }
    }
}