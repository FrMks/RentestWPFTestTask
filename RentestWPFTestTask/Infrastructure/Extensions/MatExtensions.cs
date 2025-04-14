using OpenCvSharp;
using System.IO;

namespace RentestWPFTestTask.Infrastructure.Extensions
{
    public static class MatExtensions
    {
        public static byte[] ToBytes(this Mat mat, string format = "jpg")
        {
            var ext = format.ToLower() switch
            {
                "jpg" or "jpeg" => ".jpg",
                "png" => ".png",
                "bmp" => ".bmp",
                _ => ".jpg"
            };

            return mat.ImEncode(ext);
        }
    }
}