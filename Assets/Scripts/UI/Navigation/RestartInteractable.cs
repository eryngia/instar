
// Button for returning to start.
public class RestartInteractable : Interactable {
    
    // Load start scene.
    public override bool Interact() {
        SceneManager.Instance.LoadStartScene();
        return true;
    }

}