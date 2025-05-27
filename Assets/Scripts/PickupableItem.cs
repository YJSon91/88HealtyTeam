using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemColor
{
    RED,
    BLUE,
    GREEN,
    YELLOW,
}

public class PickupableItem : MonoBehaviour
{
    public PickupableItemData itemData;

    private Renderer renderer;

    private void Start()
    {
        Debug.Log($"create: {itemData.itemName} (ID: {itemData.itemID} COLOR: {itemData.itemColor})");

        renderer = GetComponent<Renderer>();

        renderer.material.color = ChangeColor(itemData.itemColor);
    }

    /// <summary>
    /// 아이템을 들어올렸을 때의 콜백 메소드
    /// </summary>
    private void OnPickup()
    {
        Debug.Log($"Picked up: {itemData.itemName} (ID: {itemData.itemID})");
    }

    /// <summary>
    /// 비콘에 아이템이 배치되었을 때의 콜백 메소드
    /// </summary>
    private void OnPlace()
    {

    }

    private Color ChangeColor(ItemColor itemColor)
    {
        switch (itemColor)
        {
            case ItemColor.RED:
                return Color.red;
            case ItemColor.BLUE:
                return Color.blue;
            case ItemColor.GREEN:
                return Color.green;
            case ItemColor.YELLOW:
                return Color.yellow;
            default:
                return Color.white;
        }
    }
}
