using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beacon : MonoBehaviour
{
    public BeaconData beaconData;

    private new Renderer renderer;
    [SerializeField] private bool isActivated = false;
    public IBeaconActivate beaconActivate;
    public GameObject door;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        beaconActivate = door.GetComponent<IBeaconActivate>();
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
                item.OnPlace(); // 아이템이 비콘에 배치되었을 때, 아이템 작동 메소드 호출

                ActivateGimmick();
            }
            else
            {
                Debug.Log("올바른 키 아이템이 아닙니다.");
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        PickupableItem item = collision.gameObject.GetComponent<PickupableItem>();

        if (item != null)
        {
            isActivated = false; // 비활성화 상태로 변경

            ActivateGimmick();
        }
    }

    /// <summary>
    /// 아이템이 비콘에 배치되었을 때, 올바른 상호작용 아이템인지 색상을 통해 판별
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private bool ReceiveItem(PickupableItem item)
    {
        bool result = false;

        if (item.itemData.itemColor == beaconData.beaconColor)
        {
            result = true;

            isActivated = true;
        }

        return result;
    }
    
    /// <summary>
    /// 비콘과 연결된 기믹을 동작시키는 메소드
    /// </summary>
    /// memo : 연결된 기믹을 비콘에 연결시킬 필요가 있다. 현재는 연결되어 있지 않음
    private void ActivateGimmick()
    {
        if (isActivated && beaconActivate != null)
        {
            beaconActivate.ActivateBeacon();
        }
        else
        {
            beaconActivate.DeactivateBeacon();
        }
    }

    // memo : PickupableItem의 ChangeColor코드와 중복이다. 상속받아서 쓰는 것이 나을 듯 한데..Item, 혹은 Object라는 클래스를 만들어 상속받는게 좋지 않을까?
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
