using Steamworks.Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyCard : MonoBehaviour
{
    public TextMeshProUGUI LobbyName; //set in inspector
    public TextMeshProUGUI Members; //set in inspector
    private Lobby lobby;

    public void Init(Lobby lobby)
    {
        this.lobby = lobby;
        string lobbyName = lobby.GetData("LobbyName");
        LobbyName.text = lobbyName.Equals("") ? "Somebody's Lobby" : lobbyName;
        Members.text = lobby.MemberCount + "/" + lobby.MaxMembers;
    }

    public void JoinLobby()
    {
        lobby.Join();
    }
}
