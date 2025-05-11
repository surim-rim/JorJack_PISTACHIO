using System.IO; // 파일 저장에 필요한 네임스페이스
using UnityEngine;

public class AudioRecorder : MonoBehaviour
{
    private AudioClip recordedClip;

    public void StartRecording()
    {
        // 마이크로 5초 동안 녹음
        recordedClip = Microphone.Start(null, false, 5, 16000);
        Debug.Log("녹음 시작");
    }

    public void StopAndSaveRecording()
    {
        if (Microphone.IsRecording(null))
        {
            // 녹음 종료
            Microphone.End(null);
            Debug.Log("녹음 종료");

            if (recordedClip != null)
            {
                // AudioClip을 WAV로 변환
                byte[] audioData = WavUtility.FromAudioClip(recordedClip);

                // Assets 폴더에 WAV 파일 저장
                string filePath = "Assets/RecordedAudio";
                File.WriteAllBytes(filePath, audioData);
                Debug.Log($"녹음된 오디오 데이터를 저장했습니다: {filePath}");
            }
            else
            {
                Debug.LogError("녹음된 AudioClip이 없습니다.");
            }
        }
        else
        {
            Debug.LogError("녹음이 진행 중이 아닙니다.");
        }
    }
}