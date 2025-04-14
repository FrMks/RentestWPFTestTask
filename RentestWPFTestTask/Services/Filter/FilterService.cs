using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using RentestWPFTestTask.Models;

namespace RentestWPFTestTask.Services.Filter
{
    internal class FilterService : IFilterService
    {
        public void ApplyFilter(ImageFilter filter, BitmapImage sourceImage)
        {
            switch (filter.Id)
            {
                case "GRAY":
                    ApplyGrayScale(sourceImage);
                    break;
                case "MEDIAN":
                    ApplyMedianFilter(sourceImage);
                    break;
            }
        }

        public IEnumerable<ImageFilter> GetAvailableFilters()
        {
            return new List<ImageFilter>
            {
                new ImageFilter("GrayScale", "GRAY"),
                new ImageFilter("Медианный фильтр", "MEDIAN")
            };
        }

        private void ApplyGrayScale(BitmapImage image) { /* ... */ }
        private void ApplyMedianFilter(BitmapImage image) { /* ... */ }
    }
}
