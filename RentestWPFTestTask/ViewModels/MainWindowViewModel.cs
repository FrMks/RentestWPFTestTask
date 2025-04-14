using System;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.Windows.Input;
using RentestWPFTestTask.Infrastructure.Commands;
using RentestWPFTestTask.ViewModels.Baze;
using RentestWPFTestTask.Services;

namespace RentestWPFTestTask.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        private readonly IImageDialogService _imageDialogService;
        public ImageViewModel ImageViewModel { get; }

        public ICommand OpenImageCommand { get; }

        public MainWindowViewModel(IImageDialogService imageDialogService)
        {
            _imageDialogService = imageDialogService;
            ImageViewModel = new ImageViewModel();

            OpenImageCommand = new LambdaCommand(OpenImage);
        }

        private void OpenImage(object parameter)
        {
            var image = _imageDialogService.OpenImage();
            ImageViewModel.LoadImage(image);
        }
    }
}