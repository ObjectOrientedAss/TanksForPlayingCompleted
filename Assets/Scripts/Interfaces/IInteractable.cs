using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    uint GUID { get; set; } //the global unique identifier of this interactable across all the players
    string InteractionText { get; } //the text to display when near to this interactable
    bool IsInteractable { get; set; } //does a player have permission to interact with this interactable?

    void RequestInteraction(); //request interaction with this interactable
    void Interact(); //localuser interacts with this interactable after permission is given
    void Remove(); //remove this interactable
}