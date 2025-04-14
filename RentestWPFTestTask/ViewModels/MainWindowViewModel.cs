using System.Windows.Input;
using RentestWPFTestTask.Infrastructure.Commands;
using RentestWPFTestTask.ViewModels.Baze;
using RentestWPFTestTask.Services.Filter;
using RentestWPFTestTask.Services.SaveImage;
using RentestWPFTestTask.Services;

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

        private async void SaveImage(object parameter)
        {
            await _imageSaveService.SaveImageAsync(ImageViewModel.Image);
        }

        private bool CanSaveImage(object parameter) => ImageViewModel.IsFiltered;

        private async void OpenImage(object parameter)
        {
            var image = await _imageDialogService.OpenImageAsync();
            ImageViewModel.LoadImage(image);
        }
    }
}