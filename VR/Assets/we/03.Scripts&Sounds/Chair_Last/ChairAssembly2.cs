/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChairAssembly2 : MonoBehaviour
{
    public GameObject chairLeg;   // 의자 다리1 오브젝트
    public GameObject chairLeg2;  // 의자 다리2 오브젝트
    public GameObject chairCloth; // 의자 천 오브젝트
    public GameObject chairScrew; // 의자 나사 오브젝트
    public GameObject newChairLeg;  // 새 의자 다리1
    public GameObject newChairLeg2; // 새 의자 다리2
    public GameObject newChairCloth; // 새 의자 천

    public AudioClip drillImpactSound; // 드릴 충돌 소리
    private AudioSource audioSource; // 오디오 소스
    private int drillHitCount = 0; // 드릴 충돌 횟수
    public float shakeIntensity = 0.05f; // 진동 강도
    public float shakeDuration = 0.1f; // 진동 시간

    // SitOnChair 스크립트 참조
    public SitOnChair sitOnChairScript;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); // 오디오 소스 초기화
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Wood":  // 나무 충돌 → 의자 다리1 활성화 + 나무 제거
                chairLeg.SetActive(true);
                Destroy(other.gameObject); // 나무 삭제
                Debug.Log("의자 다리1 생성!");
                break;

            case "Wood2":  // 나무 충돌 → 의자 다리2 활성화 + 나무 제거
                chairLeg2.SetActive(true);
                Destroy(other.gameObject); // 나무 삭제
                Debug.Log("의자 다리2 생성!");
                break;

            case "Cloth": // 천 충돌 → 의자 천 활성화 + 천 제거
                chairCloth.SetActive(true);
                Destroy(other.gameObject); // 천 삭제
                Debug.Log("의자 천 생성!");
                break;

            case "Screw": // 나사 충돌 → 의자 나사 활성화 + 나사 제거
                chairScrew.SetActive(true);
                Destroy(other.gameObject); // 나사 삭제
                Debug.Log("의자 나사 생성!");
                break;

            case "Drill": // 드릴 충돌 → 기존 다리 제거 & 새로운 다리 생성
                drillHitCount++;

                // 충돌 시 소리 재생
                if (audioSource != null && drillImpactSound != null)
                {
                    audioSource.PlayOneShot(drillImpactSound);
                }

                // 충격 애니메이션 및 진동 효과
                if (drillHitCount == 1)
                {
                    Destroy(chairLeg); // 기존 의자 다리1 삭제
                    newChairLeg.SetActive(true); // 새로운 의자 다리1 생성
                    StartCoroutine(ShakeObject(newChairLeg)); // 진동 시작
                    Debug.Log("의자 다리1 업그레이드!");
                }
                else if (drillHitCount == 2)
                {
                    Destroy(chairLeg2); // 기존 의자 다리2 삭제
                    newChairLeg2.SetActive(true); // 새로운 의자 다리2 생성
                    StartCoroutine(ShakeObject(newChairLeg2)); // 진동 시작
                    Debug.Log("의자 다리2 업그레이드!");
                }
                else if (drillHitCount == 3)  // 3번째 충돌 시 천 생성
                {
                    Destroy(chairCloth); // 기존 의자 천 삭제
                    newChairCloth.SetActive(true); // 새로운 의자 천 생성
                    StartCoroutine(ShakeObject(newChairCloth)); // 진동 시작
                    Debug.Log("의자 천 업그레이드!");

                    // 의자에 앉기 버튼 활성화
                    if (sitOnChairScript != null)
                    {
                        sitOnChairScript.sitButton.gameObject.SetActive(true);
                    }
                }

                // 드릴은 사라지지 않음
                break;
        }
    }

    // 진동 효과를 위한 코루틴
    private IEnumerator ShakeObject(GameObject obj)
    {
        Vector3 originalPosition = obj.transform.position;

        float elapsedTime = 0f;
        while (elapsedTime < shakeDuration)
        {
            float x = Random.Range(-shakeIntensity, shakeIntensity);
            float y = Random.Range(-shakeIntensity, shakeIntensity);
            obj.transform.position = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj.transform.position = originalPosition; // 원래 위치로 돌아감
    }
}
*/

