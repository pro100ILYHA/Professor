using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace Lab1
{
    public class ContactManager
    {
        public List<Contact> Contacts { get; private set; }
        public List <Group> Groups { get; private set; }
        private string contactsFilePath1;
        private string groupsFilePath1;
        public ContactManager() : this("contacts.txt", "groups.txt")
        { 
        }
        public ContactManager(string contactsFilePath, string groupsFilePath)
        {
            contactsFilePath1 = contactsFilePath;
            groupsFilePath1 = groupsFilePath;
            Contacts = new List<Contact>();
            Groups = new List<Group>();
           
            LoadContacts();
            LoadGroups();
            EnsureDefaultGroup();
        }
        private void EnsureDefaultGroup()
        {
            if(!Groups.Any(g => g.Name == "Без группы"))
            {
                Groups.Insert(0, new Group("Без группы"));
                SaveGroups();
            }    
            
        }
        public void AddGroup(string groupName)
        {
            if (string.IsNullOrWhiteSpace(groupName))
                throw new
                    ArgumentException("Имя группы должно быть заполненно");
            if (Groups.Any(g => g.Name == groupName))
                throw new
                    ArgumentException($"{groupName} Группа уже существует");
            Groups.Add(new Group(groupName));
            SaveGroups();
        }
        public void RemoveGroup(string groupName)
        {
            if (groupName == "Без группы")
                throw new ArgumentException("Нельзя удалить группу 'Без группы'");
            var group = Groups.FirstOrDefault(g => g.Name == groupName);
            if (group != null)
            {
                foreach(var contact in Contacts.Where(c => c.GroupName == groupName))
                {
                    contact.GroupName = "Без группы";
                }
                Groups.Remove(group);
                SaveContacts();
                SaveGroups();
            }
        }
        public List<Group> GetAllGroups()
        {
            return Groups.ToList(); 
        }
        public void ChangeContactGroup(Contact contact, string newGroupName)
        {
            if (contact == null)
                throw new ArgumentNullException(nameof(contact));
            if (!Groups.Any(g => g.Name == newGroupName))
                throw new ArgumentException($"Группа '{newGroupName}' не существует");
            contact.GroupName = newGroupName;
            SaveContacts();
            SaveGroups();
        }
        public List<Contact> GetContactsByGroup(string groupName)
        {
            if(string.IsNullOrEmpty(groupName) || groupName == "Все контакты")
                return Contacts.ToList();
            return Contacts.Where(c => c.GroupName == groupName).ToList();
        }
        public void AddContact(Contact contact)
        {
            if (contact == null)
                throw new ArgumentNullException(nameof(contact));
            if (string.IsNullOrEmpty(contact.GroupName) || !Groups.Any(g => g.Name == contact.GroupName))
            {
                contact.GroupName = "Без группы";
            }
            Contacts.Add(contact);
            SaveContacts();
        }
        
        public void RemoveContact(Contact contact)
        {
            if (contact == null)
            {
                throw new ArgumentNullException(nameof(contact));
            }
            Contacts.Remove(contact);
            SaveContacts();
        }
        public List<Contact> SearchContacts(string query)
        {
            return Contacts.Where(c => c.Name.Contains(query) ||
            c.PhoneNumber.Contains(query)).ToList();
        }
        private void SaveGroups()
        {
            var lines = Groups.Select(g => g.Name);
            File.WriteAllLines(groupsFilePath1, lines);
        }
        private void SaveContacts()
        {
            var lines = Contacts.Select(c => $"{c.Name}|{c.PhoneNumber}|{c.GroupName}");
            File.WriteAllLines(contactsFilePath1, lines);
        }
        public List<Contact> SearchContacts(string query, string groupName)
        {
            var contactsByGroup = GetContactsByGroup(groupName);
            if (string.IsNullOrEmpty(query))
                return contactsByGroup;
            return contactsByGroup.Where(c => (c.Name?.Contains(query) ?? false) || (c.PhoneNumber?.Contains(query) ?? false)).ToList();
        }
        private void LoadContacts()
        {
            if (File.Exists(contactsFilePath1))
            {
                var lines = File.ReadAllLines(contactsFilePath1);
                Contacts.Clear();
                foreach (var line in lines)
                {
                    var parts = line.Split('|');
                    if (parts.Length >= 2)
                    {
                        string name = parts[0];
                        string phoneNumber = parts[1];
                        string groupName = parts.Length > 3 ? parts[2] : "Без группы";
                        Contacts.Add(new Contact(name, phoneNumber, groupName));
                    }
                }
            }
        }
        public void LoadGroups()
        {
            if (File.Exists(groupsFilePath1))
            {
                var lines = File.ReadAllLines(groupsFilePath1);
                Groups.Clear();
                foreach( var line in lines)
                {
                    if(!string.IsNullOrWhiteSpace(line))
                    {
                        Groups.Add(new Group(line.Trim()));
                    }
                }
            }
            if(!Groups.Any(g => g.Name == "Без группы"))
            {
                Groups.Add(new Group("Без группы"));
            }
        }
    }
}
