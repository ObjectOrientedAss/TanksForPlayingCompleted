using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RulesPanel : MonoBehaviour
{
    public TMP_Dropdown ScoreCondition;
    public TMP_Dropdown RespawnDelay;
    public TMP_Dropdown RoundsToWin;
    public TMP_Dropdown RoundDuration;

    private Dictionary<string, int> scoreConditionMapper;
    private Dictionary<int, int> respawnDelayMapper;
    private Dictionary<int, int> roundsToWinMapper;
    private Dictionary<int, int> roundDurationMapper;

    private void Awake()
    {
        scoreConditionMapper = new Dictionary<string, int>();
        for (int i = 0; i < ScoreCondition.options.Count; i++)
        {
            scoreConditionMapper.Add(ScoreCondition.options[i].text, i);
        }

        respawnDelayMapper = new Dictionary<int, int>();
        roundsToWinMapper = new Dictionary<int, int>();
        roundDurationMapper = new Dictionary<int, int>();

        Map(RespawnDelay, respawnDelayMapper);
        Map(RoundsToWin, roundsToWinMapper);
        Map(RoundDuration, roundDurationMapper);
    }

    private void Map(TMP_Dropdown dropdown, Dictionary<int, int> mapper)
    {
        for (int i = 0; i < dropdown.options.Count; i++)
        {
            mapper.Add(int.Parse(dropdown.options[i].text), i);
        }
    }

    private void OnEnable()
    {
        LoadCurrentRules();
    }

    public void OnSaveButtonClick()
    {
        NetworkManager.CurrentLobby.SetData("ScoreCondition", ScoreCondition.options[ScoreCondition.value].text);
        NetworkManager.CurrentLobby.SetData("RespawnDelay", RespawnDelay.options[RespawnDelay.value].text);
        NetworkManager.CurrentLobby.SetData("RoundsToWin", RoundsToWin.options[RoundsToWin.value].text);
        NetworkManager.CurrentLobby.SetData("RoundDuration", RoundDuration.options[RoundDuration.value].text);
        gameObject.SetActive(false);
    }

    public void OnCancelButtonClick()
    {
        LoadCurrentRules();
        gameObject.SetActive(false);
    }

    private void LoadCurrentRules()
    {
        string currentScoreCondition = NetworkManager.CurrentLobby.GetData("ScoreCondition");
        ScoreCondition.value = scoreConditionMapper[currentScoreCondition];

        string currentRespawnDelay = NetworkManager.CurrentLobby.GetData("RespawnDelay");
        RespawnDelay.value = respawnDelayMapper[int.Parse(currentRespawnDelay)];

        string currentRoundsToWin = NetworkManager.CurrentLobby.GetData("RoundsToWin");
        RoundsToWin.value = roundsToWinMapper[int.Parse(currentRoundsToWin)];

        string currentRoundDuration = NetworkManager.CurrentLobby.GetData("RoundDuration");
        RoundDuration.value = roundDurationMapper[int.Parse(currentRoundDuration)];
    }
}
