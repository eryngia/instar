using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

// Generates textures from Voronoi diagrams.
public static class BackgroundTextureGenerator {

    // Creates a pixel texture from Voronoi cells synchronously.
    public static Texture2D GetTextureFromVoronoi(List<VoronoiCell> cells, int shift, int pixelWidth, int pixelHeight, float edgeThickness, Color backgroundColor, Color cellColor, Color edgeColor) {
        Color[] pixelColors = GetColorsFromVoronoi(cells, shift, pixelWidth, pixelHeight, edgeThickness, backgroundColor, cellColor, edgeColor);
        return TextureUtils.FromColors(pixelColors, pixelWidth, pixelHeight);
    }

    // Creates a pixel color grid from Voronoi cells synchronously.
    public static Color[] GetColorsFromVoronoi(List<VoronoiCell> cells, int shift, int pixelWidth, int pixelHeight, float edgeThickness, Color backgroundColor, Color cellColor, Color edgeColor) {
        
        List<Vector2[]> boundaries = VoronoiDiagram.GetBoundaryVertexPaths(cells);
        Color[] pixelColors = new Color[pixelWidth * pixelHeight];

        // calculation performed as fast as possible in parallel
        Parallel.For(0, pixelWidth, x => {
            for (int y = 0; y < pixelHeight; y++) {
                pixelColors[y * pixelWidth + x] = GetColor(new Vector2(x, y + shift), cells, boundaries, edgeThickness, backgroundColor, cellColor, edgeColor);
            }
        });

        return pixelColors;
    }

    // Creates a pixel texture from Voronoi cells asynchronously.
    public static async Task<Color[]> GetColorsFromVoronoiAsync(List<VoronoiCell> cells, int shift, int pixelWidth, int pixelHeight, float edgeThickness, Color backgroundColor, Color cellColor, Color edgeColor) {
        return await Task.Run(() => {
            List<Vector2[]> boundaries = VoronoiDiagram.GetBoundaryVertexPaths(cells);
            Color[] pixelColors = new Color[pixelWidth * pixelHeight];

            // slower process to avoid affecting down gameplay with heavy resource use
            for (int x = 0; x < pixelWidth; x++) {
                for (int y = 0; y < pixelHeight; y++) {
                    pixelColors[y * pixelWidth + x] = GetColor(new Vector2(x, y + shift), cells, boundaries, edgeThickness, backgroundColor, cellColor, edgeColor);
                }
            }

            return pixelColors;
        });
    }

    // Calculate the color of an individual pixel.
    private static Color GetColor(Vector2 pixel, List<VoronoiCell> cells, List<Vector2[]> boundaries, float edgeThickness, Color backgroundColor, Color cellColor, Color edgeColor) {
        // check if on any edge
        foreach (VoronoiCell cell in cells) {
            foreach (Edge2 edge in cell.edges) {
                if (IsOnEdge(pixel, edge, edgeThickness)) return edgeColor;
            }
        }

        // check if inside any boundary
        foreach(Vector2[] boundary in boundaries) {
            if (IsInBoundary(pixel, boundary)) return cellColor;
        }

        return backgroundColor;
    }

    // If the pixel is within a Voronoi diagram.
    private static bool IsInBoundary(Vector2 pixel, Vector2[] boundary) {
        bool inBoundary = false;
        for (int i = 0; i < boundary.Length; i++) {
            Vector2 v1 = boundary[i];
            Vector2 v2 = boundary[(i + 1) % boundary.Length];

            // raycast horizontally and count even/odd intersections with boundary
            if ((v1.y > pixel.y) != (v2.y > pixel.y) && 
                pixel.x < ((v2.x - v1.x) * (pixel.y - v1.y) / (v2.y - v1.y) + v1.x)) {
                inBoundary = !inBoundary;
            }
        }
        
        return inBoundary;
    }

    // If the pixel is close to a Voronoi edge.
    private static bool IsOnEdge(Vector2 point, Edge2 edge, float edgeThickness) {
        // calculate intersection between edge and normal to point
        float lengthSquared = (edge.v2 - edge.v1).sqrMagnitude;
        float t = Mathf.Clamp01(Vector2.Dot(point - edge.v1, edge.v2 - edge.v1) / lengthSquared); 
        Vector2 projection = edge.v1 + t * (edge.v2 - edge.v1);
        
        return Vector2.Distance(point, projection) <= edgeThickness;
    }
}