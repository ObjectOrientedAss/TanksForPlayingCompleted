using Steamworks;
using Steamworks.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviour
{
    public static Lobby CurrentLobby = default;
    private ulong hostID = 0;

    // Start is called before the first frame update
    void Awake()
    {
        try
        {
            //If we are REloading this scene, the steam client is probably already running,
            //so don't reinitialize it. Plus, we need it in background to run its callbacks.
            if (!SteamClient.IsValid)
            {
                SteamClient.Init(480, false);
                Debug.LogWarning("Steam Client Initialized!");
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }

        SteamNetworking.OnP2PSessionRequest += AcceptP2PSessionWithUser;
        SteamNetworking.OnP2PConnectionFailed += OnP2PConnectionFailed;
        SteamMatchmaking.OnLobbyMemberLeave += OnLobbyMemberLeave;
        NW_EventSystem.OnLobbyLeftEvent += OnLobbyLeft;
        GP_EventSystem.OnStartGameEvent += OnStartGame;
        SceneManager.sceneLoaded += SceneLoaded;
        DontDestroyOnLoad(gameObject);
    }

    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "MainMenu": //main menu scene
                SteamMatchmaking.OnLobbyCreated += OnLobbyCreated;
                SteamMatchmaking.OnLobbyEntered += OnLobbyEntered;
                SteamMatchmaking.OnLobbyInvite += OnLobbyInvite;
                SteamMatchmaking.OnLobbyMemberJoined += OnLobbyMemberJoined;
                UI_EventSystem.StopLoading();
                break;
            case "FinalScene": //post-game scene
                UI_EventSystem.StopLoading();
                break;
            default: //battle map scene
                //i am the host
                if (CurrentLobby.IsOwnedBy(SteamClient.SteamId))
                    WaitHandler.PlayerReady(); //set me ready
                else //i am a client
                    SteamNetworking.SendP2PPacket(CurrentLobby.Owner.Id, P2PPacketWriter.WriteSingleOperation(Operation.Ready)); //tell the host i'm ready
                break;
        }
    }

    private void OnStartGame()
    {
        SteamMatchmaking.OnLobbyCreated -= OnLobbyCreated;
        SteamMatchmaking.OnLobbyEntered -= OnLobbyEntered;
        SteamMatchmaking.OnLobbyInvite -= OnLobbyInvite;
        SteamMatchmaking.OnLobbyMemberJoined -= OnLobbyMemberJoined;

        //prepare all the users about what the host will do when all players will be ready
        WaitHandler.StartWaiting(() =>
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().InitializeGame();
        });

        UI_EventSystem.StartLoadingAsync(1);
    }

    private void OnLobbyLeft()
    {
        LeaveCurrentLobby();
    }

    private void OnLobbyMemberLeave(Lobby lobby, Friend user)
    {
        if (user.Id == hostID) //the user who left WAS the host.
        {
            //Trigger a host change event. In-Game, this should stop the game until the new host setup is completed.
            NW_EventSystem.ChangeHost(hostID, CurrentLobby.Owner.Id);
            hostID = CurrentLobby.Owner.Id; //update the host id to be the new one
            SteamNetworking.CloseP2PSessionWithUser(user.Id); //close p2p connection with former host

            if (CurrentLobby.IsOwnedBy(SteamClient.SteamId)) //i am the new host
            {
                foreach (Friend friend in CurrentLobby.Members) //tell them there is a new sheriff in town!
                {
                    if (friend.Id != SteamClient.SteamId) //do not talk to yourself, you dumb!
                        SteamNetworking.SendP2PPacket(user.Id, P2PPacketWriter.WriteSingleOperation(Operation.Handshake));
                }
                WaitHandler.CheckPlayersReady(); //i am the new host, so if there is a pending wait process, start from where the last host left!
            }
        }
        else //the user who left was a client
        {
            if (CurrentLobby.IsOwnedBy(SteamClient.SteamId)) //i am the host
                SteamNetworking.CloseP2PSessionWithUser(user.Id); //close p2p connection with left player
        }
    }

    private void AcceptP2PSessionWithUser(SteamId ownerID)
    {
        SteamNetworking.AcceptP2PSessionWithUser(ownerID);
        //Debug.Log("ACCEPTED P2P SESSION WITH USER " + ownerID);
    }

    private void OnP2PConnectionFailed(SteamId arg1, P2PSessionError arg2)
    {
        //Debug.Log("ON P2P CONNECTION FAILED " + arg2.ToString());
    }

    private void OnLobbyMemberJoined(Lobby lobby, Friend user)
    {
        //if i am the host, say hi!
        if (CurrentLobby.IsOwnedBy(SteamClient.SteamId))
            SteamNetworking.SendP2PPacket(user.Id, P2PPacketWriter.WriteSingleOperation(Operation.Handshake));
    }

    private void OnLobbyEntered(Lobby lobby)
    {
        CurrentLobby = lobby;
        hostID = lobby.Owner.Id;
        CurrentLobby.SetMemberData("TankColor", "0");
    }

    private void OnLobbyInvite(Friend friend, Lobby lobby)
    {
        DialogBehaviour dialog = UI_EventSystem.CreateDialog("Invite received!", friend.Name + " has invited you to join a lobby!");
        dialog.AddButton("Accept", () =>
        {
            LeaveCurrentLobby();
            lobby.Join();
            dialog.Close();
        });
    }

    /// <summary>
    /// Callback fired when you have succesfully created a lobby.
    /// </summary>
    /// <param name="result"></param>
    /// <param name="lobby"></param>
    private void OnLobbyCreated(Result result, Lobby lobby)
    {
        if (result == Result.OK)
        {
            CurrentLobby = lobby; //cache the lobby
            CurrentLobby.SetData("ScoreCondition", "Kills"); //kills
            CurrentLobby.SetData("RespawnDelay", "5"); //5 seconds
            CurrentLobby.SetData("RoundsToWin", "3"); //3 rounds
            CurrentLobby.SetData("RoundDuration", "120"); //120 seconds
        }
        else
            Debug.Log("Lobby creation failed with the following result " + result.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        SteamClient.RunCallbacks();

        while (SteamNetworking.IsP2PPacketAvailable())
        {
            P2Packet? packet = SteamNetworking.ReadP2PPacket();

            if (packet.HasValue)
                P2PPacketReader.ReadPacket(packet.Value.SteamId, packet.Value.Data);
        }
    }

    void OnApplicationQuit()
    {
        LeaveCurrentLobby();
        SteamClient.Shutdown();
    }

    private void LeaveCurrentLobby()
    {
        if (!CurrentLobby.Equals(default(Lobby)))
        {
            //if i was the host of this lobby
            if (hostID == SteamClient.SteamId)
            {
                foreach (Friend user in CurrentLobby.Members)
                {
                    if (user.Id == SteamClient.SteamId)
                        continue;
                    SteamNetworking.CloseP2PSessionWithUser(user.Id);
                }
            }
            else //if i was a client
                SteamNetworking.CloseP2PSessionWithUser(hostID);

            CurrentLobby.Leave();
            CurrentLobby = default;
            hostID = 0;
            Debug.Log("Lobby Left Correctly");
        }
    }
}