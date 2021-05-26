using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NW_EventSystem
{
    public delegate void OnLobbyLeft();
    public static event OnLobbyLeft OnLobbyLeftEvent;

    public static void LeaveLobby()
    {
        OnLobbyLeftEvent?.Invoke();
    }

    public delegate void OnHostChanged(ulong formerHostID, ulong newHostID);
    public static event OnHostChanged OnHostChangedEvent;

    public static void ChangeHost(ulong formerHostID, ulong newHostID)
    {
        OnHostChangedEvent?.Invoke(formerHostID, newHostID);
    }
}
