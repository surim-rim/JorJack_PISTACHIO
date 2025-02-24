/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKYPRO_Sun_Rotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 0.5f;

    void FixedUpdate()
    {
        Rotate();
    }

    void Rotate()
    {
        //transform.localEulerAngles.x + ((rotationSpeed / 10) * Time.deltaTime)
        transform.localEulerAngles = new Vector3(30 + Time.time * rotationSpeed, 20, 0);
    }
}

*/



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKYPRO_Sun_Rotator : MonoBehaviour
{
    // 태양 오브젝트의 세 가지 특정 각도를 정의
    private Vector3[] sunRotations = new Vector3[]
    {
        new Vector3(70, 180, 0), // 낮
        new Vector3(2, -90, 0),  // 저녁
        new Vector3(-30, -90, 0) // 밤
    };

    private int currentRotationIndex = 0;

    void Update()
    {
        // "L" 키를 누르면 다음 각도로 변경
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ChangeSunRotation();
        }
    }

    // 현재 각도에서 다음 각도로 변경하는 메서드
    void ChangeSunRotation()
    {
        // 현재 인덱스 순환
        currentRotationIndex = (currentRotationIndex + 1) % sunRotations.Length;
        // 새로운 각도를 적용
        transform.localEulerAngles = sunRotations[currentRotationIndex];
    }
}
