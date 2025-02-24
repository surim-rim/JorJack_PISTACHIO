using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class EventTest : MonoBehaviour
{

    public XRBaseController controller; //XR 컨트롤러 연결
    public float intensity = 0.5f; // 진동 강도
    public float duration = 0.2f; // 진동 지속시간

    void Start()
    {
        if(controller == null)
        {
            Debug.LogError("XR No Controller");
        }
        
    }

    #region HoverEvent
    // interactable 오브젝트에 처음 겹쳤을때(hover) 발생하는 이벤트, 처음 한번만 호출
    public void OnFirstHoverEntered()
    {
        Debug.Log($"{gameObject.name} - OnFirstHoverEntered");
    }
    // interactable 오브젝트에 겹친 상태에서 벗어났을 때 호출 이벤트, 처음 한번만 호출

    public void OnLastHoverExited()
    {
        Debug.Log($"{gameObject.name} - OnLastHoverExited");
    }
    public void OnHoverEntered()
    {
        Debug.Log($"{gameObject.name} - OnHoverEntered");
    }
    public void OnHoverExited()
    {
        Debug.Log($"{gameObject.name} - OnHoverExited");
    }
    #endregion

    #region SelectEvent
    public void OnFirstSelectEntered()
    {
        Debug.Log($"{gameObject.name} - OnFirstSelectEntered");
    }
    public void OnLastSelectExited()
    {
        Debug.Log($"{gameObject.name} - OnLastSelectExited");
    }
    // 오브젝트 선택 할 때마다 호출
    public void OnSelectEntered()
    {
        if(controller != null)
        {
            controller.SendHapticImpulse(intensity, duration);
            Debug.Log("Haptic Feedback Trigered!!!!");
        }
        Debug.Log($"{gameObject.name} - OnSelectEntered");
    }
    public void OnSelectExited()
    {
        Debug.Log($"{gameObject.name} - OnSelectExited");
    }
    #endregion

    #region FocusEvent
    public void OnFirstFocusEntered()
    {
        Debug.Log($"{gameObject.name} - OnFirstFocusEntered");
    }
    public void OnLastFocusExited()
    {
        Debug.Log($"{gameObject.name} - OnLastFocusExited");
    }
    public void OnFocusEntered()
    {
        Debug.Log($"{gameObject.name} - OnFocusEntered");
    }
    public void OnFocusExited()
    {
        Debug.Log($"{gameObject.name} - OnFocusExited");
    }
    #endregion

    public void OnActivated()
    {
        Debug.Log($"{gameObject.name} - OnActivated");
    }
    public void OnDeactivated()
    {
        Debug.Log($"{gameObject.name} - OnDeactivated");
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
