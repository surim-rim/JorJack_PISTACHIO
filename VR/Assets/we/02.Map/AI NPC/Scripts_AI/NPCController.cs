using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NPCController : MonoBehaviour
{
    public SpeechToText speechToText; // STT 처리
    public TextToSpeech textToSpeech; // TTS 처리
    public Button startConversationButton; // 대화 시작 버튼
    public Button pauseResumeButton; // 🎛️ 일시정지/재개 버튼 추가
    public Text subtitleText; // 자막을 표시할 Text UI 요소
    private TextProcessor textProcessor; // 텍스트 처리기
    private Coroutine conversationCoroutine; // 대화 코루틴 저장용
    private AudioSource audioSource; // 🎤 음성 재생기
    private bool isPaused = false; // 음성 일시정지 상태 저장

    void Start()
    {
        Debug.Log("Start 메서드 실행됨");

        // 🎤 AudioSource 가져오기 (없으면 자동 추가)
        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();

        // SpeechToText, TextToSpeech, TextProcessor 컴포넌트 가져오기
        speechToText = speechToText ?? GetComponent<SpeechToText>();
        textToSpeech = textToSpeech ?? GetComponent<TextToSpeech>();

        // 🔹 TextProcessor가 없으면 자동 추가
        textProcessor = textProcessor ?? GetComponent<TextProcessor>();
        if (textProcessor == null)
        {
            textProcessor = gameObject.AddComponent<TextProcessor>();
            Debug.Log("✅ TextProcessor가 자동으로 추가되었습니다!");
        }

        // 필수 컴포넌트가 없으면 오류 출력
        if (speechToText == null) Debug.LogError("❌ SpeechToText 컴포넌트가 없습니다!");
        if (textToSpeech == null) Debug.LogError("❌ TextToSpeech 컴포넌트가 없습니다!");

        // 버튼 이벤트 연결
        if (startConversationButton != null)
            startConversationButton.onClick.AddListener(OnStartConversationButtonClicked);
        else
            Debug.LogError("❌ 대화 시작 버튼이 설정되지 않았습니다!");

        if (pauseResumeButton != null)
            pauseResumeButton.onClick.AddListener(PauseOrResumeAudio);
        else
            Debug.LogError("❌ pauseResumeButton이 설정되지 않았습니다!");
    }

    public void OnStartConversationButtonClicked()
    {
        if (conversationCoroutine == null) // 대화가 진행 중이지 않을 때만 시작
            conversationCoroutine = StartCoroutine(StartConversation());
        else
            Debug.Log("⚠️ 이미 대화가 진행 중입니다!");
    }

    public IEnumerator StartConversation()
    {
        Debug.Log("🎤 대화가 시작되었습니다.");

        while (true) // 대화 반복
        {
            Debug.Log("🎤 NPC가 사용자의 말을 듣고 있습니다...");

            // 🔹 AI 음성이 끝날 때까지 대기
            while (audioSource.isPlaying || isPaused) yield return null;

            if (isPaused) // 일시정지 상태면 대화 흐름을 멈추고 대기
            {
                Debug.Log("⏸ 대화가 일시정지 상태입니다. 재개될 때까지 대기...");
                while (isPaused) yield return null;
            }

            // 🔹 STT 활성화
            Debug.Log("🎤 NPC가 사용자의 입력을 기다립니다...");
            string recognizedText = null;
            string detectedEmotion = "neutral"; // 기본값

            speechToText.StartRecording((resultText, emotion) =>
            {
                recognizedText = resultText;
                detectedEmotion = emotion;
            });

            // 🔹 사용자가 말을 마칠 때까지 대기
            yield return new WaitUntil(() => recognizedText != null);

            Debug.Log($"📝 STT 결과: {recognizedText} | 감정 분석: {detectedEmotion}");

            // 🔹 사용자가 "종료" 또는 "그만"을 말하면 대화 종료
            if (recognizedText.Contains("종료") || recognizedText.Contains("그만"))
            {
                Debug.Log("👋 사용자가 대화를 종료했습니다.");
                StopConversation();
                yield break;
            }

            // 🔹 감정 기반 GPT 응답 생성
            string gptResponse = null;
            yield return StartCoroutine(textProcessor.GenerateResponse(recognizedText, detectedEmotion, (response) => gptResponse = response));

            if (string.IsNullOrEmpty(gptResponse))
            {
                Debug.LogError("❌ GPT 응답이 비어 있습니다!");
                continue;
            }

            Debug.Log($"💬 NPC 응답: {gptResponse}");
            ShowSubtitle(gptResponse);

            // 🔹 음성을 변환하여 NPC가 말하도록 설정
            yield return StartCoroutine(textToSpeech.SynthesizeSpeech(gptResponse, (audioClip) =>
            {
                if (audioClip != null)
                {
                    PlayResponse(audioClip);
                }
                else
                {
                    Debug.LogError("TTS 요청 실패: AudioClip이 null입니다.");
                }
            }));
        }
    }

    private void StopConversation()
    {
        if (conversationCoroutine != null)
        {
            StopCoroutine(conversationCoroutine);
            conversationCoroutine = null;
        }

        if (pauseResumeButton != null)
        {
            pauseResumeButton.GetComponentInChildren<Text>().text = "⏸ 일시정지";
            isPaused = false;
        }
    }

    private void ShowSubtitle(string text)
    {
        if (subtitleText != null)
            subtitleText.text = text;
        else
            Debug.LogError("❌ 자막 UI 텍스트가 설정되지 않았습니다!");
    }

    private void PlayResponse(AudioClip audioClip)
    {
        if (audioClip == null)
        {
            Debug.LogError("❌ AudioClip이 null입니다! 음성이 재생되지 않습니다.");
            return;
        }

        if (audioSource == null)
        {
            Debug.LogError("❌ AudioSource가 NPC에 없습니다!");
            return;
        }

        audioSource.clip = audioClip;
        audioSource.Play();
        Debug.Log($"🔊 NPC가 말하는 중... (길이: {audioClip.length}초)");
    }

    // 🎛️ 음성 일시정지 / 재개 기능 추가
    public void PauseOrResumeAudio()
    {
        if (audioSource == null)
        {
            Debug.LogError("❌ AudioSource가 없습니다! 음성을 일시정지할 수 없습니다.");
            return;
        }

        if (pauseResumeButton == null)
        {
            Debug.LogError("❌ pauseResumeButton이 연결되지 않았습니다!");
            return;
        }

        isPaused = !isPaused;

        if (isPaused)
        {
            audioSource.Pause();
            pauseResumeButton.GetComponentInChildren<Text>().text = "▶ 재개";
            Debug.Log("⏸ 음성 일시정지");
        }
        else
        {
            if (!audioSource.isPlaying)
            {
                Debug.Log("🔄 음성이 완전히 멈춰 있어서 다시 재생합니다.");
                audioSource.Play();
            }
            else
            {
                audioSource.UnPause();
            }

            pauseResumeButton.GetComponentInChildren<Text>().text = "⏸ 일시정지";
            Debug.Log("▶ 음성 재개");
        }
    }
}
