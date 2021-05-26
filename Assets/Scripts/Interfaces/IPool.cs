using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolType { MachineGunBullet, CannonBullet, MortarBullet, Crate, VFX }

public interface IPool
{
    PoolType PoolType { get; }
}
