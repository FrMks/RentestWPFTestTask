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
        public async Task<BitmapImage> ApplyFilterAsync(ImageFilter filter, string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return null;

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
                            return ImageConverter.MatToBitmapImage(mat);
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
            return null;
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
            if (inputImage.Channels() == 1)
            {
                inputImage.CopyTo(outputImage);
            }
            else
            {
                Cv2.CvtColor(inputImage, outputImage, ColorConversionCodes.BGR2GRAY);
            }
            return outputImage;
        }

        private Mat ApplyMedianFilter(Mat inputImage)
        {
            Mat matForFilter = inputImage;
            if (inputImage.Channels() != 1)
            {
                matForFilter = new Mat();
                Cv2.CvtColor(inputImage, matForFilter, ColorConversionCodes.BGR2GRAY);
            }

            if (matForFilter.Depth() != MatType.CV_8U)
            {
                Mat tmp = new Mat();
                Cv2.Normalize(matForFilter, tmp, 0, 255, NormTypes.MinMax);
                tmp.ConvertTo(tmp, MatType.CV_8U);
                matForFilter.Dispose();
                matForFilter = tmp;
            }

            Mat outputImage = new Mat();
            Cv2.MedianBlur(matForFilter, outputImage, 21);

            if (matForFilter != inputImage)
                matForFilter.Dispose();

            return outputImage;
        }

        private Mat ApplySobelFilter(Mat inputImage)
        {
            var matForFilter = ApplyGrayScale(inputImage);

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
