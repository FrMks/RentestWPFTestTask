using System;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.Windows.Input;
using RentestWPFTestTask.Infrastructure.Commands;
using RentestWPFTestTask.ViewModels.Baze;
using RentestWPFTestTask.Services.Dialog;
using RentestWPFTestTask.Services.Filter;
using System.IO;

namespace RentestWPFTestTask.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        private readonly IImageDialogService _imageDialogService;
        public ImageViewModel ImageViewModel { get; }
        public FilterViewModel FilterViewModel { get; }
        public ICommand OpenImageCommand { get; }
        public ICommand SaveImageCommand { get; }

        public MainWindowViewModel(IImageDialogService imageDialogService,
    IFilterService filterService,
    ImageViewModel imageViewModel)
        {
            _imageDialogService = imageDialogService;
            ImageViewModel = imageViewModel;
            FilterViewModel = new FilterViewModel(filterService, imageViewModel);
            OpenImageCommand = new LambdaCommand(OpenImage);
            SaveImageCommand = new LambdaCommand(SaveImage, CanSaveImage);
        }

        private void SaveImage(object parameter)
        {
            var dialog = new SaveFileDialog
            {
                Title = "Сохранить изображение",
                Filter = "PNG (*.png)|*.png|JPEG (*.jpg)|*.jpg|BMP (*.bmp)|*.bmp",
                FileName = "filtered_image"
            };

            if (dialog.ShowDialog() == true)
            {
                using (var fileStream = new FileStream(dialog.FileName, FileMode.Create))
                {
                    BitmapEncoder encoder;

                    switch (Path.GetExtension(dialog.FileName).ToLower())
                    {
                        case ".jpg":
                            encoder = new JpegBitmapEncoder();
                            break;
                        case ".bmp":
                            encoder = new BmpBitmapEncoder();
                            break;
                        default:
                            encoder = new PngBitmapEncoder();
                            break;
                    }

                    encoder.Frames.Add(BitmapFrame.Create(ImageViewModel.Image));
                    encoder.Save(fileStream);
                }
            }
        }

        private bool CanSaveImage(object parameter) => ImageViewModel.IsFiltered;

        private void OpenImage(object parameter)
        {
            var image = _imageDialogService.OpenImage();
            ImageViewModel.LoadImage(image);
        }
    }
}