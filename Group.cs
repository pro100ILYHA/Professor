using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    public class Group
    {
        public string Name { get; set; }
        public Group(string name)
        {
            Name = name;
        }
        public override string ToString()
        {
            return Name;
        }
        public override bool Equals(object obj)
        {
            return obj is Group other && other.Name == this.Name;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
