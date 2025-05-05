using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace RentestWPFTestTask.Services.SaveImage
{
    internal class ImageSaveService : IImageSaveService
    {
        public async Task SaveImageAsync(BitmapImage image)
        {
            var dialog = new SaveFileDialog
            {
                Title = "Сохранить изображение",
                Filter = "PNG (*.png)|*.png|JPEG (*.jpg)|*.jpg|BMP (*.bmp)|*.bmp|TIFF (*.tif, *.tiff)|*.tif;*.tiff",
                FileName = "filtered_image"
            };
            if (dialog.ShowDialog() == true)
            {
                using var fileStream = new FileStream(dialog.FileName, FileMode.Create);
                BitmapEncoder encoder = dialog.FilterIndex switch
                {
                    2 => new JpegBitmapEncoder(),
                    3 => new BmpBitmapEncoder(),
                    _ => new PngBitmapEncoder()
                };

                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(fileStream);
            }
        }
    }
}
