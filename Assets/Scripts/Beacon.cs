using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beacon : MonoBehaviour
{
    public BeaconData beaconData;

    private Renderer renderer;
    [SerializeField] private bool isActivated = false;

    void Start()
    {
        renderer = GetComponent<Renderer>();

        renderer.material.color = ChangeColor(beaconData.beaconColor);
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        PickupableItem item = collision.gameObject.GetComponent<PickupableItem>();

        if (item != null)
        {
            if (ReceiveItem(item))
            {
                item.OnPlace<Beacon>(this); // �������� ���ܿ� ��ġ�Ǿ��� ��, ������ �۵� �޼ҵ� ȣ��

                ActivateGimmick();
            }
            else
            {
                Debug.Log("�ùٸ� Ű �������� �ƴմϴ�.");
            }
        }
    }

    /// <summary>
    /// �������� ���ܿ� ��ġ�Ǿ��� ��, �ùٸ� ��ȣ�ۿ� ���������� ������ ���� �Ǻ�
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private bool ReceiveItem(PickupableItem item)
    {
        bool result = false;

        if (item.itemData.itemColor == beaconData.beaconColor)
        {
            result = true;
        }

        return result;
    }
    
    /// <summary>
    /// ���ܰ� ����� ����� ���۽�Ű�� �޼ҵ�
    /// </summary>
    /// memo : ����� ����� ���ܿ� �����ų �ʿ䰡 �ִ�. ����� ����Ǿ� ���� ����
    private void ActivateGimmick()
    {
        // �θ��� bool���� ������Ű�ų� �Լ��� �۵���Ű��?
    }

    // memo : PickupableItem�� ChangeColor�ڵ�� �ߺ��̴�. ��ӹ޾Ƽ� ���� ���� ���� �� �ѵ�..Item, Ȥ�� Object��� Ŭ������ ����� ��ӹ޴°� ���� ������?
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
