using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WhistleButton : MonoBehaviour
{
    public AudioSource whistleSound; // 피리 소리
    private Vector3 startPos;
    public float pressThreshold = 0.02f; // 눌리는 거리

    private void Start()
    {
        startPos = transform.localPosition; // 버튼의 초기 위치 저장
    }

    private void Update()
    {
        if (transform.localPosition.y < startPos.y - pressThreshold)
        {
            PlayWhistle();
            transform.localPosition = startPos; // 버튼 원래 위치로 되돌리기
        }
    }

    private void PlayWhistle()
    {
        if (whistleSound != null && !whistleSound.isPlaying)
        {
            whistleSound.Play();
        }
    }
}
