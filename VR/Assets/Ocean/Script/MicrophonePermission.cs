using UnityEngine;
using UnityEngine.Android;

public class MicrophonePermission : MonoBehaviour
{
    void Start()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Debug.Log(" 마이크 권한이 없습니다. 요청 중...");
            Permission.RequestUserPermission(Permission.Microphone);
        }
        else
        {
            Debug.Log(" 마이크 권한이 이미 부여됨!");
        }
    }
}
