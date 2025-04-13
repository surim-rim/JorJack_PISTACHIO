using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogSplit : MonoBehaviour
{
    public GameObject smallLogPrefab; // 작은 장작 Prefab
    public float splitOffset = 0.2f; // 분리되는 거리
    public float splitForce = 2f; // 분리될 때 힘

    void OnCollisionEnter(Collision collision)
    {
        // 도끼와 충돌했는지 확인
        if (collision.gameObject.CompareTag("Axe"))
        {
            // 원래 장작의 위치와 회전값을 가져옴
            Vector3 logPosition = transform.position;
            Quaternion logRotation = transform.rotation;

            // 기존 장작 제거
            Destroy(gameObject);

            // 작은 장작 두 개 생성
            GameObject log1 = Instantiate(smallLogPrefab, logPosition + new Vector3(-splitOffset, 0, 0), logRotation);
            GameObject log2 = Instantiate(smallLogPrefab, logPosition + new Vector3(splitOffset, 0, 0), logRotation);

            // Rigidbody 추가 및 분리되는 힘 적용
            Rigidbody rb1 = log1.GetComponent<Rigidbody>();
            Rigidbody rb2 = log2.GetComponent<Rigidbody>();

            if (rb1 != null) rb1.AddForce(new Vector3(-splitForce, 2f, 0), ForceMode.Impulse);
            if (rb2 != null) rb2.AddForce(new Vector3(splitForce, 2f, 0), ForceMode.Impulse);

            Debug.Log("장작이 두 개로 분리됨!");
        }
    }
}
