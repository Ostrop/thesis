using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTable.Classes
{
    public class Group
    {
        private Nullable<int> _groupId;
        public int? GroupId
        {
            get { return _groupId; }
            set { _groupId = value; }
        }

        private Nullable<int> _specialityNumber;
        public int? SpecialityNumber
        {
            get { return _specialityNumber; }
            set { _specialityNumber = value; }
        }

        private Nullable<int> _groupNumber;
        public int? GroupNumber
        {
            get { return _groupNumber; }
            set { _groupNumber = value; }
        }

        private Nullable<DateTime> _date;
        public DateTime? Date
        {
            get { return _date; }
            set { _date = value; }
        }

        private Nullable<int> _course;
        public int? Course
        {
            get { return _course; }
            set { _course = value; }
        }

    }
}
