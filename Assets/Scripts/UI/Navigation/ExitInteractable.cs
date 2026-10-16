using UnityEngine;

// Button to exit game.
public class ExitInteractable : Interactable {
    
    // Exit the editor or game.
    public override bool Interact() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; 
        #else
            Application.Quit();
        #endif
        return true;
    }

}