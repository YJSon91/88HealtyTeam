using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoorButton : MonoBehaviour
{
    public GameObject exitDoor;
    public OXPanel OXPanel;
    public bool isExitDoorOpen = false;
    public IBeaconActivate beaconActivate;

    private void Start()
    {
        beaconActivate = exitDoor.GetComponent<IBeaconActivate>();
    }

    public void TriggerExitDoor()
    {
        if (!isExitDoorOpen)
        {
            beaconActivate.ActivateBeacon();
            OXPanel.ShowPanelO();
        }
        if (isExitDoorOpen)
        {
            beaconActivate.DeactivateBeacon();
            OXPanel.ShowPanelX();
        }
    }
}
