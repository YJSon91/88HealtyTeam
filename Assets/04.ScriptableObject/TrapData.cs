using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trap", menuName = "Objects/Trap", order = 1)]
public class TrapData : ScriptableObject
{
    public string trapName;
    public string description;
    public float trapDamage;
    public float trapDebuffRate;   // Ʈ���� ����� ��ġ
}
