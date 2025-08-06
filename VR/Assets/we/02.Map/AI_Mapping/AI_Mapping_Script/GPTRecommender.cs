using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GPTRecommender : MonoBehaviour
{
    [System.Serializable]
    public class GPTRequest
    {
        public string model = "openai/gpt-3.5-turbo"; // ✅ 유효한 모델로 교체
        public GPTMessage[] messages;
    }

    [System.Serializable]
    public class GPTMessage
    {
        public string role;
        public string content;
    }

    [System.Serializable]
    public class GPTResponse
    {
        public GPTChoice[] choices;
    }

    [System.Serializable]
    public class GPTChoice
    {
        public GPTMessage message;
    }

    [System.Serializable]
    public class GPTMapResult
    {
        public string mapName;
        public string reason;
    }

    public string openRouterApiKey = "sk-or-xxxxx"; // ✅ 본인의 유효한 키로 교체하세요

    public TMP_Text loadingText;
    public Button nextButton;
    public TMP_Text mapNameText;
    public TMP_Text reasonText;
    public UnityEngine.UI.Image mapImage;
    public Sprite swissMapSprite;
    public Sprite jejuMapSprite;
    public Button toMainButton;
    public Button toMapButton;
    public GameObject mapResultPanel;
    public GameObject loadingPanel;

    private string recommendedMapName = "";
    public static GPTRecommender Instance;
    private bool isRequestInProgress = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (toMainButton != null)
            toMainButton.onClick.AddListener(() => SceneManager.LoadScene("MainScene"));

        if (toMapButton != null)
            toMapButton.onClick.AddListener(GoToRecommendedMap);

        HideAllUI();
    }

    public void RecommendMap(string surveyJson)
    {
        if (isRequestInProgress) return;

        if (string.IsNullOrWhiteSpace(surveyJson))
        {
            Debug.LogWarning("RecommendMap: 설문 응답이 비어 있습니다.");
            ShowError("⚠ 설문 응답이 없습니다. 먼저 설문을 완료해주세요.");
            return;
        }

        isRequestInProgress = true;
        Debug.Log("RecommendMap 호출됨, 전달된 surveyJson: " + surveyJson);

        ShowLoading("맵을 연결 중 입니다...\n잠시만 기다려주세요.");

        if (nextButton != null)
            nextButton.interactable = false;

        StartCoroutine(SendGPTRequest(surveyJson, 0));
    }

    IEnumerator SendGPTRequest(string surveyJson, int retryCount)
    {
        string prompt = BuildPrompt(surveyJson);

        GPTRequest requestData = new GPTRequest
        {
            model = "openai/gpt-3.5-turbo", // ✅ 가용 모델
            messages = new GPTMessage[]
            {
                new GPTMessage
                {
                    role = "system",
                    content = "너는 사용자의 설문 응답을 기반으로 힐링 VR 맵을 추천하는 사용자 맞춤형 AI 설계사야."
                },
                new GPTMessage
                {
                    role = "user",
                    content = prompt
                }
            }
        };

        string json = JsonUtility.ToJson(requestData);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest www = new UnityWebRequest("https://openrouter.ai/api/v1/chat/completions", "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", "Bearer " + openRouterApiKey);
            www.SetRequestHeader("HTTP-Referer", "https://example.com");
            www.SetRequestHeader("X-Title", "MyUnityGame");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string rawText = www.downloadHandler.text.Trim();
                Debug.Log("✅ GPT 응답 원문: " + rawText);

                // ✅ HTML 반환 감지 및 차단
                if (rawText.StartsWith("<!DOCTYPE html") || rawText.Contains("<html"))
                {
                    ShowError("⚠ 서버에서 오류 페이지를 반환했습니다. 모델을 확인해주세요.");
                    Debug.LogError("⚠ HTML 페이지가 반환되었습니다. 모델이 잘못되었을 수 있습니다.");
                    isRequestInProgress = false;
                    RestoreUI();
                    yield break;
                }

                GPTResponse response = JsonUtility.FromJson<GPTResponse>(rawText);
                string resultContent = response.choices[0].message.content.Trim();
                Debug.Log("GPT 모델 응답 내용: " + resultContent);

                GPTMapResult mapResult = null;
                try
                {
                    string jsonOnly = ExtractJson(resultContent);
                    Debug.Log("📦 추출된 JSON: " + jsonOnly);
                    mapResult = JsonUtility.FromJson<GPTMapResult>(jsonOnly);
                }
                catch
                {
                    Debug.LogError("❌ JSON 파싱 실패. GPT 응답:\n" + resultContent);
                    ShowError("⚠ GPT 응답 파싱에 실패했습니다. 다시 시도해주세요.");
                    isRequestInProgress = false;
                    RestoreUI();
                    yield break;
                }

                if (mapResult != null)
                {
                    recommendedMapName = mapResult.mapName;
                    ShowMapResult(mapResult.mapName, mapResult.reason);
                }
            }
            else
            {
                Debug.LogError("❌ GPT 오류: " + www.responseCode + " / " + www.error);
                ShowError("⚠ 추천에 실패했습니다. 잠시 후 다시 시도해주세요.");

                if (www.responseCode == 429 && retryCount < 3)
                {
                    yield return new WaitForSeconds(5f);
                    yield return SendGPTRequest(surveyJson, retryCount + 1);
                    yield break;
                }
            }
        }

        RestoreUI();
        isRequestInProgress = false;
    }

    private string ExtractJson(string rawText)
    {
        int startIndex = rawText.IndexOf('{');
        int endIndex = rawText.LastIndexOf('}');
        if (startIndex >= 0 && endIndex >= 0 && endIndex > startIndex)
        {
            return rawText.Substring(startIndex, endIndex - startIndex + 1);
        }
        return null;
    }

    private void ShowLoading(string message)
    {
        HideAllUI();
        if (loadingPanel != null) loadingPanel.SetActive(true);
        if (loadingText != null)
        {
            loadingText.text = message;
            loadingText.gameObject.SetActive(true);
        }
    }

    private void ShowError(string message)
    {
        HideAllUI();
        if (loadingPanel != null) loadingPanel.SetActive(true);
        if (loadingText != null)
        {
            loadingText.text = message;
            loadingText.gameObject.SetActive(true);
        }
    }

    private void ShowMapResult(string mapName, string reason)
    {
        if (loadingPanel != null) loadingPanel.SetActive(false);
        HideAllUI();

        if (mapNameText != null)
        {
            mapNameText.text = mapName;
            mapNameText.gameObject.SetActive(true);
        }

        if (reasonText != null)
        {
            reasonText.text = reason;
            reasonText.gameObject.SetActive(true);
        }

        if (mapImage != null)
        {
            if (mapName.Contains("스위스"))
                mapImage.sprite = swissMapSprite;
            else if (mapName.Contains("제주"))
                mapImage.sprite = jejuMapSprite;
        }

        if (toMainButton != null) toMainButton.gameObject.SetActive(true);
        if (toMapButton != null) toMapButton.gameObject.SetActive(true);
        if (mapResultPanel != null) mapResultPanel.SetActive(true);
    }

    private void HideAllUI()
    {
        if (loadingPanel != null) loadingPanel.SetActive(false);
        if (loadingText != null) loadingText.gameObject.SetActive(false);
        if (mapNameText != null) mapNameText.gameObject.SetActive(false);
        if (reasonText != null) reasonText.gameObject.SetActive(false);
        if (mapResultPanel != null) mapResultPanel.SetActive(false);
        if (toMainButton != null) toMainButton.gameObject.SetActive(false);
        if (toMapButton != null) toMapButton.gameObject.SetActive(false);
    }

    private void RestoreUI()
    {
        if (nextButton != null)
            nextButton.interactable = true;
    }

    private void GoToRecommendedMap()
    {
        if (recommendedMapName.Contains("스위스"))
            SceneManager.LoadScene("SwissScene");
        else if (recommendedMapName.Contains("제주"))
            SceneManager.LoadScene("JejuScene");
        else
            Debug.LogWarning("추천된 맵을 인식할 수 없습니다: " + recommendedMapName);
    }

    private string BuildPrompt(string surveyJson)
    {
        return
            "다음 설문 응답을 기반으로 적절한 힐링 VR 맵을 추천해주세요.\n\n" +
            $"설문 응답:\n{surveyJson}\n\n" +
            "맵 목록:\n" +
            "1. 스위스 설산 맵: 호수, 낚시, 오로라, 캠프파이어, 암벽등반, 의자 제작, 물고기 구워먹기, 캠핑.\n" +
            "2. 제주도 배 맵: 바다, 배, 바다 낚시, 물속 체험, 돌고래 관찰, 폭죽놀이.\n\n" +
            "다음 JSON 형식으로 한 줄로만, 다른 말 없이 출력해주세요:\n" +
            "{ \"mapName\": \"[맵 이름]\", \"reason\": \"[추천 이유]\" }";
    }
}
