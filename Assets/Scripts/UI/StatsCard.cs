using Steamworks;
using Steamworks.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum StatType { Kills, Deaths, Damage, Score }

public class StatsCard : MonoBehaviour
{
    public TextMeshProUGUI Player;
    public TextMeshProUGUI Kills;
    public TextMeshProUGUI Deaths;
    public TextMeshProUGUI Damage;
    public TextMeshProUGUI Score;
    private Friend friend;

    public void Init(Friend friend)
    {
        this.friend = friend;

        if (friend.Id == SteamClient.SteamId)
            Player.fontStyle = FontStyles.Bold;

        Player.text = friend.Name;
        Kills.text = "0";
        Deaths.text = "0";
        Damage.text = "0";
        Score.text = "0";

        GP_EventSystem.OnStatChangedEvent += OnStatChanged;
        UI_EventSystem.ToggleStatsEvent += RefreshStats;
        SteamMatchmaking.OnLobbyMemberLeave += OnLobbyMemberLeave;
    }

    private void OnDestroy()
    {
        GP_EventSystem.OnStatChangedEvent -= OnStatChanged;
        UI_EventSystem.ToggleStatsEvent -= RefreshStats;
        SteamMatchmaking.OnLobbyMemberLeave -= OnLobbyMemberLeave;
    }

    private void OnLobbyMemberLeave(Lobby lobby, Friend user)
    {
        if (user.Id == friend.Id)
        {
            StopAllCoroutines();
            Destroy(gameObject);
        }
    }

    private void RefreshStats()
    {
        Kills.text = NetworkManager.CurrentLobby.GetMemberData(friend, "Kills");
        Deaths.text = NetworkManager.CurrentLobby.GetMemberData(friend, "Deaths");
        Damage.text = NetworkManager.CurrentLobby.GetMemberData(friend, "Damage");
        Score.text = NetworkManager.CurrentLobby.GetMemberData(friend, "Score");
    }

    private void OnStatChanged(Events.StatData data)
    {
        if(data.StatOwner == friend.Id) //the stat of this player has changed
        {
            switch (data.Type)
            {
                case StatType.Kills:
                    Kills.text = data.Value.ToString();
                    break;
                case StatType.Deaths:
                    Deaths.text = data.Value.ToString();
                    break;
                case StatType.Damage:
                    Damage.text = data.Value.ToString();
                    break;
                case StatType.Score:
                    Score.text = data.Value.ToString();
                    break;
            }
        }
    }

    private void OnEnable()
    {
        if(friend.Id != SteamClient.SteamId) //my card is automatically refreshed in real time
            StartCoroutine(RefreshWhileActive());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator RefreshWhileActive()
    {
        while(true)
        {
            yield return new WaitForSecondsRealtime(1);
            RefreshStats();
        }
    }
}