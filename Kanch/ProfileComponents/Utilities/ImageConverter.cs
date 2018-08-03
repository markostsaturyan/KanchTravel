using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Kanch.ProfileComponents.Utilities
{
    public static class ImageConverter
    {
        public static ImageSource ConvertImageToImageSource(byte[] image)
        {
            ImageSource result;
            using (var stream = new MemoryStream(image))
            {
                result = BitmapFrame.Create(
                    stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            }

            return result;
        }
    }
}
