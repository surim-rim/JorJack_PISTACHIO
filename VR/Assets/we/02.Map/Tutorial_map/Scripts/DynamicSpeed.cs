using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit; // XR Interaction Toolkit 관련 함수 사용

public class DynamicSpeed : MonoBehaviour
{
    public ContinuousMoveProviderBase moveProvider; // 프로바이더 객체 받음
    public float normalSpeed = 2f;
    public float boostSpeed = 5f;

    void Update()
    {
        if(Input.GetKey(KeyCode.LeftShift)) // LeftShift 키를 누르면
        {
            moveProvider.moveSpeed = boostSpeed;
        }
        else
        {
            moveProvider.moveSpeed = normalSpeed;
        }
    }
}
