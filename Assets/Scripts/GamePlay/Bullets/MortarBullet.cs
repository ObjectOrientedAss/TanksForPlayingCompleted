using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarBullet : Bullet
{
    private bool isFalling;

    protected override void OnEnable()
    {
        isFalling = false;
        base.OnEnable();
    }

    private void FixedUpdate()
    {
        if (!isFalling)
        {
            if (rb.velocity.y < 0 && Physics.Raycast(transform.position, Vector3.down, 10f))
            {
                isFalling = true;
                AudioManager.PlaySFX("Mortar_Fall");
            }
        }
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        AudioManager.PlaySFX("Mortar_Hit");
    }
}
