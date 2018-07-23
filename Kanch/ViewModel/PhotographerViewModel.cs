using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public PhotographerViewModel()
        {
            this.photographerVisible = Visibility.Collapsed;
        }

    }
}
