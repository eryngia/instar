using UnityEngine;

// A procedural insect appendage.
public abstract class InsectAppendage : InsectPart {
    
    
    public Vector2 start; // attachment point on body
    public float curve; // curve bend
    public Vector2 end; // end point
    
    public InsectAppendage(Vector2 start, float curve, Vector2 end) {
        this.start = start;
        this.curve = curve;
        this.end = end;
    }

    public InsectAppendage(int pixelSize, Vector2 start, float width, float height, float curve) {
        this.start = start;
        this.curve = curve;
        end = start + TextureUtils.GetPixel(width, -height, pixelSize);
    }

    public override int GetTop() {
        return (int) Mathf.Min(start.y, end.y);
    }

    public override int GetBottom() {
        return (int) Mathf.Max(start.y, end.y);
    }

    public override int GetSide() {
        return (int) Mathf.Max(start.x, end.x);
    }
}