using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTable.Classes
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Teacher(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
