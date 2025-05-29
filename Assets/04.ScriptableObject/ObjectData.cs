using UnityEngine;
using System;

public enum ObjectColor
{
    RED,
    BLUE,
    GREEN,
    YELLOW,
}

[CreateAssetMenu(fileName = "Object", menuName = "Objects/Object")]
public class ObjectData : ScriptableObject
{
    public string objectID;
    public string objectName;
    public ItemColor objectColor;
    public string description;

#if UNITY_EDITOR
    // 에디터에서 값이 바뀔 때 자동 호출됨
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(objectID))
        {
            objectID = Guid.NewGuid().ToString();
            UnityEditor.EditorUtility.SetDirty(this); // 변경사항 저장 표시
        }
    }
#endif
}
