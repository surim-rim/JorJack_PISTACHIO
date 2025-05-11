using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchIgnition : MonoBehaviour
{
    public GameObject fireEffectPrefab; // 불 이펙트 프리팹
    public Transform firePoint;         // 성냥 앞부분 위치
    private GameObject fireInstance;    // 생성된 이펙트

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Matchbox") && fireInstance == null)
        {
            // 이펙트 생성하고 위치 고정
            fireInstance = Instantiate(fireEffectPrefab, firePoint.position, firePoint.rotation, firePoint);
        }
    }
}