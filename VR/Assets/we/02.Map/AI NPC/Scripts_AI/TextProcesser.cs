using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class TextProcessor : MonoBehaviour
{
    private string openAiApiKey;
    private string endpoint = "https://openrouter.ai/api/v1/chat/completions"; // OpenRouter API URL
    private EmotionAnalyzer emotionAnalyzer;

    private float lastApiCallTime = 0f;
    private float apiCallCooldown = 5f; // 5초 쿨다운
    private int maxRequestsPerMinute = 5; // 1분에 최대 5번 요청 가능
    private int requestCount = 0;
    private float resetTime;

    private int npcAge = 55;
    private string npcGender = "여성";
    private string npcSituation = "너와 스위스 산 정상에서 캠프파이어를 하면서 오로라를 감상하는 상황";
    private string npcRelationship = "친구 관계";

    void Awake()
    {
        emotionAnalyzer = gameObject.AddComponent<EmotionAnalyzer>(); // 감정 분석 클래스 추가

        openAiApiKey = PlayerPrefs.GetString("OpenAI_API_Key", "GJnk1Mi7ne6919pItamlznVq5U9vZQt7HYQ7To2SLTOIGGigmVm8JQQJ99AKACNns7RXJ3w3AAAYACOGU3xo").Trim();
        Debug.Log($"🔑 불러온 API Key: {openAiApiKey}");

        if (string.IsNullOrEmpty(openAiApiKey) || openAiApiKey.Length < 20)
        {
            Debug.LogError("⚠️ OpenRouter API 키가 올바르게 설정되지 않았습니다. 환경 설정을 확인하세요.");
        }
        else
        {
            Debug.Log("✅ API 키가 정상적으로 불러와졌습니다!");
        }
    }

    public IEnumerator GenerateResponse(string userInput, string emotion, System.Action<string> onResult)
    {
        Debug.Log($"🧠 감정 분석 결과: {emotion}");

        string emotionContext = GetEmotionContext(emotion); // 감정에 맞는 문맥 생성

        string systemMessage = $"너의 나이는 {npcAge}세이고, 성별은 {npcGender}야. 지금 {npcSituation}이고, 우리는 {npcRelationship}야. " +
                               "대답을 너무 길게 하지 말고, 짧고 간결하게 대화해 줘. 한 문장 또는 최대 2~3문장으로만 답변해 줘." +
                               $"{emotionContext}";

        string userMessage = $"사용자: {userInput}";

        string requestBody = "{\"model\": \"deepseek/deepseek-chat:free\", \"messages\": [{" +
            "\"role\": \"system\", \"content\": \"" + systemMessage + "\"}, " +
            "{\"role\": \"user\", \"content\": \"" + userMessage + "\"}], " +
            "\"max_tokens\": 50}";

        byte[] bodyData = Encoding.UTF8.GetBytes(requestBody);
        using (UnityWebRequest request = new UnityWebRequest(endpoint, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyData);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Authorization", "Bearer " + openAiApiKey);
            request.SetRequestHeader("Content-Type", "application/json");

            Debug.Log($"📤 OpenRouter API 요청 보내는 중... {requestBody}");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseText = request.downloadHandler.text;
                Debug.Log($"✅ OpenRouter API 응답: {responseText}");

                string gptResponse = ParseGPTResponse(responseText);
                onResult?.Invoke(gptResponse);
            }
            else
            {
                Debug.LogError($"❌ OpenRouter API 요청 실패: {request.responseCode} {request.error}");
                Debug.LogError($"🛑 응답 본문: {request.downloadHandler.text}");
                onResult?.Invoke("서버 오류가 발생했어요. 다시 시도해 주세요.");
            }
        }
    }


    private string GetEmotionContext(string emotion)
    {
        switch (emotion)
        {
            case "anger": return "지금 사용자는 화가 난 것 같아. 차분하게 응대해 줘.";
            case "fear": return "사용자는 지금 두려움을 느끼고 있어. 위로하는 말을 해 줘.";
            case "anticipation": return "사용자는 기대감이 있는 상태야. 긍정적인 반응을 보여줘.";
            case "surprise": return "사용자가 놀랐어. 친절하게 반응해 줘.";
            case "joy": return "사용자가 행복해 보이네! 함께 기뻐해 줘.";
            case "sadness": return "사용자가 슬픈 것 같아. 공감하며 위로해 줘.";
            case "trust": return "사용자가 신뢰하고 있어. 신뢰를 더욱 강화할 말을 해 줘.";
            case "disgust": return "사용자가 역겨움을 느껴. 이를 이해하고 부드럽게 반응해 줘.";
            default: return "감정을 감지하지 못했어. 평범한 대화를 이어가 줘.";
        }
    }

    private string ParseGPTResponse(string json)
    {
        if (string.IsNullOrEmpty(json))
        {
            Debug.LogError("⚠️ GPT 응답이 비어 있습니다.");
            return "응답을 이해할 수 없어요.";
        }

        try
        {
            OpenAiResponse response = JsonUtility.FromJson<OpenAiResponse>(json);
            if (response != null && response.choices != null && response.choices.Length > 0 && response.choices[0].message != null)
            {
                return response.choices[0].message.content;
            }
            else
            {
                Debug.LogError("⚠️ GPT 응답에서 메시지를 찾을 수 없습니다.");
                return "응답을 이해할 수 없어요.";
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"⚠️ JSON 파싱 실패: {ex.Message}");
            return "응답을 이해할 수 없어요.";
        }
    }

    [System.Serializable]
    private class OpenAiResponse
    {
        public Choice[] choices;
    }

    [System.Serializable]
    private class Choice
    {
        public Message message;
    }

    [System.Serializable]
    private class Message
    {
        public string content;
    }
}
