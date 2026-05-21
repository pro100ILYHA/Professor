using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lab1;
using System.Collections.Generic;
using System.Linq;
using System.IO;
namespace ContactManagerTest
{
    [TestClass]
    public class ContactManagerTests
    {
        [TestMethod]
        public void ПриДобавленииКонтактаОнПоявляетсяВСписке()
        {
            if (File.Exists("contacts.txt"))
                File.Delete("contacts.txt");

                //arrange
                var manager1 = new ContactManager();
            var contact = new Contact("Максим", "89110844273");
            int count = 1;
            //act
            manager1.AddContact(contact);
            //assert
            Assert.AreEqual(count, manager1.Contacts.Count);
            Assert.IsTrue(manager1.Contacts.Contains(contact));
        }
        [TestMethod]
        public void УдалениеСуществующегоКонтакта()
        {
            //arrange
            ContactManager manager2 = new ContactManager();
            Contact contactToRemove = new Contact("Иван", "89110544273");
            manager2.AddContact(contactToRemove);
            int countBeforeRemove = manager2.Contacts.Count;
            //act
            manager2.RemoveContact(contactToRemove);
            //assert
            Assert.AreEqual(countBeforeRemove - 1, manager2.Contacts.Count);
            Assert.IsFalse(manager2.Contacts.Contains(contactToRemove));
        }
        [TestMethod]
        public void ПоискСуществующегоКонтакта()
        {
            if (File.Exists("contacts.txt"))
                File.Delete("contacts.txt");
            //arrange
            ContactManager manager3 = new ContactManager();
            manager3.AddContact(new Contact("Максим Драмбович", "89110844273"));
            manager3.AddContact(new Contact("Илья Субботин", "22222222222"));
            manager3.AddContact(new Contact("Илья Иванов", "11111111111"));
            //act
            List<Contact> searchResults = manager3.SearchContacts("Илья");
            //assert
            Assert.AreEqual(2, searchResults.Count);
            bool foundMaximDrambovich = false;
            bool foundIlyaSubbotin = false;
            bool foundIlyaIvanov = false;
            foreach (Contact contact in searchResults)
            {
                if (contact.Name == "Максим Драмбович")
                    foundMaximDrambovich = true;
                if (contact.Name == "Илья Субботин")
                    foundIlyaSubbotin = true;
                if (contact.Name == "Илья Иванов")
                    foundIlyaIvanov = true;
            }
            Assert.IsTrue(foundIlyaIvanov);
            Assert.IsFalse(foundMaximDrambovich);
            Assert.IsTrue(foundIlyaSubbotin);
        }
        [TestMethod]
        public void ТестированиеМетодаSaveContactЧерезПубличныйМетодAddContact1()
        {
            if (File.Exists("contacts.txt"))
                File.Delete("contacts.txt");
            //arrange
            var manager = new ContactManager();
            var contact = new Contact("Максим", "89110844273");
            //act
            manager.AddContact(contact);
            //assert
            Assert.IsTrue(File.Exists("contacts.txt"));
            var fileContent = File.ReadAllText("contacts.txt");
            Assert.AreEqual("Максим|89110844273", fileContent.Trim());
        }
        [TestMethod]
        public void ТестированиеМетодаLoadContactsЧерезКонструктор()
        {
            if (File.Exists("contacts.txt"))
                File.Delete("contacts.txt");
            //arrange
            File.WriteAllLines("contacts.txt", new[]
            {
        "Максим|111",
        "Илья|222"
            });
            //act
            var manager = new ContactManager();
            //assert
            Assert.AreEqual(2, manager.Contacts.Count);
            Assert.AreEqual("Максим", manager.Contacts[0].Name);
            Assert.AreEqual("111", manager.Contacts[0].PhoneNumber);
        }
    }
}
