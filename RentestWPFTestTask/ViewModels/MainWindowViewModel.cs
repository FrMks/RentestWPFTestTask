using System;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.Windows.Input;
using RentestWPFTestTask.Infrastructure.Commands;
using RentestWPFTestTask.ViewModels.Baze;
using RentestWPFTestTask.Services.Dialog;
using RentestWPFTestTask.Services.Filter;
using System.IO;
using RentestWPFTestTask.Services.SaveImage;

namespace RentestWPFTestTask.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        private readonly IImageDialogService _imageDialogService;
        private readonly IImageSaveService _imageSaveService;
        public ImageViewModel ImageViewModel { get; }
        public FilterViewModel FilterViewModel { get; }
        public ICommand OpenImageCommand { get; }
        public ICommand SaveImageCommand { get; }

        public MainWindowViewModel(
            IImageDialogService imageDialogService,
            IFilterService filterService,
            IImageSaveService imageSaveService,
            ImageViewModel imageViewModel)
        {
            _imageDialogService = imageDialogService;
            _imageSaveService = imageSaveService;
            ImageViewModel = imageViewModel;
            FilterViewModel = new FilterViewModel(filterService, imageViewModel);

            OpenImageCommand = new LambdaCommand(OpenImage);
            SaveImageCommand = new LambdaCommand(SaveImage, CanSaveImage);
        }

        private void SaveImage(object parameter)
        {
            _imageSaveService.SaveImage(ImageViewModel.Image);
        }

        private bool CanSaveImage(object parameter) => ImageViewModel.IsFiltered;

        private void OpenImage(object parameter)
        {
            var image = _imageDialogService.OpenImage();
            ImageViewModel.LoadImage(image);
        }
    }
}