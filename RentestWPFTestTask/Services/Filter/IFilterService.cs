using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using OpenCvSharp;
using RentestWPFTestTask.Models;

namespace RentestWPFTestTask.Services.Filter
{
    internal interface IFilterService
    {
        Task<(BitmapImage, Mat)> ApplyFilterAsync(ImageFilter filter, string filePath);
        IEnumerable<ImageFilter> GetAvailableFilters();
    }
}
