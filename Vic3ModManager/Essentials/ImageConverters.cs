using CSharpImageLibrary;

namespace Vic3ModManager.Essentials
{
    internal static class ImageConverters
    {

        public static void ConvertToDDS(string imagePath, string outputPath)
        {
            // Load the PNG or JPG image
            using ImageEngineImage image = new(imagePath);

            // Set DDS format options
            var options = new ImageFormats.ImageEngineFormatDetails(
                inFormat: ImageEngineFormat.DDS_RGB_8
            );

            // Convert and save the image as DDS
            image.Save(outputPath, options, MipHandling.KeepTopOnly);
        }

    }
}
