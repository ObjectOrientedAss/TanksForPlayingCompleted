using Steamworks;
using Steamworks.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public GameObject NetworkManagerPrefab; //set in inspector
    public GameObject LoadingScreenPrefab; //set in inspector
    public GameObject DialogPrefab; //set in inspector
    public GameObject LobbyPanel; //set in inspector
    private GameObject currentPanel;

    void Awake()
    {
        Cursor.visible = true;

        //instantiate all the needed stuff to be used between scenes
        GameObject networkManager = GameObject.Find("NetworkManager");
        if (networkManager == null)
            Instantiate(NetworkManagerPrefab).name = "NetworkManager";

        GameObject loadingScreen = GameObject.Find("LoadingScreen");
        if(loadingScreen == null)
            Instantiate(LoadingScreenPrefab).name = "LoadingScreen";

        GameObject audioManager = GameObject.Find("AudioManager");
        if (audioManager == null)
            new GameObject("AudioManager").AddComponent<AudioManager>();
        AudioManager.PlayMusic("OST");

        GameObject vfxManager = GameObject.Find("VFXManager");
        if (vfxManager == null)
            new GameObject("VFXManager").AddComponent<VFXManager>();

        //-----------------------------------------------------------

        currentPanel = GameObject.Find("MainMenuPanel");
        UI_EventSystem.NewPanelOpenedEvent += NewPanelOpenedEvent;
        UI_EventSystem.DialogCreatedEvent += DialogCreatedEvent;
    }

    private void Start()
    {
        SteamMatchmaking.OnLobbyEntered += OnLobbyEntered;
    }

    private void OnDestroy()
    {
        UI_EventSystem.NewPanelOpenedEvent -= NewPanelOpenedEvent;
        UI_EventSystem.DialogCreatedEvent -= DialogCreatedEvent;
        SteamMatchmaking.OnLobbyEntered -= OnLobbyEntered;
    }

    private void OnLobbyEntered(Lobby obj)
    {
        NewPanelOpenedEvent(LobbyPanel);
    }

    /// <summary>
    /// Creates a dialog box with the given title and content.
    /// To add more buttons to the dialog, use the DialogBehaviour.AddButton method.
    /// </summary>
    /// <param name="title">The title of the dialog.</param>
    /// <param name="content">The content of the dialog.</param>
    /// <param name="closeButtonIncluded">If true or unspecified, the dialog comes with a default button to close and destroy it.</param>
    /// <returns></returns>
    private DialogBehaviour DialogCreatedEvent(string title, string content, bool closeButtonIncluded = true)
    {
        Transform mainCanvas = GameObject.Find("Canvas").transform;
        GameObject dialog = Instantiate(DialogPrefab, mainCanvas);
        DialogBehaviour db = dialog.GetComponent<DialogBehaviour>();
        db.Title.text = title;
        db.Content.text = content;
        if (closeButtonIncluded)
            db.AddButton("Close", db.Close);
        return db;
    }

    private void NewPanelOpenedEvent(GameObject newPanel)
    {
        currentPanel.SetActive(false);
        newPanel.SetActive(true);
        currentPanel = newPanel;
    }
}