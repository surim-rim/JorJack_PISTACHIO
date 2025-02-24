using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxChanger : MonoBehaviour
{
    public Material[] skyboxMaterials;  // 변경할 스카이박스 배열
    public float changeInterval = 10f;  // 변경 주기 (초 단위)

    private int currentIndex = 0;

    void Start()
    {
        if (skyboxMaterials.Length > 0)
        {
            RenderSettings.skybox = skyboxMaterials[currentIndex];  // 초기 설정
        }

        InvokeRepeating("ChangeSkybox", changeInterval, changeInterval);
    }

    void ChangeSkybox()
    {
        currentIndex = (currentIndex + 1) % skyboxMaterials.Length; // 다음 스카이박스로 변경
        RenderSettings.skybox = skyboxMaterials[currentIndex];

        DynamicGI.UpdateEnvironment();  // 글로벌 조명 업데이트
    }
}
