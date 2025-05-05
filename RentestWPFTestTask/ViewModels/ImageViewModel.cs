using System.Windows.Media.Imaging;
using OpenCvSharp;
using RentestWPFTestTask.ViewModels.Baze;

namespace RentestWPFTestTask.ViewModels
{
    internal class ImageViewModel : ViewModel
    {
        private string _filePath;
        public string FilePath
        {
            get => _filePath;
            set => Set(ref _filePath, value);
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
        
        private Mat _currentMat;
        public Mat CurrentMat
        {
            get => _currentMat;
            set => Set(ref _currentMat, value);
        }


        public void LoadImage(string filePath)
        {
            FilePath = filePath;
            CurrentMat?.Dispose();
            CurrentMat = Infrastructure.ImageConverter.ImageConverter.FilePathToMat(filePath);
            Image = Infrastructure.ImageConverter.ImageConverter.MatToBitmapImage(CurrentMat);

            HasImage = true;
            IsFiltered = false;
        }
    }
}