using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class WeaponsHandler : MonoBehaviour
{
    public User UserType;
    private Weapon[] weapons;
    private int currentWeaponIndex;

    private void Awake()
    {
        currentWeaponIndex = 0;
        weapons = GetComponentsInChildren<Weapon>();
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.SetActive(false);
        }
        weapons[currentWeaponIndex].gameObject.SetActive(true);

        if (UserType == User.Local) //only the local user listens to this callback
            GP_EventSystem.OnWeaponLockChangedEvent += OnWeaponLockChanged;
    }

    private void OnDestroy()
    {
        if(UserType == User.Local) //only the local user deregisters from this callback
            GP_EventSystem.OnWeaponLockChangedEvent -= OnWeaponLockChanged;
    }

    public void SelectNextWeapon()
    {
        //Debug.Log("Next Weapon");

        int temp = currentWeaponIndex;
        do
        {
            if (++currentWeaponIndex >= weapons.Length)
                currentWeaponIndex = 0;
        } while (!weapons[currentWeaponIndex].IsUnlocked);
        weapons[temp].gameObject.SetActive(false);
        weapons[currentWeaponIndex].gameObject.SetActive(true);

        SendWeaponSelection();
    }

    public void SelectPreviousWeapon()
    {
        //Debug.Log("Prev Weapon");

        int temp = currentWeaponIndex;
        do
        {
            if (--currentWeaponIndex < 0)
                currentWeaponIndex = weapons.Length - 1;
        } while (!weapons[currentWeaponIndex].IsUnlocked);
        weapons[temp].gameObject.SetActive(false);
        weapons[currentWeaponIndex].gameObject.SetActive(true);

        SendWeaponSelection();
    }

    private void SendWeaponSelection()
    {
        if (NetworkManager.CurrentLobby.IsOwnedBy(SteamClient.SteamId))
        {
            foreach (Player player in GameManager.Players.Values)
            {
                SteamNetworking.SendP2PPacket(player.SteamData.Id, P2PPacketWriter.WriteSelectWeapon(SteamClient.SteamId, currentWeaponIndex));
            }
        }
        else
            SteamNetworking.SendP2PPacket(NetworkManager.CurrentLobby.Owner.Id, P2PPacketWriter.WriteSelectWeapon(SteamClient.SteamId, currentWeaponIndex));
    }

    public void SelectWeapon(int index)
    {
        weapons[currentWeaponIndex].gameObject.SetActive(false);
        currentWeaponIndex = index;
        weapons[currentWeaponIndex].gameObject.SetActive(true);
    }

    public Weapon GetCurrentWeapon()
    {
        return weapons[currentWeaponIndex];
    }

    public void OnWeaponLockChanged(Events.WeaponData data)
    {
        weapons[data.WeaponIndex].IsUnlocked = data.Unlocked;
    }
}