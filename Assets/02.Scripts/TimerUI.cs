using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private bool emergencyTriggered = false;

    void Update()
    {
        if (GameManager.Instance == null) return;

        float time = GameManager.Instance.GetCurrentTimer();

        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        timerText.text = $"{minutes:00}:{seconds:00}";

        // 30초 이하일 때 긴급 BGM 전환 (한 번만)
        if (time <= 30f && !emergencyTriggered)
        {
            SoundManager.Instance.PlayStageBGM("Stage1", true); // true = 긴급 상황
            emergencyTriggered = true;
        }
    }
}
