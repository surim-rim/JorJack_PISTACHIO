using System;
using System.IO;
using UnityEngine;

/// <summary>
/// AudioClip ↔ WAV byte array 변환 유틸리티.
/// </summary>
public static class WavUtility1
{
    const int HEADER_SIZE = 44;

    /// <summary>
    /// AudioClip → WAV byte[]
    /// </summary>
    public static byte[] FromAudioClip(AudioClip clip)
    {
        MemoryStream stream = new MemoryStream();

        int sampleCount = clip.samples * clip.channels;
        int frequency = clip.frequency;
        int channels = clip.channels;
        float[] samples = new float[sampleCount];
        clip.GetData(samples, 0);

        byte[] wavBytes = ConvertToWav(samples, channels, frequency);
        stream.Write(wavBytes, 0, wavBytes.Length);
        return stream.ToArray();
    }

    /// <summary>
    /// WAV byte[] → AudioClip
    /// </summary>
    public static AudioClip ToAudioClip(byte[] wavBytes, string clipName = "wavClip")
    {
        int channels;
        int frequency;
        float[] data = ConvertFromWav(wavBytes, out channels, out frequency);

        AudioClip clip = AudioClip.Create(clipName, data.Length / channels, channels, frequency, false);
        clip.SetData(data, 0);
        return clip;
    }

    /// <summary>
    /// float[] → WAV byte[]
    /// </summary>
    private static byte[] ConvertToWav(float[] samples, int channels, int sampleRate)
    {
        short bitsPerSample = 16;
        int byteRate = sampleRate * channels * bitsPerSample / 8;
        int subchunk2Size = samples.Length * sizeof(short);
        int chunkSize = 36 + subchunk2Size;

        using (MemoryStream stream = new MemoryStream())
        using (BinaryWriter writer = new BinaryWriter(stream))
        {
            // RIFF header
            writer.Write(System.Text.Encoding.ASCII.GetBytes("RIFF"));
            writer.Write(chunkSize);
            writer.Write(System.Text.Encoding.ASCII.GetBytes("WAVE"));

            // fmt subchunk
            writer.Write(System.Text.Encoding.ASCII.GetBytes("fmt "));
            writer.Write(16); // Subchunk1Size
            writer.Write((short)1); // AudioFormat (PCM)
            writer.Write((short)channels);
            writer.Write(sampleRate);
            writer.Write(byteRate);
            writer.Write((short)(channels * bitsPerSample / 8)); // BlockAlign
            writer.Write(bitsPerSample);

            // data subchunk
            writer.Write(System.Text.Encoding.ASCII.GetBytes("data"));
            writer.Write(subchunk2Size);

            // write samples
            foreach (var sample in samples)
            {
                short intSample = (short)(Mathf.Clamp(sample, -1.0f, 1.0f) * short.MaxValue);
                writer.Write(intSample);
            }

            return stream.ToArray();
        }
    }

    /// <summary>
    /// WAV byte[] → float[]
    /// </summary>
    private static float[] ConvertFromWav(byte[] wavBytes, out int channels, out int sampleRate)
    {
        using (MemoryStream stream = new MemoryStream(wavBytes))
        using (BinaryReader reader = new BinaryReader(stream))
        {
            // Read WAV header
            string riff = new string(reader.ReadChars(4));
            int chunkSize = reader.ReadInt32();
            string wave = new string(reader.ReadChars(4));
            string fmt = new string(reader.ReadChars(4));
            int subchunk1Size = reader.ReadInt32();
            short audioFormat = reader.ReadInt16();
            channels = reader.ReadInt16();
            sampleRate = reader.ReadInt32();
            int byteRate = reader.ReadInt32();
            short blockAlign = reader.ReadInt16();
            short bitsPerSample = reader.ReadInt16();

            // Skip potential extra bytes in fmt chunk
            if (subchunk1Size > 16)
            {
                reader.ReadBytes(subchunk1Size - 16);
            }

            string dataChunkId = new string(reader.ReadChars(4));
            int dataSize = reader.ReadInt32();

            byte[] data = reader.ReadBytes(dataSize);

            int totalSamples = dataSize / (bitsPerSample / 8);
            float[] samples = new float[totalSamples];

            for (int i = 0; i < totalSamples; i++)
            {
                short sample = BitConverter.ToInt16(data, i * 2);
                samples[i] = sample / 32768f;
            }

            return samples;
        }
    }
}
