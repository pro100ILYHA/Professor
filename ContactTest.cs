using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lab1;

namespace UnitTestProject1
{
    [TestClass]
    public class ContactTests
    {
        [TestMethod]
        public void ТестВводаИмениИНомераВПолеИмяИНомер()
        {
            //arrange
            var name = "Максим";
            var number = "89110844273";
            //act 
            var contact = new Contact(name, number);
            //assert
            Assert.AreEqual(name, contact.Name);
            Assert.AreEqual(number, contact.PhoneNumber);
        }
        [TestMethod]
        public void ИзменениеУжеСуществующихЗначенийИмениИНомераТелефона()
        {
            //arrange
            var contact = new Contact("Максим", "89110844273");
            //act
            contact.Name = "Илья";
            contact.PhoneNumber = "89813235136";
            //assert
            Assert.AreEqual("Илья", contact.Name);
            Assert.AreEqual("89813235136", contact.PhoneNumber);
        }
    }
}
