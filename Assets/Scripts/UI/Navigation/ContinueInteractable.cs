

// Button for loading next level.
public class ContinueInteractable : Interactable {
    
    // Load the next level.
    public override bool Interact() {
        SceneManager.Instance.LoadGameScene();
        return true;
    }

}