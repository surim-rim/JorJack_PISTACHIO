using UnityEngine;

public class FireworkUnit : MonoBehaviour
{
    public ParticleSystem fx; // 파티클 시스템
    public AudioClip sound;   // 소리
    private AudioSource audioSource; // AudioSource 컴포넌트
    private bool isPlaying = false;

    void Start()
    {
        // AudioSource 설정
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = sound;
        audioSource.loop = false; // 반복 안 함
    }

    void Update()
    {
        // 파티클이 재생될 때마다 소리도 함께 나게
        if (fx.isPlaying && !isPlaying)
        {
            isPlaying = true;
            PlaySound();
        }

        // 파티클이 멈추면 소리도 멈춤
        if (!fx.isPlaying && isPlaying)
        {
            isPlaying = false;
        }
    }

    // 소리 재생
    void PlaySound()
    {
        if (audioSource != null)
            audioSource.Play();
    }
}
