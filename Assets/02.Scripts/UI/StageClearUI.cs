using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageClearUI : MonoBehaviour
{
    public Image stageClear;

    private void OnTriggerEnter(Collider other)
    {
        if(stageClear != null)
        {
            stageClear.gameObject.SetActive(true);
        }
    }
}
