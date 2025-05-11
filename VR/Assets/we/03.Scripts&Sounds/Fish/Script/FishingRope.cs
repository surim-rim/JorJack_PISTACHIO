using UnityEngine;

public class FishingRope : MonoBehaviour
{
    public Transform fishingRodTip; // 낚싯대 끝
    public Transform fishingHook;   // 낚싯바늘
    public GameObject fishingLine;  // 낚싯줄 오브젝트

    void Update()
    {
        // 낚싯줄의 위치와 회전을 업데이트
        Vector3 direction = fishingHook.position - fishingRodTip.position;
        fishingLine.transform.position = fishingRodTip.position + direction / 2;
        fishingLine.transform.rotation = Quaternion.LookRotation(direction);
        fishingLine.transform.localScale = new Vector3(0.01f, 0.01f, direction.magnitude);
    }
}
