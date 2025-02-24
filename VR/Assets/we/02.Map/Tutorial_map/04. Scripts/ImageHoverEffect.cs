using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ImageHoverEffect : MonoBehaviour
{
    public Image targetImage; // UI 이미지
    public Sprite hoverSprite; // 마우스를 올릴 때 변경될 이미지
    private Sprite originalSprite; // 원래 이미지
    private bool isHovered = false;
    private float fadeSpeed = 5f; // 전환 속도

    private void Start()
    {
        if (targetImage == null)
            targetImage = GetComponent<Image>();

        if (targetImage != null)
            originalSprite = targetImage.sprite;
    }

    private void Update()
    {
        if (isHovered && targetImage.sprite != hoverSprite)
        {
            targetImage.color = Color.Lerp(targetImage.color, new Color(1, 1, 1, 1), Time.deltaTime * fadeSpeed);
            targetImage.sprite = hoverSprite;
        }
        else if (!isHovered && targetImage.sprite != originalSprite)
        {
            targetImage.color = Color.Lerp(targetImage.color, new Color(1, 1, 1, 1), Time.deltaTime * fadeSpeed);
            targetImage.sprite = originalSprite;
        }
    }

    public void OnPointerEnter()
    {
        isHovered = true;
    }

    public void OnPointerExit()
    {
        isHovered = false;
    }

    public void OnPointerClick()
    {
        SceneManager.LoadScene("Snowmap_1"); // NextScene > 이동할 씬 이름으로 변경
    }
}