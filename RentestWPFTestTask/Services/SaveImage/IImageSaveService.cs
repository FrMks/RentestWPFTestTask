using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace RentestWPFTestTask.Services.SaveImage
{
    internal interface IImageSaveService
    {
        Task SaveImageAsync(BitmapImage image);
    }
}
