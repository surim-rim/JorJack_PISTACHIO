using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ConchGrabDetector : MonoBehaviour
{
    public bool isGrabbed = false;

    private void OnEnable()
    {
        var grab = GetComponent<XRGrabInteractable>();
        grab.selectEntered.AddListener(OnGrabbed);
        grab.selectExited.AddListener(OnReleased);
    }

    private void OnDisable()
    {
        var grab = GetComponent<XRGrabInteractable>();
        grab.selectEntered.RemoveListener(OnGrabbed);
        grab.selectExited.RemoveListener(OnReleased);
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        isGrabbed = true;
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        isGrabbed = false;
    }
}
