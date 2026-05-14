using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CollectWordGame
{
    public class SettingsForm : Form
    {
        private SettingsManager settings;

        public SettingsForm()
        {
            settings = new SettingsManager();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Настройки";
            this.Size = new Size(350, 280);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            TextBox txtName = new TextBox
            {
                Location = new Point(120, 20),
                Size = new Size(150, 24),
                Text = settings.PlayerName
            };

            ComboBox cmbTime = new ComboBox
            {
                Location = new Point(120, 60),
                Size = new Size(100, 24),
                DataSource = settings.AllowedTimes.ToList()
            };
            cmbTime.SelectedItem = settings.TimerSeconds;

            // Метка с описанием очков и сложности
            Label lblPointsInfo = new Label
            {
                Location = new Point(120, 100),
                Size = new Size(200, 45),          // увеличена высота для двух строк
                Font = new Font("Arial", 9),
                Text = GetPointsDescription(settings.TimerSeconds)
            };

            cmbTime.SelectedIndexChanged += (s, e) =>
            {
                if (cmbTime.SelectedItem != null)
                {
                    int time = (int)cmbTime.SelectedItem;
                    lblPointsInfo.Text = GetPointsDescription(time);
                }
            };

            Button btnSave = new Button { Text = "Сохранить", Location = new Point(80, 170), Size = new Size(100, 30) };
            btnSave.Click += (s, e) =>
            {
                settings.PlayerName = txtName.Text.Trim();
                settings.TimerSeconds = (int)cmbTime.SelectedItem;
                settings.SaveSettings();
                MessageBox.Show("Настройки сохранены.", "Информация");
                this.Close();
            };

            Button btnCancel = new Button { Text = "Отмена", Location = new Point(190, 170), Size = new Size(100, 30) };
            btnCancel.Click += (s, e) => this.Close();

            Controls.AddRange(new Control[] {
                new Label { Text = "Имя игрока:", Location = new Point(20, 20), Size = new Size(90, 20) },
                txtName,
                new Label { Text = "Таймер (сек):", Location = new Point(20, 60), Size = new Size(90, 20) },
                cmbTime,
                lblPointsInfo,
                btnSave, btnCancel
            });
        }

        // Возвращает строку с очками и уровнем сложности
        private string GetPointsDescription(int seconds)
        {
            string points;
            string difficulty;
            switch (seconds)
            {
                case 20:
                    points = "100 очков за слово";
                    difficulty = "Сложность: Высокая";
                    break;
                case 30:
                    points = "80 очков за слово";
                    difficulty = "Сложность: Средняя";
                    break;
                case 60:
                    points = "60 очков за слово";
                    difficulty = "Сложность: Низкая";
                    break;
                case 90:
                    points = "50 очков за слово";
                    difficulty = "Сложность: Очень низкая";
                    break;
                default:
                    return "";
            }
            return points + "\r\n" + difficulty;   // две строки в одной метке
        }
    }
}