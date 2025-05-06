using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using OpenCvSharp;
using RentestWPFTestTask.ViewModels;
using Point = OpenCvSharp.Point;
using Window = System.Windows.Window;

namespace RentestWPFTestTask.Views;

public partial class HistogramWindow : Window
{
    internal HistogramWindow(HistogramWindowViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        Loaded += (s, e) =>BuildHistogram(viewModel.Image);
    }

    private async void BuildHistogram(Mat image)
    {
        if (image.Channels() != 1)
        {
            MessageBox.Show("Изображение должно быть в градациях серого для построения гистограммы");
            return;
        }
        
        HistogramCanvas.Children.Clear();
        
        int histogramSize = 256;
        Rangef histRange = new Rangef(0, 65535);

        // Вычисляем гистограмму
        var hist = await Task.Run(() =>
        {
            Mat histogram = new Mat();
            Cv2.CalcHist(
                images: new[] { image },
                channels: new[] { 0 },
                mask: null,
                hist: histogram,
                dims: 1,
                histSize: new[] { histogramSize },
                ranges: new[] { histRange }
            );
            return histogram;
        });
        
        
        double maxVal;
        Cv2.MinMaxLoc(hist, out _, out maxVal);
        
        //гарантируем, что все 256 столбцов равномерно заполнят ширину Canvas без перекрытий или пустот
        double canvasWidth = HistogramCanvas.ActualWidth;
        double canvasHeight = HistogramCanvas.ActualHeight;
        double binWidth = canvasWidth / histogramSize;

        Application.Current.Dispatcher.BeginInvoke(() =>
        {
            for (int i = 0; i < histogramSize; i++)
            {
                float binValue = hist.At<float>(i);
                double binHeight = (binValue / maxVal) * canvasHeight;
                Line line = new Line
                {
                    X1 = i * binWidth,
                    Y1 = canvasHeight,
                    X2 = i * binWidth,
                    Y2 = canvasHeight - binHeight,
                    Stroke = Brushes.Black,
                    StrokeThickness = binWidth,
                    SnapsToDevicePixels = true
                };
            
                HistogramCanvas.Children.Add(line);
            }    
        });
        
    }

    private BitmapSource ConvertMatToBitmapSource(Mat mat)
    {
        using (var memoryStream = new MemoryStream())
        {
            var imageBytes = mat.ToBytes(".png");
            memoryStream.Write(imageBytes, 0, imageBytes.Length);
            memoryStream.Position = 0;

            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.StreamSource = memoryStream;
            bitmapImage.EndInit();
            bitmapImage.Freeze();

            return bitmapImage;
        }
    }

    private void HistogramCanvas_SizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
    {
        if (DataContext is HistogramWindowViewModel viewModel && viewModel.Image != null)
        {
            BuildHistogram(viewModel.Image);
        }
    }
}