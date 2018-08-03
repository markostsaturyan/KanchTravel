using Kanch.DataModel;
using Kanch.ProfileComponents.DataModel;
using Kanch.ProfileComponents.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Kanch.ProfileComponents.ViewModels
{
    public class GuideViewModel : INotifyPropertyChanged
    {
        public GuideViewModel() { }

        public event PropertyChangedEventHandler PropertyChanged;

        private HttpClient httpClient;


        public GuideInfo guide;

        public GuideInfo Guide
        {
            get
            {
                return this.guide;
            }

            set
            {
                this.guide = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Guide"));
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

        public async void GetGuideInfoAsync()
        {
            var response = await httpClient.GetAsync("api/Guide/" + ConfigurationSettings.AppSettings["userId"]);

            var content = response.Content;

            var jsonContent = await content.ReadAsStringAsync();

            var guide = JsonConvert.DeserializeObject<Guide>(jsonContent);

            var guideInfo = new GuideInfo
            {
                Id = guide.Id,
                FirstName = guide.FirstName,
                LastName = guide.LastName,
                Gender = guide.Gender,
                DateOfBirth = guide.DateOfBirth,
                Email = guide.Email,
               // Image = ImageConverter.ConvertImageToImageSource(guide.Image),
                EducationGrade = guide.EducationGrade,
                PhoneNumber = guide.PhoneNumber,
                KnowledgeOfLanguages = guide.KnowledgeOfLanguages,
                NumberOfAppraisers = guide.NumberOfAppraisers,
                UserName = guide.UserName,
                Profession = guide.Profession,
                Places = guide.Places,
                Rating = guide.Rating,
                WorkExperience = guide.WorkExperience
            };

            Guide = guideInfo;
        }

        public async void JoinToTripAsync(object campingTripId)
        {
            var tripId = campingTripId as string;

            if (tripId == null) return;

            await httpClient.PutAsync("api/MembersOfCampingTrip/" + Guide.Id, new StringContent(tripId));
        }
    }
}
