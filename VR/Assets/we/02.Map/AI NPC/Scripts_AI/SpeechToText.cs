using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class SpeechToText : MonoBehaviour
{
    private string subscriptionKey;
    private string region = "koreacentral";
    private string endpoint = "https://koreacentral.stt.speech.microsoft.com/speech/recognition/conversation/cognitiveservices/v1?language=ko-KR";

    private AudioClip recordedClip;
    private bool isRecording = false;
    private EmotionAnalyzer emotionAnalyzer; // 감정 분석기 추가

    void Awake()
    {
        // 🔹 API 키 불러오기
        subscriptionKey = PlayerPrefs.GetString("Azure_STT_Key", "GJnk1Mi7ne6919pItamlznVq5U9vZQt7HYQ7To2SLTOIGGigmVm8JQQJ99AKACNns7RXJ3w3AAAYACOGU3xo").Trim();
        emotionAnalyzer = gameObject.AddComponent<EmotionAnalyzer>(); // 감정 분석 추가

        if (string.IsNullOrEmpty(subscriptionKey))
        {
            Debug.LogError("❌ Azure STT API 키가 설정되지 않았습니다. PlayerPrefs를 확인하세요.");
        }
        else
        {
            Debug.Log("✅ Azure STT API 키가 정상적으로 불러와졌습니다!");
        }
    }

    public void StartRecording(System.Action<string, string> onResult)
    {
        if (isRecording) return;

        isRecording = true;
        Debug.Log("🎤 사용자가 말을 시작할 때까지 기다리는 중...");

        StartCoroutine(WaitForSpeechAndRecord(onResult));
    }

    private IEnumerator WaitForSpeechAndRecord(System.Action<string, string> onResult)
    {
        float silenceThreshold = 0.002f;
        float maxWaitTime = 5f;
        float waitTime = 0f;

        recordedClip = Microphone.Start(null, false, 10, 16000);
        if (!Microphone.IsRecording(null))
        {
            Debug.LogError("❌ 마이크 녹음이 시작되지 않았습니다.");
            onResult?.Invoke("음성을 인식할 수 없습니다.", "neutral");
            yield break;
        }

        Debug.Log("🎤 마이크 녹음 시작됨");

        while (waitTime < maxWaitTime)
        {
            float[] samples = new float[1024];
            recordedClip.GetData(samples, 0);
            float currentVolume = GetRMSVolume(samples);

            if (currentVolume > silenceThreshold)
            {
                Debug.Log("🎤 사용자가 말을 시작했습니다! 녹음 유지...");
                break;
            }

            waitTime += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(2f);

        Debug.Log("🛑 자동 녹음 종료 조건 감지 중...");
        StartCoroutine(CheckForSilenceAndStop(onResult));
    }

    private IEnumerator CheckForSilenceAndStop(System.Action<string, string> onResult)
    {
        float silenceThreshold = 0.002f;
        float silenceDuration = 0f;
        float minRecordTime = 2f;
        float elapsedTime = 0f;

        while (Microphone.IsRecording(null))
        {
            float[] samples = new float[recordedClip.samples];
            recordedClip.GetData(samples, 0);
            float currentVolume = GetRMSVolume(samples);

            if (currentVolume < silenceThreshold)
            {
                silenceDuration += Time.deltaTime;
                if (silenceDuration > 1.5f && elapsedTime > minRecordTime)
                {
                    Debug.Log("🛑 사용자가 말을 멈췄습니다. 녹음 종료.");
                    break;
                }
            }
            else
            {
                silenceDuration = 0f;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Microphone.End(null);
        isRecording = false;

        if (recordedClip == null || recordedClip.samples == 0)
        {
            Debug.LogError("❌ 녹음된 AudioClip이 비어 있습니다. STT 요청을 보내지 않습니다.");
            onResult?.Invoke("음성을 인식할 수 없습니다.", "neutral");
            yield break;
        }

        Debug.Log($"🎤 녹음된 AudioClip 샘플 수: {recordedClip.samples}");

        byte[] audioData = WavUtility.FromAudioClip(recordedClip);

        if (audioData == null || audioData.Length == 0)
        {
            Debug.LogError("❌ 변환된 오디오 데이터가 비어 있습니다.");
            onResult?.Invoke("음성을 인식할 수 없습니다.", "neutral");
            yield break;
        }

        StartCoroutine(RecognizeSpeech(audioData, onResult));
    }

    private IEnumerator RecognizeSpeech(byte[] audioData, System.Action<string, string> onResult)
    {
        using (UnityWebRequest request = UnityWebRequest.Put(endpoint, audioData))
        {
            request.method = UnityWebRequest.kHttpVerbPOST;
            request.SetRequestHeader("Ocp-Apim-Subscription-Key", subscriptionKey);
            request.SetRequestHeader("Content-Type", "audio/wav");
            request.SetRequestHeader("Accept", "application/json");

            Debug.Log("📡 STT API 요청 전송...");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("✅ STT API 호출 성공");
                string recognizedText = ParseJsonResponse(request.downloadHandler.text);
                Debug.Log($"📝 STT 결과: {recognizedText}");

                string detectedEmotion = emotionAnalyzer.DetectEmotion(recognizedText);
                Debug.Log($"🧠 감정 분석 결과: {detectedEmotion}");

                onResult?.Invoke(recognizedText, detectedEmotion);
            }
            else
            {
                Debug.LogError($"❌ STT API 요청 실패: {request.error}");
                Debug.LogError($"🛑 STT API 응답: {request.downloadHandler.text}");
                onResult?.Invoke("음성을 인식할 수 없습니다.", "neutral");
            }
        }
    }

    private string ParseJsonResponse(string json)
    {
        try
        {
            SpeechRecognitionResult result = JsonUtility.FromJson<SpeechRecognitionResult>(json);
            return !string.IsNullOrEmpty(result.DisplayText) ? result.DisplayText : "음성을 인식할 수 없습니다.";
        }
        catch (Exception ex)
        {
            Debug.LogError($"⚠️ JSON 파싱 오류: {ex.Message}");
            return "음성을 인식할 수 없습니다.";
        }
    }

    private float GetRMSVolume(float[] samples)
    {
        float sum = 0f;
        foreach (float sample in samples)
        {
            sum += sample * sample;
        }
        return Mathf.Sqrt(sum / samples.Length);
    }

    [System.Serializable]
    private class SpeechRecognitionResult
    {
        public string RecognitionStatus;
        public string DisplayText;
    }
}
