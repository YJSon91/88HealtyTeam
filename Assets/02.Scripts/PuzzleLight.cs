using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleLight : MonoBehaviour
{
    public bool isLightON = false;
    public void LightsUpAndDown()
    {
        isLightON = !isLightON; // 라이트가 키거나 끄는 불값
        PuzzleManager.Instance.TurnSideLightS(this);
    }
}
