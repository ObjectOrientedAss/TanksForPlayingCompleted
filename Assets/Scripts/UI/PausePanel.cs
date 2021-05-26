using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : MonoBehaviour
{
    public GameObject OptionsPanel;

    public void OnLeaveGameButtonClick()
    {
        NW_EventSystem.LeaveLobby();
        UI_EventSystem.StartLoadingAsync(0);
    }

    public void OnQuitToWindowsButtonClick()
    {
        Application.Quit();
    }

    public void OnOptionsButtonClick()
    {
        UI_EventSystem.OpenNewPanel(OptionsPanel);
    }
}
