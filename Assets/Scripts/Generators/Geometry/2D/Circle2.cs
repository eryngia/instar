using UnityEngine;

// A 2D circle.
public class Circle2 {

    public Vector2 c {get;}
    public float r {get;}

    public Circle2(Vector2 c, float r) {
        this.c = c;
        this.r = r;
    }

    public bool Contains(Vector2 v) {
        return (v - c).sqrMagnitude <= r * r;
    }

    public override bool Equals(object obj) {
        if (obj is Circle2 other)
            return c == other.c && r == other.r;
        else 
            return false;
    }

    public override int GetHashCode() {
        return c.GetHashCode() ^ r.GetHashCode();
    }

}