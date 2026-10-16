using System.Collections.Generic;
using UnityEngine;

// Perform filtering of points based on fractal noise curve.
public static class Noise { 

    // Filter points using a Perlin noise curve.
    public static List<Vector2> FilterPoints(
        List<Vector2> points, 
        float width, float height, 
        int octaves = 5, 
        float frequency = 15f, float amplitude = 1f, 
        float lacunarity = 2f, float gain = 0.5f, 
        float pillarFrequency = 0.001f, float pillarHeight = 1f, float pillarWidth = 50f) {
        
        List<Vector2> filteredPoints = new List<Vector2>();
        int noiseSeed = Random.Range(0, 10000);

        // generate pillars
        float[] pillarOffsets = new float[(int) height];
        for (int y = 0; y < height; y++) {
            if (Random.value < pillarFrequency) {
                float thisPillarWidth = RandomUtils.RangeAvg(0.5f, 1.5f) * pillarWidth;
                float thisPillarHeight = RandomUtils.RangeAvg(0.5f, 1.5f) * pillarHeight * width;
                
                for (int j = 0; j < thisPillarWidth && y - j > 0; j++) {
                    pillarOffsets[y - j] += thisPillarHeight;
                }
            } 
        }

        // filter the points based on the fractal noise curve
        foreach (Vector2 point in points) {
            float noise = FractalNoise(point.y / height, octaves, frequency, lacunarity, gain, noiseSeed);
            if (point.x < noise * amplitude * width + pillarOffsets[(int) point.y]) {
                filteredPoints.Add(point);
            }
        }

        return filteredPoints;
    }

    // Generate normalized fractal Perlin noise with multiple octaves.
    private static float FractalNoise(float y, int octaves, float frequency, float lacunarity, float gain, int seed) {
        float amplitude = 1f;
        float sum = 0f;
        float maxValue = 0f;

        for (int i = 0; i < octaves; i++) {
            sum += Mathf.PerlinNoise(0 + seed, y * frequency + seed) * amplitude;
            maxValue += amplitude;

            frequency *= lacunarity;
            amplitude *= gain;
        }

        return sum / maxValue;
    }

}