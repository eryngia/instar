using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// A wing of a procedural insect.
public class InsectWing : InsectAppendage {
    
    public Vector2 animation;
    public bool outline;
    public int spotSize;

    public InsectWing(Vector2 start, float curve, Vector2 end, Vector2 animation, bool outline, int spotSize) : base(start, curve, end){
        this.animation = animation;
        this.outline = outline;
        this.spotSize = spotSize;
    }
    public InsectWing(int pixelSize, Vector2 start, float width, float height, float curve, float animation, bool outline, int spotsize) : base(pixelSize, start, width, height, curve){
        this.animation = new Vector2(animation, 0);
        this.outline = outline;
        this.spotSize = spotsize;
    }

    public override void Color(Color[] pixels, Color color) {
        TextureUtils.ColorPixelsMirrored(pixels, Points(Vector2.zero), color);
        TextureUtils.FloodFill(pixels, CenterPoint(Vector2.zero), UnityEngine.Color.black, Points(Vector2.zero));
    }

    public override void Color1(Color[] pixels, Color color) {
        TextureUtils.ColorPixelsMirrored(pixels, Points(animation), color);
        TextureUtils.FloodFill(pixels, CenterPoint(animation), UnityEngine.Color.black, Points(animation));
    } 

    public override void Color2(Color[] pixels, Color color) {
        TextureUtils.ColorPixelsMirrored(pixels, Points(-animation), color);
        TextureUtils.FloodFill(pixels, CenterPoint(-animation), UnityEngine.Color.black, Points(-animation));
    } 

    public override void ColorHighlights(Color[] pixels, Color color) {
        TextureUtils.FloodFill(pixels, CenterPoint(Vector2.zero), UnityEngine.Color.black, Points(Vector2.zero));
        if (outline) {
            TextureUtils.ColorPixelsMirrored(pixels, OutlinePoints(Vector2.zero), color);
        }
        ColorSpots(pixels, color, CenterPoint(Vector2.zero), Vector2.zero);    
    }

    public override void ColorHighlights1(Color[] pixels, Color color) {
        TextureUtils.FloodFill(pixels, CenterPoint(animation), UnityEngine.Color.black, Points(animation));
        if (outline) {
            TextureUtils.ColorPixelsMirrored(pixels, OutlinePoints(animation), color);
        }
        ColorSpots(pixels, color, CenterPoint(animation), animation);    
    }

    public override void ColorHighlights2(Color[] pixels, Color color) {
        TextureUtils.FloodFill(pixels, CenterPoint(-animation), UnityEngine.Color.black, Points(-animation));
        if (outline) {
            TextureUtils.ColorPixelsMirrored(pixels, OutlinePoints(-animation), color);
        }
        ColorSpots(pixels, color, CenterPoint(-animation), -animation);    
    }

    protected void ColorSpots(Color[] pixels, Color color, Vector2 start, Vector2 animation) {
        // color spots on the wing
        if (spotSize > 0) {
            int i = 0;
            List<Vector2> points = TextureUtils.Flood(pixels, start, Points(animation));
            foreach (var point in points) {
                if (i++ % 15 == 0) {
                    TextureUtils.ColorPixelMirrored(pixels, point, color);
                    if (spotSize == 1) continue;
                    if (points.Contains(point + new Vector2(0, 1)))
                        TextureUtils.ColorPixelMirrored(pixels, point + new Vector2(0, 1), color);
                    if (points.Contains(point + new Vector2(1, 0)))
                        TextureUtils.ColorPixelMirrored(pixels, point + new Vector2(1, 0), color);
                    if (points.Contains(point + new Vector2(-1, 0)))
                        TextureUtils.ColorPixelMirrored(pixels, point + new Vector2(-1, 0), color);
                    if (points.Contains(point + new Vector2(0, -1)))
                        TextureUtils.ColorPixelMirrored(pixels, point + new Vector2(0, -1), color);
                }
            }
        }
    }

    public virtual List<Vector2> Points(Vector2 animation) {
        return TextureUtils.PointsOnCurve(start, end + animation, curve).Concat(TextureUtils.PointsOnCurve(start, end + animation, -curve)).ToList();
    }

    public virtual List<Vector2> OutlinePoints(Vector2 animation) {
        List<Vector2> midline = TextureUtils.PointsOnLine(start, end).Distinct().ToList();
        Vector2 highlightStart = midline[1];
        Vector2 highlightEnd = midline[midline.Count - 2];
        return TextureUtils.PointsOnCurve(highlightStart, highlightEnd + animation, curve - 2).Concat(TextureUtils.PointsOnCurve(highlightStart, highlightEnd + animation, -curve + 2)).ToList();
    }

    public virtual Vector2 CenterPoint(Vector2 animation) {
        return TextureUtils.PointOnLine(start, end + animation, 0.5f);
    }

    public static InsectWing GenerateThin(int pixelSize, Vector2 start, float width, float height, float curve, bool outline, int spotSize) {
        return new(pixelSize, start, width, height, curve, 2f, outline, spotSize);
    }

    public static InsectWing GenerateBig(int pixelSize, Vector2 start, float width, float height, float curve, bool outline, int spotSize) {
        return new(pixelSize, start, width, height, curve, 2f, outline, spotSize);
    }

    public static InsectWing GenerateTriangle(int pixelSize, Vector2 start, float width, float height, bool outline, int spotSize) {
        return new InsectTriangleWing(pixelSize, start, width, height, 2f, outline, spotSize);
    }

}