using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GP_EventSystem
{
    public delegate void OnBlockPlayerInputs();
    public static event OnBlockPlayerInputs OnBlockPlayerInputsEvent;

    public static void BlockPlayerInputs()
    {
        OnBlockPlayerInputsEvent?.Invoke();
    }

    public delegate void OnStatChanged(Events.StatData data);
    public static event OnStatChanged OnStatChangedEvent;

    public static void ChangeStat(Events.StatData data)
    {
        OnStatChangedEvent?.Invoke(data);
    }

    public delegate void OnStartGame();
    public static event OnStartGame OnStartGameEvent;

    public static void StartGame()
    {
        OnStartGameEvent?.Invoke();
    }

    public delegate void OnPrepareRound();
    public static event OnPrepareRound OnPrepareRoundEvent;

    public static void PrepareRound()
    {
        OnPrepareRoundEvent?.Invoke();
    }

    public delegate void OnStartRound();
    public static event OnStartRound OnStartRoundEvent;

    public static void StartRound()
    {
        OnStartRoundEvent?.Invoke();
    }

    public delegate void OnDie();
    public static event OnDie OnDieEvent;

    public static void Die()
    {
        OnDieEvent?.Invoke();
    }

    public delegate void OnRespawn();
    public static event OnRespawn OnRespawnEvent;

    public static void Respawn()
    {
        OnRespawnEvent?.Invoke();
    }

    public delegate void OnInteractableRemoved(IInteractable interactable);
    public static event OnInteractableRemoved OnInteractableRemovedEvent;

    public static void RemoveInteractable(IInteractable interactable)
    {
        OnInteractableRemovedEvent?.Invoke(interactable);
    }

    public delegate void OnWeaponLockChanged(Events.WeaponData data);
    public static event OnWeaponLockChanged OnWeaponLockChangedEvent;

    public static void ChangeWeaponLock(Events.WeaponData data)
    {
        OnWeaponLockChangedEvent?.Invoke(data);
    }

    public delegate void OnShoot(Events.ShootData data);
    public static event OnShoot OnShootEvent;

    public static void Shoot(Events.ShootData data)
    {
        OnShootEvent?.Invoke(data);
    }

    public delegate void OnPlayerDamaged(Events.DamageData data);
    public static event OnPlayerDamaged OnPlayerDamagedEvent;

    public static void DamagePlayer(Events.DamageData data)
    {
        OnPlayerDamagedEvent?.Invoke(data);
    }

    public delegate void OnPlayerKilled(Events.KillData data);
    public static event OnPlayerKilled OnPlayerKilledEvent;

    public static void KillPlayer(Events.KillData data)
    {
        OnPlayerKilledEvent?.Invoke(data);
    }

    public delegate void OnEndRound();
    public static event OnEndRound OnEndRoundEvent;

    public static void EndRound()
    {
        OnEndRoundEvent?.Invoke();
    }

    public delegate void OnRoundWinnerConfirmed(bool winner);
    public static event OnRoundWinnerConfirmed OnRoundWinnerConfirmedEvent;

    public static void ConfirmRoundWinner(bool winner)
    {
        OnRoundWinnerConfirmedEvent?.Invoke(winner);
    }

    public delegate void OnGameWinnerConfirmed(bool winner);
    public static event OnGameWinnerConfirmed OnGameWinnerConfirmedEvent;

    public static void ConfirmGameWinner(bool winner)
    {
        OnGameWinnerConfirmedEvent?.Invoke(winner);
    }
}