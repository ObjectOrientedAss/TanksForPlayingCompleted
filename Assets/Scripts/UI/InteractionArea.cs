using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionArea : MonoBehaviour
{
    public TextMeshProUGUI InteractionHint; //set in inspector
    private IInteractable nearestInteractable;
    private InputHandler inputHandler;

    private void Awake()
    {
        InteractionHint.gameObject.SetActive(false);
        inputHandler = GetComponentInParent<InputHandler>();

        GP_EventSystem.OnInteractableRemovedEvent += OnInteractableRemoved;
        GP_EventSystem.OnEndRoundEvent += OnEndRound;
    }

    private void OnEndRound()
    {
        if (nearestInteractable != null)
        {
            nearestInteractable = null;
            InteractionHint.text = "";
            InteractionHint.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        GP_EventSystem.OnInteractableRemovedEvent -= OnInteractableRemoved;
        GP_EventSystem.OnEndRoundEvent -= OnEndRound;
    }

    private void OnInteractableRemoved(IInteractable interactable)
    {
        if(interactable == nearestInteractable)
        {
            nearestInteractable = null;
            InteractionHint.text = "";
            InteractionHint.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IInteractable interactable = other.gameObject.GetComponent<IInteractable>();

        if (nearestInteractable == null)
        {
            nearestInteractable = interactable;
            InteractionHint.text = nearestInteractable.InteractionText;
            InteractionHint.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IInteractable interactable = other.gameObject.GetComponent<IInteractable>();

        if (nearestInteractable != null && nearestInteractable == interactable)
        {
            nearestInteractable = null;
            InteractionHint.text = "";
            InteractionHint.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (inputHandler.interactButtonPressed && nearestInteractable != null)
            nearestInteractable.RequestInteraction();
    }
}