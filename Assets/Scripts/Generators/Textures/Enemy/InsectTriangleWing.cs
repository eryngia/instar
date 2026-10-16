using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// A triangle moth-like wing of a procedural insect.
public class InsectTriangleWing : InsectWing {
    
    public Vector2 middle;

    public InsectTriangleWing(Vector2 start, Vector2 middle, Vector2 end, Vector2 animation, bool outline, int spotSize) : base(start, 0f, end, animation, outline, spotSize){
        this.middle = middle;
    }
    public InsectTriangleWing(int pixelSize, Vector2 start, float width, float height, float animation, bool outline, int spotSize) : base(pixelSize, start, width, height, 0f, animation, outline, spotSize){
        middle = start - TextureUtils.GetPixel(-0.02f, height, pixelSize);
        this.animation = new Vector2(-animation, animation);
    }

    public override Vector2 CenterPoint(Vector2 animation) {
        return new Vector2((start.x + middle.x + end.x + 2*animation.x) / 3f, (start.y + middle.y + end.y + 2*animation.y) / 3f);
    }

    public override List<Vector2> Points(Vector2 animation) {
        return TextureUtils.PointsOnLine(start, middle + animation / 2)
            .Concat(TextureUtils.PointsOnCurve(middle + animation / 2, end + animation, -4f))
            .Concat(TextureUtils.PointsOnLine(end + animation, start))
            .ToList();
    }

    public override List<Vector2> OutlinePoints(Vector2 animation) {
        Vector2 outlineStart = start + new Vector2(2, 3);
        Vector2 outlineMiddle = middle + new Vector2(1, -1);
        Vector2 outlineEnd = end + new Vector2(-1, 0);
        return TextureUtils.PointsOnLine(outlineStart, outlineMiddle + animation / 2)
            .Concat(TextureUtils.PointsOnCurve(outlineMiddle + animation / 2, outlineEnd + animation, -3f))
            .Concat(TextureUtils.PointsOnLine(outlineEnd + animation, outlineStart))
            .ToList();
    }


}