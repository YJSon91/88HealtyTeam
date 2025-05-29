// PlayerData.cs (새 C# 스크립트 생성)
using System;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public Vector3 playerPosition;
    public Quaternion playerRotation;
    public float playerHealth;
    public float playerStamina;

    // 생성자: 기본값으로 초기화 (선택적)
    public GameData()
    {
        playerPosition = Vector3.zero; // 기본 시작 위치나 스폰 포인트로 설정 가능
        playerRotation = Quaternion.identity;
        playerHealth = 100f; // 기본 체력
        playerStamina = 100f; // 기본 스태미나
    }
}