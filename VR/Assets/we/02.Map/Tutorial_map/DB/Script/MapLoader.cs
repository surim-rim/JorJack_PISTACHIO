using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Database;

public class MapLoader : MonoBehaviour
{
    private DatabaseReference databaseReference;
    private string userId;
    private string selectedPlace = "";
    private string selectedWeather = "";

    void Start()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        userId = PlayerPrefs.GetString("UserId", "");

        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("❌ User ID is empty!");
            return;
        }

        Debug.Log($"🔥 Firebase Database initialized! User ID: {userId}");

        // 🔥 Firebase 데이터 변경 감지
        databaseReference.Child("SurveyResponses").Child(userId).ValueChanged += OnSurveyDataChanged;
    }

    // 🔥 장소와 날씨 데이터가 변경될 때 호출됨
    private void OnSurveyDataChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError("❌ Firebase Database Error: " + args.DatabaseError.Message);
            return;
        }

        Debug.Log("🔥 Firebase ValueChanged triggered!");

        // 전체 데이터 확인 (강제 출력)
        Debug.Log($"📌 Firebase에서 받은 데이터: {args.Snapshot.GetRawJsonValue()}");

        if (!args.Snapshot.Exists || args.Snapshot.Value == null)
        {
            Debug.LogWarning("⚠️ 데이터가 없습니다. 새로운 데이터 저장 중일 수 있습니다...");
            return;
        }

        if (args.Snapshot.Child("place").Exists)
        {
            selectedPlace = args.Snapshot.Child("place").Value.ToString().Trim();
            Debug.Log($"🚀 Retrieved place from Firebase: {selectedPlace}");
        }
        else
        {
            Debug.LogWarning("⚠️ 장소 데이터가 존재하지 않습니다.");
        }

        if (args.Snapshot.Child("weather").Exists)
        {
            selectedWeather = args.Snapshot.Child("weather").Value.ToString().Trim();
            Debug.Log($"🌦️ Retrieved weather from Firebase: {selectedWeather}");
        }
        else
        {
            Debug.LogWarning("⚠️ 날씨 데이터가 존재하지 않습니다.");
        }
    }


    public void LoadSelectedScene()
    {
        string sceneToLoad = $"{selectedPlace}";
        if (SceneExists(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogError($"❌ Scene '{sceneToLoad}' does not exist in Build Settings!");
        }
    }

    private string FormatSceneName(string name)
    {
        if (string.IsNullOrEmpty(name)) return "";
        return name.Substring(0, 1).ToUpper() + name.Substring(1).ToLower();
    }

    public bool SceneExists(string sceneName)
    {
        bool sceneExists = false;
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneNameInBuild = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            if (sceneNameInBuild == sceneName)
            {
                sceneExists = true;
                break;
            }
        }
        return sceneExists;
    }
}