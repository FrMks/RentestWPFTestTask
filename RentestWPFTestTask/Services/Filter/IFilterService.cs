using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using RentestWPFTestTask.Models;

namespace RentestWPFTestTask.Services.Filter
{
    internal interface IFilterService
    {
        IEnumerable<ImageFilter> GetAvailableFilters();
        Task<BitmapImage> ApplyFilterAsync(ImageFilter filter, BitmapImage sourceImage);
    }
}
