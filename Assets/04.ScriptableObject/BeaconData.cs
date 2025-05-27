using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Beacon", menuName = "Objects/Beacon", order = 1)]
public class BeaconData : ScriptableObject
{
    public string beaconID;
    public string beaconName;
    public ItemColor beaconColor;
    public string description;

#if UNITY_EDITOR
    // �����Ϳ��� ���� �ٲ� �� �ڵ� ȣ���
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(beaconID))
        {
            beaconID = Guid.NewGuid().ToString();
            UnityEditor.EditorUtility.SetDirty(this); // ������� ���� ǥ��
        }
    }
#endif
}
