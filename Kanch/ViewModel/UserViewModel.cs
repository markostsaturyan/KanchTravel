using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Kanch.ViewModel
{
    public class UserViewModel : INotifyPropertyChanged
    {

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
            this.male = false;
            this.female = false;
        }


        private bool FirstNameValidation(ref string status)
        {
            if (FirstName == null)
            {
                status = "Enter your first name.";
                return false;
            }
            return true;
        }

        private bool LastNameValidation(ref string status)
        {
            if (LastName == null)
            {
                status = "Enter your last name";
                return false;
            }
            return true;
        }

        private bool UserNameValidation(ref string status)
        {
            if (UserName == null)
            {
                status = "Enter a user name";
                return false;
            }
            return true;
        }

        private bool DateOfBirthValidation(ref string status)
        {
            if (DateOfBirth == null)
            {
                status = "Enter your date of birth";
                return false;
            }
            return true;
        }

        private bool GenderValidation(ref string status)
        {
            if (male == null && female == null)
            {
                status = "Check your gender";
                return false;
            }
            return true;
        }

        private bool EmailValidation(ref string status)
        {
            if (Email.Length == 0)
            {
                status = "Enter an email.";
                return false;
            }
            else if (!Regex.IsMatch(email, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))

            {
                status = "Enter a valid email.";
                return false;
            }
            return true;
        }

        private bool PasswordValidation(ref string status)
        {
            if (Password.Length == 0)
            {
                status = "Enter password.";
                return false;
            }
            else if (ConfirmPassword.Length == 0)
            {
                status = "Enter Confirm password.";
                return false;
            }
            else if (Password != ConfirmPassword)
            {
                status = "Confirm password must be same as password.";
                return false;
            }
            return true;
        }

        private bool PhoneNumberValidation(ref string status)
        {
            int index = 0;
            if (PhoneNumber[0] == '+')
                index++;
            while (index < PhoneNumber.Length)
            {
                if (PhoneNumber[index] <= '0' || PhoneNumber[index] >= '9')
                {
                    status = "Enter a valid phone number.";
                    return false;
                }
                index++;
            }
            return true;
        }

        public bool UserInfoValidationResult(out string status)
        {
            status = "";
            if (!FirstNameValidation(ref status))
                return false;
            else if (!LastNameValidation(ref status))
                return false;
            else if (!UserNameValidation(ref status))
                return false;
            else if (!DateOfBirthValidation(ref status))
                return false;
            else if (!GenderValidation(ref status))
                return false;
            else if (!EmailValidation(ref status))
                return false;
            else if (!PasswordValidation(ref status))
                return false;
            else if (!PhoneNumberValidation(ref status))
                return false;
            return true;
        }


    }
}
