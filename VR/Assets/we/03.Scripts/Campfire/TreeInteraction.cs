using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeInteraction : MonoBehaviour
{
    public GameObject logPrefab; // 장작 Prefab
    public Transform spawnPoint; // 장작 생성 위치
    private bool isChopped = false; // 한 번만 생성되도록 방지

    void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트가 "Axe" 태그를 가진 경우
        if (collision.gameObject.CompareTag("Axe") && !isChopped)
        {
            isChopped = true; // 중복 실행 방지

            // 장작 생성
            Instantiate(logPrefab, spawnPoint.position, spawnPoint.rotation);

            // 나무 오브젝트 제거 (선택 사항)
            Destroy(gameObject, 0.5f); // 0.5초 후 나무 제거
        }
    }
}
