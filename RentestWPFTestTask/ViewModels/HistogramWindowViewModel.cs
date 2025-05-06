using OpenCvSharp;
using RentestWPFTestTask.ViewModels.Baze;

namespace RentestWPFTestTask.ViewModels;

internal class HistogramWindowViewModel :  ViewModel
{
    public Mat Image { get; }
    
    
    public HistogramWindowViewModel(Mat image)
    {
        Image = image;
    }

}