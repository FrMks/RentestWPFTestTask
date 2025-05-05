
using System.IO;
using System.Windows.Media;
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
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                encoder.Save(stream);

                var bytes = stream.ToArray();

                var mat = Cv2.ImDecode(bytes, ImreadModes.Unchanged);
                Console.WriteLine($"BitmapImageToMat: Mat type: {mat.Type()}, mat channelsL {mat.Channels()}");
                return mat;
            }
        }

        public static BitmapImage MatToBitmapImage(Mat mat, string format = ".png")
        {
            var image = new BitmapImage();
            using (var stream = new MemoryStream())
            {
                var imageBytes = mat.ImEncode(format);
                stream.Write(imageBytes, 0, imageBytes.Length);
                stream.Position = 0;

                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream;
                image.EndInit();
                image.Freeze();
            }
            
            Console.WriteLine("MatToBitmapImage: The conversion is completed");
            return image;
        }
    }
}
