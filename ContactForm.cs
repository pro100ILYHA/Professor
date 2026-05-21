using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Lab1
{
    public class ContactForm : Form
    {
        private Label nameLabel;
        private Label phoneNumberLabel;
        private Label searchLabel;
        private ContactManager contactManager;
        private TextBox nameTextBox;
        private TextBox phoneNumberTextBox;
        private Button addContactButton;
        private Button removeContactButton;
        private TextBox searchTextBox;
        private Button searchButton;
        private ListBox contactsListBox;
        private ComboBox groupComboBox;
        private ComboBox filterGroupComboBox;
        private TextBox newGroupTextBox;
        private Button addGroupButton;
        private Button removeGroupButton;
        private Label groupLabel;
        private Label groupTextBoxLabel;
        private Label filterGroupLabel;
        public ContactForm()
        {
            this.Text = "Управление контактами";
            this.Width = 700;
            this.Height = 450;
            nameLabel = new Label
            {
                Location = new System.Drawing.Point(10, 12),
                Width = 40,
                Text = "Имя:"
            };
            searchLabel = new Label
            {
                Location = new System.Drawing.Point(15, 74),
                Width = 45,
                Text = "Поиск:"
            };
            phoneNumberLabel = new Label
            {
                Location = new System.Drawing.Point(205, 12),
                Width = 55,
                Text = "Телефон:"
            };
            groupLabel = new Label
            {
                Text = "Группа", Location = new System.Drawing.Point(500, 10),
                Width = 50
            };
            groupComboBox = new ComboBox
            {
                Location = new Point(550, 10),
                Width = 100, 
                DropDownStyle = ComboBoxStyle.DropDownList,
            };
            newGroupTextBox = new TextBox
            {
                Location = new System.Drawing.Point(550, 40),
                Width = 120
            };
            groupTextBoxLabel = new Label
            {
                Location= new System.Drawing.Point(460, 40),
                Width = 80,
                Text = "Новая группа"
            };
            addGroupButton.Click += AddGroupButton_Click;
            addGroupButton = new Button
            {
                Location = new System.Drawing.Point(500, 80),
                Text = "Добавить",
                Width = 80
            };
            removeGroupButton = new Button
            {
                Location = new System.Drawing.Point(600, 80),
                Text = "Удалить",
                Width = 80
            };
            filterGroupLabel = new Label
            {
                Text = "Фильтр",
                Location = new System.Drawing.Point(500, 130)
            };
            filterGroupComboBox = new ComboBox
            {
                Location = new Point(550, 130)
            };
            nameTextBox = new TextBox
            {
                Location = new System.Drawing.Point(50, 10),
                Width = 150
            };
            phoneNumberTextBox = new TextBox
            {

                Location = new System.Drawing.Point(265, 10),
                Width = 150
            };
            phoneNumberTextBox.KeyPress += phoneNumberTextBox_KeyPress;
            addContactButton = new Button
            {
                Location = new System.Drawing.Point(10, 40),
                Text = "Добавить",
                Width = 100
            };
            addContactButton.Click += AddContactButton_Click;
            removeContactButton = new Button
            {
                Location = new System.Drawing.Point(120, 40),
                Text = "Удалить",
                Width = 100
            };
            removeContactButton.Click += RemoveContactButton_Click;
            searchTextBox = new TextBox
            {
                Location = new System.Drawing.Point(70, 72),
                Width = 200
            };
            searchButton = new Button
            {
                Location = new System.Drawing.Point(285, 70),
                Text = "Искать",
                Width = 80
            };
            searchButton.Click += SearchButton_Click;
            contactsListBox = new ListBox
            {
                Location = new System.Drawing.Point(10, 100),
                Width = 450,
                Height = 200
            };
            this.Controls.Add(filterGroupComboBox);
            this.Controls.Add(filterGroupLabel);
            this.Controls.Add(removeGroupButton);
            this.Controls.Add(addGroupButton);
            this.Controls.Add(groupTextBoxLabel);
            this.Controls.Add(newGroupTextBox);
            this.Controls.Add(groupComboBox);
            this.Controls.Add(searchLabel);
            this.Controls.Add(groupLabel);
            this.Controls.Add(phoneNumberLabel);
            this.Controls.Add(nameLabel);
            this.Controls.Add(nameTextBox);
            this.Controls.Add(phoneNumberTextBox);
            this.Controls.Add(addContactButton);
            this.Controls.Add(removeContactButton);
            this.Controls.Add(searchTextBox);
            this.Controls.Add(searchButton);
            this.Controls.Add(contactsListBox);
            contactManager = new ContactManager();
            UpdateContactsList();
        }
        private void phoneNumberTextBox_KeyPress(object sender, KeyPressEventArgs e)
        { 
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Convert.ToChar(8))
            {
                e.Handled = true;
            }
        }
        private void LoadGroupsIntoComboBoxes()
        {
            var groups = contactManager.GetAllGroups();
            var groupName = groups.Select(g => g.Name).ToList();
            groupComboBox.Items.Clear();
            groupComboBox.Items.AddRange(groupName.ToArray());
            if (groupComboBox.Items.Count > 0)
                groupComboBox.SelectedItem = "Без группы";
            filterGroupComboBox.Items.Clear();
            filterGroupComboBox.Items.AddRange(groupName.ToArray());
            filterGroupComboBox.SelectedItem = "Все контакты";
        }
        private void AddGroupButton_Click(object sender, EventArgs e)
        {
            try
            {
                string newGroupName = newGroupTextBox.Text.Trim();
                if (string.IsNullOrEmpty(newGroupName))
                {
                    MessageBox.Show("Введите название группы!", "Ошибка");
                }
            }
        }
        private void UpdateContactsList()
        {
            contactsListBox.Items.Clear();
            foreach (var contact in contactManager.Contacts)
            {
                contactsListBox.Items.Add($"{contact.Name} - {contact.PhoneNumber}");
            }
        }
        private void AddContactButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(nameTextBox.Text) ||
            string.IsNullOrEmpty(phoneNumberTextBox.Text))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }
            Contact newContact = new Contact(nameTextBox.Text, phoneNumberTextBox.Text);
            try
            {
                contactManager.AddContact(newContact);
                nameTextBox.Clear();
                phoneNumberTextBox.Clear();
                UpdateContactsList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void RemoveContactButton_Click(object sender, EventArgs e)
        {
            if (contactsListBox.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите контакт для удаления!");
                return;
            }
            string selectedItem = contactsListBox.SelectedItem.ToString();
            string[] parts = selectedItem.Split(new[] { '-' }, StringSplitOptions.None);
            if (parts.Length >= 2)
            {
                string name = parts[0].Trim();
                string phoneNumber = parts[1].Trim();
                var contactToRemove = contactManager.Contacts.Find(c => c.Name == name &&
                c.PhoneNumber == phoneNumber);
                if (contactToRemove != null)
                {
                    try
                    {
                        contactManager.RemoveContact(contactToRemove);
                        UpdateContactsList();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        private void SearchButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(searchTextBox.Text))
            {
                UpdateContactsList();
                return;
            }
            var searchResults = contactManager.SearchContacts(searchTextBox.Text);
            contactsListBox.Items.Clear();
            foreach (var contact in searchResults)
            {
                contactsListBox.Items.Add($"{contact.Name} - {contact.PhoneNumber}");
            }
        }
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ContactForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "ContactForm";
            this.Load += new System.EventHandler(this.ContactForm_Load);
            this.ResumeLayout(false);

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
       
        private void ContactForm_Load(object sender, EventArgs e)
        {

        }
    
       
        }
    }

