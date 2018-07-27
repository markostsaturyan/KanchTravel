using System.ComponentModel;

namespace Kanch.ViewModel
{
    class RegistrationViewModel:INotifyPropertyChanged
    {
        private PhotographerViewModel photographerViewModel;
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

        private DriverViewModel driverViewModel;
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
        private GuideViewModel guideViewModel;
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

        public RegistrationViewModel()
        {
            photographerViewModel = new PhotographerViewModel();
            guideViewModel = new GuideViewModel();
            driverViewModel = new DriverViewModel();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }
}
