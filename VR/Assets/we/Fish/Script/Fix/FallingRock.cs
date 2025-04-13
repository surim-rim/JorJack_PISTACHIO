using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FallingRock : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private Rigidbody playerRigidbody;
    private Rigidbody rockRigidbody;
    private CharacterController playerCharacterController;

    private ConfigurableJoint configurableJoint;

    private void Start()
    {
        Debug.Log("[FallingRock] Start() 실행됨", gameObject);

        grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable == null)
        {
            Debug.LogError("[오류] XRGrabInteractable을 찾을 수 없음!", gameObject);
            return;
        }

        Debug.Log("[FallingRock] XRGrabInteractable 감지됨", gameObject);

        grabInteractable.onSelectEntered.AddListener(OnGrab);
        grabInteractable.onSelectExited.AddListener(OnGrabEnd);

        Debug.Log("[FallingRock] 이벤트 등록 완료!", gameObject);

        rockRigidbody = GetComponent<Rigidbody>();
        rockRigidbody.useGravity = false; // 처음엔 돌에 중력 적용 안함
        rockRigidbody.isKinematic = true; // 돌을 물리엔진에서 처리하지 않음
    }

    public void OnGrab(XRBaseInteractor interactor)
    {
        Debug.Log(" 돌 잡힘! Interactor: " + interactor.name, gameObject);

        playerRigidbody = interactor.GetComponentInParent<Rigidbody>();
        playerCharacterController = interactor.GetComponentInParent<CharacterController>();

        if (playerRigidbody != null && rockRigidbody != null)
        {
            Debug.Log(" 플레이어 Rigidbody 찾음: " + playerRigidbody.gameObject.name);

            if (playerCharacterController != null)
            {
                Debug.Log(" 플레이어 CharacterController 찾음!");
                playerCharacterController.enabled = false;
            }

            playerRigidbody.useGravity = true;
            playerRigidbody.isKinematic = false;

            rockRigidbody.useGravity = true;
            rockRigidbody.isKinematic = false;

            // 회전 제한
            playerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;

            // 조인트 추가
            configurableJoint = playerRigidbody.gameObject.AddComponent<ConfigurableJoint>();
            configurableJoint.connectedBody = rockRigidbody;
        }
        else
        {
            Debug.LogError("Rigidbody를 찾을 수 없음!", gameObject);
        }
    }

    private void OnGrabEnd(XRBaseInteractor interactor)
    {
        // Character Controller 다시 활성화
        if (playerCharacterController != null)
        {
            playerCharacterController.enabled = true;
        }

        // ConfigurableJoint 연결 해제
        if (configurableJoint != null)
        {
            Destroy(configurableJoint);
        }

        // 플레이어의 Rigidbody를 다시 Kinematic으로 설정하여 물리적 충돌 방지
        if (playerRigidbody != null)
        {
            playerRigidbody.isKinematic = true; // 물리엔진 충돌 방지
            playerRigidbody.useGravity = false; // 중력 제거
        }

        // 돌의 Rigidbody를 Kinematic으로 설정하여 떨어지지 않게 만듬
        if (rockRigidbody != null)
        {
            rockRigidbody.isKinematic = true;
            rockRigidbody.useGravity = false;
        }
    }
}