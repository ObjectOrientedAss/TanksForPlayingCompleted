using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour, IPool, IInteractable
{
    public int WeaponIndex;
    private Rigidbody rb;

    public PoolType PoolType { get { return PoolType.Crate; } }

    public string InteractionText { get { return "Pick Up"; } }
    public uint GUID { get; set; }
    public bool IsInteractable { get; set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        StartCoroutine(AutoDisable());
        IsInteractable = true;
    }

    private IEnumerator AutoDisable()
    {
        yield return new WaitForSecondsRealtime(20);
        gameObject.SetActive(false);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void Interact()
    {
        Events.WeaponData data;
        data.WeaponIndex = WeaponIndex;
        data.Unlocked = true;
        GP_EventSystem.ChangeWeaponLock(data);

        Remove();
    }

    public void Remove()
    {
        IsInteractable = false;
        InteractablesManager.RemoveInteractable(GUID);
        GP_EventSystem.RemoveInteractable(this);
        gameObject.SetActive(false);
    }

    public void RequestInteraction()
    {
        //if i am the host
        if (NetworkManager.CurrentLobby.IsOwnedBy(SteamClient.SteamId))
        {
            if (IsInteractable) //if this interactable is valid
            {
                Interact(); //interact with it
                //and tell to all the other players that i have interacted with this object
                foreach (Player player in GameManager.Players.Values)
                {
                    SteamNetworking.SendP2PPacket(player.SteamData.Id, P2PPacketWriter.WriteRemoveInteractable(GUID));
                }
            }
        }
        else //if i am a client ask the host permission to interact with this object
            SteamNetworking.SendP2PPacket(NetworkManager.CurrentLobby.Owner.Id, P2PPacketWriter.WriteInteractionRequest(GUID));
    }
}
