using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ������Ʈ�� �Ѱ��ϴ� ��ũ��Ʈ
/// </summary>
public class Trap : MonoBehaviour
{
    public TrapData trapData;

    public float trapDebuffTime;  // ������ ����� ���ӽð�
    public float trapDebuffAmount;  // ������ ����� ��
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

    // memo: ���ڸ��� �ߵ��ϴ� ���� �ƴ� Ÿ���� ������ �߰��� ���� ����� OnCollisionEnter, OnCollisionExit���� Player������Ʈ�� �������� �ʰ�, Ʈ�� �ߵ� �� ���� �޼ҵ带 �߰����� ����
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