// 2번째 버전
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChairAssembly2 : MonoBehaviour
{
    public GameObject chairLeg;
    public GameObject chairLeg2;
    public GameObject chairCloth;
    public GameObject chairScrew1; // 추가: 나사1
    public GameObject chairScrew2; // 추가: 나사2
    public GameObject chairScrew3; // 추가: 나사3
    public GameObject chairScrew4; // 추가: 나사4
    public GameObject newChairLeg;
    public GameObject newChairLeg2;
    public GameObject newChairCloth;
    public GameObject nail; // 추가: 못 오브젝트

    public AudioClip drillImpactSound;
    private AudioSource audioSource;
    private int drillHitCount = 0;
    public float shakeIntensity = 0.05f;
    public float shakeDuration = 0.1f;

    public SitOnChair sitOnChairScript;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Wood":
                chairLeg.SetActive(true);
                Destroy(other.gameObject);
                Debug.Log("의자 다리1 생성!");
                break;

            case "Wood2":
                chairLeg2.SetActive(true);
                Destroy(other.gameObject);
                Debug.Log("의자 다리2 생성!");
                break;

            case "Cloth":
                chairCloth.SetActive(true);
                Destroy(other.gameObject);
                Debug.Log("의자 천 생성!");
                break;

            case "Drill":
                drillHitCount++;

                if (audioSource != null && drillImpactSound != null)
                {
                    audioSource.PlayOneShot(drillImpactSound);
                }

                if (drillHitCount == 1)
                {
                    Destroy(chairScrew1); // 나사1 삭제
                    Destroy(chairLeg);
                    newChairLeg.SetActive(true);
                    StartCoroutine(ShakeObject(newChairLeg));
                    Debug.Log("의자 다리1 업그레이드!");
                }
                else if (drillHitCount == 2)
                {
                    Destroy(chairScrew2); // 나사2 삭제
                    Destroy(chairLeg2);
                    newChairLeg2.SetActive(true);
                    StartCoroutine(ShakeObject(newChairLeg2));
                    Debug.Log("의자 다리2 업그레이드!");
                }
                else if (drillHitCount == 3)
                {
                    Destroy(chairScrew3); // 나사3 삭제
                    Destroy(chairCloth);
                    newChairCloth.SetActive(true);
                    StartCoroutine(ShakeObject(newChairCloth));
                    Debug.Log("의자 천 업그레이드!");

                    
                }
                else if (drillHitCount == 4)
                {
                    Destroy(chairScrew4); // 나사4 삭제
                    if (nail != null)
                    {
                        nail.SetActive(true); // 못 생성
                        StartCoroutine(ShakeObject(nail));
                        Debug.Log("못 생성!");
                    }

                    if (sitOnChairScript != null)
                    {
                        sitOnChairScript.sitButton.gameObject.SetActive(true);
                    }
                }
                break;
        }
    }

    private IEnumerator ShakeObject(GameObject obj)
    {
        Vector3 originalPosition = obj.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            float x = Random.Range(-shakeIntensity, shakeIntensity);
            float y = Random.Range(-shakeIntensity, shakeIntensity);
            obj.transform.position = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj.transform.position = originalPosition;
    }
}



