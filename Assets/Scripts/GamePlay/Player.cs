using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class Player : MonoBehaviour
{
    public LagCompensation LagCompensation; //set in inspector

    //Common things (Every Player gameobject must have them)
    public Friend SteamData;

    public Transform Base; //set in inspector -> the tank base, used for movement and rotation
    public Transform Turret; //set in inspector -> the tank turret, used to fuck up opponents

    public WeaponsHandler WeaponsHandler; //set in inspector -> the bringers of death lord and master

    //Myself data (Only the local player's script must have them), MOVE THIS STUFF INTO TANKCONTROLLER!
    public float MovementSpeed; //<- forward/backward movement speed
    public float RotationSpeed; //<- yaw rotation speed
    public float TurretRotationSpeed; //<- used for orienting your turret toward the crosshair
    public float ManovrabilityAngle; //<- used to determine the "steering" easiness of the tank
                                     //low values -> realistic tank movement | high values -> easier movement

    private void OnEnable()
    {
        GP_EventSystem.OnPlayerKilledEvent += OnPlayerKilled;
    }

    private void OnDisable()
    {
        GP_EventSystem.OnPlayerKilledEvent -= OnPlayerKilled;
    }

    public void OnPlayerKilled(Events.KillData data)
    {
        if (data.KilledPlayer == SteamData.Id) //if this is the killed player
            gameObject.SetActive(false);
    }

    public void Respawn(Transform spawnPoint)
    {
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
        gameObject.SetActive(true);

        //if i am the host
        if (NetworkManager.CurrentLobby.IsOwnedBy(SteamData.Id))
        {
            foreach (Player player in GameManager.Players.Values)
            {
                SteamNetworking.SendP2PPacket(player.SteamData.Id, P2PPacketWriter.WriteRespawn(SteamData.Id));
            }
        }
        else //if i am a client
            SteamNetworking.SendP2PPacket(NetworkManager.CurrentLobby.Owner.Id, P2PPacketWriter.WriteRespawn(SteamData.Id));
    }
}