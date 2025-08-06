using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurveyManager : MonoBehaviour
{
    public List<GameObject> panels;
    public Dictionary<string, List<string>> userResponses = new Dictionary<string, List<string>>();

    private int currentPanelIndex = 0;

    void Start()
    {
        for (int i = 0; i < panels.Count; i++)
            panels[i].SetActive(i == 0);
    }

    /// <summary>
    /// 설문 결과를 문자열로 변환하여 GPT에 전달
    /// </summary>
    private void PrintResults()
    {
        // 디버그용 로그 출력
        foreach (var entry in userResponses)
        {
            Debug.Log($"{entry.Key} : {string.Join(", ", entry.Value)}");
        }

        // 문자열로 구성된 설문 응답 포맷 생성
        string formattedSurvey = "";
        foreach (var entry in userResponses)
        {
            formattedSurvey += $"{entry.Key}: {string.Join(", ", entry.Value)}\n";
        }

        // GPT 추천 요청
        GPTRecommender.Instance.RecommendMap(formattedSurvey);
    }

    /// <summary>
    /// 다음 버튼 클릭 시 패널 전환 및 응답 저장
    /// </summary>
    public void NextButtonClicked()
    {
        if (currentPanelIndex < panels.Count)
        {
            SaveCurrentResponse();

            panels[currentPanelIndex].SetActive(false);
            currentPanelIndex++;

            if (currentPanelIndex < panels.Count)
            {
                panels[currentPanelIndex].SetActive(true);
            }
            else
            {
                Debug.Log("설문 완료!");
                PrintResults();  // 최종 응답 정리 및 GPT 호출
            }
        }
        else
        {
            Debug.LogWarning("더 이상 패널이 없습니다.");
        }
    }

    /// <summary>
    /// 현재 패널에서 체크된 Toggle 항목 저장
    /// </summary>
    private void SaveCurrentResponse()
    {
        GameObject panel = panels[currentPanelIndex];
        string key = panel.name;

        List<string> selected = new List<string>();
        foreach (Toggle toggle in panel.GetComponentsInChildren<Toggle>())
        {
            if (toggle.isOn)
            {
                // TMP를 사용할 경우 text 가져오는 부분 수정 필요할 수 있음
                selected.Add(toggle.GetComponentInChildren<Text>().text);
            }
        }

        userResponses[key] = selected;
    }
}
