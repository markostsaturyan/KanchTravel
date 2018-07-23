using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kanch.ViewModel
{
    public class GuideViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Visibility guideVisible;

        public Visibility GuideVisible
        {
            get
            {
                return guideVisible;
            }

            set
            {
                if (guideVisible != value)
                {
                    guideVisible = value;
                    NotifyPropertyChanged("GuideVisible");
                }
            }
        }


        public GuideViewModel()
        {
            this.GuideVisible = Visibility.Hidden;
        }

        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
