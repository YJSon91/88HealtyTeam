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
    // 에디터에서 값이 바뀔 때 자동 호출됨
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(beaconID))
        {
            beaconID = Guid.NewGuid().ToString();
            UnityEditor.EditorUtility.SetDirty(this); // 변경사항 저장 표시
        }
    }
#endif
}
