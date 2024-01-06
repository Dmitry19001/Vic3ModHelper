//using NAudio.Wave;
//using NAudio.Vorbis;
//using NVorbis;

//public static class AudioConverter
//{
//    public static void ConvertMp3ToOgg(string mp3FilePath, string oggFilePath)
//    {
//        using var reader = new Mp3FileReader(mp3FilePath);
//        using var writer = new VorbisWaveWriter(oggFilePath, reader.WaveFormat.Channels, reader.WaveFormat.SampleRate);
//        byte[] buffer = new byte[reader.WaveFormat.SampleRate * reader.WaveFormat.Channels * 4]; // 4 bytes per sample (16-bit stereo)
//        int bytesRead;
//        while ((bytesRead = reader.Read(buffer, 0, buffer.Length)) > 0)
//        {
//            writer.Write(buffer, 0, bytesRead);
//        }
//    }

//}
