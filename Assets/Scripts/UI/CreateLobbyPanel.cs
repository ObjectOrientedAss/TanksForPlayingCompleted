using Steamworks;
using Steamworks.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreateLobbyPanel : MonoBehaviour
{
    public TMP_InputField LobbyName; //set in inspector
    public TMP_InputField MinMembers; //set in inspector
    public TMP_InputField MaxMembers; //set in inspector
    public TMP_Dropdown JoinMode; //set in inspector

    public GameObject LobbyPanel; //set in inspector
    public GameObject MainMenuPanel; //set in inspector

    private void Awake()
    {
        //Register to the Lobby Creation Callback
        SteamMatchmaking.OnLobbyCreated += OnLobbyCreated;
    }

    private void OnDestroy()
    {
        SteamMatchmaking.OnLobbyCreated -= OnLobbyCreated;
    }

    protected void OnLobbyCreated(Result result, Lobby lobby)
    {
        //If the result is ok, the lobby has been created correctly, and is now set in NetworkManager.CurrentLobby.
        //So, refer to it (it should be static) and set its stuff from here!
        if (result == Result.OK)
        {
            switch (JoinMode.value)
            {
                case 0: //public
                    NetworkManager.CurrentLobby.SetPublic();
                    break;
                case 1: //private
                    NetworkManager.CurrentLobby.SetPrivate();
                    break;
                case 2: //friends only
                    NetworkManager.CurrentLobby.SetFriendsOnly();
                    break;
            }

            NetworkManager.CurrentLobby.SetData("LobbyName", LobbyName.text.Length > 0 ? LobbyName.text : "DefaultLobby");
            NetworkManager.CurrentLobby.SetData("GameName", "TanksForPlaying");
            int minMembers;
            NetworkManager.CurrentLobby.SetData("MinMembers", int.TryParse(MinMembers.text, out minMembers) ? minMembers.ToString() : "1");
            NetworkManager.CurrentLobby.SetData("PlayersReady", "0");
        }
    }

    public void OnCreateLobbyButtonClick()
    {
        int maxMembers;
        if (int.TryParse(MaxMembers.text, out maxMembers))
            SteamMatchmaking.CreateLobbyAsync(int.Parse(MaxMembers.text));
        else
            SteamMatchmaking.CreateLobbyAsync(4);
    }

    public void OnBackButtonClick()
    {
        UI_EventSystem.OpenNewPanel(MainMenuPanel);
    }
}