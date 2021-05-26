using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunBullet : Bullet
{
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        AudioManager.PlaySFX("MG_Hit");
    }
}
