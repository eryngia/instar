using System;
using UnityEngine;

// Custom success and failure sound effect for interacting with button.
public class InteractableAudio : MonoBehaviour {

    [SerializeField] private AudioClip[] interactSuccessAudio;
    [SerializeField] private AudioClip[] interactFailureAudio;

    private void Start() {
        GetComponent<Interactable>().OnInteractSuccess += OnInteractSuccessHandler;
        GetComponent<Interactable>().OnInteractFailure += OnInteractFailureHandler;
    }

    private void OnInteractSuccessHandler(object sender, EventArgs e) {
        AudioUtils.PlayRandom(interactSuccessAudio);
    }

    private void OnInteractFailureHandler(object sender, EventArgs e) {
        AudioUtils.PlayRandom(interactFailureAudio);
    }

}