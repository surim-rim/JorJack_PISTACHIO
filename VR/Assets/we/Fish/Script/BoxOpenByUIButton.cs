using UnityEngine;
using UnityEngine.UI;

public class BoxOpenByUIButton : MonoBehaviour
{
    public Animator doorAnimator;
    private bool isOpen = false;
    private bool isClose = true;

    void Start()
    {
        // 버튼 클릭 이벤트 등록
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    // 버튼 클릭 시 호출되는 메서드
    public void OnButtonClick()
    {
        if (!isOpen && isClose)
        {
            // 문이 닫혀있는 상태이면 열리는 애니메이션 재생
            doorAnimator.Play("BoxOpen");
            isOpen = true;
            isClose = false;
            Debug.Log("open");
        }
        else if (isOpen && !isClose)  // <-- else if로 변경
        {
            // 문이 열려있는 상태이면 닫는 애니메이션 재생
            doorAnimator.Play("BoxClose");
            isOpen = false;
            isClose = true;
            Debug.Log("close");
        }
    }
}
