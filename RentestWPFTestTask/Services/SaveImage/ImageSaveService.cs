using Microsoft.Win32;
using OpenCvSharp;
using System.IO;
using System.Threading.Tasks;

namespace RentestWPFTestTask.Services.SaveImage
{
    internal class ImageSaveService : IImageSaveService
    {
        public async Task SaveImageAsync(Mat mat, string defaultFileName = "filtered_image")
        {
            var dialog = new SaveFileDialog
            {
                Title = "Сохранить изображение",
                Filter = "PNG (*.png)|*.png|JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|BMP (*.bmp)|*.bmp|TIFF (*.tif;*.tiff)|*.tif;*.tiff",
                FileName = defaultFileName
            };

            if (dialog.ShowDialog() == true)
            {
                string ext = Path.GetExtension(dialog.FileName).ToLower();

                string format = ext switch
                {
                    ".jpg" or ".jpeg" => ".jpg",
                    ".png" => ".png",
                    ".bmp" => ".bmp",
                    ".tif" or ".tiff" => ".tiff",
                    _ => ".png"
                };

                byte[] imageBytes = mat.ImEncode(format);

                await File.WriteAllBytesAsync(dialog.FileName, imageBytes);
            }
        }
    }
}