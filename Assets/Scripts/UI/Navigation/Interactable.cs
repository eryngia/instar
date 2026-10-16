
using System;
using UnityEngine;

// An interactable button.
public abstract class Interactable : MonoBehaviour {

    public event EventHandler<EventArgs> OnInteractSuccess;
    public event EventHandler<EventArgs> OnInteractFailure;
    
    // Interact and trigger repose event.
    public bool RunInteract() {
        bool isSuccess = Interact();
        (isSuccess ? OnInteractSuccess : OnInteractFailure)?.Invoke(this, EventArgs.Empty);
        return isSuccess;
    }

    public abstract bool Interact();

}