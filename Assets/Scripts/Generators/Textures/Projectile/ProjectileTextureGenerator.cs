using UnityEngine;

// Generates pixel projectile textures (unused in favor of pixel perfect conversion of high res image).
public static class ProjectileTextureGenerator {

    public static Texture2D GeneratePixels(float height, float width, Color innerColor, Color outerColor) {

        int pixelWidth = Mathf.CeilToInt(width * GameManager.Constants.pixelsPerUnit);
        if (pixelWidth % 2 == 0) pixelWidth += 1; // force odd for symmetry

        int pixelHeight = Mathf.CeilToInt(height * GameManager.Constants.pixelsPerUnit);
        if (pixelHeight % 2 == 0) pixelHeight += 1; // force odd for symmetry

        Color[] colors = new Color[pixelWidth * pixelHeight];
    
        float centerX = (pixelWidth - 1) * 0.5f;
        float centerY = (pixelHeight - 1) * 0.5f;

        for (int y = 0; y < pixelHeight; y++) {
            for (int x = 0; x < pixelWidth; x++) {

                float normalizedX = (x - centerX) / centerX;
                float normalizedY = (y - centerY) / centerY;

                float distSq = (normalizedX * normalizedX) + (normalizedY * normalizedY);

                if (distSq <= 0.5f) {
                    colors[y * pixelWidth + x] = innerColor;  
                } else if (distSq <= 1f) {
                    colors[y * pixelWidth + x] = outerColor;  
                } else {
                    colors[y * pixelWidth + x] = Color.clear;  
                }
            }
        }

        return TextureUtils.FromColors(colors, pixelWidth, pixelHeight);
    }

}