/*3번째버전_내가 그냥 바꿔본 거_실행은 잘 됨_다른 스크립트랑 안맞아서 패스
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChairAssembly2 : MonoBehaviour
{
    public GameObject chairLeg;   // 의자 다리1 오브젝트
    public GameObject chairLeg2;  // 의자 다리2 오브젝트
    public GameObject chairCloth; // 의자 천 오브젝트
    
    public GameObject newChairLeg;  // 새 의자 다리1
    public GameObject newChairLeg2; // 새 의자 다리2
    public GameObject newChairCloth; // 새 의자 천

    public GameObject nail; // 추가: 못 오브젝트

    public AudioClip drillImpactSound; // 드릴 충돌 소리
    private AudioSource audioSource; // 오디오 소스
    private int drillHitCount = 0; // 드릴 충돌 횟수
    public float shakeIntensity = 0.05f; // 진동 강도
    public float shakeDuration = 0.1f; // 진동 시간

    // SitOnChair 스크립트 참조
    public SitOnChair sitOnChairScript;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); // 오디오 소스 초기화
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Wood":  // 나무 충돌 → 의자 다리1 활성화 + 나무 제거
                chairLeg.SetActive(true);
                Destroy(other.gameObject); // 나무 삭제
                Debug.Log("의자 다리1 생성!");
                break;

            case "Wood2":  // 나무 충돌 → 의자 다리2 활성화 + 나무 제거
                chairLeg2.SetActive(true);
                Destroy(other.gameObject); // 나무 삭제
                Debug.Log("의자 다리2 생성!");
                break;

            case "Cloth": // 천 충돌 → 의자 천 활성화 + 천 제거
                chairCloth.SetActive(true);
                Destroy(other.gameObject); // 천 삭제
                Debug.Log("의자 천 생성!");
                break;

            

            case "Drill": // 드릴 충돌 → 기존 다리 제거 & 새로운 다리 생성
                drillHitCount++;

                // 충돌 시 소리 재생
                if (audioSource != null && drillImpactSound != null)
                {
                    audioSource.PlayOneShot(drillImpactSound);
                }

                // 충격 애니메이션 및 진동 효과
                if (drillHitCount == 1)
                {
                    Destroy(chairLeg); // 기존 의자 다리1 삭제
                    newChairLeg.SetActive(true); // 새로운 의자 다리1 생성
                    StartCoroutine(ShakeObject(newChairLeg)); // 진동 시작
                    Debug.Log("의자 다리1 업그레이드!");
                }
                else if (drillHitCount == 2)
                {
                    Destroy(chairLeg2); // 기존 의자 다리2 삭제
                    newChairLeg2.SetActive(true); // 새로운 의자 다리2 생성
                    StartCoroutine(ShakeObject(newChairLeg2)); // 진동 시작
                    Debug.Log("의자 다리2 업그레이드!");
                }
                else if (drillHitCount == 3)  // 3번째 충돌 시 천 생성
                {
                    Destroy(chairCloth); // 기존 의자 천 삭제
                    newChairCloth.SetActive(true); // 새로운 의자 천 생성
                    StartCoroutine(ShakeObject(newChairCloth)); // 진동 시작
                    Debug.Log("의자 천 업그레이드!");

                }
                else if (drillHitCount == 4)
                {
                    if (nail != null)
                    {
                        nail.SetActive(true); // 못 생성
                        StartCoroutine(ShakeObject(nail));
                        Debug.Log("못 생성!");
                    }

                    // 의자에 앉기 버튼 활성화
                    if (sitOnChairScript != null)
                    {
                        sitOnChairScript.sitButton.gameObject.SetActive(true);
                    }
                }

                // 드릴은 사라지지 않음
                break;
        }
    }

    // 진동 효과를 위한 코루틴
    private IEnumerator ShakeObject(GameObject obj)
    {
        Vector3 originalPosition = obj.transform.position;

        float elapsedTime = 0f;
        while (elapsedTime < shakeDuration)
        {
            float x = Random.Range(-shakeIntensity, shakeIntensity);
            float y = Random.Range(-shakeIntensity, shakeIntensity);
            obj.transform.position = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj.transform.position = originalPosition; // 원래 위치로 돌아감
    }
}
*/