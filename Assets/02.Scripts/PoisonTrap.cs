using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonTrap : Trap
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        Player player = collision.gameObject.GetComponent<Player>();

        if (player != null)
        {
            ActivateTrap(player.gameObject);
        }
    }
    protected override void OnCollisionExit(Collision collision)
    {
        base.OnCollisionExit(collision);

        Player player = collision.gameObject.GetComponent<Player>();

        if (player != null)
        {
            DeactivateTrap(player.gameObject);
        }
    }
    protected override void ActivateTrap(GameObject player)
    {
        base.ActivateTrap(player);

        ApplyTrapDebuff(player);
    }
    protected override void DeactivateTrap(GameObject player)
    {
        base.DeactivateTrap(player);

        RemoveTrapDebuff(player);
    }

    protected override void ApplyTrapDebuff(GameObject player)
    {
        base.ApplyTrapDebuff(player);
    }

    protected override void RemoveTrapDebuff(GameObject player)
    {
        base.RemoveTrapDebuff(player);
    }
}
