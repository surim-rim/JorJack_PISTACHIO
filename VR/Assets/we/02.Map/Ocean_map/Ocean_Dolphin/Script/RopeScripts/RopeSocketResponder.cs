using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RopeSocketResponder : MonoBehaviour
{
    public Animator ropeAnimator;

    private XRSocketInteractor socket;

    void Start()
    {
        socket = GetComponent<XRSocketInteractor>();
        socket.selectEntered.AddListener(OnSocketed);
    }

    private void OnSocketed(SelectEnterEventArgs args)
    {
        ropeAnimator.SetTrigger("RopeUp");
    }

    void OnDestroy()
    {
        socket.selectEntered.RemoveListener(OnSocketed);
    }
}
