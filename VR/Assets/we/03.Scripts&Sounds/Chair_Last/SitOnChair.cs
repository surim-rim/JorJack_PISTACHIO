using UnityEngine;
using UnityEngine.UI;

public class SitOnChair : MonoBehaviour
{
    public Transform seatPosition;  // 의자의 앉는 위치
    public Quaternion seatRotation; // 의자의 회전 값
    public Transform player;        // 플레이어 캐릭터
    public Button sitButton;        // "의자에 앉기" 버튼
    public Button standUpButton;    // "일어나기" 버튼

    private Vector3 originalPosition;  // 앉기 전 플레이어 위치 저장
    private Quaternion originalRotation; // 앉기 전 플레이어 회전 저장
    private bool isSitting = false;

    void Start()
    {
        sitButton.onClick.AddListener(Sit);
        standUpButton.onClick.AddListener(StandUp);
        standUpButton.gameObject.SetActive(false); // 처음에는 "일어나기" 버튼 숨김
    }

    void Sit()
    {
        if (!isSitting)
        {
            // 현재 플레이어 위치 & 회전값 저장 (일어날 때 복구)
            originalPosition = player.position;
            originalRotation = player.rotation;

            // 의자 위치 & 회전값 적용
            player.position = seatPosition.position;
            player.rotation = seatRotation; // 추가된 회전 값 적용
            isSitting = true;

            // UI 버튼 상태 변경
            sitButton.gameObject.SetActive(false);
            standUpButton.gameObject.SetActive(true);
        }
    }

    void StandUp()
    {
        if (isSitting)
        {
            // 저장된 원래 위치 & 회전으로 복귀
            player.position = originalPosition;
            player.rotation = originalRotation;
            isSitting = false;

            // UI 버튼 상태 변경
            standUpButton.gameObject.SetActive(false);
            sitButton.gameObject.SetActive(true);
        }
    }
}
