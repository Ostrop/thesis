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
    public class GeneralStudyPlan : INotifyPropertyChanged, ICloneable
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
        }
        private int? disciplineId;
        public int? DisciplineId
        {
            get { return disciplineId; }
            set
            {
                if (disciplineId != value)
                {
                    disciplineId = value;
                    if (DisciplineName != dtbCommunication.GetDisciplineName(value))
                        DisciplineName = dtbCommunication.GetDisciplineName(value);
                    OnPropertyChanged(nameof(DisciplineId));
                    UpdateTotalNumberOfHours();
                }
            }
        }
        private DateTime? mondayOfWeek;
        public DateTime? MondayOfWeek
        {
            get { return mondayOfWeek; }
            set
            {
                if (mondayOfWeek != value)
                {
                    mondayOfWeek = value;
                    OnPropertyChanged(nameof(MondayOfWeek));
                    UpdateTotalNumberOfHours();
                }
            }
        }
        private int? studyPlan_DisciplinesByWeekId;
        public int? StudyPlan_DisciplinesByWeekId
        {
            get { return studyPlan_DisciplinesByWeekId; }
            set
            {
                if (studyPlan_DisciplinesByWeekId != value)
                {
                    studyPlan_DisciplinesByWeekId = value;
                    OnPropertyChanged(nameof(StudyPlan_DisciplinesByWeekId));
                    UpdateTotalNumberOfHours();
                }
            }
        }
        public bool isDeleted = false;
        public bool isWeek = false;

        private string disciplineName;
        public string DisciplineName
        {
            get { return disciplineName; }
            set
            {
                if (disciplineName != value)
                {
                    disciplineName = value;
                    if (DisciplineId != dtbCommunication.GetDisciplineId(value))
                        DisciplineId = dtbCommunication.GetDisciplineId(value);
                    OnPropertyChanged(nameof(DisciplineName));
                    UpdateTotalNumberOfHours();
                }
            }
        }
        private int? studyPlanId;
        public int? StudyPlanId
        {
            get { return studyPlanId; }
            set
            {
                if (studyPlanId != value)
                {
                    studyPlanId = value;
                    OnPropertyChanged(nameof(StudyPlanId));
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
            if (isWeek == false)
            {
                // Вычисление общего количества часов
                int? totalHours = HoursOfLectures + HoursOfLaboratory + HoursOfLaboratoryWithComputers;
                TotalNumberOfHours = totalHours;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public object Clone()
        {
            return new GeneralStudyPlan
            {
                StudyPlan_DisciplineId = this.StudyPlan_DisciplineId,
                DisciplineId = this.DisciplineId,
                MondayOfWeek = this.MondayOfWeek,
                StudyPlan_DisciplinesByWeekId = this.StudyPlan_DisciplinesByWeekId,
                isDeleted = this.isDeleted,
                isWeek = this.isWeek,
                DisciplineName = this.DisciplineName,
                StudyPlanId = this.StudyPlanId,
                TotalNumberOfHours = this.TotalNumberOfHours,
                HoursOfLectures = this.HoursOfLectures,
                HoursOfLaboratory = this.HoursOfLaboratory,
                HoursOfLaboratoryWithComputers = this.HoursOfLaboratoryWithComputers
            };
        }
    }
}
