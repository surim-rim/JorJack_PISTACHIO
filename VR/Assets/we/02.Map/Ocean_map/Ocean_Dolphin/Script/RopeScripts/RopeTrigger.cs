using UnityEngine;

public class RopeTrigger : MonoBehaviour
{
    public Transform player;
    public Transform boat;
    public ConchGrabDetector conch;
    public Animator ropeAnimator;
    public float triggerDistance = 5f;
    private bool hasTriggered = false;

    void Update()
    {
        if (hasTriggered) return;

        float distance = Vector3.Distance(player.position, boat.position);

        if (conch.isGrabbed && distance < triggerDistance)
        {
            TriggerRopeDrop();
        }
    }

    void TriggerRopeDrop()
    {
        ropeAnimator.SetTrigger("RopeDown");
        hasTriggered = true;
    }
}
