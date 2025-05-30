using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBeaconActivate
{
    void ActivateBeacon();
    void DeactivateBeacon();
}

public class Door : MonoBehaviour, IBeaconActivate
{
    [SerializeField] private OXPanel oxPanel;
    public void ActivateBeacon()
    {
        gameObject.SetActive(false);
        oxPanel.ShowPanelO();

        //SoundManager.Instance.PlaySFX("doorOpenSound");
    }
    public void DeactivateBeacon()
    {
        gameObject.SetActive(true);
        oxPanel.ShowPanelX();
    }
}
