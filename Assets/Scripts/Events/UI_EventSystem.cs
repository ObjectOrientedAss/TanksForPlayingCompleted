using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UI_EventSystem
{
    public delegate void OnPauseGame();
    public static event OnPauseGame OnPauseGameEvent;

    public static void PauseGame()
    {
        OnPauseGameEvent?.Invoke();
    }

    public delegate void OnTryInterruptPause();
    public static event OnTryInterruptPause OnTryInterruptPauseEvent;

    public static void TryInterruptPause()
    {
        OnTryInterruptPauseEvent?.Invoke();
    }

    public delegate DialogBehaviour OnDialogCreated(string title, string content, bool closeButtonIncluded = true);
    public static event OnDialogCreated DialogCreatedEvent;

    public static DialogBehaviour CreateDialog(string title, string content, bool closeButtonIncluded = true)
    {
        return DialogCreatedEvent?.Invoke(title, content, closeButtonIncluded);
    }

    public delegate void OnNewPanelOpened(GameObject newPanel);
    public static event OnNewPanelOpened NewPanelOpenedEvent;

    public static void OpenNewPanel(GameObject newPanel)
    {
        NewPanelOpenedEvent?.Invoke(newPanel);
    }

    public delegate void OnToggleStats();
    public static event OnToggleStats ToggleStatsEvent;

    public static void ToggleStats()
    {
        ToggleStatsEvent?.Invoke();
    }

    public delegate void OnStartLoading();
    public static event OnStartLoading OnStartLoadingEvent;

    public static void StartLoading()
    {
        OnStartLoadingEvent?.Invoke();
    }

    public delegate void OnStopLoading();
    public static event OnStopLoading OnStopLoadingEvent;

    public static void StopLoading()
    {
        OnStopLoadingEvent?.Invoke();
    }

    public delegate void OnStartLoadingAsync(int sceneIndex);
    public static event OnStartLoadingAsync OnStartLoadingAsyncEvent;

    public static void StartLoadingAsync(int sceneIndex)
    {
        OnStartLoadingAsyncEvent?.Invoke(sceneIndex);
    }
}
