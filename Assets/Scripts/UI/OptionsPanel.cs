using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsPanel : MonoBehaviour
{
    public GameObject PausePanel;
    public Slider EffectsVolume; //set in inspector
    public Slider MusicVolume; //set in inspector

    private void OnEnable()
    {
        EffectsVolume.value = AudioManager.EffectsVolume;
        MusicVolume.value = AudioManager.MusicVolume;
    }

    public void OnEffectsVolumeChanged(float val)
    {
        AudioManager.EffectsVolume = val;
    }

    public void OnMusicVolumeChanged(float val)
    {
        AudioManager.MusicVolume = val;
    }

    public void OnBackButtonClick()
    {
        UI_EventSystem.OpenNewPanel(PausePanel);
    }
}
