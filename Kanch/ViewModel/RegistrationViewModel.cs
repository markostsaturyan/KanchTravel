using System;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Kanch.Commands;
using Kanch.DataModel;
using Kanch.Views;
using Newtonsoft.Json;
using Kanch.Views;

namespace Kanch.ViewModel
{
    class RegistrationViewModel : INotifyPropertyChanged
    {
        private PhotographerViewModel photographerViewModel;

        private DriverViewModel driverViewModel;

        private GuideViewModel guideViewModel;

        private UserViewModel userViewModel;

        private PhotographerRegistration photographerRegistration;

        private DriverRegistration driverRegistration;

        private GuideRegistration guideRegistration;

        private string errorMessage;

        public ICommand ResetCommand { get; set; }

        public string ErrorMessage
        {
            get { return this.errorMessage; }
            set
            {
                if (this.errorMessage != value)
                {
                    this.errorMessage = value;
                    NotifyPropertyChanged("ErrorMessage");
                }
            }
        }

        public ICommand SubmitCommand { get; set; }

        public PhotographerViewModel PhotographerViewModel
        {
            get { return this.photographerViewModel; }
            set
            {
                if (this.photographerViewModel != null)
                {
                    this.photographerViewModel = value;
                    this.NotifyPropertyChanged("PhotographerViewModel");
                }
            }
        }

        public DriverViewModel DriverViewModel
        {
            get { return this.driverViewModel; }
            set
            {
                if (this.driverViewModel != null)
                {
                    this.driverViewModel = value;
                    this.NotifyPropertyChanged("DriverViewModel");
                }
            }
        }

        public GuideViewModel GuideViewModel
        {
            get { return this.guideViewModel; }
            set
            {
                if (this.guideViewModel != null)
                {
                    if (this.guideViewModel != null)
                    {
                        this.guideViewModel = value;
                        this.NotifyPropertyChanged("GuideViewModel");
                    }
                }
            }
        }

        public UserViewModel UserViewModel
        {
            get { return this.userViewModel; }
            set
            {
                if(this.userViewModel != null)
                {
                    this.userViewModel = value;
                    this.NotifyPropertyChanged("UserViewModel");
                }
            }
        }

        public DriverRegistration DriverRegistration
        {
            get { return this.driverRegistration; }
            set
            {
                if(this.driverRegistration != null)
                {
                    this.driverRegistration = value;
                    this.NotifyPropertyChanged("DriverRegistration");
                }
            }
        }

        public PhotographerRegistration PhotographerRegistration
        {
            get { return this.photographerRegistration; }
            set
            {
                if(this.photographerRegistration != null)
                {
                    this.photographerRegistration = value;
                    this.NotifyPropertyChanged("PhotographerRegistration");
                }
            }
        }

        public GuideRegistration GuideRegistration
        {
            get { return this.guideRegistration; }
            set
            {
                if(this.guideRegistration != null)
                {
                    this.guideRegistration = value;
                    this.NotifyPropertyChanged("GuideRegistration");
                }
            }
        }

        public RegistrationViewModel()
        {
            photographerViewModel = new PhotographerViewModel();
            guideViewModel = new GuideViewModel();
            driverViewModel = new DriverViewModel();
            userViewModel = new UserViewModel();
            photographerRegistration = new PhotographerRegistration();
            guideRegistration = new GuideRegistration();
            driverRegistration = new DriverRegistration();
            ResetCommand = new Command((o) => Reset());
            SubmitCommand = new Command((o) => Submit());
        }

        private void Reset()
        {
            this.DriverViewModel.Reset();
            this.PhotographerViewModel.Reset();
            this.GuideViewModel.Reset();
            this.UserViewModel.Reset();
        }

        private async Task Submit()
        {
            if (RegistrationValidation())
            {
                if (await RegisterAsUser())
                    return;
                ErrorMessage = "It's ok.";
                Reset();
            }

        }

        private async Task<bool> RegisterAsUser()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationSettings.AppSettings["baseUrl"]);
            var user = new Photographer()
            {
                FirstName = UserViewModel.FirstName,
                LastName = UserViewModel.LastName,
                UserName = UserViewModel.UserName,
                Email = UserViewModel.Email,
                DateOfBirth = UserViewModel.DateOfBirth,
                PhoneNumber = UserViewModel.PhoneNumber,
                Password = UserViewModel.Password,
            };
            if (UserViewModel.Male == true)
            {
                user.Gender = "Male";
            }
            else
            {
                user.Gender = "Female";
            }

            var requestResult =  await client.PostAsync("api/User", new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"));
            var content = requestResult.Content;

            var jsonContent = content.ReadAsStringAsync().Result;

            var status = JsonConvert.DeserializeObject<Status>(jsonContent);
            if (!status.IsOk)
            {
                ErrorMessage = status.Message;
                return false;
            }
            return true;
        }

