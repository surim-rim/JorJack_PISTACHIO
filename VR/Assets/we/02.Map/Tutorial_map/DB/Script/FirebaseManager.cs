using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    private FirebaseFirestore db;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            if (task.Result == DependencyStatus.Available)
            {
                db = FirebaseFirestore.DefaultInstance;
                Debug.Log("Firebase 연결 성공!");
            }
            else
            {
                Debug.LogError("Firebase 연결 실패: " + task.Result);
            }
        });
    }
}
