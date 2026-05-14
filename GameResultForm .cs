using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace CollectWordGame
{
    public class GameResultForm : Form
    {
        private GameSession session;
        private RecordManager records;
        private SettingsManager settings;

        public GameResultForm(GameSession session, RecordManager records, SettingsManager settings)
        {
            this.session = session;
            this.records = records;
            this.settings = settings;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Итоги игры";
            this.Size = new Size(500, 450);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            Label lblTotal = new Label
            {
                Text = $"Итоговый счёт: {session.Score}",
                Location = new Point(20, 20),
                Font = new Font("Arial", 14, FontStyle.Bold),
                AutoSize = true
            };

            // Список слов с результатами
            ListBox lstResults = new ListBox
            {
                Location = new Point(20, 60),
                Size = new Size(440, 150),
                Font = new Font("Consolas", 10)
            };
            foreach (var res in session.RoundResults)
            {
                string status = res.Correct ? "✓" : "✗";
                lstResults.Items.Add($"{status} {res.Topic}: {res.Word} ({res.PointsEarned} очк.)");
            }

            int skipped = session.RoundResults.FindAll(r => !r.Correct).Count;
            Label lblSkipped = new Label
            {
                Text = $"Пропущено слов: {skipped}",
                Location = new Point(20, 220),
                Font = new Font("Arial", 10),
                AutoSize = true
            };

            // Проверка рекорда
            bool newRecord = records.IsNewRecord(session.Score);
            if (newRecord)
            {
                records.AddRecord(settings.PlayerName, session.Score);
                MessageBox.Show($"Новый рекорд! {session.Score} очков.", "Поздравляем!");
            }

            Button btnSave = new Button { Text = "Сохранить протокол", Location = new Point(20, 270), Size = new Size(130, 35) };
            btnSave.Click += BtnSave_Click;

            Button btnRestart = new Button { Text = "Начать заново", Location = new Point(170, 270), Size = new Size(130, 35) };
            btnRestart.Click += (s, e) => { this.DialogResult = DialogResult.Retry; this.Close(); };

            Button btnExit = new Button { Text = "Выйти в меню", Location = new Point(320, 270), Size = new Size(130, 35) };
            btnExit.Click += (s, e) => { this.DialogResult = DialogResult.OK; this.Close(); };

            Controls.AddRange(new Control[] { lblTotal, lstResults, lblSkipped, btnSave, btnRestart, btnExit });
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Текстовые файлы (*.txt)|*.txt",
                FileName = $"Протокол игры {DateTime.Now:yyyy-MM-dd HH-mm}.txt"
            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"Протокол игры от {DateTime.Now}");
                sb.AppendLine($"Игрок: {settings.PlayerName}");
                sb.AppendLine($"Итоговый счёт: {session.Score}");
                sb.AppendLine(new string('-', 30));
                foreach (var res in session.RoundResults)
                {
                    sb.AppendLine($"Раунд {res.Round}: {res.Topic} - {res.Word} {(res.Correct ? "угадано" : "пропущено")} (+{res.PointsEarned})");
                }
                File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                MessageBox.Show("Протокол сохранён.", "Информация");
            }
        }
    }
}