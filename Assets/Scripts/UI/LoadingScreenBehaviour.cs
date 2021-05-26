using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenBehaviour : MonoBehaviour
{
    ///<summary>
    ///<para>The action to execute as soon as the loading screen goes totally fade to black.</para>
    ///<para>You can put all the operations to do while the user cannot see in here.</para>
    ///<para>Remember to specify the Loading Parameters before start loading.</para>
    ///<para>It is automatically cleared after the latest loading operation end.</para>
    ///</summary>
    public static Action LoadingAction;
    private static float screenRemovalWaitTime;
    private static TextMeshProUGUI loadingMessage;
    private static bool autoInterruptLoading;
    private Animator animator;
    private Canvas parentCanvas;
    private int asyncSceneIndex = -1;

    private void Awake()
    {
        parentCanvas = GetComponentInParent<Canvas>();
        animator = GetComponent<Animator>();
        loadingMessage = GameObject.Find("LoadingMessage").GetComponent<TextMeshProUGUI>();
        screenRemovalWaitTime = 1f;
        UI_EventSystem.OnStartLoadingEvent += StartLoading;
        UI_EventSystem.OnStopLoadingEvent += StopLoading;
        UI_EventSystem.OnStartLoadingAsyncEvent += LoadAsync;
        SceneManager.sceneLoaded += SceneLoaded;
        DontDestroyOnLoad(transform.parent.gameObject);
        Hide();
    }

    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        parentCanvas.worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    /// <summary>
    /// Explicitly define if the loading screen must automatically disappear after the loading action is over.
    /// If not, you will have to manually call the Game Event System -> End Loading event method.
    /// It is automatically reset to true after the latest loading operation end.
    /// </summary>
    /// <param name="auto"></param>
    public static void SetAutoInterruptLoading(bool auto)
    {
        autoInterruptLoading = auto;
    }

    /// <summary>
    /// Explicitly define the amount of time that must pass between the loading end, and the fade out animation start.
    /// It is automatically reset to 1 right after the latest loading operation end.
    /// </summary>
    /// <param name="time"></param>
    public static void SetScreenRemovalWaitTime(float time)
    {
        screenRemovalWaitTime = time >= 0f ? time : 1f;
    }

    /// <summary>
    /// Set the text to show during the loading.
    /// It is automatically reset to empty after the latest loading operation end.
    /// </summary>
    /// <param name="message"></param>
    public static void SetLoadingText(string message)
    {
        loadingMessage.text = message;
    }

    /// <summary>
    /// Callback for StartLoadingAsync event. New scenes should always be loaded asynchronously.
    /// </summary>
    /// <param name="sceneIndex"></param>
    private void LoadAsync(int sceneIndex)
    {
        if (sceneIndex >= 0)
        {
            asyncSceneIndex = sceneIndex;
            StartLoading();
        }
    }

    /// <summary>
    /// Callback for StartLoading event. Used for in-scene loadings. Remember to set the LoadingAction to be performed while the screen is black.
    /// If you omit this parameter before calling this method, you will have to manually stop the loading through event or the screen will "load" forever.
    /// </summary>
    private void StartLoading()
    {
        gameObject.SetActive(true);
        animator.SetBool("Loading", true);
    }

    /// <summary>
    /// This is executed as an animation event as soon as the screen goes totally fade to visible.
    /// </summary>
    public void Load()
    {
        if (LoadingAction != null)
        {
            LoadingAction.Invoke();
            //after performing the action, stop loading.
            if (autoInterruptLoading)
                UI_EventSystem.StopLoading();
        }
        else
            SceneManager.LoadSceneAsync(asyncSceneIndex);
    }

    /// <summary>
    /// Callback for StopLoading event.
    /// </summary>
    private void StopLoading()
    {
        //Debug.Log("STOP LOADING CALLED!");
        if(gameObject.activeSelf)
            StartCoroutine(StopLoad());
    }

    /// <summary>
    /// After a small amount of time, start fading out the loading screen and reset the loading data.
    /// </summary>
    /// <returns></returns>
    private IEnumerator StopLoad()
    {
        yield return new WaitForSeconds(screenRemovalWaitTime);
        animator.SetBool("Loading", false);
        LoadingAction = null;
        SetLoadingText("");
        screenRemovalWaitTime = 1f;
        autoInterruptLoading = true;
        asyncSceneIndex = -1;
        StopAllCoroutines();
    }

    /// <summary>
    /// This is executed as an animation event as soon as the screen returns totally fade to invisible.
    /// </summary>
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
