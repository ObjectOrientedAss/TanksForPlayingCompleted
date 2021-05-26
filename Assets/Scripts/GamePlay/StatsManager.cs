using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    private void Awake()
    {
        GP_EventSystem.OnPlayerDamagedEvent += OnPlayerDamaged;
        GP_EventSystem.OnPlayerKilledEvent += OnPlayerKilled;
        GP_EventSystem.OnEndRoundEvent += OnRoundEnd;
    }

    private void OnDestroy()
    {
        GP_EventSystem.OnPlayerDamagedEvent -= OnPlayerDamaged;
        GP_EventSystem.OnPlayerKilledEvent -= OnPlayerKilled;
        GP_EventSystem.OnEndRoundEvent -= OnRoundEnd;
    }

    private void OnPlayerDamaged(Events.DamageData data)
    {
        if (data.DamagedPlayer == SteamClient.SteamId)
            ScoreStat(data.Amount, StatType.Damage);
    }

    private void OnPlayerKilled(Events.KillData data)
    {
        if (data.KilledPlayer == SteamClient.SteamId) //if i've been killed
            ScoreStat(1, StatType.Deaths); //add 1 death to my stats
        else if (data.KillerPlayer == SteamClient.SteamId) //if i've killed
            ScoreStat(1, StatType.Kills); //add 1 kill to my stats
    }

    private void ScoreStat(int amount, StatType statType)
    {
        int currentStat = int.Parse(NetworkManager.CurrentLobby.GetMemberData(GameManager.MySelf.SteamData, statType.ToString()));
        currentStat += amount;
        NetworkManager.CurrentLobby.SetMemberData(statType.ToString(), currentStat.ToString());

        Events.StatData data;
        data.StatOwner = SteamClient.SteamId;
        data.Type = statType;
        data.Value = currentStat;

        GP_EventSystem.ChangeStat(data);
    }

    private void OnRoundEnd()
    {
        GP_EventSystem.BlockPlayerInputs();

        string scoreCondition = NetworkManager.CurrentLobby.GetData("ScoreCondition");

        int myScore = 0;
        int highestScore = 0;
        switch (scoreCondition)
        {
            case "Kills":
                myScore = int.Parse(NetworkManager.CurrentLobby.GetMemberData(GameManager.MySelf.SteamData, "Kills"));
                highestScore = myScore;
                foreach (Player player in GameManager.Players.Values)
                {
                    int playerScore = int.Parse(NetworkManager.CurrentLobby.GetMemberData(player.SteamData, "Kills"));
                    if (myScore < playerScore)
                    {
                        highestScore = playerScore;
                        break;
                    }
                }
                break;
            case "Damage Inflicted":
                myScore = int.Parse(NetworkManager.CurrentLobby.GetMemberData(GameManager.MySelf.SteamData, "Damage"));
                highestScore = myScore;
                foreach (Player player in GameManager.Players.Values)
                {
                    int playerScore = int.Parse(NetworkManager.CurrentLobby.GetMemberData(player.SteamData, "Damage"));
                    if (myScore < playerScore)
                    {
                        highestScore = playerScore;
                        break;
                    }
                }
                break;
        }

        if (myScore == highestScore)
        {
            ScoreStat(1, StatType.Score);
            GP_EventSystem.ConfirmRoundWinner(true);
        }
        else
            GP_EventSystem.ConfirmRoundWinner(false);

        StartCoroutine(CheckGameScore());
    }

    private IEnumerator CheckGameScore()
    {
        yield return new WaitForSecondsRealtime(5);

        int damage = int.Parse(NetworkManager.CurrentLobby.GetMemberData(GameManager.MySelf.SteamData, "Damage"));
        int kills = int.Parse(NetworkManager.CurrentLobby.GetMemberData(GameManager.MySelf.SteamData, "Kills"));
        int deaths = int.Parse(NetworkManager.CurrentLobby.GetMemberData(GameManager.MySelf.SteamData, "Deaths"));
        int totalDamage = int.Parse(NetworkManager.CurrentLobby.GetMemberData(GameManager.MySelf.SteamData, "TotalDamage"));
        int totalKills = int.Parse(NetworkManager.CurrentLobby.GetMemberData(GameManager.MySelf.SteamData, "TotalKills"));
        int totalDeaths = int.Parse(NetworkManager.CurrentLobby.GetMemberData(GameManager.MySelf.SteamData, "TotalDeaths"));

        totalDamage += damage;
        totalKills += kills;
        totalDeaths += deaths;

        NetworkManager.CurrentLobby.SetMemberData("TotalDamage", totalDamage.ToString());
        NetworkManager.CurrentLobby.SetMemberData("TotalKills", totalKills.ToString());
        NetworkManager.CurrentLobby.SetMemberData("TotalDeaths", totalDeaths.ToString());
        NetworkManager.CurrentLobby.SetMemberData("Damage", "0");
        NetworkManager.CurrentLobby.SetMemberData("Kills", "0");
        NetworkManager.CurrentLobby.SetMemberData("Deaths", "0");

        Dictionary<ulong, int> winningUsers = new Dictionary<ulong, int>();

        int roundsToWin = int.Parse(NetworkManager.CurrentLobby.GetData("RoundsToWin"));
        foreach (Friend user in NetworkManager.CurrentLobby.Members)
        {
            int userScore = int.Parse(NetworkManager.CurrentLobby.GetMemberData(user, "Score"));
            if (userScore >= roundsToWin)
                winningUsers.Add(user.Id, userScore);
        }

        //if there is ONE winner return true to end the game.
        if (winningUsers.Count == 1)
        {
            if (winningUsers.ContainsKey(GameManager.MySelf.SteamData.Id)) //i am the winner
                AudioManager.PlayMusic("OST", 1.2f);
            else //HA! looser gg wp ez
                AudioManager.PlayMusic("OST", 0.8f);

            GP_EventSystem.ConfirmGameWinner(true);
        }
        else if (winningUsers.Count == 0) //if there isn't a winner, return false to keep playing
            GP_EventSystem.ConfirmGameWinner(false);
        else //if there is more than one winner, increase the number of rounds to win to keep playing
        {
            roundsToWin++;
            NetworkManager.CurrentLobby.SetData("RoundsToWin", roundsToWin.ToString());
            GP_EventSystem.ConfirmGameWinner(false);
        }
    }
}
