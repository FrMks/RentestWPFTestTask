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
                    }

                    if (resultMat != null)
                    {
                        var newImage = ImageConverter.MatToBitmapImage(resultMat);
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
                new ImageFilter("GrayScale", "GRAY"),
                new ImageFilter("Медианный фильтр", "MEDIAN")
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
            var outputImage = new Mat();
            Cv2.MedianBlur(inputImage, outputImage, 5);
            return outputImage;
        }
    }
}