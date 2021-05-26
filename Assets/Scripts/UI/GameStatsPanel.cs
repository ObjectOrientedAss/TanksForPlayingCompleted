using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatsPanel : MonoBehaviour
{
    public GameObject StatsCardPrefab; //set in inspector
    public Transform StatsContainer; //set in inspector

    private void Awake()
    {
        foreach (Friend player in NetworkManager.CurrentLobby.Members)
        {
            GameObject card = Instantiate(StatsCardPrefab, StatsContainer);
            card.GetComponent<StatsCard>().Init(player);
        }

        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        //sort the cards!
        //todo
    }
}
