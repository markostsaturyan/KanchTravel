using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Kanch.DataModel;

namespace Kanch.Validation
{
    public class DriverInfoValidation
    {
        private bool BrandOfCarValidation(string brand,ref string status)
        {
            if (brand == null)
            {
                status = "Enter brand of the car.";
                return false;
            }
            return true;
        }

        private bool NumberOfSeatsValidation(int numberOfSeats, ref string status)
        {
            if (numberOfSeats == 0)
            {
                status = "Enter the number of seats.";
                return false;
            }
            return true;
        }

        private bool FuelTypeValidation(string fuelType, ref string status)
        {
            if (fuelType == null)
            {
                status = "Enter the fuel type.";
                return false;
            }
            return true;
        }

        private bool LicensePlateValidation(string licensePlate, ref string status)
        {
            if (licensePlate == null)
            {
                status = "Enter the License plate";
                return false;
            }
            return true;
        }

        private bool CarPicturesValidation(Image carPicture1, Image carPicture2, Image carPicture3, ref string status)
        {
            if(carPicture1==null && carPicture2==null && carPicture3 == null)
            {
                status = "Add at least one photo of the car.";
                return false;
            }
            return true;
        }

        public bool DriverInfoValidationResult(Driver driver, out string status)
        {
            status = "";
            if (!BrandOfCarValidation(driver.Car.Brand, ref status))
                return false;
            else if (!NumberOfSeatsValidation(driver.Car.NumberOfSeats, ref status))
                return false;
            else if (!FuelTypeValidation(driver.Car.FuelType, ref status))
                return false;
            else if (!LicensePlateValidation(driver.Car.LicensePlate, ref status))
                return false;
            else if (!CarPicturesValidation(driver.Car.CarPicture1, driver.Car.CarPicture2, driver.Car.CarPicture3, ref status))
                return false;
            return true;
        }

    }
    
}
