using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum User { Local, NonLocal }

public class Events
{
    public struct StatData
    {
        public ulong StatOwner;
        public StatType Type;
        public int Value;
    }

    public struct ShootData
    {
        public PoolType BulletType;
        public Vector3 Position;
        public Vector3 Forward;
        public float Distance;
        public float Force;
        public ulong BulletOwner;
    }

    public struct WeaponData
    {
        public int WeaponIndex;
        public bool Unlocked;
    }

    public struct DamageData
    {
        public ulong DamagedPlayer;
        public ulong DamagerPlayer;
        public int Amount;
    }

    public struct KillData
    {
        public ulong KilledPlayer;
        public ulong KillerPlayer;
    }
}
