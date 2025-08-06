using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class TextToSpeech : MonoBehaviour
{
    private string subscriptionKey;
    private string region = "koreacentral";
    private string endpoint = "https://koreacentral.tts.speech.microsoft.com/cognitiveservices/v1";

    void Awake()
    {
        Debug.Log("PlayerPrefs에서 Azure TTS API 키 불러오는 중...");

        subscriptionKey = PlayerPrefs.GetString("Azure_TTS_Key", "GJnk1Mi7ne6919pItamlznVq5U9vZQt7HYQ7To2SLTOIGGigmVm8JQQJ99AKACNns7RXJ3w3AAAYACOGU3xo").Trim();

        if (string.IsNullOrEmpty(subscriptionKey))
        {
            Debug.LogError("Azure TTS API 키가 설정되지 않았습니다. PlayerPrefs를 확인하세요.");
        }
        else
        {
            Debug.Log("Azure TTS API 키가 정상적으로 불러와졌습니다: " + subscriptionKey);
        }
    }


    public IEnumerator SynthesizeSpeech(string text, System.Action<AudioClip> onAudioReady)
    {
        if (string.IsNullOrEmpty(subscriptionKey))
        {
            Debug.LogError("API 키가 설정되지 않았습니다. TTS 요청을 보낼 수 없습니다.");
            onAudioReady?.Invoke(null);
            yield break;
        }

        string requestBody = $@"
        <speak version='1.0' xmlns='http://www.w3.org/2001/10/synthesis' xml:lang='ko-KR'>
            <voice name='ko-KR-InJoonNeural'>{text}</voice>
        </speak>";



        byte[] data = System.Text.Encoding.UTF8.GetBytes(requestBody);

        using (UnityWebRequest request = new UnityWebRequest(endpoint, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(data);
            request.downloadHandler = new DownloadHandlerBuffer();

            // 🔹 API 키가 null이 아닐 때만 요청 헤더 추가
            request.SetRequestHeader("Ocp-Apim-Subscription-Key", subscriptionKey);
            request.SetRequestHeader("Content-Type", "application/ssml+xml");
            request.SetRequestHeader("X-Microsoft-OutputFormat", "riff-16khz-16bit-mono-pcm"); // WAV 형식 지정

            Debug.Log($"TTS API 요청 전송 중...");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("TTS API 호출 성공");

                if (request.downloadHandler.data.Length > 0)
                {
                    byte[] audioData = request.downloadHandler.data;
                    AudioClip audioClip = WavUtility.ToAudioClip(audioData);

                    if (audioClip != null)
                    {
                        onAudioReady?.Invoke(audioClip);
                    }
                    else
                    {
                        Debug.LogError("오디오 변환 실패");
                        onAudioReady?.Invoke(null);
                    }
                }
                else
                {
                    Debug.LogError("서버에서 빈 오디오 데이터를 반환했습니다.");
                    onAudioReady?.Invoke(null);
                }
            }
            else
            {
                Debug.LogError($"TTS API 요청 실패: {request.error}");
                onAudioReady?.Invoke(null);
            }
        }
    }
}
