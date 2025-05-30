using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum StageNum
{
    LOBBY,
    STAGE_ONE,
    STAGE_TWO
}
public class ShowStageNameUI : MonoBehaviour
{

    public StageNum stageNum;
    public Image stage1;


    public Coroutine coroutine;

    bool hasShown = false;

    void ShowName()
    {
        if (stageNum == StageNum.STAGE_ONE && stage1 != null && !hasShown)
        {
            coroutine = StartCoroutine(ShowTime());
        }
        
    }


    IEnumerator ShowTime()
    {
        stage1.gameObject.SetActive(true);

        yield return new WaitForSeconds(2);
        hasShown = true;
        stage1.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            ShowName();
        }
        
    }
}
