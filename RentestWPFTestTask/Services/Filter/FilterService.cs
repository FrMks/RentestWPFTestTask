using OpenCvSharp;
using RentestWPFTestTask.Infrastructure.ImageConverter;
using RentestWPFTestTask.Models;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;

namespace RentestWPFTestTask.Services.Filter
{
    internal class FilterService : IFilterService
    {
        public async Task<BitmapImage> ApplyFilterAsync(ImageFilter filter, BitmapImage sourceImage)
        {
            if (sourceImage == null) return null;

            using (var mat = ImageConverter.BitmapImageToMat(sourceImage))
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
                            return sourceImage;
                    }

                    if (resultMat != null)
                    {
                        var newImage = ImageConverter.MatToBitmapImage(resultMat, ".tiff");
                        return newImage;
                    }
                }
                finally
                {
                    resultMat?.Dispose();
                }
            }

            return sourceImage;
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
            Cv2.CvtColor(inputImage, outputImage, ColorConversionCodes.BGR2GRAY);
            return outputImage;
        }

        private Mat ApplyMedianFilter(Mat inputImage)
        {
            Mat matForFilter = ApplyGrayScale(inputImage);
            Console.WriteLine("ApplyMedianFilter: Apply grayscale");
            
            var outputImage = new Mat();
            Cv2.MedianBlur(matForFilter, outputImage, 21);
            return outputImage;
        }
        
        private Mat ApplySobelFilter(Mat inputImage)
        {
            Mat matForFilter = ApplyGrayScale(inputImage);
            Console.WriteLine("ApplyMedianFilter: Apply grayscale");

            // Матрицы для градиентов по X и Y
            Mat gradX = new Mat();
            Mat gradY = new Mat();
            
            Cv2.Sobel(matForFilter, gradX, MatType.CV_16S, 1, 0, ksize: 3);
            Cv2.Sobel(matForFilter, gradY, MatType.CV_16S, 0, 1, ksize: 3);
            
            Mat absGradX = new Mat();
            Mat absGradY = new Mat();
            Cv2.ConvertScaleAbs(gradX, absGradX);
            Cv2.ConvertScaleAbs(gradY, absGradY);
            
            Mat outputImage = new Mat();
            Cv2.AddWeighted(absGradX, 0.5, absGradY, 0.5, 0, outputImage);
            return outputImage;
        }
    }
}