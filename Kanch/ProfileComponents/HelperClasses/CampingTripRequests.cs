using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Kanch.ProfileComponents.DataModel;

namespace Kanch.ProfileComponents.HelperClasses
{
    public class CampingTripRequests:INotifyPropertyChanged
    {
        private double price;

        public string ID { get; set; }

        public string Place { get; set; }

        public DateTime DepartureDate { get; set; }

        public DateTime ArrivalDate { get; set; }

        public List<string> Direction { get; set; }

        public TypeOfCampingTrip TypeOfTrip { get; set; }

        public int MinAge { get; set; }

        public int MaxAge { get; set; }

        public int MinCountOfMembers { get; set; }

        public int MaxCountOfMembers { get; set; }

        public int CountOfMembers { get; set; }

        public DriverInfo Driver { get; set; }

        public GuideInfo Guide { get; set; }

        public PhotographerInfo Photographer { get; set; }

        public double Price
        {

            get { return this.price; }
            set
            {
                if (price != value)
                {
                    price = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("InputDirection"));
                }
                if (price != 0)
                {
                    this.IsActive = true;
                }
            }
        }

        public bool IsActive { get; set; }

        public ICommand Accept { get; set; }

        public ICommand Ignore { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
