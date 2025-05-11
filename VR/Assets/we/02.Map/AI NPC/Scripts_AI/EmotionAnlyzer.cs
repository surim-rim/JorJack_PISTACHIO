using System.Collections.Generic;
using UnityEngine;

public class EmotionAnalyzer : MonoBehaviour
{
    private Dictionary<string, string> emotionKeywords = new Dictionary<string, string>
    {
        {"화나", "anger"}, {"짜증", "anger"}, {"열받", "anger"}, {"무서워", "fear"}, {"두려워", "fear"},
        {"기대", "anticipation"}, {"설레", "anticipation"}, {"놀랐", "surprise"}, {"충격", "surprise"},
        {"행복", "joy"}, {"기뻐", "joy"}, {"즐거워", "joy"}, {"슬퍼", "sadness"}, {"우울", "sadness"},
        {"믿어", "trust"}, {"안심", "trust"}, {"역겨", "disgust"}, {"더러워", "disgust"}
    };

    public string DetectEmotion(string userText)
    {
        foreach (var keyword in emotionKeywords)
        {
            if (userText.Contains(keyword.Key))
            {
                return keyword.Value; // 감정 반환
            }
        }
        return "neutral"; // 감정이 감지되지 않으면 중립
    }
}
