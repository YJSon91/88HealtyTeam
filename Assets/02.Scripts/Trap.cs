using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 함정 오브젝트를 총괄하는 스크립트
/// </summary>
public class Trap : MonoBehaviour
{
    public TrapData trapData;

    public float trapDebuffTime;  // 함정의 디버프 지속시간
    public float trapDebuffAmount;  // 함정의 디버프 텀
    private bool isTrapActive = false;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {

    }

    protected virtual void FixedUpdate()
    {

    }

    protected virtual void LateUpdate()
    {

    }

    // memo: 닿자마자 발동하는 것이 아닌 타입의 함정이 추가될 것을 고려해 OnCollisionEnter, OnCollisionExit에서 Player컴포넌트를 참조하지 않고, 트랩 발동 및 해제 메소드를 추가하지 않음
    protected virtual void OnCollisionEnter(Collision collision)
    {
        
    }

    protected virtual void OnCollisionExit(Collision collision)
    {
        
    }

    protected virtual void ActivateTrap(GameObject player)
    {
        if (isTrapActive) return;

        isTrapActive = true;

        Debug.Log($"trap activated");
    }

    protected virtual void DeactivateTrap(GameObject player)
    {
        if (!isTrapActive) return;

        isTrapActive = false;
        
        Debug.Log($"trap deactivated");
    }

    protected virtual void ApplyTrapDebuff(GameObject player)
    {
        Debug.Log($"player has apply trap debuff");
    }

    protected virtual void ApplyTrapDamage(GameObject player)
    {
        Debug.Log($"player has apply trap damage");
    }

    protected virtual void RemoveTrapDebuff(GameObject player)
    {
        Debug.Log($"player has remove trap debuff");
    }
}
