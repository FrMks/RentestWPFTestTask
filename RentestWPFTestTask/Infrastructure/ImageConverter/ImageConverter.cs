
using System.IO;
using System.Windows.Media.Imaging;
using OpenCvSharp;

namespace RentestWPFTestTask.Infrastructure.ImageConverter
{
    internal class ImageConverter
    {
        public static Mat BitmapImageToMat(BitmapImage bitmapImage)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapImage)); 
                encoder.Save(stream);
                return Cv2.ImDecode(stream.ToArray(), ImreadModes.Color);
            }
        }

        public static BitmapImage MatToBitmapImage(Mat mat)
        {
            var image = new BitmapImage();
            using (var stream = new MemoryStream())
            {
                var imageBytes = mat.ImEncode(".png");
                stream.Write(imageBytes, 0, imageBytes.Length);
                stream.Position = 0;

                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream;
                image.EndInit();
                image.Freeze();
            }
            return image;
        }
    }
}
