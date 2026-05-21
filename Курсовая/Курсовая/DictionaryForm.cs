using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WordGuessGame.Classes;

namespace WordGuessGame
{
    public partial class DictionaryForm : Form
    {
        private WordDictionary dictionary;
        private GameSettings settings;
        private TabControl tabControl;
        private ListBox lstWords;
        private TextBox txtLetterFilter;
        private TextBox txtAddWord;
        private TextBox txtAddTheme;
        private TextBox txtRemoveWord;
        private Label lblWordCount;

        public DictionaryForm(GameSettings settings)
        {
            this.settings = settings;
            dictionary = new WordDictionary();
            InitializeComponent();
            SettingsManager.ApplyTheme(this, settings);

            dictionary.LoadDictionary();
            UpdateWordCount();
        }

        private void InitializeComponent()
        {
            this.Text = "Управление словарём";
            this.Size = new Size(550, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            tabControl = new TabControl();
            tabControl.Location = new Point(10, 10);
            tabControl.Size = new Size(515, 400);

            // Вкладка просмотра
            TabPage viewPage = new TabPage("Просмотр словаря");

            Label lblLetter = new Label();
            lblLetter.Text = "Начать с буквы:";
            lblLetter.Location = new Point(10, 20);
            lblLetter.Size = new Size(100, 25);

            txtLetterFilter = new TextBox();
            txtLetterFilter.Location = new Point(120, 18);
            txtLetterFilter.Size = new Size(50, 25);
            txtLetterFilter.MaxLength = 1;
            txtLetterFilter.TextChanged += TxtLetterFilter_TextChanged;

            Button btnShow = new Button();
            btnShow.Text = "Показать";
            btnShow.Location = new Point(180, 16);
            btnShow.Size = new Size(80, 30);
            btnShow.Click += BtnShow_Click;

            Button btnRefresh = new Button();
            btnRefresh.Text = "Обновить";
            btnRefresh.Location = new Point(270, 16);
            btnRefresh.Size = new Size(80, 30);
            btnRefresh.Click += BtnRefresh_Click;

            lstWords = new ListBox();
            lstWords.Location = new Point(10, 55);
            lstWords.Size = new Size(480, 260);
            lstWords.Font = new Font("Consolas", 10);

            lblWordCount = new Label();
            lblWordCount.Text = "Всего слов: 0";
            lblWordCount.Location = new Point(10, 325);
            lblWordCount.Size = new Size(200, 25);

            viewPage.Controls.AddRange(new Control[] { lblLetter, txtLetterFilter, btnShow, btnRefresh, lstWords, lblWordCount });

            // Вкладка добавления
            TabPage addPage = new TabPage("Добавление слова");

            Label lblNewWord = new Label();
            lblNewWord.Text = "Новое слово:";
            lblNewWord.Location = new Point(10, 30);
            lblNewWord.Size = new Size(100, 25);

            txtAddWord = new TextBox();
            txtAddWord.Location = new Point(120, 28);
            txtAddWord.Size = new Size(150, 25);
            txtAddWord.MaxLength = 30;

            Label lblTheme = new Label();
            lblTheme.Text = "Тема:";
            lblTheme.Location = new Point(10, 65);
            lblTheme.Size = new Size(100, 25);

            txtAddTheme = new TextBox();
            txtAddTheme.Location = new Point(120, 63);
            txtAddTheme.Size = new Size(150, 25);

            Button btnAdd = new Button();
            btnAdd.Text = "Добавить";
            btnAdd.Location = new Point(280, 45);
            btnAdd.Size = new Size(100, 30);
            btnAdd.Click += BtnAdd_Click;

            Label lblHint = new Label();
            lblHint.Text = "Слова сохранятся после нажатия «Сохранить изменения»";
            lblHint.Location = new Point(10, 100);
            lblHint.Size = new Size(400, 25);
            lblHint.ForeColor = Color.Gray;

            addPage.Controls.AddRange(new Control[] { lblNewWord, txtAddWord, lblTheme, txtAddTheme, btnAdd, lblHint });

            // Вкладка удаления
            TabPage removePage = new TabPage("Удаление слова");

            Label lblRemoveWord = new Label();
            lblRemoveWord.Text = "Удалить слово:";
            lblRemoveWord.Location = new Point(10, 30);
            lblRemoveWord.Size = new Size(100, 25);

            txtRemoveWord = new TextBox();
            txtRemoveWord.Location = new Point(120, 28);
            txtRemoveWord.Size = new Size(150, 25);
            txtRemoveWord.MaxLength = 30;

            Button btnRemove = new Button();
            btnRemove.Text = "Удалить";
            btnRemove.Location = new Point(280, 26);
            btnRemove.Size = new Size(100, 30);
            btnRemove.Click += BtnRemove_Click;

            removePage.Controls.AddRange(new Control[] { lblRemoveWord, txtRemoveWord, btnRemove });

            tabControl.TabPages.Add(viewPage);
            tabControl.TabPages.Add(addPage);
            tabControl.TabPages.Add(removePage);

            // Кнопки внизу
            Button btnSave = new Button();
            btnSave.Text = "Сохранить изменения";
            btnSave.Location = new Point(10, 420);
            btnSave.Size = new Size(150, 35);
            btnSave.Click += BtnSave_Click;

            Button btnClose = new Button();
            btnClose.Text = "Закрыть";
            btnClose.Location = new Point(170, 420);
            btnClose.Size = new Size(100, 35);
            btnClose.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] { tabControl, btnSave, btnClose });
        }

