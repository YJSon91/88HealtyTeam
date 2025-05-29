using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleLight : MonoBehaviour
{
    public bool isLightON = false;

    public GameObject on;
    public GameObject off;

    private void Update()
    {
        if (isLightON)
        {
            on.SetActive(true);
            off.SetActive(false);

        }
        else
        {
            on.SetActive(false);
            off.SetActive(true);
        }
        
    }

    public void LightsUpAndDown()
    {
        isLightON = !isLightON; // 라이트가 키거나 끄는 불값
        PuzzleManager.Instance.TurnSideLights(this);
    }
}
