using System.Diagnostics;
using System.IO;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;

namespace Vic3ModManager.Essentials
{
    internal static class ImageHelpers
    {

        public static BitmapSource? BitmapImageFromDDS(string ddsPath)
        {
            try
            {
                byte[] ddsBytes = File.ReadAllBytes(ddsPath);
                using Pfim.IImage dds = Pfim.Dds.Create(ddsBytes, new Pfim.PfimConfig());
                dds.Decompress();

                var format = PixelFormats.Bgra32; // Default format, adjust as needed
                var stride = (dds.Width * format.BitsPerPixel + 7) / 8; // Calculate stride
                var bitmap = new WriteableBitmap(dds.Width, dds.Height, 96, 96, format, null);
                bitmap.WritePixels(new Int32Rect(0, 0, dds.Width, dds.Height), dds.Data, stride, 0);

                bitmap.Freeze(); // Makes the image read-only and thread-safe
                return bitmap;
            }
            catch (Exception ex)
            {
                // Handle the exception (log or throw)
                Debug.WriteLine("Error loading DDS image: " + ex.Message);
                return null;
            }
        }


        public static BitmapSource? BitmapSourceFromDDS(string ddsPath)
        {
            BitmapSource? bitmapSource = null;

            try
            {
                byte[] ddsBytes = File.ReadAllBytes(ddsPath);
                Pfim.PfimConfig pConfig = new();
                Pfim.Dds dds = Pfim.Dds.Create(ddsBytes, pConfig);
                dds.Decompress();

                bitmapSource = BitmapSource.Create(dds.Width, dds.Height, 96, 96, PixelFormats.Bgra32, null, dds.Data, dds.Stride);
            }
            catch { }

            return bitmapSource;
        }
    }
}