using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairBuilder : MonoBehaviour
{
    public GameObject[] chairStages; // 의자 단계별 오브젝트 배열 (다리, 좌석, 등받이)
    public GameObject completeChair; // 완성된 의자 오브젝트
    private int currentStage = 0;

    public void BuildNextStage()
    {
        // 마지막 충돌 이전까지는 단계를 순서대로 활성화
        if (currentStage < chairStages.Length)
        {
            chairStages[currentStage].SetActive(true); // 현재 단계 활성화
            currentStage++;
        }
        else
        {
            // 마지막 충돌 시 모든 구성 요소를 비활성화하고 완성된 의자 활성화
            foreach (GameObject stage in chairStages)
            {
                stage.SetActive(false); // 기존 구성 요소 비활성화
            }
            completeChair.SetActive(true); // 완성된 의자 활성화
        }
    }
}
