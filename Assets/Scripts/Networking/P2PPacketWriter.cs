using Steamworks;
using System.IO;
using UnityEngine;

public enum Operation
{
    Handshake, //when a member enters a lobby to start a P2P connection (Host -> Client)
    Kick, //when a member must be kicked from a lobby (Host -> Client)
    LoadGame, //when the host orders to start loading the game scene (Host -> Client)
    Ready, //when a player is ready for something (Client -> Host)
    Spawn, //when a tank must be spawned for the first time at the very beginning of the game (Host -> Client)
    PrepareRound, //when a new round is about to start (Host -> Client)
    TankTransform, //when a player sends his own tank updated transform data (Host -> Client | Client -> Host)
    Shoot, //when a player shoots a bullet (Host -> Client | Client -> Host)
    Damage, //when the host validates that a player took damage (Host -> Client)
    Death, //when the host validates that a player is dead (Host -> Client)
    Respawn, //when a player tells to other players he has respawned (Client -> Host | Host -> Client)
    ResetRound, //when the host tells the other players to reposition on spawn points for new round (Host -> Client)
    SelectWeapon, //when a player switches to another weapon (Client -> Host | Host -> Client)
    SpawnCrate, //when the host tells to the other players that a crate has spawned (Host -> Client)
    InteractionRequest, //when the client asks to the host to interact with an interactable (Client -> Host)
    InteractionResponse, //when the host sends answer to a previous interaction request (Host -> Client)
    RemoveInteractable //when the host tells to the other players to remove an interactable from the game (Host -> Client)
}

public static class P2PPacketWriter
{
    public static byte[] WriteSingleOperation(Operation op)
    {
        MemoryStream stream = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(stream);
        writer.Write((int)op);
        return stream.ToArray();
    }

    public static byte[] WriteSpawnTank(int spawnPointIndex, ulong id)
    {
        MemoryStream stream = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(stream);
        writer.Write((int)Operation.Spawn);
        writer.Write(spawnPointIndex);
        writer.Write(id);
        return stream.ToArray();
    }

    public static byte[] WriteTankTransform(Vector3 tankPosition, Quaternion tankRotation, Quaternion turretRotation, ulong playerID)
    {
        MemoryStream stream = new MemoryStream();
        CustomBinaryWriter writer = new CustomBinaryWriter(stream);
        writer.Write((int)Operation.TankTransform);
        writer.Write(tankPosition);
        writer.Write(tankRotation);
        writer.Write(turretRotation);
        writer.Write(playerID);
        return stream.ToArray();
    }

    public static byte[] WriteShoot(Vector3 bulletPosition, Vector3 bulletForward, float distance, float force, PoolType bulletType, ulong shooterID)
    {
        MemoryStream stream = new MemoryStream();
        CustomBinaryWriter writer = new CustomBinaryWriter(stream);
        writer.Write((int)Operation.Shoot);
        writer.Write(bulletPosition);
        writer.Write(bulletForward);
        writer.Write(distance);
        writer.Write(force);
        writer.Write((int)bulletType);
        writer.Write(shooterID);
        return stream.ToArray();
    }

    public static byte[] WriteDamage(int damage, ulong damagerID, ulong damagedID)
    {
        MemoryStream stream = new MemoryStream();
        CustomBinaryWriter writer = new CustomBinaryWriter(stream);
        writer.Write((int)Operation.Damage);
        writer.Write(damage);
        writer.Write(damagerID);
        writer.Write(damagedID);
        return stream.ToArray();
    }

    public static byte[] WriteDeath(ulong killedID, ulong killerID)
    {
        MemoryStream stream = new MemoryStream();
        CustomBinaryWriter writer = new CustomBinaryWriter(stream);
        writer.Write((int)Operation.Death);
        writer.Write(killedID);
        writer.Write(killerID);
        return stream.ToArray();
    }

    public static byte[] WriteRespawn(ulong respawnedID)
    {
        MemoryStream stream = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(stream);
        writer.Write((int)Operation.Respawn);
        writer.Write(respawnedID);
        return stream.ToArray();
    }

    public static byte[] WriteResetRound(SpawnPoint spawnPoint)
    {
        MemoryStream stream = new MemoryStream();
        CustomBinaryWriter writer = new CustomBinaryWriter(stream);
        writer.Write((int)Operation.ResetRound);
        writer.Write(spawnPoint.transform.position);
        writer.Write(spawnPoint.transform.rotation);
        return stream.ToArray();
    }

    public static byte[] WriteSelectWeapon(ulong playerID, int weaponIndex)
    {
        MemoryStream stream = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(stream);
        writer.Write((int)Operation.SelectWeapon);
        writer.Write(playerID);
        writer.Write(weaponIndex);
        return stream.ToArray();
    }

    public static byte[] WriteSpawnCrate(Vector3 cratePosition, Quaternion crateRotation, int weaponIndex, uint GUID)
    {
        MemoryStream stream = new MemoryStream();
        CustomBinaryWriter writer = new CustomBinaryWriter(stream);
        writer.Write((int)Operation.SpawnCrate);
        writer.Write(cratePosition);
        writer.Write(crateRotation);
        writer.Write(weaponIndex);
        writer.Write(GUID);
        return stream.ToArray();
    }

    public static byte[] WriteInteractionRequest(uint GUID)
    {
        MemoryStream stream = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(stream);
        writer.Write((int)Operation.InteractionRequest);
        writer.Write(GUID);
        return stream.ToArray();
    }

    public static byte[] WriteInteractionResponse(bool agreed, uint GUID)
    {
        MemoryStream stream = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(stream);
        writer.Write((int)Operation.InteractionResponse);
        writer.Write(agreed);
        writer.Write(GUID);
        return stream.ToArray();
    }

    public static byte[] WriteRemoveInteractable(uint GUID)
    {
        MemoryStream stream = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(stream);
        writer.Write((int)Operation.RemoveInteractable);
        writer.Write(GUID);
        return stream.ToArray();
    }
}