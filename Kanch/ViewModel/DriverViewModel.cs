﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kanch.ViewModel
{
    public class DriverViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Visibility driverVisible;

        public Visibility DriverVisible
        {
            get
            {
                return driverVisible;
            }

            set
            {
                if (driverVisible != value)
                {
                    driverVisible = value;
                    NotifyPropertyChanged("DriverVisible");
                }
            }
        }

        public DriverViewModel()
        {
            this.driverVisible = Visibility.Collapsed;
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