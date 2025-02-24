using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nail : MonoBehaviour
{
    public float hammerDepth = 0.1f; // 한 번 칠 때 내려가는 거리
    public ChairBuilder chairBuilder; // 의자 제어 스크립트 연결
    private bool isHammered = false;
    private static int hammerCount = 0; // 총 충돌 횟수
    private const int maxHammerCount = 4; // 최대 충돌 횟수

    private void OnCollisionEnter(Collision collision)
    {
        // 망치와 충돌 & 이미 못 박히지 않은 상태 & 최대 충돌 횟수 미만일 때만 동작
        if (collision.gameObject.CompareTag("Hammer") && !isHammered && hammerCount < maxHammerCount)
        {
            isHammered = true;
            hammerCount++; // 충돌 횟수 증가
            StartCoroutine(HammerNail());
            chairBuilder.BuildNextStage();
        }
    }

    private IEnumerator HammerNail()
    {
        Vector3 targetPosition = transform.position - new Vector3(0, hammerDepth, 0);
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5f);
            yield return null;
        }
        isHammered = false; // 다시 못 박힐 수 있도록 초기화 (단, hammerCount는 유지됨)
    }
}
