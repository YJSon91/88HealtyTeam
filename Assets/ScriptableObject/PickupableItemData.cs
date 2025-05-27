using UnityEngine;
using System;

[CreateAssetMenu(fileName = "PickupableItem", menuName = "Items/PickupableItem")]
public class PickupableItemData : ScriptableObject
{
    public string itemID;
    public string itemName;
    public ItemColor itemColor;
    public string description;

#if UNITY_EDITOR
    // 에디터에서 값이 바뀔 때 자동 호출됨
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(itemID))
        {
            itemID = Guid.NewGuid().ToString();
            UnityEditor.EditorUtility.SetDirty(this); // 변경사항 저장 표시
        }
    }
#endif
}