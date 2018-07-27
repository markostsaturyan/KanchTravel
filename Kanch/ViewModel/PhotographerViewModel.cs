using System.ComponentModel;
using System.Windows;

namespace Kanch.ViewModel
{
    public class PhotographerViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Visibility photographerVisible;

        public Visibility PhotographerVisible
        {
            get
            {
                return photographerVisible;
            }

            set
            {
                if (photographerVisible != value)
                {
                    photographerVisible = value;
                    NotifyPropertyChanged("PhotographerVisible");
                }
            }
        }
        private void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        public PhotographerViewModel()
        {
            this.photographerVisible = Visibility.Collapsed;
        }

    }
}
