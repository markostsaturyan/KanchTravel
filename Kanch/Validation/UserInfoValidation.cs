using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Kanch.DataModel;

namespace Kanch.Validation
{
    public class UserInfoValidation
    {
        private bool FirstNameValidation(string firstName, ref string status)
        {
            if (firstName == null)
            {
                status = "Enter your first name.";
                return false;
            }
            return true;
        }
        private bool LastNameValidation(string lastName, ref string status)
        {
            if (lastName == null)
            {
                status = "Enter your last name";
                return false;
            }
            return true;
        }
        private bool UserNameValidation(string userName,ref string status)
        {
            if (userName == null)
            {
                status = "Enter a user name";
                return false;
            }
            return true;
        }
        private bool DateOfBirthValidation(DateTime dateTime,ref string status)
        {
            if (dateTime == null)
            {
                status = "Enter your date of birth";
                return false;
            }
            return true;
        }
        private bool GenderValidation(string gender, ref string status)
        {
            if (gender == null)
            {
                status = "Check your gender";
                return false;
            }
            return true;
        }
        private bool EmailValidation(string email, ref string status)
        {
            if (email.Length == 0)
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
        private bool PasswordValidation(string password, string confirmPassword, ref string status)
        {
            if (password.Length == 0)
            {
                status = "Enter password.";
                return false;
            }
            else if (confirmPassword.Length == 0)
            {
                status = "Enter Confirm password.";
                return false;
            }
            else if (password != confirmPassword)
            {
                status = "Confirm password must be same as password.";
                return false;
            }
            return true;
        }
        private bool PhoneNumberValidation(string phoneNumber, ref string status)
        {
            int index = 0;
            if (phoneNumber[0] == '+')
                index++;
            while (index < phoneNumber.Length)
            {
                if (phoneNumber[index] <= '0' || phoneNumber[index] >= '9')
                {
                    status = "Enter a valid phone number.";
                    return false;
                }
                index++;
            }
            return true;
        }

        public bool UserInfoValidationResult(User user,string confirmPassword,out string status)
        {
            status = "";
            if (!FirstNameValidation(user.FirstName, ref status))
                return false;
            else if (!LastNameValidation(user.LastName, ref status))
                return false;
            else if (!UserNameValidation(user.UserName, ref status))
                return false;
            else if (!DateOfBirthValidation(user.DateOfBirth, ref status))
                return false;
            else if (!GenderValidation(user.Gender, ref status))
                return false;
            else if (!EmailValidation(user.Email, ref status))
                return false;
            else if (!PasswordValidation(user.Password, confirmPassword, ref status))
                return false;
            else if (!PhoneNumberValidation(user.PhoneNumber, ref status))
                return false;
            return true;
        }
    }
}