        private void UpdateWordCount()
        {
            lblWordCount.Text = "Всего слов: " + dictionary.Count;
        }

        private void BtnShow_Click(object sender, EventArgs e)
        {
            if (!dictionary.IsLoaded)
            {
                MessageBox.Show("Словарь не загружен. Нажмите «Обновить» для повторной загрузки.",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string filter = txtLetterFilter.Text.Trim();
            if (string.IsNullOrEmpty(filter))
            {
                MessageBox.Show("Введите букву для фильтрации!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            char letter = filter.ToLower()[0];
            var words = dictionary.GetWordsStartingWith(letter);

            lstWords.Items.Clear();
            foreach (WordEntry entry in words)
            {
                lstWords.Items.Add(entry.ToString());
            }

            if (words.Count == 0)
            {
                MessageBox.Show("Нет слов, начинающихся на букву «" + letter + "».",
                    "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            dictionary.LoadDictionary();
            UpdateWordCount();
            lstWords.Items.Clear();

            if (dictionary.IsLoaded)
            {
                MessageBox.Show("Словарь обновлён. Загружено " + dictionary.Count + " слов.",
                    "Обновление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            string newWord = txtAddWord.Text.Trim().ToLower();
            string theme = txtAddTheme.Text.Trim();

            if (string.IsNullOrEmpty(newWord))
            {
                MessageBox.Show("Введите слово для добавления!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(theme))
            {
                MessageBox.Show("Введите тему слова!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dictionary.AddWord(newWord, theme))
            {
                UpdateWordCount();
                MessageBox.Show("Слово «" + newWord + "» (тема: " + theme + ") добавлено в память.\nНе забудьте нажать «Сохранить изменения».",
                    "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtAddWord.Text = "";
                txtAddTheme.Text = "";
            }
            else
            {
                MessageBox.Show("Слово «" + newWord + "» уже есть в словаре.", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnRemove_Click(object sender, EventArgs e)
        {
            string removeWord = txtRemoveWord.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(removeWord))
            {
                MessageBox.Show("Введите слово для удаления!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dictionary.ContainsWord(removeWord))
            {
                string theme = dictionary.GetThemeOfWord(removeWord);
                DialogResult result = MessageBox.Show(
                    "Вы уверены, что хотите удалить слово «" + removeWord + "» (тема: " + theme + ")?",
                    "Подтверждение удаления",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    dictionary.RemoveWord(removeWord);
                    UpdateWordCount();
                    MessageBox.Show("Слово «" + removeWord + "» удалено из памяти.\nНе забудьте нажать «Сохранить изменения».",
                        "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtRemoveWord.Text = "";
                }
            }
            else
            {
                MessageBox.Show("Слово «" + removeWord + "» не найдено в словаре.", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            dictionary.SaveDictionary();
            MessageBox.Show("Изменения сохранены в файл.", "Сохранение",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void TxtLetterFilter_TextChanged(object sender, EventArgs e)
        {
            if (txtLetterFilter.Text.Length > 1)
            {
                txtLetterFilter.Text = txtLetterFilter.Text.Substring(0, 1);
                txtLetterFilter.Select(txtLetterFilter.Text.Length, 0);
            }
        }
    }
}