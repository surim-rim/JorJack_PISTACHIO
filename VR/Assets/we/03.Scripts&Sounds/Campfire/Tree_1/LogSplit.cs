using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogSplit : MonoBehaviour
{
    public GameObject smallLogPrefab; // 작은 장작 Prefab
    public float splitOffset = 0.2f; // 분리되는 거리
    public float splitForce = 2f; // 분리될 때 힘
    public AudioClip chopSound; // 효과음 클립
    private AudioSource audioSource; // 오디오 소스

    void Start()
    {
        // AudioSource 컴포넌트를 동적으로 추가하거나 이미 있다면 가져오기
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Axe"))
        {
            // 사운드 재생
            if (chopSound != null)
            {
                audioSource.PlayOneShot(chopSound);
            }

            // 장작 위치와 회전
            Vector3 logPosition = transform.position;
            Quaternion logRotation = transform.rotation;

            // 기존 장작 제거
            Destroy(gameObject);

            // 작은 장작 두 개 생성
            GameObject log1 = Instantiate(smallLogPrefab, logPosition + new Vector3(-splitOffset, 0, 0), logRotation);
            GameObject log2 = Instantiate(smallLogPrefab, logPosition + new Vector3(splitOffset, 0, 0), logRotation);

            // 힘 적용
            Rigidbody rb1 = log1.GetComponent<Rigidbody>();
            Rigidbody rb2 = log2.GetComponent<Rigidbody>();

            if (rb1 != null) rb1.AddForce(new Vector3(-splitForce, 2f, 0), ForceMode.Impulse);
            if (rb2 != null) rb2.AddForce(new Vector3(splitForce, 2f, 0), ForceMode.Impulse);

            Debug.Log("장작이 두 개로 분리됨!");
        }
    }
}
