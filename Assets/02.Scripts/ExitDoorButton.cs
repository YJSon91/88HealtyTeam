using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoorButton : MonoBehaviour
{
    [SerializeField] private GameObject exitDoor;
    [SerializeField] private OXPanel OXPanel;
    [SerializeField] private bool isExitDoorOpen;
    public IBeaconActivate beaconActivate;

    private void Start()
    {
        isExitDoorOpen = false;
        beaconActivate = exitDoor.GetComponent<IBeaconActivate>();
    }

    public void TriggerExitDoor()
    {
        if (!isExitDoorOpen)
        {
            OXPanel.ShowPanelO();
            beaconActivate.ActivateBeacon();
            isExitDoorOpen = true;
            return;
        }
        if (isExitDoorOpen)
        {
            OXPanel.ShowPanelX();
            beaconActivate.DeactivateBeacon();
            isExitDoorOpen = false;
            return;
        }
    }
}
