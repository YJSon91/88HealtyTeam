using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
 * 모든 튜토리얼은 코루틴으로 관리
 * TutorialCoroutine을 만들어 튜토리얼이 동작할 때마다 해당하는 튜토리얼을 실행하는 함수를 코루틴에 넣는다
 * 이미 동작중인 코루틴이 있을 경우, 기존 코루틴을 취소하고, TutorialSwitch와 같은 함수를 만들어 기존에 표시하고 있던 튜토리얼은 화면에서 지우고, 새로운 튜토리얼이 나타날 수 있도록 한다
 * 
 * 1. 튜토리얼 UI를 캔버스에 추가
 * 2. OnTrigger든, OnCollision이든 이벤트가 동작하면 coroutine에 해당 튜토리얼을 실행하는 함수를 넣는다
 * 3. 코루틴에는 활성화시킬 UI오브젝트, 오브젝트의 내용을 넣는다
 */
/// <summary>
/// 플레이 방법, 또는 지역의 특징 등을 설명하는 튜토리얼 클래스
/// </summary>
public class Tutorial : MonoBehaviour
{
    private Coroutine coroutine;
    private float waitTime = 5.0f; // 튜토리얼 UI가 표시되는 시간

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(this.gameObject.name + "과 충돌함");

        switch (this.gameObject.name)
        {
            // memo: 오브젝트 이름이 바뀌면 이 스위치문을 일일히 수정해야 한다. 다른 방법이 없을까?
            case "BasicTutorial":
                break;
            case "LobbyTutorial":
                break;
            case "PoisonMapTutorial":
                break;
            case "PoisonMapPuzzleTutorial":
                break;
            case "BeaconGimmickTutorial":
                break;
            default:
                Debug.Log("알 수 없는 튜토리얼 오브젝트입니다.");
                break;
        }
    }

    /// <summary>
    /// 일정 시간 후, 현재 실행중인 튜토리얼 비활성화
    /// </summary>
    /// <returns></returns>
    IEnumerable DisableTutorialUI()
    {
        yield return new WaitForSeconds(waitTime);

        // 현재 활성화된 튜토리얼 UI를 비활성화
    }

    /// <summary>
    /// 현재 표시중인 튜토리얼 UI를 즉시 비활성화 시키고, 새로운 튜토리얼 UI를 표시
    /// </summary>
    private void CancleTutorialUI()
    {

    }
}
