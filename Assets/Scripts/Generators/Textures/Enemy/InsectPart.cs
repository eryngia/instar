using UnityEngine;

// An abstract part of a procedural insect.
public abstract class InsectPart {
    
    // Draw the part onto the pixel grid.
    public abstract void Color(Color[] pixels, Color color);

    // Draw animation frame 1 onto the pixel grid.
    public virtual void Color1(Color[] pixels, Color color) {
        Color(pixels, color);
    }

    // Draw animation frame 2 onto the pixel grid.
    public virtual void Color2(Color[] pixels, Color color) {
        Color(pixels, color);
    }

    // Draw the part highlights onto the pixel grid.
    public abstract void ColorHighlights(Color[] pixels, Color color);

    // Draw the highlights of animation frame 1 onto the pixel grid.
    public virtual void ColorHighlights1(Color[] pixels, Color color) {
        ColorHighlights(pixels, color);
    }

    // Draw the highlights of animation frame 2 onto the pixel grid.
    public virtual void ColorHighlights2(Color[] pixels, Color color) {
        ColorHighlights(pixels, color);
    }

    // Get the topmost pixel.
    public abstract int GetTop();
    // Get the bottommost pixel.
    public abstract int GetBottom();
    // Get the sidemost pixel.
    public abstract int GetSide();

}