using Kanch.DataModel;
using Kanch.ProfileComponents.DataModel;
using Kanch.ProfileComponents.Utilities;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Net.Http;

namespace Kanch.ProfileComponents.ViewModels
{
    public class PhotographerViewModel : INotifyPropertyChanged
    {
        public PhotographerViewModel() { }


        public event PropertyChangedEventHandler PropertyChanged;

        private HttpClient httpClient;

        public PhotographerInfo photographer;

        public PhotographerInfo Photographer
        {
            get
            {
                return this.photographer;
            }

            set
            {
                this.photographer = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Photographer"));
            }
        }

        public List<CampingTripInfo> campingTrips;

        public List<CampingTripInfo> CampingTrips
        {
            get
            {
                return this.campingTrips;
            }

            set
            {
                this.campingTrips = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CampingTrips"));
            }
        }

        public async void GetPhotographerInfoAsync()
        {
            var response = await httpClient.GetAsync("api/Photographer/" + ConfigurationSettings.AppSettings["userId"]);

            var content = response.Content;

            var jsonContent = await content.ReadAsStringAsync();

            var photographer = JsonConvert.DeserializeObject<Photographer>(jsonContent);

            var photographerInfo = new PhotographerInfo
            {
                Id = photographer.Id,
                FirstName = photographer.FirstName,
                LastName = photographer.LastName,
                Gender = photographer.Gender,
                Email = photographer.Email,
                PhoneNumber = photographer.PhoneNumber,
                DateOfBirth = photographer.DateOfBirth,
                Image = ImageConverter.ConvertImageToImageSource(photographer.Image),
                UserName = photographer.UserName,
                Camera = new CameraInfo
                {
                    Id = photographer.Camera.Id,
                    IsProfessional = photographer.Camera.IsProfessional,
                    Model = photographer.Camera.Model,
                },
                HasCameraStabilizator = photographer.HasCameraStabilizator,
                HasDron = photographer.HasDron,
                HasGopro = photographer.HasGopro,
                KnowledgeOfLanguages = photographer.KnowledgeOfLanguages,
                Profession = photographer.Profession,
                NumberOfAppraisers = photographer.NumberOfAppraisers,
                WorkExperience = photographer.WorkExperience,
                Raiting = photographer.Raiting
            };

            Photographer = photographerInfo;
        }

        public async void JoinToTripAsync(object campingTripId)
        {
            var tripId = campingTripId as string;

            if (tripId == null) return;

            await httpClient.PutAsync("api/MembersOfCampingTrip/" + Photographer.Id, new StringContent(tripId));
        }

        public async void GetCampingTripsAsync()
        {
            var response = await httpClient.GetAsync("api/CampingTrips");

            var content = response.Content;

            var jsonContent = await content.ReadAsStringAsync();

            CampingTrips = JsonConvert.DeserializeObject<List<CampingTripInfo>>(jsonContent);
        }
    }
}
