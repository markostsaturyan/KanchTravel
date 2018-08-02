using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Kanch.Commands;
using Kanch.DataModel;
using Newtonsoft.Json;

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
        private List<ListItem> languages;
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

        public ICommand ResetCommand { get; set; }
        public ICommand SubmitCommand { get; set; }

        public PhotographerViewModel()
        {
            this.MoreInformation = new List<ListItem>();
            this.MoreInformation.Add(new ListItem() { Text = "HasGopro", IsSelected = false });
            this.MoreInformation.Add(new ListItem() { Text = "HasCameraStabilizator", IsSelected = false });
            this.MoreInformation.Add(new ListItem() { Text = "HasDron", IsSelected = false });

            this.Languages = new List<ListItem>();
            this.Languages.Add(new ListItem() { Text = "Armenian", IsSelected = false });
            this.Languages.Add(new ListItem() { Text = "Russian", IsSelected = false });
            this.Languages.Add(new ListItem() { Text = "English", IsSelected = false });
            this.Languages.Add(new ListItem() { Text = "German", IsSelected = false });
            this.Languages.Add(new ListItem() { Text = "Italian", IsSelected = false });
            this.Languages.Add(new ListItem() { Text = "French", IsSelected = false });
            this.ResetCommand = new Command((o) => Reset());
            this.SubmitCommand = new Command((o) => Submit());
        }

        public void Reset()
        {
            this.FirstName = null;
            this.LastName = null;
            this.UserName = null;
            this.Email = null;
            this.PhoneNumber = null;
            this.Password = null;
            this.ConfirmPassword = null;
            this.Male = null;
            this.Female = null;
            this.Profession = "";
            this.WorkExperience = "";
            this.CameraModel = "";
            this.IsProfessional = false;
            foreach(var info in MoreInformation)
            {
                info.IsSelected = false;
            }
        }

        public async void Submit()
        {
            if (PhotographerInfoValidationResult())
            {
                if (await Registration())
                {
                    var verification = new Verification();
                    var myWindow = Application.Current.MainWindow;
                    verification.Show();
                    myWindow.Close();
                }
            }

            return;
        }

        public async Task<bool> Registration()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationSettings.AppSettings["baseUrl"]);
            var photographer = new Photographer()
            {
                FirstName = this.FirstName,
                LastName = this.LastName,
                UserName = this.UserName,
                Email = this.Email,
                DateOfBirth = this.DateOfBirth,
                PhoneNumber = this.PhoneNumber,
                Password = this.Password,
                Profession = this.Profession,
                WorkExperience = this.WorkExperience,
                HasGopro = this.MoreInformation[0].IsSelected,
                HasCameraStabilizator = this.MoreInformation[1].IsSelected,
                HasDron = this.MoreInformation[2].IsSelected,
                Camera = new Camera()
                {
                    Model = this.CameraModel,
                    IsProfessional = this.IsProfessional
                }
            };
            if (Male == true)
            {
                photographer.Gender = "Male";
            }
            else
            {
                photographer.Gender = "Female";
            }

            photographer.KnowledgeOfLanguages = "";
            foreach (var language in this.Languages)
            {
                if (language.IsSelected)
                    photographer.KnowledgeOfLanguages += language.Text + ',';
            }


            var requestResult = await client.PostAsync("api/Photographer", new StringContent(JsonConvert.SerializeObject(photographer), Encoding.UTF8, "application/json"));
            var content = requestResult.Content;

            var jsonContent = content.ReadAsStringAsync().Result;

            var status = JsonConvert.DeserializeObject<Status>(jsonContent);
            if (!status.IsOk)
            {
                this.ErrorMessage = status.Message;
                return false;
            }
            return true;
        }

        private bool FirstNameValidation()
        {
            if (this.FirstName == null)
            {
                this.ErrorMessage = "Enter your first name.";
                return false;
            }
            return true;
        }

        private bool LastNameValidation()
        {
            if (this.LastName == null)
            {
                this.ErrorMessage = "Enter your last name";
                return false;
            }
            return true;
        }

        private bool UserNameValidation()
        {
            if (this.UserName == null)
            {
                this.ErrorMessage = "Enter a user name";
                return false;
            }
            return true;
        }

        private bool DateOfBirthValidation()
        {
            if (this.DateOfBirth == null)
            {
                this.ErrorMessage = "Enter your date of birth";
                return false;
            }
            return true;
        }

        private bool GenderValidation()
        {
            if (this.male == null && this.female == null)
            {
                this.ErrorMessage = "Check your gender";
                return false;
            }
            return true;
        }

        private bool EmailValidation()
        {
            if (this.Email.Length == 0)
            {
                this.ErrorMessage = "Enter an email.";
                return false;
            }
            else if (!Regex.IsMatch(this.Email, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))

            {
                this.ErrorMessage = "Enter a valid email.";
                return false;
            }
            return true;
        }

        private bool PasswordValidation()
        {
            if (this.Password.Length == 0)
            {
                this.ErrorMessage = "Enter password.";
                return false;
            }
            else if (this.ConfirmPassword.Length == 0)
            {
                this.ErrorMessage = "Enter Confirm password.";
                return false;
            }
            else if (this.Password != this.ConfirmPassword)
            {
                this.ErrorMessage = "Confirm password must be same as password.";
                return false;
            }
            return true;
        }

        private bool PhoneNumberValidation()
        {
            int index = 0;
            if (this.PhoneNumber[0] == '+')
                index++;
            while (index < this.PhoneNumber.Length)
            {
                if (this.PhoneNumber[index] < '0' || this.PhoneNumber[index] > '9')
                {
                    this.ErrorMessage = "Enter a valid phone number.";
                    return false;
                }
                index++;
            }
            return true;
        }

        private bool ProfessionValidation()
        {
            if (this.Profession == null)
            {
                this.ErrorMessage = "Enter your profession.";
                return false;
            }
            return true;
        }

        private bool WorkExperienceValidation()
        {
            if (this.WorkExperience == null)
            {
                this.ErrorMessage = "Enter your work experience.";
                return false;
            }
            return true;
        }

        private bool CameraModelValidation()
        {
            if (this.CameraModel == null)
            {
                this.ErrorMessage = "Enter model of the Camera.";
                return false;
            }
            return true;
        }

        public bool PhotographerInfoValidationResult()
        {
            if (!FirstNameValidation())
                return false;
            else if (!LastNameValidation())
                return false;
            else if (!UserNameValidation())
                return false;
            else if (!DateOfBirthValidation())
                return false;
            else if (!GenderValidation())
                return false;
            else if (!EmailValidation())
                return false;
            else if (!PasswordValidation())
                return false;
            else if (!PhoneNumberValidation())
                return false;
            else if (!ProfessionValidation())
                return false;
            else if (!WorkExperienceValidation())
                return false;
            else if (!CameraModelValidation())
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
