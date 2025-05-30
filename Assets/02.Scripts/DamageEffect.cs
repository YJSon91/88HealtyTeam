using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageEffect : MonoBehaviour
{
    public Image image;
    public float flashSpeed;

    private Coroutine coroutine;

    void Start()
    {
        CharacterManager.Instance.Player.condition.onTakeDamage += Flash;
    }

    public void Flash()
    {
        bool isGameOver = GameManager.Instance.isGameOver;
        if (!isGameOver)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }

            image.enabled = true;
            image.color = new Color(1f, 150f / 255f, 150f / 255f);
            coroutine = StartCoroutine(FadeAway());
        }
    }

    private IEnumerator FadeAway()
    {
        float startAlpha = 0.3f;
        float a = startAlpha;

        while (a > 0)
        {
            a-=(startAlpha/flashSpeed)*Time.deltaTime;
            image.color = new Color(1f, 150f/255f, 150f/255f, a);
            yield return null;
        }

        image.enabled = false;
    }
}
