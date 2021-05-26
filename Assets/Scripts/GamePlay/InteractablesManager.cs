using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablesManager : MonoBehaviour
{
    private static Dictionary<uint, IInteractable> interactables;
    private static uint interactablesGUID = 0;

    public void Awake()
    {
        interactables = new Dictionary<uint, IInteractable>();
    }

    public static void AddInteractable(IInteractable interactable)
    {
        interactables.Add(interactable.GUID, interactable);
    }

    public static void RemoveInteractable(uint interactable)
    {
        interactables.Remove(interactable);
    }

    public static bool IsValid(uint interactable)
    {
        return interactables.ContainsKey(interactable) && interactables[interactable].IsInteractable;
    }

    public static IInteractable GetInteractable(uint interactable)
    {
        return interactables[interactable];
    }

    public static void GenerateCrate()
    {
        Vector3 pos = new Vector3(Random.insideUnitCircle.x * 20, Random.Range(10, 15), Random.insideUnitCircle.y * 20);
        Quaternion rot = Random.rotation;
        int weapon = Random.Range(1, 3);

        foreach (Player player in GameManager.Players.Values)
        {
            SteamNetworking.SendP2PPacket(player.SteamData.Id, P2PPacketWriter.WriteSpawnCrate(pos, rot, weapon, interactablesGUID));
        }

        SpawnCrate(pos, rot, weapon, interactablesGUID);
        //Debug.Log("Spawned crate with Weapon Index: " + weapon);
    }

    public static void SpawnCrate(Vector3 position, Quaternion rotation, int weaponIndex, uint GUID)
    {
        Crate crate = (Crate)PoolsManager.Get(PoolType.Crate);
        crate.transform.position = position;
        crate.transform.rotation = rotation;
        crate.WeaponIndex = weaponIndex;
        crate.GUID = GUID;
        crate.gameObject.SetActive(true);
        interactablesGUID++;
        AddInteractable(crate);
    }
}
