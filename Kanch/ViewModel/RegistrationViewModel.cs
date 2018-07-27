using System.ComponentModel;
using System.Windows.Input;
using Kanch.Commands;

namespace Kanch.ViewModel
{
    class RegistrationViewModel:INotifyPropertyChanged
    {
        private PhotographerViewModel photographerViewModel;

        private DriverViewModel driverViewModel;

        private GuideViewModel guideViewModel;

        private UserViewModel userViewModel;

        private PhotographerRegistration photographerRegistration;

        private DriverRegistration driverRegistration;

        private GuideRegistration guideRegistration;

        private string errorMessage;

        public ICommand ResetCommand { get; set; }

        public string ErrorMessage
        {
            get { return this.errorMessage; }
            set
            {
                if (this.errorMessage == value)
                {
                    this.errorMessage = value;
                    NotifyPropertyChanged("ErrorMessage");
                }
            }
        }

        public PhotographerViewModel PhotographerViewModel
        {
            get { return this.photographerViewModel; }
            set
            {
                if (this.photographerViewModel != null)
                {
                    this.photographerViewModel = value;
                    this.NotifyPropertyChanged("PhotographerViewModel");
                }
            }
        }

        public DriverViewModel DriverViewModel
        {
            get { return this.driverViewModel; }
            set
            {
                if (this.driverViewModel != null)
                {
                    this.driverViewModel = value;
                    this.NotifyPropertyChanged("DriverViewModel");
                }
            }
        }

        public GuideViewModel GuideViewModel
        {
            get { return this.guideViewModel; }
            set
            {
                if (this.guideViewModel != null)
                {
                    if (this.guideViewModel != null)
                    {
                        this.guideViewModel = value;
                        this.NotifyPropertyChanged("GuideViewModel");
                    }
                }
            }
        }

        public UserViewModel UserViewModel
        {
            get { return this.userViewModel; }
            set
            {
                if(this.userViewModel != null)
                {
                    this.userViewModel = value;
                    this.NotifyPropertyChanged("UserViewModel");
                }
            }
        }

        public DriverRegistration DriverRegistration
        {
            get { return this.driverRegistration; }
            set
            {
                if(this.driverRegistration != null)
                {
                    this.driverRegistration = value;
                    this.NotifyPropertyChanged("DriverRegistration");
                }
            }
        }

        public PhotographerRegistration PhotographerRegistration
        {
            get { return this.photographerRegistration; }
            set
            {
                if(this.photographerRegistration != null)
                {
                    this.photographerRegistration = value;
                    this.NotifyPropertyChanged("PhotographerRegistration");
                }
            }
        }

        public GuideRegistration GuideRegistration
        {
            get { return this.guideRegistration; }
            set
            {
                if(this.guideRegistration != null)
                {
                    this.guideRegistration = value;
                    this.NotifyPropertyChanged("GuideRegistration");
                }
            }
        }

        public RegistrationViewModel()
        {
            photographerViewModel = new PhotographerViewModel();
            guideViewModel = new GuideViewModel();
            driverViewModel = new DriverViewModel();
            userViewModel = new UserViewModel();
            photographerRegistration = new PhotographerRegistration();
            guideRegistration = new GuideRegistration();
            driverRegistration = new DriverRegistration();
            ResetCommand = new Command((o) => Reset());
        }

        public void Reset()
        {
            this.DriverViewModel.Reset();
            this.PhotographerViewModel.Reset();
            this.GuideViewModel.Reset();
            this.UserViewModel.Reset();
        }

        public bool RegistrationValidation()
        {
            if(!UserViewModel.UserInfoValidationResult(out string userErrorMessage))
            {
                this.ErrorMessage = userErrorMessage;
                return false;
            }

            if (DriverViewModel.DriverVisible == Visibility.Visible)
            {
                if(!DriverViewModel.DriverInfoValidation(out string driverErrorMessage))
                {
                    this.ErrorMessage = driverErrorMessage;
                    return false;
                }
            }

            if(GuideViewModel.GuideVisible == Visibility.Visible)
            {
                if(!GuideViewModel.GuideInfoValidation(out string guideErrorMessage))
                {
                    this.ErrorMessage = guideErrorMessage;
                    return false;
                }
            }

            if(PhotographerViewModel.PhotographerVisible == Visibility.Visible)
            {
                if(!PhotographerViewModel.PhotographerInfoValidation(out string photographerErrorMessage))
                {
                    this.ErrorMessage = photographerErrorMessage;
                    return false;
                }
            }

            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }
}
