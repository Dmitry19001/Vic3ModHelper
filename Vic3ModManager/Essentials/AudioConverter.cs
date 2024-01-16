using FFMpegCore;
using FFMpegCore.Enums;
using System.Diagnostics;
using System.Windows;


public class AudioConverter
{
    private bool ffmpegFound;

    public AudioConverter()
    {
        ffmpegFound = false;
    }

    public bool FindFFMpeg()
    {
        ffmpegFound = System.IO.File.Exists("./ffmpeg/ffmpeg.exe");
        if (ffmpegFound) {
            GlobalFFOptions.Configure(new FFOptions { BinaryFolder = "./ffmpeg", TemporaryFilesFolder = "./tmp" });
            MessageBox.Show("FFMpeg found"); 
        }
        else {
            //trying again with slight different path
            ffmpegFound = System.IO.File.Exists("./ffmpeg/bin/ffmpeg.exe");

            if (ffmpegFound)
            {
                GlobalFFOptions.Configure(new FFOptions { BinaryFolder = "./ffmpeg/bin", TemporaryFilesFolder = "./tmp" });
                MessageBox.Show("FFMpeg found");
            }
            else
            {
                MessageBox.Show("FFMpeg not found");
            }

        }
        return ffmpegFound;
    }


    public void ConvertToOgg(string sourcePath, string destinationPath)
    {
        if (!ffmpegFound)
        {
            Debug.WriteLine("FFMpeg not found");
            return;
        }

        FFMpegArguments.FromFileInput(sourcePath)
            .OutputToFile(destinationPath, true, options => options
                .WithAudioCodec(AudioCodec.LibVorbis)
                .WithoutMetadata()
                .WithCustomArgument("-vn")
            ).ProcessSynchronously();
    }

}
