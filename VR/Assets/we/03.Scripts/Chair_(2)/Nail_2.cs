using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nail_2 : MonoBehaviour
{
    private bool isHit = false; // 못이 이미 맞았는지 확인
    private Vector3 targetPosition; // 못이 박힐 목표 위치
    private float moveSpeed = 0.1f; // 못이 박히는 속도

    void Start()
    {
        targetPosition = transform.position - new Vector3(0, 0.05f, 0); // Y축으로 0.05 내려감
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isHit && collision.gameObject.CompareTag("Hammer")) // 망치와 충돌 확인
        {
            isHit = true;
            StartCoroutine(SinkNail());
            ChairAssembly.instance.NailHit(); // 의자 조립 체크
        }
    }

    private System.Collections.IEnumerator SinkNail()
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.001f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed);
            yield return null;
        }
    }
}
