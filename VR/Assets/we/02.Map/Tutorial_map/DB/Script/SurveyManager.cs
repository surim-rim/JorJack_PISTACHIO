using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;

public class SurveyManager : MonoBehaviour
{
    public GameObject placeQuestionPanel;
    public GameObject weatherQuestionPanel;

    public Button mountainButton;
    public Button oceanButton;
    public Button spaceButton;
    public Button nextButton1;

    public Button sunnyButton;
    public Button rainyButton;
    public Button snowyButton;
    public Button nextButton2;

    public TMP_Text placeWarningText;
    public TMP_Text weatherWarningText;

    private string selectedPlace = "";
    private string selectedWeather = "";
    private DatabaseReference databaseReference;
    private string userId;

    private Button selectedPlaceButton = null;
    private Button selectedWeatherButton = null;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            if (task.Result == DependencyStatus.Available)
            {
                databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
                ResetDatabase();
            }
            else
            {
                Debug.LogError("❌ Firebase dependencies not available: " + task.Result);
            }
        });
    }

    void ResetDatabase()
    {
        databaseReference.RemoveValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsCompleted)
            {
                Debug.Log("✅ Database reset successfully.");
                InitializeUser();
                SetupButtonListeners();
                InitializeUI();
            }
            else
            {
                Debug.LogError("❌ Failed to reset database: " + task.Exception);
            }
        });
    }

    void InitializeUser()
    {
        userId = System.Guid.NewGuid().ToString();
        PlayerPrefs.SetString("UserId", userId);
        PlayerPrefs.Save();
        Debug.Log($"🔥 User ID: {userId}");
    }

    void SetupButtonListeners()
    {
        nextButton1.onClick.RemoveAllListeners();
        nextButton2.onClick.RemoveAllListeners();

        mountainButton.onClick.AddListener(() => SelectPlace("Mountain", mountainButton));
        oceanButton.onClick.AddListener(() => SelectPlace("Ocean", oceanButton));
        spaceButton.onClick.AddListener(() => SelectPlace("Space", spaceButton));
        nextButton1.onClick.AddListener(SavePlaceAndNext);

        sunnyButton.onClick.AddListener(() => SelectWeather("Sunny", sunnyButton));
        rainyButton.onClick.AddListener(() => SelectWeather("Rainy", rainyButton));
        snowyButton.onClick.AddListener(() => SelectWeather("Snowy", snowyButton));
        nextButton2.onClick.AddListener(SaveWeatherAndFinish);
    }

    void InitializeUI()
    {
        placeQuestionPanel.SetActive(true);
        weatherQuestionPanel.SetActive(false);

        placeWarningText.gameObject.SetActive(false);
        weatherWarningText.gameObject.SetActive(false);
    }

    void SelectPlace(string place, Button clickedButton)
    {
        if (selectedPlaceButton != null)
        {
            ResetButtonColor(selectedPlaceButton);
        }

        selectedPlace = place;
        selectedPlaceButton = clickedButton;
        SetButtonColor(clickedButton);
        placeWarningText.gameObject.SetActive(false);
    }

    void SelectWeather(string weather, Button clickedButton)
    {
        if (selectedWeatherButton != null)
        {
            ResetButtonColor(selectedWeatherButton);
        }

        selectedWeather = weather;
        selectedWeatherButton = clickedButton;
        SetButtonColor(clickedButton);
        weatherWarningText.gameObject.SetActive(false);
    }

    void SavePlaceAndNext()
    {
        if (!string.IsNullOrEmpty(selectedPlace))
        {
            databaseReference.Child("SurveyResponses").Child(userId).Child("place").SetValueAsync(selectedPlace)
                .ContinueWithOnMainThread(task => {
                    if (task.IsCompleted)
                    {
                        Debug.Log($"✅ Place '{selectedPlace}' saved for UserId: {userId}");
                    }
                    else
                    {
                        Debug.LogError("❌ Failed to save place: " + task.Exception);
                    }
                });

            placeQuestionPanel.SetActive(false);
            weatherQuestionPanel.SetActive(true);
        }
        else
        {
            placeWarningText.text = "Please select a place.";
            placeWarningText.gameObject.SetActive(true);
        }
    }

    void SaveWeatherAndFinish()
    {
        if (!string.IsNullOrEmpty(selectedWeather))
        {
            databaseReference.Child("SurveyResponses").Child(userId).Child("weather").SetValueAsync(selectedWeather)
                .ContinueWithOnMainThread(task =>
                {
                    if (task.IsCompleted)
                    {
                        Debug.Log("✅ Weather saved successfully! Survey finished.");
                        FindObjectOfType<MapLoader>().LoadSelectedScene();
                    }
                    else
                    {
                        Debug.LogError("❌ Failed to save weather: " + task.Exception);
                    }
                });
        }
        else
        {
            weatherWarningText.text = "Please select a weather.";
            weatherWarningText.gameObject.SetActive(true);
        }
    }

    void SetButtonColor(Button button)
    {
        ColorBlock cb = button.colors;
        cb.normalColor = new Color(0.3f, 0.7f, 1f);
        cb.selectedColor = new Color(0.3f, 0.7f, 1f);
        cb.highlightedColor = new Color(0.5f, 0.8f, 1f);
        button.colors = cb;
    }

    void ResetButtonColor(Button button)
    {
        ColorBlock cb = button.colors;
        cb.normalColor = Color.white;
        cb.selectedColor = Color.white;
        cb.highlightedColor = Color.white;
        button.colors = cb;
    }
}