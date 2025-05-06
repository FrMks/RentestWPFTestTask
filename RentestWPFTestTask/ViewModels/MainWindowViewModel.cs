using System.Linq;
using System.Windows.Input;
using RentestWPFTestTask.Infrastructure.Commands;
using RentestWPFTestTask.Models;
using RentestWPFTestTask.Services.Filter;
using RentestWPFTestTask.Services.SaveImage;
using RentestWPFTestTask.Services;
using RentestWPFTestTask.ViewModels.Baze;
using RentestWPFTestTask.Views;

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
        
        public ICommand OpenHistogramCommand { get; }

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
            OpenHistogramCommand = new LambdaCommand(OpenHistogram);
        }

        private async void SaveImage(object parameter)
        {
            await _imageSaveService.SaveImageAsync(ImageViewModel.CurrentMat, "filtered_image");
        }

        private bool CanSaveImage(object parameter) => ImageViewModel.IsFiltered;

        private async void OpenImage(object parameter)
        {
            var filePath = await _imageDialogService.OpenImageAsync(); 
            if (!string.IsNullOrEmpty(filePath))
            {
                ImageViewModel.LoadImage(filePath);
                await FilterViewModel.ApplyFilterAsync(new ImageFilter("Оттенки серого", "GRAY"), filePath);
                FilterViewModel.SelectedFilter = FilterViewModel.AvailableFilters.FirstOrDefault(f => f.Id == "none");
            }
        }

        private void OpenHistogram(object parameter)
        {
            var histogramWindow = new HistogramWindow();
            histogramWindow.Show();
        }
    }
}
