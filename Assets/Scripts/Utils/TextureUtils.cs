using System;
using System.Collections.Generic;
using UnityEngine;

// Util functions for texture drawing.
public static class TextureUtils {

    // Convert a texture into a sprite with the standard PPU.
    public static Sprite ToSprite(Texture2D texture, Vector2 pivot) {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), pivot, GameManager.Constants.pixelsPerUnit);
    }

    // Create a pixel texture from an array of colors.
    public static Texture2D FromColors(Color[] pixelColors, int pixelWidth, int pixelHeight) {
        Texture2D texture = new Texture2D(pixelWidth, pixelHeight);
        texture.filterMode = FilterMode.Point;
        texture.SetPixels(pixelColors);
        texture.Apply();

        return texture; 
    }

    // Flood fill background color on all edges with given color.
   public static void MakeBackgroundTransparent(Color[] pixels, Color originalBackgroundColor) {
        Queue<Vector2> queue = new Queue<Vector2>();
        int width = (int) Mathf.Sqrt(pixels.Length);

        for (int i = 0; i < width; i++) {
            if (IsBackground(i, 0)) queue.Enqueue(new Vector2(i, 0)); // top border
            if (IsBackground(i, width - 1)) queue.Enqueue(new Vector2(i, width - 1)); // bottom border
            if (IsBackground(0, i)) queue.Enqueue(new Vector2(0, i)); // left border
            if (IsBackground(width - 1, i)) queue.Enqueue(new Vector2(width - 1, i)); // right border
        }

        bool[,] visited = new bool[width, width];
        while (queue.Count > 0) {
            Vector2 current = queue.Dequeue();
            int currentX = (int) current.x;
            int currentY = (int) current.y;

            if (visited[currentX, currentY]) continue;

            visited[currentX, currentY] = true;

            if (IsBackground(currentX, currentY)) 
                pixels[currentY * width + currentX] = Color.clear; 

            Vector2[] neighbors = new Vector2[] {
                new Vector2(currentX + 1, currentY),
                new Vector2(currentX - 1, currentY),
                new Vector2(currentX, currentY + 1),
                new Vector2(currentX, currentY - 1),
            };

            foreach (Vector2 neighbor in neighbors) {
                if (IsInBounds(neighbor, width) 
                    && !visited[(int) neighbor.x, (int) neighbor.y] 
                    && IsBackground( (int) neighbor.x, (int) neighbor.y)) {
                    queue.Enqueue(neighbor);
                }
            }
        }

        bool IsBackground(int x, int y) {
            return pixels[y * width + x] == originalBackgroundColor;
        }
    } 


    // Returns true if the x and y coordinates are within bounds.
    public static bool IsInBounds(Vector2 pixel, int pixelSize) {
        return pixel.x >= 0 && pixel.x < pixelSize && pixel.y >= 0 && pixel.y < pixelSize;
    }

    // Returns an interpolated pixel along a line segment.
    public static Vector2 PointOnLine(Vector2 p1, Vector2 p2, float t) {
        int dx = (int) p2.x - (int) p1.x;
        int dy = (int) p2.y - (int) p1.y;

        int steps = Mathf.Max(Mathf.Abs(dx), Mathf.Abs(dy)); 
        float xIncrement = (float)dx / steps;
        float yIncrement = (float)dy / steps;

        float x = p1.x + xIncrement * steps * t;
        float y = p1.y + yIncrement * steps * t;

        return new Vector2(Mathf.Round(x), Mathf.Round(y));
    }    

    // Returns the pixels along a line segment.
    public static List<Vector2> PointsOnLine(Vector2 p1, Vector2 p2) {
        List<Vector2> points = new List<Vector2>();

        int dx = (int) p2.x - (int) p1.x;
        int dy = (int) p2.y - (int) p1.y;

        int steps = Mathf.Max(Mathf.Abs(dx), Mathf.Abs(dy)); 
        float xIncrement = (float)dx / steps;
        float yIncrement = (float)dy / steps;

        float x = p1.x;
        float y = p1.y;

        for (int i = 0; i <= steps; i++) {
            points.Add(new Vector2(Mathf.Round(x), Mathf.Round(y))); 
            x += xIncrement;
            y += yIncrement;
        }

        return points;
    }    

    // Returns an interpolated pixel along a quadratic Bezier curve.
    public static Vector2 PointOnCurve(Vector2 p1, Vector2 p2, float curvature, float t) {
        Vector2 midpoint = (p1 + p2) / 2;
        Vector2 direction = new Vector2(p2.y - p1.y, -(p2.x - p1.x)).normalized; 
        Vector2 controlPoint = midpoint + direction * curvature;

        float x = Mathf.Ceil(Mathf.Pow(1 - t, 2) * p1.x + 2 * (1 - t) * t * controlPoint.x + Mathf.Pow(t, 2) * p2.x);
        float y = Mathf.Ceil(Mathf.Pow(1 - t, 2) * p1.y + 2 * (1 - t) * t * controlPoint.y + Mathf.Pow(t, 2) * p2.y);
        
        return new Vector2(x, y);
    }

    // Returns multiple interpolated points along two combined curves.
    public static List<Vector2> PointsOnCurves(Vector2 p1, float curve1, Vector2 p2, float curve2, Vector3 p3, float[] ts) {
        List<Vector2> points = new List<Vector2>();

        // estimate with straight lengths
        float length1 = Vector2.Distance(p1, p2);
        float length2 = Vector2.Distance(p2, p3);
        float lengthTotal = length1 + length2;

        foreach (float t in ts) {
            float d = t * lengthTotal;
            if (d <= length1) {
                float t1 = d / length1;  // normalise within p1, p2
                points.Add(PointOnCurve(p1, p2, curve1, t1));
            } else {
                float t2 = (d - length1) / length2;  // normalize within p2, p3
                points.Add(PointOnCurve(p2, p3, curve2, t2));
            }
        }

        return points;
    }

    // Returns the pixels along a quadratic Bezier curve.
    public static List<Vector2> PointsOnCurve(Vector2 p1, Vector2 p2, float curvature) {
        List<Vector2> points = new List<Vector2>();
    
        Vector2 midpoint = (p1 + p2) / 2;
        Vector2 direction = new Vector2(p2.y - p1.y, -(p2.x - p1.x)).normalized;
        Vector2 controlPoint = midpoint + direction * curvature;
        
        int steps = (int) (2 * (Mathf.Abs(p1.x - p2.x) + Mathf.Abs(p1.y - p2.y)));

        for (int i = 0; i <= steps; i++) {
            float t = (float) i / steps;
            float x = Mathf.Ceil(Mathf.Pow(1 - t, 2) * p1.x + 2 * (1 - t) * t * controlPoint.x + Mathf.Pow(t, 2) * p2.x);
            float y = Mathf.Ceil(Mathf.Pow(1 - t, 2) * p1.y + 2 * (1 - t) * t * controlPoint.y + Mathf.Pow(t, 2) * p2.y);
            points.Add(new Vector2(x, y));
        }

        return points;
    }

    // Colors a list of pixels on the left of the grid.
    public static void ColorPixelsLeft(Color[] pixels, List<Vector2> pixelsToColor, Color color) {
        foreach (Vector2 pixelToColor in pixelsToColor) {
            ColorPixelLeft(pixels, pixelToColor, color);
        }
    }

    // Colors a list of pixels on the right of the grid.
    public static void ColorPixelsRight(Color[] pixels, List<Vector2> pixelsToColor, Color color) {
        foreach (Vector2 pixelToColor in pixelsToColor) {
            ColorPixelRight(pixels, pixelToColor, color);
        }
    }

    // Colors a list of pixels on both sides of the grid.
    public static void ColorPixelsMirrored(Color[] pixels, List<Vector2> pixelsToColor, Color color) {
        foreach (Vector2 pixelToColor in pixelsToColor) {
            ColorPixelMirrored(pixels, pixelToColor, color);
        }
    }

    // Colors a pixels on both sides of the grid.
    public static void ColorPixelMirrored(Color[] pixels, Vector2 pixelToColor, Color color) {
        ColorPixelLeft(pixels, pixelToColor, color);
        ColorPixelRight(pixels, pixelToColor, color);
    }

    // Colors a pixel on the right of the grid.
    public static void ColorPixelRight(Color[] pixels, Vector2 pixelToColor, Color color) {
        int pixelSize = (int) Mathf.Sqrt(pixels.Length);
        Vector2 pixel = new Vector2(pixelToColor.x + pixelSize / 2, pixelToColor.y);
        if (IsInBounds(pixel, pixelSize)) {
            pixels[(int) pixel.y * pixelSize +  (int) pixel.x] = color;
        }
    }

    // Colors a pixel on the left of the grid.
    public static void ColorPixelLeft(Color[] pixels, Vector2 pixelToColor, Color color) {
        int pixelSize = Mathf.RoundToInt(Mathf.Sqrt(pixels.Length));
        Vector2 pixel = new Vector2(pixelSize / 2 - 1 - pixelToColor.x, pixelToColor.y);
        if (IsInBounds(pixel, pixelSize)) {
            pixels[(int) pixel.y * pixelSize + (int) pixel.x] = color;
        }
    }

    // Converts a grid proportion into a number of pixels, with the ratio 2:pixelSize.
    public static Vector2 GetPixel(float x, float y, int pixelSize) {
        return new Vector2(Mathf.Round(x * pixelSize / 2), Mathf.Round(y * pixelSize / 2));
    }

    
    // Fills a flooded are with a color.
    public static void FloodFill(Color[] pixels, Vector2 start, Color fillColor, List<Vector2> outline) {
        ColorPixelsMirrored(pixels, Flood(pixels, start, outline), fillColor); 
    }

    // Floods an area beginning at the start pixel and bounded by the outline. If too much is
    // filled, returns an error.
    public static List<Vector2> Flood(Color[] pixels, Vector2 start, List<Vector2> outline) {
        int pixelSize = (int) Mathf.Sqrt(pixels.Length);
        int pixelsFilled = 0;
        
        List<Vector2> visited = new List<Vector2>();
        Stack<Vector2> stack = new Stack<Vector2>();
        
        stack.Push(start);

        while (stack.Count > 0) {
            Vector2 pixel = stack.Pop();

            if (IsInBounds(pixel, pixelSize) && !outline.Contains(pixel) && !visited.Contains(pixel)) {
                visited.Add(pixel);
                pixelsFilled += 2;

                stack.Push(new Vector2(pixel.x + 1, pixel.y));
                stack.Push(new Vector2(pixel.x - 1, pixel.y));
                stack.Push(new Vector2(pixel.x, pixel.y + 1));
                stack.Push(new Vector2(pixel.x, pixel.y - 1));
            }
        }

        if (pixelsFilled > 0.8 * pixels.Length)
            throw new Exception("Likely gap in outline");

        return visited;
    }

}