using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Kanch.Commands;
using Kanch.DataModel;

namespace Kanch.ViewModel
{
    public class DriverViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Visibility driverVisible;
        private Driver driver;
        private string brandOfCar;
        private int numberOfSeats;
        private string fuelType;
        private string licensePlate;
        private List<ListItem> moreInformation;
        private ImageSource carPicture1;
        private ImageSource carPicture2;
        private ImageSource carPicture3;
        private List<ListItem> languages;
        public ICommand AddCarPicture1 { get; set; }
        public ICommand AddCarPicture2 { get; set; }
        public ICommand AddCarPicture3 { get; set; }
        public Driver Driver
        {
            get
            {
                return driver;
            }
            set
            {
                if(driver != value)
                {
                    driver = value;
                    NotifyPropertyChanged("Driver");
                }
            }
        }

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

        public DriverViewModel()
        {

            var addImage = new BitmapImage(new Uri(String.Format("Images/addPhoto.png"), UriKind.Relative));
            addImage.Freeze();

            this.driverVisible = Visibility.Collapsed;
            this.Languages = new List<ListItem>();
            this.Languages.Add(new ListItem() { Text = "Armenian", IsSelected = false });
            this.Languages.Add(new ListItem() { Text = "Russian", IsSelected = false });
            this.Languages.Add(new ListItem() { Text = "English", IsSelected = false });
            this.Languages.Add(new ListItem() { Text = "German", IsSelected = false });
            this.Languages.Add(new ListItem() { Text = "Italian", IsSelected = false });
            this.Languages.Add(new ListItem() { Text = "French", IsSelected = false });
            this.CarPicture1 = addImage;
            this.CarPicture2 = addImage;
            this.CarPicture3 = addImage;

            this.MoreInformation = new List<ListItem>();
            this.MoreInformation.Add(new ListItem() { Text = "HasWiFi", IsSelected = false });
            this.MoreInformation.Add(new ListItem() { Text = "HasMicrophone", IsSelected = false });
            this.MoreInformation.Add(new ListItem() { Text = "HasAirConditioner", IsSelected = false });
            this.MoreInformation.Add(new ListItem() { Text = "HasKitchen", IsSelected = false });
            this.MoreInformation.Add(new ListItem() { Text = "HasToilet", IsSelected = false });
            this.AddCarPicture1 = new Command(AddCarPhoto);
            this.AddCarPicture2 = new Command(AddCarPhoto);
            this.AddCarPicture3 = new Command(AddCarPhoto);
        }

        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        private bool BrandOfCarValidation(ref string status)
        {
            if (BrandOfCar == null)
            {
                status = "Enter brand of the car.";
                return false;
            }
            return true;
        }
        private bool NumberOfSeatsValidation(ref string status)
        {
            if (NumberOfSeats == 0)
            {
                status = "Enter the number of seats.";
                return false;
            }
            return true;
        }
        private bool FuelTypeValidation(ref string status)
        {
            if (FuelType == null)
            {
                status = "Enter the fuel type.";
                return false;
            }
            return true;
        }
        private bool LicensePlateValidation(ref string status)
        {
            if (LicensePlate == null)
            {
                status = "Enter the License plate";
                return false;
            }
            return true;
        }
        private bool CarPicturesValidation(ref string status)
        {
            if (CarPicture1 == null && CarPicture2 == null && CarPicture3 == null)
            {
                status = "Add at least one photo of the car.";
                return false;
            }
            return true;
        }

        public bool DriverInfoValidation(out string status)
        {
            status = "";
            if (!BrandOfCarValidation(ref status))
                return false;
            else if (!NumberOfSeatsValidation(ref status))
                return false;
            else if (!FuelTypeValidation(ref status))
                return false;
            else if (!LicensePlateValidation(ref status))
                return false;
            else if (!CarPicturesValidation(ref status))
                return false;
            return true;
        }

        public void Reset()
        {
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
