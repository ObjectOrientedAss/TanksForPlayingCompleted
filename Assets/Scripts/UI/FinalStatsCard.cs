using Steamworks;
using TMPro;
using UnityEngine;

public class FinalStatsCard : MonoBehaviour
{
    public TextMeshProUGUI Player;
    public TextMeshProUGUI Kills;
    public TextMeshProUGUI Deaths;
    public TextMeshProUGUI Damage;
    public TextMeshProUGUI Score;

    public void Init(Friend friend)
    {
        if (friend.Id == SteamClient.SteamId)
            Player.fontStyle = FontStyles.Bold;

        Player.text = friend.Name;
        Damage.text = NetworkManager.CurrentLobby.GetMemberData(friend, "TotalDamage");
        Kills.text = NetworkManager.CurrentLobby.GetMemberData(friend, "TotalKills");
        Deaths.text = NetworkManager.CurrentLobby.GetMemberData(friend, "TotalDeaths");
        Score.text = NetworkManager.CurrentLobby.GetMemberData(friend, "Score");
    }
}