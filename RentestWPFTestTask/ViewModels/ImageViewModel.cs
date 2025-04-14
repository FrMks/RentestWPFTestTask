using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using RentestWPFTestTask.ViewModels.Baze;

namespace RentestWPFTestTask.ViewModels
{
    internal class ImageViewModel : ViewModel
    {
        private BitmapImage _image;
        /// <summary>
        /// Храним выбранное изображение
        /// </summary>
        public BitmapImage Image
        {
            get => _image;
            set => Set(ref _image, value);
        }

        private bool _hasImage;
        public bool HasImage
        {
            get => _hasImage;
            set => Set(ref _hasImage, value);
        }

        public void LoadImage(BitmapImage image)
        {
            Image = image;
            HasImage = image != null;
        }
    }
}
