using System.Windows.Media.Imaging;
using RentestWPFTestTask.Models;
using RentestWPFTestTask.Services.Filter;
using RentestWPFTestTask.ViewModels.Baze;

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
                    _imageViewModel.Image = _imageViewModel.OriginalImage;
                    _imageViewModel.IsFiltered = false;
                }
                else
                {
                    _imageViewModel.Image = _imageViewModel.OriginalImage;
                    
                    var filtered = await _filterService.ApplyFilterAsync(SelectedFilter, _imageViewModel.Image);
                    _imageViewModel.Image = filtered;
                    _imageViewModel.IsFiltered = true;
                }
            }
        }

        public async Task ApplyGrayScaleOnLoadAsync(BitmapImage image)
        {
            if (image == null)
                return;

            var filtered = await _filterService.ApplyFilterAsync
                (new ImageFilter("GrayScale", "GRAY"), image);
            _imageViewModel.Image = filtered;
            _imageViewModel.IsFiltered = true;
        }

    }
}
