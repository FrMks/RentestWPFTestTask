using System.Windows.Media.Imaging;
using OpenCvSharp;
using RentestWPFTestTask.Infrastructure.ImageConverter;
using RentestWPFTestTask.Services.Filter;
using RentestWPFTestTask.ViewModels.Baze;

namespace RentestWPFTestTask.ViewModels
{
    internal class ImageViewModel : ViewModel
    {
        private BitmapImage _originalImage;
        public BitmapImage OriginalImage
        {
            get => _originalImage;
            set => Set(ref _originalImage, value);
        }
        
        
        private BitmapImage _image;
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

        private bool _isFiltered;
        public bool IsFiltered
        {
            get => _isFiltered;
            set => Set(ref _isFiltered, value);
        }

        
        public void LoadImage(BitmapImage image)
        { 
            OriginalImage = image;
            Image = image;
            HasImage = image != null;
            IsFiltered = false;
        }
    }
}
