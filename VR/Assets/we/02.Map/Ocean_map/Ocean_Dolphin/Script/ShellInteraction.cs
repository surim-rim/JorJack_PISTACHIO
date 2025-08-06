using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class ShellInteraction : MonoBehaviour
{
    public AudioSource audioSource; // 소리
    public GameObject uiPanel; // UI
    private Coroutine hideUICoroutine;

    private void Start()
    {
        if (uiPanel != null)
            uiPanel.SetActive(false); // 처음엔 UI 숨김
    }

    public void OnGrab()
    {
        if (audioSource != null)
            audioSource.Play(); // 소리 재생

        if (uiPanel != null)
        {
            uiPanel.SetActive(true); // UI 활성화

            // 이전 코루틴이 있다면 멈추기
            if (hideUICoroutine != null)
                StopCoroutine(hideUICoroutine);

            // 새로 시작
            hideUICoroutine = StartCoroutine(HideUIAfterSeconds(3f));
        }
    }

    public void OnRelease()
    {
        if (uiPanel != null)
        {
            // 잡자마자 놓는 경우에도 바로 끄기
            uiPanel.SetActive(false);

            if (hideUICoroutine != null)
            {
                StopCoroutine(hideUICoroutine);
                hideUICoroutine = null;
            }
        }
    }

    private IEnumerator HideUIAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        if (uiPanel != null)
            uiPanel.SetActive(false);
    }
}
