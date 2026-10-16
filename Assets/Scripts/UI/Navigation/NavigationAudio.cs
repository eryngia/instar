using System;
using UnityEngine;

// Plays sound effects for menu navigation and interaction.
public class NavigationAudio : MonoBehaviour {

    [SerializeField] private AudioClip[] navigationAudio;
    [SerializeField] private AudioClip[] defaultInteractAudio;
    private Navigation navigation;

    private void Start() {
        navigation = GetComponent<Navigation>();
        navigation.OnNavigate += OnNavigateHandler;
        navigation.OnDefaultInteract += OnDefaultInteractHandler;
    }

    private void OnNavigateHandler(object sender, EventArgs e) {
        AudioUtils.PlayRandom(navigationAudio);
    }

    private void OnDefaultInteractHandler(object sender, EventArgs e) {
        if (navigation.GetSelection().GetComponent<InteractableAudio>() == null)
            AudioUtils.PlayRandom(defaultInteractAudio);
    }

}