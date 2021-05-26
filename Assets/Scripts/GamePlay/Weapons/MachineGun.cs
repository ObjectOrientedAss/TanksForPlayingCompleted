using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : Weapon
{
    protected override void OnEnable()
    {
        //machinegun can immediately shoot when selected, no matter if it was cooling-down the last time it was selected.
        CanShoot = true;
    }

    public override void Shoot(float distance)
    {
        StartCoroutine(Cooldown());
        //machinegun shoots imprecisely
        Vector3 dir = BulletSpawnPoint.forward + new Vector3(Mathf.Clamp(Random.insideUnitCircle.x, -0.1f, 0.1f), 0, 0);

        if (NetworkManager.CurrentLobby.IsOwnedBy(SteamClient.SteamId)) //i am the host
        {
            //send to each player the shoot operation
            foreach (ulong friend in GameManager.Players.Keys)
            {
                SteamNetworking.SendP2PPacket(friend, P2PPacketWriter.WriteShoot(BulletSpawnPoint.position, dir, distance, Force, BulletType, SteamClient.SteamId));
            }
        }
        else //i am a client, send to the host my transform, he will redirect it to other players too
            SteamNetworking.SendP2PPacket(NetworkManager.CurrentLobby.Owner.Id, P2PPacketWriter.WriteShoot(BulletSpawnPoint.position, dir, distance, Force, BulletType, SteamClient.SteamId));

        Events.ShootData data;
        data.BulletType = BulletType;
        data.BulletOwner = SteamClient.SteamId;
        data.Position = BulletSpawnPoint.position;
        data.Forward = dir;
        data.Distance = distance;
        data.Force = Force;

        GP_EventSystem.Shoot(data);
    }
}