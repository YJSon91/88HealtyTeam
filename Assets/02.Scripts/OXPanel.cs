using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OXPanel : MonoBehaviour
{
    [SerializeField] private GameObject panelO;
    [SerializeField] private GameObject panelX;
    
    public void ShowPanelO()
    {
        panelO.SetActive(true);
        panelX.SetActive(false);

        //SoundManager.Instance.PlaySFX("buttonPressSound");
    }

    public void ShowPanelX()
    {
        panelX.SetActive(true);
        panelO.SetActive(false);

        //SoundManager.Instance.PlaySFX("buttonPressSound");
    }

}
