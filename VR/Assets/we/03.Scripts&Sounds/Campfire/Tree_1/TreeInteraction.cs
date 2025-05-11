using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeInteraction : MonoBehaviour
{
    public GameObject logPrefab;         // 장작 프리팹
    public Transform spawnPoint;         // 장작 생성 위치
    public AudioClip chopSound;          // 도끼질 소리
    public AudioClip fallSound;          // 나무 쓰러지는 소리
    public float fallDuration = 2f;      // 쓰러지는 시간
    public float fallAngle = 70f;        // 회전 각도

    private AudioSource audioSource;
    private bool isChopped = false;
    private bool isFalling = false;
    private Vector3 fallDirection;

    void Start()
    {
        // AudioSource 세팅
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Axe") && !isChopped)
        {
            isChopped = true;

            // 장작 생성
            Instantiate(logPrefab, spawnPoint.position, spawnPoint.rotation);

            // 충돌 방향 → 반대 방향으로 쓰러뜨리기
            fallDirection = (transform.position - collision.transform.position).normalized;

            // 도끼질 소리 재생
            if (chopSound != null)
                audioSource.PlayOneShot(chopSound);

            // 쓰러지기 시작
            StartCoroutine(FallTree());

            Debug.Log("장작 생성됨, 나무 쓰러지는 중");
        }
    }

    IEnumerator FallTree()
    {
        isFalling = true;

        // 쓰러지는 소리
        if (fallSound != null)
            audioSource.PlayOneShot(fallSound);

        Quaternion startRot = transform.rotation;

        // 목표 회전 방향 계산 (fallDirection을 기준으로 회전축 구함)
        Vector3 axis = Vector3.Cross(Vector3.up, fallDirection);
        Quaternion endRot = Quaternion.AngleAxis(fallAngle, axis) * startRot;

        float t = 0;
        while (t < fallDuration)
        {
            t += Time.deltaTime;
            float lerp = t / fallDuration;
            transform.rotation = Quaternion.Slerp(startRot, endRot, lerp);
            yield return null;
        }

        isFalling = false;
    }
}
