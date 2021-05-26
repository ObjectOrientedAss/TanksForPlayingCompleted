using Steamworks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinalResultsPanel : MonoBehaviour
{
    public GameObject FinalStatsCardPrefab;
    public Transform FinalStatsCardContainer;

    private void Awake()
    {
        Cursor.visible = true;
        foreach (Friend user in NetworkManager.CurrentLobby.Members)
        {
            GameObject card = Instantiate(FinalStatsCardPrefab, FinalStatsCardContainer);
            card.GetComponent<FinalStatsCard>().Init(user);
        }

        StartCoroutine(QuitGame());
    }

    private IEnumerator QuitGame()
    {
        yield return new WaitForSecondsRealtime(20);
        NW_EventSystem.LeaveLobby();
        UI_EventSystem.StartLoadingAsync(0);
    }
}
