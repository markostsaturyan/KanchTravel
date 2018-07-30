using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace Kanch.ViewModel
{
    public class GuideViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Visibility guideVisible;
        private string profession;
        private string educationGrade;
        private string workExperience;
        private List<ListItem> languages;

        public Visibility GuideVisible
        {
            get
            {
                return guideVisible;
            }

            set
            {
                if (guideVisible != value)
                {
                    guideVisible = value;
                    NotifyPropertyChanged("GuideVisible");
                }
            }
        }

        public string Profession
        {
            get
            {
                return this.profession;
            }
            set
            {
                if (this.profession != value)
                {
                    this.profession = value;
                    NotifyPropertyChanged("Profession");
                }
            }
        }

        public string EducationGrade
        {
            get
            {
                return this.educationGrade;
            }
            set
            {
                if (this.educationGrade != value)
                {
                    this.educationGrade = value;
                    NotifyPropertyChanged("EducationGrade");
                }
            }
        }

        public string WorkExperience
        {
            get
            {
                return this.workExperience;
            }
            set
            {
                if (this.workExperience != value)
                {
                    this.workExperience = value;
                    NotifyPropertyChanged("WorkExperience");
                }
            }
        }

        public List<ListItem> Languages
        {
            get { return this.languages; }
            set
            {
                if (this.languages != value)
                {
                    this.languages = value;
                    NotifyPropertyChanged("Languages");
                }
            }
        }

        public GuideViewModel()
        {
            this.GuideVisible = Visibility.Collapsed;
            Languages = new List<ListItem>();
            Languages.Add(new ListItem() { Text = "Armenian", IsSelected = false });
            Languages.Add(new ListItem() { Text = "Russian", IsSelected = false });
            Languages.Add(new ListItem() { Text = "English", IsSelected = false });
            Languages.Add(new ListItem() { Text = "German", IsSelected = false });
            Languages.Add(new ListItem() { Text = "Italian", IsSelected = false });
            Languages.Add(new ListItem() { Text = "French", IsSelected = false });
        }

        private void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        public void Reset()
        {
            this.Profession = "";
            this.EducationGrade = "";
            this.WorkExperience = "";
            foreach (var language in this.Languages)
            {
                language.IsSelected = false;
            }
        }

        private bool ProfessionValidation(ref string status)
        {
            if (Profession == null)
            {
                status = "Enter your profession.";
                return false;
            }
            return true;
        }

        private bool EducationGradeValidation(ref string status)
        {
            if (EducationGrade == null)
            {
                status = "Enter your education grade.";
                return false;
            }
            return true;
        }

        private bool WorkExperienceValidation(ref string status)
        {
            if (WorkExperience == null)
            {
                status = "Enter your work experience.";
                return false;
            }
            return true;
        }

        public bool GuideInfoValidation(out string status)
        {
            status = "";
            if (!ProfessionValidation(ref status))
                return false;
            else if (!EducationGradeValidation(ref status))
                return false;
            else if (!WorkExperienceValidation(ref status))
                return false;
            return true;
        }

        public class ListItem : INotifyPropertyChanged
        {
            private bool isSelected;
            public string Text { get; set; }

            public bool IsSelected
            {
                get { return this.isSelected; }
                set
                {
                    this.isSelected = value;
                    this.OnPropertyChanged("IsSelected");
                }
            }
            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged(string strPropertyName)
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(strPropertyName));
            }
        }
    }
}
