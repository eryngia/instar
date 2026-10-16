using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages the creation of environment and background textures.
public class LevelCreator : MonoBehaviour {
   
    private static Color backgroundColor = Color.clear;
    private static Color cellColor = Color.black;

    // Create asynchronously expanding environment texture without a noise curve.
    public List<VoronoiCell> Create(SpriteRenderer spriteRenderer, int totalSections, int initialSections, int width, int height, float radius, float edgeThickness, Color edgeColor) {
        int sectionHeight = height / totalSections;

        // generate points
        List<Vector2> points = PoissonDisc.GeneratePoints(width, height, radius);
        // generate Voronoi cells
        List<VoronoiCell> voronoiCells = VoronoiDiagram.Partition(points, width, height, int.MaxValue);
        // generate and apply sprite
        Texture2D currentTexture = BackgroundTextureGenerator.GetTextureFromVoronoi(voronoiCells, 0, width, initialSections * sectionHeight, edgeThickness, backgroundColor, cellColor, edgeColor);
        spriteRenderer.sprite = TextureUtils.ToSprite(currentTexture, Vector2.zero);
        
        // continue generating other sections
        StartCoroutine(ContinueGenerating(spriteRenderer, voronoiCells, edgeThickness, edgeColor, sectionHeight, initialSections, totalSections));
        return voronoiCells;
    }

    // Create asynchronously expanding environment texture with a noise curve.
    public List<VoronoiCell> CreateCurve(SpriteRenderer spriteRenderer, int totalSections, int initialSections, int width, int height, float radius, float edgeThickness, Color edgeColor) {
        int sectionHeight = height / totalSections;
        
        // generate filtered points
        List<Vector2> points = Noise.FilterPoints(PoissonDisc.GeneratePoints(width, height, radius), width, height);
        // generate Voronoi cells
        List<VoronoiCell> voronoiCells = VoronoiDiagram.Partition(points, width, height, 2 * radius);
        // generate and apply sprite
        Texture2D currentTexture = BackgroundTextureGenerator.GetTextureFromVoronoi(voronoiCells, 0, width, initialSections * sectionHeight, edgeThickness, backgroundColor, cellColor, edgeColor);
        spriteRenderer.sprite = TextureUtils.ToSprite(currentTexture, Vector2.zero);
        
        // continue generating other sections
        StartCoroutine(ContinueGenerating(spriteRenderer, voronoiCells, edgeThickness, edgeColor, sectionHeight, initialSections, totalSections));
        return voronoiCells;
    }

    // Continue generating the textures asynchronously and updating the sprite.
    private IEnumerator ContinueGenerating(SpriteRenderer spriteRenderer, List<VoronoiCell> voronoiCells, float edgeThickness, Color edgeColor, int sectionHeight, int initialSections, int totalSections) {
        Texture2D currentTexture = spriteRenderer.sprite.texture;
       
        for (int sectionIndex = initialSections; sectionIndex < totalSections; sectionIndex++) {
            Debug.Log("Generating new texture...");
            var task = BackgroundTextureGenerator.GetColorsFromVoronoiAsync(voronoiCells, sectionIndex * sectionHeight, currentTexture.width, sectionHeight, edgeThickness, backgroundColor, cellColor, edgeColor);
            yield return new WaitUntil(() => task.IsCompleted);

            Color[] nextPixels = task.Result;
            Color[] currentPixels = currentTexture.GetPixels();

            Texture2D combinedTexture = new Texture2D(currentTexture.width, currentTexture.height + sectionHeight);
            combinedTexture.SetPixels(0, 0, currentTexture.width, currentTexture.height, currentPixels);
            combinedTexture.SetPixels(0, currentTexture.height, currentTexture.width, sectionHeight, nextPixels);
            combinedTexture.filterMode = FilterMode.Point;
            combinedTexture.Apply();

            currentTexture = combinedTexture;
            spriteRenderer.sprite = TextureUtils.ToSprite(currentTexture, Vector2.zero);
            Debug.Log("Texture expanded.");
        }
    }
}
