using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;// 플레이어의 이동 및 카메라 회전을 담당하는 PlayerController 클래스의 인스턴스
    public PlayerCondition condition; // 플레이어의 상태를 나타내는 Condition 클래스의 인스턴스


    private void Awake()
    {
        CharacterManager.Instance.Player = this;// CharacterManager의 Player 속성에 현재 Player 인스턴스를 할당
        controller = GetComponent<PlayerController>();// PlayerController 컴포넌트를 가져와서 controller 변수에 할당
        condition = GetComponent<PlayerCondition>();// PlayerCondition 컴포넌트를 가져와서 condition 변수에 할당
    }
}

