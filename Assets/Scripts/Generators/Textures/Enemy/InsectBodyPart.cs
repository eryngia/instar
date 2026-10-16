using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// An insect main body part.
public class InsectBodyPart : InsectPart {
    
    public float height; // height of body part
    public Vector2 top; // middle top point
    public float topCurve; // curve between top and middle
    public Vector2 middle; // middle side point
    public float bottomCurve; // curve between middle and bottom
    public Vector2 bottom; // middle bottom point
    public bool outline = false; // outline highlight
    public int stripeWidth = 0; // stripe highlight
    public int spotSize = 0; // spot highlight
    public List<int> spotIndices = null; // placement of spots
    
    public InsectBodyPart(Vector2 top, float topCurve, Vector2 middle, float bottomCurve, Vector2 bottom, bool outline, int stripeWidth, int spotSize, List<int> spotIndices) {
        this.top = top;
        this.topCurve = topCurve;
        this.middle = middle;
        this.bottomCurve = bottomCurve;
        this.bottom = bottom;
        this.outline = outline;
        this.stripeWidth = stripeWidth;
        this.spotSize = spotSize;
        this.spotIndices = spotIndices;
        height = bottom.y - top.y;
    }

    public InsectBodyPart(int pixelSize, float height, float relativeWidth, float skew, float curve, Vector2 attachPixel, bool outline, int stripeWidth, int spotSize, List<int> spotIndices) {
        this.height = height;
        top = attachPixel;
        middle = top + TextureUtils.GetPixel(height * relativeWidth, skew * height, pixelSize);
        bottom = top + TextureUtils.GetPixel(0, height, pixelSize);
        topCurve = (middle.y - top.y) * height * curve;
        bottomCurve = (bottom.y - middle.y) * height * curve;
        this.outline = outline;
        this.stripeWidth = stripeWidth;
        this.spotSize = spotSize;
        this.spotIndices = spotIndices;
    }

    public override void Color(Color[] pixels, Color color) {
        TextureUtils.ColorPixelsMirrored(pixels, Points(), color);
    }

    public override void ColorHighlights(Color[] pixels, Color color) {
        // color outline highlight
        if (outline) {
            TextureUtils.ColorPixelsMirrored(pixels, TextureUtils.PointsOnCurve(top + new Vector2(0, 1f), middle - new Vector2(1f, 0), topCurve), color);
            TextureUtils.ColorPixelsMirrored(pixels, TextureUtils.PointsOnCurve(middle - new Vector2(1f, 0), bottom - new Vector2(0, 1f), bottomCurve), color);
        }
        // color stripe highlight
        if (stripeWidth > 0) {
            foreach (var point in TextureUtils.Flood(pixels, new Vector2(0f, middle.y), Points())) {
                if ((int)point.y / stripeWidth % 2 == 0) {
                    TextureUtils.ColorPixelMirrored(pixels, point, color);
                }
            }
        }
        // color spot highlights
        if (spotSize > 0) {
            int i = 0;
            List<Vector2> points = TextureUtils.Flood(pixels, new Vector2(0f, middle.y), Points());
            foreach (var point in points) {
                if (i++ % 10 == 0) {
                    TextureUtils.ColorPixelMirrored(pixels, point, color);
                    if (spotSize == 1) continue;
                    // color neighbouring pixels
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

    public List<Vector2> Points() {
        return TextureUtils.PointsOnCurve(top, middle, topCurve).Concat(TextureUtils.PointsOnCurve(middle, bottom, bottomCurve)).ToList();
    }

    public static InsectBodyPart GenerateThin(int pixelSize, float height, Vector2 attachPixel, bool outline, int stripeWidth, int spotSize) {
        return new InsectBodyPart(
            pixelSize,
            height: height,
            relativeWidth: RandomUtils.RangeAvg(0.15f, 0.3f),
            skew: RandomUtils.RangeAvg(0.3f, 0.7f),
            curve: .5f, 
            attachPixel: attachPixel,
            outline: outline,
            stripeWidth: stripeWidth,
            spotSize: spotSize,
            spotIndices: null
        );
    }

    public static InsectBodyPart GenerateFlat(int pixelSize, float height, Vector2 attachPixel, bool outline, int stripeWidth, int spotSize) {
        return new InsectBodyPart(
            pixelSize,
            height: height,
            relativeWidth: RandomUtils.RangeAvg(0.5f, 0.7f),
            skew: 1f,
            curve: 1f, 
            attachPixel: attachPixel,
            outline: outline,
            stripeWidth: stripeWidth,
            spotSize: spotSize,
            spotIndices: null
        );
    }

    public static InsectBodyPart GenerateRound(int pixelSize, float height, Vector2 attachPixel, bool outline, int stripeWidth, int spotSize) {
        return new InsectBodyPart(
            pixelSize,
            height: height,
            relativeWidth: RandomUtils.RangeAvg(0.3f, 0.6f),
            skew: RandomUtils.RangeAvg(0.3f, 0.7f),
            curve: .5f, 
            attachPixel: attachPixel,
            outline: outline,
            stripeWidth: stripeWidth,
            spotSize: spotSize,
            spotIndices: null
        );
    }

    public static InsectBodyPart GenerateManyRound(int pixelSize, float height, Vector2 attachPixel, bool outline, int stripeWidth, int spotSize) {
        return new InsectBodyPart(
            pixelSize,
            height: height,
            relativeWidth: RandomUtils.RangeAvg(0.3f, 0.6f),
            skew: 0.5f,
            curve: .5f, 
            attachPixel: attachPixel,
            outline: outline,
            stripeWidth: stripeWidth,
            spotSize: spotSize,
            spotIndices: null
        );
    }

    public override int GetTop() {
        return (int) top.y;
    }

    public override int GetBottom() {
        return (int) bottom.y;
    }

    public override int GetSide() {
        return (int) middle.x;
    }

}