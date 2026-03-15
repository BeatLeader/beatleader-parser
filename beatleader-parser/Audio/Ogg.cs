using System.IO;

namespace Parser.Audio
{
    internal static class Ogg
    {
        public static double AudioStreamToLength(Stream audioStream)
        {
            try
            {
                using var vorbis = new NVorbis.VorbisReader(audioStream);
                double audioLengthInSeconds = (double)vorbis.TotalSamples / vorbis.SampleRate;
                return audioLengthInSeconds;
            }
            catch
            {
                return 0;
            }
        }
    }
}
