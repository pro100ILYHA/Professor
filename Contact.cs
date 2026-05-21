using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace Lab1
{
    public class Contact
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string GroupName { get; set; }
        public Contact(string name, string phoneNumber) : this(name, phoneNumber, "Без группы") 
        {
        }
        public Contact(string name, string phoneNumber, string groupName)
        {
            Name = name;
            PhoneNumber = phoneNumber;
            GroupName = string.IsNullOrEmpty(groupName) ? "Без группы" : groupName;
        }
        public override string ToString()
        {
            return $"{Name} - {PhoneNumber}[{GroupName}]";
        }
    }
    }


