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
        private bool isActive;
        private string place;
        private DateTime dapartureDate;
        private DateTime arrivalDate;
        private TypeOfCampingTrip typeOfCampingTrip;
        public int minAge;
        public int maxAge;
        public int minCountOfMembers;
        public int maxCountOfMembers;
        public int countOfMembers;


        public string ID { get; set; }

        public string Place
        {
            get { return this.place; }
            set
            {
                this.place = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Place"));
            }
        }

        public DateTime DepartureDate
        {
            get { return this.dapartureDate; }
            set
            {
                this.dapartureDate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DepartureDate"));
            }
        }

        public DateTime ArrivalDate
        {
            get { return this.arrivalDate; }
            set
            {
                this.arrivalDate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ArriavalDate"));
            }
        }

        public List<string> Direction { get; set; }

        public TypeOfCampingTrip TypeOfTrip
        {
            get { return this.typeOfCampingTrip; }
            set
            {
                this.typeOfCampingTrip = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TypeOfTrip"));
            }
        }

        public int MinAge
        {
            get { return this.minAge; }
            set
            {
                this.minAge = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MinAge"));
            }
        }

        public int MaxAge
        {
            get { return this.maxAge; }
            set
            {
                this.maxAge = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MaxAge"));
            }
        }

        public int MinCountOfMembers
        {
            get { return this.minCountOfMembers; }
            set
            {
                this.minCountOfMembers = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MinCountOfMembers"));
            }
        }

        public int MaxCountOfMembers
        {
            get { return this.maxCountOfMembers; }
            set
            {
                this.maxCountOfMembers = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MaxCountOfMembers"));
            }
        }

        public int CountOfMembers
        {
            get { return this.countOfMembers; }
            set
            {
                this.countOfMembers = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CountOfMembers"));
            }
        }

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
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Price"));
                }
                if (price != 0)
                {
                    this.IsActive = true;
                }
                else
                {
                    this.IsActive = false;
                }
            }
        }

        public bool IsActive
        {
            get { return this.isActive; }
            set
            {
                this.isActive = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsActive"));
            }
        }

        public ICommand Accept { get; set; }

        public ICommand Ignore { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
