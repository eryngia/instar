using System;
using UnityEngine;
using UnityEngine.UI;

// A segmented UI display bar.
public abstract class Bar : MonoBehaviour {

    [SerializeField] private Color fillColor;
    
    private int height;
    private int width;

    protected abstract int GetCurrent();
    protected abstract int GetMax();

    private void Start() {
        RectTransform rectTransform = GetComponent<RectTransform>(); 
        height = (int) rectTransform.rect.height;
        width = (int) rectTransform.rect.width;

        UpdateSprite();
    }

    protected void OnVariableChangedHandler(object sender, EventArgs e) {
        UpdateSprite();
    }

    // Update the bar sprite to reflect the variable state.
    private void UpdateSprite() {
        Texture2D texture = GenerateBarTexture(width, height, GetCurrent(), GetMax(), fillColor);
        Sprite sprite = TextureUtils.ToSprite(texture, Vector2.zero);
        GetComponent<Image>().sprite = sprite;
    } 

    // Create a texture with max segements and current filled segments.
    private Texture2D GenerateBarTexture(int width, int height, int current, int max, Color fillColor) {
        Color[] colors = new Color[width * height];

        int interval = Mathf.CeilToInt((float) height / max);

        // fill correct portion
        for (int y = 0; y < height && y < interval * current; y++) {
            for (int x = 0; x < width; x++) {
                colors[y * width + x] = fillColor;
            } 
        }

        // separate segments
        for (int y = interval - 1; y < height; y+= interval) {
            for (int x = 0; x < width; x++) {
                colors[y * width + x] = Color.clear;
            }
        }

        return TextureUtils.FromColors(colors, width, height);
    }
}
