using UnityEngine;

public class SaveAPIKey : MonoBehaviour
{
    [SerializeField] private string openAiApiKey; // OpenAI API 키 입력
    [SerializeField] private string subscriptionKey; // 추가적인 구독 키 입력

    void Awake()
    {
        // OpenAI API Key 저장
        if (!string.IsNullOrEmpty(openAiApiKey) && openAiApiKey.StartsWith("sk-"))
        {
            PlayerPrefs.SetString("OpenAI_API_Key", openAiApiKey.Trim());
            Debug.Log("✅ OpenAI API 키가 성공적으로 저장되었습니다!");
        }
        else
        {
            Debug.LogError("⚠️ 올바른 OpenAI API 키를 입력해야 합니다! (sk-로 시작해야 합니다)");
        }

        // Subscription Key 저장
        if (!string.IsNullOrEmpty(subscriptionKey))
        {
            PlayerPrefs.SetString("Subscription_Key", subscriptionKey.Trim());
            Debug.Log("✅ Subscription 키가 성공적으로 저장되었습니다!");
        }
        else
        {
            Debug.LogError("⚠️ 올바른 Subscription 키를 입력해야 합니다!");
        }

        PlayerPrefs.Save(); // 반드시 호출해야 저장됨
    }
}
