using System.Collections;
using UnityEngine;

public class FishCooker : MonoBehaviour
{
    public GameObject rawFishPrefab;  // 생선 모델 (생선 Prefab)
    public GameObject cookedFishPrefab; // 익힌 생선 모델 (익힌 생선 Prefab)

    private void OnTriggerEnter(Collider other)
    {
        // 물고기가 들어오면 감지
        if (other.CompareTag("Fish"))
        {
            StartCoroutine(CookFish(other.gameObject));
        }
    }

    private IEnumerator CookFish(GameObject fish)
    {
        // 3초 대기
        yield return new WaitForSeconds(3f);

        // 기존 물고기 제거
        Vector3 fishPosition = fish.transform.position;
        Quaternion fishRotation = fish.transform.rotation;
        Destroy(fish);

        // 익은 물고기 생성
        Instantiate(cookedFishPrefab, fishPosition, fishRotation); //woeifjowefoiejf
    }
}
