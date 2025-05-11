using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CampfireManager : MonoBehaviour
{
    public XRSocketInteractor socket1; // Socket Interactor_1
    public XRSocketInteractor socket2; // Socket Interactor_2
    public XRSocketInteractor socket3; // Socket Interactor_3

    public GameObject matchObject1; // 첫 번째 성냥 오브젝트
    public GameObject matchObject2; // 두 번째 성냥 오브젝트

    private void Start()
    {
        // 성냥 오브젝트를 초기 비활성화
        matchObject1.SetActive(false);
        matchObject2.SetActive(false);
    }

    private void Update()
    {
        // 세 Socket Interactor가 모두 장작을 가지고 있는지 체크
        if (IsSocketFilled(socket1) && IsSocketFilled(socket2) && IsSocketFilled(socket3))
        {
            // 성냥 오브젝트 두 개 모두 활성화
            matchObject1.SetActive(true);
            matchObject2.SetActive(true);
        }
        else
        {
            // 조건 미충족 시 성냥 오브젝트 비활성화
            matchObject1.SetActive(false);
            matchObject2.SetActive(false);
        }
    }

    private bool IsSocketFilled(XRSocketInteractor socket)
    {
        // Socket에 오브젝트가 연결되어 있는지 확인
        return socket.hasSelection;
    }
}
