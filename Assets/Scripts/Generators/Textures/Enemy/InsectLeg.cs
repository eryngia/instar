using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// A procedural curved insect leg.
public class InsectLeg : InsectAppendage {

    public Vector2 animation;
    public bool highlighted;
    public InsectLeg(Vector2 start, float curve, Vector2 end, Vector2 animation, bool highligted) : base(start, curve, end){
        this.animation = animation;
        this.highlighted = highligted;
    }
    public InsectLeg(int pixelSize, Vector2 start, float width, float height, float curve, float animation, bool highligted) : base(pixelSize, start, width, height, curve){
        this.animation = new Vector2(0, animation);
        this.highlighted = highligted;
    }

    public override void Color(Color[] pixels, Color color) {
        TextureUtils.ColorPixelsMirrored(pixels, TextureUtils.PointsOnCurve(start, end, curve), color);
    }

    public override void Color1(Color[] pixels, Color color) {
        TextureUtils.ColorPixelsRight(pixels, TextureUtils.PointsOnCurve(start, end + animation, curve), color);
        TextureUtils.ColorPixelsLeft(pixels, TextureUtils.PointsOnCurve(start, end - animation, curve), color);
    } 

    public override void Color2(Color[] pixels, Color color) {
        TextureUtils.ColorPixelsRight(pixels, TextureUtils.PointsOnCurve(start, end - animation, curve), color);
        TextureUtils.ColorPixelsLeft(pixels, TextureUtils.PointsOnCurve(start, end + animation, curve), color);
    } 

    public override void ColorHighlights(Color[] pixels, Color color) {
        // color tips of legs
        if (highlighted) {
            List<Vector2> points = TextureUtils.PointsOnCurve(start, end, curve).TakeLast(3).ToList();
            TextureUtils.ColorPixelsMirrored(pixels, points, color);
        }
    }

    public override void ColorHighlights1(Color[] pixels, Color color) {
        // color tips of legs
        if (highlighted) {
            List<Vector2> pointsRight = TextureUtils.PointsOnCurve(start, end + animation, curve).TakeLast(3).ToList();
            TextureUtils.ColorPixelsRight(pixels, pointsRight, color);
            List<Vector2> pointsLeft = TextureUtils.PointsOnCurve(start, end - animation, curve).TakeLast(3).ToList();
            TextureUtils.ColorPixelsLeft(pixels, pointsLeft, color);
        }
    }

    public override void ColorHighlights2(Color[] pixels, Color color) {
        // color tips of legs
        if (highlighted) {
            List<Vector2> pointsRight = TextureUtils.PointsOnCurve(start, end - animation, curve).TakeLast(3).ToList();
            TextureUtils.ColorPixelsRight(pixels, pointsRight, color);
            List<Vector2> pointsLeft = TextureUtils.PointsOnCurve(start, end + animation, curve).TakeLast(3).ToList();
            TextureUtils.ColorPixelsLeft(pixels, pointsLeft, color);
        }
    }

    public static InsectLeg GenerateCurved(int pixelSize, Vector2 start, float width, float height, float curve, float animation, bool highlighted) {
        return new InsectLeg(pixelSize, start, width, height, curve, animation, highlighted);
    }

    public static InsectJointedLeg GenerateJointed(int pixelSize, Vector2 start, float width, float height, float jointPoint, Vector2 jointOffset, float animation, bool highlighted) {
        return new InsectJointedLeg(pixelSize, start, width, height, jointPoint, jointOffset, animation, highlighted);
    }

}