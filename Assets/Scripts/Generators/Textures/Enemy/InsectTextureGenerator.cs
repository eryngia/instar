using System;
using UnityEngine;

// Generator for insect textures.
public static class InsectTextureGenerator {

    public static (Texture2D[] textures, int width, int height) Generate(int pixelSize, int highlightCount, int maxTries = 20) {

        Color[][] pixels = new Color[2][];
        int width = 0;
        int height = 0;

        // try a limited number of times to draw the insect
        for (int tries = 0; tries < maxTries; tries++) {
            try {
                // fill the pixel grid with black
                for (int i = 0; i < pixels.Length; i++) {
                    pixels[i] = new Color[pixelSize * pixelSize];
                    for (int j = 0; j < pixels[i].Length; j++) {
                        pixels[i][j] = Color.black;
                    }
                }

                // generate an insect and color the 2 animation frames
                Insect insect = Insect.Generate(pixelSize, highlightCount);
                insect.Color1(pixels[0], Color.white, Color.red);
                insect.Color2(pixels[1], Color.white, Color.red);

                // extract the width and height
                width = insect.GetWidth();
                height = insect.GetHeight();

                Debug.Log("Success generating insect (width " + width + ", height " + height + ")");
                break;

            } catch (Exception) {
                // irregular gaps in structure
                Debug.Log("Error generating insect, retrying");
            }
        }
    
        // remove backgrounds and create textures
        Texture2D[] textures = new Texture2D[pixels.Length];
        for (int i = 0; i < pixels.Length; i++) {
            TextureUtils.MakeBackgroundTransparent(pixels[i], Color.black);
            textures[i] = TextureUtils.FromColors(pixels[i], pixelSize, pixelSize);
        }

        return (textures, width, height);        
    }

}