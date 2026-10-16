using UnityEngine;

// Fix trails to not show when moving backwards.
public class PlayerTrail : MonoBehaviour {

    private PlayerController playerController;
    private TrailRenderer trailRenderer;

    private void Start() {
        playerController = GetComponentInParent<PlayerController>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    private void Update() {
        if (trailRenderer == null) return;
        if (playerController.moveDirectionInput.y > 0) 
            trailRenderer.emitting = true;
        else
            trailRenderer.emitting = false;
    }

}