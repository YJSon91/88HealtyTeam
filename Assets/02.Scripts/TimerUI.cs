using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    void Update()
    {
        if (GameManager.Instance == null) return;

        float time = GameManager.Instance.GetCurrentTimer(); // GameManager에서 현재 시간 가져옴

        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}
