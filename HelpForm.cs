using System;
using System.Drawing;
using System.Windows.Forms;

namespace CollectWordGame
{
    public class HelpForm : Form
    {
        public HelpForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Справка";
            this.Size = new Size(450, 350);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            TextBox txtHelp = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Location = new Point(10, 10),
                Size = new Size(410, 250),
                Font = new Font("Arial", 10),
                Text = "Правила игры «Собери слово»:\r\n\r\n" +
                       "- Вам показывается анаграмма (перемешанные буквы) английского слова.\r\n" +
                       "- Тема слова отображается сверху.\r\n" +
                       "- Введите правильное слово в поле и нажмите «Проверить».\r\n" +
                       "- За отгаданное слово начисляются очки (зависят от таймера).\r\n" +
                       "- Можно взять подсказку (первая буква), но очки уменьшатся вдвое.\r\n" +
                       "- Игра состоит из 10 раундов, слова не повторяются.\r\n" +
                       "- Управление словарём: можно добавлять, просматривать и удалять слова.\r\n" +
                       "\r\nРазработчик: Субботин И.А., группа 23290907/3091\r\n" +
                       "СПбПУ Петра Великого, 2026"
            };

            Button btnClose = new Button { Text = "Закрыть", Location = new Point(170, 270), Size = new Size(100, 30) };
            btnClose.Click += (s, e) => this.Close();

            Controls.Add(txtHelp);
            Controls.Add(btnClose);
        }
    }
}