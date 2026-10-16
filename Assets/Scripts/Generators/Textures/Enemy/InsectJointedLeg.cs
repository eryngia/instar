using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// A procedural insect limb with two straight segments.
public class InsectJointedLeg : InsectLeg {

    public Vector2 joint;

    public InsectJointedLeg(Vector2 start, float curve, Vector2 end, Vector2 joint, Vector2 animation, bool highlighted) : base(start, curve, end, animation, highlighted){
        this.joint = joint;
    }

    public InsectJointedLeg(int pixelSize, Vector2 start, float width, float height, float jointPoint, Vector2 jointOffset, float animation, bool highlighted) : base(pixelSize, start, width, height, 0f, animation, highlighted){
        joint = TextureUtils.PointOnLine(start, end, jointPoint) + jointOffset;
    }

    public override void Color(Color[] pixels, Color color) {
        TextureUtils.ColorPixelsMirrored(pixels, TextureUtils.PointsOnLine(start, joint), color);
        TextureUtils.ColorPixelsMirrored(pixels, TextureUtils.PointsOnLine(joint, end), color);
    }

    public override void Color1(Color[] pixels, Color color) {
        TextureUtils.ColorPixelsRight(pixels, TextureUtils.PointsOnLine(start, joint + animation), color);
        TextureUtils.ColorPixelsLeft(pixels, TextureUtils.PointsOnLine(start, joint - animation), color);
        TextureUtils.ColorPixelsRight(pixels, TextureUtils.PointsOnLine(joint + animation, end + animation), color);
        TextureUtils.ColorPixelsLeft(pixels, TextureUtils.PointsOnLine(joint - animation, end - animation), color);  
    } 

    public override void Color2(Color[] pixels, Color color) {
        TextureUtils.ColorPixelsRight(pixels, TextureUtils.PointsOnLine(start, joint - animation), color);
        TextureUtils.ColorPixelsLeft(pixels, TextureUtils.PointsOnLine(start, joint + animation), color);
        TextureUtils.ColorPixelsRight(pixels, TextureUtils.PointsOnLine(joint - animation, end - animation), color);
        TextureUtils.ColorPixelsLeft(pixels, TextureUtils.PointsOnLine(joint + animation, end + animation), color);
    } 

    public override void ColorHighlights(Color[] pixels, Color color) {
        // color tips of legs
        if (highlighted) {
            List<Vector2> points = TextureUtils.PointsOnLine(start, end).TakeLast(3).ToList();
            TextureUtils.ColorPixelsMirrored(pixels, points, color);
        }
    }

    public override void ColorHighlights1(Color[] pixels, Color color) {
        // color tips of legs
        if (highlighted) {
            List<Vector2> pointsRight = TextureUtils.PointsOnLine(joint + animation, end + animation).TakeLast(3).ToList();
            TextureUtils.ColorPixelsRight(pixels, pointsRight, color);
            List<Vector2> pointsLeft = TextureUtils.PointsOnLine(joint - animation, end - animation).TakeLast(3).ToList();
            TextureUtils.ColorPixelsLeft(pixels, pointsLeft, color);
        }
    }

    public override void ColorHighlights2(Color[] pixels, Color color) {
        // color tips of legs
        if (highlighted) {
            List<Vector2> pointsRight = TextureUtils.PointsOnLine(joint - animation, end - animation).TakeLast(3).ToList();
            TextureUtils.ColorPixelsRight(pixels, pointsRight, color);
            List<Vector2> pointsLeft = TextureUtils.PointsOnLine(joint + animation, end + animation).TakeLast(3).ToList();
            TextureUtils.ColorPixelsLeft(pixels, pointsLeft, color);
        }
    }

    
}