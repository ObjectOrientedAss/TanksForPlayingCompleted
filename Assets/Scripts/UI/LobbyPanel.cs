using Steamworks;
using Steamworks.Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPanel : MonoBehaviour
{
    public TextMeshProUGUI LobbyName; //set in inspector
    public GameObject FriendCardPrefab; //set in inspector
    public GameObject TankCardPrefab; //set in inspector
    public GameObject MainMenuPanel; //set in inspector
    public GameObject RulesPanel; //set in inspector
    public Transform FriendsContainer; //set in inspector
    public Transform TanksContainer; //set in inspector
    public Button StartGameButton; //set in inspector
    public Button RulesButton; //set in inspector

    private void Awake()
    {
        SteamMatchmaking.OnLobbyMemberJoined += OnLobbyMemberJoined;
        SteamMatchmaking.OnLobbyMemberLeave += OnLobbyMemberLeave;
        NW_EventSystem.OnHostChangedEvent += OnHostChangedEvent;
        NW_EventSystem.OnLobbyLeftEvent += OnLobbyLeftEvent;
    }

    private void OnDisable()
    {
        for (int i = FriendsContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(FriendsContainer.GetChild(i).gameObject);
        }
        for (int i = TanksContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(TanksContainer.GetChild(i).gameObject);
        }

        RulesPanel.SetActive(false); //if we have accepted to join a lobby while in the rules panel, be sure to hide it.
    }

    private void OnDestroy()
    {
        SteamMatchmaking.OnLobbyMemberJoined -= OnLobbyMemberJoined;
        SteamMatchmaking.OnLobbyMemberLeave -= OnLobbyMemberLeave;
        NW_EventSystem.OnHostChangedEvent -= OnHostChangedEvent;
        NW_EventSystem.OnLobbyLeftEvent -= OnLobbyLeftEvent;
    }

    private void OnLobbyMemberLeave(Lobby lobby, Friend user)
    {
        if(NetworkManager.CurrentLobby.IsOwnedBy(SteamClient.SteamId))
        {
            StartGameButton.interactable = NetworkManager.CurrentLobby.MemberCount >= int.Parse(NetworkManager.CurrentLobby.GetData("MinMembers"));
        }
    }

    private void OnHostChangedEvent(ulong formerHostID, ulong newHostID)
    {
        if (newHostID == SteamClient.SteamId)
        {
            StartGameButton.gameObject.SetActive(true);
            RulesButton.gameObject.SetActive(true);
        }
    }

    private void OnLobbyLeftEvent()
    {
        UI_EventSystem.OpenNewPanel(MainMenuPanel);
    }

    void OnEnable()
    {
        //if i am the host, if the minimum number of players is reached, set the button as interactable
        if (NetworkManager.CurrentLobby.IsOwnedBy(SteamClient.SteamId))
            StartGameButton.interactable = NetworkManager.CurrentLobby.MemberCount >= int.Parse(NetworkManager.CurrentLobby.GetData("MinMembers"));
        else //i'm not the host, hide the start game button
        {
            StartGameButton.gameObject.SetActive(false);
            RulesButton.gameObject.SetActive(false);
        }

        //set the lobby name
        LobbyName.text = NetworkManager.CurrentLobby.GetData("LobbyName");

        //for each player in lobby create his user card and give to its button the kick method
        foreach (Friend player in NetworkManager.CurrentLobby.Members)
        {
            AddTankCard(player);
        }

        //for each friend create his user card
        LoadFriends();
    }

    private void AddFriendCard(Friend friend)
    {
        if ((int)friend.State >= 1 && (int)friend.State <= 3) //online(1)/away(2)/busy(3)
        {
            GameObject card = Instantiate(FriendCardPrefab, FriendsContainer);
            card.GetComponent<FriendCard>().Init(friend);
        }
    }

    private void AddTankCard(Friend player, bool kickable = false)
    {
        GameObject card = Instantiate(TankCardPrefab, TanksContainer);
        card.GetComponent<TankCard>().Init(player, kickable);
    }

    private void OnLobbyMemberJoined(Lobby lobby, Friend player)
    {
        //if i am the host and someone has joined my lobby
        if (NetworkManager.CurrentLobby.IsOwnedBy(SteamClient.SteamId))
        {
            //add his tank card, this time i want the kick button to kick his butt out of the lobby if i wish so
            AddTankCard(player, true);
            //check if the right number of players is reached, enable the start game button
            StartGameButton.interactable = NetworkManager.CurrentLobby.MemberCount >= int.Parse(NetworkManager.CurrentLobby.GetData("MinMembers"));
        }
        else //i am just a humble client, i'll just add the new player card and go back in my corner crying...
            AddTankCard(player);
    }

    public void OnRefreshFriendsButtonClick()
    {
        LoadFriends();
    }

    private void LoadFriends()
    {
        //clear the friends list
        for (int i = FriendsContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(FriendsContainer.GetChild(i).gameObject);
        }

        //for each friend create his user card and give to its button the invite method
        foreach (Friend friend in SteamFriends.GetFriends())
        {
            AddFriendCard(friend);
        }
    }

    public void OnToggleFriendsButtonClick()
    {
        GetComponentInChildren<Animator>().SetTrigger("Toggle");
    }

    public void OnLeaveLobbyButtonClick()
    {
        NW_EventSystem.LeaveLobby();
    }

    public void OnStartGameButtonClick()
    {
        //this stuff is only done by the host, as he is the only one able to click this button.
        NetworkManager.CurrentLobby.SetJoinable(false);
        foreach (Friend friend in NetworkManager.CurrentLobby.Members)
        {
            if (friend.Id == SteamClient.SteamId) //do not talk to yourself, dumbass.
                continue;
            SteamNetworking.SendP2PPacket(friend.Id, P2PPacketWriter.WriteSingleOperation(Operation.LoadGame));
        }
        GP_EventSystem.StartGame();
    }

    public void OnRulesButtonClick()
    {
        RulesPanel.SetActive(true);
    }
}
