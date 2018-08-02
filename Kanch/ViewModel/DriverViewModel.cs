using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Kanch.Commands;
using Kanch.DataModel;
using Newtonsoft.Json;

namespace Kanch.ViewModel
{
    public class DriverViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string errorMessage;
        private string firstName;
        private string lastName;
        private string userName;
        private string email;
        private bool? male;
        private bool? female;
        private DateTime dateOfBirth;
        private string phoneNumber;
        private string password;
        private string confirmPassword;
        private string brandOfCar;
        private int numberOfSeats;
        private string fuelType;
        private string licensePlate;
        private List<ListItem> moreInformation;
        private ImageSource carPicture1;
        private ImageSource carPicture2;
        private ImageSource carPicture3;
        private List<ListItem> languages;
        private ImageSource drivingLicencePicFront;
        private ImageSource drivingLicencePicBack;

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

        public string FirstName
        {
            get
            {
                return this.firstName;
            }
            set
            {
                if (this.firstName != value)
                {
                    this.firstName = value;
                    NotifyPropertyChanged("FirstName");
                }
            }
        }

        public string LastName
        {
            get
            {
                return this.lastName;
            }
            set
            {
                if (this.lastName != value)
                {
                    this.lastName = value;
                    NotifyPropertyChanged("LastName");
                }
            }
        }

        public string UserName
        {
            get
            {
                return this.userName;
            }
            set
            {
                if (this.userName != value)
                {
                    this.userName = value;
                    NotifyPropertyChanged("UserName");
                }
            }
        }

        public string Email
        {
            get { return this.email; }
            set
            {
                if (this.email != value)
                {
                    this.email = value;
                    NotifyPropertyChanged("Email");
                }
            }
        }

        public DateTime DateOfBirth
        {
            get { return this.dateOfBirth; }
            set
            {
                if (this.dateOfBirth != value)
                {
                    this.dateOfBirth = value;
                    NotifyPropertyChanged("DateOfBirth");
                }
            }
        }

        public bool? Male
        {
            get { return this.male; }
            set
            {
                this.male = value;
                NotifyPropertyChanged("Male");
            }
        }
        public bool? Female
        {
            get { return this.female; }
            set
            {
                this.female = value;
                NotifyPropertyChanged("Female");
            }
        }

        public string PhoneNumber
        {
            get
            {
                return this.phoneNumber;
            }
            set
            {
                if (this.phoneNumber != value)
                {
                    this.phoneNumber = value;
                    NotifyPropertyChanged("PhoneNumber");
                }
            }
        }

        public string Password
        {
            get
            {
                return this.password;
            }
            set
            {
                if (this.password != value)
                {
                    this.password = value;
                    NotifyPropertyChanged("Password");
                }
            }
        }

        public string ConfirmPassword
        {
            get
            {
                return this.confirmPassword;
            }
            set
            {
                if (this.confirmPassword != value)
                {
                    this.confirmPassword = value;
                    NotifyPropertyChanged("ConfirmPassword");
                }
            }
        }

        public BitmapImage AddImage { get; set; }
        public ICommand AddCarPicture1 { get; set; }
        public ICommand AddCarPicture2 { get; set; }
        public ICommand AddCarPicture3 { get; set; }
        public ICommand AddDrivingLicencePicFront { get; set; }
        public ICommand AddDrivingLicencePicBack { get; set; }


        public string BrandOfCar
        {
            get
            {
                return this.brandOfCar;
            }
            set
            {
                if (this.brandOfCar != value)
                {
                    this.brandOfCar = value;
                    NotifyPropertyChanged("BrandOfCar");
                }
            }
        }

        public int NumberOfSeats
        {
            get
            {
                return this.numberOfSeats;
            }
            set
            {
                if (this.numberOfSeats != value)
                {
                    this.numberOfSeats = value;
                    NotifyPropertyChanged("NumberOfSeats");
                }
            }
        }

        public string FuelType
        {
            get
            {
                return this.fuelType;
            }
            set
            {
                if (this.fuelType != value)
                {
                    this.fuelType = value;
                    NotifyPropertyChanged("FuelType");
                }
            }
        }

        public string LicensePlate
        {
            get
            {
                return this.licensePlate;
            }
            set
            {
                if (this.licensePlate != value)
                {
                    this.licensePlate = value;
                    NotifyPropertyChanged("LicensePlate");
                }
            }
        }

        public List<ListItem> Languages
        {
            get { return this.languages; }
            set
            {
                if(this.languages != value)
                {
                    this.languages = value;
                    NotifyPropertyChanged("Languages");
                }
            }
        }

