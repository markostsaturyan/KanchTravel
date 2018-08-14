using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Kanch.ProfileComponents.Utilities
{
    public static class ImageConverter
    {
        public static ImageSource ConvertImageToImageSource(byte[] image)
        {
            if (image == null) return null;

            ImageSource result;
            using (var stream = new MemoryStream(image))
            {
                result = BitmapFrame.Create(
                    stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            }

            return result;
        }
        public static byte[] ImageSourceToBytes(ImageSource imageSource)
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

        public static ImageSource DefaultProfilePicture(string gender)
        {
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            if (gender == "Female")
            {
                img.UriSource = new Uri(@"pack://application:,,,/Kanch;component/Images/female.jpg");
            }
            else
            {
                img.UriSource = new Uri(@"pack://application:,,,/Kanch;component/Images/male.jpg");
            }
            img.EndInit();

            return img;
        }
    }
}
