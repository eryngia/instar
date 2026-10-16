using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.SceneManagement.SceneManager;

// Apply different color palettes to the game.
public class ColorMap : MonoBehaviour {
    [SerializeField] private Material colorMappingMaterial;
    
    [Serializable]
    public class Palette {
        public Color whiteColor, blackColor, redColor;
        public Palette() {}
        public Palette(Palette palette) {
            whiteColor = palette.whiteColor;
            blackColor = palette.blackColor;
            redColor = palette.redColor;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        UpdateColorPalette();
    }

     private void OnEnable() {
        sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() {
        sceneLoaded -= OnSceneLoaded;
    }

    // Blit to apply shader to entire image.
    private void OnRenderImage(RenderTexture src, RenderTexture dest) {
        Graphics.Blit(src, dest, colorMappingMaterial);
    }

    // Set color palette to new palette.
    public void UpdateColorPalette() {
        Palette palette = CameraManager.Instance.palette;
        colorMappingMaterial.SetColor("_ColorWhite", palette.whiteColor);
        colorMappingMaterial.SetColor("_ColorBlack", palette.blackColor);
        colorMappingMaterial.SetColor("_ColorRed", palette.redColor);
    }

}
