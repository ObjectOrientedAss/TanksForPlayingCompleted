using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WaitHandler
{
    private static bool waitingForPlayers;
    private static Action OnAllPlayersReady;

    /// <summary>
    /// <para>Set a waiting state.</para>
    /// <para>This will allow the host to call the other methods of the WaitHandler.</para>
    /// <para>Clients must call this method too and set the same host action, in order to allow a safe ending in case of host migration.</para>
    /// <para>Remember to call CheckPlayersReady manually after, if needed.</para>
    /// </summary>
    /// <param name="onReadyAction">The action that the host will perform when the right number of ready players is reached and detected.</param>
    public static void StartWaiting(Action onReadyAction)
    {
        waitingForPlayers = true;
        OnAllPlayersReady = onReadyAction;
    }

    /// <summary>
    /// <para>Register a Player Ready event. Only the host should call this method.</para>
    /// <para>When the right number of players is reached, the action passed to SetOnReadyAction is fired.</para>
    /// <para>This method has effect only if there is a pending wait at the moment of the call.</para>
    /// </summary>
    /// <returns></returns>
    public static void PlayerReady()
    {
        if (waitingForPlayers)
        {
            int playersReady = int.Parse(NetworkManager.CurrentLobby.GetData("PlayersReady"));
            playersReady++;
            Debug.Log("Players Ready: " + playersReady);
            NetworkManager.CurrentLobby.SetData("PlayersReady", playersReady.ToString());

            if (playersReady >= NetworkManager.CurrentLobby.MemberCount)
                EndWaiting();
        }
    }

    /// <summary>
    /// <para>Manually check if the right number of players has set ready state.</para>
    /// <para>This check is automatically performed by the host when he calls PlayerReady.</para>
    /// <para>You should manually call this method only if the host left during a wait and you became the new host, and only in that specific moment.</para>
    /// <para>This check is performed only if there is a pending wait at the moment of the call.</para>
    /// </summary>
    public static void CheckPlayersReady()
    {
        if (waitingForPlayers)
        {
            int playersReady = int.Parse(NetworkManager.CurrentLobby.GetData("PlayersReady"));
            if (playersReady >= NetworkManager.CurrentLobby.MemberCount)
                EndWaiting();
        }
    }

    /// <summary>
    /// A Client should call this method when is sure that the host wait has been completed.
    /// </summary>
    public static void StopWaiting()
    {
        waitingForPlayers = false;
        OnAllPlayersReady = null;
    }

    /// <summary>
    /// Invoke the given action (you should do it ONLY if you are the host) and reset the states.
    /// </summary>
    private static void EndWaiting()
    {
        waitingForPlayers = false;
        OnAllPlayersReady.Invoke();
        NetworkManager.CurrentLobby.SetData("PlayersReady", "0");
        OnAllPlayersReady = null;
    }
}
