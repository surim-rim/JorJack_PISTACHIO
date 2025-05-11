using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CabinetController : XRBaseInteractable
{
    public Animator doorAnimator;
    private bool isOpen = false;

    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        base.OnSelectEntered(interactor);

        if (!isOpen)
        {
            // 열기
            doorAnimator.Play("CabinetOpen");
            isOpen = true;
        }
        else
        {
            // 닫기
            doorAnimator.Play("CabinetClose");
            isOpen = false;
        }
    }
}
