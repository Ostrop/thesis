using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace TimeTable.Classes
{
    public class GeneralStudyPlan : INotifyPropertyChanged
    {
        private int? studyPlan_DisciplineId;
        public int? StudyPlan_DisciplineId
        {
            get { return studyPlan_DisciplineId; }
            set
            {
                if (studyPlan_DisciplineId != value)
                {
                    studyPlan_DisciplineId = value;
                    OnPropertyChanged(nameof(StudyPlan_DisciplineId));
                    UpdateTotalNumberOfHours();
                }
            }
        }private int? requiredNumberOfHours;
        public int? RequiredNumberOfHours
        {
            get { return requiredNumberOfHours; }
            set
            {
                if (requiredNumberOfHours != value)
                {
                    requiredNumberOfHours = value;
                    OnPropertyChanged(nameof(RequiredNumberOfHours));
                    UpdateTotalNumberOfHours();
                }
            }
        }

        private string disciplineName;
        public string DisciplineName
        {
            get { return disciplineName; }
            set
            {
                if (disciplineName != value)
                {
                    disciplineName = value;
                    OnPropertyChanged(nameof(DisciplineName));
                    UpdateTotalNumberOfHours();
                }
            }
        }

        private int? totalNumberOfHours;
        public int? TotalNumberOfHours
        {
            get { return totalNumberOfHours; }
            set
            {
                if (totalNumberOfHours != value)
                {
                    totalNumberOfHours = value;
                    OnPropertyChanged(nameof(TotalNumberOfHours));
                }
            }
        }

        private int? hoursOfLectures;
        public int? HoursOfLectures
        {
            get { return hoursOfLectures; }
            set
            {
                if (hoursOfLectures != value)
                {
                    hoursOfLectures = value;
                    OnPropertyChanged(nameof(HoursOfLectures));
                    UpdateTotalNumberOfHours();
                }
            }
        }

        private int? hoursOfLaboratory;
        public int? HoursOfLaboratory
        {
            get { return hoursOfLaboratory; }
            set
            {
                if (hoursOfLaboratory != value)
                {
                    hoursOfLaboratory = value;
                    OnPropertyChanged(nameof(HoursOfLaboratory));
                    UpdateTotalNumberOfHours();
                }
            }
        }

        private int? hoursOfLaboratoryWithComputers;
        public int? HoursOfLaboratoryWithComputers
        {
            get { return hoursOfLaboratoryWithComputers; }
            set
            {
                if (hoursOfLaboratoryWithComputers != value)
                {
                    hoursOfLaboratoryWithComputers = value;
                    OnPropertyChanged(nameof(HoursOfLaboratoryWithComputers));
                    UpdateTotalNumberOfHours();
                }
            }
        }

        private void UpdateTotalNumberOfHours()
        {
            // Вычисление общего количества часов
            int? totalHours = HoursOfLectures + HoursOfLaboratory + HoursOfLaboratoryWithComputers;
            TotalNumberOfHours = totalHours;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
