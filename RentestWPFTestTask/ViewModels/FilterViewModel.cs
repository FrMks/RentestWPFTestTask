using RentestWPFTestTask.Models;
using RentestWPFTestTask.Services.Filter;
using RentestWPFTestTask.ViewModels.Baze;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentestWPFTestTask.ViewModels
{
    internal class FilterViewModel : ViewModel
    {
        private readonly IFilterService _filterService;
        private readonly ImageViewModel _imageViewModel;

        private ImageFilter _selectedFilter;
        public ImageFilter SelectedFilter
        {
            get => _selectedFilter;
            set
            {
                if (Set(ref _selectedFilter, value) && value != null)
                {
                    ApplyFilter();
                }
            }
        }

        public IEnumerable<ImageFilter> AvailableFilters { get; }

        public FilterViewModel(IFilterService filterService,
                               ImageViewModel imageViewModel)
        {
            _filterService = filterService;
            _imageViewModel = imageViewModel;
            AvailableFilters = _filterService.GetAvailableFilters();

            SelectedFilter = AvailableFilters.FirstOrDefault(f => f.Id == "none");
        }

        private async void ApplyFilter()
        {
            if (_imageViewModel.HasImage)
            {
                if (SelectedFilter.Id == "none")
                {
                    using (var mat = Infrastructure.ImageConverter.ImageConverter.FilePathToMat(_imageViewModel.FilePath))
                    {
                        _imageViewModel.CurrentMat?.Dispose();
                        _imageViewModel.CurrentMat = mat.Clone();
                        _imageViewModel.Image = Infrastructure.ImageConverter.ImageConverter.MatToBitmapImage(mat);
                    }
                    _imageViewModel.IsFiltered = false;
                }
                else
                {
                    var (image, mat) = await _filterService.ApplyFilterAsync(SelectedFilter, _imageViewModel.FilePath);
                    _imageViewModel.CurrentMat?.Dispose();
                    _imageViewModel.CurrentMat = mat;
                    _imageViewModel.Image = image;
                    _imageViewModel.IsFiltered = true;
                }
            }
        }

        public async Task ApplyFilterAsync(ImageFilter filter, string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return;

            var (image, mat) = await _filterService.ApplyFilterAsync(filter, filePath);
            _imageViewModel.CurrentMat?.Dispose();
            _imageViewModel.CurrentMat = mat;
            _imageViewModel.Image = image;
            _imageViewModel.IsFiltered = true;
        }
    }
}