        private async Task<bool> RegisterAsGuide()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationSettings.AppSettings["baseUrl"]);
            var guide = new Photographer()
            {
                FirstName = UserViewModel.FirstName,
                LastName = UserViewModel.LastName,
                UserName = UserViewModel.UserName,
                Email = UserViewModel.Email,
                DateOfBirth = UserViewModel.DateOfBirth,
                PhoneNumber = UserViewModel.PhoneNumber,
                Password = UserViewModel.Password,
                
            };

            if (UserViewModel.Male == true)
            {
                guide.Gender = "Male";
            }
            else
            {
                guide.Gender = "Female";
            }

            var requestResult = await client.PostAsync("api/Guide", new StringContent(JsonConvert.SerializeObject(guide), Encoding.UTF8, "application/json"));
            var content = requestResult.Content;

            var jsonContent = content.ReadAsStringAsync().Result;

            var status = JsonConvert.DeserializeObject<Status>(jsonContent);
            if (!status.IsOk)
            {
                ErrorMessage = status.Message;
                return false;
            }
            return true;

        }

        private Bitmap BitmapImage2Bitmap(ImageSource imageSource)
        {
            // BitmapImage bitmapImage = new BitmapImage(new Uri("../Images/test.png", UriKind.Relative));

            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                BitmapImage bitmapImage = imageSource as BitmapImage;
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);

                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);
                BitmapSource bmp = (BitmapSource)bitmapImage;
                //...
                return bitmap;
            }
        }

        private async Task<bool> RegisterAsPhotographer()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationSettings.AppSettings["baseUrl"]);
            var photographer = new Photographer()
            {
                FirstName = UserViewModel.FirstName,
                LastName = UserViewModel.LastName,
                UserName = UserViewModel.UserName,
                Email = UserViewModel.Email,
                DateOfBirth = UserViewModel.DateOfBirth,
                PhoneNumber = UserViewModel.PhoneNumber,
                Password = UserViewModel.Password,
            };
            if (UserViewModel.Male == true)
            {
                photographer.Gender = "Male";
            }
            else
            {
                photographer.Gender = "Female";
            }

            var requestResult = await client.PostAsync("api/Photographer", new StringContent(JsonConvert.SerializeObject(photographer), Encoding.UTF8, "application/json"));
            var content = requestResult.Content;

            var jsonContent = content.ReadAsStringAsync().Result;

            var status = JsonConvert.DeserializeObject<Status>(jsonContent);
            if (!status.IsOk)
            {
                ErrorMessage = status.Message;
                return false;
            }
            return true;
        }

        private async Task<bool> RegisterAsDriver()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationSettings.AppSettings["baseUrl"]);
            var driver = new Driver()
            {
                FirstName = UserViewModel.FirstName,
                LastName = UserViewModel.LastName,
                UserName = UserViewModel.UserName,
                Email = UserViewModel.Email,
                DateOfBirth = UserViewModel.DateOfBirth,
                PhoneNumber = UserViewModel.PhoneNumber,
                Password = UserViewModel.Password,
                Car = new Car()
                {
                    Brand = DriverViewModel.BrandOfCar,
                    NumberOfSeats = DriverViewModel.NumberOfSeats,
                    FuelType = DriverViewModel.FuelType,
                    HasWiFi = DriverViewModel.Languages[0].IsSelected,
                    HasMicrophone = DriverViewModel.Languages[1].IsSelected,
                    HasAirConditioner = DriverViewModel.Languages[2].IsSelected,
                    HasKitchen = DriverViewModel.Languages[3].IsSelected,
                    HasToilet = DriverViewModel.Languages[4].IsSelected,
                    LicensePlate = DriverViewModel.LicensePlate
                }
        };
          /*  if (DriverViewModel.CarPicture1 != DriverViewModel.AddImage)
                driver.Car.CarPicture1 = BitmapImage2Bitmap(DriverViewModel.CarPicture1);
            if (DriverViewModel.CarPicture2 != DriverViewModel.AddImage)
                driver.Car.CarPicture2 = BitmapImage2Bitmap(DriverViewModel.CarPicture2);
            if (DriverViewModel.CarPicture3 != DriverViewModel.AddImage)
                driver.Car.CarPicture3 = BitmapImage2Bitmap(DriverViewModel.CarPicture3);
            if (DriverViewModel.DrivingLicencePicBack != DriverViewModel.AddImage)
                driver.DrivingLicencePicBack = BitmapImage2Bitmap(DriverViewModel.DrivingLicencePicBack);
            if (DriverViewModel.DrivingLicencePicFront != DriverViewModel.AddImage)
                driver.DrivingLicencePicFront = BitmapImage2Bitmap(DriverViewModel.DrivingLicencePicFront);
            if (UserViewModel.Male == true)
            {
                driver.Gender = "Male";
            }
            else
            {
                driver.Gender = "Female";
            }*/

            var requestResult = await client.PostAsync("api/Driver", new StringContent(JsonConvert.SerializeObject(driver), Encoding.UTF8, "application/json"));
            var content = requestResult.Content;

            var jsonContent = content.ReadAsStringAsync().Result;

            var status = JsonConvert.DeserializeObject<Status>(jsonContent);
            if (!status.IsOk)
            {
                ErrorMessage = status.Message;
                return false;
            }
            return true;

        }

        public bool RegistrationValidation()
        {
            

            return true;
        }

        public static Bitmap BitmapSourceToBitmap(BitmapSource srs)
        {
            int width = srs.PixelWidth;
            int height = srs.PixelHeight;
            int stride = width * ((srs.Format.BitsPerPixel + 7) / 8);
            IntPtr ptr = IntPtr.Zero;
            try
            {
                ptr = Marshal.AllocHGlobal(height * stride);
                srs.CopyPixels(new Int32Rect(0, 0, width, height), ptr, height * stride, stride);
                using (var btm = new System.Drawing.Bitmap(width, height, stride, System.Drawing.Imaging.PixelFormat.Format1bppIndexed, ptr))
                {
                    // Clone the bitmap so that we can dispose it and
                    // release the unmanaged memory at ptr
                    return new System.Drawing.Bitmap(btm);
                }
            }
            finally
            {
                if (ptr != IntPtr.Zero)
                    Marshal.FreeHGlobal(ptr);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }
}
