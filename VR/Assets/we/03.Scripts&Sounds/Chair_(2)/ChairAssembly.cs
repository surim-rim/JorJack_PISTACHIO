using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairAssembly : MonoBehaviour
{
    public static ChairAssembly instance;
    public GameObject completeChair; // 완성된 의자 모델
    private int nailsHitCount = 0;
    private int totalNails = 4; // 총 4개의 못

    private void Awake()
    {
        instance = this;
        completeChair.SetActive(false); // 처음에는 비활성화
    }

    public void NailHit()
    {
        nailsHitCount++;
        if (nailsHitCount >= totalNails)
        {
            CompleteChair();
        }
    }

    private void CompleteChair()
    {
        completeChair.SetActive(true); // 완성된 의자 활성화
        Debug.Log("의자가 완성되었습니다!");
    }
}
