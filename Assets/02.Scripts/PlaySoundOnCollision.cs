using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnCollision : MonoBehaviour
{
    public string sfxName = "boxHit";

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Beacon"))
        {
            SoundManager.Instance.PlaySFX(sfxName);
        }
    }
}
