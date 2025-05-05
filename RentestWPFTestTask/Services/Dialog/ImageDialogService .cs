using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace RentestWPFTestTask.Services.Dialog
{
    internal class ImageDialogService : IImageDialogService
    {
        public async Task<string> OpenImageAsync()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpeg;*.jpg;*.bmp;*.tif;*.tiff)|*.png;*.jpeg;*.jpg;*.bmp;*.tif;*.tiff|All files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            };

            if (openFileDialog.ShowDialog() == true)
                return openFileDialog.FileName;
            return null;
        }
    }
}
