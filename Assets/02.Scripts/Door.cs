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
    public void ActivateBeacon()
    {
        gameObject.SetActive(false);
    }
    public void DeactivateBeacon()
    {
        gameObject.SetActive(true);
    }
}
