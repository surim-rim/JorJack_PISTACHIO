using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillAttach : MonoBehaviour
{
    public Transform attachPoint; // 드릴 앞부분 위치

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("NASA")) // 여기만 바꿨어!
        {
            GameObject nasa = collision.gameObject;

            // Rigidbody 비활성화해서 물리 작용 멈추기
            Rigidbody rb = nasa.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }

            // 위치와 회전 조정
            nasa.transform.position = attachPoint.position;
            nasa.transform.rotation = attachPoint.rotation;

            // 드릴에 붙이기
            nasa.transform.SetParent(attachPoint);
        }
    }
}
