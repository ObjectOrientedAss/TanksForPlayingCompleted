using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPanel : MonoBehaviour
{
    public GameObject CreateLobbyPanel; //set in inspector
    public GameObject FindLobbiesPanel; //set in inspector

    public void OnCreateLobbyButtonClick()
    {
        UI_EventSystem.OpenNewPanel(CreateLobbyPanel);
    }

    public void OnJoinLobbyButtonClick()
    {
        UI_EventSystem.OpenNewPanel(FindLobbiesPanel);
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }
}
