using System.IO;
using System.Windows.Media.Imaging;
using OpenCvSharp;

namespace RentestWPFTestTask.Infrastructure.ImageConverter
{
    internal class ImageConverter
    {
        public static Mat FilePathToMat(string filePath)
        {
            var mat = Cv2.ImRead(filePath, ImreadModes.Unchanged);
            if (mat.Empty())
                throw new IOException("failed to load image through OpenCV ImRead");
            Console.WriteLine($"Image format: {mat.Type()}");
            return mat;
        }


        public static BitmapImage MatToBitmapImage(Mat mat)
        {
            Mat displayMat = mat;
            if (mat.Type() == MatType.CV_16UC1)
            {
                displayMat = new Mat();
                Cv2.Normalize(mat, displayMat, 0, 255, NormTypes.MinMax);
                displayMat.ConvertTo(displayMat, MatType.CV_8UC1);
            }

            var image = new BitmapImage();
            using (var stream = new MemoryStream())
            {
                var imageBytes = displayMat.ImEncode(".png");
                stream.Write(imageBytes, 0, imageBytes.Length);
                stream.Position = 0;

                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream;
                image.EndInit();
                image.Freeze();
            }

            if (displayMat != mat)
                displayMat.Dispose();

            return image;
        }
    }
}