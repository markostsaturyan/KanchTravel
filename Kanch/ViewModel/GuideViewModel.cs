using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Kanch.Commands;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Configuration;
using Kanch.DataModel;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace Kanch.ViewModel
{
    public class GuideViewModel : INotifyPropertyChanged
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
        private string educationGrade;
        private string workExperience;
        private List<ListItem> languages;
        private ObservableCollection<string> places;
        private string inputPlace;

        public  string ErrorMessage
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

        public ObservableCollection<string> Places
        {
            get { return this.places; }
            set
            {
                this.places.Add(value.ToString());
                NotifyPropertyChanged("Places");
            }
        }

        public ICommand InputPlaceCommand { get; set; }

        public ICommand ResetCommand { get; set; }

        public ICommand SubmitCommand { get; set; }

        public string InputPlace
        {
            get { return this.inputPlace; }
            set
            {
                if (this.inputPlace != value)
                {
                    this.inputPlace = value;
                    NotifyPropertyChanged("InputPlace");
                }
            }
        }

        public GuideViewModel()
        {
            Languages = new List<ListItem>();
            Languages.Add(new ListItem() { Text = "Armenian", IsSelected = false });
            Languages.Add(new ListItem() { Text = "Russian", IsSelected = false });
            Languages.Add(new ListItem() { Text = "English", IsSelected = false });
            Languages.Add(new ListItem() { Text = "German", IsSelected = false });
            Languages.Add(new ListItem() { Text = "Italian", IsSelected = false });
            Languages.Add(new ListItem() { Text = "French", IsSelected = false });
            this.places = new ObservableCollection<string>();
            this.InputPlaceCommand = new Command(AddPlace);
            this.ResetCommand = new Command((o) => Reset());
            this.SubmitCommand = new Command((o) => Submit());
        }

        private async void Submit()
        {
            if (GuideInfoValidationResult())
            {
                if (await Registration())
                {
                    var verification = new Verification();
                    var myWindow = System.Windows.Application.Current.MainWindow;
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
            var guide = new Guide()
            {
                FirstName = this.FirstName,
                LastName = this.LastName,
                UserName = this.UserName,
                Email = this.Email,
                DateOfBirth = this.DateOfBirth,
                PhoneNumber = this.PhoneNumber,
                Password = this.Password,
                Profession=this.Profession,
                WorkExperience=this.WorkExperience,
                EducationGrade=this.EducationGrade,
            };
            if (Male == true)
            {
                guide.Gender = "Male";
            }
            else
            {
                guide.Gender = "Female";
            }

            guide.KnowledgeOfLanguages = "";
            foreach (var language in this.Languages)
            {
                if (language.IsSelected)
                    guide.KnowledgeOfLanguages += language.Text + ',';
            }

            foreach(var place in this.Places)
            {
                guide.Places.Add(place);
            }

            var requestResult = await client.PostAsync("api/Guide", new StringContent(JsonConvert.SerializeObject(guide), Encoding.UTF8, "application/json"));
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

        private void AddPlace(object obj)
        {
            if (InputPlace != null)
            {
                this.Places.Add(InputPlace);
                InputPlace = "";
            }
        }

        private void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
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
            this.EducationGrade = "";
            this.WorkExperience = "";
            this.Places.Clear();
            foreach (var language in this.Languages)
            {
                language.IsSelected = false;
            }
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

        private bool EducationGradeValidation()
        {
            if (this.EducationGrade == null)
            {
                this.ErrorMessage = "Enter your education grade.";
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

        public bool GuideInfoValidationResult()
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
            else if (!EducationGradeValidation())
                return false;
            else if (!WorkExperienceValidation())
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
