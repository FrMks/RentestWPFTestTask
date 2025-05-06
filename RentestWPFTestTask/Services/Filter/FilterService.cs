using OpenCvSharp;
using RentestWPFTestTask.Infrastructure.ImageConverter;
using RentestWPFTestTask.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace RentestWPFTestTask.Services.Filter
{
    internal class FilterService : IFilterService
    {
        public async Task<(BitmapImage, Mat)> ApplyFilterAsync(ImageFilter filter, string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return (null, null);

            using (var mat = ImageConverter.FilePathToMat(filePath))
            {
                Mat resultMat = null;
                try
                {
                    switch (filter.Id)
                    {
                        case "GRAY":
                            resultMat = ApplyGrayScale(mat);
                            break;
                        case "MEDIAN":
                            resultMat = ApplyMedianFilter(mat);
                            break;
                        case "SOBEL":
                            resultMat = ApplySobelFilter(mat);
                            break;
                        case "none":
                            return (ImageConverter.MatToBitmapImage(mat), mat.Clone());
                    }

                    if (resultMat != null)
                    {
                        var newImage = ImageConverter.MatToBitmapImage(resultMat);
                        return (newImage, resultMat);
                    }
                }
                catch
                {
                    resultMat?.Dispose();
                    throw;
                }
            }
            return (null, null);
        }

        public IEnumerable<ImageFilter> GetAvailableFilters()
        {
            return new List<ImageFilter>
            {
                new ImageFilter("Медианный фильтр", "MEDIAN"),
                new ImageFilter("Без фильтра", "none"),
                new ImageFilter("Собель фильтр", "SOBEL")
            };
        }

        private Mat ApplyGrayScale(Mat inputImage)
        {
            var outputImage = new Mat();
            Console.WriteLine($"inputImage.Channels :  {inputImage.Type()}");
            if (inputImage.Channels() == 1)
            {
                inputImage.CopyTo(outputImage);
            }
            else
            {
                Cv2.CvtColor(inputImage, outputImage, ColorConversionCodes.BGR2GRAY);
            }
            Console.WriteLine($"ApplyGrayScale: outputImage.Type : {outputImage.Type()}");
            return outputImage;
        }

        private Mat ApplyMedianFilter(Mat inputImage)
        {
            Mat matForFilter = inputImage;
            if (inputImage.Channels() != 1)
            {
                matForFilter = new Mat();
                matForFilter.ConvertTo(matForFilter, MatType.CV_16UC1);
                Cv2.CvtColor(inputImage, matForFilter, ColorConversionCodes.BGR2GRAY);
            }

            

            Mat outputImage = new Mat();
            outputImage.ConvertTo(outputImage, MatType.CV_16UC1);
            Cv2.MedianBlur(matForFilter, outputImage, 5);

            if (matForFilter != inputImage)
                matForFilter.Dispose();

            Console.WriteLine($"ApplyMedianFilter: outputImage.Type :  {outputImage.Type()}");
            return outputImage;
        }

        private Mat ApplySobelFilter(Mat inputImage)
        {
            var matForFilter = ApplyGrayScale(inputImage);

            Mat gradX = new Mat(matForFilter.Height, matForFilter.Width, MatType.CV_16UC1);
            Mat gradY = new Mat(matForFilter.Height, matForFilter.Width, MatType.CV_16UC1);

            Cv2.Sobel(matForFilter, gradX, MatType.CV_16UC1, 1, 0, ksize: 3);
            Cv2.Sobel(matForFilter, gradY, MatType.CV_16UC1, 0, 1, ksize: 3);

            Mat outputImage = new Mat();
            Cv2.AddWeighted(gradX, 0.5, gradY, 0.5, 0, outputImage);
            
            Console.WriteLine($"ApplySobelFilter: outputImage.Type :  {outputImage.Type()}");
            return outputImage;
        }
    }
}
