
using System.Collections.Generic;
using UnityEngine;

// Button for changing palettes.
public class ColorMapInteractable : Interactable {

    [SerializeField] private List<ColorMap.Palette> palettes;
    private int currentIndex = 0;
    private ColorMap colorMap;

    public void Start() {
        colorMap = FindAnyObjectByType<ColorMap>();
        currentIndex = CameraManager.Instance.paletteIndex + 1;
    }

    // Set next palette, or default if reached end of list.
    public override bool Interact() {
        if (currentIndex < palettes.Count) {
            SetCurrent(); 
            currentIndex++; 
        } else {
            SetDefault(); 
            currentIndex = 0; 
        }
        return true;
    }

    // Set default color palette.
    private void SetDefault() {
        CameraManager.Instance.SetDefaultPalette();
        colorMap.UpdateColorPalette();
    }

    // Set current color palette.
    private void SetCurrent() {
        CameraManager.Instance.SetColorPalette(palettes[currentIndex], currentIndex);
        colorMap.UpdateColorPalette();
    }

}