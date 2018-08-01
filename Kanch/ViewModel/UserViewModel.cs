using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Kanch.Commands;
using Kanch.DataModel;
using Newtonsoft.Json;

namespace Kanch.ViewModel
{
    /// <summary>
    /// Class for User registration.
    /// </summary>
    public class UserViewModel : INotifyPropertyChanged
    {
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

        /// <summary>
        /// Property for Error message.
        /// </summary>
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

        /// <summary>
        /// Firs name of the user.
        /// </summary>
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

        /// <summary>
        /// Last name of the user.
        /// </summary>
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

        /// <summary>
        /// Username to unique a user.
        /// </summary>
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

        /// <summary>
        /// Email to send a verification code.
        /// </summary>
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

        /// <summary>
        /// User's date of birth.
        /// </summary>
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

        /// <summary>
        /// Property for male gender.
        /// </summary>
        public bool? Male
        {
            get { return this.male; }
            set
            {
                this.male = value;
                NotifyPropertyChanged("Male");
            }
        }

        /// <summary>
        /// Property for female gender.
        /// </summary>
        public bool? Female
        {
            get { return this.female; }
            set
            {
                this.female = value;
                NotifyPropertyChanged("Female");
            }
        }

        /// <summary>
        /// Phone number of the user to contact with him.
        /// </summary>
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

        /// <summary>
        /// Password of the user.
        /// </summary>
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

        public ICommand SubmitCommand { get; set; }

        public ICommand ResetCommand { get; set; }

        /// <summary>
        /// Confirm password.
        /// </summary>
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

        public UserViewModel()
        {
            this.ResetCommand = new Command((o) => Reset());
            this.SubmitCommand = new Command((o) => Submit());
        }


        public event PropertyChangedEventHandler PropertyChanged;

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
        }

        public async void Submit()
        {
            if (UserInfoValidationResult())
            {
                if(await Registration())
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
            var user = new User()
            {
                FirstName    =  this.FirstName,
                LastName     =  this.LastName,
                UserName     =  this.UserName,
                Email        =  this.Email,
                DateOfBirth  =  this.DateOfBirth,
                PhoneNumber  =  this.PhoneNumber,
                Password     =  this.Password,
            };
            if (Male == true)
            {
                user.Gender = "Male";
            }
            else
            {
                user.Gender = "Female";
            }

            var requestResult = await client.PostAsync("api/User", new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"));
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

        public bool UserInfoValidationResult()
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
            return true;
        }


    }
}
