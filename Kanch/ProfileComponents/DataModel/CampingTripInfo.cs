using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Kanch.ProfileComponents.DataModel
{
    public class CampingTripInfo:INotifyPropertyChanged
    {
        private bool canIJoin;

        private bool iAmJoined;

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
        private ObservableCollection<FoodInfo> food;
        private List<string> direction;
        private DriverInfo driver;

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

        public List<string> Direction
        {
            get { return this.direction; }
            set
            {
                this.direction = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Direction"));
            }
        }

        public TypeOfCampingTrip TypeOfTrip
        {
            get { return this.typeOfCampingTrip; }
            set
            {
                this.typeOfCampingTrip = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TypeOfTrip"));
            }
        }

        public TypeOfOrganization OrganizationType { get; set; }

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

        public ObservableCollection<FoodInfo> Food
        {
            get { return this.food; }
            set
            {
                this.food = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Food"));
            }
        }

        public double PriceOfTrip
        {
            get { return this.price; }
            set
            {
                if (price != value)
                {
                    price = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PriceOfTrip"));
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

        public UserInfo Organizer { get; set; }

        public bool IsRegistrationCompleted { get; set; }

        public ObservableCollection<UserInfo> MembersOfCampingTrip { get; set; }

        public bool CanIJoin
        {
            get { return this.canIJoin; }
            set
            {
                this.canIJoin = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CanIJoin"));
            }
        }

        public bool IAmJoined
        {
            get { return this.iAmJoined; }
            set
            {
                this.iAmJoined = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IAmJoined"));
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

        public string Status { get; set; }

        public bool HasGuide { get; set; }

        public bool HasPhotographer { get; set; }

        public DriverInfo Driver { get { return this.driver; }


            set {
                this.driver = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Driver"));
            } }

        public GuideInfo Guide { get; set; }

        public PhotographerInfo Photographer { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
