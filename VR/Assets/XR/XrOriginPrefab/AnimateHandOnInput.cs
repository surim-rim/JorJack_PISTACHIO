using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimateHandOnInput : MonoBehaviour
{
    public InputActionProperty pinchAnimationAction;
    public InputActionProperty gripAnimationAction;

    public InputAction primaryButtonAction;
    public Animator handAnimator;
    
    void Start()
    {
        primaryButtonAction = new InputAction(binding: "<Gamepad>/buttonSouth"); // ������ �κ�
        primaryButtonAction.performed += ctx => OnAButtonPressed();
        primaryButtonAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        float triggerValue = pinchAnimationAction.action.ReadValue<float>();

        handAnimator.SetFloat("Trigger", triggerValue);

        float gripValue = gripAnimationAction.action.ReadValue<float>();

        handAnimator.SetFloat("Grip", gripValue);

        //Debug.Log(triggerValue);
    }

    void OnAButtonPressed()
    {
        Debug.Log("A ��ư�� ���Ƚ��ϴ�.");
    }
}
