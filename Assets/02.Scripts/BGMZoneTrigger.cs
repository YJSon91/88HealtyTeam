using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMZoneTrigger : MonoBehaviour
{
    public enum BGMType { Lobby, StageNormal, StageEmergency, StageSuccess}
    public BGMType bgmType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (bgmType)
            {
                case BGMType.Lobby:
                    SoundManager.Instance?.PlayLobbyBGM();
                    break;
                case BGMType.StageNormal:
                    SoundManager.Instance?.PlayStageBGM("Stage", false);
                    break;
                case BGMType.StageEmergency:
                    SoundManager.Instance?.PlayStageBGM("Stage", true);
                    break;
                case BGMType.StageSuccess:
                    SoundManager.Instance?.PlayStageSuccessBGM();
                    break;
            }
        }
    }
}
