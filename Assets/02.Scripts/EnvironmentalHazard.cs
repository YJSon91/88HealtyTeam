using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum HazardType
{
    SLOWING_LIQUID,
    POISON_GAS_AREA,
    POISON_POOL
}


public class EnvironmentalHazard : MonoBehaviour
{
    public HazardType type;
    private float oriSpeed;
    private float effectValue;

    private Coroutine poisoning;
    private Coroutine deepPoisoning;

    private void Start()
    {
        oriSpeed = CharacterManager.Instance.Player.controller.moveSpeed;
    }


    public void ApplyEffect(Player player)
    {
        if (type == HazardType.SLOWING_LIQUID)
        {
            effectValue = 2.0f;
            player.controller.moveSpeed /= effectValue;
            Debug.Log($"속도 감소: {player.controller.moveSpeed}");
        }
        else if (type == HazardType.POISON_GAS_AREA)
        {
            if(poisoning != null)
            {
                StopCoroutine(poisoning);
            }
            effectValue = 1.0f;
            poisoning = StartCoroutine(Poisoning((int)effectValue));
        }
        else if (type == HazardType.POISON_POOL)
        {
            if (deepPoisoning != null)
            {
                StopCoroutine(deepPoisoning);
            }
            effectValue = 2.0f;
            deepPoisoning = StartCoroutine(Poisoning((int)effectValue));
        }
    }

    public void RemoveEffect(Player player)
    {
        if (type == HazardType.SLOWING_LIQUID)
        {
            player.controller.moveSpeed = oriSpeed;
            Debug.Log($"속도 정상화: {player.controller.moveSpeed}");
        }
        else if (type == HazardType.POISON_GAS_AREA)
        {
            StopCoroutine(poisoning);
            Debug.Log($"중독 종료, 현재체력{player.condition.health}");
        }
        else if (type == HazardType.POISON_POOL)
        {
            StopCoroutine(deepPoisoning);
        }
    }

    public IEnumerator Poisoning(int damage)
    {
        while(true)
        {
            Debug.Log($"{damage}만큼의 독뎀!");
            CharacterManager.Instance.Player.condition.TakePhysicalDamage(damage);
            yield return new WaitForSeconds(2);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>(); 
        if (player != null)
        {
            ApplyEffect(player);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        var player = other.GetComponent<Player>(); 
        if (player != null)
        {
            RemoveEffect(player);
        }
    }

}
