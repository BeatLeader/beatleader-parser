using System.IO;

namespace BeatMapParser.Audio
{
    internal class Ogg
    {
        public double AudioStreamToLength(Stream audioStream)
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
