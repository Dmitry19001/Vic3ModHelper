using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using CSharpImageLibrary;

namespace Vic3ModManager.Essentials
{
    internal static class ImageConverters
    {
        // TODO: implement image converters

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


        public static void ConverterTester(string imagePath, string outputPath)
        {
            // TESTED AND WORKING FORMAT FOR GAME IS DDS_RGB_8

            // need to remove .dds from outputPath
            outputPath = outputPath.Replace(".dds", "");

            Debug.WriteLine($"from {imagePath}");
            Debug.WriteLine($"to {outputPath}");

            // need all ImageEngineFormat dds enums

            ImageEngineFormat[] imageEngineFormats = (ImageEngineFormat[])Enum.GetValues(typeof(ImageEngineFormat));
            ImageEngineFormat[] ddsValues = imageEngineFormats.Where(val => val.ToString().StartsWith("DDS")).ToArray();

            // BAD FORMATS NEED TO EXLUDE: DDS_ARGB_32F, DDS_CUSTOM, DDS_DX10, DDS_R5G6B5, DDS_V8U8, DDS_A8L8, DDS_A8, DDS_G16_R16, DDS_G8_L8
            ImageEngineFormat[] badFormats =
            [
                ImageEngineFormat.DDS_ARGB_32F,
                ImageEngineFormat.DDS_CUSTOM,
                ImageEngineFormat.DDS_DX10,
                ImageEngineFormat.DDS_R5G6B5,
                ImageEngineFormat.DDS_V8U8,
                ImageEngineFormat.DDS_A8L8,
                ImageEngineFormat.DDS_A8,
                ImageEngineFormat.DDS_G16_R16,
                ImageEngineFormat.DDS_G8_L8,
                ImageEngineFormat.DDS_ATI1,
                ImageEngineFormat.DDS_ATI2_3Dc,
            ];

            ddsValues = ddsValues.Where(val => !badFormats.Contains(val)).ToArray();

            for (int x = 0; x < ddsValues.Length; x++)
            {
                ImageEngineFormat format = ddsValues[x];
                Console.WriteLine("Converting to " + format.ToString());

                string postfix = format.ToString().Replace("DDS_", "");
                try
                {
                    // Load the PNG or JPG image
                    using ImageEngineImage image = new(imagePath);

                    // Set DDS format options
                    var options = new ImageFormats.ImageEngineFormatDetails(inFormat: format);
                    

                    // Convert and save the image as DDS
                    image.Save($"{outputPath}_{postfix}.dds", options, MipHandling.KeepTopOnly);


                    Debug.WriteLine($"saved {outputPath}_{postfix}.dds");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error converting to {outputPath}_{postfix}.dds: {ex.Message}");
                }
            }

        }
    }
}
