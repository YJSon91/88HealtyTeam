using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    private FieldInfo timerField;

    void Start()
    {
        // private float currentTimer ÇÊµå¸¦ ¸®ÇÃ·º¼ÇÀ¸·Î Ã£¾Æ³¿
        timerField = typeof(GameManager).GetField("currentTimer", BindingFlags.NonPublic | BindingFlags.Instance);
    }

    void Update()
    {
        if (GameManager.Instance == null || timerField == null) return;

        float time = (float)timerField.GetValue(GameManager.Instance); //private °ª »Ì¾Æ¿È

        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}
