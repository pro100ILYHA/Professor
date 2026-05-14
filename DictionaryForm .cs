using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CollectWordGame
{
    public class DictionaryForm : Form
    {
        private DictionaryManager dictionary;
        private TabControl tabControl;
        private TabPage tabAdd, tabView, tabDelete;

        public DictionaryForm()
        {
            dictionary = new DictionaryManager();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Управление словарём";
            this.Size = new Size(500, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            tabControl = new TabControl { Location = new Point(10, 10), Size = new Size(460, 330) };

            tabAdd = new TabPage("Добавить слово");
            tabView = new TabPage("Просмотр слов");
            tabDelete = new TabPage("Удалить слово");

            SetupAddTab();
            SetupViewTab();
            SetupDeleteTab();

            tabControl.TabPages.Add(tabAdd);
            tabControl.TabPages.Add(tabView);
            tabControl.TabPages.Add(tabDelete);

            Controls.Add(tabControl);
        }

        private void SetupAddTab()
        {
            ComboBox cmbTopics = new ComboBox { Location = new Point(20, 30), Size = new Size(200, 24) };
            cmbTopics.DataSource = dictionary.GetAllTopics();

            TextBox txtWord = new TextBox { Location = new Point(20, 70), Size = new Size(200, 24) };
            txtWord.Text = "Новое слово";
            txtWord.ForeColor = System.Drawing.Color.Gray;
            txtWord.Enter += (s, e) => {
                if (txtWord.Text == "Новое слово")
                {
                    txtWord.Text = "";
                    txtWord.ForeColor = System.Drawing.Color.Black;
                }
            };
            txtWord.Leave += (s, e) => {
                if (string.IsNullOrWhiteSpace(txtWord.Text))
                {
                    txtWord.Text = "Новое слово";
                    txtWord.ForeColor = System.Drawing.Color.Gray;
                }
            };
            Button btnAdd = new Button { Text = "Добавить", Location = new Point(240, 70), Size = new Size(100, 30) };
            btnAdd.Click += (s, e) =>
            {
                if (cmbTopics.SelectedItem != null && !string.IsNullOrWhiteSpace(txtWord.Text))
                {
                    dictionary.AddWord(cmbTopics.SelectedItem.ToString(), txtWord.Text);
                    MessageBox.Show("Слово добавлено.", "Информация");
                    txtWord.Clear();
                }
            };

            tabAdd.Controls.AddRange(new Control[] { new Label { Text = "Тема:", Location = new Point(20, 10) }, cmbTopics,
                new Label { Text = "Слово:", Location = new Point(20, 50) }, txtWord, btnAdd });
        }

        private void SetupViewTab()
        {
            ComboBox cmbTopics = new ComboBox { Location = new Point(20, 30), Size = new Size(200, 24) };
            cmbTopics.DataSource = dictionary.GetAllTopics();
            ListBox lstWords = new ListBox { Location = new Point(20, 70), Size = new Size(200, 200), Font = new Font("Arial", 11) };

            cmbTopics.SelectedIndexChanged += (s, e) =>
            {
                lstWords.DataSource = dictionary.GetWordsByTopic(cmbTopics.SelectedItem.ToString());
            };
            if (cmbTopics.Items.Count > 0) cmbTopics.SelectedIndex = 0;

            tabView.Controls.AddRange(new Control[] { new Label { Text = "Тема:", Location = new Point(20, 10) }, cmbTopics, lstWords });
        }

        private void SetupDeleteTab()
        {
            ComboBox cmbTopics = new ComboBox { Location = new Point(20, 30), Size = new Size(200, 24) };
            cmbTopics.DataSource = dictionary.GetAllTopics();
            ListBox lstWords = new ListBox { Location = new Point(20, 70), Size = new Size(200, 150), Font = new Font("Arial", 11) };

            cmbTopics.SelectedIndexChanged += (s, e) =>
            {
                lstWords.DataSource = dictionary.GetWordsByTopic(cmbTopics.SelectedItem.ToString());
            };
            if (cmbTopics.Items.Count > 0) cmbTopics.SelectedIndex = 0;

            Button btnDelete = new Button { Text = "Удалить выбранное", Location = new Point(240, 100), Size = new Size(120, 35) };
            btnDelete.Click += (s, e) =>
            {
                if (lstWords.SelectedItem != null)
                {
                    string topic = cmbTopics.SelectedItem.ToString();
                    string word = lstWords.SelectedItem.ToString();
                    if (MessageBox.Show($"Удалить слово '{word}'?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        dictionary.RemoveWord(topic, word);
                        lstWords.DataSource = dictionary.GetWordsByTopic(topic);
                    }
                }
            };

            tabDelete.Controls.AddRange(new Control[] { new Label { Text = "Тема:", Location = new Point(20, 10) }, cmbTopics, lstWords, btnDelete });
        }
    }
}