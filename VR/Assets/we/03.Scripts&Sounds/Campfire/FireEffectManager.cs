using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEffectManager : MonoBehaviour
{
    public GameObject fireEffectPrefab; // 불 이펙트 프리팹
    public Transform effectSpawnPoint; // 이펙트 생성 위치

    private void OnTriggerEnter(Collider other)
    {
        // 성냥 오브젝트와 충돌했는지 확인
        if (other.CompareTag("Match"))
        {
            // 불 이펙트 생성
            Instantiate(fireEffectPrefab, effectSpawnPoint.position, effectSpawnPoint.rotation);

            // Debug 메시지 출력 (필요하면 제거 가능)
            Debug.Log("Fire effect activated!");
        }
    }
}
