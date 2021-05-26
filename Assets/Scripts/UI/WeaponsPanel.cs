using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponsPanel : MonoBehaviour
{
    public GameObject[] PadLocks; //set in inspector

    private void Awake()
    {
        GP_EventSystem.OnWeaponLockChangedEvent += OnWeaponLockChanged;
    }

    private void OnDestroy()
    {
        GP_EventSystem.OnWeaponLockChangedEvent -= OnWeaponLockChanged;
    }

    private void OnWeaponLockChanged(Events.WeaponData data)
    {
        //if unlocking deactivate padlock, and viceversa
        PadLocks[data.WeaponIndex].SetActive(!data.Unlocked);
    }
}