        public List<ListItem> MoreInformation
        {
            get { return this.moreInformation; }
            set
            {
                this.moreInformation = value;
                NotifyPropertyChanged("MoreInformation");
            }
        }

        public ImageSource CarPicture1
        {
            get { return this.carPicture1; }
            set
            {
                if (this.carPicture1 != value)
                {
                    this.carPicture1 = value;
                    NotifyPropertyChanged("CarPicture1");
                }
            }
        }

        public ImageSource CarPicture2
        {
            get { return this.carPicture2; }
            set
            {
                if (this.carPicture2 != value)
                {
                    this.carPicture2 = value;
                    NotifyPropertyChanged("CarPicture2");
                }
            }
        }

        public ImageSource CarPicture3
        {
            get { return this.carPicture3; }
            set
            {
                if (this.carPicture3 != value)
                {
                    this.carPicture3 = value;
                    NotifyPropertyChanged("CarPicture3");
                }
            }
        }

        public ImageSource DrivingLicencePicFront
        {
            get { return this.drivingLicencePicFront; }
            set
            {
                if (this.drivingLicencePicFront != value)
                {
                    this.drivingLicencePicFront = value;
                    NotifyPropertyChanged("DrivingLicencePicFront");
                }
            }
        }

        public ImageSource DrivingLicencePicBack
        {
            get { return this.drivingLicencePicBack; }
            set
            {
                if (this.drivingLicencePicBack != value)
                {
                    this.drivingLicencePicBack = value;
                    NotifyPropertyChanged("DrivingLicencePicBack");
                }
            }
        }

        public ICommand SubmitCommand { get; set; }

        public ICommand ResetCommand { get; set; }

        public DriverViewModel()
        {

            this.AddImage = new BitmapImage(new Uri(String.Format("Images/addPhoto.png"), UriKind.Relative));
            AddImage.Freeze();

            this.Languages = new List<ListItem>();
            this.Languages.Add(new ListItem() { Text = "Armenian", IsSelected = false });
            this.Languages.Add(new ListItem() { Text = "Russian", IsSelected = false });
            this.Languages.Add(new ListItem() { Text = "English", IsSelected = false });
            this.Languages.Add(new ListItem() { Text = "German", IsSelected = false });
            this.Languages.Add(new ListItem() { Text = "Italian", IsSelected = false });
            this.Languages.Add(new ListItem() { Text = "French", IsSelected = false });
            this.CarPicture1 = AddImage;
            this.CarPicture2 = AddImage;
            this.CarPicture3 = AddImage;
            this.DrivingLicencePicBack = AddImage;
            this.DrivingLicencePicFront = AddImage;

            this.MoreInformation = new List<ListItem>();
            this.MoreInformation.Add(new ListItem() { Text = "HasWiFi", IsSelected = false });
            this.MoreInformation.Add(new ListItem() { Text = "HasMicrophone", IsSelected = false });
            this.MoreInformation.Add(new ListItem() { Text = "HasAirConditioner", IsSelected = false });
            this.MoreInformation.Add(new ListItem() { Text = "HasKitchen", IsSelected = false });
            this.MoreInformation.Add(new ListItem() { Text = "HasToilet", IsSelected = false });
            this.AddCarPicture1 = new Command(AddCarPhoto);
            this.AddCarPicture2 = new Command(AddCarPhoto);
            this.AddCarPicture3 = new Command(AddCarPhoto);
            this.AddDrivingLicencePicBack = new Command(AddDrivingLicencePic);
            this.AddDrivingLicencePicFront = new Command(AddDrivingLicencePic);

            this.SubmitCommand = new Command((o) => Submit());
            this.ResetCommand = new Command((o) => Reset());
        }

        private async void Submit()
        {
            if (DriverInfoValidationResult())
            {
                if (await Registration())
                {
                    var verification = new Verification();
                    var myWindow = System.Windows.Application.Current.MainWindow;
                    verification.Show();
                    myWindow.Close();
                }
            }

            return;
        }


