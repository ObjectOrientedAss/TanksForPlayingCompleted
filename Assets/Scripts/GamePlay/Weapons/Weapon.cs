using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public abstract class Weapon : MonoBehaviour
{
    public string SoundName; //set in inspector
    public bool IsUnlocked; //set in inspector
    public PoolType BulletType; //set in inspector
    public float ShootCD; //set in inspector
    public Transform BulletSpawnPoint; //set in inspector
    public float Force; //set in inspector
    public VisualEffect VFX; //set in inspector
    public bool CanShoot { get; protected set; }

    protected virtual void Awake()
    {
        CanShoot = true;
    }

    protected virtual void OnEnable()
    {
        if (!CanShoot) //if this weapon was reloading before being switched, restart reloading when reselecting it
            StartCoroutine(Cooldown());
    }

    public virtual void Shoot(float distance)
    {
        StartCoroutine(Cooldown());
        if (NetworkManager.CurrentLobby.IsOwnedBy(SteamClient.SteamId)) //i am the host
        {
            //send to each player the shoot operation
            foreach (ulong friend in GameManager.Players.Keys)
            {
                SteamNetworking.SendP2PPacket(friend, P2PPacketWriter.WriteShoot(BulletSpawnPoint.position, BulletSpawnPoint.forward, distance, Force, BulletType, SteamClient.SteamId));
            }
        }
        else //i am a client, send to the host my transform, he will redirect it to other players too
            SteamNetworking.SendP2PPacket(NetworkManager.CurrentLobby.Owner.Id, P2PPacketWriter.WriteShoot(BulletSpawnPoint.position, BulletSpawnPoint.forward, distance, Force, BulletType, SteamClient.SteamId));

        Events.ShootData data;
        data.BulletType = BulletType;
        data.BulletOwner = SteamClient.SteamId;
        data.Position = BulletSpawnPoint.position;
        data.Forward = BulletSpawnPoint.forward;
        data.Distance = distance;
        data.Force = Force;

        GP_EventSystem.Shoot(data);
    }

    protected IEnumerator Cooldown()
    {
        CanShoot = false;
        yield return new WaitForSecondsRealtime(ShootCD);
        CanShoot = true;
    }
}
