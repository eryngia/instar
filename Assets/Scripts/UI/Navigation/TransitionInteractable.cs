using UnityEngine;

// Button for transitioning to a new screen.
public class TransitionInteractable : Interactable {
    
    [SerializeField] private GameObject from;
    [SerializeField] private GameObject to;

    // Activate new screen and deactivate old screen.
    public override bool Interact() {
        to.SetActive(true);
        from.SetActive(false);
        return true;
    }

}