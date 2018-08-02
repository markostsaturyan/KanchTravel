using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace Kanch.ViewModel
{
    public class PhotographerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string errorMessage;
        private string firstName;
        private string lastName;
        private string userName;
        private string email;
        private bool? male;
        private bool? female;
        private DateTime dateOfBirth;
        private string phoneNumber;
        private string password;
        private string confirmPassword;
        private string profession;
        private string workExperience;
        private List<ListItem> moreInformation;
        private string cameraModel;
        private bool isProfessional;

        public string ErrorMessage
        {
            get { return this.errorMessage; }
            set
            {
                if (this.errorMessage != value)
                {
                    this.errorMessage = value;
                    NotifyPropertyChanged("ErrorMessage");
                }
            }
        }

        public string FirstName
        {
            get
            {
                return this.firstName;
            }
            set
            {
                if (this.firstName != value)
                {
                    this.firstName = value;
                    NotifyPropertyChanged("FirstName");
                }
            }
        }

        public string LastName
        {
            get
            {
                return this.lastName;
            }
            set
            {
                if (this.lastName != value)
                {
                    this.lastName = value;
                    NotifyPropertyChanged("LastName");
                }
            }
        }

        public string UserName
        {
            get
            {
                return this.userName;
            }
            set
            {
                if (this.userName != value)
                {
                    this.userName = value;
                    NotifyPropertyChanged("UserName");
                }
            }
        }

        public string Email
        {
            get { return this.email; }
            set
            {
                if (this.email != value)
                {
                    this.email = value;
                    NotifyPropertyChanged("Email");
                }
            }
        }

        public DateTime DateOfBirth
        {
            get { return this.dateOfBirth; }
            set
            {
                if (this.dateOfBirth != value)
                {
                    this.dateOfBirth = value;
                    NotifyPropertyChanged("DateOfBirth");
                }
            }
        }

        public bool? Male
        {
            get { return this.male; }
            set
            {
                this.male = value;
                NotifyPropertyChanged("Male");
            }
        }
        public bool? Female
        {
            get { return this.female; }
            set
            {
                this.female = value;
                NotifyPropertyChanged("Female");
            }
        }

        public string PhoneNumber
        {
            get
            {
                return this.phoneNumber;
            }
            set
            {
                if (this.phoneNumber != value)
                {
                    this.phoneNumber = value;
                    NotifyPropertyChanged("PhoneNumber");
                }
            }
        }

        public string Password
        {
            get
            {
                return this.password;
            }
            set
            {
                if (this.password != value)
                {
                    this.password = value;
                    NotifyPropertyChanged("Password");
                }
            }
        }

        public string ConfirmPassword
        {
            get
            {
                return this.confirmPassword;
            }
            set
            {
                if (this.confirmPassword != value)
                {
                    this.confirmPassword = value;
                    NotifyPropertyChanged("ConfirmPassword");
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


        public string CameraModel
        {
            get
            {
                return this.cameraModel;
            }
            set
            {
                if (this.cameraModel != value)
                {
                    this.cameraModel = value;
                    NotifyPropertyChanged("CameraModel");
                }
            }
        }

        public bool IsProfessional
        {
            get
            {
                return this.isProfessional;
            }
            set
            {
                if (this.isProfessional != value)
                {
                    this.isProfessional = value;
                    NotifyPropertyChanged("IsProfessional");
                }
            }
        }

        public List<ListItem> MoreInformation
        {
            get { return this.moreInformation; }
            set
            {
                this.moreInformation = value;
                NotifyPropertyChanged("MoreInformation");
            }
        }
        private void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        public PhotographerViewModel()
        {
            this.MoreInformation = new List<ListItem>();
            this.MoreInformation.Add(new ListItem() { Text = "HasGopro", IsSelected = false });
            this.MoreInformation.Add(new ListItem() { Text = "HasCameraStabilizator", IsSelected = false });
            this.MoreInformation.Add(new ListItem() { Text = "HasDron", IsSelected = false });
        }

        public void Reset()
        {
            this.Profession = "";
            this.WorkExperience = "";
            this.CameraModel = "";
            this.IsProfessional = false;
            foreach(var info in MoreInformation)
            {
                info.IsSelected = false;
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

        private bool WorkExperienceValidation(ref string status)
        {
            if (WorkExperience == null)
            {
                status = "Enter your work experience.";
                return false;
            }
            return true;
        }

        private bool CameraModelValidation(ref string status)
        {
            if (CameraModel == null)
            {
                status = "Enter model of the Camera.";
                return false;
            }
            return true;
        }

        public bool PhotographerInfoValidation(out string status)
        {
            status = "";
            if (!ProfessionValidation(ref status))
                return false;
            else if (!WorkExperienceValidation(ref status))
                return false;
            else if (!CameraModelValidation(ref status))
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
