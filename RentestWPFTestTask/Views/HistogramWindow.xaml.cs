using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
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
        BuildHistogram(viewModel.Image);
    }

    private void BuildHistogram(Mat image)
    {
        if (image.Channels() != 1)
        {
            MessageBox.Show("Изображение должно быть в градациях серого для построения гистограммы");
            return;
        }
    
        // Параметры гистограммы
        int histogramSize = 256;
        Rangef histRange = new Rangef(0, 256);

        // Вычисляем гистограмму
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
    
        // Нормализуем гистограмму
        Cv2.Normalize(histogram, histogram, 0, 100, NormTypes.MinMax);
    
        int histWidth = 800;
        int histHeight = 600;
        int binWidth = histWidth / histogramSize;
    
        Mat histImage = new Mat(histHeight, histWidth, MatType.CV_8UC3, Scalar.All(255));
    
        // Рисуем гистограмму
        for (int i = 1; i < histogramSize; i++)
        {
            Point pt1 = new Point(binWidth * (i - 1), histHeight - (int)histogram.At<float>(i - 1));
            Point pt2 = new Point(binWidth * i, histHeight - (int)histogram.At<float>(i));
            Cv2.Line(histImage, pt1, pt2, new Scalar(0, 0, 0), 2);
        }

        BitmapSource bitmapSource = ConvertMatToBitmapSource(histImage);
    
        HistogramImage.Source = bitmapSource;

        histogram.Dispose();
        histImage.Dispose();
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
}