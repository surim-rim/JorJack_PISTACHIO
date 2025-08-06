using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    public TMP_Text aiText;

    public void SetAIText(string text)
    {
        aiText.text = text;
    }
}
