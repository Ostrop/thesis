using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTable.Model;

namespace TimeTable.Classes
{
    public class TimetableWeek : INotifyPropertyChanged
    {
        private string time;
        public string Time
        {
            get { return time; }
            set
            {
                if (time != value)
                {
                    time = value;
                    OnPropertyChanged(nameof(Time));
                }
            }
        }

        private string monday;
        public string Monday
        {
            get { return monday; }
            set
            {
                if (monday != value)
                {
                    monday = value;
                    OnPropertyChanged(nameof(Monday));
                }
            }
        }

        private string tuesday;
        public string Tuesday
        {
            get { return tuesday; }
            set
            {
                if (tuesday != value)
                {
                    tuesday = value;
                    OnPropertyChanged(nameof(Tuesday));
                }
            }
        }

        private string wednesday;
        public string Wednesday
        {
            get { return wednesday; }
            set
            {
                if (wednesday != value)
                {
                    wednesday = value;
                    OnPropertyChanged(nameof(Wednesday));
                }
            }
        }

        private string thursday;
        public string Thursday
        {
            get { return thursday; }
            set
            {
                if (thursday != value)
                {
                    thursday = value;
                    OnPropertyChanged(nameof(Thursday));
                }
            }
        }

        private string friday;
        public string Friday
        {
            get { return friday; }
            set
            {
                if (friday != value)
                {
                    friday = value;
                    OnPropertyChanged(nameof(Friday));
                }
            }
        }

        private string saturday;
        public string Saturday
        {
            get { return saturday; }
            set
            {
                if (saturday != value)
                {
                    saturday = value;
                    OnPropertyChanged(nameof(Saturday));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TimetableWeek(string time, List<Availability> availability)
        {
            Time = time;
            Monday = availability.FirstOrDefault(a => a.Date == new DateTime(2000, 01, 03))?.Rule;
            Tuesday = availability.FirstOrDefault(a => a.Date == new DateTime(2000, 01, 04))?.Rule;
            Wednesday = availability.FirstOrDefault(a => a.Date == new DateTime(2000, 01, 05))?.Rule;
            Thursday = availability.FirstOrDefault(a => a.Date == new DateTime(2000, 01, 06))?.Rule;
            Friday = availability.FirstOrDefault(a => a.Date == new DateTime(2000, 01, 07))?.Rule;
            Saturday = availability.FirstOrDefault(a => a.Date == new DateTime(2000, 01, 08))?.Rule;
        }

        public TimetableWeek(string time, List<Availability> availability, DateTime startDate)
        {
            Time = time;
            Monday = availability?.FirstOrDefault(a => a.Date == startDate)?.Rule == null ? "Все равно" : availability.FirstOrDefault(a => a.Date == startDate)?.Rule;
            Tuesday = availability?.FirstOrDefault(a => a.Date == startDate.AddDays(1))?.Rule == null ? "Все равно" : availability.FirstOrDefault(a => a.Date == startDate.AddDays(1))?.Rule;
            Wednesday = availability?.FirstOrDefault(a => a.Date == startDate.AddDays(2))?.Rule == null ? "Все равно" : availability.FirstOrDefault(a => a.Date == startDate.AddDays(2))?.Rule;
            Thursday = availability?.FirstOrDefault(a => a.Date == startDate.AddDays(3))?.Rule == null ? "Все равно" : availability.FirstOrDefault(a => a.Date == startDate.AddDays(3))?.Rule;
            Friday = availability?.FirstOrDefault(a => a.Date == startDate.AddDays(4))?.Rule == null ? "Все равно" : availability.FirstOrDefault(a => a.Date == startDate.AddDays(4))?.Rule;
            Saturday = availability?.FirstOrDefault(a => a.Date == startDate.AddDays(5))?.Rule == null ? "Все равно" : availability.FirstOrDefault(a => a.Date == startDate.AddDays(5))?.Rule;

        }
    }
}