        private void AddDrivingLicencePic(object obj)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            //For any other formats
            fileDialog.Filter = "Image Files (*.bmp;*.jpg;*.jpeg,*.png)|*.BMP;*.JPG;*.JPEG;*.PNG";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                if (obj.ToString() == "Front")
                    DrivingLicencePicFront = ConvertImageToImageSource(Bitmap.FromFile(fileDialog.FileName));
                else if (obj.ToString() == "Back")
                    DrivingLicencePicBack = ConvertImageToImageSource(Bitmap.FromFile(fileDialog.FileName));

            }
        }

        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        private bool FirstNameValidation()
        {
            if (this.FirstName == null)
            {
                this.ErrorMessage = "Enter your first name.";
                return false;
            }
            return true;
        }

        private bool LastNameValidation()
        {
            if (this.LastName == null)
            {
                this.ErrorMessage = "Enter your last name";
                return false;
            }
            return true;
        }

        private bool UserNameValidation()
        {
            if (this.UserName == null)
            {
                this.ErrorMessage = "Enter a user name";
                return false;
            }
            return true;
        }

        private bool DateOfBirthValidation()
        {
            if (this.DateOfBirth == null)
            {
                this.ErrorMessage = "Enter your date of birth";
                return false;
            }
            return true;
        }

        private bool GenderValidation()
        {
            if (this.male == null && this.female == null)
            {
                this.ErrorMessage = "Check your gender";
                return false;
            }
            return true;
        }

        private bool EmailValidation()
        {
            if (this.Email.Length == 0)
            {
                this.ErrorMessage = "Enter an email.";
                return false;
            }
            else if (!Regex.IsMatch(this.Email, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))

            {
                this.ErrorMessage = "Enter a valid email.";
                return false;
            }
            return true;
        }

        private bool PasswordValidation()
        {
            if (this.Password.Length == 0)
            {
                this.ErrorMessage = "Enter password.";
                return false;
            }
            else if (this.ConfirmPassword.Length == 0)
            {
                this.ErrorMessage = "Enter Confirm password.";
                return false;
            }
            else if (this.Password != this.ConfirmPassword)
            {
                this.ErrorMessage = "Confirm password must be same as password.";
                return false;
            }
            return true;
        }

        private bool PhoneNumberValidation()
        {
            int index = 0;
            if (this.PhoneNumber[0] == '+')
                index++;
            while (index < this.PhoneNumber.Length)
            {
                if (this.PhoneNumber[index] < '0' || this.PhoneNumber[index] > '9')
                {
                    this.ErrorMessage = "Enter a valid phone number.";
                    return false;
                }
                index++;
            }
            return true;
        }


        private bool BrandOfCarValidation()
        {
            if (this.BrandOfCar == null)
            {
                this.ErrorMessage = "Enter brand of the car.";
                return false;
            }
            return true;
        }

        private bool NumberOfSeatsValidation()
        {
            if (this.NumberOfSeats == 0)
            {
                this.ErrorMessage = "Enter the number of seats.";
                return false;
            }
            return true;
        }

        private bool FuelTypeValidation()
        {
            if (this.FuelType == null)
            {
                this.ErrorMessage = "Enter the fuel type.";
                return false;
            }
            return true;
        }

        private bool LicensePlateValidation()
        {
            if (this.LicensePlate == null)
            {
                this.ErrorMessage = "Enter the License plate";
                return false;
            }
            return true;
        }

        private bool CarPicturesValidation()
        {
            if (this.CarPicture1 == AddImage && this.CarPicture2 == AddImage && this.CarPicture3 == AddImage)
            {
                this.ErrorMessage = "Add at least one photo of the car.";
                return false;
            }
            return true;
        }

        private bool DrivingLicencePicValidation()
        {
            if (this.DrivingLicencePicFront == this.AddImage || this.DrivingLicencePicBack == this.AddImage)
            {
                this.ErrorMessage = "Add driving licence's back and front parts photos.";
                return false;
            }
            return true;
        }

        public bool DriverInfoValidationResult()
        {
            if (!FirstNameValidation())
                return false;
            else if (!LastNameValidation())
                return false;
            else if (!UserNameValidation())
                return false;
            else if (!DateOfBirthValidation())
                return false;
            else if (!GenderValidation())
                return false;
            else if (!EmailValidation())
                return false;
            else if (!PasswordValidation())
                return false;
            else if (!PhoneNumberValidation())
                return false;
            else if (!BrandOfCarValidation())
                return false;
            else if (!NumberOfSeatsValidation())
                return false;
            else if (!FuelTypeValidation())
                return false;
            else if (!LicensePlateValidation())
                return false;
            else if (!CarPicturesValidation())
                return false;
            else if (!DrivingLicencePicValidation())
                return false;
            return true;
        }

        public async Task<bool> Registration()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationSettings.AppSettings["baseUrl"]);
            var driver = new Driver()
            {
                FirstName = this.FirstName,
                LastName = this.LastName,
                UserName = this.UserName,
                Email = this.Email,
                DateOfBirth = this.DateOfBirth,
                PhoneNumber = this.PhoneNumber,
                Password = this.Password,
                Car = new Car()
                {
                    Brand = BrandOfCar,
                    NumberOfSeats = NumberOfSeats,
                    FuelType = FuelType,
                    HasWiFi = Languages[0].IsSelected,
                    HasMicrophone = Languages[1].IsSelected,
                    HasAirConditioner = Languages[2].IsSelected,
                    HasKitchen =  Languages[3].IsSelected,
                    HasToilet = Languages[4].IsSelected,
                    LicensePlate = LicensePlate
                },
                
            };
            if (Male == true)
            {
                driver.Gender = "Male";
            }
            else
            {
                driver.Gender = "Female";
            }

            if (this.CarPicture1 != this.AddImage)
                driver.Car.CarPicture1 = ImageSourceToBytes(this.CarPicture1);
            if (this.CarPicture2 != this.AddImage)
                driver.Car.CarPicture2 = ImageSourceToBytes(this.CarPicture2);
            if (this.CarPicture3 != this.AddImage)
                driver.Car.CarPicture3 = ImageSourceToBytes(this.CarPicture3);
            if (this.DrivingLicencePicBack != this.AddImage)
                driver.DrivingLicencePicBack = ImageSourceToBytes(this.DrivingLicencePicBack);
            if (this.DrivingLicencePicFront != this.AddImage)
                driver.DrivingLicencePicFront = ImageSourceToBytes(this.DrivingLicencePicFront);
            driver.KnowledgeOfLanguages = "";
            foreach(var language in this.Languages)
            {
                if (language.IsSelected)
                    driver.KnowledgeOfLanguages += language.Text + ',';
            }



            var requestResult = await client.PostAsync("api/Driver", new StringContent(JsonConvert.SerializeObject(driver), Encoding.UTF8, "application/json"));
            var content = requestResult.Content;

            var jsonContent = content.ReadAsStringAsync().Result;

            var status = JsonConvert.DeserializeObject<Status>(jsonContent);
            if (!status.IsOk)
            {
                this.ErrorMessage = status.Message;
                return false;
            }
            return true;
        }

        public void Reset()
        {
            this.FirstName = null;
            this.LastName = null;
            this.UserName = null;
            this.Email = null;
            this.PhoneNumber = null;
            this.Password = null;
            this.ConfirmPassword = null;
            this.Male = null;
            this.Female = null;
            this.BrandOfCar = "";
            this.NumberOfSeats = 0;
            this.FuelType = "";
            foreach (var info in this.MoreInformation)
            {
                info.IsSelected = false;
            }
            this.LicensePlate = null;
            foreach (var language in this.Languages)
            {
                language.IsSelected = false;
            }
            this.CarPicture1 = this.AddImage;
            this.CarPicture2 = this.AddImage;
            this.CarPicture3 = this.AddImage;
            this.DrivingLicencePicBack = this.AddImage;
            this.DrivingLicencePicFront = this.AddImage;

        }
        public void AddCarPhoto(object numberOfPhoto)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            //For any other formats
            fileDialog.Filter = "Image Files (*.bmp;*.jpg;*.jpeg,*.png)|*.BMP;*.JPG;*.JPEG;*.PNG";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                if (numberOfPhoto.ToString() == "1")
                    CarPicture1 = ConvertImageToImageSource(Bitmap.FromFile(fileDialog.FileName));
                else if (numberOfPhoto.ToString() == "2")
                    CarPicture2 = ConvertImageToImageSource(Bitmap.FromFile(fileDialog.FileName));
                else
                    CarPicture3 = ConvertImageToImageSource(Bitmap.FromFile(fileDialog.FileName));

            }
        }

        private ImageSource ConvertImageToImageSource(System.Drawing.Image image)
        {
            // ImageSource ...

            BitmapImage bi = new BitmapImage();

            bi.BeginInit();

            MemoryStream ms = new MemoryStream();

            // Save to a memory stream...

            image.Save(ms, ImageFormat.Bmp);

            // Rewind the stream...

            ms.Seek(0, SeekOrigin.Begin);

            // Tell the WPF image to use this stream...

            bi.StreamSource = ms;

            bi.EndInit();

            return bi;
        }

        public byte[] ImageSourceToBytes(ImageSource imageSource)
        {
            byte[] bytes = null;
            var bitmapSource = imageSource as BitmapSource;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            if (bitmapSource != null)
            {
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

                using (var stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    bytes = stream.ToArray();
                }
            }

            return bytes;
        }



        /*private Bitmap BitmapImage2Bitmap(ImageSource imageSource)
        {
            var i =new ImageSourceValueSerializer();


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
        }*/
        public class ListItem:INotifyPropertyChanged
        {
            private bool isSelected;
            public string Text { get; set; }

            public bool IsSelected
            {
                get { return this.isSelected; }
                set
                {
                    this.isSelected = value;
                    this.OnPropertyChanged("IsSelected");
                }
            }
            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged(string strPropertyName)
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(strPropertyName));
            }
        }


        
    }
}
