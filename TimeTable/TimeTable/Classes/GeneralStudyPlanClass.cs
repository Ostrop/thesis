using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace TimeTable.Classes
{
    public class GeneralStudyPlan
    {
        public int Number { get; set; }
        public string DisciplineName { get; set; }
        public int TotalNumberOfHours { get; set; }
        public int HoursOfLectures { get; set; }
        public int HoursOfLaboratory { get; set; }
        public int HoursOfLaboratoryWithComputers { get; set; }
        public ICommand DeleteCommand { get; private set; }

    }
}
