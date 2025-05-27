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
    // �����Ϳ��� ���� �ٲ� �� �ڵ� ȣ���
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(itemID))
        {
            itemID = Guid.NewGuid().ToString();
            UnityEditor.EditorUtility.SetDirty(this); // ������� ���� ǥ��
        }
    }
#endif
}