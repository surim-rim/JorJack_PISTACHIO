using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class Flute : MonoBehaviour
{
    public Transform head; // HMD 또는 입 위치
    public float playDistance = 0.2f; // 입과의 거리 기준
    public AudioSource fluteSound;
    public Animator dolphinAnimator1;
    public Animator dolphinAnimator2;
    public Animator dolphinAnimator3;
    public AudioSource dolphinSound;

    private bool hasPlayed = false;
    private bool dolphinSpawned = false;
    private bool fluteFinished = false;

    private XRGrabInteractable grabInteractable;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    void Update()
    {
        if (fluteFinished) return;

        bool isGrabbed = grabInteractable != null && grabInteractable.isSelected;

        bool isNearMouth = Vector3.Distance(transform.position, head.position) < playDistance;

        if (isGrabbed && isNearMouth)
        {
            if (!hasPlayed)
            {
                fluteSound.Play();
                hasPlayed = true;
                fluteFinished = true;

                if (!dolphinSpawned)
                {
                    StartCoroutine(SpawnDolphinsSequentially());
                }
            }
        }
        else
        {
            hasPlayed = false;
        }
    }

    IEnumerator SpawnDolphinsSequentially()
    {
        dolphinSpawned = true;

        yield return new WaitForSeconds(3f);
        if (dolphinSound != null) dolphinSound.Play();
        if (dolphinAnimator1 != null)
        {
            dolphinAnimator1.gameObject.SetActive(true);
            dolphinAnimator1.SetTrigger("Jump");
        }

        yield return new WaitForSeconds(3f);
        if (dolphinSound != null) dolphinSound.Play();
        if (dolphinAnimator2 != null)
        {
            dolphinAnimator2.gameObject.SetActive(true);
            dolphinAnimator2.SetTrigger("Jump");
        }

        yield return new WaitForSeconds(3f);
        if (dolphinSound != null) dolphinSound.Play();
        if (dolphinAnimator3 != null)
        {
            dolphinAnimator3.gameObject.SetActive(true);
            dolphinAnimator3.SetTrigger("Jump");
        }
    }
}
