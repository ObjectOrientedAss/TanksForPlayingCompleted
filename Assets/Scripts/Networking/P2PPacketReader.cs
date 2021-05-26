using Steamworks;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class P2PPacketReader
{
    public delegate void OnSpawnTankPacketReceived(int spawnPoint, ulong id);
    public static event OnSpawnTankPacketReceived OnSpawnTankPacketReceivedEvent;

    public static void ReadPacket(SteamId senderId, byte[] data)
    {
        Operation operation = (Operation)BitConverter.ToInt32(data, 0);
        switch (operation)
        {
            case Operation.Handshake:
                if (data.Length == 4)
                    Debug.Log("Handshake with " + senderId);
                break;
            case Operation.Kick:
                if (data.Length == 4)
                {
                    NW_EventSystem.LeaveLobby();
                    UI_EventSystem.CreateDialog("Arrivedorci", "The host kicked your ass out from the lobby!");
                }
                break;
            case Operation.LoadGame:
                if (data.Length == 4)
                    GP_EventSystem.StartGame();
                break;
            case Operation.Ready:
                if (data.Length == 4)
                {
                    WaitHandler.PlayerReady();
                }
                break;
            case Operation.Spawn:
                if (data.Length == 16)
                {
                    int spawnPoint = BitConverter.ToInt32(data, 4);
                    ulong id = BitConverter.ToUInt64(data, 8);
                    OnSpawnTankPacketReceivedEvent?.Invoke(spawnPoint, id);
                }
                break;
            case Operation.PrepareRound:
                if (data.Length == 4)
                {
                    WaitHandler.StopWaiting(); //if i am a client, i'm probably still waiting, so stop it.
                    UI_EventSystem.StopLoading(); //remove the loading screen
                    GP_EventSystem.PrepareRound();
                }
                break;
            case Operation.TankTransform:
                if (data.Length == 56)
                {
                    if (NetworkManager.CurrentLobby.IsOwnedBy(SteamClient.SteamId)) //host retransmit
                        Retransmit(data, senderId);

                    Vector3 tankPosition = ReadVector(data, 4);
                    Quaternion tankRotation = ReadQuaternion(data, 16);
                    Quaternion turretRotation = ReadQuaternion(data, 32);
                    ulong tankOwner = BitConverter.ToUInt64(data, 48);

                    //GameManager.Players[tankOwner].SetTransformByPacket(tankPosition, tankRotation, turretRotation);
                    GameManager.Players[tankOwner].LagCompensation.Receive(tankPosition, tankRotation, turretRotation);
                }
                break;
            case Operation.Shoot:
                if (data.Length == 48)
                {
                    if (NetworkManager.CurrentLobby.IsOwnedBy(SteamClient.SteamId)) //host retransmit
                        Retransmit(data, senderId);

                    Events.ShootData sData;
                    sData.Position = ReadVector(data, 4);
                    sData.Forward = ReadVector(data, 16);
                    sData.Distance = BitConverter.ToSingle(data, 28);
                    sData.Force = BitConverter.ToSingle(data, 32);
                    sData.BulletType = (PoolType)BitConverter.ToInt32(data, 36);
                    sData.BulletOwner = BitConverter.ToUInt64(data, 40);

                    GP_EventSystem.Shoot(sData);
                }
                break;
            case Operation.Damage:
                if (data.Length == 24)
                {
                    Events.DamageData dData;
                    dData.Amount = BitConverter.ToInt32(data, 4);
                    dData.DamagerPlayer = BitConverter.ToUInt64(data, 8);
                    dData.DamagedPlayer = BitConverter.ToUInt64(data, 16);

                    GP_EventSystem.DamagePlayer(dData);
                }
                break;
            case Operation.Death:
                if (data.Length == 20)
                {
                    Events.KillData kData;
                    kData.KilledPlayer = BitConverter.ToUInt64(data, 4);
                    kData.KillerPlayer = BitConverter.ToUInt64(data, 12);

                    GP_EventSystem.KillPlayer(kData);
                }
                break;
            case Operation.Respawn:
                if (data.Length == 12)
                {
                    ulong respawnedID = BitConverter.ToUInt64(data, 4);
                    if (NetworkManager.CurrentLobby.IsOwnedBy(SteamClient.SteamId)) //host retransmit
                        Retransmit(data, senderId);

                    //we don't need position or other stuff because transform is automatically sent for now...
                    GameManager.Players[respawnedID].gameObject.SetActive(true);
                }
                break;
            case Operation.ResetRound:
                if (data.Length == 32)
                {
                    LoadingScreenBehaviour.LoadingAction = () =>
                    {
                        Vector3 spawnPointPosition = ReadVector(data, 4);
                        Quaternion spawnPointRotation = ReadQuaternion(data, 16);
                        GameManager.MySelf.transform.position = spawnPointPosition;
                        GameManager.MySelf.transform.rotation = spawnPointRotation;
                        SteamNetworking.SendP2PPacket(NetworkManager.CurrentLobby.Owner.Id, P2PPacketWriter.WriteSingleOperation(Operation.Ready));
                    };
                    UI_EventSystem.StartLoading();
                }
                break;
            case Operation.SelectWeapon:
                if (data.Length == 16)
                {
                    ulong playerID = BitConverter.ToUInt64(data, 4);
                    int weaponIndex = BitConverter.ToInt32(data, 12);
                    if (NetworkManager.CurrentLobby.IsOwnedBy(SteamClient.SteamId)) //host retransmit
                        Retransmit(data, senderId);

                    GameManager.Players[playerID].WeaponsHandler.SelectWeapon(weaponIndex);
                }
                break;
            case Operation.SpawnCrate:
                if (data.Length == 40)
                {
                    Vector3 cratePosition = ReadVector(data, 4);
                    Quaternion crateRotation = ReadQuaternion(data, 16);
                    int weaponIndex = BitConverter.ToInt32(data, 32);
                    uint GUID = BitConverter.ToUInt32(data, 36);

                    InteractablesManager.SpawnCrate(cratePosition, crateRotation, weaponIndex, GUID);
                }
                break;
            case Operation.InteractionRequest:
                if (data.Length == 8)
                {
                    uint GUID = BitConverter.ToUInt32(data, 4);

                    bool agreed = InteractablesManager.IsValid(GUID);

                    if (agreed) //if he can
                    {
                        IInteractable interactable = InteractablesManager.GetInteractable(GUID); //get the interactable

                        foreach (Player player in GameManager.Players.Values) //tell to all the players about this successful interaction
                        {
                            if (player.SteamData.Id == senderId) //this player is the one who asked permission, tell him to interact
                                SteamNetworking.SendP2PPacket(senderId, P2PPacketWriter.WriteInteractionResponse(agreed, GUID));
                            else //this player is another client, tell him to remove the interactable from game
                                SteamNetworking.SendP2PPacket(player.SteamData.Id, P2PPacketWriter.WriteRemoveInteractable(GUID));
                        }
                        interactable.Remove(); //host removes this interactable too
                    }
                    else //tell only to the requester that he cannot interact with this object (THIS COULD BE AVOIDED IN OUR CASE!)
                        SteamNetworking.SendP2PPacket(senderId, P2PPacketWriter.WriteInteractionResponse(agreed, GUID));
                }
                break;
            case Operation.InteractionResponse:
                if (data.Length == 9)
                {
                    bool agreed = BitConverter.ToBoolean(data, 4);
                    uint GUID = BitConverter.ToUInt32(data, 5);

                    if (agreed) //host has given consent to this interaction
                    {
                        IInteractable interactable = InteractablesManager.GetInteractable(GUID); //get the interactable
                        if (InteractablesManager.IsValid(GUID)) //if it is valid
                            interactable.Interact(); //let the requester player interact with it
                    }
                }
                break;
            case Operation.RemoveInteractable:
                if(data.Length == 8)
                {
                    uint GUID = BitConverter.ToUInt32(data, 4);
                    IInteractable interactable = InteractablesManager.GetInteractable(GUID);
                    if (InteractablesManager.IsValid(GUID))
                        interactable.Remove();
                }
                break;
        }
    }

    private static void Retransmit(byte[] data, ulong sender)
    {
        //retransmit to all players the given packet
        foreach (ulong friend in GameManager.Players.Keys)
        {
            if (friend == sender) //do not retransmit to the user who sent this packet
                continue;

            SteamNetworking.SendP2PPacket(friend, data);
        }
    }

    private static Vector3 ReadVector(byte[] data, int startIndex)
    {
        Vector3 v;
        v.x = BitConverter.ToSingle(data, startIndex);
        v.y = BitConverter.ToSingle(data, startIndex + 4);
        v.z = BitConverter.ToSingle(data, startIndex + 8);
        return v;
    }

    private static Quaternion ReadQuaternion(byte[] data, int startIndex)
    {
        Quaternion q;
        q.x = BitConverter.ToSingle(data, startIndex);
        q.y = BitConverter.ToSingle(data, startIndex + 4);
        q.z = BitConverter.ToSingle(data, startIndex + 8);
        q.w = BitConverter.ToSingle(data, startIndex + 12);
        return q;
    }
}