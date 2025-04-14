using System;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.Windows.Input;
using RentestWPFTestTask.Infrastructure.Commands;
using RentestWPFTestTask.ViewModels.Baze;
using RentestWPFTestTask.Services.Dialog;
using RentestWPFTestTask.Services.Filter;

namespace RentestWPFTestTask.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        private readonly IImageDialogService _imageDialogService;
        public ImageViewModel ImageViewModel { get; }
        public FilterViewModel FilterViewModel { get; }
        public ICommand OpenImageCommand { get; }

        public MainWindowViewModel(IImageDialogService imageDialogService,
            IFilterService filterService,
            ImageViewModel imageViewModel)
        {
            _imageDialogService = imageDialogService;
            ImageViewModel = imageViewModel;
            FilterViewModel = new FilterViewModel(filterService, imageViewModel);
            OpenImageCommand = new LambdaCommand(OpenImage);
        }

        private void OpenImage(object parameter)
        {
            var image = _imageDialogService.OpenImage();
            ImageViewModel.LoadImage(image);
        }
    }
}