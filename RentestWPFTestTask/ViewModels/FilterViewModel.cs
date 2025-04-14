using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public FilterViewModel(IFilterService filterService, ImageViewModel imageViewModel)
        {
            _filterService = filterService;
            _imageViewModel = imageViewModel;
            AvailableFilters = _filterService.GetAvailableFilters();
        }

        private void ApplyFilter()
        {
            if (_imageViewModel.HasImage)
            {
                _filterService.ApplyFilter(SelectedFilter, _imageViewModel.Image);
            }
        }
    }
}
