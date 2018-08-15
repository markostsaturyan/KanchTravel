using Kanch.DataModel;
using Kanch.ProfileComponents.DataModel;
using Kanch.ProfileComponents.HelperClasses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Net.Http;

namespace Kanch
{
    class CampingTripsForHomePageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private HttpClient httpClient;
        private ObservableCollection<TripsInProgress> tripsInProgresses;
        public ObservableCollection<TripsInProgress> TripsInProgress
        {
            get { return this.tripsInProgresses; }
            set
            {
                this.tripsInProgresses = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TripsInProgress"));
            }
        }

        public CampingTripsForHomePageViewModel()
        {
            this.httpClient = new HttpClient();
            this.httpClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["baseUrl"]);
            this.TripsInProgress = new ObservableCollection<TripsInProgress>();
            GetAllInProgressTrips();
        }

        public void GetAllInProgressTrips()
        {
            var response = httpClient.GetAsync("api/CampingTrips").Result;

            var content = response.Content;

            var jsonContent = content.ReadAsStringAsync().Result;

            var trips = JsonConvert.DeserializeObject<List<CampingTrip>>(jsonContent);

            var campingTrips = new ObservableCollection<TripsInProgress>();

            if (trips != null)
            {
                foreach (var trip in trips)
                {
                    var campingtrip = new CampingTripInfo()
                    {
                        Place = trip.Place,
                        DepartureDate = trip.DepartureDate,
                        ArrivalDate = trip.ArrivalDate,
                        CountOfMembers = trip.CountOfMembers,
                        MinAge = trip.MinAge,
                        MaxAge = trip.MaxAge,
                        MinCountOfMembers = trip.MinCountOfMembers,
                        MaxCountOfMembers = trip.MaxCountOfMembers,
                        Direction = trip.Direction,
                        HasGuide = trip.HasGuide,
                        HasPhotographer = trip.HasPhotographer,
                        ID = trip.ID,
                        PriceOfTrip = trip.PriceOfTrip
                    };
                    if (trip.TypeOfTrip == Kanch.DataModel.TypeOfCampingTrip.Campaign)
                    {
                        campingtrip.TypeOfTrip = ProfileComponents.DataModel.TypeOfCampingTrip.Campaign;
                    }
                    else if (trip.TypeOfTrip == Kanch.DataModel.TypeOfCampingTrip.CampingTrip)
                    {
                        campingtrip.TypeOfTrip = ProfileComponents.DataModel.TypeOfCampingTrip.CampingTrip;
                    }
                    else
                    {
                        campingtrip.TypeOfTrip = ProfileComponents.DataModel.TypeOfCampingTrip.Excursion;
                    }
                    if (trip.Food != null)
                    {
                        campingtrip.Food = new ObservableCollection<FoodInfo>();
                        foreach (var food in trip.Food)
                        {
                            campingtrip.Food.Add(new FoodInfo()
                            {
                                Name = food.Name,
                                Measure = food.Measure,
                                MeasurementUnit = food.MeasurementUnit
                            });
                        }
                    }

                    var tripInProgress = new TripsInProgress();

                    tripInProgress.CampingTrip = campingtrip;

                    this.TripsInProgress.Add(tripInProgress);
                }
            }
        }
    }
}
