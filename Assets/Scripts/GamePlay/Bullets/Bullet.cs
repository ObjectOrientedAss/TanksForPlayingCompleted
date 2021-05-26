using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IPool
{
    public ulong Owner { get; set; }

    public PoolType PoolType { get { return BulletType; } }

    public PoolType BulletType;

    public int Damage;
    [HideInInspector] public Rigidbody rb;

    protected void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void OnEnable()
    {
        StartCoroutine(AutoDisable());
    }

    protected IEnumerator AutoDisable()
    {
        yield return new WaitForSecondsRealtime(6);
        gameObject.SetActive(false);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        //only the host checks for pvp collisions
        if (NetworkManager.CurrentLobby.IsOwnedBy(SteamClient.SteamId) && collision.gameObject.CompareTag("Player"))
        {
            //get the hit player id
            ulong playerID = collision.gameObject.GetComponent<Player>().SteamData.Id;

            //only perform checks and send informations if the owner of the bullet is not the same person who got hit
            if (Owner != playerID)
            {
                Events.DamageData data;
                data.Amount = Damage;
                data.DamagerPlayer = Owner;
                data.DamagedPlayer = playerID;
                GP_EventSystem.DamagePlayer(data);

                //tell to each client that a bullet owner by player X has hit a player Y with Z damage
                foreach (Player player in GameManager.Players.Values)
                {
                    SteamNetworking.SendP2PPacket(player.SteamData.Id, P2PPacketWriter.WriteDamage(Damage, Owner, playerID));
                }
            }
        }

        gameObject.SetActive(false);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        StopAllCoroutines();
    }
}
