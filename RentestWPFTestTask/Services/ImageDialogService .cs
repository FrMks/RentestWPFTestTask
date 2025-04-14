using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace RentestWPFTestTask.Services
{
    internal class ImageDialogService : IImageDialogService
    {
        public BitmapImage OpenImage()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpeg;*.jpg;*.bmp)|*.png;*.jpeg;*.jpg;*.bmp|All files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    return new BitmapImage(new Uri(openFileDialog.FileName));
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }
    }
}
