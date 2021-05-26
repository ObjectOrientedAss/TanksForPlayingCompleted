using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankCanvas : MonoBehaviour
{
    private Player player;
    private Transform mainCamera;

    public RectTransform LifeBar; //set in inspector
    public int MaxHealth; //set in inspector -> the max health of this player
    private int health;    //set in inspector -> the current health of this player

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    private void OnEnable()
    {
        mainCamera = Camera.main.transform;
        GP_EventSystem.OnPlayerDamagedEvent += OnPlayerDamaged;

        health = MaxHealth;
        LifeBar.localScale = new Vector3((float)health / (float)MaxHealth, LifeBar.localScale.y, LifeBar.localScale.z);
    }

    private void OnDisable()
    {
        GP_EventSystem.OnPlayerDamagedEvent -= OnPlayerDamaged;
    }

    private void OnPlayerDamaged(Events.DamageData data)
    {
        if (data.DamagedPlayer == player.SteamData.Id) //if this is the damaged player
        {
            health -= data.Amount;
            LifeBar.localScale = new Vector3((float)health / (float)MaxHealth, LifeBar.localScale.y, LifeBar.localScale.z);

            //when a user gets damaged (even himself), the host checks if the life is below or equal to 0 to announce the death
            if (NetworkManager.CurrentLobby.IsOwnedBy(SteamClient.SteamId))
            {
                if (health <= 0)
                {
                    foreach (ulong player in GameManager.Players.Keys)
                    {
                        SteamNetworking.SendP2PPacket(player, P2PPacketWriter.WriteDeath(data.DamagedPlayer, data.DamagerPlayer));
                    }

                    Events.KillData kData;
                    kData.KilledPlayer = data.DamagedPlayer;
                    kData.KillerPlayer = data.DamagerPlayer;

                    GP_EventSystem.KillPlayer(kData); //kill this babbeozzo
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.forward = mainCamera.transform.forward;
    }
}
