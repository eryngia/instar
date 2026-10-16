using UnityEngine;

// A head appendage of an insect (antenna, horn, ...).
public class InsectHeadAppendage : InsectAppendage {

    public bool highlighted;
    public InsectHeadAppendage(Vector2 start, float curve, Vector2 end, bool highlighted) : base(start, curve, end){
        this.highlighted = highlighted;
    }
    public InsectHeadAppendage(int pixelSize, Vector2 start, float width, float height, float curve, bool highlighted) : base(pixelSize, start, width, height, curve){
        this.highlighted = highlighted;
    }

    public override void Color(Color[] pixels, Color color) {
        TextureUtils.ColorPixelsMirrored(pixels, TextureUtils.PointsOnCurve(start, end, curve), color);
    }

    public override void ColorHighlights(Color[] pixels, Color color) {
        if (highlighted)
            TextureUtils.ColorPixelsMirrored(pixels, TextureUtils.PointsOnCurve(start, end, curve), color);
    }


    public static InsectHeadAppendage GenerateHorn(int pixelSize, Vector2 start, bool highlighted) {
        return new InsectHeadAppendage(pixelSize, start, RandomUtils.RangeAvg(0f, 0.05f), 0.1f + RandomUtils.RangeAvg(0f, 0.1f), RandomUtils.RangeAvg(-4f, 0f), highlighted);
    }

    public static InsectHeadAppendage GenerateAntenna(int pixelSize, Vector2 start, bool highlighted) {
        float width = (Random.value > 0.3 ? 0.1f : 0.3f) + RandomUtils.RangeAvg(0f, 0.1f);
        return new InsectHeadAppendage(pixelSize, start, width, 0.05f + RandomUtils.RangeAvg(0f, 0.1f), RandomUtils.RangeAvg(0f, 4f), highlighted);
    }


}