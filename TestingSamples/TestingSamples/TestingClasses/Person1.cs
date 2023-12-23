using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSamples.AnotherClass
{
    public class Person1
    {
        // Properties
        public string Name { get; set; }
        public int Age { get; set; }
        public int Address { get; set; }
        public int Qualification { get; set; }

        // Constructor
        public Person1(string name, int age)
        {
            Name = name;
            Age = age;
        }
        //#
    }
}
