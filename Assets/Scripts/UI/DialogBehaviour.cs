using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogBehaviour : MonoBehaviour
{
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Content;
    public Transform ButtonsContainer;

    public GameObject ButtonPrefab;

    public void AddButton(string text, UnityAction action)
    {
        GameObject button = Instantiate(ButtonPrefab, ButtonsContainer);
        button.GetComponentInChildren<TextMeshProUGUI>().text = text;
        button.GetComponent<Button>().onClick.AddListener(action);
    }

    public void Close()
    {
        Destroy(gameObject);
    }
}
