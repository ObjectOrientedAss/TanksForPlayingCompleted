using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsManager : MonoBehaviour
{
    private void Awake()
    {
        GP_EventSystem.OnShootEvent += OnShoot;
    }

    private void OnDestroy()
    {
        GP_EventSystem.OnShootEvent -= OnShoot;
    }

    private void OnShoot(Events.ShootData data)
    {
        Bullet bullet = (Bullet)PoolsManager.Get(data.BulletType);
        bullet.Owner = data.BulletOwner;
        bullet.transform.position = data.Position;
        bullet.transform.forward = data.Forward;
        bullet.gameObject.SetActive(true);
        bullet.rb.AddForceAtPosition(bullet.transform.forward * Mathf.Clamp(data.Distance, 10, 20) * data.Force, bullet.transform.position);
    }
}