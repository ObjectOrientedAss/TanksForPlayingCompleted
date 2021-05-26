using Steamworks;
using Steamworks.Data;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

public class FindLobbiesPanel : MonoBehaviour
{
    public GameObject MainMenuPanel;
    public GameObject LobbyCardPrefab; //set in inspector
    public Transform LobbiesContainer; //set in inspector
    public TMP_InputField FreeSlots; //set in inspector
    public TMP_InputField LobbyName; //set in inspector
    public TMP_InputField MinPlayers; //set in inspector

    private void OnDisable()
    {
        ClearLobbies();
    }

    public void OnBackButtonClick()
    {
        UI_EventSystem.OpenNewPanel(MainMenuPanel);
    }

    public void OnRefreshButtonClick()
    {
        ClearLobbies();
        _ = RequestLobbiesAsync();
    }

    private void ClearLobbies()
    {
        for (int i = LobbiesContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(LobbiesContainer.GetChild(i).gameObject);
        }
    }

    async Task RequestLobbiesAsync()
    {
        Lobby[] lobbies;
        LobbyQuery lq = new LobbyQuery();

        lq.WithMaxResults(30); //limit the number of lobbies to show with this (you can make it an exposed parameter)
        lq.WithKeyValue("GameName", "TanksForPlaying"); //only search for lobbies of OUR game (we are using SpaceWars, so whoever is using it is gonna be a candidate for the lobby search)

        //if the user has wrote something in the free slots field
        if (FreeSlots.text.Length > 0)
        {
            int freeSlots;
            lq.WithSlotsAvailable(int.TryParse(FreeSlots.text, out freeSlots) ? freeSlots : 1); //try to convert this value, if we success, use it, else use 1
        }

        //if the user has wrote something in the lobby name field
        if (LobbyName.text.Length > 0)
            lq.WithKeyValue("LobbyName", LobbyName.text); //use that name as search parameter
        else
            lq.WithKeyValue("LobbyName", "DefaultLobby"); //else use "DefaultLobby" as search parameter

        //if the user has wrote something in the min players field
        if (MinPlayers.text.Length > 0)
        {
            int minPlayers;
            lq.WithKeyValue("MinMembers", (int.TryParse(MinPlayers.text, out minPlayers) ? minPlayers : 1).ToString()); //try to convert this value, if we success, use it, else use 1
        }

        lobbies = await lq.RequestAsync();

        if (lobbies != null)
            ShowLobbies(lobbies);
        else
            UI_EventSystem.CreateDialog("Hello darkness my old friend...", "No lobbies were found with the given criteria, sorry!");
    }

    public void ShowLobbies(Lobby[] lobbies)
    {
        for (int i = 0; i < lobbies.Length; i++)
        {
            GameObject card = Instantiate(LobbyCardPrefab, LobbiesContainer);
            card.GetComponent<LobbyCard>().Init(lobbies[i]);
        }
    }
}
