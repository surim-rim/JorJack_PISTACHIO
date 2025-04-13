using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screw : MonoBehaviour
{
    public float rotationSpeed = 200f; // 나사 회전 속도
    public float moveSpeed = 0.02f; // 나사가 들어가는 속도
    public float maxDepth = 0.5f; // 나사가 들어갈 최대 깊이
    private bool isTurning = false;
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position; // 초기 위치 저장
    }

    void Update()
    {
        if (isTurning)
        {
            // 나사가 회전하면서 아래로 이동
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
            transform.position -= new Vector3(0, moveSpeed * Time.deltaTime, 0);

            // 최대 깊이에 도달하면 멈춤
            if (initialPosition.y - transform.position.y >= maxDepth)
            {
                isTurning = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Drill")) // 드릴과 충돌하면 회전 시작
        {
            isTurning = true;
        }
    }
}
