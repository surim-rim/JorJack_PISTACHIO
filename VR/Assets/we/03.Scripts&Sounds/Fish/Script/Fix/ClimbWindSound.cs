using UnityEngine;

public class ClimbWindSound : MonoBehaviour
{
    public AudioSource windSound; // AudioSource 컴포넌트
    private bool hasPlayed = false; // 소리 한번만 재생되게

    void OnTriggerEnter(Collider other)
    {
        // 만약 "손"이나 "클라이밍 장비"가 충돌했다면
        if (other.CompareTag("PlayerHand") && !hasPlayed)
        {
            windSound.Play(); // 바람 소리 시작
            windSound.loop = true; // 계속 반복
            hasPlayed = true; // 소리 한번만 재생
        }
    }
}
    /*void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
        {
            windSound.Stop(); // 손이 떨어지면 바람 소리 멈춤
            hasPlayed = false; // 다시 재생 가능하도록 설정
        }
    }
}*/
