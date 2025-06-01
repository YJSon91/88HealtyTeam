using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TutorialType
{
    Basic,
    Lobby,
    PoisonMap,
    PoisonMapPuzzle,
    BeaconGimmick
}

/// <summary>
/// 플레이 방법, 또는 지역의 특징 등을 설명하는 튜토리얼 클래스
/// </summary>
public class Tutorial : MonoBehaviour
{
    private TutorialUI ui;

    [SerializeField] private TutorialType type;

    void Start()
    {
        ui = FindObjectOfType<TutorialUI>();
    }

    void Update()
    {
        
    }

    // 이미 표시한 튜토리얼은 name을 리스트에 저장한 후, 같은 name과 충돌했을 경우, 표시하지 않도록 한다
    private void OnTriggerEnter(Collider other)
    {
        if (this.GetComponent<Tutorial>() != null) // 튜토리얼 트리거와 충돌했을 때
        {
            string tutorialText = "";   // 튜토리얼 텍스트
            float waitTime = 5f; // UI표시 시간

            switch (this.type)
            {
                case TutorialType.Basic:
                    tutorialText = "이동: W, A, S, D\r\nL_Click: 상호작용\r\nR_Click: 조사\r\nL_Shift: 달리기\r\nSpace: 점프, 더블 점프";
                    waitTime = 10f;

                    break;
                case TutorialType.Lobby:
                    tutorialText = "모든 맵의 비콘을 동작시키면 탈출구를 개방할 수 있습니다\r\n모든 맵의 퍼즐을 풀어 탈출하세요";
                    break;
                case TutorialType.PoisonMap:
                    tutorialText = "맹독 지역입니다\r\n숨만 쉬어도 체력이 깎여나가며, 맹독 늪에 빠지면 더 많은 체력을 잃습니다";

                    break;
                case TutorialType.PoisonMapPuzzle:
                    tutorialText = "상호작용을 통해 퍼즐을 풀 수 있습니다\r\n구슬을 클릭하면 십자범위의 구슬색이 변합니다\r\n모든 구슬의 색을 똑같이 바꾸면 성공입니다";

                    break;
                case TutorialType.BeaconGimmick:
                    tutorialText = "발판을 적절히 움직여 타고 올라가 키 아이템을 손에 넣을 수 있습니다\r\n키 아이템 습득 후, 아이템과 같은 색깔의 비콘에 넣어 맵을 클리어할 수 있습니다";
                    waitTime = 10f;

                    break;
                default:
                    Debug.Log("알 수 없는 튜토리얼 오브젝트입니다.");
                    break;
            }

            ui.ShowTutorial(type, tutorialText, waitTime);
        }
    }
}
