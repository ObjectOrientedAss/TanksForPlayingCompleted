using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public TextMeshProUGUI Countdown; //set in inspector
    public TextMeshProUGUI RoundTime; //set in inspector
    public TextMeshProUGUI RoundResult; //set in inspector
    public GameObject GameStatsPanel; //set in inspector
    public GameObject PausePanel; //set in inspector

    private GameObject currentPanel;

    private void Awake()
    {
        UI_EventSystem.NewPanelOpenedEvent += NewPanelOpened;
        UI_EventSystem.OnPauseGameEvent += OnPauseGame;
        UI_EventSystem.OnTryInterruptPauseEvent += OnTryInterruptPause;
        UI_EventSystem.ToggleStatsEvent += ToggleStats;
        GP_EventSystem.OnPrepareRoundEvent += OnPrepareRound;
        GP_EventSystem.OnPlayerKilledEvent += OnPlayerKilled;
        GP_EventSystem.OnRoundWinnerConfirmedEvent += OnRoundWinnerConfirmed;

        GameStatsPanel.SetActive(true);
    }

    private void NewPanelOpened(GameObject newPanel)
    {
        currentPanel.SetActive(false);
        newPanel.SetActive(true);
        currentPanel = newPanel;
    }

    private void OnDestroy()
    {
        UI_EventSystem.NewPanelOpenedEvent -= NewPanelOpened;
        UI_EventSystem.OnPauseGameEvent -= OnPauseGame;
        UI_EventSystem.OnTryInterruptPauseEvent -= OnTryInterruptPause;
        UI_EventSystem.ToggleStatsEvent -= ToggleStats;
        GP_EventSystem.OnPrepareRoundEvent -= OnPrepareRound;
        GP_EventSystem.OnPlayerKilledEvent -= OnPlayerKilled;
        GP_EventSystem.OnRoundWinnerConfirmedEvent -= OnRoundWinnerConfirmed;
    }

    private void OnRoundWinnerConfirmed(bool winner)
    {
        RoundResult.text = winner ? "ROUND WON" : "ROUND LOST";
        RoundResult.gameObject.SetActive(true);
    }

    private void OnTryInterruptPause()
    {
        if (PausePanel.activeSelf)
        {
            PausePanel.SetActive(false);
            GameManager.IsPaused = false;
            Cursor.visible = false;
        }
    }

    private void OnPauseGame()
    {
        GameStatsPanel.SetActive(false);
        PausePanel.SetActive(true);
        currentPanel = PausePanel;
        Cursor.visible = true;
        GameManager.IsPaused = true;
    }

    private void ToggleStats()
    {
        GameStatsPanel.SetActive(!GameStatsPanel.activeSelf);
    }

    private void OnPrepareRound()
    {
        RoundResult.gameObject.SetActive(false);
        GameStatsPanel.SetActive(false);
        StartCoroutine(RoundCountDown(3));
    }

    private IEnumerator RespawnCountDown(int seconds)
    {
        Countdown.gameObject.SetActive(true);
        for (int i = seconds; i >= 1; i--)
        {
            Countdown.text = i.ToString();
            //TODO: play sound for second change
            AudioManager.PlaySFX("Death_CD_Tick");
            yield return new WaitForSecondsRealtime(1);
        }
        Countdown.gameObject.SetActive(false);
        GP_EventSystem.Respawn();
    }

    private void OnPlayerKilled(Events.KillData data)
    {
        if (data.KilledPlayer == SteamClient.SteamId) //if the local user is dead
        {
            int respawnDelay = int.Parse(NetworkManager.CurrentLobby.GetData("RespawnDelay"));
            StartCoroutine(RespawnCountDown(respawnDelay));
        }
    }

    private IEnumerator RoundCountDown(int seconds)
    {
        Countdown.gameObject.SetActive(true);
        for (int i = seconds; i >= 1; i--)
        {
            Countdown.text = i.ToString();
            //TODO: play sound for second change
            AudioManager.PlaySFX("Round_CD_Tick");
            yield return new WaitForSecondsRealtime(1);
        }
        AudioManager.PlaySFX("Round_CD_End");
        Countdown.gameObject.SetActive(false);
        RoundTime.text = NetworkManager.CurrentLobby.GetData("RoundDuration");
        RoundTime.gameObject.SetActive(true);
        GP_EventSystem.StartRound();
        StartCoroutine(ProcessRound());
        //TODO: play sound for game start
    }

    private IEnumerator ProcessRound()
    {
        int time = int.Parse(NetworkManager.CurrentLobby.GetData("RoundDuration"));
        for (int i = time; i >= 1; i--)
        {
            RoundTime.text = i.ToString();
            yield return new WaitForSecondsRealtime(1);
        }

        RoundTime.gameObject.SetActive(false);
        GameStatsPanel.SetActive(true);

        GP_EventSystem.EndRound();
    }
}