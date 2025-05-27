using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum HazardType
{
    SLOWING_LIQUID,
    POISON_GAS_AREA
}


public class EnvironmentalHazard : MonoBehaviour
{
    public HazardType type;
    private float oriSpeed;
    private float effectValue;


    private void Start()
    {
        // 플레이어의 원래 속도를 할당
    }


    public void ApplyEffect()
    {
        if (type == HazardType.SLOWING_LIQUID)
        {
            // 플레이어 속도 감소
        }
        else if (type == HazardType.POISON_GAS_AREA)
        {
            // 플레이어 중독
        }
    }

    public void RemoveEffect()
    {
        if (type == HazardType.SLOWING_LIQUID)
        {
            // 플레이어 속도 복구
        }
        else if (type == HazardType.POISON_GAS_AREA)
        {
            // 플레이어 중독해제
        }
    }

    private void OnTriggerEnter(Collider other)
    {

    }

}
