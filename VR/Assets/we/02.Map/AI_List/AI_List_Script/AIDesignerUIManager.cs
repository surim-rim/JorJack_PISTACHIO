using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AIDesignerUIManager : MonoBehaviour
{
    [System.Serializable]
    public class BucketListButtonData
    {
        public Button button;
        public TMP_Text buttonText;
        public string bucketListType;
    }

    [Header("버튼 데이터 리스트")]
    public List<BucketListButtonData> buttonDataList;

    [Header("시스템 안내 텍스트")]
    public TMP_Text resultText;

    [Header("AI 설계사 응답 텍스트")]
    public TMP_Text aiResponseText;

    [Header("질문하기 버튼")]
    public Button questionButton;

    [Header("AI 디자이너 매니저 (컴포넌트 모음)")]
    public GameObject aiDesignerContainer;

    private SpeechToText stt;
    private TextProcessor textProc;
    private TextToSpeech tts;

    private void Awake()
    {
        if (aiDesignerContainer != null)
        {
            stt = aiDesignerContainer.GetComponent<SpeechToText>();
            textProc = aiDesignerContainer.GetComponent<TextProcessor>();
            tts = aiDesignerContainer.GetComponent<TextToSpeech>();
        }
    }

    private void Start()
    {
        // 버킷리스트 버튼 설정
        foreach (var data in buttonDataList)
        {
            var captured = data;
            captured.button.onClick.AddListener(() => OnBucketButtonClicked(captured));
        }

        // 질문 버튼 설정
        if (questionButton != null)
        {
            questionButton.onClick.AddListener(OnQuestionButtonClicked);
        }
    }

    // 🎯 1. 버킷리스트 버튼 클릭 시 동작
    private void OnBucketButtonClicked(BucketListButtonData data)
    {
        resultText.text = $"{data.bucketListType} 활동 음성 인식 중…";

        if (stt != null)
        {
            stt.StartRecording((userSpeech, _) =>
            {
                UpdateButtonText(data, userSpeech);

                if (textProc != null)
                {
                    StartCoroutine(textProc.GenerateResponse(userSpeech, data.bucketListType, gptReply =>
                    {
                        resultText.text = $"AI 설계사 응답 도착!";
                        if (aiResponseText != null)
                            aiResponseText.text = gptReply;

                        if (tts != null)
                            StartCoroutine(tts.SynthesizeSpeech(gptReply, clip =>
                            {
                                if (clip != null)
                                {
                                    AudioSource src = GetOrAddAudioSource();
                                    src.clip = clip;
                                    src.Play();
                                }
                            }));
                    }));
                }
                else
                {
                    aiResponseText.text = $"[{data.bucketListType}]에 대한 응답이 없습니다.";
                }
            });
        }
        else
        {
            Debug.LogWarning("SpeechToText 컴포넌트가 연결되지 않았습니다.");
        }
    }

    // 🎯 2. 질문 버튼 클릭 시 동작
    private void OnQuestionButtonClicked()
    {
        resultText.text = $"질문을 말씀해 주세요...";

        if (stt != null)
        {
            stt.StartRecording((userSpeech, _) =>
            {
                resultText.text = $"답변 생성 중...";

                if (textProc != null)
                {
                    StartCoroutine(textProc.GenerateResponse(userSpeech, "질문", gptReply =>
                    {
                        resultText.text = "AI 설계사 응답 도착!";
                        aiResponseText.text = gptReply;

                        if (tts != null)
                            StartCoroutine(tts.SynthesizeSpeech(gptReply, clip =>
                            {
                                if (clip != null)
                                {
                                    AudioSource src = GetOrAddAudioSource();
                                    src.clip = clip;
                                    src.Play();
                                }
                            }));
                    }));
                }
                else
                {
                    aiResponseText.text = "응답을 처리할 수 없습니다.";
                }
            });
        }
        else
        {
            Debug.LogWarning("SpeechToText 컴포넌트가 연결되지 않았습니다.");
        }
    }

    // 🔄 공통 AudioSource 처리
    private AudioSource GetOrAddAudioSource()
    {
        AudioSource src = GetComponent<AudioSource>();
        if (src == null) src = gameObject.AddComponent<AudioSource>();
        return src;
    }

    public void UpdateButtonText(BucketListButtonData data, string newText)
    {
        if (data.buttonText != null)
            data.buttonText.text = newText;
    }

    public void UpdateResultText(string text)
    {
        if (resultText != null)
            resultText.text = text;
    }
}
