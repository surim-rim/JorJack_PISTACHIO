using System;
using System.IO;
using UnityEngine;

public static class WavUtility
{
    // Convert AudioClip to WAV byte array
    public static byte[] FromAudioClip(AudioClip audioClip)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            WriteHeader(stream, audioClip);
            WriteData(stream, audioClip);
            return stream.ToArray();
        }
    }

    // Convert WAV byte array to AudioClip
    public static AudioClip ToAudioClip(byte[] wavFileData)
    {
        using (MemoryStream stream = new MemoryStream(wavFileData))
        using (BinaryReader reader = new BinaryReader(stream))
        {
            // Skip unnecessary headers
            reader.BaseStream.Seek(22, SeekOrigin.Begin); // Channels
            int channels = reader.ReadInt16();

            reader.BaseStream.Seek(24, SeekOrigin.Begin); // Sample Rate
            int sampleRate = reader.ReadInt32();

            reader.BaseStream.Seek(40, SeekOrigin.Begin); // Data Size
            int dataSize = reader.ReadInt32();

            // Read audio samples
            float[] audioData = new float[dataSize / 2];
            for (int i = 0; i < audioData.Length; i++)
            {
                audioData[i] = reader.ReadInt16() / 32768f; // Convert to float (-1 to 1)
            }

            // Create AudioClip
            AudioClip audioClip = AudioClip.Create("Generated WAV", audioData.Length, channels, sampleRate, false);
            audioClip.SetData(audioData, 0);
            return audioClip;
        }
    }

    private static void WriteHeader(Stream stream, AudioClip clip)
    {
        if (!stream.CanWrite)
        {
            throw new ArgumentException("Stream is not writable.");
        }

        int fileSize = 36 + clip.samples * clip.channels * 2;
        short audioFormat = 1; // PCM
        short channels = (short)clip.channels;
        int sampleRate = clip.frequency;
        short bitsPerSample = 16;
        int byteRate = sampleRate * channels * bitsPerSample / 8;
        short blockAlign = (short)(channels * bitsPerSample / 8);

        using (BinaryWriter writer = new BinaryWriter(stream, System.Text.Encoding.UTF8, true)) // leaveOpen=true
        {
            writer.Write("RIFF".ToCharArray());
            writer.Write(fileSize);
            writer.Write("WAVE".ToCharArray());
            writer.Write("fmt ".ToCharArray());
            writer.Write(16); // Subchunk1Size
            writer.Write(audioFormat);
            writer.Write(channels);
            writer.Write(sampleRate);
            writer.Write(byteRate);
            writer.Write(blockAlign);
            writer.Write(bitsPerSample);
            writer.Write("data".ToCharArray());
            writer.Write(clip.samples * clip.channels * 2);
        }
    }

    private static void WriteData(Stream stream, AudioClip clip)
    {
        if (!stream.CanWrite)
        {
            throw new ArgumentException("Stream is not writable.");
        }

        float[] audioData = new float[clip.samples * clip.channels];
        clip.GetData(audioData, 0);

        using (BinaryWriter writer = new BinaryWriter(stream, System.Text.Encoding.UTF8, true)) // leaveOpen=true
        {
            foreach (float sample in audioData)
            {
                short intSample = (short)(Mathf.Clamp(sample, -1.0f, 1.0f) * 32767);
                writer.Write(intSample);
            }
        }
    }
}