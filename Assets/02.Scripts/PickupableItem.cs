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

public class PickupableItem : MonoBehaviour, IInteractable
{
    public ObjectData objectData;

    private Renderer renderer;

    private void Start()
    {
        renderer = GetComponent<Renderer>();

        renderer.material.color = ChangeColor(objectData.objectColor);
    }

    /// <summary>
    /// 아이템을 들어올렸을 때의 콜백 메소드
    /// </summary>
    public void OnPickup()
    {
        Debug.Log($"Picked up: {objectData.objectName} (ID: {objectData.objectID})");
    }

    /// <summary>
    /// 비콘에 아이템이 배치되었을 때의 콜백 메소드
    /// </summary>
    public void OnPlace()
    {
        Debug.Log($"Placed {objectData.objectName} on");
    }

    // memo : Beacon의 ChangeColor코드와 중복이다. 상속받아서 쓰는 것이 나을 듯 한데..Item, 혹은 Object라는 클래스를 만들어 상속받는게 좋지 않을까?
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

    public ObjectData GetInteractableInfo()
    {
        return objectData;
    }
}
